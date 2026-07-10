using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TelaVitoria : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoVencedor;

    void Start()
    {
        int vencedor = PlayerPrefs.GetInt("VencedorPartida", 1);

        if (textoVencedor != null)
        {
            textoVencedor.text = "Jogador " + vencedor + " venceu!";
        }
    }

    public void VoltarAoMenu()
    {
        PlayerPrefs.DeleteKey("VencedorPartida");
        SceneManager.LoadScene("CenaSelecao");
    }
}