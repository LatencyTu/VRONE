using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Templete;
using UnityEngine;
using UnityEngine.UI;

//��ȡ��ǰҳ���url
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
                    TestDebug.Instance().Log("δ����վ�϶�ȡcid����Ϊ1"+"��վ��"+ Application.absoluteURL);
                    cid = "1";
                    return "1";
                }
                else
                {
                    TestDebug.Instance().Log("��վ��" + Application.absoluteURL + "cid��" + sArray[sArray.Length - 1]);
                    cid = sArray[sArray.Length - 1];
                    return cid;
                }
            }
            else if(cid != null)
            {
                TestDebug.Instance().Log("cid��Ϊ" + cid);
                return cid;
            }
            else
            {
                TestDebug.Instance().Log("δ��ȡ��ַ����Ϊ1" + Application.absoluteURL);
                cid = "1";
                return "1";
            }
        }
        set 
        {
            cid = value;
            TestDebug.Instance().Log("cid��Ϊ" + value);
        }
    }

    void Start()
    {
        TestDebug.Instance().Log("��ǰ��ַ"+Application.absoluteURL);
    }
    string GetCaid(string url)
    {
        string[] sArray = url.Split('/');
        return sArray[sArray.Length - 1];
    }
}

