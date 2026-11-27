using UnityEngine;

// Un seul script pour tous les projectiles : leur comportement vient du ProjectileData.
// Le prefab a juste un SpriteRenderer et ce script (pas de Rigidbody2D ni de Collider2D).
[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour, IListed
{
    private const float OffScreenMargin = 1f;

    private SpriteRenderer spriteRenderer;
    private ProjectileData data;
    private IProjectileSpawner spawner;
    private Vector2 position;
    private Vector2 velocity;
    private float lifeTimer;
    private bool fromPlayer;
    private bool isActive;

    public int ListIndex { get; set; }

    public Vector2 Position
    {
        get { return position; }
    }

    public float Radius
    {
        get { return data.radius; }
    }

    public int Damage
    {
        get { return data.damage; }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ListIndex = -1;
    }

    // Appelé par le spawner juste après la création ou la sortie du pool.
    public void Init(ProjectileData newData, Vector2 startPosition, Vector2 direction, bool isFromPlayer, IProjectileSpawner owner)
    {
        data = newData;
        spawner = owner;
        fromPlayer = isFromPlayer;
        position = startPosition;
        velocity = direction.normalized * data.speed;
        lifeTimer = data.maxLifetime;

        if (data.sprite != null)
        {
            spriteRenderer.sprite = data.sprite;
        }
        spriteRenderer.color = data.color;

        transform.position = position;
        transform.localScale = Vector3.one * data.scale;

        // Oriente le sprite dans la direction du tir (le sprite doit pointer vers le haut).
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (fromPlayer)
        {
            EntityRegistry.PlayerProjectiles.Add(this);
        }
        else
        {
            EntityRegistry.EnemyProjectiles.Add(this);
        }

        isActive = true;
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        float deltaTime = Time.deltaTime;

        position += velocity * deltaTime;
        transform.position = position;

        lifeTimer -= deltaTime;

        if (lifeTimer <= 0f || ScreenBounds.IsOutside(position, OffScreenMargin))
        {
            Release();
        }
    }

    // Rend le projectile à son spawner (pool ou Destroy selon la version).
    public void Release()
    {
        if (!isActive)
        {
            return;
        }

        isActive = false;
        Unregister();
        spawner.Release(this);
    }

    private void OnDisable()
    {
        isActive = false;
        Unregister();
    }

    private void Unregister()
    {
        if (fromPlayer)
        {
            EntityRegistry.PlayerProjectiles.Remove(this);
        }
        else
        {
            EntityRegistry.EnemyProjectiles.Remove(this);
        }
    }
}
