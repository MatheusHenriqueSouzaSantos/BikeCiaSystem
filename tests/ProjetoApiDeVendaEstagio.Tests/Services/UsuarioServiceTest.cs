using ApiEstagioBicicletaria.Dtos.Usuario;
using ApiEstagioBicicletaria.Dtos.UsuarioDtos;
using ApiEstagioBicicletaria.Dtos.VendedorDtos;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendedorDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Seguranca;
using ApiEstagioBicicletaria.Services;
using ApiEstagioBicicletaria.Services.Interfaces;
using ApiEstagioBicicletaria.Services.LogServices.InterfacesLog;
using ApiEstagioBicicletaria.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoApiDeVendaEstagio.Tests.Services
{
    public class UsuarioServiceTest
    {
        private readonly ContextoDb _contexto;
        private readonly Mock<IUsuarioLogadoService> _usuarioLogadoServiceMock = new();
        private readonly Mock<IServicoJwt> _servicoJwtMock = new();
        private readonly Mock<IUsuarioLogService> _usuarioLogServiceMock = new();
        private readonly Mock<IGeradorCodigoIdentificador> _geradorCodigoIdentificadorMock = new();
        private readonly SenhaService _senhaService = new();
        private readonly UsuarioService _service;

        public UsuarioServiceTest()
        {
            var optionsBd = new DbContextOptionsBuilder<ContextoDb>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _contexto = new ContextoDb(optionsBd);

            Usuario usuarioLogado = new("abcd", "usuarioLogado", "usuariologado@gmail.com", _senhaService.GerarHashDaSenha("usuariologado"), PerfilUsuario.Admin);

            _usuarioLogadoServiceMock.Setup(u => u.ObterUsuario()).Returns(usuarioLogado);

            _servicoJwtMock.Setup(s => s.GerarJWT(It.IsAny<Usuario>())).Returns("token");

            _geradorCodigoIdentificadorMock.Setup(g => g.GerarCodigoUsuario()).Returns("abch");

            _service = new(_servicoJwtMock.Object, _senhaService, _contexto,_usuarioLogServiceMock.Object,_usuarioLogadoServiceMock.Object,
                _geradorCodigoIdentificadorMock.Object);
        }

        [Fact]
        public void BuscarTodosUsuarioAtivosComSucesso()
        {
            _contexto.Usuarios.Add(new Usuario("abce","usuarioUm","usuarioum@gmail.com",_senhaService.GerarHashDaSenha("testeUm"),PerfilUsuario.Admin));
            _contexto.Usuarios.Add(new Usuario("abcf","usuarioDois","usuariodois@gmail.com",_senhaService.GerarHashDaSenha("testedois"),PerfilUsuario.User));
            _contexto.SaveChanges();

            List<UsuarioOutputDto> usuariosAtivos = _service.BuscarTodosAtivos();

            Assert.True(usuariosAtivos.Count > 0);
            Assert.Equal(2, usuariosAtivos.Count);
        }

        [Fact]
        public void BuscarTodosUsuarioInativoComSucesso()
        {
            _contexto.Usuarios.Add(new Usuario("abce", "usuarioUm", "usuarioum@gmail.com", _senhaService.GerarHashDaSenha("testeUm"), PerfilUsuario.Admin));

            _contexto.SaveChanges();

            Usuario usuarioUm = _contexto.Usuarios.FirstOrDefault(u => u.Email == "usuarioum@gmail.com");
            usuarioUm.Ativo = false;
            _contexto.Usuarios.Update(usuarioUm);
            _contexto.SaveChanges();

            _contexto.Usuarios.Add(new Usuario("abcf", "usuarioDois", "usuariodois@gmail.com", _senhaService.GerarHashDaSenha("testedois"), PerfilUsuario.User));

            _contexto.SaveChanges();

            Usuario usuarioDois = _contexto.Usuarios.FirstOrDefault(u => u.Email == "usuariodois@gmail.com");
            usuarioDois.Ativo = false;
            _contexto.Usuarios.Update(usuarioDois);
            _contexto.SaveChanges();
            _contexto.SaveChanges();

            List<UsuarioOutputDto> usuarios = _service.BuscarTodosInativos();

            Assert.True(usuarios.Count > 0);
            Assert.Equal(2, usuarios.Count);
            foreach (UsuarioOutputDto usuario in usuarios)
            {
                Assert.False(usuario.Ativo);
            }
        }

        [Fact]
        public void BuscarUsuarioAtivoPorIdComSucesso()
        {
            _contexto.Usuarios.Add(new Usuario("abce", "usuarioUm", "usuarioum@gmail.com", _senhaService.GerarHashDaSenha("testeUm"), PerfilUsuario.Admin));
            _contexto.SaveChanges();
            Guid idUsuarioCriado = _contexto.Usuarios.FirstOrDefault().Id;

            UsuarioOutputDto usuarioDto = _service.BuscarPorIdAtivo(idUsuarioCriado);

            Assert.NotNull(usuarioDto);
            Assert.Equal(idUsuarioCriado, usuarioDto.Id);
            Assert.Equal("abce", usuarioDto.CodigoUsuario);
            Assert.Equal("usuarioUm", usuarioDto.Nome);
            Assert.Equal(PerfilUsuario.Admin,usuarioDto.PerfilUsuario);
        }

        [Fact]
        public void BuscarUsuarioAtivoPorIdInexistenteComFalha()
        {
            _contexto.Usuarios.Add(new Usuario("abce", "usuarioUm", "usuarioum@gmail.com", _senhaService.GerarHashDaSenha("testeUm"), PerfilUsuario.Admin));
            _contexto.SaveChanges();
            Guid idUsuarioCriado = _contexto.Usuarios.FirstOrDefault().Id;

            UsuarioOutputDto usuarioDto = _service.BuscarPorIdAtivo(idUsuarioCriado);

            Assert.Throws<ExcecaoDeRegraDeNegocio>(() => _service.BuscarPorIdAtivo(Guid.NewGuid()));
        }

        [Fact]
        public void BuscarUsuarioLogadoComSucesso()
        {
            UsuarioOutputDto usuarioDto = _service.BuscarUsuarioLogado();
            Assert.NotNull(usuarioDto);
            Assert.Equal("abcd", usuarioDto.CodigoUsuario);
            Assert.Equal("usuarioLogado", usuarioDto.Nome);
            Assert.Equal("usuariologado@gmail.com", usuarioDto.Email);
            Assert.Equal(PerfilUsuario.Admin, usuarioDto.PerfilUsuario);
        }


        [Fact]
        public void CadastrarUsuarioComSucesso()
        {
            UsuarioCreateDto dtoDeCriacao = new("usuarioUm", "usuarioum@gmail.com", "testeUm", PerfilUsuario.Admin);

            Guid IdUsuarioCriado = _service.Cadastrar(dtoDeCriacao).Id;
            Usuario? usuarioCriado = _contexto.Usuarios.FirstOrDefault(v => v.Id == IdUsuarioCriado);
            Assert.NotNull(usuarioCriado);
            Assert.Equal(IdUsuarioCriado, usuarioCriado.Id);
            Assert.Equal("usuarioUm", usuarioCriado.Nome);
            Assert.Equal("usuarioum@gmail.com", usuarioCriado.Email);
            Assert.Equal(PerfilUsuario.Admin, usuarioCriado.PerfilUsuario);
        }

        [Fact]
        public void AtualizarUsuarioComSucesso()
        {
            _contexto.Usuarios.Add(new Usuario("abce", "usuarioUm", "usuarioum@gmail.com", _senhaService.GerarHashDaSenha("testeUm"), PerfilUsuario.User));
            _contexto.SaveChanges();
            Guid idUsuarioCriado = _contexto.Usuarios.FirstOrDefault().Id;
            UsuarioUpdateDto dtoDeAtualizacao = new("usuarioAtualizado", "usuarioatualizado@gmail.com", "testeatualizado", PerfilUsuario.Admin);
            _service.Atualizar(idUsuarioCriado, dtoDeAtualizacao);
            Usuario? usuarioVindoDoBanco = _contexto.Usuarios.FirstOrDefault(v => v.Id == idUsuarioCriado);
            Assert.NotNull(usuarioVindoDoBanco);
            Assert.Equal(idUsuarioCriado, usuarioVindoDoBanco.Id);
            Assert.Equal("usuarioAtualizado", usuarioVindoDoBanco.Nome);
            Assert.Equal("usuarioatualizado@gmail.com", usuarioVindoDoBanco.Email);
            Assert.Equal(PerfilUsuario.Admin, usuarioVindoDoBanco.PerfilUsuario);
        }
        [Fact]
        public void AtualizarUsuarioLogadoComSucesso()
        {
            Usuario usuarioLogado = new("abcd", "usuarioLogado", "usuariologado@gmail.com",
            _senhaService.GerarHashDaSenha("usuariologado"), PerfilUsuario.Admin);
            _contexto.Usuarios.Add(usuarioLogado);
            _contexto.SaveChanges();

            _usuarioLogadoServiceMock.Setup(u => u.ObterUsuario()).Returns(usuarioLogado);
            AlteracaoDeUsuarioLogadoDto dtoDeAtualizacao = new("usuarioAlterado", "usuarioalterado@gmail.com", "usuariologado", null);
            _service.AtualizarUsuarioLogado(dtoDeAtualizacao);
            Usuario? usuarioVindoDoBanco = _contexto.Usuarios.FirstOrDefault(v => v.Email == "usuarioalterado@gmail.com");
            Assert.NotNull(usuarioVindoDoBanco);
            Assert.Equal("usuarioAlterado", usuarioVindoDoBanco.Nome);
            Assert.Equal("usuarioalterado@gmail.com", usuarioVindoDoBanco.Email);
        }

        [Fact]
        public void InativarUsuarioComSucesso()
        {
            Usuario usuario = new Usuario("abce", "usuarioUm", "usuarioum@gmail.com", _senhaService.GerarHashDaSenha("testeUm"), PerfilUsuario.Admin);
            Usuario usuarioDois = new Usuario("abcg", "usuarioDois", "usuariodois@gmail.com", _senhaService.GerarHashDaSenha("testedois"), PerfilUsuario.Admin);
            _contexto.Usuarios.Add(usuario);
            _contexto.Usuarios.Add(usuarioDois);    
            _contexto.SaveChanges();
            _service.Inativar(usuario.Id);
            Usuario usuarioVindoDoBanco = _contexto.Usuarios.FirstOrDefault(v => v.Id == usuario.Id);
            Assert.NotNull(usuarioVindoDoBanco);
            Assert.False(usuarioVindoDoBanco.Ativo);
        }

        [Fact]
        public void ReativarUsuarioComSucesso()
        {
            Usuario usuario = new Usuario("abce", "usuarioUm", "usuarioum@gmail.com", "testeUm", PerfilUsuario.Admin);
            usuario.Ativo = false;
            _contexto.Usuarios.Add(usuario);
            _contexto.SaveChanges();
            _service.Reativar(usuario.Id);
            Usuario usuarioVindoDoBanco = _contexto.Usuarios.FirstOrDefault(v => v.Id == usuario.Id);
            Assert.NotNull(usuarioVindoDoBanco);
            Assert.True(usuarioVindoDoBanco.Ativo);
        }

        [Fact]
        public void LoginUsuarioComSucesso()
        {
            Usuario usuario = new Usuario("abcd", "usuarioUm", "usuarioum@gmail.com", _senhaService.GerarHashDaSenha("testeUm"), PerfilUsuario.Admin);
            _contexto.Usuarios.Add(usuario);
            _contexto.SaveChanges();
            string jwtMock= _service.Login(new UsuarioLoginDto(usuario.Email, "testeUm"));
            Assert.NotNull(jwtMock);
            Assert.Equal("token", jwtMock); 
        }
    }
}
