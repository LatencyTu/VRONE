using UnityEngine;
using Templete;

//Unity����Js�Ĳ���
public class JsTest : MonoBehaviour
{
    void Start()
    {
        //�༭�����޷�����
#if UNITY_WEBGL && !UNITY_EDITOR
        UnityCallJs();
#endif
    }

    void UnityCallJs()
    {
        //Unityִ��js����,���������Ļģʽ
        //JsFunction.DoJs("alert(screen.orientation.type)");
        //Unity����js����,����Ƿ����ƶ�ƽ̨
        //JsFunction.UnityAlert(JsFunction.IsMobileBroswer().ToString());
    }
}