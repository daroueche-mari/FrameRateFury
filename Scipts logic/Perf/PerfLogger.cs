using System;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

// Enregistre régulièrement le temps, les FPS et le nombre d'entités dans un fichier CSV.
// Le fichier sert à faire les graphiques du rapport de performance.
// Emplacement : Application.persistentDataPath (le chemin est affiché dans la Console au démarrage).
public class PerfLogger : MonoBehaviour
{
    [SerializeField] private float logInterval = 0.5f;

    private StringBuilder csv = new StringBuilder();
    private string filePath;
    private float timer;
    private float totalTime;
    private string mode;

    private void Start()
    {
        mode = "naive";
        if (SpawnService.Instance.UseOptimization)
        {
            mode = "pooling";
        }

        string date = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        filePath = Path.Combine(Application.persistentDataPath, "perf_" + mode + "_" + date + ".csv");

        // Point-virgule : Excel en français l'ouvre directement en colonnes.
        csv.AppendLine("temps_s;fps;entites;ennemis;balles_joueur;balles_ennemies;mode");

        Debug.Log("[PerfLogger] Fichier CSV : " + filePath);
    }

    private void Update()
    {
        float deltaTime = Time.unscaledDeltaTime;
        timer += deltaTime;
        totalTime += deltaTime;

        if (timer < logInterval)
        {
            return;
        }

        timer = 0f;

        csv.Append(totalTime.ToString("F2", CultureInfo.InvariantCulture)).Append(';');
        csv.Append(FpsCounter.CurrentFps.ToString("F1", CultureInfo.InvariantCulture)).Append(';');
        csv.Append(EntityRegistry.ActiveCount).Append(';');
        csv.Append(EntityRegistry.Enemies.Count).Append(';');
        csv.Append(EntityRegistry.PlayerProjectiles.Count).Append(';');
        csv.Append(EntityRegistry.EnemyProjectiles.Count).Append(';');
        csv.AppendLine(mode);
    }

    // Sur Android, OnApplicationQuit n'est pas toujours appelé : on sauvegarde aussi à la mise en arrière-plan.
    private void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            Save();
        }
    }

    private void OnDestroy()
    {
        Save();
    }

    private void Save()
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        File.WriteAllText(filePath, csv.ToString());
    }
}
