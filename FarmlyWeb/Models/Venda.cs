using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmlyWeb.Models
{
    [Table("tblVenda")]
    public class Venda
    {
        [Key]
        [Column("id_venda")]
        [Display(Name = "Código da Venda")]
        public int Id { get; set; }

        [Column("id_cliente")]
        [Display(Name = "Cliente")]
        public int IdCliente { get; set; }

        [Column("data_venda")]
        [Display(Name = "Data da Venda")]
        public DateTime DataVenda { get; set; }

        [Column("preco_total")]
        [Display(Name = "Preço total da venda")]
        [Precision(18, 2)] // Define a precisão e a escala diretamente na propriedade
        public decimal Preco { get; set; }

        [Column("forma_pagamento")]
        [Display(Name = "Forma de pagamento")]
        public decimal Pagamento { get; set; }

        [Column("status")]
        [Display(Name = "Status do pagamento")]
        public int Status { get; set; }


    }
}
