using UnityEngine;
using Templete;

public class JsTest : MonoBehaviour
{
    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        UnityCallJs();
#endif
    }

    void UnityCallJs()
    {
        //JsFunction.DoJs("alert(screen.orientation.type)");
        //JsFunction.DoJs("screen.orientation.lock('landscape-primary')");
        //TestDebug.Log(JsFunction.IsMobileBroswer());
        //JsFunction.UnityAlert(JsFunction.IsMobileBroswer().ToString());
    }
}