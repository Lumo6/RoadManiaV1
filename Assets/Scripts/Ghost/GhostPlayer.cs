using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Classe GhostPlayer
/// </para>
/// <para>
/// Rejoue les mouvements d'un "fantôme" basé sur des données enregistrées (positions, rotations, et timestamps).
/// Utilisée pour afficher une trajectoire préenregistrée d'un joueur ou d'un objet.
/// </para>
/// </summary>
public class GhostPlayer : MonoBehaviour
{
    private GhostData ghostData; // Données du fantôme (positions, rotations, timestamps).
    private int currentIndex = 0; // Index actuel dans les données de timestamps.
    private float playbackTime = 0; // Temps de lecture accumulé.

    void Awake()
    {
        // Vérifie si la difficulté active est au niveau 3 (Chrono).
        if (MenuManager.activeDifficulty.difficultyLevel == 3)
        {
            // Chargement des données du fantôme.
            SaverManager saver = SaverManager.Instance;
            ghostData = saver.LoadGhostData("top_run_player");

            if (ghostData == null)
            {
                // Pas de données disponibles, destruction de l'objet.
                Debug.LogWarning("Aucun temps enregistré pour le niveau de difficulté 3 !");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Données fantômes chargées avec succès !");
            }

            // Désactive tous les colliders attachés au fantôme.
            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }
            Debug.Log("Tous les colliders ont été supprimés du Ghost.");
        }
        else
        {
            // Si la difficulté n'est pas Chrono, destruction de l'objet.
            Debug.Log("Difficulté différente, pas de données fantômes chargées.");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Vérifie si les données sont valides et si on n'a pas atteint la fin de la trajectoire.
        if (ghostData == null || currentIndex >= ghostData.timestamps.Count - 1)
            return;

        // Met à jour le temps de lecture.
        playbackTime += Time.deltaTime;

        // Avance l'index jusqu'à trouver le timestamp correspondant au temps actuel.
        while (currentIndex < ghostData.timestamps.Count - 1 && playbackTime > ghostData.timestamps[currentIndex + 1])
        {
            currentIndex++;
        }

        if (currentIndex < ghostData.timestamps.Count - 1)
        {
            /* Interpolation entre deux positions
               (playbackTime - ghostData.timestamps[currentIndex]) = durée depuis le dernier enregistrement du fantôme jusqu'au moment actuel
               ghostData.timestamps[currentIndex + 1] - ghostData.timestamps[currentIndex] = durée totale l'enregistrement de la position suivante et la position actuelle
             */
            float t = (playbackTime - ghostData.timestamps[currentIndex]) / (ghostData.timestamps[currentIndex + 1] - ghostData.timestamps[currentIndex]);

            // Interpolation de la position entre deux points.
            transform.position = Vector3.Lerp(ghostData.positions[currentIndex], ghostData.positions[currentIndex + 1], t);

            // Interpolation de la rotation entre deux points.
            transform.rotation = Quaternion.Slerp(ghostData.rotations[currentIndex], ghostData.rotations[currentIndex + 1], t);
        }
    }
}
