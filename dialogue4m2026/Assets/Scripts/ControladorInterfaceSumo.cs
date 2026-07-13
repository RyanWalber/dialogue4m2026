using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControladorInterfaceSumo : MonoBehaviour
{
    [Header("Placar de Rounds")]
    [SerializeField] private TextMeshProUGUI textoPlacar;

    [Header("Contadores de Moedas Individuais")]
    [SerializeField] private TextMeshProUGUI textoMoedasJ1;
    [SerializeField] private TextMeshProUGUI textoMoedasJ2;

    [Header("Barras de Cooldown (Sliders)")]
    [SerializeField] private Slider barraCooldownJ1;
    [SerializeField] private Slider barraCooldownJ2;

    void OnEnable()
    {
        PlayerObserverManager.OnPlacarAtualizado += AtualizarPlacarUI;
        PlayerObserverManager.OnCooldownAtualizado += AtualizarBarrasCooldown;
        PlayerObserverManager.OnMoedasAtualizadas += AtualizarMoedasUI;
    }

    void OnDisable()
    {
        PlayerObserverManager.OnPlacarAtualizado -= AtualizarPlacarUI;
        PlayerObserverManager.OnCooldownAtualizado -= AtualizarBarrasCooldown;
        PlayerObserverManager.OnMoedasAtualizadas -= AtualizarMoedasUI;
    }

    void Start()
    {
        if (GameManager.Instance != null)
        {
            AtualizarPlacarUI(GameManager.Instance.ObterVitoriasJ1(), GameManager.Instance.ObterVitoriasJ2());
        }

        if (barraCooldownJ1 != null) barraCooldownJ1.value = 1f;
        if (barraCooldownJ2 != null) barraCooldownJ2.value = 1f;

        ResetarMoedasUI();
    }

    private void AtualizarPlacarUI(int vitoriasJ1, int vitoriasJ2)
    {
        if (textoPlacar != null)
        {
            textoPlacar.text = vitoriasJ1 + "  X  " + vitoriasJ2;
        }
    }

    private void AtualizarMoedasUI(int numeroJogador, int quantidadeMoedas)
    {
        if (numeroJogador == 1 && textoMoedasJ1 != null)
        {
            textoMoedasJ1.text = quantidadeMoedas.ToString();
        }
        else if (numeroJogador == 2 && textoMoedasJ2 != null)
        {
            textoMoedasJ2.text = quantidadeMoedas.ToString();
        }
    }

    public void ResetarMoedasUI()
    {
        if (textoMoedasJ1 != null) textoMoedasJ1.text = "0";
        if (textoMoedasJ2 != null) textoMoedasJ2.text = "0";
    }

    private void AtualizarBarrasCooldown(int numeroJogador, float progresso)
    {
        if (numeroJogador == 1 && barraCooldownJ1 != null)
        {
            barraCooldownJ1.value = progresso;
        }
        else if (numeroJogador == 2 && barraCooldownJ2 != null)
        {
            barraCooldownJ2.value = progresso;
        }
    }
}