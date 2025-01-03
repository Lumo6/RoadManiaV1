using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CountTimer : MonoBehaviour
{
    public GameObject player;
    public TMP_Text countdownText;

    private float startTime = MenuManager.activeDifficulty.timeChrono;
    private float currentTime;
    private bool isChronoMode;

    void Start()
    {
        if (startTime == 0) // Pas en mode chrono
        {
            currentTime = 0.0f;
            isChronoMode = false;
        } 
        else 
        {
            currentTime = startTime;
            isChronoMode = true;
        }  
    }

    void Update()
    {
        if (isChronoMode)
        {
            currentTime -= Time.deltaTime;
            currentTime = Mathf.Clamp(currentTime, 0.0f, startTime);
        }
        else { 
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

        if (isChronoMode && currentTime <= 0)
        {
            GhostData data = player.GetComponent<GhostRecorder>().GetRecordedData();

            SaverManager saver = SaverManager.Instance;
            saver.SaveBestDistances(GlobalVariables.distanceParcourue, data, GlobalVariables.top_3_run_filename);

            SceneManager.LoadScene("SceneLoser");
        }
    }
}
