using UnityEngine;

// Point d'accès unique aux spawners. La case "Use Optimization" choisit la version :
// décochée = Instantiate / Destroy, cochée = object pooling.
// C'est le seul réglage à changer pour mesurer "avant / après optimisation".
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

    // Appelé quand GameRoot est désactivé (fin de partie, retour au menu) :
    // tous les ennemis et toutes les balles retournent dans les pools.
    // Les objets "Projectiles" et "Enemies" sont en dehors de GameRoot, donc ils sont encore actifs ici.
    private void OnDisable()
    {
        EntityRegistry.ReleaseAll();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
