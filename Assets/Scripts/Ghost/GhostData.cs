using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GhostData
{
    public List<Vector3> positions = new List<Vector3>();
    public List<Quaternion> rotations = new List<Quaternion>();
    public List<float> timestamps = new List<float>();
}