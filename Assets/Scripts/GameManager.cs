using System.Linq;
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
    public MeshCollider carCollider; // Reference to the car's collider
    public GameObject car; // Reference to the car
    private int originalLayer;
    private Color originalColor;

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

        originalLayer = car.layer;
        originalColor = carRenderer.material.color;
    }

    public void MalusScreen(float duration)
    {
        if (MalusVision != null)
        {
            MalusVision.enabled = true;
        }
        StartCoroutine(RemoveEffectAfterTime(duration));
        Debug.Log("la je suis dans malus");
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
        Debug.Log("Je suis dans le BonusGhost");
        if (carRenderer != null && carCollider != null)
        {
            StartCoroutine(ApplyGhostEffect(duration));
        }
    }

    private System.Collections.IEnumerator ApplyGhostEffect(float duration)
    {
        SetLayerRecursively(car, LayerMask.NameToLayer("NoCollisionLayer"));

        // Make the car semi-transparent
        carRenderer.material.color = Color.blue;

        // Wait for the duration
        yield return new WaitForSeconds(duration);

        SetLayerRecursively(car, LayerMask.NameToLayer("Default"));

        // Restore original color
        carRenderer.material.color = originalColor;
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
