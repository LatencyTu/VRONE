using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameStart : MonoBehaviour
{
    public static void Run()
    {
        Addressables.LoadSceneAsync("Loading", UnityEngine.SceneManagement.LoadSceneMode.Single).Completed += (obj) =>
        {
            Debug.Log("Loading Scene");
        };
    }
}
