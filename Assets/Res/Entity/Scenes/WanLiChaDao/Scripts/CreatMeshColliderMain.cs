using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatMeshColliderMain : MonoBehaviour
{
    List<CombineInstance> combineInstances = new List<CombineInstance>();
    CombineInstance combineInstance;
    Mesh combineMesh;
    MeshCollider meshCollider;

    void Start()
    {
        foreach (Transform t in transform.GetComponentsInChildren<Transform>())
        {
            if (t.GetComponent<MeshFilter>())
            {
                combineInstance = new CombineInstance
                {
                    mesh = t.GetComponent<MeshFilter>().mesh,
                    transform = Matrix4x4.TRS(t.position, t.rotation, t.localScale)
                };

                combineInstances.Add(combineInstance);
            }
        }

        //
        combineMesh = new Mesh { indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
        combineMesh.CombineMeshes(combineInstances.ToArray());
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = combineMesh;
        //meshCollider.convex = true;
    }


    void Update()
    {

    }
}
