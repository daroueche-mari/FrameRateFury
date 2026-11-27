using UnityEngine;

// Fait apparaître le boss tous les 10 000 points.
// Pendant le combat contre le boss, les vagues normales sont mises en pause.
public class BossManager : MonoBehaviour
{
    [SerializeField] private EnemyData bossData;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private int pointsPerBoss = 10000;
    [SerializeField] private bool pauseWavesDuringBoss = true;

    private int nextBossScore;
    private bool bossAlive;

    // Appelé à chaque fois que GameRoot est activé : nouvelle partie, prochain boss à 10 000.
    private void OnEnable()
    {
        GameEvents.ScoreChanged += OnScoreChanged;
        GameEvents.BossDefeated += OnBossDefeated;

        bossAlive = false;
        nextBossScore = pointsPerBoss;
    }

    private void OnDisable()
    {
        GameEvents.ScoreChanged -= OnScoreChanged;
        GameEvents.BossDefeated -= OnBossDefeated;
    }

    private void OnScoreChanged(int score)
    {
        if (bossAlive || score < nextBossScore)
        {
            return;
        }

        // Prochain boss au multiple de 10 000 suivant (20 000, 30 000...).
        nextBossScore = (score / pointsPerBoss + 1) * pointsPerBoss;
        SpawnBoss();
    }

    private void SpawnBoss()
    {
        bossAlive = true;

        if (pauseWavesDuringBoss && waveManager != null)
        {
            waveManager.SetPaused(true);
        }

        float centerX = (ScreenBounds.Min.x + ScreenBounds.Max.x) / 2f;
        Vector2 spawnPosition = new Vector2(centerX, ScreenBounds.Max.y + 2f);

        SpawnService.Instance.Enemies.Spawn(bossData, spawnPosition, 1, 1f);
        GameEvents.RaiseBossSpawned(bossData.maxHealth);
    }

    private void OnBossDefeated()
    {
        bossAlive = false;

        if (waveManager != null)
        {
            waveManager.SetPaused(false);
        }
    }
}
