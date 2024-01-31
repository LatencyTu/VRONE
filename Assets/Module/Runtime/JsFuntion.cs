using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

//Js交互的方法
public class JsFunction : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void DoJs(string core);

    [DllImport("__Internal")]
    public static extern void Hello();

    [DllImport("__Internal")]
    public static extern bool IsMobileBroswer();

    [DllImport("__Internal")]
    public static extern bool IsMobileBroswer2();

    [DllImport("__Internal")]
    public static extern void UnitySetFullscreen();

    [DllImport("__Internal")]
    public static extern void UnityAlert(string msg);

    [DllImport("__Internal")]
    public static extern void HelloString(string str);

    [DllImport("__Internal")]
    public static extern void PrintFloatArray(float[] array, int size);

    [DllImport("__Internal")]
    public static extern int AddNumbers(int x, int y);

    [DllImport("__Internal")]
    public static extern string StringReturnValueFunction();

    [DllImport("__Internal")]
    public static extern void BindWebGLTexture(int texture);

}
