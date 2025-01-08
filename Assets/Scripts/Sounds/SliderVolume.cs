using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe SliderVolume
/// <para>
/// Classe qui gère le contrôle du volume via un slider.
/// Elle permet de changer le niveau sonore global en ajustant la valeur du slider.
/// <para>
/// </summary>
public class SliderVolume : MonoBehaviour
{
    [SerializeField]
    private Slider volumeSlider; // Le slider utilisé pour ajuster le volume

    void Start()
    {
        if (volumeSlider != null)
        {
            // Ajoute un écouteur d'événements pour changer le volume lorsque le slider est modifié
            volumeSlider.onValueChanged.AddListener(ChangeVolume);
            volumeSlider.value = GlobalVariables.soundLevel; // Initialise la valeur du slider avec le niveau de volume actuel
        }
    }

    /// <summary>
    /// Change le volume global en fonction de la valeur du slider.
    /// </summary>
    /// <param name="volume">Le nouveau volume à appliquer.</param>
    public void ChangeVolume(float volume)
    {
        GlobalVariables.soundLevel = volume; // Met à jour le niveau sonore global
    }
}
