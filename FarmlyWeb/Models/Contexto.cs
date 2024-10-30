using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using FarmlyWeb.Models;

namespace FarmlyWeb.Models
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Venda> Venda { get; set; }
        public DbSet<VendaItens> VendaItens { get; set; }
        public DbSet<Produto> Produto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Venda>()
                .Property(v => v.Pagamento)
                .HasColumnType("decimal(18,2)"); // Ajuste a precisão e escala conforme necessário
        }


    }



}



