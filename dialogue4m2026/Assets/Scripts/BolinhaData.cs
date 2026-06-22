using UnityEngine;

[CreateAssetMenu(fileName = "NovaBolinha", menuName = "Sumo/Bolinha Data")]
public class BolinhaData : ScriptableObject
{
    public string nomeBolinha;
    public float velocidadeInicial;
    public float forcaEmpurraoBase;
    public float tamanho;
    public float tempoCooldown;
    public Material materialJogador1;
    public Material materialJogador2;
}