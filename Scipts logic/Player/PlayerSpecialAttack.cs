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

    // Appelé à chaque fois que GameRoot est activé : nouvelle partie, jauge vide, bombe cachée.
    private void OnEnable()
    {
        GameEvents.EnemyKilled += OnEnemyKilled;

        charge = 0;
        bomb.Cancel();
        GameEvents.RaiseSpecialChargeChanged(charge, killsRequired);
    }

    private void OnDisable()
    {
        GameEvents.EnemyKilled -= OnEnemyKilled;
    }

    private void OnEnemyKilled(Vector2 enemyPosition)
    {
        // Pendant qu'une bombe est en vol ou explose, les morts ne comptent pas :
        // sinon la bombe se rechargerait elle-même.
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
