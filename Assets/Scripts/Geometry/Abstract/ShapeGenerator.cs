using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Абстрактный базовый класс для всех фигур
public abstract class ShapeGenerator : MonoBehaviour
{
    // Абстрактный метод для генерации фигуры
    public abstract Mesh CreateShape();

    protected void GenerateShape()
    {
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }
        meshFilter.mesh = CreateShape();

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material(Shader.Find("Standard"));
        }
    }

    public abstract List<GameObject> GetSliders();

    // Метод для проверки изменений параметров (если необходимо)
    protected abstract bool HasParametersChanged();

    // Метод для хранения изменённых параметров
    protected abstract void StoreLastValues();


    protected abstract void SetUpSliders();

    private void Awake()
    {
        GenerateShape();
        StoreLastValues();
        SetUpSliders();
    }

    private void Start()
    {
        gameObject.AddComponent<MeshCollider>();
    }


    private void Update()
    {
        if (HasParametersChanged())
        {
            GenerateShape();
            StoreLastValues();
        }
    }
}