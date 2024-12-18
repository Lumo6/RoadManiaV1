using System.Collections.Generic;
using UnityEngine;
public class CreateRealTimeRoad : MonoBehaviour
{
    [Header("Player")]
    public Transform player;
    private float lastPosPlayer;
    private Vector3 dimPattern;
    [Space(20)]

    [Header("Road & Patterns")]
    public GameObject pattern;
    [Range(10, 30)]
    public int nbPatternsInitRoad = 20; // valeur par défaut
    public Transform posInitPattern;
    [Tooltip("Distance au player au dela de laquelle un pattern procédural est supprimé")]
    public float distDestructionPattern;
    public Transform trsfParentRoad = null;
    private Vector3 lastPosPattern;


    [Header("Obstacles")]
    [Tooltip("LA probabilité d'avoir un obsctacle lorsqu'un pattern de route est créé")]
    [Range(0.0f, 1.0f)]
    public float probaAllObst;
    [System.Serializable]

    public class probaObst
    {
        public float proba;
        // probabilité d'avoir cet objet quand il faut ajouter un obstacle
        public GameObject obst;
    }
    [Space(20)]
    public probaObst[] probaObsts;
    // tous les obstacles possibles et leur probabilité (relative)
    private float totalProbaObst;
    private Vector3 dimObst;
    [Space(20)]
    [Header("difficulty")]
    [Tooltip("la difficulté (d'avoir un ostacle ) augmente toutes les ... secondes ")]
    [Range(5, 30)]
    public float increasePeriod = 10;
    private const float COEFF_INCREASE_DIFF = 1.05f;
    // une constante : augmentation de 5% de la difficulté à chaque période
    private BuildInitRoad ir; // la route initiale
    private float coeff;// Creer la route initale dans le AWAKE, première fonction appelée

    private GameObject SkyDome;
    private GameObject MountainSkybox;

    private List<GameObject> obstacleList = new List<GameObject>();

    public bool isDebug = false;

    void Awake()
    {
        SkyDome = GameObject.Find("SkyDome");
        MountainSkybox = GameObject.Find("MountainSkybox");

        ir = new BuildInitRoad(posInitPattern, pattern, nbPatternsInitRoad, out lastPosPattern, trsfParentRoad);
        dimPattern = pattern.GetComponent<Renderer>().bounds.size;
        player.position += new Vector3(0, 0, 10);
        lastPosPlayer = player.position.z;

        // modification du tableau de proba pour simplifier les calculs dans le update
        // e.g. 10 , 10, 30, 150 devient 10 , 20, 50 , 200
        totalProbaObst = 0.0f;
        float previousProb = 0;
        foreach (probaObst p in probaObsts)
        {
            totalProbaObst += p.proba;
            p.proba += previousProb;
            previousProb = p.proba;
        }
        // puis 10 , 20, 50 , 200 devient 0.05 , 0.10, 0.25,1
        foreach (probaObst p in probaObsts)
            p.proba /= totalProbaObst;
        InvokeRepeating("UpdateDifficulty", increasePeriod, increasePeriod);
        coeff = 0.0f;
    }

