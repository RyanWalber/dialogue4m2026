using UnityEngine;

public class CameraMultiplayer : MonoBehaviour
{
    private Transform jogador1;
    private Transform jogador2;

    [Header("Configurações de Enquadramento")]
    [SerializeField] private Vector3 proporcaoDirecao = new Vector3(0f, 1.2f, -1f); 
    [SerializeField] private float distanciaMinimaCamera = 12f; 
    [SerializeField] private float multiplicadorAfastamento = 1.1f; 
    [SerializeField] private float suavidade = 0.15f;

    private Vector3 velocidadeCamera;

    void Start()
    {
        BuscarJogadoresNaCena();
    }

    void LateUpdate()
    {
        if (jogador1 == null || jogador2 == null)
        {
            BuscarJogadoresNaCena();
            return;
        }

        Vector3 pontoCentral = (jogador1.position + jogador2.position) / 2f;

        float distanciaEntreEles = Vector3.Distance(jogador1.position, jogador2.position);

        float distanciaFinalCamera = distanciaMinimaCamera + (distanciaEntreEles * multiplicadorAfastamento);

        Vector3 direcaoInclinacao = proporcaoDirecao.normalized;
        Vector3 posicaoAlvo = pontoCentral + (direcaoInclinacao * distanciaFinalCamera);

        transform.position = Vector3.SmoothDamp(transform.position, posicaoAlvo, ref velocidadeCamera, suavidade);

        transform.LookAt(pontoCentral);
    }

    private void BuscarJogadoresNaCena()
    {
        BolinhaController[] jogadores = FindObjectsByType<BolinhaController>(FindObjectsSortMode.None);
        foreach (var jog in jogadores)
        {
            if (jog.numeroJogador == 1) jogador1 = jog.transform;
            if (jog.numeroJogador == 2) jogador2 = jog.transform;
        }
    }
}