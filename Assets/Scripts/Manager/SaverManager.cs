using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BestDistancesData
{
    public List<float> bestDistances = new List<float>();
}

public class SaverManager
{
    // Singleton
    private static SaverManager _instance;
    public static SaverManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SaverManager();
            }
            return _instance;
        }
    }

    private SaverManager() { }

    // Enregistrement de la trajectoire du fantôme
    public void SaveGhostData(GhostData ghostData, string fileName)
    {
        string json = JsonUtility.ToJson(ghostData);
        File.WriteAllText(Application.persistentDataPath + "/" + fileName + ".json", json);
    }

    // Chargement de la trajectoire du fantôme
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

    // Enregistrement des 3 meilleures distances
    public void SaveBestDistances(float distance, GhostData data, string fileName)
    {
        BestDistancesData bestDistancesData = LoadBestDistances(fileName);

        if (bestDistancesData == null)
        {
            bestDistancesData = new BestDistancesData();
            bestDistancesData.bestDistances.Add(distance);

            SaveGhostData(data, GlobalVariables.bestRunFileName);
        }
        else
        {
            bestDistancesData.bestDistances.Add(distance);
            bestDistancesData.bestDistances.Sort((a, b) => b.CompareTo(a)); // Tri décroissant

            // Garde uniquement les 3 meilleurs scores
            if (bestDistancesData.bestDistances.Count > 3)
            {
                bestDistancesData.bestDistances.RemoveAt(3);
            }

            // Sauvegarde le fantôme uniquement si la distance est le meilleur score
            if (bestDistancesData.bestDistances[0] == distance)
            {
                SaveGhostData(data, GlobalVariables.bestRunFileName);
            }
        }

        string json = JsonUtility.ToJson(bestDistancesData);
        File.WriteAllText(Application.persistentDataPath + "/" + fileName + ".json", json);
    }

    // Chargement des 3 meilleures distances
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
