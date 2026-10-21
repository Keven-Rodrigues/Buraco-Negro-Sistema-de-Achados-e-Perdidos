using System.ComponentModel;
using AppUsuario.Models;
using AppUsuario.Services;

namespace AppUsuario.ViewModels;

public class UsuarioViewModel : INotifyPropertyChanged
{
    private readonly DatabaseService banco = DatabaseService.Instancia;

    private int id;
    private string nome = "";
    private string cpf = "";
    private string email = "";
    private string senha = "";
    private string tipo = "Cliente";
    private DateTime dataNascimento = DateTime.Today;

    public event PropertyChangedEventHandler? PropertyChanged;

    // Opções que aparecem na caixa de escolha do tipo de usuário.
    public List<string> TiposUsuario { get; } = new() { "Cliente", "Administrador" };

    public int Id
    {
        get => id;
        set { id = value; Avisar(); }
    }

    public string Nome
    {
        get => nome;
        set { nome = value; Avisar(); }
    }

    public string Cpf
    {
        get => cpf;
        set { cpf = value; Avisar(); }
    }

    public string Email
    {
        get => email;
        set { email = value; Avisar(); }
    }

    public string Senha
    {
        get => senha;
        set { senha = value; Avisar(); }
    }

    public string Tipo
    {
        get => tipo;
        set { tipo = value; Avisar(); }
    }

    public DateTime DataNascimento
    {
        get => dataNascimento;
        set { dataNascimento = value; Avisar(); }
    }

    // O tipo só pode ser escolhido no cadastro novo.
    public bool PodeEscolherTipo => Id == 0;

    // Cadastra ou atualiza o usuário.
    public string Salvar()
    {
        if (string.IsNullOrWhiteSpace(Nome) ||
            string.IsNullOrWhiteSpace(Cpf) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Senha))
            return "Preencha todos os campos.";

        if (Id == 0 && Tipo != "Cliente" && Tipo != "Administrador")
            return "Escolha o tipo de usuário.";

        Email = Email.Trim();
        Cpf = Cpf.Trim();

        Usuario? existe = banco.BuscarUsuarioPorEmail(Email, Id);

        if (existe != null)
            return "Este e-mail já está cadastrado.";

        Usuario usuario = new Usuario
        {
            Id = Id,
            Nome = Nome,
            Cpf = Cpf,
            Email = Email,
            Senha = Senha,
            DataNascimento = DataNascimento
        };

        if (Id == 0)
        {
            // Em um sistema real, o cadastro público não deve deixar
            // qualquer pessoa virar administradora. Aqui é só para a atividade.
            usuario.Tipo = Tipo;
            banco.InserirUsuario(usuario);
        }
        else
        {
            // O tipo vem sempre do banco: ninguém troca o próprio tipo ao editar.
            Usuario? atual = banco.BuscarUsuarioPorId(Id);

            if (atual == null)
                return "Usuário não encontrado.";

            usuario.Tipo = atual.Tipo;

            banco.AtualizarUsuario(usuario);
            SalvarUsuarioLogado(usuario);
        }

        return "";
    }

    // Valida o login.
    public bool Entrar()
    {
        Email = Email.Trim();

        Usuario? usuario = banco.Autenticar(Email, Senha);

        if (usuario == null)
            return false;

        SalvarUsuarioLogado(usuario);
        return true;
    }

    // Guarda o usuário logado no Singleton (sem a senha).
    public void SalvarUsuarioLogado(Usuario usuario)
    {
        UsuarioAtual.Instancia.Definir(usuario);
    }

    // Preenche os campos com os dados de quem está logado.
    public void CarregarUsuarioLogado()
    {
        CarregarDados();
    }

    // Busca no banco os dados atualizados de quem está logado.
    // O Singleton não guarda senha nem data de nascimento,
    // por isso esses dados vêm do banco.
    public void CarregarDados()
    {
        if (!UsuarioAtual.Instancia.EstaLogado)
            return;

        Usuario? usuario = banco.BuscarUsuarioPorId(UsuarioAtual.Instancia.Id);

        if (usuario != null)
        {
            SalvarUsuarioLogado(usuario);
            PreencherCampos(usuario);
        }
    }

    private void PreencherCampos(Usuario usuario)
    {
        Id = usuario.Id;
        Nome = usuario.Nome;
        Cpf = usuario.Cpf;
        Email = usuario.Email;
        Senha = usuario.Senha;
        Tipo = string.IsNullOrEmpty(usuario.Tipo) ? "Cliente" : usuario.Tipo;
        DataNascimento = usuario.DataNascimento;
    }

    private void Avisar()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
    }
}