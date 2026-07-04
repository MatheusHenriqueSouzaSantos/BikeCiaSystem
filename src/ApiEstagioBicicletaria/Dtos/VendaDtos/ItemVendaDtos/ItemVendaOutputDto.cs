using ApiEstagioBicicletaria.Dtos.ProdutoDtos;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos
{
    public class ItemVendaOutputDto
    {
        public Guid Id { get; private set; }

        //public Guid IdVenda { get; private set; }

        public ProdutoDtoOutPut Produto { get; private set; }

        public DateTime DataCriacao { get; private set; }

        public int Quantidade { get; set; }

        public decimal DescontoUnitario { get; set; }

        public decimal PrecoUnitarioDoProdutoNaVenda { get; set; }

        public decimal PrecoUnitarioDoProdutoNaVendaComDescontoAplicado { get; set; }

        public decimal ValorTotalDoItem { get; set; }

        public bool Ativo {  get; set; }

        protected ItemVendaOutputDto()
        {

        }

        public ItemVendaOutputDto(Guid id, ProdutoDtoOutPut produtoDto, DateTime dataCriacao, int quantidade, decimal descontoUnitario, 
            decimal precoUnitarioDoProdutoNaVenda, decimal precoUnitarioDoProdutoNaVendaComDescontoAplicado, decimal valorTotalDoItem, bool ativo)
        {
            Id = id;
            Produto = produtoDto;
            DataCriacao = dataCriacao;
            Quantidade = quantidade;
            DescontoUnitario = descontoUnitario;
            PrecoUnitarioDoProdutoNaVenda = precoUnitarioDoProdutoNaVenda;
            PrecoUnitarioDoProdutoNaVendaComDescontoAplicado = precoUnitarioDoProdutoNaVendaComDescontoAplicado;
            ValorTotalDoItem = valorTotalDoItem;
            Ativo = ativo;
        }
    }
}
