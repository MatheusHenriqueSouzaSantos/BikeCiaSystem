using ApiEstagioBicicletaria.Repositories;
using System;

namespace ApiEstagioBicicletaria.Utils
{
    public interface IGeradorCodigoIdentificador
    {
        string GerarCodigoVenda();

        string GerarCodigoEntradaEstoque();

        string GerarCodigoUsuario();
    }
}
