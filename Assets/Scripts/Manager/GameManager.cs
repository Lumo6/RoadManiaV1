using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Classe GameManager
/// <para>
/// Cette classe gère les événements liés aux bonus et malus dans le jeu, tels que l'effet fantôme (qui rend la voiture intangible) et les malus visuels (qui affectent la vision du joueur).
/// Elle assure aussi le contrôle de la vitesse maximale et minimale de la voiture et l'activation des effets visuels de manière temporaire.
/// </para>
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager _instance; // Instance unique de la classe GameManager.
    [Range(10, 30)]
    public float maxSpeed = 20f; // Vitesse maximale de la voiture.
    [Range(3, 9)]
    public float minSpeed = 6f; // Vitesse minimale de la voiture.
    public static GameObject bonusmalus; // Référence à un objet de type bonus ou malus.
    public GameObject MalusVision; // Référence au malus de vision.
    public Renderer carRenderer; // Référence au renderer de la voiture.
    public MeshCollider carCollider; // Référence au collider de la voiture.
    public GameObject car; // Référence à l'objet voiture.
    private int originalLayer; // Couche originale de la voiture.
    private Color originalColor; // Couleur originale de la voiture.

    // Événements pour le bonus et malus
    public UnityEvent<float> OnBonusGhost; // Événement déclenché pour appliquer l'effet bonus fantôme.
    public UnityEvent<float> OnMalusScreen; // Événement déclenché pour appliquer le malus visuel.

    /// <summary>
    /// Instance unique du GameManager.
    /// <para>
    /// Permet d'accéder à une seule instance de GameManager à travers le jeu.
    /// </para>
    /// </summary>
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

        // Initialisation des événements si non définis
        if (OnBonusGhost == null)
            OnBonusGhost = new UnityEvent<float>();
        if (OnMalusScreen == null)
            OnMalusScreen = new UnityEvent<float>();

        // Ajouts des méthodes aux évènements
        OnBonusGhost.AddListener(BonusGhost);
        OnMalusScreen.AddListener(MalusScreen);

        // Sauvegarde de la couche et de la couleur d'origine de la voiture
        originalLayer = car.layer;
        originalColor = carRenderer.material.color;
    }

    /// <summary>
    /// Applique le malus de vision à l'écran pendant une durée spécifiée.
    /// <para>
    /// Active un effet visuel (par exemple, une réduction de la vision) et le désactive après un certain délai.
    /// </para>
    /// </summary>
    /// <param name="duration">Durée pendant laquelle le malus est appliqué.</param>
    public void MalusScreen(float duration)
    {
        if (MalusVision != null)
        {
            MalusVision.SetActive(true); // Active l'effet de malus de vision.
        }
        StartCoroutine(RemoveEffectAfterTime(duration)); // Démarre une coroutine pour supprimer l'effet après un délai.
    }

    /// <summary>
    /// Coroutine pour supprimer l'effet de malus après une durée spécifiée.
    /// </summary>
    /// <param name="duration">Durée d'attente avant de supprimer l'effet.</param>
    /// <returns>System.Collections.IEnumerator</returns>
    private System.Collections.IEnumerator RemoveEffectAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration); // Attend la durée spécifiée.
        if (MalusVision != null)
        {
            MalusVision.SetActive(false); // Désactive l'effet de malus de vision.
        }
    }

    /// <summary>
    /// Applique l'effet bonus "fantôme" à la voiture pendant une durée spécifiée.
    /// <para>
    /// Rend la voiture intangible (pas de collision) et change sa couleur en bleu pendant un certain temps.
    /// </para>
    /// </summary>
    /// <param name="duration">Durée pendant laquelle l'effet bonus est appliqué.</param>
    public void BonusGhost(float duration)
    {
        if (carRenderer != null && carCollider != null)
        {
            StartCoroutine(ApplyGhostEffect(duration)); // Démarre une coroutine pour appliquer l'effet fantôme.
        }
    }

    /// <summary>
    /// Coroutine pour appliquer l'effet "fantôme" à la voiture.
    /// <para>
    /// Change la couche de la voiture pour qu'elle ne rentre pas en collision et la rend semi-transparente.
    /// </para>
    /// </summary>
    /// <param name="duration">Durée de l'effet fantôme.</param>
    /// <returns></returns>
    private System.Collections.IEnumerator ApplyGhostEffect(float duration)
    {
        // Change la couche de la voiture pour qu'elle ne génère pas de collisions.
        SetLayerRecursively(car, LayerMask.NameToLayer("NoCollisionLayer"));

        // Change la couleur de la voiture pour qu'elle devienne semi-transparente (bleue).
        carRenderer.material.color = Color.blue;

        yield return new WaitForSeconds(duration); // Attend la durée de l'effet.

        // Restaure la couche et la couleur d'origine de la voiture.
        SetLayerRecursively(car, originalLayer);
        carRenderer.material.color = originalColor;
    }

    /// <summary>
    /// Change la couche de l'objet et de ses enfants récursivement.
    /// <para>
    /// Utilisé pour appliquer les effets de collision (comme l'effet fantôme).
    /// </para>
    /// </summary>
    /// <param name="obj">L'objet dont la couche doit être modifiée.</param>
    /// <param name="newLayer">La nouvelle couche à appliquer.</param>
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer; // Change la couche de l'objet.
        foreach (Transform child in obj.transform) // Applique la même modification aux enfants.
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
