using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe GhostRecorder
/// <para>
/// Permet d'enregistrer les positions, rotations, et timestamps d'un objet (par exemple, une voiture) pour créer un "fantôme".
/// Ces données peuvent être utilisées pour rejouer les mouvements de l'objet dans un mode fantôme.
/// </para>
/// <para>
/// Le script fonctionne uniquement en mode Chrono (niveau 3 de difficulté).
/// Si une autre difficulté est active, le script est automatiquement désactivé.
/// </para>
/// </summary>
public class GhostRecorder : MonoBehaviour
{
    private GhostData ghostData = new GhostData(); // Contient les données enregistrées du fantôme.
    private float startTime; // Temps de départ de l'enregistrement.
    private Transform carTransform; // Transform de l'objet à enregistrer (ex : voiture).

    void Start()
    {
        // Si la difficulté active n'est pas le mode Chrono, désactive le script.
        if (MenuManager.activeDifficulty.difficultyLevel != 3)
        {
            this.enabled = false; // Désactive ce composant.
        }
        else
        {
            Debug.Log("Début de l'enregistrement");
        }

        // Initialise le temps de départ.
        startTime = Time.time;

        // Récupère le transform de l'objet associé à ce script.
        carTransform = gameObject.GetComponent<Renderer>().transform;
    }

    void FixedUpdate()
    {
       // Ajoute la position actuelle de l'objet à la liste des positions enregistrées.
        ghostData.positions.Add(carTransform.position);

        // Ajoute la rotation actuelle de l'objet à la liste des rotations enregistrées.
        ghostData.rotations.Add(carTransform.rotation);

        // Ajoute le temps écoulé depuis le début de l'enregistrement à la liste des timestamps.
        ghostData.timestamps.Add(Time.time - startTime);

        Debug.Log("Nouvelle position enregistrée");
    }

    /// <summary>
    /// Récupère les données enregistrées du fantôme.
    /// <para>
    /// Les données incluent les positions, rotations, et timestamps capturés pendant l'exécution.
    /// </para>
    /// </summary>
    /// <returns>
    /// Un objet GhostData contenant les positions, rotations, et timestamps enregistrés.
    /// </returns>
    public GhostData GetRecordedData()
    {
        return ghostData;
    }
}
