using AppUsuario.Models;
using AppUsuario.Views;

namespace AppUsuario.Services;

public static class Navegacao
{
    // Encerra a sessão e volta para a tela de login.
    public static void IrParaLogin()
    {
        // 1) Apaga os dados de quem estava logado.
        UsuarioAtual.Instancia.Limpar();

        // 2) Troca toda a navegação por uma nova, começando no login.
        //    Isso descarta as telas antigas, então não dá para "voltar" a elas.
        Window? janela = Application.Current?.Windows.FirstOrDefault();

        if (janela != null)
            janela.Page = new NavigationPage(new LoginPage());
    }
}