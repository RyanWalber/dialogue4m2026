using UnityEngine;
using System.Collections;

public class GeradorMoedas : MonoBehaviour
{
    [Header("Configura��es do Prefab")]
    [SerializeField] private GameObject prefabMoeda;

    [Header("Configura��es de Tempo")]
    [SerializeField] private float intervaloSpawn = 4f;

    [Header("Limites da Arena (�rea de Spawn)")]
    [SerializeField] private float limiteX = 12f;
    [SerializeField] private float limiteZ = 12f;
    [SerializeField] private float alturaY = 1.5f;

    private bool gerando = true;

    void Start()
    {
        StartCoroutine(RotinaSpawnMoedas());
    }

    private IEnumerator RotinaSpawnMoedas()
    {
        while (gerando)
        {
            yield return new WaitForSeconds(intervaloSpawn);
            SpawnarMoeda();
        }
    }

    private void SpawnarMoeda()
    {
        if (prefabMoeda == null) return;

        float posX = Random.Range(-limiteX, limiteX);
        float posZ = Random.Range(-limiteZ, limiteZ);
        Vector3 posicaoAleatoria = new Vector3(posX, alturaY, posZ);

        Instantiate(prefabMoeda, posicaoAleatoria, Quaternion.identity);
    }

    public void PararGeracao() => gerando = false;
    public void IniciarGeracao() { gerando = true; StartCoroutine(RotinaSpawnMoedas()); }
}