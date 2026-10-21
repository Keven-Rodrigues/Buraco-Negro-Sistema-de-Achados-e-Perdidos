using AppUsuario.Models;
using AppUsuario.Services;
using AppUsuario.ViewModels;

namespace AppUsuario.Views;

public partial class ClientePage : ContentPage
{
    private readonly ItemViewModel vm = new();

    public ClientePage()
    {
        InitializeComponent();
        BindingContext = vm;
    }

    // Toda vez que a tela aparece, confere o acesso, recarrega a lista
    // e atualiza o nome.
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Proteção: só cliente pode ficar nesta tela.
        if (!UsuarioAtual.Instancia.EhCliente)
        {
            Navegacao.IrParaLogin();
            return;
        }

        vm.CarregarItens();

        BindingContext = null;
        BindingContext = vm;
    }

    private async void btnHistorico_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HistoricoPage());
    }

    private async void btnMeusDados_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PrincipalPage());
    }

    private async void btnSair_Clicked(object sender, EventArgs e)
    {
        bool confirmou = await DisplayAlert(
            "Sair",
            "Deseja encerrar a sessão?",
            "Sair",
            "Cancelar");

        if (!confirmou)
            return;

        Navegacao.IrParaLogin();
    }

    // O botão está dentro do cartão do objeto, então o BindingContext
    // dele é o próprio Item daquele cartão.
    private async void btnDetalhes_Clicked(object sender, EventArgs e)
    {
        if (sender is Button botao && botao.BindingContext is Item item)
            await Navigation.PushAsync(new DetalhesItemPage(item));
    }
}