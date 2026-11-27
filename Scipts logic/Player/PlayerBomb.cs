using UnityEngine;

// La bombe
public class PlayerBomb : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private float fuseTime = 1.2f;          // explose toute seule après ce délai
    [SerializeField] private float contactRadius = 0.4f;     // taille de l'orbe pour toucher un ennemi
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private int damage = 5;

    private Vector2 position;
    private float timer;
    private bool isActive;

    public bool IsActive
    {
        get { return isActive; }
    }

    public void Launch(Vector2 startPosition)
    {
        position = startPosition;
        transform.position = position;
        timer = fuseTime;
        isActive = true;
        gameObject.SetActive(true);
    }

    // Fait disparaître la bombe sans explosion (remise à zéro de la partie).
    public void Cancel()
    {
        isActive = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        float deltaTime = Time.deltaTime;

        position.y += speed * deltaTime;
        transform.position = position;
        timer -= deltaTime;

        if (timer <= 0f || position.y > ScreenBounds.Max.y || TouchesEnemy())
        {
            Explode();
        }
    }

    private bool TouchesEnemy()
    {
        EntityList<Enemy> enemies = EntityRegistry.Enemies;

        for (int i = 0; i < enemies.Count; i++)
        {
            Enemy enemy = enemies[i];
            float radiusSum = contactRadius + enemy.Radius;

            if ((enemy.Position - position).sqrMagnitude <= radiusSum * radiusSum)
            {
                return true;
            }
        }

        return false;
    }

    private void Explode()
    {
        Vector2 center = position;

        // Dégâts aux ennemis dans le rayon.
        // Boucle à l'envers : un ennemi tué est retiré de la liste pendant la boucle.
        EntityList<Enemy> enemies = EntityRegistry.Enemies;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (i >= enemies.Count)
            {
                continue;
            }

            Enemy enemy = enemies[i];
            float reach = explosionRadius + enemy.Radius;

            if ((enemy.Position - center).sqrMagnitude <= reach * reach)
            {
                enemy.TakeDamage(damage);
            }
        }

        // Efface les balles ennemies dans le rayon.
        EntityList<Projectile> enemyProjectiles = EntityRegistry.EnemyProjectiles;

        for (int i = enemyProjectiles.Count - 1; i >= 0; i--)
        {
            Projectile projectile = enemyProjectiles[i];

            if ((projectile.Position - center).sqrMagnitude <= explosionRadius * explosionRadius)
            {
                projectile.Release();
            }
        }

        GameEvents.RaiseBombExploded(center, explosionRadius);

        // isActive passe à false APRÈS les dégâts : les ennemis tués par la bombe
        // ne rechargent pas la jauge (voir PlayerSpecialAttack).
        isActive = false;
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
