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
                Debug.LogWarning("Aucun temps enregistr� pour le niveau de difficult� 3 !");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Donn�es fant�mes charg�es avec succ�s !");
            }

            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }
            Debug.Log("Tous les colliders ont �t� supprim�s du Ghost.");
        }
        else
        {
            Debug.Log("Difficult� diff�rente, pas de donn�es fant�mes charg�es.");
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
               (playbackTime - ghostData.timestamps[currentIndex]) = dur�e depuis le dernier enregistrement du fant�me jusqu'au moment actuel
               ghostData.timestamps[currentIndex + 1] - ghostData.timestamps[currentIndex] = dur�e totale l'enregistrement de la position suivante et la position actuelle
             */
            float t = (playbackTime - ghostData.timestamps[currentIndex]) / (ghostData.timestamps[currentIndex + 1] - ghostData.timestamps[currentIndex]);

            transform.position = Vector3.Lerp(ghostData.positions[currentIndex], ghostData.positions[currentIndex + 1], t);

            transform.rotation = Quaternion.Slerp(ghostData.rotations[currentIndex], ghostData.rotations[currentIndex + 1], t);
        }
    }
}
