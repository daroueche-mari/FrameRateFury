using UnityEngine;

// Point d'accès unique aux spawners.
[DefaultExecutionOrder(-50)]
public class SpawnService : MonoBehaviour
{
    public static SpawnService Instance { get; private set; }

    [SerializeField] private bool useOptimization = true;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private int projectilePrewarmCount = 500;
    [SerializeField] private int projectilePoolMaxSize = 5000;
    [SerializeField] private int enemyPoolMaxSizePerType = 300;

    public IProjectileSpawner Projectiles { get; private set; }
    public IEnemySpawner Enemies { get; private set; }

    public bool UseOptimization
    {
        get { return useOptimization; }
    }

    private void Awake()
    {
        Instance = this;

        Transform projectileParent = new GameObject("Projectiles").transform;
        Transform enemyParent = new GameObject("Enemies").transform;

        if (useOptimization)
        {
            Projectiles = new PooledProjectileSpawner(projectilePrefab, projectileParent, projectilePrewarmCount, projectilePoolMaxSize);
            Enemies = new PooledEnemySpawner(enemyParent, enemyPoolMaxSizePerType);
        }
        else
        {
            Projectiles = new NaiveProjectileSpawner(projectilePrefab, projectileParent);
            Enemies = new NaiveEnemySpawner(enemyParent);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
