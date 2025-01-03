using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public int nbPatternsInitRoad = 20;
    public Transform posInitPattern;
    [Tooltip("Distance au player au-delà de laquelle un pattern procédural est supprimé")]
    public Transform trsfParentRoad = null;
    private Vector3 lastPosPattern;

    [Header("Obstacles")]
    [Tooltip("La probabilité d'avoir un obstacle lorsqu'un pattern de route est créé")]
    [Range(0.0f, 1.0f)]
    public float probaAllObst;

    [System.Serializable]
    public class ProbaObst
    {
        public float proba; // Probabilité d'avoir cet objet
        public GameObject obst; // L'objet obstacle
    }

    [Header("Paramètres des obstacles")]
    [Tooltip("Liste des obstacles avec leur probabilité d'apparition associé")]
    public ProbaObst[] probaObsts;
    private float totalProbaObst;
    private Vector3 dimObst;

    [Space(20)]
    [Header("Difficulty")]
    [Tooltip("La difficulté (d'avoir un obstacle) augmente toutes les ... secondes")]
    [Range(5, 30)]
    public float increasePeriod = 10.0f;

    [Header("Outils de déboguage")]
    [Tooltip("Booléan empêchant la génération d'obstacles")]
    public bool isDebug = false;

    private const float COEFF_INCREASE_DIFF = 1.05f;
    private BuildInitRoad ir;
    private float coeff;

    // Environnement
    private GameObject SkyDome;
    private GameObject MountainSkybox;

    // Liste des obstacles
    private List<GameObject> obstacleList = new List<GameObject>();

    // Difficulté du niveau
    private Difficulty activeDifficulty;

    // UI : Texte pour affichage des mètres parcourus
    public TMP_Text TMP_Text_Meters;

    // Variables pour la distance
    private float startPlayerPosition;

    private float lastUpdateDistance = 0.0f;

    void Awake()
    {
        SkyDome = GameObject.Find("SkyDome");
        MountainSkybox = GameObject.Find("MountainSkybox");

        ir = new BuildInitRoad(posInitPattern, pattern, nbPatternsInitRoad, out lastPosPattern, trsfParentRoad);
        dimPattern = pattern.GetComponent<Renderer>().bounds.size;
        player.position += new Vector3(0, 0, 10);
        lastPosPlayer = player.position.z;

        startPlayerPosition = lastPosPlayer;

        totalProbaObst = 0.0f;
        float previousProb = 0;
        foreach (ProbaObst p in probaObsts)
        {
            totalProbaObst += p.proba;
            p.proba += previousProb;
            previousProb = p.proba;
        }

        foreach (ProbaObst p in probaObsts)
            p.proba /= totalProbaObst;

        InvokeRepeating(nameof(UpdateDifficulty), increasePeriod, increasePeriod);

        activeDifficulty = Instantiate(MenuManager.activeDifficulty);

        if (activeDifficulty != null && activeDifficulty.GetOptionsEvolutionDifficulte() == Difficulty.OptionsEvolutionDifficulte.Temps)
        {
            Debug.Log("Mode d'évolution des paramètres de difficultés : Temps");
            InvokeRepeating(nameof(UpdateDifficultyLevel), activeDifficulty.toutesLesNbSecondes, activeDifficulty.toutesLesNbSecondes);
        } else
        {
            Debug.Log("Mode d'évolution des paramètres de difficultés : Mètres");
        }

        coeff = 0.0f;
    }

    void Update()
    {
        // Calcul de la distance parcourue
        GlobalVariables.distanceParcourue = player.position.z - startPlayerPosition;

        // Vérifier si la difficulté doit être mise à jour (en fonction de la distance)
        if (activeDifficulty.GetOptionsEvolutionDifficulte() == Difficulty.OptionsEvolutionDifficulte.Distance
            && GlobalVariables.distanceParcourue >= lastUpdateDistance + activeDifficulty.toutesLesNbMetres)
        {
            UpdateDifficultyLevel();
            lastUpdateDistance += activeDifficulty.toutesLesNbMetres;
        }

        if ((player.position.z - lastPosPlayer) > dimPattern.x)
        {
            lastPosPlayer = player.position.z;
        }

        if (lastPosPattern.z - lastPosPlayer <= dimPattern.x * 30)
        {
            GameObject newPattern = ir.AddPatternRoad(pattern, ref lastPosPattern, "patternRd" + nbPatternsInitRoad++, trsfParentRoad);

            SkyDome.transform.position += Vector3.forward * dimPattern.x;
            MountainSkybox.transform.position += Vector3.forward * dimPattern.x;

            if (!isDebug)
            {
                float randValue = Random.value;
                foreach (ProbaObst obstacleData in probaObsts)
                {
                    if (randValue <= obstacleData.proba)
                    {
                        Vector3 position = newPattern.transform.position;

                        float offsetX = Random.Range(-dimPattern.x / 2 - 5f, dimPattern.x / 2 + 5f);
                        float offsetZ = Random.Range(-dimPattern.z / 2 - 5f, dimPattern.z / 2 + 5f);

                        Vector3 obstaclePosition = position + new Vector3(offsetX, obstacleData.obst.transform.position.y, offsetZ);

                        bool isAllowed = true;
                        foreach (GameObject o in obstacleList)
                        {
                            if (o == null) continue;

                            Renderer renderer = GetRenderer(o);
                            Renderer obstacleRenderer = GetRenderer(obstacleData.obst);

                            if (renderer != null && obstacleRenderer != null)
                            {
                                float minDistance = Mathf.Max(obstacleRenderer.bounds.size.x, obstacleRenderer.bounds.size.z) * 2;
                                int generateNumberForKnowIfGenerationOfObstaclesIsPossibleForDifficultyLevel = Random.Range(1, 101);
                                if (Vector3.Distance(obstaclePosition, o.transform.position) <= minDistance * activeDifficulty.offsetObstacle || generateNumberForKnowIfGenerationOfObstaclesIsPossibleForDifficultyLevel >= activeDifficulty.probaGeneration)
                                {
                                    isAllowed = false;
                                    break;
                                }
                            }
                        }

                        if (isAllowed)
                        {
                            // Instancier l'obstacle à la position calculée
                            GameObject obstacle = Instantiate(obstacleData.obst, obstaclePosition, obstacleData.obst.transform.rotation, newPattern.transform);
                            obstacleList.Add(obstacle);
                        }
                        break;
                    }
                }
            } 

            if (ir.roads.Count > 50)
            {
                GameObject roadToDestroy = ir.roads[0];
                ir.roads.RemoveAt(0);
                Destroy(roadToDestroy);
            }
        }
        UpdateTextMeters();
    }

    private void UpdateDifficulty()
    {
        probaAllObst *= COEFF_INCREASE_DIFF;
        increasePeriod *= COEFF_INCREASE_DIFF;
    }

    private void UpdateDifficultyLevel()
    {
        activeDifficulty.offsetObstacle *= 1 - (activeDifficulty.pourcentageReductionOffset/100);
        activeDifficulty.probaGeneration *= 1 + (activeDifficulty.pourcentageAugmentationProbaGenration / 100);

        Debug.Log($"Mise à jour des paramètres de difficulté : Offset : {activeDifficulty.offsetObstacle}, Proba : {activeDifficulty.probaGeneration} ");
    } 

    private Renderer GetRenderer(GameObject obj)
    {
        LODGroup lodGroup = obj.GetComponent<LODGroup>();
        if (lodGroup != null)
        {
            LOD[] lods = lodGroup.GetLODs();
            if (lods.Length > 0 && lods[0].renderers.Length > 0)
                return lods[0].renderers[0];
        }
        return obj.GetComponent<Renderer>();
    }

    private void UpdateTextMeters()
    {
        if (TMP_Text_Meters != null)
        {
            // Formater l'affichage pour avoir deux chiffres pour les secondes et millisecondes
            TMP_Text_Meters.text = (Mathf.Abs(GlobalVariables.distanceParcourue)).ToString("F2") + " m";
        }
    }
}

public class BuildInitRoad
{
    public List<GameObject> roads = new List<GameObject>();

    public BuildInitRoad(Transform posInit, GameObject pattern, int nbPatterns, out Vector3 lastPos, Transform trsfParentRoad)
    {
        lastPos = posInit.position;
        for (int i = 0; i < nbPatterns; i++)
            AddPatternRoad(pattern, ref lastPos, "patternRd" + i, trsfParentRoad);
    }

    public GameObject AddPatternRoad(GameObject pattern, ref Vector3 pos, string name = "patternRd", Transform trsfParentRoad = null)
    {
        GameObject obj = GameObject.Instantiate(pattern, pos, Quaternion.Euler(0, 90, 0), trsfParentRoad);
        obj.name = name;
        pos += Vector3.forward * pattern.GetComponent<Renderer>().bounds.size.x;
        roads.Add(obj);
        return obj;
    }
}
