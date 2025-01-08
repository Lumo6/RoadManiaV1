using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Classe MalusScreen
/// <para>
/// Classe représentant un objet malus "Écran" qui, lorsqu'il est collecté par le joueur, déclenche un effet de malus sur le joueur.
/// </para>
/// </summary>
public class MalusScreen : MonoBehaviour
{
    public float duration = 12f; // Durée pendant laquelle l'effet de malus "Écran" sera actif.

    /// <summary>
    /// Méthode appelée lorsque le joueur entre en collision avec l'objet "MalusScreen".
    /// Cette méthode déclenche un effet de malus et joue le son associé.
    /// </summary>
    /// <param name="other">Le collider avec lequel l'objet entre en collision.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si le collider appartient au joueur
        if (other.CompareTag("Player"))
        {
            // Déclenche l'effet de malus pour le joueur
            GameManager.Instance.OnMalusScreen.Invoke(duration);

            // Joue le son associé au malus "Écran"
            SoundManager.Instance.OnMalusScreen.Invoke();

            // Détruit l'objet malus après sa collecte
            Destroy(gameObject);
        }
    }
}
