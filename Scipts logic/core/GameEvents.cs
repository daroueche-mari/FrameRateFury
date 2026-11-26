using System;
using UnityEngine;

// Contrat entre la logique et l'UI / les effets visuels.
public static class GameEvents
{
    public static event Action<GameState> StateChanged;
    public static event Action<int> ScoreChanged;
    public static event Action<int, int> HealthChanged;      // (pv actuels, pv max)
    public static event Action<int> WaveChanged;
    public static event Action<Vector2> EnemyKilled;         // position, pour l'explosion
    public static event Action<Vector2> PlayerHit;           // position, pour les étincelles
    public static event Action<int, int> GameOver;           // (score, meilleur score)
    public static event Action GameReset;                    // Recommencer / Retour au menu : tout revient à zéro
    public static event Action<int, int> SpecialChargeChanged;   // (jauge actuelle, jauge max) pour le HUD
    public static event Action<Vector2, float> BombExploded;     // (position, rayon) pour l'explosion
    public static event Action<int> BossSpawned;                 // (pv max) : afficher la barre de vie du boss
    public static event Action<int, int> BossHealthChanged;      // (pv actuels, pv max)
    public static event Action BossDefeated;                     // cacher la barre de vie

    public static void RaiseStateChanged(GameState state)
    {
        StateChanged?.Invoke(state);
    }

    public static void RaiseScoreChanged(int score)
    {
        ScoreChanged?.Invoke(score);
    }

    public static void RaiseHealthChanged(int current, int max)
    {
        HealthChanged?.Invoke(current, max);
    }

    public static void RaiseWaveChanged(int wave)
    {
        WaveChanged?.Invoke(wave);
    }

    public static void RaiseEnemyKilled(Vector2 position)
    {
        EnemyKilled?.Invoke(position);
    }

    public static void RaisePlayerHit(Vector2 position)
    {
        PlayerHit?.Invoke(position);
    }

    public static void RaiseGameOver(int score, int bestScore)
    {
        GameOver?.Invoke(score, bestScore);
    }

    public static void RaiseGameReset()
    {
        GameReset?.Invoke();
    }

    public static void RaiseSpecialChargeChanged(int current, int max)
    {
        SpecialChargeChanged?.Invoke(current, max);
    }

    public static void RaiseBombExploded(Vector2 position, float radius)
    {
        BombExploded?.Invoke(position, radius);
    }

    public static void RaiseBossSpawned(int maxHealth)
    {
        BossSpawned?.Invoke(maxHealth);
    }

    public static void RaiseBossHealthChanged(int current, int max)
    {
        BossHealthChanged?.Invoke(current, max);
    }

    public static void RaiseBossDefeated()
    {
        BossDefeated?.Invoke();
    }
}