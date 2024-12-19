using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip bonusSound;
    public AudioClip malusSound;
    public AudioClip collisionSound;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

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
