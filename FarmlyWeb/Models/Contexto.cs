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
        public DbSet<Estoque> Estoque { get; set; }


    }



}



