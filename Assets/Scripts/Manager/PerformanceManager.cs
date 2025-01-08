using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Classe PerformanceManager
/// <para>
/// Gère le changement entre les modes de performance (FPS ou Réaliste) et met à jour l'affichage du mode sélectionné.
/// </para>
/// </summary>
public class PerformanceManager : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro; // Référence au composant TextMeshPro utilisé pour afficher le mode de performance.

    void Start() {
        // Mise a jour du texte par défaut
        GlobalVariables.graphismMode = textMeshPro.text;
    }

    public void OnChange() {
        // Mise a jour du texte lors du clic pour choisir le mode de performance
        textMeshPro.text = textMeshPro.text == "FPS" ? "Realiste" : "FPS";
        GlobalVariables.graphismMode = textMeshPro.text;
    }
}
