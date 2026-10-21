namespace AppUsuario.Models;

// Usada só para mostrar o histórico na tela. Não é uma tabela do banco.
public class HistoricoDetalhe
{
    public int Id { get; set; }
    public string UsuarioNome { get; set; } = "";
    public string ItemNome { get; set; } = "";
    public DateTime DataHora { get; set; }
}