using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlayer : MonoBehaviour
{
    private GhostData ghostData;

    private int currentIndex = 0;
    private float playbackTime = 0;

    void Awake()
    {
        if (MenuManager.activeDifficulty.difficultyLevel == 3)
        {
            SaverManager saver = SaverManager.Instance;
            ghostData = saver.LoadGhostData("top_run_player");

            if (ghostData == null)
            {
                Debug.LogWarning("Aucun temps enregistré pour le niveau de difficulté 3 !");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Données fantômes chargées avec succès !");
            }

            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }
            Debug.Log("Tous les colliders ont été supprimés du Ghost.");
        }
        else
        {
            Debug.Log("Difficulté différente, pas de données fantômes chargées.");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (ghostData == null || currentIndex >= ghostData.timestamps.Count - 1)
            return;

        playbackTime += Time.deltaTime;
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

            transform.position = Vector3.Lerp(ghostData.positions[currentIndex], ghostData.positions[currentIndex + 1], t);

            transform.rotation = Quaternion.Slerp(ghostData.rotations[currentIndex], ghostData.rotations[currentIndex + 1], t);
        }
    }
}
