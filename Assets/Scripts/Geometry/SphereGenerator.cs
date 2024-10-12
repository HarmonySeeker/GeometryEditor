using UnityEngine;

public class SphereGenerator : ShapeGenerator
{
    [SerializeField, Range(0, 20f)] private float radius = 1f;
    [SerializeField, Range(0, 100)] private int longitudeSegments = 24;
    [SerializeField, Range(0, 100)] private int latitudeSegments = 24;

    private float lastRadius;
    private int lastLongitudeSegments;
    private int lastLatitudeSegments;

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

    // Реализуем абстрактный метод для генерации сферы
    public override Mesh CreateShape()
    {
        return CreateSphere(radius, longitudeSegments, latitudeSegments);
    }

    // Проверка, изменились ли параметры
    protected override bool HasParametersChanged()
    {
        return radius != lastRadius ||
               longitudeSegments != lastLongitudeSegments ||
               latitudeSegments != lastLatitudeSegments;
    }

    private void StoreLastValues()
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
}