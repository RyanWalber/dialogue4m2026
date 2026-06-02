using UnityEngine;
using TMPro;

public class ControladorInterface : MonoBehaviour
{
    [Header("Componente de Interface")]
    [SerializeField] private TextMeshProUGUI campoTextoMoedas;

    void OnEnable()
    {
        PlayerObserverManager.OnMoedaColetada += AtualizarTexto;
    }

    void OnDisable()
    {
        PlayerObserverManager.OnMoedaColetada -= AtualizarTexto;
    }

    private void AtualizarTexto(int quantidadeDeMoedas)
    {
        if (campoTextoMoedas != null)
        {
            campoTextoMoedas.text = "Moedas: " + quantidadeDeMoedas;
        }
    }
}