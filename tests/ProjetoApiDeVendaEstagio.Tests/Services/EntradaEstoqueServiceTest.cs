using ApiEstagioBicicletaria;
using ApiEstagioBicicletaria.Dtos.EntradaEstoqueDtos.Input;
using ApiEstagioBicicletaria.Dtos.VendedorDtos;
using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Entities.EstoqueDomain;
using ApiEstagioBicicletaria.Entities.FornedorDomain;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Entities.VendedorDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;
using ApiEstagioBicicletaria.Repository.Repositorios;
using ApiEstagioBicicletaria.Seguranca;
using ApiEstagioBicicletaria.Services;
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
    public class EntradaEstoqueServiceTest
    {
        private readonly ContextoDb _contexto;

        private readonly Mock<IGeradorCodigoIdentificador> _geradorCodigoAleatorioMock = new();
        private readonly Mock<IEntradaEstoqueLogService> _entradaEstoqueLogServiceMock = new();
        private readonly Mock<IItemEntradaEstoqueLogService> _itemEntradaEstoqueLogServiceMock = new();
        private readonly Mock<IEstoqueLogService> _estoqueLogServiceMock = new();
        private readonly Mock<IUsuarioLogadoService> _usuarioLogadoServiceMock = new();
        private readonly EntradaEstoqueService _service;

        public EntradaEstoqueServiceTest()
        {
            var optionsBd = new DbContextOptionsBuilder<ContextoDb>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _contexto = new ContextoDb(optionsBd);

            Usuario usuarioLogado = new("abcd", "usuarioLogado", "usuariologado@gmail.com", "usarioLogado", PerfilUsuario.Admin);

            _usuarioLogadoServiceMock.Setup(u => u.ObterUsuario()).Returns(usuarioLogado);

            _geradorCodigoAleatorioMock.Setup(u => u.GerarCodigoEntradaEstoque()).Returns("abcdef");

            _service = new EntradaEstoqueService(_contexto, _geradorCodigoAleatorioMock.Object,_entradaEstoqueLogServiceMock.Object,
                _itemEntradaEstoqueLogServiceMock.Object,_estoqueLogServiceMock.Object,_usuarioLogadoServiceMock.Object);
        }

        [Fact]
        public void BuscarTodasEntradasAtivasComSucesso()
        {
            Produto produtoUm = new("123456789", "Produto Teste", "bom produto", 100);
            _contexto.Produtos.Add(produtoUm);
            Estoque estoqueDoProdutoUm = new Estoque(produtoUm);
            _contexto.Estoques.Add(estoqueDoProdutoUm);
            Fornecedor fornecedorUm = new("998767574", "fornecedor@gmail.com", "fornecedor teste ltda", "fornecedor teste", "90685427000163", "123456789");
            _contexto.Fornecedores.Add(fornecedorUm);
            EntradaEstoque entradaEstoqueUm = new(fornecedorUm, "abcdef", StatusEntradaEstoque.Criada);
            _contexto.EntradasEstoque.Add(entradaEstoqueUm);
            ItemEntradaEstoque itemEntradaEstoqueUm = new(entradaEstoqueUm, produtoUm, 10);
            _contexto.ItensEntradaEstoque.Add(itemEntradaEstoqueUm);

            Produto produtoDois = new("123456787", "Produto Teste dois", "bom produto dois", 200);
            _contexto.Produtos.Add(produtoDois);
            Estoque estoqueDoProdutoDois = new Estoque(produtoDois);
            _contexto.Estoques.Add(estoqueDoProdutoDois);
            Fornecedor fornecedorDois = new("998767573", "fornecedorDois@gmail.com", "fornecedor teste Dois ltda", "fornecedor teste Dois",
                "25873458000186", "123456787");
            _contexto.Fornecedores.Add(fornecedorDois);
            EntradaEstoque entradaEstoqueDois = new(fornecedorDois, "abcdeg", StatusEntradaEstoque.Criada);
            _contexto.EntradasEstoque.Add(entradaEstoqueDois);
            ItemEntradaEstoque itemEntradaEstoqueDois = new(entradaEstoqueDois, produtoDois, 20);
            _contexto.ItensEntradaEstoque.Add(itemEntradaEstoqueDois);
            _contexto.SaveChanges();    

            List<EntradaEstoqueOutputDto> dtos=_service.BuscarEntradasAtivas();
            EntradaEstoqueOutputDto entradaUm = dtos.FirstOrDefault(e=>e.CodigoEntrada== "abcdef");
            Assert.NotNull(entradaUm);
            Assert.Equal(fornecedorUm.RazaoSocial, entradaUm.Fornecedor.RazaoSocial);
            Assert.Equal(StatusEntradaEstoque.Criada, entradaUm.Status);
            ItemEntradaEstoqueOutputDto itemEntradaUm = entradaUm.Itens[0];
            Assert.Equal(produtoUm.NomeProduto, itemEntradaUm.Produto.NomeProduto);
            Assert.Equal(10, itemEntradaUm.Quantidade);

            EntradaEstoqueOutputDto entradaDois = dtos.FirstOrDefault(e => e.CodigoEntrada == "abcdeg");
            Assert.NotNull(entradaDois);
            Assert.Equal(fornecedorDois.RazaoSocial, entradaDois.Fornecedor.RazaoSocial);
            Assert.Equal(StatusEntradaEstoque.Criada, entradaDois.Status);
            ItemEntradaEstoqueOutputDto itemEntradaDois = entradaDois.Itens[0];
            Assert.Equal(produtoDois.NomeProduto, itemEntradaDois.Produto.NomeProduto);
            Assert.Equal(20, itemEntradaDois.Quantidade);

        }
        [Fact]
        public void BuscarTodasEntradasInativasComSucesso()
        {
            Produto produtoUm = new("123456789", "Produto Teste", "bom produto", 100);
            _contexto.Produtos.Add(produtoUm);
            Estoque estoqueDoProdutoUm = new Estoque(produtoUm);
            _contexto.Estoques.Add(estoqueDoProdutoUm);
            Fornecedor fornecedorUm = new("998767574", "fornecedor@gmail.com", "fornecedor teste ltda", "fornecedor teste", "90685427000163", "123456789");
            _contexto.Fornecedores.Add(fornecedorUm);
            EntradaEstoque entradaEstoqueUm = new(fornecedorUm, "abcdef", StatusEntradaEstoque.Criada);
            entradaEstoqueUm.Ativo=false;
            _contexto.EntradasEstoque.Add(entradaEstoqueUm);
            ItemEntradaEstoque itemEntradaEstoqueUm = new(entradaEstoqueUm, produtoUm, 10);
            itemEntradaEstoqueUm.Ativo = false;
            _contexto.ItensEntradaEstoque.Add(itemEntradaEstoqueUm);

            Produto produtoDois = new("123456787", "Produto Teste dois", "bom produto dois", 200);
            _contexto.Produtos.Add(produtoDois);
            Estoque estoqueDoProdutoDois = new Estoque(produtoDois);
            _contexto.Estoques.Add(estoqueDoProdutoDois);
            Fornecedor fornecedorDois = new("998767573", "fornecedorDois@gmail.com", "fornecedor teste Dois ltda", "fornecedor teste Dois",
                "25873458000186", "123456787");
            _contexto.Fornecedores.Add(fornecedorDois);
            EntradaEstoque entradaEstoqueDois = new(fornecedorDois, "abcdeg", StatusEntradaEstoque.Criada);
            entradaEstoqueDois.Ativo = false;
            _contexto.EntradasEstoque.Add(entradaEstoqueDois);
            ItemEntradaEstoque itemEntradaEstoqueDois = new(entradaEstoqueDois, produtoDois, 20);
            itemEntradaEstoqueDois.Ativo=false;
            _contexto.ItensEntradaEstoque.Add(itemEntradaEstoqueDois);
            _contexto.SaveChanges();

            List<EntradaEstoqueOutputDto> dtos = _service.BuscarEntradasInativas();
            EntradaEstoqueOutputDto entradaUm = dtos.FirstOrDefault(e => e.CodigoEntrada == "abcdef");
            Assert.NotNull(entradaUm);
            Assert.False(entradaUm.Ativo);
            Assert.Equal(fornecedorUm.RazaoSocial, entradaUm.Fornecedor.RazaoSocial);
            Assert.Equal("abcdef", entradaUm.CodigoEntrada);
            Assert.Equal(StatusEntradaEstoque.Criada, entradaUm.Status);
            ItemEntradaEstoqueOutputDto itemEntradaUm = entradaUm.Itens[0];
            Assert.False(itemEntradaUm.Ativo);
            Assert.Equal(produtoUm.NomeProduto, itemEntradaUm.Produto.NomeProduto);
            Assert.Equal(10, itemEntradaUm.Quantidade);

            EntradaEstoqueOutputDto entradaDois = dtos.FirstOrDefault(e => e.CodigoEntrada == "abcdeg");
            Assert.NotNull(entradaDois);
            Assert.False(entradaDois.Ativo);
            Assert.Equal(fornecedorDois.RazaoSocial, entradaDois.Fornecedor.RazaoSocial);
            Assert.Equal("abcdeg", entradaDois.CodigoEntrada);
            Assert.Equal(StatusEntradaEstoque.Criada, entradaDois.Status);
            ItemEntradaEstoqueOutputDto itemEntradaDois = entradaDois.Itens[0];
            Assert.False(itemEntradaDois.Ativo);
            Assert.Equal(produtoDois.NomeProduto, itemEntradaDois.Produto.NomeProduto);
            Assert.Equal(20, itemEntradaDois.Quantidade);
        }

        [Fact]
        public void BuscarEntradaAtivaPorIdComSucesso()
        {
            Produto produto = new("123456789", "Produto Teste", "bom produto", 100);
            _contexto.Produtos.Add(produto);
            Estoque estoqueDoProduto = new Estoque(produto);
            _contexto.Estoques.Add(estoqueDoProduto);
            Fornecedor fornecedor = new("998767574", "fornecedor@gmail.com", "fornecedor teste ltda", "fornecedor teste", "90685427000163", "123456789");
            _contexto.Fornecedores.Add(fornecedor);
            EntradaEstoque entradaEstoque = new(fornecedor, "abcdef", StatusEntradaEstoque.Criada);
            _contexto.EntradasEstoque.Add(entradaEstoque);
            ItemEntradaEstoque itemEntradaEstoque = new(entradaEstoque, produto, 10);
            _contexto.ItensEntradaEstoque.Add(itemEntradaEstoque);
            _contexto.SaveChanges();

            Guid idEntradaEstoqueSalva=_contexto.EntradasEstoque.First().Id;

            EntradaEstoqueOutputDto dto = _service.BuscarEntradasAtivaOuInativaPorId(idEntradaEstoqueSalva);

            Assert.NotNull(dto);    
            Assert.Equal(fornecedor.RazaoSocial, dto.Fornecedor.RazaoSocial);
            Assert.Equal("abcdef", dto.CodigoEntrada);
            Assert.Equal(StatusEntradaEstoque.Criada, dto.Status);
            ItemEntradaEstoqueOutputDto itemEntradaUm = dto.Itens[0];
            Assert.Equal(produto.NomeProduto, itemEntradaUm.Produto.NomeProduto);
            Assert.Equal(10, itemEntradaUm.Quantidade);

        }

        [Fact]
        public void BuscarEntradaInativaPorIdComSucesso()
        {
            Produto produto = new("123456789", "Produto Teste", "bom produto", 100);
            _contexto.Produtos.Add(produto);
            Estoque estoqueDoProduto = new Estoque(produto);
            _contexto.Estoques.Add(estoqueDoProduto);
            Fornecedor fornecedor = new("998767574", "fornecedor@gmail.com", "fornecedor teste ltda", "fornecedor teste", "90685427000163", "123456789");
            _contexto.Fornecedores.Add(fornecedor);
            EntradaEstoque entradaEstoque = new(fornecedor, "abcdef", StatusEntradaEstoque.Criada);
            entradaEstoque.Ativo=false;
            _contexto.EntradasEstoque.Add(entradaEstoque);
            ItemEntradaEstoque itemEntradaEstoque = new(entradaEstoque, produto, 10);
            itemEntradaEstoque.Ativo=false;
            _contexto.ItensEntradaEstoque.Add(itemEntradaEstoque);
            _contexto.SaveChanges();

            Guid idEntradaEstoqueSalva = _contexto.EntradasEstoque.First().Id;

            EntradaEstoqueOutputDto dto = _service.BuscarEntradasAtivaOuInativaPorId(idEntradaEstoqueSalva);

            Assert.NotNull(dto);
            Assert.False(dto.Ativo);
            Assert.Equal(fornecedor.RazaoSocial, dto.Fornecedor.RazaoSocial);
            Assert.Equal("abcdef", dto.CodigoEntrada);
            Assert.Equal(StatusEntradaEstoque.Criada, dto.Status);
            ItemEntradaEstoqueOutputDto itemEntradaUm = dto.Itens[0];
            Assert.False(itemEntradaUm.Ativo);
            Assert.Equal(produto.NomeProduto, itemEntradaUm.Produto.NomeProduto);
            Assert.Equal(10, itemEntradaUm.Quantidade);
            

        }

        [Fact]
        public void BuscarEntradaAtivaPorIdInexistenteComFalha()
        {
            Produto produto = new("123456789", "Produto Teste", "bom produto", 100);
            _contexto.Produtos.Add(produto);
            Estoque estoqueDoProduto = new Estoque(produto);
            _contexto.Estoques.Add(estoqueDoProduto);
            Fornecedor fornecedor = new("998767574", "fornecedor@gmail.com", "fornecedor teste ltda", "fornecedor teste", "90685427000163", "123456789");
            _contexto.Fornecedores.Add(fornecedor);
            EntradaEstoque entradaEstoque = new(fornecedor, "abcdef", StatusEntradaEstoque.Criada);
            entradaEstoque.Ativo = false;
            _contexto.EntradasEstoque.Add(entradaEstoque);
            ItemEntradaEstoque itemEntradaEstoque = new(entradaEstoque, produto, 10);
            itemEntradaEstoque.Ativo = false;
            _contexto.ItensEntradaEstoque.Add(itemEntradaEstoque);
            _contexto.SaveChanges();

             Assert.Throws<ExcecaoDeRegraDeNegocio>(()=> _service.BuscarEntradasAtivaOuInativaPorId(Guid.NewGuid()));
        }


        [Fact]
        public void CadastrarEntradaEnviandoIdProdutoInexistenteFalha()
        {
            Fornecedor fornecedor = new("998767574", "fornecedor@gmail.com", "fornecedor teste ltda", "fornecedor teste", "90685427000163", "123456789");
            _contexto.Fornecedores.Add(fornecedor);
            Produto produto = new("123456789", "Produto Teste", "bom produto", 100);
            _contexto.Produtos.Add(produto);
            Estoque estoque = new(produto);
            _contexto.Estoques.Add(estoque);
            _contexto.SaveChanges();
            Guid idFornecedorVindoDoBanco = _contexto.Fornecedores.First().Id;
            Guid idProdutoVindoDoBanco = _contexto.Produtos.First().Id;

            List<ItemEntradaEstoqueCreateDto> itensCreateDto = new();
            itensCreateDto.Add(new ItemEntradaEstoqueCreateDto(Guid.NewGuid(), 10));

            EntradaEstoqueCreateDto createDto = new(idFornecedorVindoDoBanco, itensCreateDto);

            Assert.Throws<ExcecaoDeRegraDeNegocio>(() =>_service.Cadastrar(createDto));
        }

        [Fact]
        public void CadastrarEntradaComSucesso()
        {
            Fornecedor fornecedor = new("998767574", "fornecedor@gmail.com", "fornecedor teste ltda", "fornecedor teste", "90685427000163", "123456789");
            _contexto.Fornecedores.Add(fornecedor);
            Produto produto = new("123456789", "Produto Teste", "bom produto", 100);
            _contexto.Produtos.Add(produto);
            Estoque estoque = new(produto);
            _contexto.Estoques.Add(estoque);
            _contexto.SaveChanges();
            Guid idFornecedorVindoDoBanco=_contexto.Fornecedores.First().Id;
            Guid idProdutoVindoDoBanco=_contexto.Produtos.First().Id;

            List<ItemEntradaEstoqueCreateDto> itensCreateDto = new();
            itensCreateDto.Add(new ItemEntradaEstoqueCreateDto(idProdutoVindoDoBanco,10));

            EntradaEstoqueCreateDto createDto = new(idFornecedorVindoDoBanco, itensCreateDto);
            _service.Cadastrar(createDto);

            EntradaEstoque entradaEstoqueSalva = _contexto.EntradasEstoque.FirstOrDefault();

            Assert.NotNull(entradaEstoqueSalva);
            Assert.True(entradaEstoqueSalva.Ativo);
            Assert.Equal(fornecedor.RazaoSocial, entradaEstoqueSalva.Fornecedor.RazaoSocial);
            ItemEntradaEstoque itemEntradaUm= _contexto.ItensEntradaEstoque.FirstOrDefault(i=>i.IdEntradaEstoque==entradaEstoqueSalva.Id);
            Assert.NotNull(itemEntradaUm);
            Assert.Equal(produto.NomeProduto, itemEntradaUm.Produto.NomeProduto);
            Assert.Equal(10, itemEntradaUm.Quantidade);
            Assert.Equal(10, estoque.QuantidadeEmEstoque);

        }

        [Fact]
        public void AtualizarEntradaComSucesso()
        {
            Produto produtoUm = new("123456789", "Produto um", "bom produto um", 100);
            _contexto.Produtos.Add(produtoUm);
            Estoque estoqueDoProdutoUm = new Estoque(produtoUm);
            estoqueDoProdutoUm.AdicionarQuantidadeEmEstoque(10);
            _contexto.Estoques.Add(estoqueDoProdutoUm);

            Produto produtoDois = new("123456787", "Produto um", "bom produto dois", 200);
            _contexto.Produtos.Add(produtoDois);
            Estoque estoqueDoProdutoDois = new Estoque(produtoDois);
            estoqueDoProdutoDois.AdicionarQuantidadeEmEstoque(20);
            _contexto.Estoques.Add(estoqueDoProdutoDois);

            Produto produtoTres = new("123456786", "Produto tres", "bom produto tres", 300);
            _contexto.Produtos.Add(produtoTres);
            Estoque estoqueDoProdutoTres = new Estoque(produtoTres);
            _contexto.Estoques.Add(estoqueDoProdutoTres);
            Fornecedor fornecedorOriginal =
                new("998767574", "fornecedor@gmail.com", "fornecedor teste ltda", "fornecedor teste", "90685427000163", "123456789");
            _contexto.Fornecedores.Add(fornecedorOriginal);
            Fornecedor fornecedorAtualizado =
                new("99875734", "fornecedorAtualizado@gmail.com", "fornecedor atualizado ltda", "fornecedor atualizado", "18788142000120", "123456787");
            _contexto.Fornecedores.Add(fornecedorAtualizado);
            EntradaEstoque entradaEstoque = new(fornecedorOriginal, "abcdef", StatusEntradaEstoque.Criada);
            _contexto.EntradasEstoque.Add(entradaEstoque);
            ItemEntradaEstoque itemEntradaEstoqueUm = new(entradaEstoque, produtoUm, 10);
            ItemEntradaEstoque itemEntradaEstoqueDois = new(entradaEstoque, produtoDois, 20);
            _contexto.ItensEntradaEstoque.Add(itemEntradaEstoqueUm);
            _contexto.ItensEntradaEstoque.Add(itemEntradaEstoqueDois);
            _contexto.SaveChanges();

            ItemEntradaEstoqueUpdateDto itemUpdateDto = new(itemEntradaEstoqueDois.Id, 30);

            ItemEntradaEstoqueCreateDto itemCreateDto = new(produtoTres.Id, 40);

            EntradaEstoqueUpdateDto updateDto = new(fornecedorAtualizado.Id, new List<Guid>() { itemEntradaEstoqueUm.Id },
                new List<ItemEntradaEstoqueUpdateDto>() { itemUpdateDto }, new List<ItemEntradaEstoqueCreateDto>() { itemCreateDto });


            _service.Atualizar(entradaEstoque.Id, updateDto);

            EntradaEstoque entradaEstoqueAtualizada = _contexto.EntradasEstoque.FirstOrDefault(e => e.Id == entradaEstoque.Id);
            Assert.NotNull(entradaEstoqueAtualizada);
            Assert.Equal(StatusEntradaEstoque.Atualizada, entradaEstoqueAtualizada.Status);
            Assert.Equal(fornecedorAtualizado.Id, entradaEstoque.IdFornecedor);

            ItemEntradaEstoque itemExcluido = _contexto.ItensEntradaEstoque.FirstOrDefault(i => i.Id == itemEntradaEstoqueUm.Id);
            Assert.NotNull(itemExcluido);
            Assert.False(itemExcluido.Ativo);
            Assert.False(itemExcluido.Atual);
            Assert.Equal(0, estoqueDoProdutoUm.QuantidadeEmEstoque);

            ItemEntradaEstoque itemAtualizado = _contexto.ItensEntradaEstoque.FirstOrDefault(i => i.Id == itemEntradaEstoqueDois.Id);
            Assert.NotNull(itemAtualizado);
            Assert.Equal(30, itemAtualizado.Quantidade);
            Assert.Equal(30, estoqueDoProdutoDois.QuantidadeEmEstoque);
            ItemEntradaEstoque itemCriado = _contexto.ItensEntradaEstoque.FirstOrDefault(i => i.IdProduto == produtoTres.Id);
            Assert.NotNull(itemCriado);
            Assert.Equal(40, itemCriado.Quantidade);
            Assert.Equal(40, estoqueDoProdutoTres.QuantidadeEmEstoque);

        }

        [Fact]
        public void InativarEntradaComSucesso()
        {
            Produto produto = new("123456789", "Produto Teste", "bom produto", 100);
            _contexto.Produtos.Add(produto);
            Estoque estoqueDoProduto = new Estoque(produto);
            estoqueDoProduto.AdicionarQuantidadeEmEstoque(10);
            _contexto.Estoques.Add(estoqueDoProduto);
            Fornecedor fornecedor = new("998767574", "fornecedor@gmail.com", "fornecedor teste ltda", "fornecedor teste", "90685427000163", "123456789");
            _contexto.Fornecedores.Add(fornecedor);
            EntradaEstoque entradaEstoque = new(fornecedor, "abcdef", StatusEntradaEstoque.Criada);
            _contexto.EntradasEstoque.Add(entradaEstoque);
            ItemEntradaEstoque itemEntradaEstoque = new(entradaEstoque, produto, 10);
            _contexto.ItensEntradaEstoque.Add(itemEntradaEstoque);
            _contexto.SaveChanges();
  

            _service.InativarEntradaEstoque(entradaEstoque.Id);

            EntradaEstoque entradaEstoqueInativada = _contexto.EntradasEstoque.FirstOrDefault(e => e.Id == entradaEstoque.Id);
            Assert.NotNull(entradaEstoqueInativada);
            Assert.False(entradaEstoqueInativada.Ativo);
            ItemEntradaEstoque itemEntradaEstoqueUm= _contexto.ItensEntradaEstoque.FirstOrDefault(i => i.IdEntradaEstoque == entradaEstoque.Id);
            Assert.NotNull(itemEntradaEstoqueUm);
            Assert.False(itemEntradaEstoqueUm.Ativo);
        }

    }
}
