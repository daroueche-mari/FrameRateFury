using UnityEngine;

// Deux versions existent
public interface IProjectileSpawner
{
    int CreatedCount { get; }

    Projectile Spawn(ProjectileData data, Vector2 position, Vector2 direction, bool fromPlayer);

    void Release(Projectile projectile);
}
