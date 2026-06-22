using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public enum EstadoDoJogo { Iniciando, MenuPrincipal, Gameplay }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public EstadoDoJogo estadoAtual;

    [Header("Dados de Seleção")]
    public BolinhaData bolinhaEscolhidaJ1;
    public BolinhaData bolinhaEscolhidaJ2;

    [Header("Controle de Pontuação (Melhor de 3)")]
    private int vitoriasJ1 = 0;
    private int vitoriasJ2 = 0;
    private int jogadorVencedorPartida = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        PlayerObserverManager.OnJogadorCaiu += ContabilizarQueda;
    }

    void OnDisable()
    {
        PlayerObserverManager.OnJogadorCaiu -= ContabilizarQueda;
    }

    public void MudarEstado(EstadoDoJogo novoEstado)
    {
        estadoAtual = novoEstado;
    }

    public void CarregarCena(string nomeDaCena)
    {
        if (nomeDaCena == "MainMenu") MudarEstado(EstadoDoJogo.MenuPrincipal);
        if (nomeDaCena == "SampleScene") MudarEstado(EstadoDoJogo.Gameplay);

        SceneManager.LoadScene(nomeDaCena);

        if (nomeDaCena == "SampleScene")
        {
            SceneManager.LoadScene("GUI", LoadSceneMode.Additive);
        }
    }

    public void AlocarInput(PlayerInput playerInput)
    {
        if (estadoAtual == EstadoDoJogo.Gameplay && playerInput != null)
        {
            playerInput.enabled = true;
        }
    }

    private void ContabilizarQueda(int numeroJogadorQueCaiu)
    {
        if (numeroJogadorQueCaiu == 1)
        {
            vitoriasJ2++;
        }
        else if (numeroJogadorQueCaiu == 2)
        {
            vitoriasJ1++;
        }

        PlayerObserverManager.NotificarPlacarAtualizado(vitoriasJ1, vitoriasJ2);

        if (vitoriasJ1 >= 2)
        {
            FinalizarPartida(1);
        }
        else if (vitoriasJ2 >= 2)
        {
            FinalizarPartida(2);
        }
        else
        {
            ReiniciarRound();
        }
    }

    private void ReiniciarRound()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void FinalizarPartida(int vencedor)
    {
        jogadorVencedorPartida = vencedor;
        SceneManager.LoadScene("CenaVitoria");
    }

    public void IniciarNovaPartida()
    {
        vitoriasJ1 = 0;
        vitoriasJ2 = 0;
        jogadorVencedorPartida = 0;
        SceneManager.LoadScene("SampleScene");
    }

    public void VoltarParaSelecao()
    {
        SceneManager.LoadScene("CenaSelecao");
    }

    public string ObterNomeBolinhaVencedora()
    {
        if (jogadorVencedorPartida == 1 && bolinhaEscolhidaJ1 != null) return bolinhaEscolhidaJ1.nomeBolinha;
        if (jogadorVencedorPartida == 2 && bolinhaEscolhidaJ2 != null) return bolinhaEscolhidaJ2.nomeBolinha;
        return "Bolinha Desconhecida";
    }

    public int ObterJogadorVencedor() => jogadorVencedorPartida;
    public int ObterVitoriasJ1() => vitoriasJ1;
    public int ObterVitoriasJ2() => vitoriasJ2;
}