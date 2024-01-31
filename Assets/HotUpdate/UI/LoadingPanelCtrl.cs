using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Templete;
using UnityEngine.AddressableAssets;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Networking;
//���ؽ�����������
public class LoadingPanelCtrl:SingleBase<LoadingPanelCtrl>
{
    //������UI����
    public GameObject root;
    //���������صĽű�
    public LoadingPanel script;
    //��ʼ��
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
    //addressable���ص�
    public void OpenLoadingPanel(AsyncOperationHandle handle,string msg = "",bool close = true)
    {
        Init(msg);
        LoadingWithHandle(handle, close);
    }
    //UnityWebRequest���ص�
    public void OpenLoadingPanel(UnityWebRequest req, string msg = "", bool close = true)
    {
        Init(msg);
        LoadingWithWebRequest(req, close);
    }
    public void CloseLoadingPanel()
    {
        if (root) root.SetActive(false);
    }
    //���½�����������addressable���ؽ���
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
    //���½�����������UnityWebRequest���ؽ���
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
