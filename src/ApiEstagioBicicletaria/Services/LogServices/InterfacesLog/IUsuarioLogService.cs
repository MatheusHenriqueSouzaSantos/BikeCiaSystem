using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices.InterfacesLog
{
    public interface IUsuarioLogService
    {
        void CriarLogDeCriacao(Usuario usuario, Usuario usuarioResponsavel);

        void CriarLogsDeAtualizacao(Usuario usuarioAntigo, Usuario usuarioAtualizado, Usuario usuarioResponsavel);

        void CriarLogsDeInativacao(Usuario usuario, Usuario usuarioResponsavel);


        void CriarLogsDeReativacao(Usuario usuario, Usuario usuarioResponsavel);
   
    }
}
