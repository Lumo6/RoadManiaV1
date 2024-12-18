using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    public AnimationCurve cameraAnimationCurve;
    
    // Enum pour flexibilité d'ajout de caméra
    private enum CameraView
    {
        FPV, // First Person View
        TPV // Third Person View
    }

    private CameraView cameraView = CameraView.FPV;

    void Start()
    {
        float targetZ;
        switch (cameraView)
        {    
            case CameraView.FPV:
                break;

            case CameraView.TPV:
            default:
                targetZ = player.transform.position.z - 10;
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y,
                    Mathf.SmoothDamp(transform.position.z, targetZ, ref velocity.z, smoothTime)
                );
                break;
            
        }
    }

    void LateUpdate()
    {
        float targetZ = player.transform.position.z - 10;

        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            Mathf.SmoothDamp(transform.position.z, targetZ, ref velocity.z, smoothTime)
        );

        if (Input.GetKey(KeyCode.C))
        {
            
        }
    }
}
