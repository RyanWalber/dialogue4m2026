using UnityEngine;
using TMPro;

public class ZonaVitoria : MonoBehaviour
{
    [SerializeField] private GameObject painelVitoria;
    [SerializeField] private TextMeshProUGUI textoMoedas;
    [SerializeField] private int proximaFaseIndex = 2;
    private bool vitoriaAtiva = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !vitoriaAtiva)
        {
            vitoriaAtiva = true;
            Time.timeScale = 0f;

            int totalMoedasNaCena = FindObjectsByType<MoedaItem>(FindObjectsSortMode.None).Length + GameManager.Instance.moedasAtuaisFase;
            if (textoMoedas != null)
            {
                textoMoedas.text = $"Moedas: {GameManager.Instance.moedasAtuaisFase} / {totalMoedasNaCena}";
            }

            if (painelVitoria != null) painelVitoria.SetActive(true);

            GameManager.Instance.dadosAtuais.checkpointAlcancado = false;
            GameManager.Instance.dadosAtuais.faseAtualIndex = proximaFaseIndex;
            GameManager.Instance.dadosAtuais.moedasNoCheckpoint = 0;
            GameManager.Instance.dadosAtuais.idsMoedasColetadasNoCheckpoint.Clear();
            GameManager.Instance.SalvarEmSlot(0);
        }
    }

    private void Update()
    {
        if (vitoriaAtiva && Input.anyKeyDown)
        {
            Time.timeScale = 1f;
            GameManager.Instance.moedasAtuaisFase = 0;
            GameManager.Instance.moedasColetadasNestaSessao.Clear();
            GameManager.Instance.CarregarFase(proximaFaseIndex);
        }
    }
}