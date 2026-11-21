using UnityEngine;

public class SaveManagement : MonoBehaviour
{
    [SerializeField]
    public Transform playerTransform;

    [System.Serializable]
    public struct Data
    {
        public string modelData;
        public string nameData;
        public int hpData;
        public int waveData;
        public int enemyData;
        public int scoreData;
    }

    public Data myData;


//    à adapter
//    public void SaveGame(string slotName)
//    {
//        1.Récupérer les valeurs actuelles du jeu vers la struct
//        Exemple : currentData.hpData = playerHealth.currentHp;
//    currentData.scoreData = GameManager.instance.score;

//         2. Convertir la struct en texte JSON et sauvegarder dans le slot
//        string json = JsonUtility.ToJson(currentData);
//    PlayerPrefs.SetString(slotName, json);
//        PlayerPrefs.Save();

//        Debug.Log($"Partie sauvegardée dans {slotName} : {json}");
//    }

//public void LoadGame(string slotName)
//{
//    1.Vérifier si une sauvegarde existe dans ce slot
//        if (!PlayerPrefs.HasKey(slotName))
//    {
//        Debug.LogWarning($"Aucune sauvegarde trouvée dans {slotName}");
//        return;
//    }

//    2.Lire le JSON et le reconvertir en struct Data
//        string json = PlayerPrefs.GetString(slotName);
//currentData = JsonUtility.FromJson<Data>(json);

//3.Appliquer les valeurs de currentData aux GameObjects du jeu
//         Exemple : playerHealth.currentHp = currentData.hpData;
//GameManager.instance.score = currentData.scoreData;

//Debug.Log($"Partie chargée depuis {slotName} pour le joueur {currentData.nameData}");
//    }
}
