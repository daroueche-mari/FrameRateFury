using UnityEngine;

// Vagues infinies : chaque vague fait apparaître plus d'ennemis, choisis au hasard selon des poids.
// Une vague est finie quand tous ses ennemis sont apparus et qu'il n'en reste plus à l'écran.
public class WaveManager : MonoBehaviour
{
    [SerializeField] private WaveSettings settings;
    [SerializeField] private float spawnHeightOffset = 1f;    // apparition au-dessus de l'écran
    [SerializeField] private float spawnSideMargin = 0.6f;

    private int currentWave;
    private int enemiesLeftToSpawn;
    private float spawnTimer;
    private bool waitingForNextWave;
    private float nextWaveTimer;

    public int CurrentWave
    {
        get { return currentWave; }
    }

    private bool paused;   // true pendant le combat contre le boss

    // Appelé à chaque fois que GameRoot est activé : nouvelle partie, retour à la vague 1.
    private void OnEnable()
    {
        paused = false;
        SpawnStats.Reset();
        StartWave(1);
    }

    // Le BossManager met les vagues en pause pendant le boss, puis les relance.
    public void SetPaused(bool value)
    {
        paused = value;
    }

    private void Update()
    {
        if (paused)
        {
            return;
        }

        float deltaTime = Time.deltaTime;

        if (waitingForNextWave)
        {
            nextWaveTimer -= deltaTime;

            if (nextWaveTimer <= 0f)
            {
                StartWave(currentWave + 1);
            }
            return;
        }

        if (enemiesLeftToSpawn > 0)
        {
            spawnTimer -= deltaTime;

            if (spawnTimer <= 0f)
            {
                SpawnEnemy();
                enemiesLeftToSpawn--;
                spawnTimer = settings.spawnInterval;
            }
            return;
        }

        if (EntityRegistry.Enemies.Count == 0)
        {
            EndWave();
        }
    }

    private void StartWave(int wave)
    {
        currentWave = wave;
        enemiesLeftToSpawn = settings.GetEnemyCount(wave);
        spawnTimer = 0f;
        waitingForNextWave = false;

        GameEvents.RaiseWaveChanged(currentWave);
    }

    private void EndWave()
    {
        ScoreManager.Instance.AddScore(settings.waveBonusPerWave * currentWave);
        waitingForNextWave = true;
        nextWaveTimer = settings.delayBetweenWaves;
    }

    private void SpawnEnemy()
    {
        EnemyData enemy = PickEnemy();

        if (enemy == null)
        {
            return;
        }

        SpawnStats.Record(currentWave, enemy.name);

        float x = Random.Range(ScreenBounds.Min.x + spawnSideMargin, ScreenBounds.Max.x - spawnSideMargin);
        float y = ScreenBounds.Max.y + spawnHeightOffset;
        float fireRateMultiplier = 1f + settings.fireRateBonusPerWave * (currentWave - 1);

        SpawnService.Instance.Enemies.Spawn(enemy, new Vector2(x, y), currentWave, fireRateMultiplier);
    }

    // Tirage pondéré : plus le poids d'un ennemi est grand, plus il a de chances d'être choisi.
    private EnemyData PickEnemy()
    {
        float total = 0f;

        foreach (WaveSettings.EnemyWeight entry in settings.enemies)
        {
            total += entry.GetWeight(currentWave);
        }

        if (total <= 0f)
        {
            if (settings.enemies.Count > 0)
            {
                return settings.enemies[0].enemy;
            }
            return null;
        }

        float roll = Random.Range(0f, total);

        foreach (WaveSettings.EnemyWeight entry in settings.enemies)
        {
            roll -= entry.GetWeight(currentWave);

            if (roll <= 0f)
            {
                return entry.enemy;
            }
        }

        return settings.enemies[settings.enemies.Count - 1].enemy;
    }
}
