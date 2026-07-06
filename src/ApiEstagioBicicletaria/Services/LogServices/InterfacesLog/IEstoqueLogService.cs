using ApiEstagioBicicletaria.Entities;
using ApiEstagioBicicletaria.Entities.EstoqueDomain;
using ApiEstagioBicicletaria.Entities.ProdutoDomain;
using ApiEstagioBicicletaria.Entities.UsuarioDomain;
using System.Reflection;

namespace ApiEstagioBicicletaria.Services.LogServices.InterfacesLog
{
    public interface IEstoqueLogService
    {
        void CriarLogsDeCriacao(Estoque estoque, Produto produtoDoEstoque, Usuario usuarioResponsavel);
        void CriarLogDeAtualizacaoQuantidadeEmEstoque(Estoque estoqueModificado, Produto produtoDoEstoque, int quantidadeAnterior, int quantidadeAtual,
            AcaoQueAlterouEstoque acaoQueAlterouEstoque, Usuario usuarioResponsavel);

        void CriarLogsDeInativacao(Estoque estoque, Produto produtoDoEstoque, Usuario usuarioResponsavel);

        void CriarLogsDeReativacao(Estoque estoque, Produto produtoDoEstoque, Usuario usuarioResponsavel);
        
    }
}
