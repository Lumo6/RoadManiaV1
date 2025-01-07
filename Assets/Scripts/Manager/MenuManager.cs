using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static Difficulty activeDifficulty;

    public void PlayGame(int difficulty)
    {
        GlobalVariables.difficulty = difficulty;

        Transform sliderTransform = transform.parent.Find("OptionsMenu/Slider");

        if (sliderTransform  != null)
        {
            GameObject slider = sliderTransform.gameObject;
            Debug.Log("Slider trouvé : " + slider.name);
            GlobalVariables.soundLevel = slider.GetComponent<Slider>().value;
        }
        else
        {
            Debug.LogError("Slider introuvable !");
            GlobalVariables.soundLevel = 0.5f;
        }

        // Les diff�rents niveaux de difficult�s
        Difficulty[] difficultyList = Resources.LoadAll<Difficulty>("Scriptable Objects");

        if (difficultyList.Length == 0)
        {
            Debug.LogError("Aucune difficult� trouv�e dans les ressources !");
            return;
        }

        foreach (Difficulty obj in difficultyList)
        {
            Debug.Log($"Difficult� trouv�e : {obj.difficultyLevel}");
            if (difficulty == obj.difficultyLevel)
            {
                activeDifficulty = obj;
                Debug.Log($"Difficult� active d�finie : {activeDifficulty.difficultyLevel}");
                break;
            }
        }

        if (activeDifficulty == null)
        {
            Debug.LogError($"Aucune difficult� ne correspond au niveau {difficulty} !");
            return;
        }

        SceneManager.LoadScene("SceneGame");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Retry()
    {
        PlayGame(GlobalVariables.difficulty);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
