using AppUsuario.Models;
using AppUsuario.ViewModels;

namespace AppUsuario.Views;

public partial class DetalhesItemPage : ContentPage
{
    private readonly Item objeto;
    private readonly HistoricoViewModel vm = new();

    // Recebe o objeto escolhido na lista e liga a tela a ele.
    public DetalhesItemPage(Item item)
    {
        InitializeComponent();

        objeto = item;
        BindingContext = item;

        // O botão só aparece para clientes.
        btnSelecionar.IsVisible = UsuarioAtual.Instancia.EhCliente;
    }

    private async void btnSelecionar_Clicked(object sender, EventArgs e)
    {
        // Pede confirmação antes de registrar.
        bool confirmou = await DisplayAlert(
            "Confirmar seleção",
            $"Deseja registrar seu interesse no objeto \"{objeto.Nome}\"?",
            "Confirmar",
            "Cancelar");

        if (!confirmou)
            return;

        string erro = vm.Selecionar(objeto);

        if (erro != "")
        {
            await DisplayAlert("Atenção", erro, "OK");
            return;
        }

        await DisplayAlert(
            "Seleção registrada",
            "Seu interesse foi registrado. Isso não significa que o objeto foi devolvido ou recuperado.",
            "OK");
    }
}