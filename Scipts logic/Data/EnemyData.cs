using UnityEngine;

public enum EnemyMovementType
{
    Straight,       // descend en ligne droite
    ZigZag,         // descend en zigzag
    StopAndGo,      // s'arrête en haut de l'écran, puis repart
    Boss            // descend, puis reste en haut en se balançant de gauche à droite
}

// Données d'un type d'ennemi
[CreateAssetMenu(fileName = "EnemyData", menuName = "FrameRateFury/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Prefab (doit avoir le script Enemy)")]
    public Enemy prefab;

    [Header("Stats")]
    public int maxHealth = 1;
    public int scoreValue = 10;
    public float radius = 0.4f;
    public bool isBoss = false;   // un boss ne meurt pas en percutant le joueur

    [Header("Déplacement")]
    public EnemyMovementType movement = EnemyMovementType.Straight;
    public float speed = 3f;
    public float zigZagAmplitude = 1.5f;
    public float zigZagFrequency = 2f;
    public float stopDistanceFromTop = 2.5f;
    public float stopDuration = 3f;

    [Header("Tir")]
    public FirePattern pattern;
    public FirePattern advancedPattern;   // remplace le pattern à partir d'une vague
    public int advancedFromWave = 5;
    public ProjectileData projectile;
    public float fireInterval = 1.5f;
    public bool aimAtPlayer = false;
}