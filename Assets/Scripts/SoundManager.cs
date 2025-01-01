using UnityEngine;
using UnityEngine.Events;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public AudioClip bonusGhostSound;
    public AudioClip malusScreenSound;
    // Events for bonus and malus
    public UnityEvent OnBonusGhost;
    public UnityEvent OnMalusScreen;


    public AudioClip collisionSound;

    private AudioSource audioSource;

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("GameManager is null !!!");

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

        // Subscribe methods to the events
        OnBonusGhost.AddListener(PlayBonusGhostSound);
        OnMalusScreen.AddListener(PlayMalusScreenSound);

        audioSource = GetComponent<AudioSource>();
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
