using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Classe CreateRealTimeRoad
/// <para>
/// Classe permettant la génération de la route avec des obstacles.
/// </para>
/// </summary>
public class CreateRealTimeRoad : MonoBehaviour
{
    [Header("Player")]
    public Transform player; // Joueur
    private float lastPosPlayer; // Dernière position du joueur
    private Vector3 dimPattern; // Taille du pattern de la route

    [Space(20)]
    [Header("Road & Patterns")]
    public GameObject pattern; // Pattern de la route
    [Range(10, 30)]
    public int nbPatternsInitRoad = 20; // Nombre initial de pattern lors du lancement du jeu
    public Transform posInitPattern; // Position initial du premier pattern
    [Tooltip("Distance au player au-delà de laquelle un pattern procédural est supprimé")]
    public Transform trsfParentRoad = null; // Transform permettant de mettre les routes en sous objets d'un objet parent regroupant les patterns de route. Ce transform est un objet "englobant"
    private Vector3 lastPosPattern; // Dernière position du pattern

    [Header("Obstacles")]
    [Tooltip("La probabilité d'avoir un obstacle lorsqu'un pattern de route est créé")]
    [Range(0.0f, 1.0f)]
    public float probaAllObst; // Liste des probabilités d'apparition des objets

    [System.Serializable]
    /// <summary>
    /// Classe ProbaObst
    /// <para>
    /// Classe qui représente un obstacle avec une probabilité d'apparition.
    /// Elle contient la probabilité associée à cet obstacle ainsi que l'objet lui-même.
    /// </para>
    /// </summary>
    public class ProbaObst
    {
        public float proba; // Probabilité d'avoir cet objet
        public GameObject obst; // L'objet obstacle
    }

    [Header("Paramètres des obstacles")]
    [Tooltip("Liste des obstacles avec leur probabilité d'apparition associé")]
    public ProbaObst[] probaObsts; // Liste des obstacles avec leurs probabilités associées
    private float totalProbaObst; // Total des probas
    private Vector3 dimObst; // Dimensions obstacles

    [Space(20)]
    [Header("Difficulty")]
    [Tooltip("La difficulté (d'avoir un obstacle) augmente toutes les ... secondes")]
    [Range(5, 30)]
    public float increasePeriod = 10.0f; // Tous les ... secondes

    [Header("Outils de déboguage")]
    [Tooltip("Booléan empêchant la génération d'obstacles")]
    public bool isDebug = false; // Empêche la génération des obstacles ou non

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
        // Récupération de l'environnement
        SkyDome = GameObject.Find("SkyDome");
        MountainSkybox = GameObject.Find("MountainSkybox");

        // Création de la route initial
        ir = new BuildInitRoad(posInitPattern, pattern, nbPatternsInitRoad, out lastPosPattern, trsfParentRoad);
        dimPattern = pattern.GetComponent<Renderer>().bounds.size;
        player.position += new Vector3(0, 0, 10);
        lastPosPlayer = player.position.z;

        startPlayerPosition = lastPosPlayer;

        // Mise a jour des probabilités d'apparitions des obstacles
        totalProbaObst = 0.0f;
        float previousProb = 0;
        foreach (ProbaObst p in probaObsts)
        {
            totalProbaObst += p.proba;
            p.proba += previousProb;
            previousProb = p.proba;

            // Pour chaque obstacles, si le mode est Réaliste, on désactive les LODs
            if (GlobalVariables.graphismMode == "Realiste") {
                LODGroup lod = p.obst.GetComponent<LODGroup>();
                if(lod != null) lod.enabled = false;
            }
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

        // Si le joueur se trouve a une certaine distance de la fin de la route, on ajoute une portion
        if (lastPosPattern.z - lastPosPlayer <= dimPattern.x * 30)
        {
            // Ajout d'une portion de route
            GameObject newPattern = ir.AddPatternRoad(pattern, ref lastPosPattern, "patternRd" + nbPatternsInitRoad++, trsfParentRoad);

            // On déplace l'environnement
            SkyDome.transform.position += Vector3.forward * dimPattern.x;
            MountainSkybox.transform.position += Vector3.forward * dimPattern.x;

            if (!isDebug)
            {
                // Valeur aléatoire permettant de savoir si l'obstacles va être ajouté ou non sur la route
                float randValue = Random.value; // Valeur entre 0 et 1
                foreach (ProbaObst obstacleData in probaObsts)
                {
                    if (randValue <= obstacleData.proba)
                    {
                        // Positionnement de l'obstacles
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

                            // Vérification que 2 obstacles ne se superpose pas
                            if (renderer != null && obstacleRenderer != null)
                            {
                                // On récupère la plus grande taille entre les 2 obstacles comparés
                                float minDistance = Mathf.Max(obstacleRenderer.bounds.size.x, obstacleRenderer.bounds.size.z) * 2;
                                // Nombre entre 1 et 100
                                int generateNumberForKnowIfGenerationOfObstaclesIsPossibleForDifficultyLevel = Random.Range(1, 101);

                                // Si la distance entre les objets n'est assez grande par rapport a ce qui est définie dans les paramètres de difficultés
                                // et que le nombre généré est supérieur à la probabilité d'apparition de l'objet (par exemple nombre généré 92 et probaGeneration = 90, alors pas de génération)
                                // on ne permet pas la génération de l'obstacles
                                if (Vector3.Distance(obstaclePosition, o.transform.position) <= minDistance * activeDifficulty.offsetObstacle ||
                                    generateNumberForKnowIfGenerationOfObstaclesIsPossibleForDifficultyLevel >= activeDifficulty.probaGeneration)
                                {
                                    isAllowed = false;
                                    break;
                                }
                            }
                        }

                        // Vérification pour savoir si l'obstacle est autorisé à être généré
                        if (isAllowed)
                        {
                            // Instantiate the obstacle and attach appropriate script
                            GameObject obstacle = Instantiate(obstacleData.obst, obstaclePosition, obstacleData.obst.transform.rotation, GameObject.Find("Obstacles").transform);
                            obstacleList.Add(obstacle);
                        }
                        break;
                    }
                }
            }

            // Si plus de 50 portions de routes, alors on supprime la première portion dans la liste
            if (ir.roads.Count > 50)
            {
                GameObject roadToDestroy = ir.roads[0];
                ir.roads.RemoveAt(0);
                Destroy(roadToDestroy);
            }
        }

