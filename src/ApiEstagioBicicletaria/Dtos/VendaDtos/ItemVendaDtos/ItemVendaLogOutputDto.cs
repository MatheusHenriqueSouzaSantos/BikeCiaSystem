using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos.ItemVendaDtos
{
    public class ItemVendaLogOutputDto : BaseLogOutputDto
    {
        public Guid IdItemVenda {  get; set; }

        public Guid IdProdutoDoItem { get; set; }
        public string NomeProdutoDoItem { get; private set; }
        public ItemVendaLogOutputDto(Guid idItemVenda,Guid idProdutoDoItem, string nomeProdutoDoItem,LogAcao acao, string campoAlterado,
            string valorAntigo, string valorNovo, Guid idUsuarioResponsavel, DateTime dataCriacao, string codigoUsuarioResponsavel) 
            : base(TipoDtoLog.ItemVenda,acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao, codigoUsuarioResponsavel)
        {
            IdItemVenda = idItemVenda;
            IdProdutoDoItem = idProdutoDoItem;
            NomeProdutoDoItem = nomeProdutoDoItem;
        }
    }
}
