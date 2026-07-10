namespace ApiEstagioBicicletaria.Dtos.RelatorioDtos
{
    public class VendaNoFormatoASerExibidoRelatorioDto
    {

        public string CodigoVenda { get; set; }
        public string NomeCliente { get; set; }

        public string NomeVendedor { get; set; }

        public string TipoDePagamento {  get; set; }

        public string DataDaVenda { get; set; }

        public decimal ValorTotalPago { get; set; }

        public decimal ValorTotal { get; set; }

        public string Status { get; set; }

        public VendaNoFormatoASerExibidoRelatorioDto(string codigoVenda, string nomeCliente, string nomeVendedor, 
            string tipoDePagamento, string dataDaVenda, decimal valorTotalPago, decimal valorTotal, string status)
        {
            CodigoVenda = codigoVenda;
            NomeCliente = nomeCliente;
            NomeVendedor = nomeVendedor;
            TipoDePagamento = tipoDePagamento;
            DataDaVenda = dataDaVenda;
            ValorTotalPago = valorTotalPago;
            ValorTotal = valorTotal;
            Status = status;
        }
    }
}
