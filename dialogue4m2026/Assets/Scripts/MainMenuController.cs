using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void BotaoIniciar()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CarregarCena("CenaSelecao");
        }
    }

    public void BotaoSair()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }
}