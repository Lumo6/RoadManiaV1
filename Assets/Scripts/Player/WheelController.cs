using UnityEngine;

public class WheelController : MonoBehaviour
{
    public GameObject frontLeftWheel;
    public GameObject frontRightWheel;
    public GameObject rearLeftWheel;
    public GameObject rearRightWheel;

    public Rigidbody rb;

    public ParticleSystem brakeParticlesFrontLeft;
    public ParticleSystem brakeParticlesFrontRight;
    public ParticleSystem brakeParticlesRearLeft;
    public ParticleSystem brakeParticlesRearRight;

    public AudioClip brakeSound;
    public AudioSource sourceSound;

    private float frontLeftRadius;
    private float frontRightRadius;
    private float rearLeftRadius;
    private float rearRightRadius;

    void Awake()
    {
        sourceSound.volume = GlobalVariables.soundLevel;

        // Calculer les rayons des roues dynamiquement
        frontLeftRadius = GetWheelRadius(frontLeftWheel);
        frontRightRadius = GetWheelRadius(frontRightWheel);
        rearLeftRadius = GetWheelRadius(rearLeftWheel);
        rearRightRadius = GetWheelRadius(rearRightWheel);
    }

    void Update()
    {
        float speed = rb.velocity.magnitude;

        // Appliquer la rotation des roues
        RotateWheels(speed);

        // Tourner les roues avant
        float steerInput = Input.GetAxis("Horizontal");
        SteerWheels(steerInput);

        // Vérifier si le véhicule freine
        bool isBraking = Input.GetKey(KeyCode.S);
        ApplyBrakeEffects(isBraking);
    }

    float GetWheelRadius(GameObject wheel)
    {
        MeshRenderer renderer = wheel.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            return renderer.bounds.extents.y;
        }

        Debug.LogWarning($"Impossible de déterminer le rayon de la roue {wheel.name}. Valeur par défaut utilisée.");
        return 0.33f; // Valeur par défaut si aucun composant valide n'est trouvé
    }

    void RotateWheels(float speed)
    {
        // Calculer la vitesse angulaire (omega) pour chaque roue
        float frontLeftRotationSpeed = speed * 360f / (2f * Mathf.PI * frontLeftRadius);
        float frontRightRotationSpeed = speed * 360f / (2f * Mathf.PI * frontRightRadius);
        float rearLeftRotationSpeed = speed * 360f / (2f * Mathf.PI * rearLeftRadius);
        float rearRightRotationSpeed = speed * 360f / (2f * Mathf.PI * rearRightRadius);

        // Appliquer la rotation autour de l'axe X
        frontLeftWheel.transform.Rotate(frontLeftRotationSpeed * Time.deltaTime, 0f, 0f);
        frontRightWheel.transform.Rotate(frontRightRotationSpeed * Time.deltaTime, 0f, 0f);
        rearLeftWheel.transform.Rotate(rearLeftRotationSpeed * Time.deltaTime, 0f, 0f);
        rearRightWheel.transform.Rotate(rearRightRotationSpeed * Time.deltaTime, 0f, 0f);
    }

    void SteerWheels(float steerInput)
    {
        float steerAngle = steerInput * 30f; // Limiter l'angle de braquage à 30°
        frontLeftWheel.transform.localRotation = Quaternion.Euler(0f, steerAngle, 0f);
        frontRightWheel.transform.localRotation = Quaternion.Euler(0f, steerAngle, 0f);
    }

    void ApplyBrakeEffects(bool isBraking)
    {
        UpdateParticleEffect(isBraking, frontLeftWheel, brakeParticlesFrontLeft);
        UpdateParticleEffect(isBraking, frontRightWheel, brakeParticlesFrontRight);
        UpdateParticleEffect(isBraking, rearLeftWheel, brakeParticlesRearLeft);
        UpdateParticleEffect(isBraking, rearRightWheel, brakeParticlesRearRight);

        if (isBraking && !sourceSound.isPlaying)
        {
            sourceSound.PlayOneShot(brakeSound);
        }
        else if (!isBraking && sourceSound.isPlaying)
        {
            sourceSound.Stop();
        }
    }

    void UpdateParticleEffect(bool isBraking, GameObject wheel, ParticleSystem brakeParticles)
    {
        if (isBraking)
        {
            if (!brakeParticles.isPlaying)
            {
                brakeParticles.Play();
            }
            brakeParticles.transform.position = wheel.transform.position;
        }
        else
        {
            if (brakeParticles.isPlaying)
            {
                brakeParticles.Stop();
            }
        }
    }
}
