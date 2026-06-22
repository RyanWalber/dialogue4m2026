using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControladorInterfaceSumo : MonoBehaviour
{
    [Header("Placar de Rounds")]
    [SerializeField] private TextMeshProUGUI textoPlacar;

    [Header("Barras de Cooldown (Sliders)")]
    [SerializeField] private Slider barraCooldownJ1;
    [SerializeField] private Slider barraCooldownJ2;

    void OnEnable()
    {
        PlayerObserverManager.OnPlacarAtualizado += AtualizarPlacarUI;
        PlayerObserverManager.OnCooldownAtualizado += AtualizarBarrasCooldown;
    }

    void OnDisable()
    {
        PlayerObserverManager.OnPlacarAtualizado -= AtualizarPlacarUI;
        PlayerObserverManager.OnCooldownAtualizado -= AtualizarBarrasCooldown;
    }

    void Start()
    {
        if (GameManager.Instance != null)
        {
            AtualizarPlacarUI(GameManager.Instance.ObterVitoriasJ1(), GameManager.Instance.ObterVitoriasJ2());
        }

        if (barraCooldownJ1 != null) barraCooldownJ1.value = 1f;
        if (barraCooldownJ2 != null) barraCooldownJ2.value = 1f;
    }

    private void AtualizarPlacarUI(int vitoriasJ1, int vitoriasJ2)
    {
        if (textoPlacar != null)
        {
            textoPlacar.text = vitoriasJ1 + "  X  " + vitoriasJ2;
        }
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