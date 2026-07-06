using UnityEngine;
using UnityEngine.SceneManagement;

public class SelecaoController : MonoBehaviour
{
    [SerializeField] private BolinhaData[] opcoesBolinhas;

    public void SelecionarJogador1(int index)
    {
        if (index >= 0 && index < opcoesBolinhas.Length)
        {
            DadosSelecao.BolinhaJogador1 = opcoesBolinhas[index];
        }
    }

    public void SelecionarJogador2(int index)
    {
        if (index >= 0 && index < opcoesBolinhas.Length)
        {
            DadosSelecao.BolinhaJogador2 = opcoesBolinhas[index];
        }
    }

    public void IniciarJogo()
    {
        if (DadosSelecao.BolinhaJogador1 != null && DadosSelecao.BolinhaJogador2 != null)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}