using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleGenerator : ShapeGenerator
{
    [SerializeField, Range(6, 20f)] private int sectors; // ���������� �������� (������������ �������)
    [SerializeField, Range(1, 20f)] private float height; // ������ �������
    [SerializeField, Range(1, 20f)] private float radius; // ������ �������

    private int lastSectors; // ��� �������� ����������� �������� ��������
    private float lastHeight; // ��� �������� ����������� �������� ������
    private float lastRadius; // ��� �������� ����������� �������� �������

    private void Start()
    {
        GenerateShape(); // ��������� ������� ��� ������
        StoreLastValues(); // ���������� ������� ����������
    }

    private void Update()
    {
        if (HasParametersChanged()) // ��������, ���������� �� ���������
        {
            GenerateShape(); // ��������� ����� �������, ���� ��������� ����������
            StoreLastValues(); // ���������� ����� ����������
        }
    }

    // �������� ��������� ����������
    protected override bool HasParametersChanged()
    {
        return sectors != lastSectors || height != lastHeight || radius != lastRadius; // ���������� true, ���� ��������� ����������
    }

    private void StoreLastValues()
    {
        lastSectors = sectors; // ���������� �������� ���������� ��������
        lastHeight = height; // ���������� ������� ������
        lastRadius = radius; // ���������� �������� �������
    }

    public override Mesh CreateShape()
    {
        MeshFilter mf = new MeshFilter(); // �������� ������� ����
        Mesh mesh = new Mesh(); // �������� ������ ����

        List<Vector3> verts = new List<Vector3>(); // ������ ��� �������� ������
        List<int> tris = new List<int>(); // ������ ��� �������� �������������
        List<Vector2> uvs = new List<Vector2>(); // ������ ��� �������� UV ���������
        Vector3 dimensions = Vector3.one; // ������, ������ � ������� (�� ��������� �������)

        int meridians = 10; // ���������� ����������
        int equatorialMeridian = meridians / 2; // ����������� ��������������� ���������

        for (int i = 0; i <= sectors; i++) // ������ �� ��������
        {
            float longitude = (Mathf.PI * 2 * i) / sectors; // ���� �� ����������
            float verticalOffset = -height / 2; // �������� �� ��������� ��� ������������� �������

            const int extraMeridians = 4; // �������������� ��������� ��� ���������

            int createEquator = extraMeridians - 1; // ������� ��� �������������� �����

            for (int j = 0; j <= meridians; j++) // ������ �� ����������
            {
                bool emitTriangles = true; // ���� ��� �������� �������������

                int effectiveJ = j; // ����������� ������ ���������

                if (j == equatorialMeridian) // ���� �� �� �������������� ���������
                {
                    if (createEquator > 0)
                    {
                        if (createEquator == 2) // ��������� ������ ����� ��������
                        {
                            emitTriangles = false; // �� ��������� �������� �� ������ �����
                        }
                        if (createEquator == 1) // ��������� ������� ����� ��������
                        {
                            verticalOffset = -verticalOffset; // �������� ��������
                        }

                        createEquator--; // ��������� �������
                        j--; // ��������� ������ ���������
                    }
                    else
                    {
                        emitTriangles = false; // �� ��������� ��������, ���� �� �� �������������� ���������
                    }
                }

                int n = verts.Count; // ������� ������ ������

                float latitude = (Mathf.PI * effectiveJ) / meridians - Mathf.PI / 2; // ���� �� ������

                // ���������� ��������� ������� �� ������ ����������� ���������
                Vector3 sphericalPoint = new Vector3(
                    Mathf.Cos(longitude) * Mathf.Cos(latitude) * radius, // X ����������
                    Mathf.Sin(latitude) * dimensions.y + verticalOffset, // Y ����������
                    Mathf.Sin(longitude) * Mathf.Cos(latitude) * radius); // Z ����������

                verts.Add(sphericalPoint); // ���������� ������� � ������

                // ���������� UV ���������
                float v = sphericalPoint.y / (dimensions.y * 2 + height) + 0.5f; // ������������ Y ��� UV
                Vector2 uvPoint = new Vector2((float)i / sectors, v); // UV ����������
                uvs.Add(uvPoint); // ���������� UV ��������� � ������

                if (emitTriangles) // ���� ����� ��������� ������������
                {
                    if (i > 0 && j > 0) // ��������, ����� �� �������� �� �������
                    {
                        int effectiveMeridians = meridians + extraMeridians; // ���� �������������� ����������

                        // ���������� �������� �������������
                        tris.Add(n);
                        tris.Add(n - effectiveMeridians - 1);
                        tris.Add(n - effectiveMeridians);

                        tris.Add(n);
                        tris.Add(n - 1);
                        tris.Add(n - effectiveMeridians - 1);
                    }
                }
            }
        }

        mesh.vertices = verts.ToArray(); // ��������� ������ � ���
        mesh.triangles = tris.ToArray(); // ��������� ������������� � ���
        mesh.uv = uvs.ToArray(); // ��������� UV ��������� � ���

        mesh.RecalculateBounds(); // �������� ������ ����
        mesh.RecalculateNormals(); // �������� �������� ����

        return mesh; // ������� �������� ����
    }
}
