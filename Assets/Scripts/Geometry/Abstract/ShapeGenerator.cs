using UnityEngine;

// ����������� ������� ����� ��� ���� �����
public abstract class ShapeGenerator : MonoBehaviour
{
    // ����������� ����� ��� ��������� ������
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

    // ����� ��� �������� ��������� ���������� (���� ����������)
    protected abstract bool HasParametersChanged();
}