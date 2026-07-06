using ApiEstagioBicicletaria.Entities.EntradaEstoque;
using ApiEstagioBicicletaria.Entities.VendaDomain;
using ApiEstagioBicicletaria.Excecoes;
using ApiEstagioBicicletaria.Repositories;

namespace ApiEstagioBicicletaria.Utils
{
    public class GeradorCodigoIndentificador: IGeradorCodigoIdentificador
    {
        private static  Random _random = new Random();

        private readonly ContextoDb _contextoDb;

        private const int _tamanhoDoCodigoMovimento = 6;
        private const int _tamanhoDoCodigoUsuario = 4;
        private const string _caracteresParaACombinacao = "abcdefghjkmnpqrstuvwxyz23456789";

        public GeradorCodigoIndentificador(ContextoDb contextoDb)
        {
            _contextoDb= contextoDb;
        }

        public string GerarCodigoVenda()
        {

            string codigoGerado;

            do
            {
                char[] codigo = new char[_tamanhoDoCodigoMovimento];
                for (int i = 0; i < _tamanhoDoCodigoMovimento; i++)
                {
                    int indexAleatorio = _random.Next(_caracteresParaACombinacao.Length);
                    codigo[i] = _caracteresParaACombinacao[indexAleatorio];
                }
                codigoGerado = new string(codigo);

            }
            while (VerificarSeOCodigoVendaGeradoJaExisteNoBanco(codigoGerado));

            return codigoGerado;

        }

        private bool VerificarSeOCodigoVendaGeradoJaExisteNoBanco(string codigoGerado)
        {
            return _contextoDb.Vendas.Any(v => v.CodigoVenda == codigoGerado);
        }

        public string GerarCodigoEntradaEstoque()
        {
           
            string codigoGerado;

            do
            {
                char[] codigo = new char[_tamanhoDoCodigoMovimento];
                for (int i = 0; i < _tamanhoDoCodigoMovimento; i++)
                {
                    int indexAleatorio = _random.Next(_caracteresParaACombinacao.Length);
                    codigo[i] = _caracteresParaACombinacao[indexAleatorio];
                }
                codigoGerado = new string(codigo);

            }
            while (VerificarSeOCodigoEntradaEstoqueGeradoJaExisteNoBanco(codigoGerado));

            return codigoGerado;

        }

        private bool VerificarSeOCodigoEntradaEstoqueGeradoJaExisteNoBanco(string codigoGerado)
        {
            return _contextoDb.EntradasEstoque.Any(e => e.CodigoEntrada == codigoGerado);
        }

        public string GerarCodigoUsuario()
        {

            string codigoGerado;

            do
            {
                char[] codigo = new char[_tamanhoDoCodigoUsuario];
                for (int i = 0; i < _tamanhoDoCodigoUsuario; i++)
                {
                    int indexAleatorio = _random.Next(_caracteresParaACombinacao.Length);
                    codigo[i] = _caracteresParaACombinacao[indexAleatorio];
                }
                codigoGerado = new string(codigo);

            }
            while (VerificarSeOCodigoUsuarioGeradoJaExisteNoBanco(codigoGerado));

            return codigoGerado;

        }

        private bool VerificarSeOCodigoUsuarioGeradoJaExisteNoBanco(string codigoGerado)
        {   
            return _contextoDb.Usuarios.Any(u => u.CodigoUsuario == codigoGerado); 
        }
    }
}
