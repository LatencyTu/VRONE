using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Templete;

public class WebMgr : MonoBehaviour
{
    static GameGlobar GameGlobar;
    void Awake()
    {
        GameGlobar = GameObject.Find("MainUICanvas").GetComponent<GameGlobar>();
        DontDestroyOnLoad(this);
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
        //else { TestDebug.Log("type error"); yield break; }
        T id = new T() ;

        (id as IInitializeID)?.InitializeID(ID);

        string jsonString = JsonMapper.ToJson(id);

        TestDebug.Log("�ϴ�json"+jsonString);

        UnityWebRequest req =  UnityWebRequest.Post(serverUrl, jsonString, "application/json");
        //LoadingPanelCtrl.Instance().OpenLoadingPanel(req, serverUrl);
        req.downloadHandler = new DownloadHandlerBuffer();

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            string json = req.downloadHandler.text;
            TestDebug.Log("json���سɹ�"+json);
            U jsonData = JsonMapper.ToObject<U>(json);
            myAction(jsonData);
        }
        else
        {
            TestDebug.Log("json����ʧ��" + req.result + req.error + req.responseCode);
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
                a => { SpotDatas.Instance.list[i].dataSource = a; TestDebug.Log("ͼƬ��ַΪ"+SpotDatas.Instance.list[i].dataSource.url); })); }
            if (SpotDatas.Instance.list[i].dataTypeId == "4")
            { yield return StartCoroutine(DownloadJson<DataID, DataSource>(SpotDatas.Instance.list[i].dataSourceId, "http://121.4.240.32:8080/VRdemo/video/getVideoData", 
                a => { SpotDatas.Instance.list[i].dataSource = a; TestDebug.Log("��Ƶ��ַΪ" + SpotDatas.Instance.list[i].dataSource.url); })); }
        }
    }
    IEnumerator DownLoadCoverImage()
    {
        yield return StartCoroutine(DownloadJsons());
        TestDebug.Log(SpotDatas.Instance.list.Length + "������");
        for (int i = 0; i < SpotDatas.Instance.list.Length; i++)
        {
            if (SpotDatas.Instance.list[i].dataTypeId == "3")
            {
                TestDebug.Log(i + "��ͼƬ");
            }
            else
            {
                TestDebug.Log(i + "����Ƶ");
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
        TestDebug.Log("��ʼ��" + url + "��������");
        UnityWebRequest req = UnityWebRequest.Get(url);
        //LoadingPanelCtrl.Instance().OpenLoadingPanel(req, url,false);
        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.Success)
        {
            byte[] data = req.downloadHandler.data;
            myAction(data);
            TestDebug.Log("��ȡ���ݳɹ�" + url);
        }
        else
        {
            TestDebug.Log("��ȡ����ʧ��" + req.result + req.error + req.responseCode);
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
        if (GameGlobar.Map.ContainsKey("Player") && GameGlobar.Map.ContainsKey("DragPanel"))
        {
            (GameGlobar.Map["Player"] as GameObject).SetActive(false);
            (GameGlobar.Map["DragPanel"] as GameObject).SetActive(false);
        }
        AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync("APP" + GetCid.Cid, UnityEngine.SceneManagement.LoadSceneMode.Single);
        LoadingPanelCtrl.Instance().OpenLoadingPanel(handle,$"Loading APP{GetCid.Cid}");
        handle.Completed += (obj) =>
        {
            SceneManager.SetActiveScene(obj.Result.Scene);
            AsyncOperation async = obj.Result.ActivateAsync();
            async.completed += (a) =>
            {

                CheckTick.AddRule(check, active);
                bool check()
                {
                    return GameGlobar.Map["Player"] as GameObject && GameGlobar.Map["DragPanel"] as GameObject;
                }
                bool active()
                {
                    (GameGlobar.Map["Player"] as GameObject).SetActive(true);
                    (GameGlobar.Map["DragPanel"] as GameObject).SetActive(true);
                    return true;
                }
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
                TestDebug.Log("LoadingData����ģʽδ����");
                return null;
            }
        }
        set
        {
            if (instance == null)
            {
                TestDebug.Log("LoadingData����ģʽ������");
                instance = value;
            }
            else
            {
                TestDebug.Log("LoadingData����ģʽ�ظ�����");
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
                //TestDebug.Log("SpotDatas����ģʽδ����");
                return null;
            }
        }
        set 
        {
            if (instance == null)
            {
                TestDebug.Log("SpotDatas����ģʽ������");
                instance = value;
            }
            else
            {
                TestDebug.Log("SpotDatas����ģʽ�ظ�����");
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
