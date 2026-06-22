using UnityEngine;

public class ControladorSelecao : MonoBehaviour
{
    [Header("Lista de Bolinhas Disponíveis")]
    [SerializeField] private BolinhaData[] todasBolinhas;

    private int indexJ1 = 0;
    private int indexJ2 = 0;

    public void SelecionarBolinhaJ1(int index)
    {
        if (index >= 0 && index < todasBolinhas.Length)
        {
            indexJ1 = index;
            if (GameManager.Instance != null) GameManager.Instance.bolinhaEscolhidaJ1 = todasBolinhas[indexJ1];
        }
    }

    public void SelecionarBolinhaJ2(int index)
    {
        if (index >= 0 && index < todasBolinhas.Length)
        {
            indexJ2 = index;
            if (GameManager.Instance != null) GameManager.Instance.bolinhaEscolhidaJ2 = todasBolinhas[indexJ2];
        }
    }

    public void ConfirmarEIniciarJogo()
    {
        if (GameManager.Instance != null)
        {
            // Garante que uma bolinha padrão esteja selecionada caso não cliquem em nada
            if (GameManager.Instance.bolinhaEscolhidaJ1 == null && todasBolinhas.Length > 0) GameManager.Instance.bolinhaEscolhidaJ1 = todasBolinhas[0];
            if (GameManager.Instance.bolinhaEscolhidaJ2 == null && todasBolinhas.Length > 0) GameManager.Instance.bolinhaEscolhidaJ2 = todasBolinhas[0];

            GameManager.Instance.IniciarNovaPartida();
        }
    }
}