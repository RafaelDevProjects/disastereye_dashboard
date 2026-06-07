using System.ComponentModel.DataAnnotations;

namespace DisasterEye.Web.Models;

public class TecnologiaViewModel
{
    public int      Id               { get; set; }
    public string   Nome             { get; set; } = string.Empty;
    public string   Descricao        { get; set; } = string.Empty;
    public string   OrigemEspacial   { get; set; } = string.Empty;
    public double?  Latitude         { get; set; }
    public double?  Longitude        { get; set; }
    public string   Status           { get; set; } = string.Empty;
    public DateTime DataCadastro     { get; set; }
    public int      CategoriaImpactoId { get; set; }
    public string   CategoriaNome    { get; set; } = string.Empty;
}

public class TecnologiaFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [MaxLength(200)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Origem espacial é obrigatória")]
    public string OrigemEspacial { get; set; } = string.Empty;

    public double? Latitude  { get; set; }
    public double? Longitude { get; set; }

    [Required]
    public string Status { get; set; } = "Ativo";

    [Required(ErrorMessage = "Categoria é obrigatória")]
    public int CategoriaImpactoId { get; set; }

    public List<CategoriaViewModel> Categorias { get; set; } = new();
}

public class CategoriaViewModel
{
    public int    Id           { get; set; }
    public string Nome         { get; set; } = string.Empty;
    public string TipoDesastre { get; set; } = string.Empty;
}

public class DashboardViewModel
{
    public int TotalTecnologias         { get; set; }
    public int TotalMissoesMapeadas     { get; set; }
    public int TotalSetoresBeneficiados { get; set; }
    public List<SetorContagemViewModel> ContagemPorCategoria { get; set; } = new();
    public List<TecnologiaViewModel>    UltimosCadastros     { get; set; } = new();
}

public class SetorContagemViewModel
{
    public string Categoria  { get; set; } = string.Empty;
    public int    Quantidade { get; set; }
}

public class LoginViewModel
{
    [Required(ErrorMessage = "E-mail é obrigatório")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = string.Empty;
}

public class RegisterViewModel
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-mail é obrigatório")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = string.Empty;

    public string Perfil { get; set; } = "Pesquisador";
}
