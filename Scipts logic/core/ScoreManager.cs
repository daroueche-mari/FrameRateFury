using UnityEngine;

// Garde le score de la partie et le meilleur score (sauvegardé sur l'appareil).
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
    }

    // Appelé à chaque fois que GameRoot est activé : le score repart à 0.
    private void OnEnable()
    {
        score = 0;
        GameEvents.RaiseScoreChanged(score);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
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
