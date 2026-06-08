using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f; 
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private bool cameraRelativeMovement = true;
    [SerializeField] private Transform cameraTransform = null;

    private Vector2 moveInput = Vector2.zero;
    private Rigidbody rb;
    private int moedasColetadas = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Start()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AlocarInput(input);
        }
    }

    void OnEnable()
    {
        PlayerObserverManager.OnMoedaColetadaNoMapa += IncrementarMoeda;
    }

    void OnDisable()
    {
        PlayerObserverManager.OnMoedaColetadaNoMapa -= IncrementarMoeda;
    }

    public void OnMove(InputValue value)
    {
        if (value == null) return;
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        if (rb == null) return;

        Vector3 desired = new Vector3(moveInput.x, 0f, moveInput.y);

        if (cameraRelativeMovement && cameraTransform != null)
        {
            Vector3 camForward = cameraTransform.forward;
            camForward.y = 0f;
            camForward.Normalize();
            
            Vector3 camRight = cameraTransform.right;
            camRight.y = 0f;
            camRight.Normalize();

            desired = camRight * moveInput.x + camForward * moveInput.y;
        }

        if (desired.sqrMagnitude > 1f)
            desired.Normalize();

        Vector3 force = desired * moveSpeed;
        rb.AddForce(force, ForceMode.VelocityChange);

        if (maxSpeed > 0f)
        {
            Vector3 horizontalVel = rb.linearVelocity;
            horizontalVel.y = 0f;
            float speed = horizontalVel.magnitude;
            
            if (speed > maxSpeed)
            {
                Vector3 limited = horizontalVel.normalized * maxSpeed;
                rb.linearVelocity = new Vector3(limited.x, rb.linearVelocity.y, limited.z);
            }
        }
    }

    private void IncrementarMoeda()
    {
        moedasColetadas++;
        PlayerObserverManager.NotificarMoedaContabilizada(moedasColetadas);
    }

    public void SetMoveSpeed(float speed) => moveSpeed = speed;
    public void SetMaxSpeed(float max) => maxSpeed = max;
    public void SetCameraRelative(bool enabled) => cameraRelativeMovement = enabled;
    public void SetCameraTransform(Transform t) => cameraTransform = t;
}