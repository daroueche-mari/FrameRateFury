using UnityEngine;

// Calcule les FPS en faisant la moyenne sur une courte période (plus lisible qu'une valeur par frame).
// Le HUD lit FpsCounter.CurrentFps.
public class FpsCounter : MonoBehaviour
{
    [SerializeField] private float sampleDuration = 0.5f;

    private static float currentFps;

    private int frameCount;
    private float elapsed;

    public static float CurrentFps
    {
        get { return currentFps; }
    }

    private void Update()
    {
        // unscaledDeltaTime : continue de compter même quand le jeu est en pause (timeScale = 0).
        frameCount++;
        elapsed += Time.unscaledDeltaTime;

        if (elapsed >= sampleDuration)
        {
            currentFps = frameCount / elapsed;
            frameCount = 0;
            elapsed = 0f;
        }
    }
}
