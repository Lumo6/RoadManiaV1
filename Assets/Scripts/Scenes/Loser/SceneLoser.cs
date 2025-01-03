using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoser : MonoBehaviour
{
    void Start()
    {
        Invoke("ChangeScene", 3.0f);
    }

    void Update()
    {
        
    }

    // Fonction permettant de revenir à la scène de jeu
    void ChangeScene()
    {
        SceneManager.LoadScene("SceneGame");
    }
}
