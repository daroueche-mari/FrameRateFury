using UnityEngine;

// Même principe que IProjectileSpawner
public interface IEnemySpawner
{
    int CreatedCount { get; }

    Enemy Spawn(EnemyData data, Vector2 position, int wave, float fireRateMultiplier);

    void Release(Enemy enemy);
}
