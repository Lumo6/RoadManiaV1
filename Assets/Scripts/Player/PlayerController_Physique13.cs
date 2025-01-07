using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController_Physique13 : MonoBehaviour
{
    public Transform positionRoute;
    public GameObject player;
    [Header("AVEC courbe d'animation")]
    [Range(3, 9)]
    public float minSpeed;
    [Range(10, 30)]
    public float maxSpeed;
    public float turnspeed;
    [Range(3, 10)]
    [Tooltip("Temps en seconde (float) pour passer de la vittesse MIN � MAX")]
    public float timeFromMinToMax = 5.0f;
    public AnimationCurve accelerationSpeedCURVE;
    private float accel_x = 0;
    private float speed;
    private int signAccel;
    private float amplitudeSpeed;
    private const float RAPPORT_DECELERATION_FREINAGE = 3.0f;
    private float horizontalInput;
    private float forwardInput;

    private float elapsedTime = 0f;
    private Rigidbody rb;

    private bool bCheckRotaY = true;

    private GameManager GM;

    // Start is called before the first frame update
    void Start()
    {
        minSpeed = GameManager.Instance.minSpeed;
        maxSpeed = GameManager.Instance.maxSpeed;
        rb = player.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        speed = minSpeed;
        amplitudeSpeed = maxSpeed - minSpeed;
    }
    // Version / D�c�l�ration automatique / Utiliser courbe d'animation
    // Ne pas pouvoir d�passer une VitesseMax ni descendre en dessous d�une VitesseMin
    void Update()
    {
        elapsedTime += Time.deltaTime;
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        if (forwardInput > 0)
        {
            signAccel = 1;
        }
        else if (forwardInput < 0)
        {
            signAccel = -1;
        }

        if (elapsedTime >= 10f)
        {
            elapsedTime = 0f;
            maxSpeed *= 1.2f;
            minSpeed *= 1.2f;
        }
        if (gameObject.transform.position.y < -1)
        {
            SceneManager.LoadScene("SceneLoser");
        }

    }
    void FixedUpdate()
    {
        accel_x = signAccel * amplitudeSpeed * accelerationSpeedCURVE.Evaluate(Time.time / timeFromMinToMax);

        if (signAccel == -1 && speed > minSpeed)
        {
            speed += accel_x / RAPPORT_DECELERATION_FREINAGE;
        }
        else if (signAccel == 1 && speed < maxSpeed)
        {
            speed += accel_x * Time.deltaTime;
        }

        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);

        bCheckRotaY = Mathf.Abs(player.transform.rotation.y) > 1.0f ? false : true;


        if(player.transform.rotation.x < -45)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, Time.fixedDeltaTime * turnspeed * horizontalInput, 0));
        }

        Vector3 direction = rb.transform.forward;
        Quaternion rotation = Quaternion.Euler(0, horizontalInput * turnspeed * Time.fixedDeltaTime, 0);
        Vector3 movement = direction * speed * Time.fixedDeltaTime;

        bCheckRotaY = Mathf.Abs(player.transform.rotation.y) > 30.0f ? false : true;

        if (bCheckRotaY)
        {
            rb.MovePosition(rb.position + movement);
            rb.MoveRotation(rb.rotation * rotation);
        }
        else
        {
            rb.MoveRotation(rb.rotation * rotation);
        }
    }
}
