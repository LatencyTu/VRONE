using UnityEngine;
using Templete;

//Unity调用Js的测试
public class JsTest : MonoBehaviour
{
    void Start()
    {
        //编辑器内无法调用
#if UNITY_WEBGL && !UNITY_EDITOR
        UnityCallJs();
#endif
    }

    void UnityCallJs()
    {
        //Unity执行js代码,弹窗输出屏幕模式
        //JsFunction.DoJs("alert(screen.orientation.type)");
        //Unity调用js弹窗,输出是否是移动平台
        //JsFunction.UnityAlert(JsFunction.IsMobileBroswer().ToString());
    }
}