using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Configura��es do Professor")]
    public float forcaMovimento = 15f;
    public float velocidadeMaxima = 8f;
    public Transform cameraTransform;

    private Vector2 moveInput;
    private Rigidbody rb;
    private int moedasColetadas = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Start()
    {
        PlayerInput input = GetComponent<PlayerInput>();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AlocarInput(input);
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log("O teclado enviou: " + moveInput);
    }

    void FixedUpdate()
    {
        if (moveInput == Vector2.zero) return;

        Vector3 direcao = new Vector3(moveInput.x, 0f, moveInput.y);

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

        if (direcao.magnitude > 1f) direcao.Normalize();

        rb.AddForce(direcao * forcaMovimento, ForceMode.Force);

        Vector3 velocidadeAtual = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (velocidadeAtual.magnitude > velocidadeMaxima)
        {
            Vector3 limitada = velocidadeAtual.normalized * velocidadeMaxima;
            rb.linearVelocity = new Vector3(limitada.x, rb.linearVelocity.y, limitada.z);
        }
    }

    public void AdicionarMoeda()
    {
        moedasColetadas++;
        if (PlayerObserverManager.OnMoedaColetada != null)
        {
            PlayerObserverManager.NotificarMoedaColetada(moedasColetadas);
        }
    }
}