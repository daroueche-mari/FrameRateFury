using UnityEngine;

// Version pas opti : chaque tir crée un objet
public class NaiveProjectileSpawner : IProjectileSpawner
{
    private Projectile prefab;
    private Transform parent;
    private int createdCount;

    public int CreatedCount
    {
        get { return createdCount; }
    }

    public NaiveProjectileSpawner(Projectile projectilePrefab, Transform projectileParent)
    {
        prefab = projectilePrefab;
        parent = projectileParent;
    }

    public Projectile Spawn(ProjectileData data, Vector2 position, Vector2 direction, bool fromPlayer)
    {
        Projectile projectile = Object.Instantiate(prefab, position, Quaternion.identity, parent);
        createdCount++;
        projectile.Init(data, position, direction, fromPlayer, this);
        return projectile;
    }

    public void Release(Projectile projectile)
    {
        // Destroy n'agit qu'en fin de frame : on désactive tout de suite
        // pour que la balle ne touche plus rien d'ici là.
        projectile.gameObject.SetActive(false);
        Object.Destroy(projectile.gameObject);
    }
}
