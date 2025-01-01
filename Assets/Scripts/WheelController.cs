using UnityEngine;

public class WheelController : MonoBehaviour
{
    public Transform frontLeftWheel;
    public Transform frontRightWheel;
    public float maxSteerAngle = 30f; // Angle maximum à faible vitesse
    public float minSteerAngle = 5f;  // Angle minimum à haute vitesse
    public float maxSpeed = 100f;     // Vitesse maximale pour normaliser l'effet

    public Rigidbody rb;

    void Start()
    {

    }

    void Update()
    {
        float speed = rb.velocity.magnitude; // Obtenir la vitesse actuelle du véhicule

        // Calculer l'angle de braquage en fonction de la vitesse
        float currentSteerAngle = Mathf.Lerp(maxSteerAngle, minSteerAngle, speed / maxSpeed);

        // Appliquer la rotation des roues avant en fonction de l'entrée du joueur
        float steerInput = Input.GetAxis("Horizontal");
        frontLeftWheel.localRotation = Quaternion.Euler(0f, steerInput * currentSteerAngle, 0f);
        frontRightWheel.localRotation = Quaternion.Euler(0f, steerInput * currentSteerAngle, 0f);
    }
}
