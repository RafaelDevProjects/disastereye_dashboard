using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DisasterEye.ApiService.Models;

[Table("TECNOLOGIAS")]
public class Tecnologia
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    [Column("NOME")]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(1000)]
    [Column("DESCRICAO")]
    public string Descricao { get; set; } = string.Empty;

    [MaxLength(200)]
    [Column("ORIGEMESPACIAL")]
    public string OrigemEspacial { get; set; } = string.Empty;

    [Column("LATITUDE")]
    public double? Latitude { get; set; }

    [Column("LONGITUDE")]
    public double? Longitude { get; set; }

    [MaxLength(50)]
    [Column("STATUS")]
    public string Status { get; set; } = "Ativo";

    [Column("DATACADASTRO")]
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

    [Column("CATEGORIAIMPACTOID")]
    public int CategoriaImpactoId { get; set; }

    [ForeignKey("CategoriaImpactoId")]
    public CategoriaImpacto? CategoriaImpacto { get; set; }
}
