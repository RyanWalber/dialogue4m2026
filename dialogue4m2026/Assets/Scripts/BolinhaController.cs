using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class BolinhaController : MonoBehaviour
{
    [Header("Configurações do Jogador")]
    [Range(1, 2)] public int numeroJogador = 1;
    public BolinhaData dadosBolinha;

    [Header("Estatísticas Atuais")]
    private float velocidadeAtual;
    private float forcaEmpurraoAtual;
    private int quantidadeMoedas = 0;

    private Vector2 comandoMovimento = Vector2.zero;
    private Rigidbody rb;
    private bool podeEmpurrar = true;
    private BolinhaController adversario;
    private float massaInicial;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        massaInicial = rb.mass;
        InitializeStatus();
        BuscarAdversario();
    }

    private void InitializeStatus()
    {
        if (GameManager.Instance != null)
        {
            if (numeroJogador == 1 && GameManager.Instance.bolinhaEscolhidaJ1 != null)
            {
                dadosBolinha = GameManager.Instance.bolinhaEscolhidaJ1;
            }
            else if (numeroJogador == 2 && GameManager.Instance.bolinhaEscolhidaJ2 != null)
            {
                dadosBolinha = GameManager.Instance.bolinhaEscolhidaJ2;
            }
        }

        if (dadosBolinha != null)
        {
            velocidadeAtual = dadosBolinha.velocidadeInicial;
            forcaEmpurraoAtual = dadosBolinha.forcaEmpurraoBase;
            transform.localScale = Vector3.one * dadosBolinha.tamanho;

            Renderer render = GetComponent<Renderer>();
            if (render != null)
            {
                render.material = (numeroJogador == 1) ? dadosBolinha.materialJogador1 : dadosBolinha.materialJogador2;
            }
        }
    }

    private void BuscarAdversario()
    {
        BolinhaController[] todasBolas = Object.FindObjectsByType<BolinhaController>(FindObjectsSortMode.None);
        foreach (var bola in todasBolas)
        {
            if (bola != this)
            {
                adversario = bola;
                break;
            }
        }
    }

    void Update()
    {
        var teclado = Keyboard.current;
        if (teclado == null) return;

        float x = 0f;
        float y = 0f;

        if (numeroJogador == 1)
        {
            if (teclado.wKey.isPressed) y = 1f;
            if (teclado.sKey.isPressed) y = -1f;
            if (teclado.aKey.isPressed) x = -1f;
            if (teclado.dKey.isPressed) x = 1f;

            comandoMovimento = new Vector2(x, y).normalized;

            if (teclado.spaceKey.wasPressedThisFrame)
            {
                ExecutarEmpurrao();
            }
        }
        else if (numeroJogador == 2)
        {
            if (teclado.upArrowKey.isPressed) y = 1f;
            if (teclado.downArrowKey.isPressed) y = -1f;
            if (teclado.leftArrowKey.isPressed) x = -1f;
            if (teclado.rightArrowKey.isPressed) x = 1f;

            comandoMovimento = new Vector2(x, y).normalized;

            if (teclado.enterKey.wasPressedThisFrame || teclado.numpadEnterKey.wasPressedThisFrame)
            {
                ExecutarEmpurrao();
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 forcaMovimento = new Vector3(comandoMovimento.x, 0f, comandoMovimento.y) * velocidadeAtual;
        rb.AddForce(forcaMovimento, ForceMode.Acceleration);
    }

    private void ExecutarEmpurrao()
    {
        if (!podeEmpurrar || adversario == null) return;

        float distancia = Vector3.Distance(transform.position, adversario.transform.position);

        Vector3 direcaoEmpurrao = (adversario.transform.position - transform.position).normalized;
        direcaoEmpurrao.y = 0f;

        float fatorDistancia = 1f / Mathf.Max(distancia, 0.5f);
        float forcaFinal = forcaEmpurraoAtual * fatorDistancia;

        Rigidbody rbAdversario = adversario.GetComponent<Rigidbody>();
        if (rbAdversario != null)
        {
            rbAdversario.AddForce(direcaoEmpurrao * forcaFinal, ForceMode.Impulse);
        }

        StartCoroutine(RotinaCooldown());
    }

    private IEnumerator RotinaCooldown()
    {
        podeEmpurrar = false;
        float tempoTotal = dadosBolinha != null ? dadosBolinha.tempoCooldown : 2f;

        float decorrido = 0f;
        while (decorrido < tempoTotal)
        {
            decorrido += Time.deltaTime;
            float progresso = decorrido / tempoTotal;
            PlayerObserverManager.NotificarProgressoCooldown(numeroJogador, progresso);
            yield return null;
        }

        podeEmpurrar = true;
        PlayerObserverManager.NotificarProgressoCooldown(numeroJogador, 1f);
    }

    public void ColetarMoedaModificadora()
    {
        quantidadeMoedas++;
        velocidadeAtual = Mathf.Max(dadosBolinha.velocidadeInicial - (quantidadeMoedas * 0.5f), 3f);
        forcaEmpurraoAtual = dadosBolinha.forcaEmpurraoBase + (quantidadeMoedas * 3f);

        if (rb != null)
        {
            rb.mass = massaInicial + (quantidadeMoedas * 0.5f);
        }
    }

    public void ResetarBolinha(Vector3 posicaoSpawn)
    {
        StopAllCoroutines();
        transform.position = posicaoSpawn;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.mass = massaInicial;
        }

        quantidadeMoedas = 0;
        podeEmpurrar = true;

        if (dadosBolinha != null)
        {
            velocidadeAtual = dadosBolinha.velocidadeInicial;
            forcaEmpurraoAtual = dadosBolinha.forcaEmpurraoBase;
        }

        PlayerObserverManager.NotificarProgressoCooldown(numeroJogador, 1f);
    }
}