using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;
    public AudioClip quitSound;

    void Update()
    {
        audioSource.volume = GlobalVariables.soundLevel;
    }

    public void PlayHoverSound()
    {
        if (audioSource != null && hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    public void PlayClickSound()
    {
        if (audioSource != null && hoverSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    public void PlayQuitSound()
    {
        if (audioSource != null && hoverSound != null)
        {
            audioSource.PlayOneShot(quitSound);
        }
    }
}