        // Mise a jour des infos UI
        UpdateTextMeters();
    }

    /// <summary>
    /// Met à jour la difficulté du jeu en ajustant la probabilité des obstacles
    /// et la période d'augmentation en fonction d'un coefficient.
    /// Cette méthode est appelée pour rendre le jeu plus difficile au fur et à mesure.
    /// </summary>
    private void UpdateDifficulty()
    {
        // Augmente la probabilité de tous les obstacles en multipliant par un coefficient
        probaAllObst *= COEFF_INCREASE_DIFF;

        // Augmente la période d'augmentation des obstacles en multipliant par un coefficient
        increasePeriod *= COEFF_INCREASE_DIFF;
    }

    /// <summary>
    /// Met à jour les paramètres de difficulté du jeu en ajustant l'offset des obstacles
    /// et la probabilité de génération des obstacles en fonction des pourcentages de réduction et d'augmentation.
    /// </summary>
    private void UpdateDifficultyLevel()
    {
        // Mise à jour de l'offset des obstacles en réduisant par un pourcentage
        activeDifficulty.offsetObstacle *= 1 - (activeDifficulty.pourcentageReductionOffset / 100);

        // Mise à jour de la probabilité de génération des obstacles en augmentant par un pourcentage
        activeDifficulty.probaGeneration *= 1 + (activeDifficulty.pourcentageAugmentationProbaGenration / 100);

        // Affichage dans la console des nouveaux paramètres de difficulté
        Debug.Log($"Mise à jour des paramètres de difficulté : Offset : {activeDifficulty.offsetObstacle}, Proba : {activeDifficulty.probaGeneration} ");
    }

    /// <summary>
    /// Récupère le composant Renderer d'un objet, en tenant compte des niveaux de détail (LOD) si présents.
    /// </summary>
    /// <param name="obj">L'objet dont on souhaite récupérer le Renderer.</param>
    /// <returns>Le Renderer du premier niveau de détail (LOD) ou le Renderer direct de l'objet.</returns>
    private Renderer GetRenderer(GameObject obj)
    {
        // Vérifie si l'objet possède un composant LODGroup
        LODGroup lodGroup = obj.GetComponent<LODGroup>();
        if (lodGroup != null)
        {
            // Récupère les niveaux de détail (LOD) et vérifie s'il y a des renderers
            LOD[] lods = lodGroup.GetLODs();
            if (lods.Length > 0 && lods[0].renderers.Length > 0)
                return lods[0].renderers[0]; // Retourne le renderer du premier LOD
        }
        // Si aucun LOD n'est trouvé, retourne le Renderer de l'objet directement
        return obj.GetComponent<Renderer>();
    }

    /// <summary>
    /// Met à jour le texte affichant la distance parcourue en mètres, formaté avec deux décimales.
    /// </summary>
    private void UpdateTextMeters()
    {
        // Vérifie si le texte pour afficher la distance existe
        if (TMP_Text_Meters != null)
        {
            // Formate l'affichage de la distance avec deux chiffres après la virgule et ajoute l'unité "m"
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
        obj.layer = LayerMask.NameToLayer("Road");
        pos += Vector3.forward * pattern.GetComponent<Renderer>().bounds.size.x;
        roads.Add(obj);
        return obj;
    }
}