    // vérifier à chaque frame , en fonction de l'avancement du véhicule player …
    // …. s'il est nécessaire une ou plusieurs portions de route
    // pour chaque portion de route créée , selon tirage aléatoire et probabilité …
    // … créer ou non un obstacle
    // pour chaque obstacle à créer , tirage aléatoire pour savoir lequel choisir en fonction …
    // … des proba fournies pour chacun d'eux
    void Update()
    {
        if ((player.position.z - lastPosPlayer) > dimPattern.x)
        {
            lastPosPlayer = player.position.z;
        }

        if (lastPosPattern.z - lastPosPlayer <= dimPattern.x * 10)
        {
            GameObject newPattern = ir.AddPatternRoad(pattern, ref lastPosPattern, "patternRd" + nbPatternsInitRoad++, trsfParentRoad);

            SkyDome.transform.position = new Vector3(
                SkyDome.transform.position.x,
                SkyDome.transform.position.y,
                SkyDome.transform.position.z + dimPattern.x
            );

            MountainSkybox.transform.position = new Vector3(
                MountainSkybox.transform.position.x,
                MountainSkybox.transform.position.y,
                MountainSkybox.transform.position.z + dimPattern.x
            );
            float randValue = Random.value;
            foreach (probaObst obstacleData in probaObsts)
            {
                if (randValue <= obstacleData.proba)
                {
                    Vector3 position = newPattern.transform.position;
                    Vector3 obstaclePosition = position + new Vector3(
                        Random.Range(-dimPattern.x, dimPattern.x),
                        obstacleData.obst.transform.position.y,
                        Random.Range(-dimPattern.z, dimPattern.z)
                    );

                    bool isAllowed = true;
                    foreach (GameObject o in obstacleList)
                    {
                        if (o == null) continue;
                        if (Vector3.Distance(obstaclePosition, o.transform.position) <= Mathf.Max(obstacleData.obst.GetComponent<Renderer>().bounds.size.x, obstacleData.obst.GetComponent<Renderer>().bounds.size.z) * 2)
                        {
                            isAllowed = false;
                            break;
                        }
                    }
                    if (isAllowed)
                    {
                        if (!isDebug)
                        {
                            GameObject obstacle = Instantiate(obstacleData.obst, obstaclePosition, obstacleData.obst.transform.rotation, newPattern.transform);
                            obstacleList.Add(obstacle);
                        }
                    }
                    break;
                }
            }

            if (ir.roads.Count > 0)
            {
                GameObject roadToDestroy = ir.roads[0];
                ir.roads.RemoveAt(0);
                Destroy(roadToDestroy);
            }
        }

    }
    private void UpdateDifficulty()
    {
        probaAllObst *= COEFF_INCREASE_DIFF;
        increasePeriod *= COEFF_INCREASE_DIFF;
    }
}

public class BuildInitRoad
{
    public List<GameObject> roads = new List<GameObject>();

    /* c'est une classe qui ne derive pas de : MonoBehaviour
    * elle ne peut donc pas être associée à un GameObject
    * elle ne recoit pas non plus des appels à Update (etc.) du moteur d'unity
    *
    * Cette classe va créeer des GameObject dans la scène (les uns à côté des autres)
    * et les associer à un gameObject parent
    */

    /* c'est le consctructeur de la classe qui va créer les objets Exercice F
    * pour l'appeler :
    * BuildInitRoad ir = new BuildInitRoad(posInit, pattern, nbpatterns, out lastPos, trsfParentRoad);
    */
    public BuildInitRoad(Transform posInit,
   GameObject pattern,
   int nbPatterns,
   out Vector3 lastPos,
   Transform trsfParentRoad)
    {
        lastPos = posInit.position;
        for (int i = 0; i < nbPatterns; i++)
            AddPatternRoad(pattern, ref lastPos, "patternRd" + i, trsfParentRoad);
    }
    // appelé par le constructeur de la classe pour construire la route initiale (ci dessus)
    // appelé également lors de l'ajout de pattern de route lors de la construction dynamique
    // en temps réel de la route au fur et a mesure de l'avancement du player : appelé par
    // la methode update de la classe CreateRealTimeRoad de l'exercice G
    public GameObject AddPatternRoad(GameObject pattern,
   ref Vector3 pos,
   string name = "patternRd",
   Transform trsfParentRoad = null)
    {
        GameObject obj = GameObject.Instantiate(pattern, pos, Quaternion.Euler(0, 90, 0),
       trsfParentRoad);
        obj.name = name;
        pos += Vector3.forward * pattern.GetComponent<Renderer>().bounds.size.x;
        roads.Add(obj);
        return obj;
    }
}