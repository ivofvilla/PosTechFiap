using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PosTech.Noticia.API.Models
{
    [Table(name: "Noticias", Schema = "public")]
    public class Noticias
    {
        [Key, Column("Id")]
        public Guid Id { get; set; }
        [Column("Titulo")]
        public string Titulo { get; set; }
        [Column("Descricao")]
        public string Descricao { get; set; }
        [Column("Chapeu")]
        public string Chapeu { get; set; }
        [Column("Autor")]
        public string Autor { get; set; }
        [Column("DataCriacao")]
        public DateTime DataPublicacao { get; set; }
        [Column("DataAtualizacao")]
        public DateTime DataAtualizacao { get; set; }
    }
}
