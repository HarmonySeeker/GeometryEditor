using UnityEngine;

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

    // Метод для проверки изменений параметров (если необходимо)
    protected abstract bool HasParametersChanged();
}