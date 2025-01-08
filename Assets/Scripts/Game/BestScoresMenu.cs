using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Cette classe gère l'affichage des trois meilleurs scores de distance dans un menu.
/// <para>
/// Elle utilise le système de sauvegarde pour charger les meilleurs scores et met à jour
/// l'interface utilisateur avec ces données.
/// </para>
/// <para>
/// Si aucun score n'est disponible, elle affiche "N/A".
/// </para>
/// </summary>
public class BestScoresMenu : MonoBehaviour
{
    // Textes pour l'affichage des 3 meilleurs scores
    public TextMeshProUGUI firstText;
    public TextMeshProUGUI secondText;
    public TextMeshProUGUI thirdText;

    void OnEnable()
    {
        // Récupération de l'instance du Saver
        SaverManager saver = SaverManager.Instance;

        // Récupération des 3 meilleurs temps
        BestDistancesData bestDistancesData = saver.LoadBestDistances(GlobalVariables.top_3_run_filename);

        if (bestDistancesData != null)
        {
            // N/A car dans certains cas il n'y a que 0, 1 ou 2 scores
            string[] texts = new string[3] { "N/A", "N/A", "N/A" };

            for (int i = 0; i < bestDistancesData.bestDistances.Count && i < texts.Length; i++)
            {
                // ToString("F2") -> 2 chiffres après la virgules
                texts[i] = $"{bestDistancesData.bestDistances[i].ToString("F2")} m";
            }

            // Mise a jour de l'UI
            firstText.text = texts[0];
            secondText.text = texts[1];
            thirdText.text = texts[2];
        } else
        {
            Debug.LogError("Impossible de charger les scores du joueur");
        }

    }
}
