using AppUsuario.Models;
using AppUsuario.Services;
using AppUsuario.ViewModels;

namespace AppUsuario.Views;

public partial class AdminPage : ContentPage
{
    private readonly ItemViewModel vm = new();

    public AdminPage()
    {
        InitializeComponent();
        BindingContext = vm;
    }

    // Toda vez que a tela aparece, confere o acesso, recarrega a lista
    // e atualiza o nome.
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Proteção: só administrador pode ficar nesta tela.
        if (!UsuarioAtual.Instancia.EhAdministrador)
        {
            Navegacao.IrParaLogin();
            return;
        }

        vm.CarregarItens();

        BindingContext = null;
        BindingContext = vm;
    }

    private async void btnCadastrar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CadastroItemPage());
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
    private async void btnEditar_Clicked(object sender, EventArgs e)
    {
        if (sender is Button botao && botao.BindingContext is Item item)
            await Navigation.PushAsync(new CadastroItemPage(item));
    }

    private async void btnExcluir_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button botao || botao.BindingContext is not Item item)
            return;

        // Pede confirmação antes de excluir.
        bool confirmou = await DisplayAlert(
            "Excluir objeto",
            $"Deseja realmente excluir \"{item.Nome}\"?",
            "Excluir",
            "Cancelar");

        if (!confirmou)
            return;

        string erro = vm.Excluir(item);

        if (erro != "")
            await DisplayAlert("Atenção", erro, "OK");
    }
}