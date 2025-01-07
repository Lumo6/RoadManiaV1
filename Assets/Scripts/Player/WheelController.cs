using UnityEngine;

public class WheelController : MonoBehaviour
{
    public Transform frontLeftWheel;
    public Transform frontRightWheel;
    public Transform rearLeftWheel;
    public Transform rearRightWheel;

    public Rigidbody rb;

    public ParticleSystem brakeParticlesFrontLeft;
    public ParticleSystem brakeParticlesFrontRight;
    public ParticleSystem brakeParticlesRearLeft;
    public ParticleSystem brakeParticlesRearRight;

    public AudioClip brakeSound;
    public AudioSource sourceSound;

    void Awake() {
        sourceSound.volume = GlobalVariables.soundLevel;
    }

    void Update()
    {
        float speed = rb.velocity.magnitude;

        float steerInput = Input.GetAxis("Horizontal");
        frontLeftWheel.localRotation = Quaternion.Euler(0f, steerInput * 30f, 0f);
        frontRightWheel.localRotation = Quaternion.Euler(0f, steerInput * 30f, 0f);

        bool isBraking = Input.GetKey(KeyCode.S);

        UpdateParticleEffect(isBraking, frontLeftWheel, brakeParticlesFrontLeft);
        UpdateParticleEffect(isBraking, frontRightWheel, brakeParticlesFrontRight);
        UpdateParticleEffect(isBraking, rearLeftWheel, brakeParticlesRearLeft);
        UpdateParticleEffect(isBraking, rearRightWheel, brakeParticlesRearRight);
    }

    void UpdateParticleEffect(bool isBraking, Transform wheel, ParticleSystem brakeParticles)
    {
        if (isBraking)
        {
            if (!brakeParticles.isPlaying)
            {
                brakeParticles.Play();
            }
            brakeParticles.transform.position = wheel.position;

            if (!sourceSound.isPlaying)
            {
                sourceSound.PlayOneShot(brakeSound);
            }
        }
        else
        {
            if (brakeParticles.isPlaying)
            {
                brakeParticles.Stop();
            }

            if (sourceSound.isPlaying)
            {
                sourceSound.Stop();
            }
        }
    }
}
