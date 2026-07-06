using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices.InterfacesLog
{
    public interface IItemEntradaEstoqueLogService
    {
        void CriarLogsDeCriacao(ItemEntradaEstoque itemEntradaEstoque, EntradaEstoque entradaEstoqueDoItem, Usuario usuarioResponsavel);

        void CriarLogsDeAtualizacao(ItemEntradaEstoque itemEntradaEstoqueAntigo, ItemEntradaEstoque itemEntradaEstoqueAtualizado,
            EntradaEstoque entradaEstoqueDoItem, Usuario usuarioResponsavel);

        void CriarLogsDeExclusao(ItemEntradaEstoque itemEntradaEstoque, EntradaEstoque entradaEstoqueDoItem, Usuario usuarioResponsavel);
    }
}
