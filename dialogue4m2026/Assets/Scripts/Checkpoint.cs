using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool ativado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!ativado && other.CompareTag("Player"))
        {
            ativado = true;
            Vector3 centroCheckpoint = transform.position;
            GameManager.Instance.RegistrarCheckpoint(centroCheckpoint);
            GameDataObserverManager.NotificarCheckpointAtivado();
        }
    }
}