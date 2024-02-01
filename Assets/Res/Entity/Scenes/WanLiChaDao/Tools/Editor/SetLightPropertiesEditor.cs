using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SetLightProperties))]
public class SetLightPropertiesEditor : Editor
{
    SetLightProperties setLightProperties;

    void OnEnable()
    {
        setLightProperties = target as SetLightProperties;
    }


    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        //
        GUILayout.Space(30);
        EditorGUILayout.HelpBox("Set children width light component properties at onece!", MessageType.Info);
        setLightProperties.toggleA = EditorGUILayout.Foldout(setLightProperties.toggleA, "Set Lights: ");
        if (setLightProperties.toggleA)
        {
            GUILayout.BeginVertical("box");
            setLightProperties.intensity = EditorGUILayout.FloatField("Light Intensity: ", setLightProperties.intensity);
            setLightProperties.lightmapBakeType = (LightmapBakeType)EditorGUILayout.EnumPopup("Light BakeType: ", setLightProperties.lightmapBakeType);
            setLightProperties.lightShadows = (LightShadows)EditorGUILayout.EnumPopup("Light Shadows: ", setLightProperties.lightShadows);
            GUILayout.Space(10);
            if (GUILayout.Button("Set Lights", GUILayout.Height(50)))
            {
                foreach (Transform t in setLightProperties.transform.GetComponentsInChildren<Transform>())
                {
                    if (t.GetComponent<Light>())
                    {
                        Light lightRef = t.GetComponent<Light>();
                        lightRef.type = LightType.Spot;
                        lightRef.intensity = setLightProperties.intensity;
                        lightRef.lightmapBakeType = setLightProperties.lightmapBakeType;
                        lightRef.shadows = setLightProperties.lightShadows;
                        lightRef.gameObject.isStatic = false;
                    }
                }
            }
            GUILayout.EndVertical();
        }

        GUILayout.Space(30);
        EditorGUILayout.HelpBox("Set children Materils by KeyString at onece!", MessageType.Info);
        setLightProperties.toggleB = EditorGUILayout.Foldout(setLightProperties.toggleB, "Set Materils: ");
        if (setLightProperties.toggleB)
        {
            GUILayout.BeginVertical("box");
            setLightProperties.keyStringB = EditorGUILayout.TextField("Check String: ", setLightProperties.keyStringB);
            setLightProperties.matRes = (Material)EditorGUILayout.ObjectField("MatRes: ", setLightProperties.matRes, typeof(Material), false);
            GUILayout.Space(10);
            if (GUILayout.Button("Set Materils", GUILayout.Height(50)))
            {
                foreach (Transform t in setLightProperties.transform.GetComponentsInChildren<Transform>())
                {
                    if (t.GetComponent<MeshRenderer>())
                    {
                        MeshRenderer meshRenderer = t.GetComponent<MeshRenderer>();
                        if (meshRenderer.sharedMaterial.name.Contains(setLightProperties.keyStringB))
                        {
                            meshRenderer.sharedMaterial = setLightProperties.matRes;
                        }

                    }
                }
            }
            GUILayout.EndVertical();
        }

        GUILayout.Space(30);
        EditorGUILayout.HelpBox("Set children Static type by KeyString at onece!", MessageType.Info);
        setLightProperties.toggleC = EditorGUILayout.Foldout(setLightProperties.toggleC, "Set Is Static Toggle: ");
        if (setLightProperties.toggleC)
        {
            GUILayout.BeginVertical("box");
            setLightProperties.keyStringC = EditorGUILayout.TextField("Check String: ", setLightProperties.keyStringC);
            setLightProperties.isStatic = EditorGUILayout.Toggle("Is Static: ", setLightProperties.isStatic);
            GUILayout.Space(10);
            if (GUILayout.Button("Set Is Static Toggle", GUILayout.Height(50)))
            {
                foreach (Transform t in setLightProperties.transform.GetComponentsInChildren<Transform>())
                {
                    if (t.GetComponent<MeshRenderer>())
                    {
                        MeshRenderer meshRenderer = t.GetComponent<MeshRenderer>();
                        if (meshRenderer.material.name.Contains(setLightProperties.keyStringC))
                        {
                            meshRenderer.gameObject.isStatic = setLightProperties.isStatic;
                        }
                    }
                }
            }
            GUILayout.EndVertical();
        }
    }
}
