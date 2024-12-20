using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public float maxSpeed = 20f;
    public float minSpeed = 6f;
    public static GameObject bonusmalus;
    public Canvas MalusVision; // Reference to the MalusVision canvas

    public static GameManager Instance
    {
        get {
            if (_instance == null)
                Debug.LogError("GameManager is null !!!");

            return _instance;
        }   
    }
    private void Awake()
    {
        _instance = this;
    }

    public void Bonus(float bonusAmount, float duration)
    {
        maxSpeed += bonusAmount;
        minSpeed += bonusAmount / 2;
        StartCoroutine(applyBonus(bonusAmount, duration));
    }

    private System.Collections.IEnumerator applyBonus(float amount, float duration)
    {
        yield return new WaitForSeconds(duration);
        maxSpeed -= amount;
        minSpeed -= amount / 2;
    }

    public void Malus(float malusAmount, float duration)
    {
        if (MalusVision != null)
        {
            MalusVision.enabled = true;
        }
        StartCoroutine(RemoveEffectAfterTime(duration));
    }

    private System.Collections.IEnumerator RemoveEffectAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (MalusVision != null)
        {
            MalusVision.enabled = false;
        }
    }
}
