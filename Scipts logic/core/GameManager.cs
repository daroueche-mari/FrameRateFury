using UnityEngine;

// Gère l'état de la partie
[DefaultExecutionOrder(-50)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int targetFrameRate = 60;

    // Pour tester la logique sans l'UI
    [SerializeField] private bool startWithoutMenu = false;

    private GameState state;

    public GameState State
    {
        get { return state; }
    }

    public bool IsPlaying
    {
        get { return state == GameState.Playing; }
    }

    private void Awake()
    {
        Instance = this;

        // Sur Android, Unity est limité à 30 FPS par défaut
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
        Time.timeScale = 1f;
    }

    private void Start()
    {
        if (startWithoutMenu)
        {
            SetState(GameState.Playing);
        }
        else
        {
            SetState(GameState.Menu);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // Appelé automatiquement quand l'application passe en arrière-plan.
    private void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            Pause();
        }
    }

    // L'UI l'appelle quand le joueur touche l'écran "prêt".
    public void StartGame()
    {
        if (state == GameState.Menu)
        {
            SetState(GameState.Playing);
        }
    }

    public void Pause()
    {
        if (state == GameState.Playing)
        {
            SetState(GameState.Paused);
        }
    }

    public void Resume()
    {
        if (state == GameState.Paused)
        {
            SetState(GameState.Playing);
        }
    }

    // Recommencer / Rejouer
    public void Restart()
    {
        ResetGame();
        SetState(GameState.Menu);
    }

    // Retour au menu principal
    public void QuitToMenu()
    {
        ResetGame();
        SetState(GameState.Menu);
    }

    public void TriggerGameOver()
    {
        if (state == GameState.GameOver)
        {
            return;
        }

        ScoreManager.Instance.SaveBestScore();
        SetState(GameState.GameOver);
        GameEvents.RaiseGameOver(ScoreManager.Instance.Score, ScoreManager.Instance.BestScore);

        Debug.Log(SpawnStats.BuildReport());
    }

    // Remet la partie à zéro sans recharger la scène
    private void ResetGame()
    {
        EntityRegistry.ReleaseAll();
        GameEvents.RaiseGameReset();
    }

    private void SetState(GameState newState)
    {
        state = newState;

        if (state == GameState.Playing)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }

        GameEvents.RaiseStateChanged(state);
    }
}