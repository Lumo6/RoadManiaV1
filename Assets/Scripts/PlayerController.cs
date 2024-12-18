using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Transform positionRoute;
    public GameObject player;
    [Header("AVEC courbe d'animation")]
    [Range(3, 9)]
    public float minSpeed = 6f;
    [Range(10, 30)]
    public float maxSpeed = 20f;
    public float turnspeed;
    [Range(3, 10)]
    [Tooltip("Temps en seconde (float) pour passer de la vittesse MIN à MAX")]
    public float timeFromMinToMax = 5.0f;
    public AnimationCurve accelerationSpeedCURVE;
    private float accel_x = 0;
    private float speed_y;
    private float speed;
    private int signAccel;
    private float amplitudeSpeed;
    // définir un coefficient entre la décélération (absence d'accélération et frein) et le freinage
    private const float RAPPORT_DECELERATION_FREINAGE = 3.0f;
    private float horizontalInput;
    private float forwardInput;

    private float elapsedTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //StatsGame.instance.InitStatsGame(player.transform);
        speed = minSpeed;
        amplitudeSpeed = maxSpeed - minSpeed;
    }
    // Version / Décélération automatique / Utiliser courbe d'animation
    // Ne pas pouvoir dépasser une VitesseMax ni descendre en dessous d’une VitesseMin
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

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        transform.Rotate(Vector3.up, horizontalInput * turnspeed * Time.deltaTime);

        if (elapsedTime >= 10f)
        {
            elapsedTime = 0f;
            maxSpeed *= 1.2f;
            minSpeed *= 1.2f;
        }
        if (gameObject.transform.position.y < -0.1)
        {
            SceneManager.LoadScene("SceneLoser");
        }
    }

}
