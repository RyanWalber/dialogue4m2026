using System;

public static class PlayerObserverManager
{
    public static event Action OnMoedaColetadaNoMapa;
    public static void DispararMoedaColetadaNoMapa() => OnMoedaColetadaNoMapa?.Invoke();

    public static event Action<int> OnMoedaContabilizada;
    public static void NotificarMoedaContabilizada(int totalMoedas) => OnMoedaContabilizada?.Invoke(totalMoedas);
}