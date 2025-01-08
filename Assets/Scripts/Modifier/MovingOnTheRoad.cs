using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe MovingObstacle
/// <para>
/// Classe représentant un obstacle mobile qui se déplace aléatoirement sur la largeur d'une route.
/// </para>
/// </summary>
public class MovingObstacle : MonoBehaviour
{
    private const float VITESSE_MIN = 5;
    private const float VITESSE_MAX = 15;
    private bool VITESSE_ALEATOIRE = false;

    [Tooltip("Vitesse de deplacement de l'objet sur la largeur de la route \n " +
             "si nulle, elle sera aléatoire à chaque nouveau déplacement \n")]
    public float speed = VITESSE_MIN;

#if UNITY_EDITOR
    void OnValidate()
    {
        if (speed == 0) VITESSE_ALEATOIRE = true;
        else
        {
            VITESSE_ALEATOIRE = false;
            if (speed < VITESSE_MIN) speed = VITESSE_MIN;
            if (speed > VITESSE_MAX) speed = VITESSE_MAX;
        }
    }
#endif

    [Header("La route")]
    public GameObject road;
    private float p_roadWidth;

    private Vector3 p_posTarget;
    private const float SEUIL_DISTANCE_NEGLIGEABLE = 0.1f;

    private bool isMoving = false;  // Contrôle si l'obstacle se déplace

    [Header("Paramètres d'importance")]
    [Tooltip("Seuil de taille à partir duquel l'obstacle est considéré comme important à l'écran (entre 0 et 1)")]
    public float importanceThreshold = 0.74f;  // Seuil de taille minimum à l'écran pour activer l'animation

    void Start()
    {
        // Calcul de la largeur de la route pour le déplacement
        p_roadWidth = road.GetComponent<Renderer>().bounds.size.x / 2;
        p_roadWidth -= this.GetComponent<Renderer>().bounds.size.x / 2;

        // Position initiale de l'obstacle
        p_posTarget = transform.position;
        p_posTarget.x = road.transform.position.x + Random.Range(-p_roadWidth, +p_roadWidth);

        // Initialisation de la vitesse
        if (VITESSE_ALEATOIRE)
            speed = Random.Range(VITESSE_MIN, VITESSE_MAX);

        // Si l'objet n'est pas à la verticale de la route, le repositionner
        if ((transform.position.x > road.transform.position.x + p_roadWidth) || (transform.position.x < road.transform.position.x - p_roadWidth))
            transform.position = new Vector3(road.transform.position.x + Random.Range(-p_roadWidth, +p_roadWidth), transform.position.y, transform.position.z);
    }

    void Update()
    {
        // Calculer la position de l'objet dans le viewport de la caméra
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        // Vérifier si l'objet est dans le champ de vision de la caméra
        bool isVisible = viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1;

        // Importance de l'objet à l'écran
        float importance = Mathf.Max(viewportPos.x, viewportPos.y);

        // Vérifier si l'objet est suffisamment important à l'écran
        bool isImportant = importance <= importanceThreshold;

        // Si l'importance est supérieur ou égale à 1 alors cela veut dire que l'objet est derrière la caméra
        if (importance >= 1.0f) Destroy(gameObject);

        // Si l'objet est visible et important, activer son comportement (ici déplacement et animation)
        if (isVisible && isImportant)
        {
            // Activer le mouvement si nécessaire
            if (!isMoving)
            {
                isMoving = true;
            }

            // Calcul de l'étape du déplacement
            float step = speed * Time.deltaTime;

            // Déplacer l'obstacle vers la position cible
            transform.position = Vector3.MoveTowards(transform.position, p_posTarget, step);

            // Si l'obstacle a atteint sa position cible, planifier un nouveau déplacement
            if (Mathf.Abs(transform.position.x - p_posTarget.x) < SEUIL_DISTANCE_NEGLIGEABLE)
            {
                p_posTarget.x = road.transform.position.x + Random.Range(-p_roadWidth, +p_roadWidth);
                if (VITESSE_ALEATOIRE)
                    speed = Random.Range(VITESSE_MIN, VITESSE_MAX);
            }
        }
        else
        {
            // Si l'objet n'est plus visible ou important, arrêter le mouvement ou l'animation
            if (isMoving)
            {
                isMoving = false;  // Arrêter le mouvement (ou animation ici)
                // Exemple : Arrêter l'animation si tu en as une
                // GetComponent<Animator>().SetTrigger("StopMoving");
            }
        }
    }
}
