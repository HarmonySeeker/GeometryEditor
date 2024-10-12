using UnityEngine;

public class PrismGenerator : ShapeGenerator
{
    [SerializeField, Range(0, 20f)] private float height = 1f; // Высота призмы
    [SerializeField, Range(3, 20)] private int sideFaces = 3; // Количество боковых граней
    [SerializeField, Range(0, 20f)] private float radius = 1f; // Радиус описанной окружности

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

    // Проверка, изменились ли параметры
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


    // Реализуем абстрактный метод для генерации параллелепипеда
    public override Mesh CreateShape()
    {
        return CreatePrism(height, sideFaces, radius);
    }

    private Mesh CreatePrism(float height, int sideFaces, float radius)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[sideFaces * 2]; // Вершины для верхнего и нижнего оснований
        int[] triangles = new int[sideFaces * 6]; // Треугольники для боковых граней

        // Генерация вершин
        for (int i = 0; i < sideFaces; i++)
        {
            float angle = 2 * Mathf.PI * i / sideFaces; // Вычисляем угол для каждой вершины
            float x = radius * Mathf.Cos(angle);
            float z = radius * Mathf.Sin(angle);

            // Нижнее основание
            vertices[i] = new Vector3(x, 0, z);
            // Верхнее основание
            vertices[i + sideFaces] = new Vector3(x, height, z);
        }

        // Генерация треугольников для боковых граней
        int triangleIndex = 0;
        for (int i = 0; i < sideFaces; i++)
        {
            int next = (i + 1) % sideFaces; // Следующая вершина (цикличность)

            // Измените порядок индексов для боковых граней
            triangles[triangleIndex++] = i + sideFaces; // Текущая вершина верхнего основания
            triangles[triangleIndex++] = next + sideFaces; // Следующая вершина верхнего основания
            triangles[triangleIndex++] = i; // Текущая вершина нижнего основания

            triangles[triangleIndex++] = next + sideFaces; // Следующая вершина верхнего основания
            triangles[triangleIndex++] = next; // Следующая вершина нижнего основания
            triangles[triangleIndex++] = i; // Текущая вершина нижнего основания
        }

        // Создание треугольников для верхнего и нижнего оснований
        int[] baseTriangles = new int[(sideFaces - 2) * 3 * 2]; // Для двух оснований (верхнего и нижнего)
        for (int i = 0; i < sideFaces - 2; i++)
        {
            // Нижнее основание
            baseTriangles[i * 3] = 0; // Центр основания
            baseTriangles[i * 3 + 1] = i + 1; // Текущая вершина
            baseTriangles[i * 3 + 2] = i + 2; // Следующая вершина

            // Верхнее основание
            baseTriangles[(sideFaces - 2) * 3 + i * 3] = sideFaces; // Центр верхнего основания
            baseTriangles[(sideFaces - 2) * 3 + i * 3 + 1] = sideFaces + i + 2; // Следующая вершина верхнего основания
            baseTriangles[(sideFaces - 2) * 3 + i * 3 + 2] = sideFaces + i + 1; // Текущая вершина верхнего основания
        }

        // Объединяем треугольники
        int[] finalTriangles = new int[triangles.Length + baseTriangles.Length];
        triangles.CopyTo(finalTriangles, 0);
        baseTriangles.CopyTo(finalTriangles, triangles.Length);

        mesh.vertices = vertices;
        mesh.triangles = finalTriangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}