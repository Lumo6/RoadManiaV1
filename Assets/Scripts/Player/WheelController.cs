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
