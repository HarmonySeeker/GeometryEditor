using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public abstract List<GameObject> GetSliders();

    // ����� ��� �������� ��������� ���������� (���� ����������)
    protected abstract bool HasParametersChanged();

    // ����� ��� �������� ��������� ����������
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