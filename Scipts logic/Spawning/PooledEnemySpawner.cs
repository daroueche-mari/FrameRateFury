using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// Version optimisée : un pool par type d'ennemi (chaque type a son propre prefab).
public class PooledEnemySpawner : IEnemySpawner
{
    private Transform parent;
    private int maxSizePerType;
    private Dictionary<EnemyData, ObjectPool<Enemy>> pools = new Dictionary<EnemyData, ObjectPool<Enemy>>();
    private int createdCount;

    public int CreatedCount
    {
        get { return createdCount; }
    }

    public PooledEnemySpawner(Transform enemyParent, int maxSize)
    {
        parent = enemyParent;
        maxSizePerType = maxSize;
    }

    public Enemy Spawn(EnemyData data, Vector2 position, int wave, float fireRateMultiplier)
    {
        Enemy enemy = GetPool(data).Get();
        enemy.Init(data, position, this, wave, fireRateMultiplier);
        return enemy;
    }

    public void Release(Enemy enemy)
    {
        GetPool(enemy.Data).Release(enemy);
    }

    // Crée le pool d'un type d'ennemi la première fois qu'on en a besoin.
    private ObjectPool<Enemy> GetPool(EnemyData data)
    {
        ObjectPool<Enemy> pool;

        if (!pools.TryGetValue(data, out pool))
        {
            pool = new ObjectPool<Enemy>(
                () => CreateEnemy(data),
                enemy => enemy.gameObject.SetActive(true),
                enemy => enemy.gameObject.SetActive(false),
                OnDestroyEnemy,
                false,
                20,
                maxSizePerType);

            pools.Add(data, pool);
        }

        return pool;
    }

    // En quittant le mode Play, les ennemis ont déjà été détruits avec la scène.
    private void OnDestroyEnemy(Enemy enemy)
    {
        if (enemy != null)
        {
            Object.Destroy(enemy.gameObject);
        }
    }

    private Enemy CreateEnemy(EnemyData data)
    {
        Enemy enemy = Object.Instantiate(data.prefab, parent);
        enemy.gameObject.SetActive(false);
        createdCount++;
        return enemy;
    }
}
