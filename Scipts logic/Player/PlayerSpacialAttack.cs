using UnityEngine;

// Jauge de l'attaque spéciale : chaque ennemi tué la remplit d'un cran.
public class PlayerSpecialAttack : MonoBehaviour
{
    [SerializeField] private PlayerBomb bomb;
    [SerializeField] private int killsRequired = 15;
    [SerializeField] private Vector2 launchOffset = new Vector2(0f, 0.6f);

    private int charge;

    public int Charge
    {
        get { return charge; }
    }

    public int KillsRequired
    {
        get { return killsRequired; }
    }

    private void OnEnable()
    {
        GameEvents.EnemyKilled += OnEnemyKilled;
        GameEvents.GameReset += OnGameReset;
    }

    private void OnDisable()
    {
        GameEvents.EnemyKilled -= OnEnemyKilled;
        GameEvents.GameReset -= OnGameReset;
    }

    // Nouvelle partie : jauge vide, bombe en vol annulée.
    private void OnGameReset()
    {
        charge = 0;
        bomb.Cancel();
        GameEvents.RaiseSpecialChargeChanged(charge, killsRequired);
    }

    private void Start()
    {
        bomb.gameObject.SetActive(false);
        GameEvents.RaiseSpecialChargeChanged(charge, killsRequired);
    }

    private void OnEnemyKilled(Vector2 enemyPosition)
    {
        // Pendant qu'une bombe est en vol ou explose, les morts ne comptent pas
        if (bomb.IsActive)
        {
            return;
        }

        charge++;

        if (charge >= killsRequired)
        {
            charge = 0;
            Vector2 start = (Vector2)transform.position + launchOffset;
            bomb.Launch(start);
        }

        GameEvents.RaiseSpecialChargeChanged(charge, killsRequired);
    }
}