using AppUsuario.Models;
using AppUsuario.ViewModels;

namespace AppUsuario.Views;

public partial class LoginPage : ContentPage
{
    private readonly UsuarioViewModel vm = new();

    public LoginPage()
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void btnEntrar_Clicked(object sender, EventArgs e)
    {
        if (!vm.Entrar())
        {
            await DisplayAlert("Erro", "Email ou senha inválidos.", "OK");
            return;
        }

        // Escolhe a tela de acordo com o tipo do usuário logado.
        if (UsuarioAtual.Instancia.EhAdministrador)
            await Navigation.PushAsync(new AdminPage());
        else
            await Navigation.PushAsync(new ClientePage());

        Navigation.RemovePage(this);
    }

    private async void btnRegistrar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CadastroPage());
    }
}