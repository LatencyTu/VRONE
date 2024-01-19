using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUICanvas :MonoBehaviour
{
    public GameObject Top;
    public GameObject Middle;
    public GameObject Bottom;
    public GameObject LoadingPanel;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

}
