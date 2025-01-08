using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// Classe PlaySound
/// <para>
/// Classe qui gère la lecture des sons pour les interactions (survol, clic, etc.).
/// Elle contrôle la lecture des sons de survol, de clic et de sortie.
/// </para>
/// </summary>
public class PlaySound : MonoBehaviour
{
    public AudioSource audioSource; // Source audio utilisée pour jouer les sons
    public AudioClip hoverSound; // Son joué lorsqu'on survole un élément
    public AudioClip clickSound; // Son joué lors d'un clic
    public AudioClip quitSound; // Son joué lorsqu'on quitte l'application

    private AudioSource[] audioSources; // Tableau contenant toutes les sources audio attachées au GameObject

    void Start() {
        audioSources = GetComponents<AudioSource>(); // Récupère toutes les sources audio attachées au GameObject
        foreach(AudioSource audio in audioSources) {
            audio.volume = GlobalVariables.soundLevel; // Applique le niveau sonore global à toutes les sources audio
        }
    }

    void Update()
    {
        audioSource.volume = GlobalVariables.soundLevel; // Met à jour le volume de la source audio principale
    }

    public void PlayHoverSound()
    {
        if (audioSource != null && hoverSound != null) // Vérifie si la source audio et le son de survol sont valides
        {
            audioSource.PlayOneShot(hoverSound); // Joue le son de survol
        }
    }

    public void PlayClickSound()
    {
        if (audioSource != null && clickSound != null) // Vérifie si la source audio et le son de clic sont valides
        {
            audioSource.PlayOneShot(clickSound); // Joue le son de clic
        }
    }

    public void PlayQuitSound()
    {
        if (audioSource != null && quitSound != null) // Vérifie si la source audio et le son de sortie sont valides
        {
            audioSource.PlayOneShot(quitSound); // Joue le son de sortie
        }
    }
}
