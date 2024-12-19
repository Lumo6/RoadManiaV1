using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyMenu : MonoBehaviour
{
    public DifficultyManager difficultyManager; // Référence au ScriptableObject

    public void PlayGame(int difficulty)
    {
        difficultyManager.difficultyLevel = difficulty;

        if (difficulty == 3) // Difficulté = 3 donc mode chrono
        {
            difficultyManager.timeChrono = 10f; // Chrono de 10s
        }

        SceneManager.LoadScene("SceneGame");
    }
}
