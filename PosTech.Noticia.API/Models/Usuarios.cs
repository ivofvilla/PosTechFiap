using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PosTech.Noticia.API.Models
{
    [Table(name: "Usuarios", Schema = "public")]
    public class Usuarios
    {
        [Key, Column("Id")]
        public Guid Id { get; set; }
        [Key, Column("Nome")]
        public string Nome { get; set; }
        [Key, Column("Login")]
        public string Login { get; set; }
        [Key, Column("Senha")]
        public string Senha { get; set; }
        [Key, Column("Ativo")]
        public bool Ativo { get; set; }
        [Key, Column("DataCriacao")]
        public DateTime DataCriacao { get; set; }
        [Key, Column("DataAtualizacao")]
        public DateTime DataAtualizacao { get; set; }
        [Key, Column("DataUltimoLogin")]
        public DateTime DataUltimoLogin { get; set; }

        public Usuarios(string login, string senha, string nome)
        {
            this.Login = login;
            this.Senha = senha;
            this.Nome = nome;
            this.Id = Guid.NewGuid();
            this.DataCriacao = DateTime.Now;
            this.DataAtualizacao = DateTime.Now;
            this.Ativo = true;
        }
    }
}
