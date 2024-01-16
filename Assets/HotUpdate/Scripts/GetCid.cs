using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Templete;
using UnityEngine;
using UnityEngine.UI;

//获取当前页面的url
public class GetCid : MonoBehaviour
{
    private static string cid;
    public static string Cid
    {
        get 
        {
            if ( Application.absoluteURL != null && Application.absoluteURL != "" && cid == null)
            {
                string[] sArray = Application.absoluteURL.Split("cid=");
                if (sArray.Length < 2)
                {
                    TestDebug.Instance().Log("未从网站上读取cid，设为1"+"网站是"+ Application.absoluteURL);
                    cid = "1";
                    return "1";
                }
                else
                {
                    TestDebug.Instance().Log("网站是" + Application.absoluteURL + "cid是" + sArray[sArray.Length - 1]);
                    cid = sArray[sArray.Length - 1];
                    return cid;
                }
            }
            else if(cid != null)
            {
                TestDebug.Instance().Log("cid设为" + cid);
                return cid;
            }
            else
            {
                TestDebug.Instance().Log("未读取网址，设为1" + Application.absoluteURL);
                cid = "1";
                return "1";
            }
        }
        set 
        {
            cid = value;
            TestDebug.Instance().Log("cid设为" + value);
        }
    }

    void Start()
    {
        TestDebug.Instance().Log("当前地址"+Application.absoluteURL);
    }
    string GetCaid(string url)
    {
        string[] sArray = url.Split('/');
        return sArray[sArray.Length - 1];
    }
}

