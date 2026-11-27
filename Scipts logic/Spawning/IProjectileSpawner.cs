using UnityEngine;

// Deux versions existent : NaiveProjectileSpawner (Instantiate / Destroy) et PooledProjectileSpawner (object pooling).
// Le reste du code ne sait pas laquelle tourne : c'est ce qui permet de comparer les deux.
public interface IProjectileSpawner
{
    int CreatedCount { get; }

    Projectile Spawn(ProjectileData data, Vector2 position, Vector2 direction, bool fromPlayer);

    void Release(Projectile projectile);
}
