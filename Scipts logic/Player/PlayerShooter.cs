using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
      [SerializeField] private ProjectileData projectile;
    [SerializeField] private float fireInterval = 0.12f;
    [SerializeField] private Vector2 muzzleOffset = new Vector2(0f, 0.5f);

    private float fireTimer;

    // Update is called once per frame
    private void Update()
    {
        if (!GameManager.Instance.IsPlaying)
        {
            return;
        }

        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            fireTimer = fireInterval;

            Vector2 origin = (Vector2)transform.position + muzzleOffset;
            SpawnService.Instance.Projectiles.Spawn(projectile, origin, Vector2.up, true);
        }
    }
}
