using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorRounds : MonoBehaviour
{
    [SerializeField] private BolinhaController jogador1;
    [SerializeField] private BolinhaController jogador2;
    [SerializeField] private Vector3 spawnJ1;
    [SerializeField] private Vector3 spawnJ2;

    private int localVitoriasJ1 = 0;
    private int localVitoriasJ2 = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jogador1"))
        {
            PontuarSessao(2);
        }
        else if (other.CompareTag("Jogador2"))
        {
            PontuarSessao(1);
        }
    }

    private void PontuarSessao(int jogadorVencedor)
    {
        if (jogadorVencedor == 1)
        {
            localVitoriasJ1++;
        }
        else
        {
            localVitoriasJ2++;
        }

        PlayerObserverManager.NotificarPlacarAtualizado(localVitoriasJ1, localVitoriasJ2);

        ControladorInterfaceSumo ui = Object.FindFirstObjectByType<ControladorInterfaceSumo>();
        if (ui != null)
        {
            ui.ResetarMoedasUI();
        }

        if (localVitoriasJ1 >= 2)
        {
            PlayerPrefs.SetInt("VencedorPartida", 1);
            SceneManager.LoadScene("CenaVitoria");
        }
        else if (localVitoriasJ2 >= 2)
        {
            PlayerPrefs.SetInt("VencedorPartida", 2);
            SceneManager.LoadScene("CenaVitoria");
        }
        else
        {
            ResetarRound();
        }
    }

    private void ResetarRound()
    {
        if (jogador1 != null)
        {
            jogador1.ResetarBolinha(spawnJ1);
        }

        if (jogador2 != null)
        {
            jogador2.ResetarBolinha(spawnJ2);
        }

        Moeda[] moedasNoMapa = Object.FindObjectsByType<Moeda>(FindObjectsSortMode.None);
        foreach (Moeda m in moedasNoMapa)
        {
            Destroy(m.gameObject);
        }
    }
}