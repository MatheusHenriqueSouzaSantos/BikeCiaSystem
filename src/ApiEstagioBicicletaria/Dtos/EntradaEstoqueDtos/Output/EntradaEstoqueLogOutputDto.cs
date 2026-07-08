using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.EntradaEstoqueDtos.Output
{
    public class EntradaEstoqueLogOutputDto : BaseLogOutputDto
    {
        public Guid IdEntradaEstoque {  get; set; }
        public EntradaEstoqueLogOutputDto(Guid idEntradaEstoque, LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, 
            Guid idUsuarioResponsavel, DateTime dataCriacao, string codigoUsuarioResponsavel) 
            : base(TipoDtoLog.EntradaEstoque, acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao, codigoUsuarioResponsavel)
        {
            IdEntradaEstoque = idEntradaEstoque;
        }
    }
}
