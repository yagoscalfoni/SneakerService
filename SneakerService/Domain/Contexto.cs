using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SneakerService.Domain;

public partial class Contexto : DbContext
{
    public Contexto()
    {
    }

    public Contexto(DbContextOptions<Contexto> options)
        : base(options)
    {
    }

    public virtual DbSet<Teni> Tenis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;port=5432;Database=sneakerdb;Username=postgres;Password=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Teni>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tenis_pk");

            entity.ToTable("tenis");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataIntegracao)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("data_integracao");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");
            entity.Property(e => e.PercentualDesconto).HasColumnName("percentual_desconto");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .HasColumnName("tipo");
            entity.Property(e => e.ValorAnterior)
                .HasPrecision(7, 2)
                .HasColumnName("valor_anterior");
            entity.Property(e => e.ValorAtual)
                .HasPrecision(7, 2)
                .HasColumnName("valor_atual");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
