using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Configurações do Professor")]
    public float forcaMovimento = 15f;
    public float velocidadeMaxima = 8f;
    public Transform cameraTransform;

    private Vector2 moveInput;
    private Rigidbody rb;
    private int moedasColetadas = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Se não tiver câmera assinalada, ele acha a Main Camera sozinho
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Start()
    {
        PlayerInput input = GetComponent<PlayerInput>();

        // Regra do professor para o GameManager gerenciar o controle
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AlocarInput(input);
        }
    }

    // ATENÇÃO: Para essa função rodar, a Action no Input System TEM que se chamar "Move"
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log("O teclado enviou: " + moveInput);
    }

    void FixedUpdate()
    {
        // Se não apertou nenhuma tecla, não faz força
        if (moveInput == Vector2.zero) return;

        // Calcula a direção baseada no WASD
        Vector3 direcao = new Vector3(moveInput.x, 0f, moveInput.y);

        // Ajusta a direção para ser relativa à câmera do jogo
        if (cameraTransform != null)
        {
            Vector3 forward = cameraTransform.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = cameraTransform.right;
            right.y = 0f;
            right.Normalize();

            direcao = (right * moveInput.x) + (forward * moveInput.y);
        }

        // Evita que andar na diagonal seja mais rápido
        if (direcao.magnitude > 1f) direcao.Normalize();

        // Empurra a bola
        rb.AddForce(direcao * forcaMovimento, ForceMode.Force);

        // Trava a velocidade para não virar o Sonic
        Vector3 velocidadeAtual = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (velocidadeAtual.magnitude > velocidadeMaxima)
        {
            Vector3 limitada = velocidadeAtual.normalized * velocidadeMaxima;
            rb.linearVelocity = new Vector3(limitada.x, rb.linearVelocity.y, limitada.z);
        }
    }

    // Regra do professor: Contar moedas e avisar o Observer
    public void AdicionarMoeda()
    {
        moedasColetadas++;
        if (PlayerObserverManager.OnMoedaColetada != null)
        {
            PlayerObserverManager.NotificarMoedaColetada(moedasColetadas);
        }
    }
}