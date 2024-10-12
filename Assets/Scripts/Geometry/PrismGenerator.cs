using UnityEngine;

public class PrismGenerator : ShapeGenerator
{
    [SerializeField, Range(0, 20f)] private float height = 1f; // ������ ������
    [SerializeField, Range(3, 20)] private int sideFaces = 3; // ���������� ������� ������
    [SerializeField, Range(0, 20f)] private float radius = 1f; // ������ ��������� ����������

    private float lastHeight;
    private int lastSideFaces;
    private float lastRadius;

    private void Start()
    {
        GenerateShape();
        StoreLastValues();
    }

    private void Update()
    {
        if (HasParametersChanged())
        {
            GenerateShape();
            StoreLastValues();
        }
    }

    // ��������, ���������� �� ���������
    protected override bool HasParametersChanged()
    {
        return height != lastHeight || sideFaces != lastSideFaces || radius != lastRadius;
    }

    private void StoreLastValues()
    {
        lastHeight = height;
        lastSideFaces = sideFaces;
        lastRadius = radius;
    }


    // ��������� ����������� ����� ��� ��������� ���������������
    public override Mesh CreateShape()
    {
        return CreatePrism(height, sideFaces, radius);
    }

    private Mesh CreatePrism(float height, int sideFaces, float radius)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[sideFaces * 2]; // ������� ��� �������� � ������� ���������
        int[] triangles = new int[sideFaces * 6]; // ������������ ��� ������� ������

        // ��������� ������
        for (int i = 0; i < sideFaces; i++)
        {
            float angle = 2 * Mathf.PI * i / sideFaces; // ��������� ���� ��� ������ �������
            float x = radius * Mathf.Cos(angle);
            float z = radius * Mathf.Sin(angle);

            // ������ ���������
            vertices[i] = new Vector3(x, 0, z);
            // ������� ���������
            vertices[i + sideFaces] = new Vector3(x, height, z);
        }

        // ��������� ������������� ��� ������� ������
        int triangleIndex = 0;
        for (int i = 0; i < sideFaces; i++)
        {
            int next = (i + 1) % sideFaces; // ��������� ������� (�����������)

            // �������� ������� �������� ��� ������� ������
            triangles[triangleIndex++] = i + sideFaces; // ������� ������� �������� ���������
            triangles[triangleIndex++] = next + sideFaces; // ��������� ������� �������� ���������
            triangles[triangleIndex++] = i; // ������� ������� ������� ���������

            triangles[triangleIndex++] = next + sideFaces; // ��������� ������� �������� ���������
            triangles[triangleIndex++] = next; // ��������� ������� ������� ���������
            triangles[triangleIndex++] = i; // ������� ������� ������� ���������
        }

        // �������� ������������� ��� �������� � ������� ���������
        int[] baseTriangles = new int[(sideFaces - 2) * 3 * 2]; // ��� ���� ��������� (�������� � �������)
        for (int i = 0; i < sideFaces - 2; i++)
        {
            // ������ ���������
            baseTriangles[i * 3] = 0; // ����� ���������
            baseTriangles[i * 3 + 1] = i + 1; // ������� �������
            baseTriangles[i * 3 + 2] = i + 2; // ��������� �������

            // ������� ���������
            baseTriangles[(sideFaces - 2) * 3 + i * 3] = sideFaces; // ����� �������� ���������
            baseTriangles[(sideFaces - 2) * 3 + i * 3 + 1] = sideFaces + i + 2; // ��������� ������� �������� ���������
            baseTriangles[(sideFaces - 2) * 3 + i * 3 + 2] = sideFaces + i + 1; // ������� ������� �������� ���������
        }

        // ���������� ������������
        int[] finalTriangles = new int[triangles.Length + baseTriangles.Length];
        triangles.CopyTo(finalTriangles, 0);
        baseTriangles.CopyTo(finalTriangles, triangles.Length);

        mesh.vertices = vertices;
        mesh.triangles = finalTriangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}