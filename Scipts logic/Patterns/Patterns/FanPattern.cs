using UnityEngine;

// Tir en éventail : plusieurs balles réparties sur un cône centré sur la direction visée.
[CreateAssetMenu(fileName = "FanPattern", menuName = "FrameRateFury/Patterns/Fan")]
public class FanPattern : FirePattern
{
    [SerializeField] private int bulletCount = 5;
    [SerializeField] private float spreadAngle = 60f;

    public override void Fire(Vector2 origin, Vector2 aimDirection, int shotIndex, ProjectileData projectile, IProjectileSpawner spawner)
    {
        if (bulletCount <= 1)
        {
            spawner.Spawn(projectile, origin, aimDirection, false);
            return;
        }

        float step = spreadAngle / (bulletCount - 1);
        float startAngle = -spreadAngle / 2f;

        for (int i = 0; i < bulletCount; i++)
        {
            Vector2 direction = Rotate(aimDirection, startAngle + step * i);
            spawner.Spawn(projectile, origin, direction, false);
        }
    }
}
