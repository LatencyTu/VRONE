using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Templete;
using UnityEngine.AddressableAssets;
using System;

public class LoadingPanelCtrl:SingleBase<LoadingPanelCtrl>
{
    public GameObject root;
    public LoadingPanel script;
    public void OpenLoadingPanel(string msg = "")
    {
        if(root == null)
        {
            root = GameObject.Find("MainUICanvas").GetComponent<MainUICanvas>().LoadingPanel;
            script = root.GetComponent<LoadingPanel>();
        }
        root.SetActive(true);
        script.Init(msg);
    }
    public void CloseLoadingPanel()
    {
        if (root) root.SetActive(false);
    }
}
