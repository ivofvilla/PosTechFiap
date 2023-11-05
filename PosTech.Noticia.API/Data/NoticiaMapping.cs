using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PosTech.Noticia.API.Models;

namespace PosTech.Noticia.API.Data
{
    public class NoticiaMapping : IEntityTypeConfiguration<Models.Noticias>
    {
        public void Configure(EntityTypeBuilder<Models.Noticias> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Autor)
                .HasDefaultValueSql("'string'");

            builder.Property(p => p.Descricao)
                .HasDefaultValueSql("'varchar'");

            builder.Property(p => p.Titulo)
                .HasDefaultValueSql("'varchar'");

            builder.Property(p => p.Autor)
                .HasDefaultValueSql("'varchar'");

            builder.Property(p => p.DataPublicacao)
                    .HasColumnType("Timestamp(0)");

            builder.Property(p => p.DataAtualizacao)
                .HasColumnType("Timestamp(0)");


            builder.Property(p => p.Descricao)
                .HasColumnType("varchar");


            builder.Property(p => p.Chapeu)
                .HasColumnType("varchar");

            builder.ToTable("Notiias");
        }
    }
}
