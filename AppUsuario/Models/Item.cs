using SQLite;

namespace AppUsuario.Models;

public class Item
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string Descricao { get; set; } = "";
    public string Categoria { get; set; } = "";

    // Local onde o objeto foi perdido ou encontrado.
    public string Local { get; set; } = "";

    public DateTime DataRegistro { get; set; } = DateTime.Now;

    // Status do objeto: "Perdido" ou "Encontrado".
    public string Status { get; set; } = "Perdido";
}