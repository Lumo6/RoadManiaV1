using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Classe SoundManager
/// <para>
/// Gestionnaire des sons du jeu, responsable de jouer les sons associés aux événements tels que le bonus, le malus et la collision.
/// </para>
/// <para>
/// Implémente un modèle Singleton.
/// </para>
/// </summary>
public class SoundManager : MonoBehaviour
{
    // Singleton
    private static SoundManager _instance;

    public AudioSource audioSource; // Source audio utilisée pour jouer les sons.

    public AudioClip bonusGhostSound; // Clip audio joué lors du bonus fantôme.

    public AudioClip malusScreenSound; // Clip audio joué lors du malus écran.

    public AudioClip collisionSound; // Clip audio joué lors d'une collision.

    public UnityEvent OnBonusGhost; // Événement déclenché lors du bonus fantôme.

    public UnityEvent OnMalusScreen; // Événement déclenché lors du malus écran.

    public UnityEvent OnCollision; // Événement déclenché lors d'une collision.

    private AudioSource[] audioSources; // Listes des différents AudioSource

    /// <summary>
    /// Instance unique de SoundManager.
    /// </summary>
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("SoundManager is null !!!");

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;

        // Initialisation des événements s'ils sont nuls
        if (OnBonusGhost == null)
            OnBonusGhost = new UnityEvent();
        if (OnMalusScreen == null)
            OnMalusScreen = new UnityEvent();
        if (OnCollision == null)
            OnCollision = new UnityEvent();

        // Souscrire les méthodes aux événements
        OnBonusGhost.AddListener(PlayBonusGhostSound);
        OnMalusScreen.AddListener(PlayMalusScreenSound);
        OnCollision.AddListener(PlayCollisionSound);

        audioSources = GetComponents<AudioSource>();

        // Définir le volume de chaque source audio
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = GlobalVariables.soundLevel;
        }
    }

    /// <summary>
    /// Joue le son du bonus fantôme.
    /// </summary>
    public void PlayBonusGhostSound()
    {
        audioSource.PlayOneShot(bonusGhostSound);
    }

    /// <summary>
    /// Joue le son du malus écran.
    /// </summary>
    public void PlayMalusScreenSound()
    {
        audioSource.PlayOneShot(malusScreenSound);
    }

    /// <summary>
    /// Joue le son de collision.
    /// </summary>
    public void PlayCollisionSound()
    {
        audioSource.PlayOneShot(collisionSound);
    }
}
