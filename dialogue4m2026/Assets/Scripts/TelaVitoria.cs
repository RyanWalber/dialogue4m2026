using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TelaVitoria : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoVencedor;

    void Start()
    {
        int vencedor = PlayerPrefs.GetInt("VencedorPartida", 1);
        string nomeBolinha = "Desconhecida";

        if (GameManager.Instance != null)
        {
            if (vencedor == 1 && GameManager.Instance.bolinhaEscolhidaJ1 != null)
            {
                nomeBolinha = GameManager.Instance.bolinhaEscolhidaJ1.name;
            }
            else if (vencedor == 2 && GameManager.Instance.bolinhaEscolhidaJ2 != null)
            {
                nomeBolinha = GameManager.Instance.bolinhaEscolhidaJ2.name;
            }
        }

        if (textoVencedor != null)
        {
            textoVencedor.text = "Jogador " + vencedor + " venceu a partida usando a bolinha " + nomeBolinha + "!";
        }
    }

    public void VoltarAoMenu()
    {
        PlayerPrefs.DeleteKey("VencedorPartida");
        SceneManager.LoadScene("CenaSelecao");
    }
}