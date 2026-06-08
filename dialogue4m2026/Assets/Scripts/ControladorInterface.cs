using UnityEngine;
using TMPro;

public class ControladorInterface : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI campoTextoMoedas;

    void OnEnable()
    {
        PlayerObserverManager.OnMoedaContabilizada += AtualizarTexto;
    }

    void OnDisable()
    {
        PlayerObserverManager.OnMoedaContabilizada -= AtualizarTexto;
    }

    private void AtualizarTexto(int quantidadeDeMoedas)
    {
        if (campoTextoMoedas != null)
        {
            campoTextoMoedas.text = "Moedas: " + quantidadeDeMoedas;
        }
    }
}