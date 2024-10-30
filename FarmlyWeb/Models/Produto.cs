using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmlyWeb.Models
{
    [Table("tblProduto")]
    public class Produto
    {
        [Key]
        [Column("id_produto")]
        [Display(Name = "Código do Produto")]
        public int Id { get; set; }

        [Column("nome")]
        [Display(Name = "Nome do Produto")]
        public string Nome { get; set; }

        [Column("descricao")]
        [Display(Name = "Descrição do Produto")]
        public string Descricao { get; set; }

        [Column("Preco")]
        [Display(Name = "Preço do Produto")]
        [Precision(18, 2)] // Define a precisão e a escala diretamente na propriedade
        public decimal Preco { get; set; }

        [Column("quantidade")]
        [Display(Name = "Quantidade em Estoque")]
        public int QuantidadeEstoque { get; set; }
    }
}
