using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderVolume : MonoBehaviour
{
    [SerializeField]
    private Slider volumeSlider;

    void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(ChangeVolume);
            volumeSlider.value = GlobalVariables.soundLevel;
        }
    }

    public void ChangeVolume(float volume)
    {
        GlobalVariables.soundLevel = volume;
    }
}
