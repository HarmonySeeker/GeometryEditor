using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreator : MonoBehaviour
{
    [SerializeField] private Material m_Material;
    private Vector3[] verticies = new Vector3[4];
    private Vector2[] uv = new Vector2[4];
    private int[] triangles = new int[6];

    private Mesh mesh;

    private void Start()
    {
        GenerateMeshData();

        mesh = new Mesh();
        mesh.name = "Custom mesh";

        GameObject meshObject = new GameObject("Mesh Object", typeof(MeshRenderer), typeof(MeshFilter));

        meshObject.GetComponent<MeshFilter>().mesh = mesh;
        meshObject.GetComponent<MeshRenderer>().material = m_Material;
        
        mesh.vertices = verticies;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    private void GenerateMeshData()
    {
        verticies[0] = new Vector3(0, 0, 0);
        verticies[1] = new Vector3(0, 1, 0);
        verticies[2] = new Vector3(1, 1, 0);
        verticies[3] = new Vector3(1, 0, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        
        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector3(1, 0);
    }
}
