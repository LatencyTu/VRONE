using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Templete;
using DG.Tweening;
using System;
//摇杆位置
public enum PosType
{
    Left,
    Right
}
//拖拽基类
public class DragTest : MonoBehaviour
{
    //复原位置
    public Vector2 start_pos;
    //复原时间
    public float duration = 0.5f;
    public float maxDistance = 50;
    //摇杆位置
    public PosType posType = PosType.Left;
    //拖拽中事件
    public Action<BaseEventData> _onDrag;
    //开始拖拽时事件
    public Action<BaseEventData> _onDragBegin;
    //结束拖拽时事件
    public Action<BaseEventData> _onDragEnd;
    public RectTransform rect;
    //存放全局变量的类
    public GameGlobar GameGlobar;
    private void Awake()
    {
        GameGlobar = GameObject.Find("MainUICanvas").GetComponent<GameGlobar>();
        //判断是否移动端，非移动端隐藏
#if UNITY_WEBGL && !UNITY_EDITOR
       CheckIsMobile();
#endif
#if UNITY_EDITOR
        //transform.parent.gameObject.SetActive(false);
#endif
        start_pos = rect.anchoredPosition;
    }

    void CheckIsMobile()
    {
        transform.parent.gameObject.SetActive(JsFunction.IsMobileBroswer());
    }
    public void OnDrag(BaseEventData eventData )
    {
        Vector2 loaclPos;
        //编辑器下，将鼠标坐标转换为当前UI父物体坐标系的坐标
#if UNITY_EDITOR
        TestDebug.Log("Input.mousePosition" + Input.mousePosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect.parent as RectTransform, Input.mousePosition, GameObject.Find("UICamera").GetComponent<Camera>(), out loaclPos);
#endif

#if !UNITY_EDITOR && UNITY_WEBGL
        //在移动平台下，当多点触控的时候
        //判断横竖屏，判断左右摇杆
        //将触点坐标转换为当前UI父物体坐标系的坐标
        if (JsFunction.IsMobileBroswer() && Input.touches.Length>1)
        {
            Vector2 target;
            Vector2 left = Input.touches[0].position;
            Vector2 right = Input.touches[0].position;
            bool isLand = (bool)GameGlobar.Map["IsLand"];
            if (isLand)
            {
                foreach (Touch touch in Input.touches)
                {
                    if (touch.position.y <= right.y) right = touch.position;
                    if (touch.position.y >= left.y) left = touch.position;
                }
            }
            else
            {
                foreach (Touch touch in Input.touches)
                {
                    if (touch.position.x >= right.x) right = touch.position;
                    if (touch.position.x <= left.x) left = touch.position;
                }
            }
            if (posType == PosType.Left) target = left;
            else target = right;
            TestDebug.Log("target" + target);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect.parent as RectTransform, target, GameObject.Find("UICamera").GetComponent<Camera>(), out loaclPos);
        }
        //在移动平台下，单点触控的时候
        //将鼠标坐标转换为当前UI父物体坐标系的坐标
        else
        {
            TestDebug.Log("Input.mousePosition" + Input.mousePosition);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect.parent as RectTransform, Input.mousePosition, GameObject.Find("UICamera").GetComponent<Camera>(), out loaclPos);
            TestDebug.Log("loaclPos" + loaclPos);
        }
#endif
        TestDebug.Log("loaclPos" + loaclPos);
        rect.anchoredPosition = loaclPos;
        if (_onDrag != null) _onDrag?.Invoke(eventData);
    }
    public void OnDragBegin(BaseEventData eventData)
    {
        start_pos = rect.anchoredPosition;
        if (_onDragBegin != null) _onDragBegin?.Invoke(eventData);
    }
    public void OnDragEnd(BaseEventData eventData)
    {
        //复原
        DOTween.To(() => rect.anchoredPosition, _pos => rect.anchoredPosition = _pos, start_pos, duration);
        if (_onDragEnd != null) _onDragEnd?.Invoke(eventData);
    }
}
