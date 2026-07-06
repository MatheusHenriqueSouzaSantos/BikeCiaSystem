using ApiEstagioBicicletaria.Dtos.FornecedorDtos;
using ApiEstagioBicicletaria.Dtos.VendedorDtos;
using ApiEstagioBicicletaria.Entities.FornedorDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendedorDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Seguranca;
using ApiEstagioBicicletaria.Services;
using ApiEstagioBicicletaria.Services.LogServices.InterfacesLog;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoApiDeVendaEstagio.Tests.Services
{
    public class FornecedorServiceTest
    {

        private readonly ContextoDb _contexto;

        private readonly Mock<IFornecedorLogService> _fornecedorLogServiceMock = new();

        private readonly Mock<IUsuarioLogadoService> _usuarioLogadoServiceMock = new();
        private readonly FornecedorService _fornecedorService;

        public FornecedorServiceTest()
        {
            var optionsBd = new DbContextOptionsBuilder<ContextoDb>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _contexto = new ContextoDb(optionsBd);

            Usuario usuarioLogado = new("abcd", "usuarioLogado", "usuariologado@gmail.com", "usarioLogado", PerfilUsuario.Admin);

            _usuarioLogadoServiceMock.Setup(u => u.ObterUsuario()).Returns(usuarioLogado);

            _fornecedorService = new FornecedorService(_contexto, _fornecedorLogServiceMock.Object, _usuarioLogadoServiceMock.Object);
        }

        [Fact]
        public void BuscarTodosFornecedoresAtivosComSucesso()
        {
            _contexto.Fornecedores.Add(new Fornecedor("3635", "fornecedor@gmail.com",
                "bike ltda", "bike produções", "21.048.789/0001-20", "123456789"));
            _contexto.Fornecedores.Add(new Fornecedor("3635", "fornecedorq@gmail.com",
                "bike ltdaq", "bike produçõesq", "95.632.803/0001-75", "12345678910"));
            _contexto.SaveChanges();
            
            List<Fornecedor> fornecedoresAtivos = _fornecedorService.BuscarTodosAtivos();

            Assert.True(fornecedoresAtivos.Count > 0);
            Assert.Equal(2, fornecedoresAtivos.Count);
        }

        [Fact]
        public void BuscarTodosFornecedoresInativoComSucesso()
        {
            _contexto.Fornecedores.Add(new Fornecedor("3635", "fornecedor@gmail.com",
                "bike ltda", "bike produções", "21.048.789/0001-20", "123456789"));

            _contexto.SaveChanges();

            Fornecedor fornecedorUm = _contexto.Fornecedores.FirstOrDefault(f=>f.Email== "fornecedor@gmail.com");
            fornecedorUm.Ativo = false;
            _contexto.Fornecedores.Update(fornecedorUm);
            _contexto.SaveChanges();

            _contexto.Fornecedores.Add(new Fornecedor("3635", "fornecedor1@gmail.com",
                "bike ltdaq", "bike produçõesq", "95.632.803/0001-75", "12345678910"));
            _contexto.SaveChanges();

            Fornecedor fornecedorDois = _contexto.Fornecedores.FirstOrDefault(f => f.Email == "fornecedor1@gmail.com");
            fornecedorDois.Ativo = false;
            _contexto.Fornecedores.Update(fornecedorDois);
            _contexto.SaveChanges();

            List<Fornecedor> fornecedoresInativos = _fornecedorService.BuscarTodosInativos();

            Assert.True(fornecedoresInativos.Count > 0);
            Assert.Equal(2, fornecedoresInativos.Count);
            foreach(Fornecedor fornecedor in fornecedoresInativos)
            {
                Assert.False(fornecedor.Ativo);
            }
        }

        [Fact]
        public void BuscarFornecedorAtivoPorIdComSucesso()
        {
            _contexto.Fornecedores.Add(new Fornecedor("3635","fornecedor@gmail.com",
                "bike ltda","bike produções", "21048789000120","123456789"));
            _contexto.SaveChanges();
            Guid idfornecedorCriado = _contexto.Fornecedores.FirstOrDefault().Id;

            Fornecedor fornecedorBuscadoo = _fornecedorService.BuscarAtivoPorId(idfornecedorCriado);

            Assert.NotNull(fornecedorBuscadoo);
            Assert.Equal(idfornecedorCriado, fornecedorBuscadoo.Id);
            Assert.Equal("3635", fornecedorBuscadoo.Telefone);
            Assert.Equal("fornecedor@gmail.com", fornecedorBuscadoo.Email);
            Assert.Equal("bike ltda", fornecedorBuscadoo.RazaoSocial);
            Assert.Equal("bike produções", fornecedorBuscadoo.NomeFantasia);
            Assert.Equal("123456789", fornecedorBuscadoo.InscricaoEstadual);
            Assert.Equal("21048789000120", fornecedorBuscadoo.Cnpj);
        }

        [Fact]
        public void BuscarFornecedorAtivoPorIdInexistenteComFalha()
        {
            _contexto.Fornecedores.Add(new Fornecedor("3635", "fornecedor@gmail.com",
                "bike ltda", "bike produções", "21.048.789/0001-20", "123456789"));
            _contexto.SaveChanges();

            Assert.Throws<ExcecaoDeRegraDeNegocio>(() => _fornecedorService.BuscarAtivoPorId(Guid.NewGuid()));
        }


        [Fact]
        public void CadastrarFornecedorEnviandoCnpjInvalidoFalha()
        {
            FornecedorCreateDto dtoDeCriacao = new("3635", "fornecedor@gmail.com",
                "bike ltda", "bike produções", "1111111111111", "123456789");
            Assert.Throws<ExcecaoDeRegraDeNegocio>(() => _fornecedorService.Cadastrar(dtoDeCriacao));
        }

        [Fact]
        public void CadastrarFornecedorComSucesso()
        {
            FornecedorCreateDto dtoDeCriacao = new("3635", "fornecedor@gmail.com",
                 "bike ltda", "bike produções", "21.048.789/0001-20", "123456789");
            Guid idUsuarioCrido=_fornecedorService.Cadastrar(dtoDeCriacao).Id;

            Fornecedor fornecedorCriado = _contexto.Fornecedores.FirstOrDefault(v => v.Id == idUsuarioCrido);

            Assert.NotNull(fornecedorCriado);
            Assert.Equal("3635", fornecedorCriado.Telefone);
            Assert.Equal("fornecedor@gmail.com", fornecedorCriado.Email);
            Assert.Equal("bike ltda", fornecedorCriado.RazaoSocial);
            Assert.Equal("bike produções", fornecedorCriado.NomeFantasia);
            Assert.Equal("123456789", fornecedorCriado.InscricaoEstadual);
            Assert.Equal("21048789000120", fornecedorCriado.Cnpj);
        }

        [Fact]
        public void AtualizarFornecedorComSucesso()
        {
            Fornecedor fornecedor = new("3635", "fornecedor@gmail.com",
                 "bike ltda", "bike produções", "21.048.789/0001-20", "123456789");
            _contexto.Fornecedores.Add(fornecedor);
            _contexto.SaveChanges();
            _fornecedorService.Atualizar(fornecedor.Id, new FornecedorUpdateDto("321", "fornecedoratualizado@gmail.com",
                 "bike ltda atualizada", "bike produções atualizada", "12345678"));
            Fornecedor fornecedorVindoDoBanco = _contexto.Fornecedores.FirstOrDefault(v => v.Id == fornecedor.Id);
            Assert.NotNull(fornecedorVindoDoBanco);
            Assert.Equal("321", fornecedor.Telefone);
            Assert.Equal("fornecedoratualizado@gmail.com", fornecedorVindoDoBanco.Email);
            Assert.Equal("bike ltda atualizada", fornecedorVindoDoBanco.RazaoSocial);
            Assert.Equal("bike produções atualizada", fornecedorVindoDoBanco.NomeFantasia);
            Assert.Equal("12345678", fornecedorVindoDoBanco.InscricaoEstadual);
        }

        [Fact]
        public void InativarFornecedorComSucesso()
        {
            Fornecedor fornecedor = new("3635", "fornecedor@gmail.com",
                 "bike ltda", "bike produções", "21.048.789/0001-20", "123456789");
            _contexto.Fornecedores.Add(fornecedor);
            _contexto.SaveChanges();
            _fornecedorService.Inativar(fornecedor.Id);
            Fornecedor fornecedorVindoDoBanco = _contexto.Fornecedores.FirstOrDefault(v => v.Id == fornecedor.Id);
            Assert.NotNull(fornecedorVindoDoBanco);
            Assert.False(fornecedorVindoDoBanco.Ativo);
        }

        [Fact]
        public void ReativarFornecedorComSucesso()
        {
            Fornecedor fornecedor = new("3635", "fornecedor@gmail.com",
                 "bike ltda", "bike produções", "21.048.789/0001-20", "123456789");
            fornecedor.Ativo = false;
            _contexto.Fornecedores.Add(fornecedor);
            _contexto.SaveChanges();
            _fornecedorService.Reativar(fornecedor.Id);
            Fornecedor fornecedorVindoDoBanco = _contexto.Fornecedores.FirstOrDefault(v => v.Id == fornecedor.Id);
            Assert.NotNull(fornecedorVindoDoBanco);
            Assert.True(fornecedorVindoDoBanco.Ativo);
        }
    }
}
