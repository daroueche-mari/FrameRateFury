using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Scriptable Objects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
   [Header("Apparence")]
    public Sprite sprite;
    public Color color = Color.yellow;
    public float scale = 0.3f;

    [Header("Gameplay")]
    public float speed = 10f;
    public int damage = 1;
    public float radius = 0.1f;          //hitbox
    public float maxLifetime = 6f; 
}
