using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Templete;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class GameStart : MonoBehaviour
{

    public static void Run()
    {
        Input.multiTouchEnabled = true;
        //Screen.SetResolution(2560, 1440, true);
        Preload();
    }
    public static void Preload()
    {
        AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync("Game", UnityEngine.SceneManagement.LoadSceneMode.Single);
        LoadingPanelCtrl.Instance().OpenLoadingPanel(handle,$"Loading Game");
        handle.Completed += (obj) =>
        {
            TestDebug.Log("Game");
        };
    }
}
