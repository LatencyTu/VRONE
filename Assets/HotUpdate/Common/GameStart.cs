using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Templete;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

//热更程序集运行的入口，不需要挂载
public class GameStart : MonoBehaviour
{
    static GameGlobar GameGlobar;
    static GameObject WebMgr;
    static GameObject Player;
    static GameObject DragPanel;
    //在LoadDll中调用
    public static void Run()
    {
        //启用多点触控
        Input.multiTouchEnabled = true;
        Preload();
    }
    public static void PlayVideo()
    {
        AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>("Video");
        asyncOperationHandle.Completed += (handle) =>
        {
            GameObject video = Instantiate(handle.Result, GameObject.Find("MainUICanvas").GetComponent<MainUICanvas>().Top.transform);
        };
    }
    public static void Preload()
    {
        //加载player，webmgr，摇杆界面
        //加载结束后加入到GameGlobar的字典
        GameGlobar = GameObject.Find("MainUICanvas").GetComponent<GameGlobar>();
        GameGlobar.Map.Add("IsLand", false);
        AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>("WebMgr");
        AsyncOperationHandle<GameObject> playerHandle = Addressables.LoadAssetAsync<GameObject>("Player");
        AsyncOperationHandle<GameObject> dragHandle = Addressables.LoadAssetAsync<GameObject>("DragPanel");
        asyncOperationHandle.Completed += (handle) =>
        {
            WebMgr = Instantiate(handle.Result); WebMgr.name = "WebMgr";
            GameGlobar.Map.Add("WebMgr", WebMgr);
        };
        CheckTick.AddRule(loadComplete, create);
        bool loadComplete()
        {
            return playerHandle.IsDone && dragHandle.IsDone;
        }
        bool create()
        {
            Player = Instantiate(playerHandle.Result);
            GameGlobar.Map.Add("Player", Player);
            Player.name = "Player"; Player.SetActive(false); DontDestroyOnLoad(Player);
            DragPanel = Instantiate(dragHandle.Result, GameObject.Find("MainUICanvas").GetComponent<MainUICanvas>().Top.transform);
            GameGlobar.Map.Add("DragPanel", DragPanel);
            DragPanel.name = "DragPanel"; DragPanel.SetActive(false);
            //PlayVideo();
            return true;
        }
    }
}
