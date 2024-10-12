using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleGenerator : ShapeGenerator
{
    [SerializeField, Range(6, 20f)] private int sectors; // Количество секторов (вертикальных делений)
    [SerializeField, Range(1, 20f)] private float height; // Высота капсулы
    [SerializeField, Range(1, 20f)] private float radius; // Радиус капсулы

    private int lastSectors; // Для хранения предыдущего значения секторов
    private float lastHeight; // Для хранения предыдущего значения высоты
    private float lastRadius; // Для хранения предыдущего значения радиуса

    private void Start()
    {
        GenerateShape(); // Генерация капсулы при старте
        StoreLastValues(); // Сохранение текущих параметров
    }

    private void Update()
    {
        if (HasParametersChanged()) // Проверка, изменились ли параметры
        {
            GenerateShape(); // Генерация новой капсулы, если параметры изменились
            StoreLastValues(); // Сохранение новых параметров
        }
    }

    // Проверка изменения параметров
    protected override bool HasParametersChanged()
    {
        return sectors != lastSectors || height != lastHeight || radius != lastRadius; // Возвращает true, если параметры изменились
    }

    private void StoreLastValues()
    {
        lastSectors = sectors; // Сохранение текущего количества секторов
        lastHeight = height; // Сохранение текущей высоты
        lastRadius = radius; // Сохранение текущего радиуса
    }

    public override Mesh CreateShape()
    {
        MeshFilter mf = new MeshFilter(); // Создание фильтра меша
        Mesh mesh = new Mesh(); // Создание нового меша

        List<Vector3> verts = new List<Vector3>(); // Список для хранения вершин
        List<int> tris = new List<int>(); // Список для хранения треугольников
        List<Vector2> uvs = new List<Vector2>(); // Список для хранения UV координат
        Vector3 dimensions = Vector3.one; // Ширина, высота и глубина (по умолчанию единица)

        int meridians = 10; // Количество меридианов
        int equatorialMeridian = meridians / 2; // Определение экваториального меридиана

        for (int i = 0; i <= sectors; i++) // Проход по секторам
        {
            float longitude = (Mathf.PI * 2 * i) / sectors; // Угол по окружности
            float verticalOffset = -height / 2; // Смещение по вертикали для центрирования капсулы

            const int extraMeridians = 4; // Дополнительные меридианы для генерации

            int createEquator = extraMeridians - 1; // Счетчик для экваториальной линии

            for (int j = 0; j <= meridians; j++) // Проход по меридианам
            {
                bool emitTriangles = true; // Флаг для создания треугольников

                int effectiveJ = j; // Эффективный индекс меридиана

                if (j == equatorialMeridian) // Если мы на экваториальном меридиане
                {
                    if (createEquator > 0)
                    {
                        if (createEquator == 2) // Обработка нижней части экватора
                        {
                            emitTriangles = false; // Не создавать полигоны на нижней части
                        }
                        if (createEquator == 1) // Обработка верхней части экватора
                        {
                            verticalOffset = -verticalOffset; // Изменяем смещение
                        }

                        createEquator--; // Уменьшаем счетчик
                        j--; // Уменьшаем индекс меридиана
                    }
                    else
                    {
                        emitTriangles = false; // Не создавать полигоны, если мы на экваториальном меридиане
                    }
                }

                int n = verts.Count; // Текущий индекс вершин

                float latitude = (Mathf.PI * effectiveJ) / meridians - Mathf.PI / 2; // Угол по высоте

                // Вычисление координат вершины на основе сферических координат
                Vector3 sphericalPoint = new Vector3(
                    Mathf.Cos(longitude) * Mathf.Cos(latitude) * radius, // X координата
                    Mathf.Sin(latitude) * dimensions.y + verticalOffset, // Y координата
                    Mathf.Sin(longitude) * Mathf.Cos(latitude) * radius); // Z координата

                verts.Add(sphericalPoint); // Добавление вершины в список

                // Вычисление UV координат
                float v = sphericalPoint.y / (dimensions.y * 2 + height) + 0.5f; // Нормализация Y для UV
                Vector2 uvPoint = new Vector2((float)i / sectors, v); // UV координаты
                uvs.Add(uvPoint); // Добавление UV координат в список

                if (emitTriangles) // Если нужно создавать треугольники
                {
                    if (i > 0 && j > 0) // Проверка, чтобы не выходить за границы
                    {
                        int effectiveMeridians = meridians + extraMeridians; // Учет дополнительных меридианов

                        // Добавление индексов треугольников
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

        mesh.vertices = verts.ToArray(); // Установка вершин в меш
        mesh.triangles = tris.ToArray(); // Установка треугольников в меш
        mesh.uv = uvs.ToArray(); // Установка UV координат в меш

        mesh.RecalculateBounds(); // Пересчет границ меша
        mesh.RecalculateNormals(); // Пересчет нормалей меша

        return mesh; // Возврат готового меша
    }
}
