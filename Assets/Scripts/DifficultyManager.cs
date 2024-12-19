using UnityEngine;

[CreateAssetMenu(fileName = "NewDifficulty", menuName = "GameSettings/Difficulty")]
public class DifficultyManager : ScriptableObject
{
    public static DifficultyManager Instance;

    [Range(0, 3)]
    public int difficultyLevel; // Niveau de difficulté (0 : Facile, 1 : Normal, 2 : Difficile, 3 : Chrono)
    public float timeChrono;

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Il y a déjà une instance de DifficultyManager.");
        }

        if (difficultyLevel == 3 && timeChrono == 0)
        {
            timeChrono = 10.0f;
        }
    }

    public float GetTimeChrono()
    {
        if (difficultyLevel == 3)
        {
            return timeChrono;
        }
        else
        {
            Debug.LogWarning("timeChrono n'est utilisé que lorsque difficultyLevel est défini sur 3 (Chrono).");
            return 0;
        }
    }
}
