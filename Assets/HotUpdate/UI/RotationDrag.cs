using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotationDrag : DragTest
{
    public ThirdPersonController controller;
    // Start is called before the first frame update
    void Start()
    {
        _onDragBegin = (data) =>
        {
            controller._input.LookInput(Vector3.zero);
        };
        _onDrag = ( data) =>
        {
            controller._input.LookInput(Vector3.Normalize(rect.anchoredPosition - start_pos));
        };
        _onDragEnd = (data) =>
        {
            controller._input.LookInput(Vector3.zero);
        };
    }
}
