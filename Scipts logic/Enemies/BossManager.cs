using UnityEngine;

// Fait apparaître le boss tous les 10 000 points.
public class BossManager : MonoBehaviour
{
    [SerializeField] private EnemyData bossData;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private int pointsPerBoss = 10000;
    [SerializeField] private bool pauseWavesDuringBoss = true;

    private int nextBossScore;
    private bool bossAlive;
    private bool wavesWerePaused;

    private void OnEnable()
    {
        GameEvents.ScoreChanged += OnScoreChanged;
        GameEvents.BossDefeated += OnBossDefeated;
        GameEvents.GameReset += OnGameReset;
    }

    private void OnDisable()
    {
        GameEvents.ScoreChanged -= OnScoreChanged;
        GameEvents.BossDefeated -= OnBossDefeated;
        GameEvents.GameReset -= OnGameReset;
    }

    private void Awake()
    {
        nextBossScore = pointsPerBoss;
    }

    // Nouvelle partie
    private void OnGameReset()
    {
        bossAlive = false;
        nextBossScore = pointsPerBoss;

        if (wavesWerePaused)
        {
            waveManager.enabled = true;
            wavesWerePaused = false;
        }
    }

    private void OnScoreChanged(int score)
    {
        if (bossAlive || score < nextBossScore)
        {
            return;
        }

        // Prochain boss au multiple de 10 000 suivant
        nextBossScore = (score / pointsPerBoss + 1) * pointsPerBoss;
        SpawnBoss();
    }

    private void SpawnBoss()
    {
        bossAlive = true;

        // On ne relancera les vagues que si c'est nous qui les avons arrêtées
        wavesWerePaused = false;
        if (pauseWavesDuringBoss && waveManager != null && waveManager.enabled)
        {
            waveManager.enabled = false;
            wavesWerePaused = true;
        }

        float centerX = (ScreenBounds.Min.x + ScreenBounds.Max.x) / 2f;
        Vector2 spawnPosition = new Vector2(centerX, ScreenBounds.Max.y + 2f);

        SpawnService.Instance.Enemies.Spawn(bossData, spawnPosition, 1, 1f);
        GameEvents.RaiseBossSpawned(bossData.maxHealth);
    }

    private void OnBossDefeated()
    {
        bossAlive = false;

        if (wavesWerePaused)
        {
            waveManager.enabled = true;
            wavesWerePaused = false;
        }
    }
}