using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Classe MenuManager
/// <para>
/// Gère le menu principal du jeu, y compris le choix de la difficulté, les réglages du son, et les actions de navigation entre les scènes.
/// </para
/// </summary>
public class MenuManager : MonoBehaviour
{
    public static Difficulty activeDifficulty; // Difficulté actuellement sélectionnée.

    /// <summary>
    /// Lance le jeu avec le niveau de difficulté spécifié.
    /// </summary>
    /// <param name="difficulty">Le niveau de difficulté choisi pour le jeu.</param>
    public void PlayGame(int difficulty)
    {
        // Mise a jour de la difficulté
        GlobalVariables.difficulty = difficulty;

        // Récupération du slider pour le volume
        Transform sliderTransform = transform.parent.Find("OptionsMenu/Slider");

        if (sliderTransform  != null)
        {
            GameObject slider = sliderTransform.gameObject;
            Debug.Log("Slider trouvé : " + slider.name);
            // Mise a jour du niveau de son pour les AudioSource
            GlobalVariables.soundLevel = slider.GetComponent<Slider>().value;
        }
        else
        {
            Debug.LogError("Slider introuvable !");
            // Si pas de slider -> son sur 50%
            GlobalVariables.soundLevel = 0.5f;
        }

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
                // Mise a jour de la difficulté choisie
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

    /// <summary>
    /// Retourne au menu principal.
    /// </summary>
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    /// <summary>
    /// Redémarre le jeu avec la difficulté actuelle.
    /// </summary>
    public void Retry()
    {
        PlayGame(GlobalVariables.difficulty);
    }

    /// <summary>
    /// Quitte le jeu.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
