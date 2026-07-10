using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class BolinhaController : MonoBehaviour
{
    [Header("Configurações do Jogador")]
    [Range(1, 2)] public int numeroJogador = 1;
    public BolinhaData dadosBolinha;

    private float velocidadeAtual;
    private float forcaEmpurraoAtual;
    private int quantidadeMoedas = 0;

    private Vector2 comandoMovimento = Vector2.zero;
    private Rigidbody rb;
    private SumoInput inputActions;
    private bool podeEmpurrar = true;
    private BolinhaController adversario;
    private float massaInicial;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new SumoInput();
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

    void OnEnable()
    {
        inputActions.Enable();

        if (numeroJogador == 1)
        {
            inputActions.Player.Move.performed += OnMoveJ1;
            inputActions.Player.Move.canceled += OnMoveJ1;
            inputActions.Player.Empurrar.performed += OnEmpurrarPerformedJ1;
        }
        else if (numeroJogador == 2)
        {
            inputActions.Player.Move.performed += OnMoveJ2;
            inputActions.Player.Move.canceled += OnMoveJ2;
            inputActions.Player.Empurrar.performed += OnEmpurrarPerformedJ2;
        }
    }

    void OnDisable()
    {
        if (numeroJogador == 1)
        {
            inputActions.Player.Move.performed -= OnMoveJ1;
            inputActions.Player.Move.canceled -= OnMoveJ1;
            inputActions.Player.Empurrar.performed -= OnEmpurrarPerformedJ1;
        }
        else if (numeroJogador == 2)
        {
            inputActions.Player.Move.performed -= OnMoveJ2;
            inputActions.Player.Move.canceled -= OnMoveJ2;
            inputActions.Player.Empurrar.performed -= OnEmpurrarPerformedJ2;
        }
        inputActions.Disable();
    }

    private void OnMoveJ1(InputAction.CallbackContext context)
    {
        var control = context.control;
        if (control.path.Contains("Arrow") || control.path.Contains("arrow")) return;

        Vector2 input = context.ReadValue<Vector2>();

        if (context.performed)
        {
            comandoMovimento = input;
        }
        else if (context.canceled)
        {
            comandoMovimento = Vector2.zero;
        }
    }

    private void OnMoveJ2(InputAction.CallbackContext context)
    {
        var control = context.control;
        if (!control.path.Contains("Arrow") && !control.path.Contains("arrow")) return;

        Vector2 input = context.ReadValue<Vector2>();

        if (context.performed)
        {
            comandoMovimento = input;
        }
        else if (context.canceled)
        {
            comandoMovimento = Vector2.zero;
        }
    }

    private void OnEmpurrarPerformedJ1(InputAction.CallbackContext context)
    {
        if (context.control.path.ToLower().Contains("space"))
        {
            ExecutarEmpurrao();
        }
    }

    private void OnEmpurrarPerformedJ2(InputAction.CallbackContext context)
    {
        if (context.control.path.ToLower().Contains("enter") || context.control.path.ToLower().Contains("return"))
        {
            ExecutarEmpurrao();
        }
    }

    void FixedUpdate()
    {
        if (comandoMovimento.sqrMagnitude > 0.01f)
        {
            Vector3 forcaMovimento = new Vector3(comandoMovimento.x, 0f, comandoMovimento.y).normalized * velocidadeAtual;
            rb.AddForce(forcaMovimento, ForceMode.Acceleration);
        }
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
        comandoMovimento = Vector2.zero;

        if (dadosBolinha != null)
        {
            velocidadeAtual = dadosBolinha.velocidadeInicial;
            forcaEmpurraoAtual = dadosBolinha.forcaEmpurraoBase;
        }

        PlayerObserverManager.NotificarProgressoCooldown(numeroJogador, 1f);
    }
}