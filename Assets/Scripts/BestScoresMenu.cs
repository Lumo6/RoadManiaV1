using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BestScoresMenu : MonoBehaviour
{
    public TextMeshProUGUI firstText;
    public TextMeshProUGUI secondText;
    public TextMeshProUGUI thirdText;

    void OnEnable()
    {
        GhostSaver saver = new GhostSaver();

        BestDistancesData bestDistancesData = saver.LoadBestDistances("best_distances_player");

        if (bestDistancesData != null)
        {
            string[] texts = new string[3] { "N/A", "N/A", "N/A" };

            for (int i = 0; i < bestDistancesData.bestDistances.Count && i < texts.Length; i++)
            {
                texts[i] = $"{bestDistancesData.bestDistances[i]} m";
            }

            firstText.text = texts[0];
            secondText.text = texts[1];
            thirdText.text = texts[2];
        } else
        {
            Debug.LogError("Impossible de charger les scores du joueur");
        }
      
    }
}
