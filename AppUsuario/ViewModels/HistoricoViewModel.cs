using System.Collections.ObjectModel;
using AppUsuario.Models;
using AppUsuario.Services;

namespace AppUsuario.ViewModels;

public class HistoricoViewModel
{
    private readonly DatabaseService banco = DatabaseService.Instancia;

    // Lista que aparece na tela de histórico.
    public ObservableCollection<HistoricoDetalhe> Registros { get; } = new();

    public string TituloTela =>
        UsuarioAtual.Instancia.EhAdministrador ? "Histórico de seleções" : "Meu histórico";

    // O nome do usuário só interessa ao administrador,
    // porque o cliente vê apenas as próprias seleções.
    public bool MostrarUsuario => UsuarioAtual.Instancia.EhAdministrador;

    // Carrega o histórico de acordo com o tipo de usuário logado.
    public void Carregar()
    {
        Registros.Clear();

        List<HistoricoDetalhe> lista;

        if (UsuarioAtual.Instancia.EhAdministrador)
        {
            // Administrador vê as seleções de todos os clientes.
            lista = banco.ListarHistoricoCompleto();
        }
        else if (UsuarioAtual.Instancia.EhCliente)
        {
            // Cliente vê somente as próprias seleções.
            lista = banco.ListarHistoricoDoUsuario(UsuarioAtual.Instancia.Id);
        }
        else
        {
            // Ninguém logado: não mostra nada.
            return;
        }

        foreach (HistoricoDetalhe registro in lista)
            Registros.Add(registro);
    }

    // Registra o interesse do cliente em um objeto.
    // Retorna "" se deu certo, ou a mensagem de erro.
    public string Selecionar(Item item)
    {
        // Proteção na lógica: só cliente logado seleciona objetos.
        if (!UsuarioAtual.Instancia.EhCliente)
            return "Apenas clientes podem selecionar objetos.";

        // Confere se o objeto ainda existe (o administrador pode ter excluído).
        if (banco.BuscarItemPorId(item.Id) == null)
            return "Este objeto não existe mais.";

        Historico historico = new Historico
        {
            UsuarioId = UsuarioAtual.Instancia.Id,
            ItemId = item.Id,
            DataHora = DateTime.Now
        };

        banco.InserirHistorico(historico);

        return "";
    }
}