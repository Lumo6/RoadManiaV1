using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    public Transform frontLeftWheel;
    public Transform frontRightWheel;
    public Transform rearLeftWheel;
    public Transform rearRightWheel;

    // Particle Systems for Brake Effect
    public ParticleSystem brakeEffectLeft;
    public ParticleSystem brakeEffectRight;

    // Parameters
    public float wheelRotationSpeed = 360f; // Speed of wheel rotation in degrees per second
    public float maxSteerAngle = 30f; // Maximum steering angle for front wheels

    // Variables
    private float currentSpeed = 0f;
    private float steeringAngle = 0f;

    void Update()
    {
        // Get inputs for speed and steering
        float acceleration = Input.GetAxis("Vertical");
        float steering = Input.GetAxis("Horizontal");

        // Update speed and steering angle
        currentSpeed = acceleration * wheelRotationSpeed * Time.deltaTime;
        steeringAngle = steering * maxSteerAngle;

        // Update wheel rotations and positions
        UpdateWheel(frontLeftWheel, true);
        UpdateWheel(frontRightWheel, true);
        UpdateWheel(rearLeftWheel, false);
        UpdateWheel(rearRightWheel, false);

        // Handle brake effects
        HandleBrakeEffects();
    }

    void UpdateWheel(Transform wheel, bool isFrontWheel)
    {
        // Rotate the wheel around its local X-axis for movement
        wheel.Rotate(Vector3.right, currentSpeed, Space.Self);

        // Apply steering angle to front wheels
        if (isFrontWheel)
        {
            Vector3 localEulerAngles = wheel.localEulerAngles;
            localEulerAngles.y = steeringAngle;
            wheel.localEulerAngles = localEulerAngles;
        }
    }

    void HandleBrakeEffects()
    {
        bool isBraking = Input.GetKey(KeyCode.S);

        // Activate or deactivate brake particles
        if (isBraking)
        {
            if (!brakeEffectLeft.isPlaying) brakeEffectLeft.Play();
            if (!brakeEffectRight.isPlaying) brakeEffectRight.Play();
        }
        else
        {
            if (brakeEffectLeft.isPlaying) brakeEffectLeft.Stop();
            if (brakeEffectRight.isPlaying) brakeEffectRight.Stop();
        }
    }
}
