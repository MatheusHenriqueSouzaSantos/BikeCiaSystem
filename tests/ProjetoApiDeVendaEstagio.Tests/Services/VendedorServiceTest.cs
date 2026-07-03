using ApiEstagioBicicletaria.Dtos.VendedorDtos;
using ApiEstagioBicicletaria.Entities.FornedorDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendedorDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Seguranca;
using ApiEstagioBicicletaria.Services;
using ApiEstagioBicicletaria.Services.Interfaces;
using ApiEstagioBicicletaria.Services.LogServices;
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
    public class VendedorServiceTest
    {
        private readonly ContextoDb _contexto;

        private readonly Mock<IVendedorLogService> _vendedorLogServiceMock=new();
        
        private readonly Mock<IUsuarioLogadoService> _usuarioLogadoServiceMock=new();
        private readonly VendedorService _vendedorService;

        public VendedorServiceTest()
        {
            var optionsBd=new DbContextOptionsBuilder<ContextoDb>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _contexto=new ContextoDb(optionsBd);

            Usuario usuarioLogado = new("1234", "usuarioLgado", "usuariologado@gmail.com", "usarioLogado", PerfilUsuario.Admin);

            _usuarioLogadoServiceMock.Setup(u=>u.ObterUsuario()).Returns(usuarioLogado);

            _vendedorService = new VendedorService(_contexto, _vendedorLogServiceMock.Object, _usuarioLogadoServiceMock.Object);
        }

        [Fact]
        public void BuscarTodosVendedoresAtivosComSucesso()
        {
            _contexto.Vendedores.Add(new Vendedor("123", "vendedor@gmailcom", "vendedor", "05681152014"));
            _contexto.Vendedores.Add(new Vendedor("1234", "vendedor1@gmailcom", "vendedor1", "160.178.460-00"));
            _contexto.SaveChanges();

            List<Vendedor> vendedoresAtivos = _vendedorService.BuscarTodosOsVendedoresAtivos();

            Assert.True(vendedoresAtivos.Count > 0);
            Assert.Equal(2, vendedoresAtivos.Count);
        }

        [Fact]
        public void BuscarTodosVendedoresInativoComSucesso()
        {
            _contexto.Vendedores.Add(new Vendedor("123", "vendedor@gmailcom", "vendedor", "05681152014"));
      
            _contexto.SaveChanges();

            Vendedor vendedorUm = _contexto.Vendedores.FirstOrDefault(f => f.Email == "vendedor@gmailcom");
            vendedorUm.Ativo = false;
            _contexto.Vendedores.Update(vendedorUm);
            _contexto.SaveChanges();

            _contexto.Vendedores.Add(new Vendedor("1234", "vendedor1@gmailcom", "vendedor1", "160.178.460-00"));
            _contexto.SaveChanges();

            Vendedor vendedorDois = _contexto.Vendedores.FirstOrDefault(f => f.Email == "vendedor1@gmailcom");
            vendedorDois.Ativo = false;
            _contexto.Vendedores.Update(vendedorDois);
            _contexto.SaveChanges();

            List<Vendedor> vendedoresInativos = _vendedorService.BuscarTodosOsVendedoresInativos();

            Assert.True(vendedoresInativos.Count > 0);
            Assert.Equal(2, vendedoresInativos.Count);
            foreach (Vendedor vendedor in vendedoresInativos)
            {
                Assert.False(vendedor.Ativo);
            }
        }

        [Fact]
        public void BuscarVendedorAtivoPorIdComSucesso()
        {
            _contexto.Vendedores.Add(new Vendedor("123", "vendedor@gmailcom", "vendedor", "05681152014"));
            _contexto.SaveChanges();
            Guid idVendedorCriado= _contexto.Vendedores.FirstOrDefault().Id;

            Vendedor vendedorBuscado = _vendedorService.BuscarVendedorAtivoPorId(idVendedorCriado);

            Assert.NotNull(vendedorBuscado);
            Assert.Equal(idVendedorCriado, vendedorBuscado.Id);
            Assert.Equal("123", vendedorBuscado.Telefone);
            Assert.Equal("vendedor@gmailcom", vendedorBuscado.Email);   
            Assert.Equal("vendedor", vendedorBuscado.NomeCompleto);
            Assert.Equal("05681152014", vendedorBuscado.Cpf);
        }

        [Fact]
        public void BuscarVendedorAtivoPorIdInexistenteComFalha()
        {
            _contexto.Vendedores.Add(new Vendedor("123", "vendedor@gmailcom", "vendedor", "05681152014"));
            _contexto.SaveChanges();
            Guid idVendedorCriado = _contexto.Vendedores.FirstOrDefault().Id;

            Assert.Throws<ExcecaoDeRegraDeNegocio>(() => _vendedorService.BuscarVendedorAtivoPorId(Guid.NewGuid()));
        }


        [Fact] 
        public void CadastrarVendedorEnviandoCpfInvalidoFalha()
        {
            VendedorCreateDto dtoDeCriacao = new VendedorCreateDto("3635", "Vendedor@gmail.com", "vendedor", "111111111");
            Assert.Throws<ExcecaoDeRegraDeNegocio>(() => _vendedorService.CadastrarVendedor(dtoDeCriacao));
        }

        [Fact]
        public void CadastrarVendedorComSucesso()
        {
            VendedorCreateDto dtoDeCriacao = new VendedorCreateDto("3635", "vendedor@gmail.com", "vendedor", "333.573.930-26");
            
            Guid idVendedorCadastrado=_vendedorService.CadastrarVendedor(dtoDeCriacao).Id;
            Vendedor? vendedorCriado= _contexto.Vendedores.FirstOrDefault(v => v.Id == idVendedorCadastrado);
            Assert.NotNull(vendedorCriado);
            Assert.Equal("3635", vendedorCriado.Telefone);
            Assert.Equal("vendedor@gmail.com", vendedorCriado.Email);
            Assert.Equal("vendedor", vendedorCriado.NomeCompleto);
            Assert.Equal("33357393026", vendedorCriado.Cpf);
        }

        [Fact]
        public void AtualizarVendedorComSucesso()
        {
            Vendedor vendedor = new Vendedor("123","vendedor@gmail.com","vendedor", "22309298018");
            _contexto.Vendedores.Add(vendedor);
            _contexto.SaveChanges();
            Guid idVendedorVindoDoBanco=_contexto.Vendedores.FirstOrDefault().Id;
            Guid idRetornado= _vendedorService.AtualizarVendedor(idVendedorVindoDoBanco, new VendedorUpdatedDto("321",
                "vendedoratualizado@gmail.com", "vendedorAtualizado")).Id;
            Vendedor? vendedorVindoDoBanco= _contexto.Vendedores.FirstOrDefault(v => v.Id == idRetornado);
            Assert.NotNull(vendedorVindoDoBanco);
            Assert.Equal("321", vendedor.Telefone);
            Assert.Equal("vendedoratualizado@gmail.com", vendedorVindoDoBanco.Email);
            Assert.Equal("vendedorAtualizado", vendedorVindoDoBanco.NomeCompleto);
        }

        [Fact]
        public void InativarVendedorComSucesso()
        {
            Vendedor vendedor = new Vendedor("123", "vendedor@gmail.com", "vendedor", "22309298018");
            _contexto.Vendedores.Add(vendedor);
            _contexto.SaveChanges();
            Guid idVendedorVindoDoBanco = _contexto.Vendedores.FirstOrDefault().Id;
            _vendedorService.InativarVendedor(idVendedorVindoDoBanco);
            Vendedor vendedorVindoDoBanco = _contexto.Vendedores.FirstOrDefault(v => v.Id == idVendedorVindoDoBanco);
            Assert.NotNull(vendedorVindoDoBanco);
            Assert.False(vendedorVindoDoBanco.Ativo);
        }

        [Fact]
        public void AtivarVendedorComSucesso()
        {
            Vendedor vendedor = new Vendedor("123", "vendedor@gmail.com", "vendedor", "22309298018");
            vendedor.Ativo = false;
            _contexto.Vendedores.Add(vendedor);
            _contexto.SaveChanges();
            Guid idVendedorVindoDoBanco = _contexto.Vendedores.FirstOrDefault().Id;
            _vendedorService.ReativarVendedor(idVendedorVindoDoBanco);
            Vendedor vendedorVindoDoBanco = _contexto.Vendedores.FirstOrDefault(v => v.Id == idVendedorVindoDoBanco);
            Assert.NotNull(vendedorVindoDoBanco);
            Assert.True(vendedorVindoDoBanco.Ativo);
        }
    }
}
