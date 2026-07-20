using UnityEngine;

public class Moeda : MonoBehaviour
{
    [SerializeField] private float velocidadeRotacao = 80f;

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

            PlayerObserverManager.DispararMoedaColetadaNoMapa();
            
            Destroy(gameObject);
        }
    }
}