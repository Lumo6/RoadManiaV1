using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BonusGhost : MonoBehaviour
{
    public float duration = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.OnBonusGhost.Invoke(duration);
            SoundManager.Instance.OnBonusGhost.Invoke();
            Destroy(gameObject);
        }
    }
}
