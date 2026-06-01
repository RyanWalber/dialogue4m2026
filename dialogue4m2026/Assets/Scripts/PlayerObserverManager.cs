using System;

public static class PlayerObserverManager
{
    public static Action<int> OnMoedaColetada;

    public static void NotificarMoedaColetada(int quantidade)
    {
        OnMoedaColetada?.Invoke(quantidade);
    }
}