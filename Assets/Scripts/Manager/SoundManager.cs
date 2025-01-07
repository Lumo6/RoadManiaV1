using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;


    public AudioSource audioSource;
    public AudioClip bonusGhostSound;
    public AudioClip malusScreenSound;
    public AudioClip collisionSound;
    // Events for bonus and malus
    public UnityEvent OnBonusGhost;
    public UnityEvent OnMalusScreen;
    public UnityEvent OnCollision;

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

        // Initialize events if null
        if (OnBonusGhost == null)
            OnBonusGhost = new UnityEvent();
        if (OnMalusScreen == null)
            OnMalusScreen = new UnityEvent();
        if (OnCollision == null)
            OnCollision = new UnityEvent();

        // Subscribe methods to the events
        OnBonusGhost.AddListener(PlayBonusGhostSound);
        OnMalusScreen.AddListener(PlayMalusScreenSound);
        OnCollision.AddListener(PlayCollisionSound);
    }

    public void PlayBonusGhostSound()
    {
        audioSource.PlayOneShot(bonusGhostSound);
    }

    public void PlayMalusScreenSound()
    {
        audioSource.PlayOneShot(malusScreenSound);
    }

    public void PlayCollisionSound()
    {
        audioSource.PlayOneShot(collisionSound);
    }
}
