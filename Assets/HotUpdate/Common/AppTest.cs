using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Templete;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AppTest : MonoBehaviour
{
    public WebMgr webMgr;
    // Start is called before the first frame update
    List<Dropdown.OptionData> sceneList = new List<Dropdown.OptionData>()
    {
        new Dropdown.OptionData("GetID"),
        new Dropdown.OptionData("APP1"),
        new Dropdown.OptionData("APP2"),
    };
    void Start()
    {
        AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>("SceneListPanel");
        asyncOperationHandle.Completed += (handle) =>
        {
            GameObject SceneListPanel = Instantiate(handle.Result, GameObject.Find("MainUICanvas").GetComponent<MainUICanvas>().Top.transform);
            SceneListPanel.name = "SceneListPanel";
            SceneListPanel.transform.SetAsFirstSibling();
            Dropdown dropdown = SceneListPanel.GetComponentInChildren<Dropdown>();
            dropdown.options = sceneList;
            dropdown.onValueChanged.AddListener((value)=> 
            {
                if(GameObject.Find("Drag"))
                GameObject.Find("Drag").SetActive(false);
                switch (value)
                {
                    case 0:
                        webMgr.StartDownLoad();
                        break;
                    case 1:
                        webMgr.StartAPP1();
                        break;
                    case 2:
                        webMgr.StartAPP2();
                        break;
                    default:
                        break;
                }
            });
        };
        webMgr.StartDownLoad();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
