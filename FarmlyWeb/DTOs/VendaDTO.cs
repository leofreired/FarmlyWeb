namespace FarmlyWeb.DTOs
{
    public class VendaDTO
    {
        public int IdCliente { get; set; }
        public DateTime DataVenda { get; set; }
        public decimal PrecoTotal { get; set; }
        public string Pagamento { get; set; }
        public string Status { get; set; }
    }
}
