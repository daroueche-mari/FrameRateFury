using UnityEngine;

// Données d'un type de projectile. Clic droit > Create > FrameRateFury > Projectile Data.
[CreateAssetMenu(fileName = "ProjectileData", menuName = "FrameRateFury/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    [Header("Apparence")]
    public Sprite sprite;
    public Color color = Color.white;
    public float scale = 0.3f;

    [Header("Gameplay")]
    public float speed = 10f;
    public int damage = 1;
    public float radius = 0.1f;          // rayon de la hitbox
    public float maxLifetime = 6f;       // sécurité si la balle ne sort jamais de l'écran
}
