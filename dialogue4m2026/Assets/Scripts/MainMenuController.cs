using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void BotaoIniciar()
    {
        GameManager.Instance.CarregarCena("SampleScene");
    }

    public void BotaoSair()
    {
        Application.Quit();
    }
}