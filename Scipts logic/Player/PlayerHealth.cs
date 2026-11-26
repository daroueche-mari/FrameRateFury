using UnityEngine;

// Points de vie du joueur et invincibilité temporaire après un coup.
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invincibilityDuration = 1.5f;
    [SerializeField] private float blinkInterval = 0.1f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private int currentHealth;
    private float invincibilityTimer;
    private bool godMode;   // utilisé par le StressTest : le joueur ne peut pas mourir

    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    public int MaxHealth
    {
        get { return maxHealth; }
    }

    public bool IsInvincible
    {
        get { return invincibilityTimer > 0f || godMode; }
    }

    public bool IsDead
    {
        get { return currentHealth <= 0; }
    }

    private void Awake()
    {
        currentHealth = maxHealth;
        GameEvents.GameReset += OnGameReset;
    }

    private void OnDestroy()
    {
        GameEvents.GameReset -= OnGameReset;
    }

    private void Start()
    {
        GameEvents.RaiseHealthChanged(currentHealth, maxHealth);
    }

    // Nouvelle partie : PV au maximum, plus d'invincibilité, sprite visible.
    private void OnGameReset()
    {
        currentHealth = maxHealth;
        invincibilityTimer = 0f;

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        GameEvents.RaiseHealthChanged(currentHealth, maxHealth);
    }

    private void Update()
    {
        if (invincibilityTimer <= 0f)
        {
            return;
        }

        invincibilityTimer -= Time.deltaTime;

        if (spriteRenderer != null)
        {
            if (invincibilityTimer <= 0f)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                // Clignotement : visible une période sur deux.
                spriteRenderer.enabled = Mathf.Repeat(invincibilityTimer, blinkInterval * 2f) > blinkInterval;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (IsDead || IsInvincible)
        {
            return;
        }

        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        GameEvents.RaiseHealthChanged(currentHealth, maxHealth);
        GameEvents.RaisePlayerHit(transform.position);

        if (IsDead)
        {
            GameManager.Instance.TriggerGameOver();
        }
        else
        {
            invincibilityTimer = invincibilityDuration;
        }
    }

    public void SetGodMode(bool enabled)
    {
        godMode = enabled;
    }
}