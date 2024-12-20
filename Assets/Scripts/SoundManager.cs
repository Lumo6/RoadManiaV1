using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public AudioClip bonusSound;
    public AudioClip malusSound;
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
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBonusSound()
    {
        audioSource.PlayOneShot(bonusSound);
    }

    public void PlayMalusSound()
    {
        audioSource.PlayOneShot(malusSound);
    }

    public void PlayCollisionSound()
    {
        audioSource.PlayOneShot(collisionSound);
    }
}
