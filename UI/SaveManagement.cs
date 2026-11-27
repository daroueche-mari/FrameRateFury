using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveManagement : MonoBehaviour
{
  

    [System.Serializable]
    public struct Data
    {
        public string nameData;
        //public int hpData;
        //public int waveData;
        //public int enemyData;
        //public int scoreData;
    }

    public static Data myData;



    public static void SaveGame(string slotName)
    {
        // 1. Récupérer le nom du personnage correspondant au slot
        if (slotName == "1")
        {
            myData.nameData = MyCharacter.mylistChar[0].Name;
        }
        else if (slotName == "2")
        {
            myData.nameData = MyCharacter.mylistChar[1].Name;
        }

        // 2. Convertir la struct en texte JSON et sauvegarder dans le slot
        string json = JsonUtility.ToJson(myData);
        PlayerPrefs.SetString(slotName, json);
        PlayerPrefs.Save();
        Debug.Log($"Partie sauvegardée dans {slotName} : {json}");
    }

    public static void LoadGame(string slotName)
    {
        // 1. Vérifier si une sauvegarde existe dans ce slot
        if (!PlayerPrefs.HasKey(slotName))
        {
            Debug.LogWarning($"Aucune sauvegarde trouvée dans {slotName}");
            return;
        }

        // 2. Lire le JSON et le reconvertir en struct Data
        string json = PlayerPrefs.GetString(slotName);
        myData = JsonUtility.FromJson<Data>(json);

        // 3. Appliquer les valeurs chargées aux données du jeu
        if (slotName == "1")
        {
            MyCharacter.mylistChar[0].Name = myData.nameData;
        }
        else if (slotName == "2")
        {
            MyCharacter.mylistChar[1].Name = myData.nameData;
        }

        Debug.Log($"Partie chargée depuis {slotName} pour le joueur {myData.nameData}");
    }
}
