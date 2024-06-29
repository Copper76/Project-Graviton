using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class InvertMesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Get the MeshFilter component
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        if (mesh != null)
        {
            // Reverse the normals
            Vector3[] normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = -normals[i];
            }
            mesh.normals = normals;

            // Flip the winding order of the triangles
            int[] triangles = mesh.triangles;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int temp = triangles[i];
                triangles[i] = triangles[i + 1];
                triangles[i + 1] = temp;
            }
            mesh.triangles = triangles;
        }
        Destroy(this);
    }
}
