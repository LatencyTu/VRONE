using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Templete;

public class GameStart : MonoBehaviour
{
    public static void Run()
    {
        TestLoading();
    }
    public static void TestLoading()
    {
        Addressables.LoadSceneAsync("Loading", UnityEngine.SceneManagement.LoadSceneMode.Single).Completed += (obj) =>
        {
            TestDebug.Instance().Log("Loading Scene");
        };
    }
}
