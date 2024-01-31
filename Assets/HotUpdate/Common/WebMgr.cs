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

        TestDebug.Log("上传json"+jsonString);

        UnityWebRequest req =  UnityWebRequest.Post(serverUrl, jsonString, "application/json");
        //LoadingPanelCtrl.Instance().OpenLoadingPanel(req, serverUrl);
        req.downloadHandler = new DownloadHandlerBuffer();

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            string json = req.downloadHandler.text;
            TestDebug.Log("json下载成功"+json);
            U jsonData = JsonMapper.ToObject<U>(json);
            myAction(jsonData);
        }
        else
        {
            TestDebug.Log("json下载失败" + req.result + req.error + req.responseCode);
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
                a => { SpotDatas.Instance.list[i].dataSource = a; TestDebug.Log("图片地址为"+SpotDatas.Instance.list[i].dataSource.url); })); }
            if (SpotDatas.Instance.list[i].dataTypeId == "4")
            { yield return StartCoroutine(DownloadJson<DataID, DataSource>(SpotDatas.Instance.list[i].dataSourceId, "http://121.4.240.32:8080/VRdemo/video/getVideoData", 
                a => { SpotDatas.Instance.list[i].dataSource = a; TestDebug.Log("视频地址为" + SpotDatas.Instance.list[i].dataSource.url); })); }
        }
    }
    IEnumerator DownLoadCoverImage()
    {
        yield return StartCoroutine(DownloadJsons());
        TestDebug.Log(SpotDatas.Instance.list.Length + "个数据");
        for (int i = 0; i < SpotDatas.Instance.list.Length; i++)
        {
            if (SpotDatas.Instance.list[i].dataTypeId == "3")
            {
                TestDebug.Log(i + "是图片");
            }
            else
            {
                TestDebug.Log(i + "是视频");
            }
        }
        for (int i = 0; i < SpotDatas.Instance.list.Length; i++)
        {
            yield return StartCoroutine(DownLoadData(SpotDatas.Instance.list[i].dataSource.coverUrl, (data) => { SpotDatas.Instance.list[i].coverImageData = data; }));
        }
        OnDownLoadComplete();
    }
    public static IEnumerator DownLoadData(string url, Action<byte[]> myAction)    //byte数据下载并存储
    {
        TestDebug.Log("开始在" + url + "下载数据");
        UnityWebRequest req = UnityWebRequest.Get(url);
        //LoadingPanelCtrl.Instance().OpenLoadingPanel(req, url,false);
        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.Success)
        {
            byte[] data = req.downloadHandler.data;
            myAction(data);
            TestDebug.Log("获取数据成功" + url);
        }
        else
        {
            TestDebug.Log("获取数据失败" + req.result + req.error + req.responseCode);
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
                    //然后再去创建场景上的对象

                    //然后再去隐藏 加载界面

                    //注意：场景资源也是可以释放的，并不会影响当前已经加载出来的场景，因为场景的本质只是配置文件
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
                TestDebug.Log("LoadingData单例模式未设置");
                return null;
            }
        }
        set
        {
            if (instance == null)
            {
                TestDebug.Log("LoadingData单例模式已设置");
                instance = value;
            }
            else
            {
                TestDebug.Log("LoadingData单例模式重复设置");
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
                //TestDebug.Log("SpotDatas单例模式未设置");
                return null;
            }
        }
        set 
        {
            if (instance == null)
            {
                TestDebug.Log("SpotDatas单例模式已设置");
                instance = value;
            }
            else
            {
                TestDebug.Log("SpotDatas单例模式重复设置");
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
