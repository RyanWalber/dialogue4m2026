using UnityEngine;
using UnityEngine.UI;

public class SeletorBolinha : MonoBehaviour
{
    [Header("Configuração dos Jogadores")]
    [SerializeField] private int numeroJogador;
    [SerializeField] private Button botaoAgil;
    [SerializeField] private Button botaoEquilibrado;
    [SerializeField] private Button botaoVeloz;
    [SerializeField] private Button botaoCanhao;
    [SerializeField] private Button botaoPesada;

    [Header("Dados das Bolinhas (ScriptableObjects)")]
    [SerializeField] private BolinhaData dadosAgil;
    [SerializeField] private BolinhaData dadosEquilibrado;
    [SerializeField] private BolinhaData dadosVeloz;
    [SerializeField] private BolinhaData dadosCanhao;
    [SerializeField] private BolinhaData dadosPesada;

    private Button[] botoes;
    private BolinhaData[] dados;
    private Color colorOriginal = Color.white;

    void Start()
    {
        botoes = new Button[] { botaoAgil, botaoEquilibrado, botaoVeloz, botaoCanhao, botaoPesada };
        dados = new BolinhaData[] { dadosAgil, dadosEquilibrado, dadosVeloz, dadosCanhao, dadosPesada };

        if (botoes[0] != null)
        {
            colorOriginal = botoes[0].image.color;
        }

        for (int i = 0; i < botoes.Length; i++)
        {
            if (botoes[i] != null && dados[i] != null)
            {
                int indice = i;
                botoes[i].onClick.AddListener(() => SelecionarBolinha(indice));
            }
        }
    }

    private void SelecionarBolinha(int indiceSelecionado)
    {
        BolinhaData bolaEscolhida = dados[indiceSelecionado];

        if (GameManager.Instance != null)
        {
            if (numeroJogador == 1)
            {
                GameManager.Instance.bolinhaEscolhidaJ1 = bolaEscolhida;
            }
            else if (numeroJogador == 2)
            {
                GameManager.Instance.bolinhaEscolhidaJ2 = bolaEscolhida;
            }
        }

        for (int i = 0; i < botoes.Length; i++)
        {
            if (botoes[i] != null)
            {
                if (i == indiceSelecionado)
                {
                    Material materialBola = (numeroJogador == 1) ? bolaEscolhida.materialJogador1 : bolaEscolhida.materialJogador2;
                    
                    if (materialBola != null)
                    {
                        botoes[i].image.color = materialBola.color;
                    }
                    else
                    {
                        botoes[i].image.color = Color.green;
                    }
                }
                else
                {
                    botoes[i].image.color = colorOriginal;
                }
            }
        }
    }
}