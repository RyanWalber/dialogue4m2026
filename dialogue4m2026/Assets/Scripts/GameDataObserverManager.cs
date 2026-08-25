using System;

public static class GameDataObserverManager
{
    public static event Action<int> OnMoedaColetada;
    public static void NotificarMoedaColetada(int total) => OnMoedaColetada?.Invoke(total);

    public static event Action OnCheckpointAtivado;
    public static void NotificarCheckpointAtivado() => OnCheckpointAtivado?.Invoke();

    public static event Action OnJogoPausado;
    public static void NotificarJogoPausado() => OnJogoPausado?.Invoke();
}