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

    // tooltip permet un affichage contextuel dans l'IDE
    [Tooltip("Vitesse de deplacement de l'objet sur la largeur de la route \n " +
             "si nulle, elle sera aléatoire à chaque nouveau déplacement \n")]
    public float speed = VITESSE_MIN;
#if UNITY_EDITOR
    void OnValidate()       // une facon de tester et initialiser les variables d'entrées
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
    // on passe la route pour la robustesse
    // on evite les constantes numériques (e.g. 10 pour la largeur de la route, ..)
    // qu'il est difficile de retrouver et comprendre pour la maintenance et l'équilibrage
    // Ce programme  restera efficace quelque soit l'objet route utilisé (e.g. si la ressources route est amenée à changer )

    private float p_roadWidth;

    private Vector3 p_posTarget;
    private const float SEUIL_DISTANCE_NEGLIGEABLE = 0.1f;

    void Start()
    {
        // largeur de la route  : le programme s'adapte à l'objet route utilisé
        p_roadWidth =   road.GetComponent<Renderer>().bounds.size.x / 2;
        p_roadWidth -=  this.GetComponent<Renderer>().bounds.size.x / 2;
        // calcul de la position cible alétaoire où va se dépacer l'objet
        p_posTarget = transform.position;         // on suppose que l'obstacle est intialement bien positionné sur la route
        p_posTarget.x = road.transform.position.x + Random.Range(-p_roadWidth, +p_roadWidth);
        // intialise si besoin la vitesse
        if (VITESSE_ALEATOIRE)
            speed = Random.Range(VITESSE_MIN, VITESSE_MAX);

        // TEST pour la robustesse
        // SI l'objet n'est pas à la verticale de la route
        // ALORS il est positionné aléatoirement sur la largeur de la route
        if ((transform.position.x > road.transform.position.x + p_roadWidth) || (transform.position.x < road.transform.position.x - p_roadWidth))
            transform.position = new Vector3(road.transform.position.x + Random.Range(-p_roadWidth, +p_roadWidth), transform.position.y, transform.position.z);
    }

    void Update()
    {
        float step = speed * Time.deltaTime;

        // faire un pas vers la direction de la position cible
        transform.position = Vector3.MoveTowards(transform.position, p_posTarget, step);

        // SI le déplacement est terminé (cible atteinte transform.position = p_posTarget)  ,
            //if (transform.position == p_posTarget) // pour la robustesse : on ne peut pas tester une égalité entre les 2 vector3 de float !!
            //if (Vector3.Distance(transform.position ,p_posTarget) < SEUIL_DISTANCE_NEGLIGEABLE )  // OK mais couteux : distance : racine carrée et puissance 2 de float
        if (Mathf.Abs(transform.position.x  -  p_posTarget.x) < SEUIL_DISTANCE_NEGLIGEABLE)     // suffisant ici : abs et soustraction => opti
        // ALORS un nouveau déplacement est planifié
                {
                // calcul de la nouvelle position cible alétaoire où va se dépacer l'objet
                p_posTarget.x = road.transform.position.x + Random.Range(-p_roadWidth, +p_roadWidth);
                // intialise si besoin la nouvelle vitesse
                if (VITESSE_ALEATOIRE)
                    speed = Random.Range(VITESSE_MIN, VITESSE_MAX);
                }
    }
}
