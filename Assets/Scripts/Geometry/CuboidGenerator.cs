using UnityEngine;


public class CuboidGenerator : ShapeGenerator
{
    [SerializeField, Range(0, 20f)] private float width = 1f;    // ������
    [SerializeField, Range(0, 20f)] private float height = 1f;   // ������
    [SerializeField, Range(0, 20f)] private float depth = 1f;    // �����

    private float lastWidth;
    private float lastHeight;
    private float lastDepth;

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

    // ��������� ����������� ����� ��� ��������� ���������������
    public override Mesh CreateShape()
    {
        return CreateCuboid(width, height, depth);
    }

    // ��������, ���������� �� ���������
    protected override bool HasParametersChanged()
    {
        return width != lastWidth || height != lastHeight || depth != lastDepth;
    }

    private void StoreLastValues()
    {
        lastWidth = width;
        lastHeight = height;
        lastDepth = depth;
    }

    private Mesh CreateCuboid(float width, float height, float depth)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[8]
        {
            new Vector3(-width / 2, -height / 2, -depth / 2), // 0
            new Vector3(width / 2, -height / 2, -depth / 2),  // 1
            new Vector3(width / 2, height / 2, -depth / 2),   // 2
            new Vector3(-width / 2, height / 2, -depth / 2),  // 3
            new Vector3(-width / 2, -height / 2, depth / 2),   // 4
            new Vector3(width / 2, -height / 2, depth / 2),    // 5
            new Vector3(width / 2, height / 2, depth / 2),     // 6
            new Vector3(-width / 2, height / 2, depth / 2)     // 7
        };

        int[] triangles = new int[36]
        {
            0, 2, 1, 0, 3, 2, // �������� �����
            4, 5, 6, 4, 6, 7, // ������ �����
            0, 1, 5, 0, 5, 4, // ������ �����
            2, 3, 7, 2, 7, 6, // ������� �����
            0, 4, 7, 0, 7, 3, // ����� �����
            1, 2, 6, 1, 6, 5  // ������ �����
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}