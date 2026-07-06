using UnityEngine;

public class Moeda : MonoBehaviour
{
    [SerializeField] private float velocidadeRotacao = 100f;

    void Update()
    {
        transform.Rotate(Vector3.up * velocidadeRotacao * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        BolinhaController bola = other.GetComponent<BolinhaController>();
        if (bola != null)
        {
            bola.ColetarMoedaModificadora();

            ControladorInterfaceSumo interfaceUI = Object.FindFirstObjectByType<ControladorInterfaceSumo>();
            if (interfaceUI != null)
            {
                if (other.CompareTag("Jogador1"))
                {
                    interfaceUI.RegistrarMoedaDoJogador(1);
                }
                else if (other.CompareTag("Jogador2"))
                {
                    interfaceUI.RegistrarMoedaDoJogador(2);
                }
            }

            PlayerObserverManager.DispararMoedaColetadaNoMapa();
            Destroy(gameObject);
        }
    }
}