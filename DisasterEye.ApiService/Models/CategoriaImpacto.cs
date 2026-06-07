using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DisasterEye.ApiService.Models;

[Table("CATEGORIASIMPACTO")]
public class CategoriaImpacto
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    [Column("NOME")]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(500)]
    [Column("DESCRICAO")]
    public string Descricao { get; set; } = string.Empty;

    [MaxLength(100)]
    [Column("TIPODESASTRE")]
    public string TipoDesastre { get; set; } = string.Empty;

    public ICollection<Tecnologia> Tecnologias { get; set; } = new List<Tecnologia>();
}
