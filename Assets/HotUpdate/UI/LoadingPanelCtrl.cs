using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Templete;
using UnityEngine.AddressableAssets;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Networking;
//加载进度条控制器
public class LoadingPanelCtrl:SingleBase<LoadingPanelCtrl>
{
    //进度条UI界面
    public GameObject root;
    //进度条挂载的脚本
    public LoadingPanel script;
    //初始化
    void Init(string msg)
    {
        if (root == null)
        {
            root = GameObject.Find("MainUICanvas").GetComponent<MainUICanvas>().LoadingPanel;
            script = root.GetComponent<LoadingPanel>();
        }
        root.SetActive(true);
        script.Init(msg);
    }
    public void OpenLoadingPanel(string msg = "")
    {
        Init(msg);
    }
    //addressable加载的
    public void OpenLoadingPanel(AsyncOperationHandle handle,string msg = "",bool close = true)
    {
        Init(msg);
        LoadingWithHandle(handle, close);
    }
    //UnityWebRequest加载的
    public void OpenLoadingPanel(UnityWebRequest req, string msg = "", bool close = true)
    {
        Init(msg);
        LoadingWithWebRequest(req, close);
    }
    public void CloseLoadingPanel()
    {
        if (root) root.SetActive(false);
    }
    //更新进度条，根据addressable加载进度
    public void LoadingWithHandle(AsyncOperationHandle handle, bool close)
    {
        CheckTick.AddRule(() =>
        {
            script.SetValue(Mathf.Lerp(script.curValue, handle.PercentComplete, Time.deltaTime));
            return handle.IsDone;
        },
        () =>
        {
            if(close) CloseLoadingPanel();
            return true;
        });
    }
    //更新进度条，根据UnityWebRequest加载进度
    public void LoadingWithWebRequest(UnityWebRequest req, bool close)
    {
        CheckTick.AddRule(() =>
        {
            script.SetValue(Mathf.Lerp(script.curValue, req.downloadProgress, Time.deltaTime));
            return req.isDone;
        },
        () =>
        {
            if (close) CloseLoadingPanel();
            return true;
        });
    }
}
