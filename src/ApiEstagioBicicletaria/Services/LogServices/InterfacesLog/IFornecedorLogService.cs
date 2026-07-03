using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.FornedorDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using ApiEstagioBicicletaria.Repository.Repositorios;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices.InterfacesLog
{
    public interface IFornecedorLogService
    {
        void CriarLogsDeCriacao(Fornecedor fornecedor, Usuario usuarioResponsavel);

        void CriarLogsDeAtualizacao(Fornecedor fornecedorAntigo, Fornecedor fornecedorAtualizado, Usuario usuarioResponsavel);

        void CriarLogsDeInativacao(Fornecedor fornecedor, Usuario usuarioResponsavel);

        void CriarLogsDeReativacao(Fornecedor fornecedor, Usuario usuarioResponsavel);
            
    }
}
