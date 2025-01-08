using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PerformanceManager : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;

    void Start() {
        GlobalVariables.graphismMode = textMeshPro.text;
    }

    public void OnChange() {
        textMeshPro.text = textMeshPro.text == "FPS" ? "Realiste" : "FPS";
        GlobalVariables.graphismMode = textMeshPro.text;
    }
}
