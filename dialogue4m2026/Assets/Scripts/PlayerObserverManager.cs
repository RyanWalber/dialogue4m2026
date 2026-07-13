using System;

public static class PlayerObserverManager
{
    public static event Action OnMoedaColetadaNoMapa;
    public static void DispararMoedaColetadaNoMapa() => OnMoedaColetadaNoMapa?.Invoke();

    public static event Action<int> OnMoedaContabilizada;
    public static void NotificarMoedaContabilizada(int totalMoedas) => OnMoedaContabilizada?.Invoke(totalMoedas);

    public static event Action<int, int> OnMoedasAtualizadas;
    public static void NotificarMoedasAtualizadas(int numeroJogador, int quantidadeMoedas) => OnMoedasAtualizadas?.Invoke(numeroJogador, quantidadeMoedas);

    public static event Action<int, float> OnCooldownAtualizado;
    public static void NotificarProgressoCooldown(int numeroJogador, float progresso) => OnCooldownAtualizado?.Invoke(numeroJogador, progresso);

    public static event Action<int> OnJogadorCaiu;
    public static void NotificarJogadorCaiu(int numeroJogadorQueCaiu) => OnJogadorCaiu?.Invoke(numeroJogadorQueCaiu);

    public static event Action<int, int> OnPlacarAtualizado;
    public static void NotificarPlacarAtualizado(int vitoriasJ1, int vitoriasJ2) => OnPlacarAtualizado?.Invoke(vitoriasJ1, vitoriasJ2);
}