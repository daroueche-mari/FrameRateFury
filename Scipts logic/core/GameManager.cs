using UnityEngine;

// Logique générale de la partie : réglages au lancement et fin de partie.
// Il est sur l'objet Managers, rangé dans GameRoot avec tout le reste du jeu.
// C'est l'UI qui active GameRoot (la partie commence) et qui le désactive (la partie s'arrête).
// La pause se fait aussi côté UI, avec Time.timeScale dans les boutons.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
 
    [SerializeField] private int targetFrameRate = 60;
 
    // Optionnel : si le champ est vide, on utilise PlayerController.Instance.
    [SerializeField] private PlayerController player;
 
    private bool isGameOver;
 
    public bool IsGameOver
    {
        get { return isGameOver; }
    }
 
    private void Awake()
    {
        Instance = this;
 
        // Sur Android, Unity est limité à 30 FPS par défaut : on force 60.
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }
 
    // Appelé à chaque fois que les Managers sont rallumés : nouvelle partie.
    private void OnEnable()
    {
        isGameOver = false;
 
        // Si la partie précédente s'est arrêtée pendant une pause, le temps était resté figé.
        Time.timeScale = 1f;
 
        // Si le Player est resté allumé depuis la partie précédente, son OnEnable ne sera pas rappelé :
        // on le remet à zéro nous-mêmes. (S'il est éteint, il se remettra à zéro tout seul en s'allumant.)
        PlayerController target = player;
        if (target == null)
        {
            target = PlayerController.Instance;
        }
 
        if (target != null && target.gameObject.activeInHierarchy)
        {
            target.ResetForNewGame();
        }
    }
 
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
 
    // Appelé par PlayerHealth quand les PV tombent à 0.
    // L'UI reçoit l'event GameOver : elle affiche l'écran de fin et met Time.timeScale à 0.
    public void TriggerGameOver()
    {
        if (isGameOver)
        {
            return;
        }
 
        isGameOver = true;
        ScoreManager.Instance.SaveBestScore();
        GameEvents.RaiseGameOver(ScoreManager.Instance.Score, ScoreManager.Instance.BestScore);
 
        // Statistiques d'apparition pour le README.
        Debug.Log(SpawnStats.BuildReport());
    }
}