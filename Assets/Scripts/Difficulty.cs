using UnityEngine;

[CreateAssetMenu(fileName = "NewDifficulty", menuName = "GameSettings/Difficulty")]
public class Difficulty : ScriptableObject
{
    public enum OptionsEvolutionDifficulte
    {
        Temps,
        Distance
    }

    [Header("Paramètres initials")]

    [Tooltip("Niveau de difficulté")]
    public int difficultyLevel; // Niveau de difficulté (0 : Facile, 1 : Normal, 2 : Difficile, 3 : Chrono)

    [Tooltip("Espace entres les obstacles")]
    public float offsetObstacle;

    [Tooltip("Probabilité d'apparition des obstacles")]
    public float probaGeneration;

    [Tooltip("Décompte pour mode chrono")]
    public float timeChrono;

    [Header("Paramètres d'évolutions")]

    [SerializeField]
    private OptionsEvolutionDifficulte evolutionDifficulte;

    [Tooltip("Evolution de la difficulté en fonction de :")]
    public OptionsEvolutionDifficulte EvolutionDifficulte => evolutionDifficulte;

    [Tooltip("Pourcentag de réduction d'espacements entre les obstacles")]
    public float pourcentageReductionOffset;

    [Tooltip("Pourcentag d'augmentation de la probabilité d'apparition des obstacles")]
    public float pourcentageAugmentationProbaGenration;

    public OptionsEvolutionDifficulte GetOptionsEvolutionDifficulte() { return evolutionDifficulte; }

    [SerializeField]
    [Tooltip("Paramètre en secondes (utilisé si Temps est sélectionné).")]
    public float toutesLesNbSecondes;

    [SerializeField]
    [Tooltip("Paramètre en mètres (utilisé si Distance est sélectionné).")]
    public float toutesLesNbMetres;
}