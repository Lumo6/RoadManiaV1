using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe GlobalVariables
/// <para>
/// Classe qui contient les variables globales utilisées à travers le projet.
/// Ces variables sont accessibles partout dans le code pour gérer les paramètres de jeu,
/// les préférences de l'utilisateur, et d'autres valeurs persistantes.
/// </para>
/// </summary>
public static class GlobalVariables
{
    /// <summary>
    /// Nom du fichier utilisé pour enregistrer la meilleure trajectoire du joueur.
    /// </summary>
    public static string bestRunFileName = "top_run_player";

    /// <summary>
    /// Nom du fichier utilisé pour enregistrer les 3 meilleures distances parcourues.
    /// </summary>
    public static string top_3_run_filename = "best_distances_player";

    /// <summary>
    /// Distance parcourue par le joueur pendant une session de jeu.
    /// </summary>
    public static float distanceParcourue = 0.0f;

    /// <summary>
    /// Niveau de difficulté du jeu, avec une valeur par défaut de 0.
    /// </summary>
    public static int difficulty = 0;

    /// <summary>
    /// Niveau sonore global du jeu, par défaut à 0.5 (milieu du volume) soit 50%.
    /// </summary>
    public static float soundLevel = 0.5f;

    /// <summary>
    /// Mode graphique actuel du jeu, avec une valeur par défaut de "Realiste".
    /// </summary>
    public static string graphismMode = "Realiste";
}
