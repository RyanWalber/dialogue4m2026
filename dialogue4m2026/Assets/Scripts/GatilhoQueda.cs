using UnityEngine;

public class GatilhoQueda : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        BolinhaController bola = other.GetComponent<BolinhaController>();
        if (bola != null)
        {
            PlayerObserverManager.NotificarJogadorCaiu(bola.numeroJogador);
        }
    }
}