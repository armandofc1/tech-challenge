using System.ComponentModel.DataAnnotations;
namespace WebApi.Models.Cliente;

public class UpdateRequest
{
    public string? Nome { get; set; }
    public string? Sobrenome { get; set; }
    public string? CPF { get; set; }
    public string? Email { get; set; }

    // treat empty string as null for password fields to 
    // make them optional in front end apps
    private string? _senha;
    [MinLength(6)]
    public string? Senha
    {
        get => _senha;
        set => _senha = replaceEmptyWithNull(value);
    }

    private string? _confirmaSenha;
    [Compare("Senha")]
    public string? ConfirmaSenha
    {
        get => _confirmaSenha;
        set => _confirmaSenha = replaceEmptyWithNull(value);
    }

    // helpers

    private string? replaceEmptyWithNull(string? value)
    {
        // replace empty string with null to make field optional
        return string.IsNullOrEmpty(value) ? null : value;
    }
}