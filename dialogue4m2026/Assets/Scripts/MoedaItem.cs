using UnityEngine;

public class MoedaItem : MonoBehaviour
{
    [SerializeField] private float velocidadeRotacao = 80f;
    public string idUnicoMoeda;

    private void Start()
    {
        if (string.IsNullOrEmpty(idUnicoMoeda))
        {
            idUnicoMoeda = $"{gameObject.name}_{transform.position.x}_{transform.position.y}_{transform.position.z}";
        }

        if (GameManager.Instance != null && GameManager.Instance.dadosAtuais.idsMoedasColetadasNoCheckpoint.Contains(idUnicoMoeda))
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * velocidadeRotacao * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.moedasAtuaisFase++;
            GameManager.Instance.moedasColetadasNestaSessao.Add(idUnicoMoeda);
            GameDataObserverManager.NotificarMoedaColetada(GameManager.Instance.moedasAtuaisFase);

            BolinhaController bola = other.GetComponent<BolinhaController>();
            if (bola != null)
            {
                bola.ColetarMoedaModificadora();
            }

            gameObject.SetActive(false);
        }
    }
}