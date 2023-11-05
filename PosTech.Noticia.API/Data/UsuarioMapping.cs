using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PosTech.Noticia.API.Models;

namespace PosTech.Noticia.API.Data
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuarios>
    {
        public void Configure(EntityTypeBuilder<Usuarios> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Ativo)
                .HasDefaultValueSql("'boolean'");

            builder.Property(p => p.Senha)
                .HasDefaultValueSql("'varchar'");

            builder.Property(p => p.Login)
                .HasDefaultValueSql("'varchar'");

            builder.Property(p => p.Nome)
                .HasDefaultValueSql("'varchar'");

            builder.Property(p => p.DataCriacao)
                    .HasColumnType("Timestamp(0)");

            builder.Property(p => p.DataAtualizacao)
                .HasColumnType("Timestamp(0)");


            builder.Property(p => p.DataUltimoLogin)
                .HasColumnType("Timestamp(0)");

            builder.ToTable("Usuarios");
        }
    }


}
