using UnityEngine;

// Tir automatique et continu du joueur, droit vers le haut.
public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private ProjectileData projectile;
    [SerializeField] private float fireInterval = 0.12f;
    [SerializeField] private Vector2 muzzleOffset = new Vector2(0f, 0.5f);

    private float fireTimer;

    // Nouvelle partie : premier tir immédiat.
    private void OnEnable()
    {
        ResetShooter();
    }

    public void ResetShooter()
    {
        fireTimer = 0f;
    }

    // En pause, Time.deltaTime = 0 : le timer n'avance plus, donc plus de tir.
    private void Update()
    {
        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            fireTimer = fireInterval;

            Vector2 origin = (Vector2)transform.position + muzzleOffset;
            SpawnService.Instance.Projectiles.Spawn(projectile, origin, Vector2.up, true);
        }
    }
}