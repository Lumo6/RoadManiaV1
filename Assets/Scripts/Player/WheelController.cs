using UnityEngine;

/// <summary>
/// Classe WheelController
/// <para>
/// Classe qui gère la rotation, la direction et les effets de freinage des roues du véhicule.
/// Elle applique la rotation des roues, la direction des roues avant et les effets de freinage.
/// </para>
/// </summary>
public class WheelController : MonoBehaviour
{
    public GameObject frontLeftWheel; // Référence à la roue avant gauche du véhicule.

    public GameObject frontRightWheel; // Référence à la roue avant droite du véhicule.

    public GameObject rearLeftWheel; // Référence à la roue arrière gauche du véhicule.

    public GameObject rearRightWheel; // Référence à la roue arrière droite du véhicule.

    public Rigidbody rb; // Le Rigidbody du véhicule pour obtenir la vitesse.

    public ParticleSystem brakeParticlesFrontLeft; // Système de particules pour les effets de freinage des roues avant gauche.

    public ParticleSystem brakeParticlesFrontRight; // Système de particules pour les effets de freinage des roues avant droite.

    public ParticleSystem brakeParticlesRearLeft; // Système de particules pour les effets de freinage des roues arrière gauche.

    public ParticleSystem brakeParticlesRearRight; // Système de particules pour les effets de freinage des roues arrière droite.

    public AudioClip brakeSound; // Clip audio pour le son de freinage.

    public AudioSource sourceSound; // Source audio utilisée pour jouer les sons de freinage.

    // Tailles des 4 roues
    private float frontLeftRadius;
    private float frontRightRadius;
    private float rearLeftRadius;
    private float rearRightRadius;

    void Awake()
    {
        // Mise a jour du niveau de volume définie dans les options dans le menu
        sourceSound.volume = GlobalVariables.soundLevel;

        // Calculer les rayons des roues dynamiquement
        frontLeftRadius = GetWheelRadius(frontLeftWheel);
        frontRightRadius = GetWheelRadius(frontRightWheel);
        rearLeftRadius = GetWheelRadius(rearLeftWheel);
        rearRightRadius = GetWheelRadius(rearRightWheel);
    }

    void Update()
    {
        // Vitesse de la voiture
        float speed = rb.velocity.magnitude;

        // Appliquer la rotation des roues
        RotateWheels(speed);

        // Tourner les roues avant en fonction de l'entrée horizontale
        float steerInput = Input.GetAxis("Horizontal");
        SteerWheels(steerInput);

        // Vérifier si le véhicule est en train de freiner
        bool isBraking = Input.GetKey(KeyCode.S);
        ApplyBrakeEffects(isBraking);
    }

    /// <summary>
    /// Calcule le rayon d'une roue à partir de son composant MeshRenderer.
    /// </summary>
    /// <param name="wheel">La roue dont on veut déterminer le rayon.</param>
    /// <returns>Le rayon de la roue.</returns>
    float GetWheelRadius(GameObject wheel)
    {
        MeshRenderer renderer = wheel.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            // taille de la rroue
            return renderer.bounds.extents.y;
        }

        Debug.LogWarning($"Impossible de déterminer le rayon de la roue {wheel.name}. Valeur par défaut utilisée.");
        return 0.33f; // Valeur par défaut si aucun composant valide n'est trouvé
    }

    /// <summary>
    /// Applique la rotation des roues en fonction de la vitesse du véhicule.
    /// </summary>
    /// <param name="speed">La vitesse du véhicule pour déterminer la rotation des roues.</param>
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

    /// <summary>
    /// Applique la direction des roues avant en fonction de l'entrée horizontale.
    /// </summary>
    /// <param name="steerInput">Entrée horizontale de l'utilisateur pour tourner le véhicule.</param>
    void SteerWheels(float steerInput)
    {
        float steerAngle = steerInput * 30f; // Limiter l'angle de braquage à 30°
        frontLeftWheel.transform.localRotation = Quaternion.Euler(0f, steerAngle, 0f);
        frontRightWheel.transform.localRotation = Quaternion.Euler(0f, steerAngle, 0f);
    }

    /// <summary>
    /// Applique les effets de freinage (particules et son) si le véhicule freine.
    /// </summary>
    /// <param name="isBraking">Indique si le véhicule est en train de freiner.</param>
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

    /// <summary>
    /// Met à jour les effets de particules de freinage en fonction de l'état de freinage.
    /// Ajoute un effet de fondu lors de l'arrêt des particules.
    /// </summary>
    /// <param name="isBraking">Indique si le véhicule est en train de freiner.</param>
    /// <param name="wheel">La roue concernée pour l'effet de particules.</param>
    /// <param name="brakeParticles">Le système de particules associé à la roue.</param>
    void UpdateParticleEffect(bool isBraking, GameObject wheel, ParticleSystem brakeParticles)
    {
        var main = brakeParticles.main;

        if (isBraking)
        {
            if (!brakeParticles.isPlaying)
            {
                brakeParticles.Play();
            }
            // Fait en sorte que les particules restent aux roues
            brakeParticles.transform.position = wheel.transform.position;

            // Réinitialise l'alpha des particules pour être visible pendant le freinage
            if (main.startColor.color.a < 1f)
            {
                main.startColor = new Color(main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, 1f);
            }
        }
        else
        {
            if (brakeParticles.isPlaying)
            {
                // Lancer un fondu avant d'arrêter les particules
                StartCoroutine(FadeOutParticles(brakeParticles, main));
            }
        }
    }

    /// <summary>
    /// Coroutine qui effectue un fondu de l'alpha des particules avant de les arrêter.
    /// </summary>
    /// <param name="brakeParticles">Le système de particules à faire fondre.</param>
    /// <param name="main">Le module principal des particules pour manipuler l'alpha.</param>
    /// <returns>Un IEnumerator pour la coroutine.</returns>
    System.Collections.IEnumerator FadeOutParticles(ParticleSystem brakeParticles, ParticleSystem.MainModule main)
    {
        float fadeDuration = 1.0f;  // Durée du fondu
        float startAlpha = main.startColor.color.a;
        float timeElapsed = 0f;

        // Diminue progressivement l'alpha des particules
        while (timeElapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, 0f, timeElapsed / fadeDuration);
            main.startColor = new Color(main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, alpha);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // S'assurer que les particules sont complètement transparentes avant de les arrêter
        main.startColor = new Color(main.startColor.color.r, main.startColor.color.g, main.startColor.color.b, 0f);
        brakeParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
