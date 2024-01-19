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
        Preload();
    }
    public static void Preload()
    {
        Addressables.LoadSceneAsync("Game", UnityEngine.SceneManagement.LoadSceneMode.Single).Completed += (obj) =>
        {
            TestDebug.Instance().Log("Game");
        };
    }
}
