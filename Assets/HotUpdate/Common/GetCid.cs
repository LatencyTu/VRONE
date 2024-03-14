using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Templete;
using UnityEngine;
using UnityEngine.UI;

//��ȡ��ǰҳ���url
public class GetCid : MonoBehaviour
{
    private static string cid = "2c";
    private static string scene_name= "����";
    public static string Cid
    {
        get 
        {
            if ( Application.absoluteURL != null && Application.absoluteURL != "")
            {
                string[] sArray = Application.absoluteURL.Split("cid=");
                if (sArray.Length < 2)
                {
                    TestDebug.Log($"δ����վ�϶�ȡcid����Ϊ{cid}��վ��{Application.absoluteURL}");
                }
                else
                {
                    TestDebug.Log($"��վ��{Application.absoluteURL}cid��{sArray[sArray.Length - 1]}");
                    cid = sArray[sArray.Length - 1];
                }
            }
            else
            {
                TestDebug.Log($"δ��ȡ��ַ����Ϊ{cid}{ Application.absoluteURL}" );
            }
            return cid;
        }
        set 
        {
            cid = value;
            TestDebug.Log("cid��Ϊ" + value);
        }
    }
    public static string Name
    {
        get
        {
            if (Application.absoluteURL != null && Application.absoluteURL != "" && cid == null)
            {
                string[] sArray = Application.absoluteURL.Split("name=");
                if (sArray.Length >= 2)
                {
                    scene_name = sArray[sArray.Length - 1];
                }
            }
            TestDebug.Log(scene_name);
            return scene_name;
        }
        set
        {
            scene_name = value;
        }
    }
    void Start()
    {
        TestDebug.Log("��ǰ��ַ"+Application.absoluteURL);
    }
    string GetCaid(string url)
    {
        string[] sArray = url.Split('/');
        return sArray[sArray.Length - 1];
    }
}

