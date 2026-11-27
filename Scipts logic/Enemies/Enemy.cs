using UnityEngine;

// Un seul script pour tous les ennemis : leurs stats, leur déplacement et leur tir viennent du EnemyData.
// Le prefab a juste un SpriteRenderer et ce script.
public class Enemy : MonoBehaviour, IListed
{
    private enum StopPhase
    {
        Arriving,
        Stopped,
        Leaving
    }

    private const float OffScreenMargin = 1.5f;

    private EnemyData data;
    private IEnemySpawner spawner;
    private FirePattern currentPattern;
    private Vector2 position;
    private float startX;
    private int health;
    private float moveTimer;
    private float stopTimer;
    private StopPhase stopPhase;
    private float fireInterval;
    private float fireTimer;
    private int shotIndex;
    private bool isActive;

    public int ListIndex { get; set; }

    public EnemyData Data
    {
        get { return data; }
    }

    public Vector2 Position
    {
        get { return position; }
    }

    public float Radius
    {
        get { return data.radius; }
    }

    public bool IsAlive
    {
        get { return isActive; }
    }

    public bool IsBoss
    {
        get { return data.isBoss; }
    }

    private void Awake()
    {
        ListIndex = -1;
    }

    // Appelé par le spawner juste après la création ou la sortie du pool.
    public void Init(EnemyData newData, Vector2 startPosition, IEnemySpawner owner, int wave, float fireRateMultiplier)
    {
        data = newData;
        spawner = owner;
        position = startPosition;
        startX = startPosition.x;
        health = data.maxHealth;
        moveTimer = 0f;
        stopPhase = StopPhase.Arriving;
        shotIndex = 0;

        if (data.advancedPattern != null && wave >= data.advancedFromWave)
        {
            currentPattern = data.advancedPattern;
        }
        else
        {
            currentPattern = data.pattern;
        }

        fireInterval = data.fireInterval / fireRateMultiplier;
        // Premier tir décalé au hasard, pour que les ennemis ne tirent pas tous en même temps.
        fireTimer = fireInterval * Random.Range(0.5f, 1f);

        transform.position = position;
        EntityRegistry.Enemies.Add(this);
        isActive = true;
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        float deltaTime = Time.deltaTime;

        Move(deltaTime);
        transform.position = position;

        // Sorti par le bas : il disparaît sans donner de points.
        if (position.y < ScreenBounds.Min.y - OffScreenMargin)
        {
            Release();
            return;
        }

        // Il ne tire que lorsqu'il est visible.
        if (position.y < ScreenBounds.Max.y)
        {
            UpdateFire(deltaTime);
        }
    }

    private void Move(float deltaTime)
    {
        switch (data.movement)
        {
            case EnemyMovementType.Straight:
                position.y -= data.speed * deltaTime;
                break;

            case EnemyMovementType.ZigZag:
                moveTimer += deltaTime;
                position.y -= data.speed * deltaTime;
                position.x = startX + Mathf.Sin(moveTimer * data.zigZagFrequency) * data.zigZagAmplitude;
                position.x = Mathf.Clamp(position.x, ScreenBounds.Min.x + data.radius, ScreenBounds.Max.x - data.radius);
                break;

            case EnemyMovementType.StopAndGo:
                MoveStopAndGo(deltaTime);
                break;

            case EnemyMovementType.Boss:
                MoveBoss(deltaTime);
                break;
        }
    }

    // Le boss descend jusqu'à sa place, puis se balance de gauche à droite sans jamais repartir.
    private void MoveBoss(float deltaTime)
    {
        if (stopPhase == StopPhase.Arriving)
        {
            position.y -= data.speed * deltaTime;

            if (position.y <= ScreenBounds.Max.y - data.stopDistanceFromTop)
            {
                stopPhase = StopPhase.Stopped;
                moveTimer = 0f;
            }
        }
        else
        {
            moveTimer += deltaTime;
            float centerX = (ScreenBounds.Min.x + ScreenBounds.Max.x) / 2f;
            position.x = centerX + Mathf.Sin(moveTimer * data.zigZagFrequency) * data.zigZagAmplitude;
        }
    }

    private void MoveStopAndGo(float deltaTime)
    {
        if (stopPhase == StopPhase.Arriving)
        {
            position.y -= data.speed * deltaTime;

            if (position.y <= ScreenBounds.Max.y - data.stopDistanceFromTop)
            {
                stopPhase = StopPhase.Stopped;
                stopTimer = data.stopDuration;
            }
        }
        else if (stopPhase == StopPhase.Stopped)
        {
            stopTimer -= deltaTime;

            if (stopTimer <= 0f)
            {
                stopPhase = StopPhase.Leaving;
            }
        }
        else
        {
            position.y -= data.speed * deltaTime;
        }
    }

    private void UpdateFire(float deltaTime)
    {
        if (currentPattern == null || data.projectile == null)
        {
            return;
        }

        fireTimer -= deltaTime;

        if (fireTimer > 0f)
        {
            return;
        }

        fireTimer = fireInterval;

        Vector2 aimDirection = Vector2.down;

        if (data.aimAtPlayer && PlayerController.Instance != null)
        {
            aimDirection = (PlayerController.Instance.Position - position).normalized;
        }

        currentPattern.Fire(position, aimDirection, shotIndex, data.projectile, SpawnService.Instance.Projectiles);
        shotIndex++;
    }

    public void TakeDamage(int damage)
    {
        if (!isActive)
        {
            return;
        }

        health -= damage;

        if (data.isBoss)
        {
            int shownHealth = health;
            if (shownHealth < 0)
            {
                shownHealth = 0;
            }
            GameEvents.RaiseBossHealthChanged(shownHealth, data.maxHealth);
        }

        if (health <= 0)
        {
            Die(true);
        }
    }

    // givesScore = false quand l'ennemi meurt en percutant le joueur.
    public void Die(bool givesScore)
    {
        if (!isActive)
        {
            return;
        }

        if (givesScore)
        {
            ScoreManager.Instance.AddScore(data.scoreValue);
        }

        GameEvents.RaiseEnemyKilled(position);

        if (data.isBoss)
        {
            GameEvents.RaiseBossDefeated();
        }

        Release();
    }

    // Fait disparaître l'ennemi sans points ni explosion (remise à zéro de la partie).
    public void Despawn()
    {
        Release();
    }

    private void Release()
    {
        if (!isActive)
        {
            return;
        }

        isActive = false;
        EntityRegistry.Enemies.Remove(this);
        spawner.Release(this);
    }

    private void OnDisable()
    {
        isActive = false;
        EntityRegistry.Enemies.Remove(this);
    }
}
