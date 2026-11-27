using UnityEngine;

// Gère toutes les collisions du jeu avec de simples tests de distance entre cercles.
// On n'utilise pas le moteur physique (pas de Rigidbody2D ni de Collider2D) :
// avec des centaines de balles, c'est beaucoup moins coûteux.
public class CollisionSystem : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private int contactDamage = 1;

    private void LateUpdate()
    {
        CheckPlayerProjectiles();
        CheckEnemyProjectiles();
        CheckEnemyContacts();
    }

    // Balles du joueur contre les ennemis.
    private void CheckPlayerProjectiles()
    {
        EntityList<Projectile> projectiles = EntityRegistry.PlayerProjectiles;
        EntityList<Enemy> enemies = EntityRegistry.Enemies;

        // On parcourt à l'envers : quand une balle est retirée, elle est remplacée
        // par la dernière de la liste, qui a déjà été vérifiée.
        for (int i = projectiles.Count - 1; i >= 0; i--)
        {
            if (i >= projectiles.Count)
            {
                continue;
            }

            Projectile projectile = projectiles[i];

            for (int j = enemies.Count - 1; j >= 0; j--)
            {
                Enemy enemy = enemies[j];

                if (Overlap(projectile.Position, projectile.Radius, enemy.Position, enemy.Radius))
                {
                    int damage = projectile.Damage;
                    projectile.Release();
                    enemy.TakeDamage(damage);
                    break;
                }
            }
        }
    }

    // Balles ennemies contre le joueur.
    private void CheckEnemyProjectiles()
    {
        if (!CanPlayerBeHit())
        {
            return;
        }

        EntityList<Projectile> projectiles = EntityRegistry.EnemyProjectiles;
        Vector2 playerPosition = PlayerController.Instance.Position;
        float playerRadius = PlayerController.Instance.HitboxRadius;

        for (int i = projectiles.Count - 1; i >= 0; i--)
        {
            Projectile projectile = projectiles[i];

            if (Overlap(projectile.Position, projectile.Radius, playerPosition, playerRadius))
            {
                int damage = projectile.Damage;
                projectile.Release();
                playerHealth.TakeDamage(damage);

                // Le joueur devient invincible après un coup : inutile de continuer.
                return;
            }
        }
    }

    // Ennemis qui foncent dans le joueur.
    private void CheckEnemyContacts()
    {
        if (!CanPlayerBeHit())
        {
            return;
        }

        EntityList<Enemy> enemies = EntityRegistry.Enemies;
        Vector2 playerPosition = PlayerController.Instance.Position;
        float playerRadius = PlayerController.Instance.HitboxRadius;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            Enemy enemy = enemies[i];

            if (Overlap(enemy.Position, enemy.Radius, playerPosition, playerRadius))
            {
                // Un ennemi normal explose en percutant le joueur. Le boss, lui, reste.
                if (!enemy.IsBoss)
                {
                    enemy.Die(false);
                }

                playerHealth.TakeDamage(contactDamage);
                return;
            }
        }
    }

    private bool CanPlayerBeHit()
    {
        if (PlayerController.Instance == null || playerHealth == null)
        {
            return false;
        }

        if (playerHealth.IsDead || playerHealth.IsInvincible)
        {
            return false;
        }

        return true;
    }

    // Deux cercles se touchent si la distance entre leurs centres est plus petite
    // que la somme de leurs rayons. On compare les carrés pour éviter une racine carrée.
    private static bool Overlap(Vector2 a, float radiusA, Vector2 b, float radiusB)
    {
        float radiusSum = radiusA + radiusB;
        return (a - b).sqrMagnitude <= radiusSum * radiusSum;
    }
}
