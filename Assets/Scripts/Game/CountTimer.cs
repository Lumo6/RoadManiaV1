using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe CountTimer
/// <para>
/// Gère un compteur de temps, qui peut fonctionner soit en mode chronomètre (comptant à la hausse),
/// soit en mode chrono (comptant à la baisse).
/// </para>
/// <para>
/// - En mode chronomètre, le temps augmente indéfiniment.
/// </para>
/// <para>
/// - En mode chrono, le temps diminue jusqu'à atteindre zéro, moment où certaines actions spécifiques
///   sont déclenchées (enregistrement des données, changement de scène, etc.).
/// </para>
/// <para>
/// Cette classe met également à jour une interface utilisateur (UI) pour afficher le temps au format
/// mm:ss:ms (minutes, secondes, millisecondes).
/// </para>
/// </summary>
public class CountTimer : MonoBehaviour
{
    public GameObject player;
    public TMP_Text countdownText;

    private float startTime;
    private float currentTime;
    private bool isChronoMode;

    void Start()
    {
        // On récupère le temps définit pour le mode chrono
        startTime = MenuManager.activeDifficulty.timeChrono;

        if (startTime == 0.0f) // Pas en mode chrono
        {
            currentTime = 0.0f;
            isChronoMode = false;
        }
        else // Mode chrono
        {
            currentTime = startTime;
            isChronoMode = true;
        }
    }

    void Update()
    {
        if (isChronoMode)
        {
            // Réduction du temps car mode chrono
            currentTime -= Time.deltaTime;
            // On s'assure que le temps soit entre 0 et le temps du début (e.g. 10 secondes)
            currentTime = Mathf.Clamp(currentTime, 0.0f, startTime);
        }
        else {
            // Pas mode chrono donc on augmente le temps
            currentTime += Time.deltaTime;
        }

        if (countdownText != null)
        {
            // Calculer les minutes, secondes et millisecondes
            int minutes = Mathf.FloorToInt(currentTime / 60); // Divise par 60 pour obtenir les minutes
            int seconds = Mathf.FloorToInt(currentTime % 60); // Prend le reste pour obtenir les secondes
            int milliseconds = Mathf.FloorToInt((currentTime * 1000) % 1000); // Multiplie par 1000 pour obtenir les millisecondes

            // Formater l'affichage pour avoir deux chiffres pour les secondes et millisecondes
            countdownText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        }

        // Fin du chrono
        if (isChronoMode && currentTime <= 0)
        {
            // On récupère la trajectoire et les autres informations nécessaires permettant de faire le fantôme
            GhostData data = player.GetComponent<GhostRecorder>().GetRecordedData();

            // On lance la sauvegarde.
            SaverManager saver = SaverManager.Instance;
            saver.SaveBestDistances(GlobalVariables.distanceParcourue, data, GlobalVariables.top_3_run_filename);

            // Changement vers la scène de fin de jeu
            SceneManager.LoadScene("SceneLoser");
        }
    }
}
