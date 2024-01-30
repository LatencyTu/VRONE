using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Templete;
using DG.Tweening;
using System;
public enum PosType
{
    Left,
    Right
}
public class DragTest : MonoBehaviour
{

    public Vector2 start_pos;
    public float duration = 0.5f;
    public float maxDistance = 50;
    public PosType posType = PosType.Left;
    public Action<BaseEventData> _onDrag;
    public Action<BaseEventData> _onDragBegin;
    public Action<BaseEventData> _onDragEnd;
    public RectTransform rect;
    private void Awake()
    {
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
        Vector2 target = start_pos;
        Vector2 left = Input.touches[0].position;
        Vector2 right = Input.touches[0].position;
        foreach (Touch touch in Input.touches)
        {
            if (touch.position.x >= right.x) right = touch.position;
            if (touch.position.x <= left.x) left = touch.position;
        }
        if (posType == PosType.Left) target = left;
        else target = right;

        float distance = Vector2.Distance(target, start_pos);
        
        //if (distance > maxDistance) target = target * (maxDistance / distance);
        rect.position = start_pos+(target- start_pos);
        if (_onDrag != null) _onDrag?.Invoke(eventData);
    }
    public void OnDragBegin(BaseEventData eventData)
    {
        if (_onDragBegin != null) _onDragBegin?.Invoke(eventData);
    }
    public void OnDragEnd(BaseEventData eventData)
    {
        DOTween.To(() => rect.anchoredPosition, _pos => rect.anchoredPosition = _pos, start_pos, duration);
        if (_onDragEnd != null) _onDragEnd?.Invoke(eventData);
    }
}
