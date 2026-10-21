using SQLite;
using AppUsuario.Models;

namespace AppUsuario.Services;

public class DatabaseService
{
    // Instância única: todas as telas usam o mesmo serviço.
    private static readonly DatabaseService instancia = new();

    public static DatabaseService Instancia => instancia;

    private readonly SQLiteConnection db;

    // Consulta que junta Historico + Usuario + Item para trazer os nomes.
    private const string SqlHistorico =
        "SELECT h.Id AS Id, u.Nome AS UsuarioNome, i.Nome AS ItemNome, h.DataHora AS DataHora " +
        "FROM Historico h " +
        "INNER JOIN Usuario u ON u.Id = h.UsuarioId " +
        "INNER JOIN Item i ON i.Id = h.ItemId ";

    private DatabaseService()
    {
        string caminho = Path.Combine(FileSystem.AppDataDirectory, "usuarios.db3");
        db = new SQLiteConnection(caminho);

        // Cria as tabelas só se ainda não existirem (não apaga dados).
        db.CreateTable<Usuario>();
        db.CreateTable<Item>();
        db.CreateTable<Historico>();
    }

    // ---------- USUÁRIO ----------

    // Procura um usuário pelo e-mail.
    // ignorarId serve para não contar o próprio usuário ao editar.
    public Usuario? BuscarUsuarioPorEmail(string email, int ignorarId = 0)
    {
        return db.Table<Usuario>()
            .FirstOrDefault(x => x.Email == email && x.Id != ignorarId);
    }

    public Usuario? BuscarUsuarioPorId(int id)
    {
        return db.Table<Usuario>()
            .FirstOrDefault(x => x.Id == id);
    }

    // Retorna o usuário se e-mail e senha estiverem corretos; senão, null.
    public Usuario? Autenticar(string email, string senha)
    {
        return db.Table<Usuario>()
            .FirstOrDefault(x => x.Email == email && x.Senha == senha);
    }

    public void InserirUsuario(Usuario usuario)
    {
        db.Insert(usuario);
    }

    public void AtualizarUsuario(Usuario usuario)
    {
        db.Update(usuario);
    }

    // ---------- ITEM ----------

    public Item? BuscarItemPorId(int id)
    {
        return db.Table<Item>()
            .FirstOrDefault(x => x.Id == id);
    }

    public void InserirItem(Item item)
    {
        db.Insert(item);
    }

    public void AtualizarItem(Item item)
    {
        db.Update(item);
    }

    // Exclui o objeto e também as seleções dele no histórico,
    // para não sobrar registro apontando para um objeto que não existe mais.
    public void ExcluirItem(int id)
    {
        db.Table<Historico>().Delete(x => x.ItemId == id);
        db.Delete<Item>(id);
    }

    // Lista todos os objetos, do mais novo para o mais antigo.
    public List<Item> ListarItens()
    {
        return db.Table<Item>()
            .OrderByDescending(x => x.DataRegistro)
            .ToList();
    }

    // ---------- HISTÓRICO ----------

    // Salva uma seleção (interesse do cliente em um objeto).
    public void InserirHistorico(Historico historico)
    {
        db.Insert(historico);
    }

    // Histórico de todos os clientes (usado pelo administrador).
    public List<HistoricoDetalhe> ListarHistoricoCompleto()
    {
        return db.Query<HistoricoDetalhe>(
            SqlHistorico + "ORDER BY h.DataHora DESC");
    }

    // Histórico de um usuário específico (usado pelo cliente).
    public List<HistoricoDetalhe> ListarHistoricoDoUsuario(int usuarioId)
    {
        return db.Query<HistoricoDetalhe>(
            SqlHistorico + "WHERE h.UsuarioId = ? ORDER BY h.DataHora DESC",
            usuarioId);
    }
}