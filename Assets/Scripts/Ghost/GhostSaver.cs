using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BestDistancesData
{
    public List<float> bestDistances = new List<float>();
}

public class GhostSaver : MonoBehaviour
{
    public void SaveGhostData(GhostData ghostData, string fileName)
    {
        string json = JsonUtility.ToJson(ghostData);
        File.WriteAllText(Application.persistentDataPath + "/" + fileName + ".json", json);
    }

    public GhostData LoadGhostData(string fileName)
    {
        string filePath = Application.persistentDataPath + "/" + fileName + ".json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Debug.Log(Application.persistentDataPath);
            return JsonUtility.FromJson<GhostData>(json);
        }
        return null;
    }

    public void SaveBestDistances(float distance, GhostData data, string fileName)
    {
        BestDistancesData bestDistancesData = LoadBestDistances(fileName);

        // Si aucune donnée n'est trouvée, on crée et on ajoute directement la distance.
        if (bestDistancesData == null)
        {
            bestDistancesData = new BestDistancesData();
            bestDistancesData.bestDistances.Insert(0, distance);
            SaveGhostData(data, "top_run_player");
        } 
        else
        {
            // Vérifier si la nouvelle distance est meilleure que l'une des trois meilleures
            if (distance > bestDistancesData.bestDistances[0])
            {
                bestDistancesData.bestDistances.Insert(0, distance);
                if (bestDistancesData.bestDistances.Count > 3) bestDistancesData.bestDistances.RemoveAt(3);

                SaveGhostData(data, "top_run_player");
            }
            else if (distance > bestDistancesData.bestDistances[1])
            {
                bestDistancesData.bestDistances.Insert(1, distance);
                if (bestDistancesData.bestDistances.Count > 3) bestDistancesData.bestDistances.RemoveAt(3);
            }
            else if (distance > bestDistancesData.bestDistances[2])
            {
                bestDistancesData.bestDistances.Insert(2, distance);
                if (bestDistancesData.bestDistances.Count > 3) bestDistancesData.bestDistances.RemoveAt(3);
            }
        }
        // Sauvegarder les meilleures distances mises à jour dans le fichier JSON
        string json = JsonUtility.ToJson(bestDistancesData);
        File.WriteAllText(Application.persistentDataPath + "/" + fileName + ".json", json);
    }

    public BestDistancesData LoadBestDistances(string fileName) 
    {
        string filePath = Application.persistentDataPath + "/" + fileName + ".json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<BestDistancesData>(json);
        }
        return null;
    }
}