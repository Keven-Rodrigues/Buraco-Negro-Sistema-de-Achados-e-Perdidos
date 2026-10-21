using AppUsuario.Models;
using AppUsuario.Services;
using AppUsuario.ViewModels;

namespace AppUsuario.Views;

public partial class HistoricoPage : ContentPage
{
    private readonly HistoricoViewModel vm = new();

    public HistoricoPage()
    {
        InitializeComponent();
        BindingContext = vm;
    }

    // Toda vez que a tela aparece, confere o acesso e recarrega o histórico.
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Proteção: só quem está logado vê o histórico.
        if (!UsuarioAtual.Instancia.EstaLogado)
        {
            Navegacao.IrParaLogin();
            return;
        }

        vm.Carregar();
    }
}