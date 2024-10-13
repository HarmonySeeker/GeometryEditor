using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CuboidGenerator : ShapeGenerator
{
    [SerializeField] private Slider widthSlider;
    [SerializeField] private Slider heightSlider;
    [SerializeField] private Slider depthSlider;

    [SerializeField] private TextMeshProUGUI widthText;
    [SerializeField] private TextMeshProUGUI heightText;
    [SerializeField] private TextMeshProUGUI depthText;

    [SerializeField, Range(1, 20f)] private float width = 1f;    // Ширина
    [SerializeField, Range(1, 20f)] private float height = 1f;   // Высота
    [SerializeField, Range(1, 20f)] private float depth = 1f;    // Длина

    private float lastWidth;
    private float lastHeight;
    private float lastDepth;

    // Реализуем абстрактный метод для генерации параллелепипеда
    public override Mesh CreateShape()
    {
        return CreateCuboid(width, height, depth);
    }

    public override List<GameObject> GetSliders()
    {
        return new List<GameObject>()
        {
            widthSlider.gameObject,
            heightSlider.gameObject,
            depthSlider.gameObject
        };
    }

    // Проверка, изменились ли параметры
    protected override bool HasParametersChanged()
    {
        return width != lastWidth || height != lastHeight || depth != lastDepth;
    }

    protected override void StoreLastValues()
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
            0, 2, 1, 0, 3, 2, // передняя грань
            4, 5, 6, 4, 6, 7, // задняя грань
            0, 1, 5, 0, 5, 4, // нижняя грань
            2, 3, 7, 2, 7, 6, // верхняя грань
            0, 4, 7, 0, 7, 3, // левая грань
            1, 2, 6, 1, 6, 5  // правая грань
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    protected override void SetUpSliders()
    {
        widthSlider.minValue = 1f;
        widthSlider.maxValue = 20f;
        widthSlider.value = width;
        widthText.text = $"Ширина: {width}";
        widthSlider.onValueChanged.AddListener(delegate { setWidth(widthSlider.value); });

        heightSlider.minValue = 1f;
        heightSlider.maxValue = 20f;
        heightSlider.value = height;
        heightText.text = $"Высота: {height}";
        heightSlider.onValueChanged.AddListener(delegate { setHeight(heightSlider.value); });

        depthSlider.minValue = 1f;
        depthSlider.maxValue = 20f;
        depthSlider.value = depth;
        depthText.text = $"Глубина: {depth}";
        depthSlider.onValueChanged.AddListener(delegate { setDepth(depthSlider.value); });
    }

    private void setWidth(float widthValue)
    {
        width = widthValue;
        widthText.text = $"Ширина: {width}";
    }

    private void setHeight(float heightValue)
    {
        height = heightValue;
        heightText.text = $"Высота: {height}";
    }

    private void setDepth(float depthValue)
    {
        depth = depthValue;
        depthText.text = $"Глубина: {depth}";
    }
}