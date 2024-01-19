using System.Collections;
using System.Collections.Generic;
using Templete;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    public Slider bar;
    public Text title;
    public Text percent;
    public float curValue;
    
    public void Init(string msg)
    {
        bar.value = 0;
        curValue = 0;
        title.text = msg;
        percent.text = "0%";
    }
    public void SetValue(float value)
    {
        bar.value = value;
        curValue = value;
        percent.text = $"{value*100}%";
    }

}
