using System.ComponentModel.DataAnnotations;
namespace WebApi.Models.Cliente;

public class CreateRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string? Nome { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(200)]
    public string? Sobrenome { get; set; }

    [Required]
    [MaxLength(11)]
    public string? CPF { get; set; }

    [EmailAddress]
    [MaxLength(250)]
    public string? Email { get; set; }

    [Required]
    [MinLength(6)]
    [MaxLength(30)]
    public string? Senha { get; set; }

    [Required]
    [Compare("Senha")]
    [MaxLength(30)]
    public string? ConfirmaSenha{ get; set; }
}