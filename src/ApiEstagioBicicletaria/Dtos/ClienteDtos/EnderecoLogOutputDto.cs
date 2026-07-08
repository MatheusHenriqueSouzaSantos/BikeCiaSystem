using ApiEstagioBicicletaria.Entities;

namespace ApiEstagioBicicletaria.Dtos.ClienteDtos
{
    public class EnderecoLogOutputDto : BaseLogOutputDto
    {

        public Guid IdEndereco { get; private set; }

        public EnderecoLogOutputDto(Guid idEndereco,LogAcao acao, string campoAlterado, string valorAntigo, string valorNovo,
            Guid idUsuarioResponsavel, DateTime dataCriacao, string codigoUsuarioResponsavel) 
            : base(TipoDtoLog.Endereco,acao, campoAlterado, valorAntigo, valorNovo, idUsuarioResponsavel, dataCriacao, codigoUsuarioResponsavel)
        {
            IdEndereco = idEndereco;
        }

    }
}
