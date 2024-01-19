using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Templete;
using DG.Tweening;
using System;

public class DragTest : MonoBehaviour
{
    public Vector3 start_pos;
    public float duration = 0.5f;
    public float maxDistance = 50;
    public RectTransform rect;
    public Action<BaseEventData> _onDrag;
    public Action<BaseEventData> _onDragBegin;
    public Action<BaseEventData> _onDragEnd;
    private void Awake()
    {
        start_pos = gameObject.GetComponent<RectTransform>().position;
        rect = gameObject.GetComponent<RectTransform>();
    }
    public void OnDrag(BaseEventData eventData )
    {
        float distance = Vector3.Distance(Input.mousePosition, start_pos);
        Vector3 dir = Vector3.Normalize(Input.mousePosition - start_pos);
        Vector3 target_pos = distance < maxDistance ? Input.mousePosition : start_pos+ dir * maxDistance;
        rect.position = target_pos;
        if (_onDrag != null) _onDrag?.Invoke(eventData);
    }
    public void OnDragBegin(BaseEventData eventData)
    {
        if (_onDragBegin != null) _onDragBegin?.Invoke(eventData);
        //gameObject.GetComponent<RectTransform>().position = eventData.position;
    }
    public void OnDragEnd(BaseEventData eventData)
    {
        rect.DOMove(start_pos, duration);
        //rect.position = start_pos;
        if (_onDragEnd != null) _onDragEnd?.Invoke(eventData);
    }
}
