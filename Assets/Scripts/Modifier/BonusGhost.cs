using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Classe BonusGhost
/// <para>
/// Classe représentant un objet bonus "Fantôme" qui, lorsqu'il est collecté par le joueur, déclenche un effet de bonus sur le joueur.
/// </para>
/// </summary>
public class BonusGhost : MonoBehaviour
{
    /// <summary>
    /// Durée pendant laquelle l'effet de bonus "Fantôme" sera actif.
    /// </summary>
    public float duration = 10f;

    /// <summary>
    /// Méthode appelée lorsque le joueur entre en collision avec l'objet "BonusGhost".
    /// Cette méthode déclenche un effet de bonus et joue le son associé.
    /// </summary>
    /// <param name="other">Le collider avec lequel l'objet entre en collision.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si le collider appartient au joueur
        if (other.CompareTag("Player"))
        {
            // Déclenche l'effet de bonus pour le joueur
            GameManager.Instance.OnBonusGhost.Invoke(duration);

            // Joue le son associé au bonus "Fantôme"
            SoundManager.Instance.OnBonusGhost.Invoke();

            // Détruit l'objet bonus après sa collecte
            Destroy(gameObject);
        }
    }
}
