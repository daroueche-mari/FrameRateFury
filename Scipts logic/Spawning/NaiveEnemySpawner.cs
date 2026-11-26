using UnityEngine;

// Version NON optimisée : Instantiate à chaque apparition, Destroy à chaque mort.
public class NaiveEnemySpawner : IEnemySpawner
{
    private Transform parent;
    private int createdCount;

    public int CreatedCount
    {
        get { return createdCount; }
    }

    public NaiveEnemySpawner(Transform enemyParent)
    {
        parent = enemyParent;
    }

    public Enemy Spawn(EnemyData data, Vector2 position, int wave, float fireRateMultiplier)
    {
        Enemy enemy = Object.Instantiate(data.prefab, position, Quaternion.identity, parent);
        createdCount++;
        enemy.Init(data, position, this, wave, fireRateMultiplier);
        return enemy;
    }

    public void Release(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        Object.Destroy(enemy.gameObject);
    }
}
