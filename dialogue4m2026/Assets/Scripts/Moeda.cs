using UnityEngine;

public class Moeda : MonoBehaviour
{
    [Header("Configurações Visuais")]
    [SerializeField] private float velocidadeRotacao = 100f;

    void Update()
    {
        // Faz a moeda ficar girando no próprio eixo para dar um efeito bonito
        transform.Rotate(Vector3.up * velocidadeRotacao * Time.deltaTime);
    }

    // A regra do professor: Usar colisores habilitados com isTrigger
    private void OnTriggerEnter(Collider other)
    {
        // CORRIGIDO: O termo correto da Unity é CompareTag!
        if (other.CompareTag("Player"))
        {
            // Tenta pegar o script PlayerController que está na bola
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                // Avisa o player para computar a moeda e disparar o Observer
                player.AdicionarMoeda();
                
                // Destrói a moeda para ela sumir do mapa
                Destroy(gameObject);
            }
        }
    }
}