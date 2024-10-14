using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SphereGenerator : ShapeGenerator
{
    [SerializeField] private Slider radiusSlider;
    [SerializeField] private Slider longitudeSlider;
    [SerializeField] private Slider latitudeSlider;

    [SerializeField] private TextMeshProUGUI radiusText;
    [SerializeField] private TextMeshProUGUI longitudeText;
    [SerializeField] private TextMeshProUGUI latitudeText;

    [SerializeField, Range(1f, 20f)] private float radius = 1f;
    [SerializeField, Range(1, 100)] private int longitudeSegments = 24;
    [SerializeField, Range(1, 100)] private int latitudeSegments = 24;

    private float lastRadius;
    private int lastLongitudeSegments;
    private int lastLatitudeSegments;

    // Реализуем абстрактный метод для генерации сферы
    public override Mesh CreateShape()
    {
        return CreateSphere(radius, longitudeSegments, latitudeSegments);
    }
    public override List<GameObject> GetSliders()
    {
        return new List<GameObject>()
        {
            radiusSlider.gameObject,
            longitudeSlider.gameObject,
            latitudeSlider.gameObject
        };
    }

    // Проверка, изменились ли параметры
    protected override bool HasParametersChanged()
    {
        return radius != lastRadius ||
               longitudeSegments != lastLongitudeSegments ||
               latitudeSegments != lastLatitudeSegments;
    }

    protected override void StoreLastValues()
    {
        lastRadius = radius;
        lastLongitudeSegments = longitudeSegments;
        lastLatitudeSegments = latitudeSegments;
    }

    private Mesh CreateSphere(float radius, int longitudeSegments, int latitudeSegments)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[(longitudeSegments + 1) * (latitudeSegments + 1)];
        int[] triangles = new int[longitudeSegments * latitudeSegments * 6];

        int vertexIndex = 0;
        for (int lat = 0; lat <= latitudeSegments; lat++)
        {
            float theta = lat * Mathf.PI / latitudeSegments;
            float sinTheta = Mathf.Sin(theta);
            float cosTheta = Mathf.Cos(theta);

            for (int lon = 0; lon <= longitudeSegments; lon++)
            {
                float phi = lon * 2 * Mathf.PI / longitudeSegments;
                float sinPhi = Mathf.Sin(phi);
                float cosPhi = Mathf.Cos(phi);

                Vector3 vertex = new Vector3(
                    radius * cosPhi * sinTheta,
                    radius * cosTheta,
                    radius * sinPhi * sinTheta
                );

                vertices[vertexIndex] = vertex;
                vertexIndex++;
            }
        }

        int triangleIndex = 0;
        for (int lat = 0; lat < latitudeSegments; lat++)
        {
            for (int lon = 0; lon < longitudeSegments; lon++)
            {
                int first = (lat * (longitudeSegments + 1)) + lon;
                int second = first + longitudeSegments + 1;

                triangles[triangleIndex++] = first;
                triangles[triangleIndex++] = first + 1;
                triangles[triangleIndex++] = second;

                triangles[triangleIndex++] = second;
                triangles[triangleIndex++] = first + 1;
                triangles[triangleIndex++] = second + 1;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    protected override void SetUpSliders()
    {
        radiusSlider.minValue = 1f;
        radiusSlider.maxValue = 20f;
        radiusSlider.value = radius;
        radiusText.text = $"Радиус: {radius}";
        radiusSlider.onValueChanged.AddListener(delegate { setRadius(radiusSlider.value); });

        longitudeSlider.minValue = 1;
        longitudeSlider.maxValue = 100;
        longitudeSlider.value = longitudeSegments;
        longitudeText.text = $"Сегменты (долгота): {longitudeSegments}";
        longitudeSlider.onValueChanged.AddListener(delegate { setLongitude((int)longitudeSlider.value); });

        latitudeSlider.minValue = 1;
        latitudeSlider.maxValue = 100;
        latitudeSlider.value = latitudeSegments;
        latitudeText.text = $"Сегменты (широта): {latitudeSegments}";
        latitudeSlider.onValueChanged.AddListener(delegate { setLatitude((int)latitudeSlider.value); });
    }

    private void setRadius(float radiusValue)
    {
        radius = radiusValue;
        radiusText.text = $"Радиус: {radius}";
    }

    private void setLongitude(int longitudeValue)
    {
        longitudeSegments = longitudeValue;
        longitudeText.text = $"Сегменты (долгота): {longitudeSegments}";
    }

    private void setLatitude(int latitudeValue)
    {
        latitudeSegments = latitudeValue;
        latitudeText.text = $"Сегменты (широта): {latitudeSegments}";
    }
}