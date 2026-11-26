using UnityEngine;

// Garde le score de la partie et le meilleur score
public class ScoreManager : MonoBehaviour
{
    public const string BestScoreKey = "BestScore";

    public static ScoreManager Instance { get; private set; }

    private int score;
    private int bestScore;

    public int Score
    {
        get { return score; }
    }

    public int BestScore
    {
        get { return bestScore; }
    }

    private void Awake()
    {
        Instance = this;
        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        GameEvents.GameReset += OnGameReset;
    }

    private void Start()
    {
        GameEvents.RaiseScoreChanged(score);
    }

    private void OnDestroy()
    {
        GameEvents.GameReset -= OnGameReset;

        if (Instance == this)
        {
            Instance = null;
        }
    }

    // Nouvelle partie
    private void OnGameReset()
    {
        score = 0;
        GameEvents.RaiseScoreChanged(score);
    }

    public void AddScore(int amount)
    {
        score += amount;
        GameEvents.RaiseScoreChanged(score);
    }

    public void SaveBestScore()
    {
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt(BestScoreKey, bestScore);
            PlayerPrefs.Save();
        }
    }
}