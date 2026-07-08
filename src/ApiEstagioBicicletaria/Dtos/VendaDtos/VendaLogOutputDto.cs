using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.VendaDtos
{
    public class VendaLogOutputDto : BaseLogOutputDto
    {
        Guid IdVenda {  get; set; }
        public VendaLogOutputDto(Guid idVenda,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo,
            Guid idUsuarioResponsavel, DateTime dataCriacao, string codigoUsuarioResponsavel)
            : base(TipoDtoLog.Venda, acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao, codigoUsuarioResponsavel)
        {
            IdVenda = idVenda;
        }
    }
}
