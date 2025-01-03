using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static Difficulty activeDifficulty;

    public void PlayGame(int difficulty)
    {
        // Les différents niveaux de difficultés
        Difficulty[] difficultyList = Resources.LoadAll<Difficulty>("Scriptable Objects");

        if (difficultyList.Length == 0)
        {
            Debug.LogError("Aucune difficulté trouvée dans les ressources !");
            return;
        }

        foreach (Difficulty obj in difficultyList)
        {
            Debug.Log($"Difficulté trouvée : {obj.difficultyLevel}");
            if (difficulty == obj.difficultyLevel)
            {
                activeDifficulty = obj;
                Debug.Log($"Difficulté active définie : {activeDifficulty.difficultyLevel}");
                break;
            }
        }

        if (activeDifficulty == null)
        {
            Debug.LogError($"Aucune difficulté ne correspond au niveau {difficulty} !");
            return;
        }

        SceneManager.LoadScene("SceneGame");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
