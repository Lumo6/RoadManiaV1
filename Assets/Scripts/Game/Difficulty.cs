using UnityEngine;

/// <summary>
/// <para>
/// Classe Difficulty
/// </para>
/// <para>
/// Représente un objet ScriptableObject pour configurer les paramètres de difficulté d'un jeu.
/// Permet de définir les paramètres initiaux ainsi que l'évolution dynamique de la difficulté
/// en fonction du temps ou de la distance.
/// </para>
/// </summary>
[CreateAssetMenu(fileName = "NewDifficulty", menuName = "GameSettings/Difficulty")]
public class Difficulty : ScriptableObject
{
    /// <summary>
    /// Enumération pour définir les options d'évolution de la difficulté.
    /// La difficulté peut évoluer en fonction du temps écoulé ou de la distance parcourue.
    /// </summary>
    public enum OptionsEvolutionDifficulte
    {
        Temps,    // Évolution basée sur le temps
        Distance  // Évolution basée sur la distance
    }

    [Header("Paramètres initials")]

    [Tooltip("Niveau de difficulté (0 : Facile, 1 : Normal, 2 : Difficile, 3 : Chrono)")]
    public int difficultyLevel; // Niveau de difficulté défini par l'utilisateur.

    [Tooltip("Espace initial entre les obstacles")]
    public float offsetObstacle; // Distance entre les obstacles au début de la partie.

    [Tooltip("Probabilité initiale d'apparition des obstacles (entre 0 et 1)")]
    public float probaGeneration; // Détermine la fréquence d'apparition des obstacles.

    [Tooltip("Durée du décompte en secondes pour le mode chrono")]
    public float timeChrono; // Temps total disponible en mode chrono.

    [Header("Paramètres d'évolutions")]

    [SerializeField]
    [Tooltip("Définit si l'évolution de la difficulté dépend du temps ou de la distance")]
    private OptionsEvolutionDifficulte evolutionDifficulte;

    /// <summary>
    /// Propriété pour accéder à l'option d'évolution de la difficulté.
    /// </summary>
    public OptionsEvolutionDifficulte EvolutionDifficulte => evolutionDifficulte;

    [Tooltip("Pourcentage de réduction de l'espacement entre les obstacles à chaque étape d'évolution")]
    public float pourcentageReductionOffset; // Réduction appliquée à l'espacement entre obstacles.

    [Tooltip("Pourcentage d'augmentation de la probabilité d'apparition des obstacles à chaque étape d'évolution")]
    public float pourcentageAugmentationProbaGenration; // Augmentation de la probabilité de génération des obstacles.

    /// <summary>
    /// Méthode pour récupérer l'option d'évolution de la difficulté (temps ou distance).
    /// </summary>
    /// <returns>Option d'évolution de la difficulté.</returns>
    public OptionsEvolutionDifficulte GetOptionsEvolutionDifficulte()
    {
        return evolutionDifficulte;
    }

    [SerializeField]
    [Tooltip("Intervalle en secondes pour appliquer une évolution (utilisé si l'option Temps est sélectionnée)")]
    public float toutesLesNbSecondes; // Temps nécessaire avant chaque étape d'évolution.

    [SerializeField]
    [Tooltip("Distance en mètres pour appliquer une évolution (utilisé si l'option Distance est sélectionnée)")]
    public float toutesLesNbMetres; // Distance nécessaire avant chaque étape d'évolution.
}
