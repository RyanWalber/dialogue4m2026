using UnityEngine;
using System.Collections;

public class SplashController : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(TimerSplash());
    }

    IEnumerator TimerSplash()
    {
        yield return new WaitForSeconds(2);
        GameManager.Instance.CarregarCena("MainMenu");
    }
}