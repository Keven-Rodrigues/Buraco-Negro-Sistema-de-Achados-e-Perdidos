using AppUsuario.ViewModels;

namespace AppUsuario.Views;

public partial class CadastroPage : ContentPage
{
    private readonly UsuarioViewModel vm = new();

    public CadastroPage()
    {
        InitializeComponent();
        BindingContext = vm;
        vm.CarregarUsuarioLogado();
    }

    private async void btnSalvar_Clicked(object sender, EventArgs e)
    {
        string erro = vm.Salvar();

        if (erro != "")
        {
            await DisplayAlert("Atenção", erro, "OK");
            return;
        }

        await DisplayAlert("Sucesso", "Cadastro salvo com sucesso!", "OK");
        await Navigation.PopAsync();
    }
}
