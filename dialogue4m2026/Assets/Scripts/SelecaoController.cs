using UnityEngine;
using UnityEngine.UI;

public class SelecaoController : MonoBehaviour
{
    [SerializeField] private BolinhaData[] opcoesBolinhas;
    [SerializeField] private Button[] botoesJ1;
    [SerializeField] private Button[] botoesJ2;

    private void MudarCorDoBotao(Button[] listaBotoes, int indiceSelecionado)
    {
        for (int i = 0; i < listaBotoes.Length; i++)
        {
            ColorBlock cb = listaBotoes[i].colors;

            if (i == indiceSelecionado)
            {
                cb.normalColor = Color.yellow;
                cb.selectedColor = Color.yellow;
                cb.highlightedColor = Color.yellow;
                cb.pressedColor = Color.yellow;
            }
            else
            {
                cb.normalColor = Color.white;
                cb.selectedColor = Color.white;
                cb.highlightedColor = Color.white;
                cb.pressedColor = Color.white;
            }

            listaBotoes[i].colors = cb;
        }
    }

    public void SelecionarJogador1(int index)
    {
        if (index >= 0 && index < opcoesBolinhas.Length && GameManager.Instance != null)
        {
            GameManager.Instance.bolinhaEscolhidaJ1 = opcoesBolinhas[index];
            MudarCorDoBotao(botoesJ1, index);
        }
    }

    public void SelecionarJogador2(int index)
    {
        if (index >= 0 && index < opcoesBolinhas.Length && GameManager.Instance != null)
        {
            GameManager.Instance.bolinhaEscolhidaJ2 = opcoesBolinhas[index];
            MudarCorDoBotao(botoesJ2, index);
        }
    }

    public void IniciarJogo()
    {
        if (GameManager.Instance != null && GameManager.Instance.bolinhaEscolhidaJ1 != null && GameManager.Instance.bolinhaEscolhidaJ2 != null)
        {
            GameManager.Instance.CarregarCena("SampleScene");
        }
    }
}