using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCoreBasico.Domain
{
    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Column("Phone")]
        public string Telefone { get; set; }
        public string CEP { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
    }
}