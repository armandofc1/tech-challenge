namespace WebApi.Entities;

using System.Text.Json.Serialization;

public class Cliente
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? SobreNome { get; set; }
    public string? CPF { get; set; }
    public string? Email { get; set; }

    [JsonIgnore]
    public string? Senha { get; set; }
}