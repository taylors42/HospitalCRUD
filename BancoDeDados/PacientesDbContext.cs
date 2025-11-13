using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Models;
using System;

namespace BancoDeDados;

public sealed class PacientesDbContext : DbContext
{
    public PacientesDbContext(DbContextOptions<PacientesDbContext> options) : base(options) { }

    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Convenio> Convenios { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Paciente>()
            .HasIndex(e => e.CPF)
            .IsUnique();

        mb.Entity<Paciente>()
            .HasIndex(e => e.RG)
            .IsUnique();

        mb.Entity<Convenio>()
            .HasIndex(c => c.Nome)
            .IsUnique();

        mb.Entity<Paciente>()
            .HasOne<Convenio>()
            .WithMany();
    }
}
