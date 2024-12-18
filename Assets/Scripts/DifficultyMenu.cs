using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static int difficultyLevel; // Static variable to store selected difficulty

    // Method to be linked to buttons
    public void PlayGame(int difficulty)
    {
        difficultyLevel = difficulty; // Set the selected difficulty
        SceneManager.LoadScene("SceneGame"); // Load the game scene
    }
}
