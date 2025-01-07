using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRecorder : MonoBehaviour
{
    private GhostData ghostData = new GhostData();
    private float startTime;
    private Transform carTransform;

    void Start()
    {
        if (MenuManager.activeDifficulty.difficultyLevel != 3)
        {
            this.enabled = false;
        } else
        {
            Debug.Log("Début de l'enregistrement");
        }

        startTime = Time.time;
        carTransform = gameObject.GetComponent<Renderer>().transform;
    }

    void FixedUpdate()
    {
        ghostData.positions.Add(carTransform.position);
        ghostData.rotations.Add(carTransform.rotation);
        ghostData.timestamps.Add(Time.time - startTime);

        Debug.Log("Nouvelle position enregistrée");
    }

    public GhostData GetRecordedData()
    {
        return ghostData;
    }
}
