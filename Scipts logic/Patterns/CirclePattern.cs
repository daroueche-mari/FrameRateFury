using UnityEngine;

// Tir en cercle : des balles dans toutes les directions, régulièrement espacées.
[CreateAssetMenu(fileName = "CirclePattern", menuName = "FrameRateFury/Patterns/Circle")]
public class CirclePattern : FirePattern
{
    [SerializeField] private int bulletCount = 12;

    public override void Fire(Vector2 origin, Vector2 aimDirection, int shotIndex, ProjectileData projectile, IProjectileSpawner spawner)
    {
        float step = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            Vector2 direction = Rotate(Vector2.down, step * i);
            spawner.Spawn(projectile, origin, direction, false);
        }
    }
}
