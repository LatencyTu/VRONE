using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Templete;
using StarterAssets;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cinemachine;

//管理场景切换，横竖屏等设置的测试类，挂载在WebMgr上
public class AppTest : MonoBehaviour
{
    //存放一些全局变量
    GameGlobar GameGlobar;
    //UI的根节点（所有屏幕UI都在这下面）
    MainUICanvas MainUICanvas;
    //UI的次级节点（一般UI在这下面，如加载进度条，摇杆，场景切换下拉框）
    RectTransform topRect;
    public WebMgr webMgr;
    //下拉框的数据
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
        //加载场景切换的UI
        AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>("SceneListPanel");
        asyncOperationHandle.Completed += (handle) =>
        {
            //实例化，添加监听事件
            GameObject SceneListPanel = Instantiate(handle.Result, GameObject.Find("MainUICanvas").GetComponent<MainUICanvas>().Top.transform);
            SceneListPanel.name = "SceneListPanel";
            SceneListPanel.transform.SetAsFirstSibling();
            Dropdown dropdown = SceneListPanel.GetComponentInChildren<Dropdown>();
            Toggle toggle = SceneListPanel.GetComponentInChildren<Toggle>();
            dropdown.options = sceneList;
            dropdown.onValueChanged.AddListener((value)=> 
            {
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
            //添加循环判断
            CheckTick.AddRule(checkload, addevent);
            //当player和摇杆界面加载完毕时
            bool checkload()
            {
                return GameGlobar.Map["Player"] as GameObject && GameGlobar.Map["DragPanel"] as GameObject;
            }
            //对复选框添加监听事件（切换横竖屏）
            bool addevent()
            {
                toggle.onValueChanged.AddListener((isOn) =>
                {
                    GameObject player = GameGlobar.Map["Player"] as GameObject;
                    CinemachineVirtualCamera virtualCamera = player.GetComponentInChildren<CinemachineVirtualCamera>();
                    if (toggle.isOn)
                    {
                        TestDebug.Log("横屏");
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
                        TestDebug.Log("竖屏");
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
