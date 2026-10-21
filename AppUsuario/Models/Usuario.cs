using SQLite;

namespace AppUsuario.Models;

public class Usuario
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string Cpf { get; set; } = "";
    public string Email { get; set; } = "";
    public string Senha { get; set; } = "";
    public DateTime DataNascimento { get; set; } = DateTime.Today;

    // Tipo do usuário: "Administrador" ou "Cliente".
    public string Tipo { get; set; } = "Cliente";
}