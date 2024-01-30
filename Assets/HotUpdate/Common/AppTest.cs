using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Templete;
using StarterAssets;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cinemachine;

public class AppTest : MonoBehaviour
{
    GameGlobar GameGlobar;
    MainUICanvas MainUICanvas;
    RectTransform topRect;
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
        GameGlobar = GameObject.Find("MainUICanvas").GetComponent<GameGlobar>();
        MainUICanvas = GameObject.Find("MainUICanvas").GetComponent<MainUICanvas>();
        topRect = MainUICanvas.Top.GetComponent<RectTransform>();
        AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>("SceneListPanel");
        asyncOperationHandle.Completed += (handle) =>
        {
            GameObject SceneListPanel = Instantiate(handle.Result, GameObject.Find("MainUICanvas").GetComponent<MainUICanvas>().Top.transform);
            SceneListPanel.name = "SceneListPanel";
            SceneListPanel.transform.SetAsFirstSibling();
            Dropdown dropdown = SceneListPanel.GetComponentInChildren<Dropdown>();
            Toggle toggle = SceneListPanel.GetComponentInChildren<Toggle>();
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
            CheckTick.AddRule(checkload, addevent);
            bool checkload()
            {
                return GameGlobar.Map["Player"] as GameObject && GameGlobar.Map["DragPanel"] as GameObject;
            }
            bool addevent()
            {
                toggle.onValueChanged.AddListener((isOn) =>
                {
                    GameObject player = GameGlobar.Map["Player"] as GameObject;
                    CinemachineVirtualCamera virtualCamera = player.GetComponentInChildren<CinemachineVirtualCamera>();
                    if (toggle.isOn)
                    {
                        TestDebug.Log("∫·∆¡");
                        virtualCamera.m_Lens.Dutch = 90;
                        MainUICanvas.Top.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -90);

                        Vector2 size = new Vector2(topRect.rect.height, topRect.rect.width) ;
                        TestDebug.Log(topRect.rect);
                        topRect.anchorMin = new Vector2(0.5f, 0.5f);
                        topRect.anchorMax = new Vector2(0.5f, 0.5f);
                        topRect.sizeDelta = size;
                        GameGlobar.Map["IsLand"] = true;
                    }
                    else
                    {
                        TestDebug.Log(" ˙∆¡");
                        virtualCamera.m_Lens.Dutch = 0;
                        MainUICanvas.Top.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0);
                        topRect.anchorMin = new Vector2(0f, 0f);
                        topRect.anchorMax = new Vector2(1f,1f);
                        topRect.sizeDelta = new Vector2(0, 0);
                        GameGlobar.Map["IsLand"] = false;
                    }
                });
                return true;
            }
        };
        webMgr.StartDownLoad();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
