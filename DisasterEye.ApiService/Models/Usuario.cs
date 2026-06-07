using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DisasterEye.ApiService.Models;

[Table("USUARIOS")]
public class Usuario
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    [Column("NOME")]
    public string Nome { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    [Column("EMAIL")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Senha armazenada com hash BCrypt — NUNCA em texto limpo
    /// </summary>
    [Required, MaxLength(255)]
    [Column("SENHAHASH")]
    public string SenhaHash { get; set; } = string.Empty;

    /// <summary>
    /// Perfil: Administrador ou Pesquisador
    /// </summary>
    [MaxLength(50)]
    [Column("PERFIL")]
    public string Perfil { get; set; } = "Pesquisador";

    [Column("DATACADASTRO")]
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

    [Column("ATIVO")]
    public int Ativo { get; set; } = 1;
}
