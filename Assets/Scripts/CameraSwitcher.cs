using UnityEngine;
using Cinemachine;
public class VirtualCameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera1;
    public CinemachineVirtualCamera virtualCamera2;

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

    void SetActiveCamera(CinemachineVirtualCamera activeCamera)
    {
        // Réglez les priorités pour activer la caméra souhaitée
        virtualCamera1.Priority = (activeCamera == virtualCamera1) ? 10 : 0;
        virtualCamera2.Priority = (activeCamera == virtualCamera2) ? 10 : 0;
    }
}
