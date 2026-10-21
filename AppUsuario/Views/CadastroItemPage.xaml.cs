using AppUsuario.Models;
using AppUsuario.Services;
using AppUsuario.ViewModels;

namespace AppUsuario.Views;

public partial class CadastroItemPage : ContentPage
{
    private readonly ItemViewModel vm = new();

    // Usado para cadastrar um objeto novo.
    public CadastroItemPage()
    {
        InitializeComponent();
        BindingContext = vm;
    }

    // Usado para editar um objeto que já existe.
    public CadastroItemPage(Item item)
    {
        InitializeComponent();
        vm.CarregarParaEdicao(item);
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Proteção: só administrador cadastra ou edita objetos.
        if (!UsuarioAtual.Instancia.EhAdministrador)
            Navegacao.IrParaLogin();
    }

    private async void btnSalvar_Clicked(object sender, EventArgs e)
    {
        string erro = vm.Salvar();

        if (erro != "")
        {
            await DisplayAlert("Atenção", erro, "OK");
            return;
        }

        await DisplayAlert("Sucesso", "Objeto salvo com sucesso!", "OK");
        await Navigation.PopAsync();
    }
}