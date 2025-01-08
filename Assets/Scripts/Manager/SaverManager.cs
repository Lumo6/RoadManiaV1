using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Classe BestDistancesData
/// <para>
/// Gestionnaire des meilleurs distances faites par la joueur
/// </para>
/// </summary>
public class BestDistancesData
{
    /// <summary>
    /// Tableau contenant les meilleurs distances
    /// </summary>
    public List<float> bestDistances = new List<float>();
}

/// <summary>
/// Classe SaverManager
/// <para>
/// Gestionnaire des sauvegardes du jeu, permettant de sauvegarder et de charger les données du fantôme et les meilleures distances.
/// </para>
/// <para>
/// Implémente un modèle Singleton.
/// </para>
/// </summary>
public class SaverManager
{
    // Singleton
    private static SaverManager _instance;

    /// <summary>
    /// Instance unique de SaverManager.
    /// </summary>
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

    /// <summary>
    /// Sauvegarde les données de la trajectoire du fantôme dans un fichier JSON.
    /// </summary>
    /// <param name="ghostData">Les données du fantôme à sauvegarder.</param>
    /// <param name="fileName">Nom du fichier dans lequel les données doivent être sauvegardées.</param>
    public void SaveGhostData(GhostData ghostData, string fileName)
    {
        string json = JsonUtility.ToJson(ghostData);
        File.WriteAllText(Application.persistentDataPath + "/" + fileName + ".json", json);
    }

    /// <summary>
    /// Charge les données du fantôme à partir d'un fichier JSON.
    /// </summary>
    /// <param name="fileName">Nom du fichier à partir duquel les données doivent être chargées.</param>
    /// <returns>Les données du fantôme chargées ou null si le fichier n'existe pas.</returns>
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

    /// <summary>
    /// Sauvegarde les trois meilleures distances parcourues et les données du fantôme associé si la distance est le meilleur score.
    /// </summary>
    /// <param name="distance">La distance parcourue à sauvegarder.</param>
    /// <param name="data">Les données du fantôme associées à cette distance.</param>
    /// <param name="fileName">Nom du fichier dans lequel les distances et les données doivent être sauvegardées.</param>
    public void SaveBestDistances(float distance, GhostData data, string fileName)
    {
        // Chargement des meilleurs distances
        BestDistancesData bestDistancesData = LoadBestDistances(fileName);

        if (bestDistancesData == null)
        {
            // Si pas distances, on en crée une
            bestDistancesData = new BestDistancesData();
            bestDistancesData.bestDistances.Add(distance);

            // Enregistrement des données
            SaveGhostData(data, GlobalVariables.bestRunFileName);
        }
        else
        {
            // Ajout de la distance
            bestDistancesData.bestDistances.Add(distance);
            // Tri décroissant pour avoir du meilleur au moins bon
            bestDistancesData.bestDistances.Sort((a, b) => b.CompareTo(a));

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

        // Enregistrement dans un json
        string json = JsonUtility.ToJson(bestDistancesData);
        File.WriteAllText(Application.persistentDataPath + "/" + fileName + ".json", json);
    }

    /// <summary>
    /// Charge les trois meilleures distances parcourues à partir d'un fichier JSON.
    /// </summary>
    /// <param name="fileName">Nom du fichier à partir duquel les distances doivent être chargées.</param>
    /// <returns>Les meilleures distances chargées ou null si le fichier n'existe pas.</returns>
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
