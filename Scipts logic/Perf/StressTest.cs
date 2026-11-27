using UnityEngine;

// Protocole de mesure : fait apparaître de plus en plus d'ennemis jusqu'à ce que les FPS s'effondrent.
// Le joueur est invincible et les vagues normales sont coupées.
// À la fin, le résultat est écrit dans la Console (et dans le CSV du PerfLogger).
public class StressTest : MonoBehaviour
{
    [SerializeField] private bool active = false;
    [SerializeField] private EnemyData enemyToSpawn;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Montée en charge")]
    [SerializeField] private float startSpawnsPerSecond = 1f;
    [SerializeField] private float spawnsPerSecondIncrease = 1f;
    [SerializeField] private float stepDuration = 5f;

    [Header("Arrêt")]
    [SerializeField] private float stopBelowFps = 20f;
    [SerializeField] private float stopAfterSeconds = 3f;

    private float elapsed;
    private float spawnAccumulator;
    private float lowFpsTimer;
    private bool finished;
    private int maxEntitiesAt60;
    private int maxEntitiesAt30;

    public bool IsRunning
    {
        get { return active && !finished; }
    }

    private void Start()
    {
        if (!active)
        {
            enabled = false;
            return;
        }

        waveManager.enabled = false;
        playerHealth.SetGodMode(true);
    }

    private void Update()
    {
        if (finished)
        {
            return;
        }

        float deltaTime = Time.deltaTime;
        elapsed += deltaTime;

        SpawnEnemies(deltaTime);
        TrackResults(deltaTime);
    }

    private void SpawnEnemies(float deltaTime)
    {
        int step = Mathf.FloorToInt(elapsed / stepDuration);
        float spawnsPerSecond = startSpawnsPerSecond + spawnsPerSecondIncrease * step;

        spawnAccumulator += spawnsPerSecond * deltaTime;

        while (spawnAccumulator >= 1f)
        {
            spawnAccumulator -= 1f;

            float x = Random.Range(ScreenBounds.Min.x + 0.5f, ScreenBounds.Max.x - 0.5f);
            Vector2 position = new Vector2(x, ScreenBounds.Max.y + 1f);
            SpawnService.Instance.Enemies.Spawn(enemyToSpawn, position, 1, 1f);
        }
    }

    private void TrackResults(float deltaTime)
    {
        float fps = FpsCounter.CurrentFps;
        int entities = EntityRegistry.ActiveCount;

        // 55 au lieu de 60 : la moyenne de FPS oscille toujours un peu sous la cible.
        if (fps >= 55f && entities > maxEntitiesAt60)
        {
            maxEntitiesAt60 = entities;
        }

        if (fps >= 30f && entities > maxEntitiesAt30)
        {
            maxEntitiesAt30 = entities;
        }

        if (fps > 0f && fps < stopBelowFps)
        {
            lowFpsTimer += deltaTime;
        }
        else
        {
            lowFpsTimer = 0f;
        }

        if (lowFpsTimer >= stopAfterSeconds)
        {
            Finish();
        }
    }

    private void Finish()
    {
        finished = true;

        string mode = "naive";
        if (SpawnService.Instance.UseOptimization)
        {
            mode = "pooling";
        }

        Debug.Log("[StressTest] Mode : " + mode
            + " | Max entités à ~60 FPS : " + maxEntitiesAt60
            + " | Max entités à 30 FPS : " + maxEntitiesAt30
            + " | Durée : " + elapsed.ToString("F1") + " s"
            + " | Objets créés (projectiles) : " + SpawnService.Instance.Projectiles.CreatedCount);
    }
}
