using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;

public class WebMgr : MonoBehaviour
{
    void Start()
    {
    }
    interface IInitializeID
    {
        public void InitializeID(string id);
    }
    public class CAID: IInitializeID
    {
        public string company_app_id;
        public void InitializeID(string id)
        {
            company_app_id = id;
        }
    }
    public class DataID: IInitializeID
    {
        public string data_source_id;

        public void InitializeID(string id)
        {
            data_source_id = id;
        }
    }

    IEnumerator DownloadJson<T,U>(string ID,string serverUrl, Action<U> myAction) where T : IInitializeID,new()
    {

        //if (typeof(T) == typeof(CAID))
        //{
        //    CAID caID = new CAID(ID);
        //    jsonString = JsonMapper.ToJson(caID);
        //}
        //else if (typeof(T) == typeof(DataID))
        //{
        //    DataID dataID = new DataID(ID);
        //    jsonString = JsonMapper.ToJson(dataID);
        //}
        //else { Debug.Log("type error"); yield break; }
        T id = new T() ;

        (id as IInitializeID)?.InitializeID(ID);

        string jsonString = JsonMapper.ToJson(id);

        print("�ϴ�json"+jsonString);

        UnityWebRequest req =  UnityWebRequest.Post(serverUrl, jsonString, "application/json");

        req.downloadHandler = new DownloadHandlerBuffer();

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            string json = req.downloadHandler.text;
            print("json���سɹ�"+json);
            U jsonData = JsonMapper.ToObject<U>(json);
            myAction(jsonData);
        }
        else
        {
            print("json����ʧ��" + req.result + req.error + req.responseCode);
        }
    }
    IEnumerator DownloadJsons()
    {
        yield return StartCoroutine(DownloadJson<CAID, LoadingData>(GetCid.Cid, "http://121.4.240.32:8080/VRdemo/loading/AppSetting", 
            a => { LoadingData.Instance = a;}));
        yield return StartCoroutine(DownloadJson<CAID, SpotDatas>(GetCid.Cid, "http://121.4.240.32:8080/VRdemo/loading/spot", 
            a => { SpotDatas.Instance = a;}));
        for (int i = 0; i < SpotDatas.Instance.list.Length; i++)
        {
            if (SpotDatas.Instance.list[i].dataTypeId == "3")
            { yield return StartCoroutine(DownloadJson<DataID, DataSource>(SpotDatas.Instance.list[i].dataSourceId, "http://121.4.240.32:8080/VRdemo/img/getImageData", 
                a => { SpotDatas.Instance.list[i].dataSource = a; print("ͼƬ��ַΪ"+SpotDatas.Instance.list[i].dataSource.url); })); }
            if (SpotDatas.Instance.list[i].dataTypeId == "4")
            { yield return StartCoroutine(DownloadJson<DataID, DataSource>(SpotDatas.Instance.list[i].dataSourceId, "http://121.4.240.32:8080/VRdemo/video/getVideoData", 
                a => { SpotDatas.Instance.list[i].dataSource = a; print("��Ƶ��ַΪ" + SpotDatas.Instance.list[i].dataSource.url); })); }
        }
    }
    IEnumerator DownLoadCoverImage()
    {
        yield return StartCoroutine(DownloadJsons());
        Debug.Log(SpotDatas.Instance.list.Length + "������");
        for (int i = 0; i < SpotDatas.Instance.list.Length; i++)
        {
            if (SpotDatas.Instance.list[i].dataTypeId == "3")
            {
                Debug.Log(i + "��ͼƬ");
            }
            else
            {
                Debug.Log(i + "����Ƶ");
            }
        }
        for (int i = 0; i < SpotDatas.Instance.list.Length; i++)
        {
            yield return StartCoroutine(DownLoadData(SpotDatas.Instance.list[i].dataSource.coverUrl, (data) => { SpotDatas.Instance.list[i].coverImageData = data; }));
        }
        OnDownLoadComplete();
    }
    public static IEnumerator DownLoadData(string url, Action<byte[]> myAction)    //byte�������ز��洢
    {
        print("��ʼ��" + url + "��������");
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.Success)
        {
            byte[] data = req.downloadHandler.data;
            myAction(data);
            print("��ȡ���ݳɹ�" + url);
        }
        else
        {
            print("��ȡ����ʧ��" + req.result + req.error + req.responseCode);
        }
    }
    public void StartDownLoad()
    {
        StartCoroutine(DownLoadCoverImage());
    }
    public void StartAPP1()
    {
        GetCid.Cid = "1";
        StartCoroutine(DownLoadCoverImage());
    }
    public void StartAPP2()
    {
        GetCid.Cid = "2";
        StartCoroutine(DownLoadCoverImage());
    }
    void OnDownLoadComplete()
    {

        Addressables.LoadSceneAsync("APP"+GetCid.Cid, UnityEngine.SceneManagement.LoadSceneMode.Single).Completed += (obj) =>
        {
            obj.Result.ActivateAsync().completed += (a) =>
            {
                    //Ȼ����ȥ���������ϵĶ���

                    //Ȼ����ȥ���� ���ؽ���

                    //ע�⣺������ԴҲ�ǿ����ͷŵģ�������Ӱ�쵱ǰ�Ѿ����س����ĳ�������Ϊ�����ı���ֻ�������ļ�
                };
        };
    }
}



public class LoadingData
{
    public string name;
    private static LoadingData instance;
    public static LoadingData Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                Debug.Log("LoadingData����ģʽδ����");
                return null;
            }
        }
        set
        {
            if (instance == null)
            {
                Debug.Log("LoadingData����ģʽ������");
                instance = value;
            }
            else
            {
                Debug.Log("LoadingData����ģʽ�ظ�����");
            }
        }
    }
}
public class SpotDatas
{
    private static SpotDatas instance;
    public static SpotDatas Instance
    {
        get 
        {
            if (instance != null)
            { 
                return instance; 
            }
            else
            {
                Debug.Log("SpotDatas����ģʽδ����");
                return null;
            }
        }
        set 
        {
            if (instance == null)
            {
                Debug.Log("SpotDatas����ģʽ������");
                instance = value;
            }
            else
            {
                Debug.Log("SpotDatas����ģʽ�ظ�����");
            }
        }
    }
    public SpotData[] list;
}
public class SpotData
{
    public string dataSourceId;
    public string dataTypeId;
    public byte[] coverImageData;
    public byte[] data;
    public DataSource dataSource;
}
public class DataSource
{
    public string coverUrl;
    public string path;
    public string url;
}
