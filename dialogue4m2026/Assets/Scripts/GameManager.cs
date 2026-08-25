using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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

    [Header("Dados de Save e Moedas (Parte 4)")]
    public SaveData dadosAtuais = new SaveData();
    public int moedasAtuaisFase = 0;
    public List<string> moedasColetadasNestaSessao = new List<string>();

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

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "_Boot")
        {
            CarregarCena("Splash");
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
        if (nomeDaCena.StartsWith("Fase") || nomeDaCena == "SampleScene") MudarEstado(EstadoDoJogo.Gameplay);

        SceneManager.LoadScene(nomeDaCena);

        if (nomeDaCena.StartsWith("Fase") || nomeDaCena == "SampleScene")
        {
            SceneManager.LoadScene("GUI", LoadSceneMode.Additive);
        }
    }

    public void NovoJogo()
    {
        dadosAtuais = new SaveData();
        moedasAtuaisFase = 0;
        moedasColetadasNestaSessao.Clear();
        CarregarFase(1);
    }

    public void SalvarEmSlot(int slot)
    {
        SaveSystem.SalvarSlot(slot, dadosAtuais);
        if (slot != 0)
        {
            SaveSystem.SalvarSlot(0, dadosAtuais);
        }
    }

    public void CarregarSlot(int slot)
    {
        SaveData data = SaveSystem.CarregarSlot(slot);
        if (data == null) return;

        dadosAtuais = data;
        if (slot != 0)
        {
            SaveSystem.SalvarSlot(0, dadosAtuais);
        }

        moedasAtuaisFase = dadosAtuais.moedasNoCheckpoint;
        moedasColetadasNestaSessao = new List<string>(dadosAtuais.idsMoedasColetadasNoCheckpoint);
        CarregarFase(dadosAtuais.faseAtualIndex);
    }

    public void CarregarFase(int indexFase)
    {
        dadosAtuais.faseAtualIndex = indexFase;
        CarregarCena($"Fase{indexFase}");
    }

    public void RegistrarCheckpoint(Vector3 posicao)
    {
        dadosAtuais.checkpointAlcancado = true;
        dadosAtuais.checkpointPosX = posicao.x;
        dadosAtuais.checkpointPosY = posicao.y;
        dadosAtuais.checkpointPosZ = posicao.z;
        dadosAtuais.moedasNoCheckpoint = moedasAtuaisFase;
        dadosAtuais.idsMoedasColetadasNoCheckpoint = new List<string>(moedasColetadasNestaSessao);

        SalvarEmSlot(0);
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
        CarregarFase(dadosAtuais.faseAtualIndex);
    }

    private void FinalizarPartida(int vencedor)
    {
        jogadorVencedorPartida = vencedor;
        CarregarCena("CenaVitoria");
    }

    public void IniciarNovaPartida()
    {
        vitoriasJ1 = 0;
        vitoriasJ2 = 0;
        jogadorVencedorPartida = 0;
        NovoJogo();
    }

    public void VoltarParaSelecao()
    {
        CarregarCena("CenaSelecao");
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