using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MalusScreen : MonoBehaviour
{
    public float duration = 12f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.OnMalusScreen.Invoke(duration);
            Destroy(gameObject);
        }
    }
}
