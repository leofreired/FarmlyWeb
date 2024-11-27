namespace FarmlyWeb.DTOs
{
    public class VendaItesDTO
    {
        public int IdVenda { get; set; }
        public int IdProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal PrecoTotal { get; set; }
    }
}