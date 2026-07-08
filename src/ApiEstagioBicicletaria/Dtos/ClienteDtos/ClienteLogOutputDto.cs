using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.ClienteDtos
{
    public class ClienteLogOutputDto : BaseLogOutputDto
    {
        public Guid IdCliente { get; private set; }
        public ClienteLogOutputDto(Guid idCliente,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo, 
            Guid idUsuarioResponsavel, DateTime dataCriacao,string codigoUsuarioResponsavel) 
            : base(TipoDtoLog.Cliente,acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao, codigoUsuarioResponsavel)
        {
            IdCliente = idCliente;
        }


    }
}
