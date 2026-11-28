using UnityEngine;

// Tir en spirale : un cercle qui tourne un peu plus à chaque tir.
[CreateAssetMenu(fileName = "SpiralPattern", menuName = "FrameRateFury/Patterns/Spiral")]
public class SpiralPattern : FirePattern
{
    [SerializeField] private int bulletCount = 6;
    [SerializeField] private float rotationPerShot = 12f;

    public override void Fire(Vector2 origin, Vector2 aimDirection, int shotIndex, ProjectileData projectile, IProjectileSpawner spawner)
    {
        float step = 360f / bulletCount;
        float offset = shotIndex * rotationPerShot;

        for (int i = 0; i < bulletCount; i++)
        {
            Vector2 direction = Rotate(Vector2.down, offset + step * i);
            spawner.Spawn(projectile, origin, direction, false);
        }
    }
}
