using AppUsuario.ViewModels;

namespace AppUsuario.Views;

public partial class PrincipalPage : ContentPage
{
    private readonly UsuarioViewModel vm = new();

    public PrincipalPage()
    {
        InitializeComponent();
        BindingContext = vm;
        vm.CarregarUsuarioLogado();
    }

    private async void btnAtualizar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CadastroPage());
    }

    private void btnVisualizar_Clicked(object sender, EventArgs e)
    {
        vm.CarregarDados();
    }
}
