namespace DisasterEye.ApiService.DTOs;

public class TecnologiaCreateDto
{
    public string Nome             { get; set; } = string.Empty;
    public string Descricao        { get; set; } = string.Empty;
    public string OrigemEspacial   { get; set; } = string.Empty;
    public double? Latitude        { get; set; }
    public double? Longitude       { get; set; }
    public string Status           { get; set; } = "Ativo";
    public int    CategoriaImpactoId { get; set; }
}

public class TecnologiaResponseDto
{
    public int    Id               { get; set; }
    public string Nome             { get; set; } = string.Empty;
    public string Descricao        { get; set; } = string.Empty;
    public string OrigemEspacial   { get; set; } = string.Empty;
    public double? Latitude        { get; set; }
    public double? Longitude       { get; set; }
    public string Status           { get; set; } = string.Empty;
    public DateTime DataCadastro   { get; set; }
    public int    CategoriaImpactoId { get; set; }
    public string CategoriaNome    { get; set; } = string.Empty;
}

public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public bool   Sucesso     { get; set; }
    public string? Mensagem   { get; set; }
    public int?   UsuarioId   { get; set; }
    public string? Nome       { get; set; }
    public string? Perfil     { get; set; }
    public string? Email      { get; set; }
}

public class UsuarioCreateDto
{
    public string Nome   { get; set; } = string.Empty;
    public string Email  { get; set; } = string.Empty;
    public string Senha  { get; set; } = string.Empty;
    public string Perfil { get; set; } = "Pesquisador";
}
