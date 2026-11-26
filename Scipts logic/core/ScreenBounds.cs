using UnityEngine;


[DefaultExecutionOrder(-100)]
[RequireComponent(typeof(Camera))]
public class ScreenBounds : MonoBehaviour
 {
    [SerializeField] private float worldWidth = 9f;

    private static Vector2 min;
    private static Vector2 max;

    private Camera cam;
    private int lastScreenWidth;
    private int lastScreenHeight;

    public static Vector2 Min
    {
        get { return min; }
    }

    public static Vector2 Max
    {
        get { return max; }
    }

    private void Awake()
    {
        cam = GetComponent<Camera>();
        Refresh();
    }

    private void Update()
    {
        // si change resolu
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            Refresh();
        }
    }

    private void Refresh()
    {
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;

        cam.orthographicSize = worldWidth / (2f * cam.aspect);

        float halfWidth = worldWidth / 2f;
        float halfHeight = cam.orthographicSize;
        Vector2 center = cam.transform.position;

        min = new Vector2(center.x - halfWidth, center.y - halfHeight);
        max = new Vector2(center.x + halfWidth, center.y + halfHeight);
    }

    public static bool IsOutside(Vector2 position, float margin)
    {
        if (position.x < min.x - margin || position.x > max.x + margin)
        {
            return true;
        }

        if (position.y < min.y - margin || position.y > max.y + margin)
        {
            return true;
        }

        return false;
    }
}
