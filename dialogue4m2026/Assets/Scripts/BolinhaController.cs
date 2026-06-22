using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class BolinhaController : MonoBehaviour
{
    [Header("Configurações do Jogador")]
    [Range(1, 2)] public int numeroJogador = 1;
    public BolinhaData dadosBolinha;

    [Header("Estatísticas Atuais (Modificadas por Moedas)")]
    private float velocidadeAtual;
    private float forcaEmpurraoAtual;
    private int quantidadeMoedas = 0;

    private Vector2 comandoMovimento = Vector2.zero;
    private Rigidbody rb;
    private SumoInput inputActions;
    private bool podeEmpurrar = true;
    private BolinhaController adversario;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new SumoInput();
    }

    void Start()
    {
        InicializarStatus();
        BuscarAdversario();
    }

    private void InicializarStatus()
    {
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
        BolinhaController[] todasBolas = FindObjectsByType<BolinhaController>(FindObjectsSortMode.None);
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
            inputActions.Player.Empurrar.performed += OnEmpurrarJ1;
        }
        else
        {
            inputActions.Player.Move.performed += OnMoveJ2;
            inputActions.Player.Move.canceled += OnMoveJ2;
            inputActions.Player.Empurrar.performed += OnEmpurrarJ2;
        }
    }

    void OnDisable()
    {
        if (numeroJogador == 1)
        {
            inputActions.Player.Move.performed -= OnMoveJ1;
            inputActions.Player.Move.canceled -= OnMoveJ1;
            inputActions.Player.Empurrar.performed -= OnEmpurrarJ1;
        }
        else
        {
            inputActions.Player.Move.performed -= OnMoveJ2;
            inputActions.Player.Move.canceled -= OnMoveJ2;
            inputActions.Player.Empurrar.performed -= OnEmpurrarJ2;
        }
        inputActions.Disable();
    }

    private void OnMoveJ1(InputAction.CallbackContext context) => comandoMovimento = (numeroJogador == 1) ? context.ReadValue<Vector2>() : comandoMovimento;
    private void OnMoveJ2(InputAction.CallbackContext context) => comandoMovimento = (numeroJogador == 2) ? context.ReadValue<Vector2>() : comandoMovimento;

    private void OnEmpurrarJ1(InputAction.CallbackContext context) { if (numeroJogador == 1) ExecutarEmpurrao(); }
    private void OnEmpurrarJ2(InputAction.CallbackContext context) { if (numeroJogador == 2) ExecutarEmpurrao(); }

    void FixedUpdate()
    {
        Vector3 forcaMovimento = new Vector3(comandoMovimento.x, 0f, comandoMovimento.y) * velocidadeAtual;
        rb.AddForce(forcaMovimento, ForceMode.Acceleration);
    }

    private void ExecutarEmpurrao()
    {
        if (!podeEmpurrar || adversario == null) return;

        float distancia = Vector3.Distance(transform.position, adversario.transform.position);

        Vector3 direcaoOpposta = (adversario.transform.position - transform.position).normalized;
        direcaoOpposta.y = 0f;

        float fatorDistancia = 1f / Mathf.Max(distancia, 0.5f);
        float forcaFinal = forcaEmpurraoAtual * fatorDistancia;

        Rigidbody rbAdversario = adversario.GetComponent<Rigidbody>();
        if (rbAdversario != null)
        {
            rbAdversario.AddForce(direcaoOpposta * forcaFinal, ForceMode.Impulse);
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

        Rigidbody rbProprio = GetComponent<Rigidbody>();
        if (rbProprio != null)
        {
            rbProprio.mass = 1f + (quantidadeMoedas * 0.5f);
        }
    }
}