using ApiEstagioBicicletaria.Dtos.VendedorDtos;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendedorDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Repository.Repositorios;
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
    public class EntradaEstoqueServiceTest
    {
        private readonly ContextoDb _contexto;

        private readonly Mock<IUsuarioLogadoService> _usuarioLogadoServiceMock = new();
        private readonly EntradaEstoqueService _entradaEstoqueService;

        public EntradaEstoqueServiceTest()
        {
            var optionsBd = new DbContextOptionsBuilder<ContextoDb>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _contexto = new ContextoDb(optionsBd);

            Usuario usuarioLogado = new("1234", "usuarioLgado", "usuariologado@gmail.com", "usarioLogado", PerfilUsuario.Admin);

            _usuarioLogadoServiceMock.Setup(u => u.ObterUsuario()).Returns(usuarioLogado);

            //_entradaEstoqueService = new EntradaEstoqueService(_contexto,);
        }

        [Fact]
        public void BuscarTodasEntradasAtivasComSucesso()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void BuscarTodasEntradasInativasComSucesso()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void BuscarEntradaAtivaPorIdComSucesso()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void BuscarEntradaAtivaPorIdInexistenteComFalha()
        {
            throw new NotImplementedException();
        }


        [Fact]
        public void CadastrarEntradaEnviandoIdProdutoInexistenteFalha()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CadastrarEntradaComSucesso()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void AtualizarEntradaComSucesso()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void InativarEntradaComSucesso()
        {
            throw new NotImplementedException();
        }

    }
}
