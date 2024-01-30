using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Templete;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotationDrag : DragTest
{
    public ThirdPersonController controller;
    // Start is called before the first frame update
    GameGlobar GameGlobar;
    void Start()
    {
        GameGlobar = GameObject.Find("MainUICanvas").GetComponent<GameGlobar>();
        CheckTick.AddRule(findctrl, addevent);
        bool findctrl()
        {
            controller = (GameGlobar.Map["Player"] as GameObject).GetComponentInChildren<ThirdPersonController>();
            return controller;
        }
        bool addevent()
        {
            _onDragBegin = (data) =>
            {
                controller._input.LookInput(Vector3.zero);
            };
            _onDrag = (data) =>
            {
                controller._input.LookInput(Vector3.Normalize(rect.anchoredPosition - start_pos));
            };
            _onDragEnd = (data) =>
            {
                controller._input.LookInput(Vector3.zero);
            };
            return true;
        }   
    }
}
