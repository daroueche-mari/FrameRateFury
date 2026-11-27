using System.Collections.Generic;
using System.Text;

// Compte les ennemis tirés au hasard, par vague, pour les tableaux statistiques du README.
// Le tableau est affiché dans la Console au Game Over (format Markdown, à copier-coller).
public static class SpawnStats
{
    private static Dictionary<int, Dictionary<string, int>> countsByWave = new Dictionary<int, Dictionary<string, int>>();
    private static List<string> enemyNames = new List<string>();
    private static int maxWave;

    public static void Reset()
    {
        countsByWave.Clear();
        enemyNames.Clear();
        maxWave = 0;
    }

    public static void Record(int wave, string enemyName)
    {
        Dictionary<string, int> waveCounts;

        if (!countsByWave.TryGetValue(wave, out waveCounts))
        {
            waveCounts = new Dictionary<string, int>();
            countsByWave.Add(wave, waveCounts);
        }

        if (!waveCounts.ContainsKey(enemyName))
        {
            waveCounts.Add(enemyName, 0);
        }
        waveCounts[enemyName]++;

        if (!enemyNames.Contains(enemyName))
        {
            enemyNames.Add(enemyName);
        }

        if (wave > maxWave)
        {
            maxWave = wave;
        }
    }

    public static string BuildReport()
    {
        StringBuilder report = new StringBuilder();
        Dictionary<string, int> totals = new Dictionary<string, int>();
        int grandTotal = 0;

        report.Append("| Vague |");
        foreach (string name in enemyNames)
        {
            report.Append(' ').Append(name).Append(" |");
            totals.Add(name, 0);
        }
        report.AppendLine();

        report.Append("| --- |");
        foreach (string name in enemyNames)
        {
            report.Append(" --- |");
        }
        report.AppendLine();

        for (int wave = 1; wave <= maxWave; wave++)
        {
            Dictionary<string, int> waveCounts;
            if (!countsByWave.TryGetValue(wave, out waveCounts))
            {
                continue;
            }

            report.Append("| ").Append(wave).Append(" |");

            foreach (string name in enemyNames)
            {
                int count = 0;
                waveCounts.TryGetValue(name, out count);
                totals[name] += count;
                grandTotal += count;
                report.Append(' ').Append(count).Append(" |");
            }
            report.AppendLine();
        }

        report.Append("| Total (%) |");
        foreach (string name in enemyNames)
        {
            float percent = 0f;
            if (grandTotal > 0)
            {
                percent = 100f * totals[name] / grandTotal;
            }
            report.Append(' ').Append(totals[name]).Append(" (").Append(percent.ToString("F1")).Append(" %) |");
        }
        report.AppendLine();

        return report.ToString();
    }
}
