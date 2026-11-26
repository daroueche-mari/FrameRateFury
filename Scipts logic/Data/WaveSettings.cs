using System.Collections.Generic;
using UnityEngine;

// Réglages des vagues infinies. Clic droit > Create > FrameRateFury > Wave Settings.
[CreateAssetMenu(fileName = "WaveSettings", menuName = "FrameRateFury/Wave Settings")]
public class WaveSettings : ScriptableObject
{
    [System.Serializable]
    public class EnemyWeight
    {
        public EnemyData enemy;
        public float startWeight = 1f;      // poids à la vague 1
        public float weightPerWave = 0f;    // ajouté à chaque vague (peut être négatif)

        public float GetWeight(int wave)
        {
            float weight = startWeight + weightPerWave * (wave - 1);

            if (weight < 0f)
            {
                weight = 0f;
            }

            return weight;
        }
    }

    [Header("Nombre d'ennemis")]
    public int baseEnemyCount = 5;
    public int enemiesPerWave = 2;

    [Header("Rythme")]
    public float spawnInterval = 0.6f;
    public float delayBetweenWaves = 2f;

    [Header("Difficulté")]
    public float fireRateBonusPerWave = 0.05f;   // +5 % de cadence de tir ennemie par vague
    public int waveBonusPerWave = 100;           // bonus de fin de vague = 100 x numéro

    [Header("Types d'ennemis et poids d'apparition")]
    public List<EnemyWeight> enemies = new List<EnemyWeight>();

    public int GetEnemyCount(int wave)
    {
        return baseEnemyCount + enemiesPerWave * (wave - 1);
    }

    // Probabilité théorique d'un ennemi pour une vague (pour le README).
    public float GetProbability(EnemyData enemy, int wave)
    {
        float total = 0f;
        float enemyWeight = 0f;

        foreach (EnemyWeight entry in enemies)
        {
            float weight = entry.GetWeight(wave);
            total += weight;

            if (entry.enemy == enemy)
            {
                enemyWeight = weight;
            }
        }

        if (total <= 0f)
        {
            return 0f;
        }

        return enemyWeight / total;
    }
}