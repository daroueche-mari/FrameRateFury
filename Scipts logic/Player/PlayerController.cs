using UnityEngine;

// Déplace le vaisseau avec le joystick virtuel et l'empêche de sortir de l'écran.
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] private VirtualJoystick joystick;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float edgePadding = 0.4f;     // demi-taille du vaisseau
    [SerializeField] private float hitboxRadius = 0.15f;   // plus petite que le sprite
    [SerializeField] private float bottomHudMargin = 2.5f; 

    private Vector2 position;
    private Vector2 startPosition;

    public Vector2 Position
    {
        get { return position; }
    }

    public float HitboxRadius
    {
        get { return hitboxRadius; }
    }

    private void Awake()
    {
        Instance = this;
        position = transform.position;
        startPosition = position;
        GameEvents.GameReset += OnGameReset;
    }

    private void OnDestroy()
    {
        GameEvents.GameReset -= OnGameReset;

        if (Instance == this)
        {
            Instance = null;
        }
    }

    //le vaisseau revient à sa place de départ.
    private void OnGameReset()
    {
        position = startPosition;
        transform.position = position;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPlaying)
        {
            return;
        }

        position += joystick.Direction * speed * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, ScreenBounds.Min.x + edgePadding, ScreenBounds.Max.x - edgePadding);
        position.y = Mathf.Clamp(position.y, ScreenBounds.Min.y + bottomHudMargin + edgePadding, ScreenBounds.Max.y - edgePadding);

        transform.position = position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, hitboxRadius);
    }
}