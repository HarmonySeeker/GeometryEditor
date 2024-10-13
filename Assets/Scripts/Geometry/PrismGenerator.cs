using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrismGenerator : ShapeGenerator
{
    [SerializeField] private Slider heightSlider;
    [SerializeField] private Slider sideFacesSlider;
    [SerializeField] private Slider radiusSlider;

    [SerializeField] private TextMeshProUGUI heightText;
    [SerializeField] private TextMeshProUGUI sideFacesText;
    [SerializeField] private TextMeshProUGUI radiusText;

    [SerializeField, Range(1, 20f)] private float height = 1f; // ������ ������
    [SerializeField, Range(3, 20)] private int sideFaces = 3; // ���������� ������� ������
    [SerializeField, Range(1, 20f)] private float radius = 1f; // ������ ��������� ����������

    private float lastHeight;
    private int lastSideFaces;
    private float lastRadius;

    // ��������, ���������� �� ���������
    protected override bool HasParametersChanged()
    {
        return height != lastHeight || sideFaces != lastSideFaces || radius != lastRadius;
    }

    protected override void StoreLastValues()
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

    public override List<GameObject> GetSliders()
    {
        return new List<GameObject>()
        {
            heightSlider.gameObject,
            sideFacesSlider.gameObject,
            radiusSlider.gameObject
        };
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

    protected override void SetUpSliders()
    {
        heightSlider.minValue = 1f;
        heightSlider.maxValue = 20f;
        heightSlider.value = height;
        heightText.text = $"������: {height}";
        heightSlider.onValueChanged.AddListener(delegate { setHeight(heightSlider.value); });

        sideFacesSlider.minValue = 3;
        sideFacesSlider.maxValue = 20;
        sideFacesSlider.value = sideFaces;
        sideFacesText.text = $"�����: {sideFaces}";
        sideFacesSlider.onValueChanged.AddListener(delegate { setSideFaces((int)sideFacesSlider.value); });

        radiusSlider.minValue = 1f;
        radiusSlider.maxValue = 20f;
        radiusSlider.value = radius;
        radiusText.text = $"������: {radius}";
        radiusSlider.onValueChanged.AddListener(delegate { setRadius(radiusSlider.value); });
    }

    private void setSideFaces(int sideFacesValue)
    {
        sideFaces = sideFacesValue;
        sideFacesText.text = $"�����: {sideFaces}";
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