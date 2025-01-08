using UnityEngine;
using Cinemachine;

/// <summary>
/// Classe VirtualCameraSwitcher
/// <para>
/// Classe permettant de basculer entre deux caméras virtuelles en fonction de la priorité.
/// La caméra active est déterminée par la priorité de chaque caméra.
/// </para>
/// </summary>
public class VirtualCameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera1; // Référence à la première caméra virtuelle.

    public CinemachineVirtualCamera virtualCamera2; // Référence à la deuxième caméra virtuelle.

    void Start()
    {
        // Activer la caméra 1 par défaut
        SetActiveCamera(virtualCamera1);
    }

    void Update()
    {
        // Vérifier si la touche C est pressée
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Basculer en fonction de la priorité actuelle
            if (virtualCamera1.Priority > virtualCamera2.Priority)
            {
                SetActiveCamera(virtualCamera2);
            }
            else
            {
                SetActiveCamera(virtualCamera1);
            }
        }
    }

    /// <summary>
    /// Définit la caméra active en fonction de la caméra passée en paramètre.
    /// Ajuste la priorité des caméras pour activer celle souhaitée.
    /// </summary>
    /// <param name="activeCamera">La caméra virtuelle à activer.</param>
    void SetActiveCamera(CinemachineVirtualCamera activeCamera)
    {
        // Réglez les priorités pour activer la caméra souhaitée
        virtualCamera1.Priority = (activeCamera == virtualCamera1) ? 10 : 0;
        virtualCamera2.Priority = (activeCamera == virtualCamera2) ? 10 : 0;
    }
}
