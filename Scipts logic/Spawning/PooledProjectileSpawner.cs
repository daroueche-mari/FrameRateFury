using UnityEngine;
using UnityEngine.Pool;

// Version optimisée : object pooling. Les balles sont créées une fois, puis réutilisées.
// Au lieu de Destroy, on les désactive et on les range dans le pool.
public class PooledProjectileSpawner : IProjectileSpawner
{
    private Projectile prefab;
    private Transform parent;
    private ObjectPool<Projectile> pool;
    private int createdCount;

    public int CreatedCount
    {
        get { return createdCount; }
    }

    public PooledProjectileSpawner(Projectile projectilePrefab, Transform projectileParent, int prewarmCount, int maxSize)
    {
        prefab = projectilePrefab;
        parent = projectileParent;

        // collectionCheck = false : la vérification anti-double Release coûte cher avec beaucoup d'objets.
        // Le double Release est déjà empêché dans Projectile.Release().
        pool = new ObjectPool<Projectile>(CreateProjectile, OnGet, OnRelease, OnDestroyProjectile, false, prewarmCount, maxSize);

        Prewarm(prewarmCount);
    }

    public Projectile Spawn(ProjectileData data, Vector2 position, Vector2 direction, bool fromPlayer)
    {
        Projectile projectile = pool.Get();
        projectile.Init(data, position, direction, fromPlayer, this);
        return projectile;
    }

    public void Release(Projectile projectile)
    {
        pool.Release(projectile);
    }

    // Crée des balles à l'avance (pendant le chargement) pour ne pas le faire en pleine partie.
    private void Prewarm(int count)
    {
        Projectile[] temp = new Projectile[count];

        for (int i = 0; i < count; i++)
        {
            temp[i] = pool.Get();
        }

        for (int i = 0; i < count; i++)
        {
            pool.Release(temp[i]);
        }
    }

    private Projectile CreateProjectile()
    {
        Projectile projectile = Object.Instantiate(prefab, parent);
        projectile.gameObject.SetActive(false);
        createdCount++;
        return projectile;
    }

    private void OnGet(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }

    private void OnRelease(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private void OnDestroyProjectile(Projectile projectile)
    {
        // En quittant le mode Play, Unity a déjà détruit les balles avec la scène :
        // on vérifie qu'elles existent encore avant d'y toucher.
        if (projectile != null)
        {
            Object.Destroy(projectile.gameObject);
        }
    }
}
