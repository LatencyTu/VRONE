using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Templete;
using UnityEngine;
using UnityEngine.UI;

//获取当前页面的url
public class GetCid : MonoBehaviour
{
    private static string cid = "2c";
    private static string scene_name= "万福堂";
    public static string Cid
    {
        get 
        {
            if ( Application.absoluteURL != null && Application.absoluteURL != "")
            {
                string[] sArray = Application.absoluteURL.Split("cid=");
                if (sArray.Length < 2)
                {
                    TestDebug.Log($"未从网站上读取cid，设为{cid}网站是{Application.absoluteURL}");
                }
                else
                {
                    TestDebug.Log($"网站是{Application.absoluteURL}cid是{sArray[sArray.Length - 1]}");
                    cid = sArray[sArray.Length - 1];
                }
            }
            else
            {
                TestDebug.Log($"未读取网址，设为{cid}{ Application.absoluteURL}" );
            }
            return cid;
        }
        set 
        {
            cid = value;
            TestDebug.Log("cid设为" + value);
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
        TestDebug.Log("当前地址"+Application.absoluteURL);
    }
    string GetCaid(string url)
    {
        string[] sArray = url.Split('/');
        return sArray[sArray.Length - 1];
    }
}

