namespace AppUsuario.Models;

// Guarda os dados de quem está logado. Existe uma única instância no app.
// A senha NÃO é guardada aqui.
public class UsuarioAtual
{
    private static readonly UsuarioAtual instancia = new();

    public static UsuarioAtual Instancia => instancia;

    private UsuarioAtual() { }

    public int Id { get; private set; }
    public string Nome { get; private set; } = "";
    public string Cpf { get; private set; } = "";
    public string Email { get; private set; } = "";
    public string Tipo { get; private set; } = "";

    // Verdadeiro quando existe alguém logado.
    public bool EstaLogado => Id != 0;

    public bool EhAdministrador => Tipo == "Administrador";

    public bool EhCliente => Tipo == "Cliente";

    // Copia os dados do usuário autenticado (sem a senha).
    public void Definir(Usuario usuario)
    {
        Id = usuario.Id;
        Nome = usuario.Nome;
        Cpf = usuario.Cpf;
        Email = usuario.Email;
        Tipo = string.IsNullOrEmpty(usuario.Tipo) ? "Cliente" : usuario.Tipo;
    }

    // Apaga os dados da sessão (usado no logout).
    public void Limpar()
    {
        Id = 0;
        Nome = "";
        Cpf = "";
        Email = "";
        Tipo = "";
    }
}