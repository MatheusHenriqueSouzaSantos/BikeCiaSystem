using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.VendedorDtos
{
    public class VendedorLogOutputDto : BaseLogOutputDto
    {
        public Guid IdVendedor { get; private set; }

        public VendedorLogOutputDto(Guid idVendedor,LogAcao acao, string campoAlterado, 
            string valorAntigo, string valorNovo, Guid idUsuarioResponsavel, DateTime dataCriacao, string codigoUsuarioResponsavel) 
            : base(TipoDtoLog.Vendedor,acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao, codigoUsuarioResponsavel)
        {
            IdVendedor= idVendedor; 
        }

        
    }
}
