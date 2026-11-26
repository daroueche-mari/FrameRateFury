using UnityEngine;

// Pattern Strategy chaque façon de tirer est une classe à part.

public abstract class FirePattern : ScriptableObject
{
    public abstract void Fire(Vector2 origin, Vector2 aimDirection, int shotIndex, ProjectileData projectile, IProjectileSpawner spawner);

    // Tourne un vecteur d'un certain angle 
    protected static Vector2 Rotate(Vector2 vector, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        return new Vector2(vector.x * cos - vector.y * sin, vector.x * sin + vector.y * cos);
    }
}
