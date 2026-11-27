using UnityEngine;

// Tir en ligne : une (ou plusieurs balles côte à côte) dans la direction visée.
[CreateAssetMenu(fileName = "LinePattern", menuName = "FrameRateFury/Patterns/Line")]
public class LinePattern : FirePattern
{
    [SerializeField] private int bulletCount = 1;
    [SerializeField] private float spacing = 0.3f;

    public override void Fire(Vector2 origin, Vector2 aimDirection, int shotIndex, ProjectileData projectile, IProjectileSpawner spawner)
    {
        // Perpendiculaire à la direction, pour placer les balles côte à côte.
        Vector2 side = new Vector2(-aimDirection.y, aimDirection.x);
        float firstOffset = -spacing * (bulletCount - 1) / 2f;

        for (int i = 0; i < bulletCount; i++)
        {
            Vector2 position = origin + side * (firstOffset + spacing * i);
            spawner.Spawn(projectile, position, aimDirection, false);
        }
    }
}
