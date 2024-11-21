using Microsoft.EntityFrameworkCore;
using EcosaveAPI.Models;
using System;
using System.Collections.Generic;

namespace EcosaveAPI.Data
{
    public class EcosaveContext : DbContext
    {
        public EcosaveContext(DbContextOptions<EcosaveContext> options) : base(options) { }

        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Dispositivo> Dispositivos { get; set; }
        public DbSet<Ponto> Pontos { get; set; }
        public DbSet<Comodo> Comodos { get; set; }
        public DbSet<Consumo> Consumos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = "User Id=rm99433;Password=090397;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=orcl)))";

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string not found.");
                }

                optionsBuilder.UseOracle(connectionString);
            }
        }

        public bool TestConnection()
        {
            try
            {
                this.Database.OpenConnection();
                this.Database.CloseConnection();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
