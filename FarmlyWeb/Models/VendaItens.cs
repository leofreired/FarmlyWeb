using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmlyWeb.Models
{
    [Table("tblVendaItens")]
    public class VendaItens
    {
        [Key]
        [Column("id_item")]
        [Display(Name = "Código do ItemVenda")]
        public int Id { get; set; }

        [Column("id_venda")]
        [Display(Name = "Código da Venda")]
        public int IdVenda { get; set; }

        [Column("id_produto")]
        [Display(Name = "Código do Produto")]
        public int IdProduto { get; set; }

        [Column("quantidade")]
        [Display(Name = "Quantidade de Itens")]
        public int Quantidade { get; set; }

        [Column("preco_unitario")]
        [Display(Name = "Preço unitário")]
        [Precision(18, 2)] // Define a precisão e a escala diretamente na propriedade
        public decimal Preco { get; set; }


    }
}
