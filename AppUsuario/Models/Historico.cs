using SQLite;

namespace AppUsuario.Models;

public class Historico
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // Quem fez a seleção (Id da tabela Usuario).
    public int UsuarioId { get; set; }

    // Qual objeto foi selecionado (Id da tabela Item).
    public int ItemId { get; set; }

    // Quando a seleção foi feita.
    public DateTime DataHora { get; set; } = DateTime.Now;
}