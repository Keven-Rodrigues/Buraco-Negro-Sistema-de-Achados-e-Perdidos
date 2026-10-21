using System.Collections.ObjectModel;
using System.ComponentModel;
using AppUsuario.Models;
using AppUsuario.Services;

namespace AppUsuario.ViewModels;

public class ItemViewModel : INotifyPropertyChanged
{
    private readonly DatabaseService banco = DatabaseService.Instancia;

    private int id;
    private string nome = "";
    private string descricao = "";
    private string categoria = "";
    private string local = "";
    private string status = "Perdido";
    private DateTime dataRegistro = DateTime.Now;

    public event PropertyChangedEventHandler? PropertyChanged;

    // Lista que aparece na tela. A ObservableCollection avisa a tela
    // sozinha quando itens são adicionados ou removidos.
    public ObservableCollection<Item> Itens { get; } = new();

    // Opções da caixa de escolha do status.
    public List<string> StatusOpcoes { get; } = new() { "Perdido", "Encontrado" };

    // Id 0 = objeto novo. Id diferente de 0 = editando um objeto existente.
    public bool EmEdicao => id != 0;

    public string TituloTela => EmEdicao ? "Editar objeto" : "Cadastrar objeto";

    public string Nome
    {
        get => nome;
        set { nome = value; Avisar(); }
    }

    public string Descricao
    {
        get => descricao;
        set { descricao = value; Avisar(); }
    }

    public string Categoria
    {
        get => categoria;
        set { categoria = value; Avisar(); }
    }

    public string Local
    {
        get => local;
        set { local = value; Avisar(); }
    }

    public string Status
    {
        get => status;
        set { status = value; Avisar(); }
    }

    // Busca os objetos no banco e atualiza a lista da tela.
    public void CarregarItens()
    {
        Itens.Clear();

        foreach (Item item in banco.ListarItens())
            Itens.Add(item);
    }

    // Preenche o formulário com os dados de um objeto para editar.
    public void CarregarParaEdicao(Item item)
    {
        id = item.Id;
        dataRegistro = item.DataRegistro;

        Nome = item.Nome;
        Descricao = item.Descricao;
        Categoria = item.Categoria;
        Local = item.Local;
        Status = item.Status;
    }

    // Cadastra um objeto novo ou atualiza um existente.
    // Retorna "" se deu certo, ou a mensagem de erro.
    public string Salvar()
    {
        // Proteção na lógica: só administrador cadastra ou edita objetos.
        if (!UsuarioAtual.Instancia.EhAdministrador)
            return "Apenas administradores podem cadastrar ou editar objetos.";

        if (string.IsNullOrWhiteSpace(Nome) ||
            string.IsNullOrWhiteSpace(Descricao) ||
            string.IsNullOrWhiteSpace(Categoria) ||
            string.IsNullOrWhiteSpace(Local))
            return "Preencha todos os campos.";

        if (Status != "Perdido" && Status != "Encontrado")
            return "Escolha o status do objeto.";

        Item item = new Item
        {
            Id = id,
            Nome = Nome.Trim(),
            Descricao = Descricao.Trim(),
            Categoria = Categoria.Trim(),
            Local = Local.Trim(),
            Status = Status,

            // Objeto novo recebe a data de agora; na edição mantém a data original.
            DataRegistro = EmEdicao ? dataRegistro : DateTime.Now
        };

        if (EmEdicao)
            banco.AtualizarItem(item);
        else
            banco.InserirItem(item);

        return "";
    }

    // Exclui um objeto. Retorna "" se deu certo, ou a mensagem de erro.
    public string Excluir(Item item)
    {
        // Proteção na lógica: só administrador exclui objetos.
        if (!UsuarioAtual.Instancia.EhAdministrador)
            return "Apenas administradores podem excluir objetos.";

        banco.ExcluirItem(item.Id);
        Itens.Remove(item);

        return "";
    }

    private void Avisar()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
    }
}