using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices.InterfacesLog
{
    public interface IEntradaEstoqueLogService
    {
        void CriarLogsDeCriacao(EntradaEstoque entradaEstoque, Usuario usuarioResponsavel);

        void CriarLogsDeAtualizacao(EntradaEstoque entradaEstoqueAntiga, EntradaEstoque entradaEstoqueAtualizada, Usuario usuarioResponsavel);

        void CriarLogsDeExclusao(EntradaEstoque entradaEstoque, StatusEntradaEstoque statusAnterior, Usuario usuarioResponsavel);
        
    }
}
