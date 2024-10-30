using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmlyWeb.Models
{
    [Table("tblCliente")]
    public class Cliente
    {
        [Key]
        [Column("id_cliente")]
        [Display(Name = "Código do Cliente")]
        public int Id { get; set; }

        [Column("nome")]
        [Display(Name = "Nome do Cliente")]
        public string Nome { get; set; }

        [Column("cpf")]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Column("senha")]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Column("endereco")]
        [Display(Name = "Endereço")]
        public string Endereco { get; set; }

        [Column("telefone")]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; }

        [Column("email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
