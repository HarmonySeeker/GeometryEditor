using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CapsuleGenerator : ShapeGenerator
{
    [SerializeField] private Slider sectorSlider;
    [SerializeField] private Slider heightSlider;
    [SerializeField] private Slider radiusSlider;

    [SerializeField] private TextMeshProUGUI sectorText;
    [SerializeField] private TextMeshProUGUI heightText;
    [SerializeField] private TextMeshProUGUI radiusText;

    [SerializeField, Range(6, 20f)] private int sectors; // ���������� �������� (������������ �������)
    [SerializeField, Range(1, 20f)] private float height; // ������ �������
    [SerializeField, Range(1, 20f)] private float radius; // ������ �������

    private int lastSectors; // ��� �������� ����������� �������� ��������
    private float lastHeight; // ��� �������� ����������� �������� ������
    private float lastRadius; // ��� �������� ����������� �������� �������

    public override Mesh CreateShape()
    {
        Mesh mesh = new Mesh();

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

        return mesh;
    }

    public override List<GameObject> GetSliders()
    {
        return new List<GameObject>()
        {
            heightSlider.gameObject,
            radiusSlider.gameObject,
            sectorSlider.gameObject
        };
    }

    // �������� ��������� ����������
    protected override bool HasParametersChanged()
    {
        return sectors != lastSectors || height != lastHeight || radius != lastRadius;
    }

    protected override void StoreLastValues()
    {
        lastSectors = sectors;
        lastHeight = height;
        lastRadius = radius;
    }

    protected override void SetUpSliders()
    {
        sectorSlider.minValue = 6;
        sectorSlider.maxValue = 20;
        sectorSlider.value = sectors;
        sectorText.text = $"�����: {sectors}";
        sectorSlider.onValueChanged.AddListener(delegate { setSector((int)sectorSlider.value); });

        heightSlider.minValue = 1f;
        heightSlider.maxValue = 20f;
        heightSlider.value = height;
        heightText.text = $"������: {height}";
        heightSlider.onValueChanged.AddListener(delegate { setHeight(heightSlider.value); });

        radiusSlider.minValue = 1f;
        radiusSlider.maxValue = 20f;
        radiusSlider.value = radius;
        radiusText.text = $"������: {radius}";
        radiusSlider.onValueChanged.AddListener(delegate { setRadius(radiusSlider.value); });
    }

    private void setSector(int sectorValue)
    {
        sectors = sectorValue;
        sectorText.text = $"�����: {sectors}";
    }

    private void setHeight(float heightValue)
    {
        height = heightValue;
        heightText.text = $"������: {height}";
    }

    private void setRadius(float radiusValue)
    {
        radius = radiusValue;
        radiusText.text = $"������: {radius}";
    }
}
