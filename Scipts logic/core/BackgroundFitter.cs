using UnityEngine;

// Fond du jeu en sprite (et pas en image d'UI) : il est toujours dessiné DERRIÈRE le vaisseau,
// les ennemis et les balles, et toujours DEVANT rien d'autre que le ciel de la caméra.
// Le script l'agrandit pour qu'il couvre tout l'écran, quel que soit le format du téléphone.
[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundFitter : MonoBehaviour
{
    [SerializeField] private int orderInLayer = -100;   // très négatif = derrière tous les autres sprites

    private SpriteRenderer spriteRenderer;
    private int lastScreenWidth;
    private int lastScreenHeight;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = orderInLayer;
    }

    private void Start()
    {
        Fit();
    }

    private void Update()
    {
        // Recalcule seulement si la taille de l'écran change (rotation, simulateur...).
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            Fit();
        }
    }

    private void Fit()
    {
        if (spriteRenderer.sprite == null)
        {
            return;
        }

        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;

        float screenWidth = ScreenBounds.Max.x - ScreenBounds.Min.x;
        float screenHeight = ScreenBounds.Max.y - ScreenBounds.Min.y;

        // Taille du sprite à l'échelle 1, en unités du monde.
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        // On prend la plus grande des deux échelles : l'image couvre tout l'écran
        // sans être déformée (quitte à dépasser un peu sur un côté).
        float scaleX = screenWidth / spriteSize.x;
        float scaleY = screenHeight / spriteSize.y;
        float scale = scaleX;

        if (scaleY > scale)
        {
            scale = scaleY;
        }

        transform.localScale = new Vector3(scale, scale, 1f);

        // Centré sur l'écran.
        Vector2 center = (ScreenBounds.Min + ScreenBounds.Max) / 2f;
        transform.position = new Vector3(center.x, center.y, 0f);
    }
}
