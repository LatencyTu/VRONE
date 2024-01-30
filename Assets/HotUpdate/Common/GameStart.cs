using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Templete;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class GameStart : MonoBehaviour
{
    static GameGlobar GameGlobar;
    static GameObject WebMgr;
    static GameObject Player;
    static GameObject DragPanel;
    public static void Run()
    {
        Input.multiTouchEnabled = true;
        Preload();
    }
    public static void Preload()
    {
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
            return true;
        }
    }
}
