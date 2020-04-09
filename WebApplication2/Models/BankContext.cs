using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Diplom_Autentif.Models
{
    public class BankContext : DbContext
    {
        public BankContext() : base("DbConnection") { }

        public DbSet<Card>      Cards      { get; set; }
        public DbSet<Bill>      Bills      { get; set; }
        public DbSet<Client>    Clients    { get; set; }
        public DbSet<Terminal>  Terminals  { get; set; }
        public DbSet<Operation> Operations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().HasMany(p => p.Cards)
                                         .WithRequired(p => p.Owner);

            modelBuilder.Entity<Bill>().HasMany(p => p.Cards)
                                       .WithRequired(p => p.Account);

            modelBuilder.Entity<Bill>().HasMany(p => p.Operations)
                                       .WithRequired(p => p.Account_from);

            modelBuilder.Entity<Bill>().HasMany(p => p.Operations)
                                       .WithRequired(p => p.Account_to);

            modelBuilder.Entity<Terminal>().HasMany(p => p.Operations)
                                           .WithRequired(p => p.Terminal);
        }

        public int GetClientId(string First, string Second, string Third)
        {
            foreach (var i in Clients)
            {
                if (i.First_name == First && i.Second_name == Second && i.Third_name == Third)
                    return i.Id;
            }
            return 0;
        }


    }
}