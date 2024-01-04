using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;
using UnityEngine.EventSystems;

public class ApplyData : MonoBehaviour
{
    public GameObject canvas;
    public MediaPlayer mediaPlayer;
    public DisplayUGUI videoDisplayUI;
    public RawImage rawImage;
    void Start()
    {
        ShowCovers();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ApplyCover(Transform spot)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(SpotDatas.Instance.list[0].coverImageData);
        spot.GetComponent<RawImage>().texture = texture;
    }
    public void ShowCovers()
    {
        for(int i = 0; i< this.transform.childCount; i++)
        {
            if (SpotDatas.Instance.list[i].dataTypeId == "3")
            {
                ApplyCover(this.transform.GetChild(i));
            }
            else if (SpotDatas.Instance.list[i].dataTypeId == "4")
            {
                ApplyCover(this.transform.GetChild(i));
            }
        }
    }
    public void ButtonDown()
    {
        EventSystem eventSystem = EventSystem.current;
        int index = eventSystem.currentSelectedGameObject.transform.GetSiblingIndex();
        print(EventSystem.current.name);
        print("按下第" + index + "按钮");
        if (SpotDatas.Instance.list[index].dataTypeId == "3"&& SpotDatas.Instance.list[index].data==null)
        {
            StartCoroutine(WebMgr.DownLoadData(SpotDatas.Instance.list[index].dataSource.url, (data) => { SpotDatas.Instance.list[index].data = data; ShowData(index); }));
        }
        else
        { 
            ShowData(index); 
        }
    }
    public void ExitPanel()
    {
        rawImage.gameObject.SetActive(false);
        videoDisplayUI.gameObject.SetActive(false);
        mediaPlayer.Stop();
        canvas.SetActive(false);
    }

    public void ShowData(int index)
    {
        if(SpotDatas.Instance.list[index].dataTypeId == "3")
        {
            canvas.SetActive(true);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(SpotDatas.Instance.list[index].data);
            rawImage.texture = texture;
            rawImage.rectTransform.sizeDelta = new Vector2(texture.width, texture.height);
            rawImage.gameObject.SetActive(true);
        }
        if (SpotDatas.Instance.list[index].dataTypeId == "4")
        {
            canvas.SetActive(true);
            mediaPlayer.OpenMedia(MediaPathType.AbsolutePathOrURL, SpotDatas.Instance.list[index].dataSource.url);
            videoDisplayUI.gameObject.SetActive(true);
        }
    }
}
