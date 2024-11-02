using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmlyWeb.Models
{
    [Table("tblEstoque")]
    public class Estoque
    {
        [Key]
        [Column("id_estoque")]
        [Display(Name = "Código do Estoque")]
        public int Id { get; set; }

        [Column("id_produto")]
        [Display(Name = "Código do Produto")]
        public int IdProduto { get; set; }

        [Column("quantidade")]
        [Display(Name = "Quantidade que movimentou")]
        public int Quantidade { get; set; }

        [Column("tipo_movimentacao")]
        [Display(Name = "Tipo da movimentação")]
        public string Movimentacao { get; set; }

        [Column("data_movimentacao")]
        [Display(Name = "Data da movimentação")]
        public DateTime Data { get; set; }
    }
}
