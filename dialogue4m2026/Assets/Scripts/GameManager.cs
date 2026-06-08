using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public enum EstadoDoJogo { Iniciando, MenuPrincipal, Gameplay }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public EstadoDoJogo estadoAtual;

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
        MudarEstado(EstadoDoJogo.Iniciando);
        CarregarCena("Splash");
    }

    public void MudarEstado(EstadoDoJogo novoEstado)
    {
        estadoAtual = novoEstado;
        Debug.Log("Estado Atual: " + estadoAtual);
    }

    public void CarregarCena(string nomeDaCena)
    {
        if (nomeDaCena == "MainMenu") MudarEstado(EstadoDoJogo.MenuPrincipal);
        
        if (nomeDaCena == "SampleScene") 
        {
            MudarEstado(EstadoDoJogo.Gameplay);
        }

        SceneManager.LoadScene(nomeDaCena);

        if (nomeDaCena == "SampleScene")
        {
            SceneManager.LoadScene("GUI", LoadSceneMode.Additive);
        }
    }

    public void AlocarInput(PlayerInput playerInput)
    {
        if (estadoAtual == EstadoDoJogo.Gameplay)
        {
            playerInput.enabled = true;
        }
    }
}