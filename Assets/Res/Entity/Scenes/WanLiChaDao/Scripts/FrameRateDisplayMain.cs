using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateDisplay : MonoBehaviour
{
    float fpsCurrent;
    float fpsRat;
    public int fpsLimit = 120;


    void Start()
    {
        Application.targetFrameRate = fpsLimit;
    }

    void Update()
    {
        fpsRat += Time.deltaTime;
        if (fpsRat > 1)
        {
            fpsRat = 0;
            fpsCurrent = 1f / Time.deltaTime;
        }
    }

    void OnGUI()
    {
        GUI.color = Color.black;
        GUI.Label(new Rect(10, 10, 100, 23), "FPS: " + fpsCurrent.ToString("f0"));
    }
}
