using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// Classe GhostData
/// </para>
/// Représente les données enregistrées pour un "fantôme", utilisé pour rejouer ou visualiser une trajectoire.
/// Elle contient les positions, rotations et timestamps associés à chaque étape du mouvement.
/// </summary>
[System.Serializable]
public class GhostData
{
    /// <summary>
    /// Liste des positions enregistrées (Vector3) du fantôme.
    /// Chaque élément correspond à une position dans l'espace à un instant donné.
    /// </summary>
    public List<Vector3> positions = new List<Vector3>();

    /// <summary>
    /// Liste des rotations enregistrées (Quaternion) du fantôme.
    /// Chaque élément correspond à une orientation à un instant donné.
    /// </summary>
    public List<Quaternion> rotations = new List<Quaternion>();

    /// <summary>
    /// Liste des timestamps (en secondes) associés aux positions et rotations.
    /// Chaque élément correspond au temps écoulé depuis le début de l'enregistrement.
    /// </summary>
    public List<float> timestamps = new List<float>();
}
