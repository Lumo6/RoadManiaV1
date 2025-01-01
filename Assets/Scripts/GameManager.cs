using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public float maxSpeed = 20f;
    public float minSpeed = 6f;
    public static GameObject bonusmalus;

    public Canvas MalusVision; // Reference to the MalusVision canvas
    public Renderer carRenderer; // Reference to the car's renderer
    public Collider carCollider; // Reference to the car's collider

    // Events for bonus and malus
    public UnityEvent<float> OnBonusGhost;
    public UnityEvent<float> OnMalusScreen;

    public static GameManager Instance
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
            OnBonusGhost = new UnityEvent<float>();
        if (OnMalusScreen == null)
            OnMalusScreen = new UnityEvent<float>();

        // Subscribe methods to the events
        OnBonusGhost.AddListener(BonusGhost);
        OnMalusScreen.AddListener(MalusScreen);
    }

    public void MalusScreen(float duration)
    {
        if (MalusVision != null)
        {
            MalusVision.enabled = true;
        }
        StartCoroutine(RemoveEffectAfterTime(duration));
    }

    private System.Collections.IEnumerator RemoveEffectAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (MalusVision != null)
        {
            MalusVision.enabled = false;
        }
    }

    public void BonusGhost(float duration)
    {
        if (carRenderer != null && carCollider != null)
        {
            StartCoroutine(ApplyGhostEffect(duration));
        }
    }

    private System.Collections.IEnumerator ApplyGhostEffect(float duration)
    {
        // Make the car semi-transparent and disable collisions
        Color originalColor = carRenderer.material.color;
        carRenderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
        carCollider.enabled = false;

        yield return new WaitForSeconds(duration);

        // Restore original state
        carRenderer.material.color = originalColor;
        carCollider.enabled = true;
    }
}
