using UnityEngine;

// À mettre sur le prefab du boss, à côté du script Enemy.
// Enemy gère les PV, le déplacement et la mort ; ce script gère le tir du boss :
// il enchaîne plusieurs patterns, en changeant toutes les quelques secondes.
[RequireComponent(typeof(Enemy))]
public class BossController : MonoBehaviour
{
    [SerializeField] private FirePattern[] patterns;
    [SerializeField] private ProjectileData projectile;
    [SerializeField] private float fireInterval = 0.4f;
    [SerializeField] private float patternDuration = 4f;   // temps passé sur chaque pattern

    private Enemy enemy;
    private int patternIndex;
    private int shotIndex;
    private float fireTimer;
    private float patternTimer;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    // Avec le pooling, le même boss peut revenir : on remet tout à zéro à chaque apparition.
    private void OnEnable()
    {
        patternIndex = 0;
        shotIndex = 0;
        fireTimer = fireInterval;
        patternTimer = 0f;
    }

    private void Update()
    {
        if (!enemy.IsAlive)
        {
            return;
        }

        if (patterns == null || patterns.Length == 0 || projectile == null)
        {
            return;
        }

        // Il ne tire pas tant qu'il n'est pas entré dans l'écran.
        if (enemy.Position.y > ScreenBounds.Max.y - 0.5f)
        {
            return;
        }

        float deltaTime = Time.deltaTime;

        patternTimer += deltaTime;

        if (patternTimer >= patternDuration)
        {
            patternTimer = 0f;
            patternIndex = (patternIndex + 1) % patterns.Length;   // revient au premier après le dernier
            shotIndex = 0;
        }

        fireTimer -= deltaTime;

        if (fireTimer > 0f)
        {
            return;
        }

        fireTimer = fireInterval;

        Vector2 aimDirection = Vector2.down;

        if (PlayerController.Instance != null)
        {
            aimDirection = (PlayerController.Instance.Position - enemy.Position).normalized;
        }

        patterns[patternIndex].Fire(enemy.Position, aimDirection, shotIndex, projectile, SpawnService.Instance.Projectiles);
        shotIndex++;
    }
}
