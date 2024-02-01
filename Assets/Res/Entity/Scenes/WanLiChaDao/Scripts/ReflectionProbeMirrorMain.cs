using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionProbeMirro : MonoBehaviour
{
    public Transform viewCamera;
    public Transform reflectionPanel;
    public Transform lightProbes;



    void Start()
    {

    }


    void Update()
    {
        Action();
    }

    void Action()
    {
        float y = reflectionPanel.position.y - viewCamera.position.y;
        lightProbes.position = new Vector3(viewCamera.position.x, reflectionPanel.position.y + y, viewCamera.position.z);
    }
}
