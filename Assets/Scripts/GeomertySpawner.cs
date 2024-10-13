using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GeomertySpawner : MonoBehaviour
{
    [SerializeField] private ShapeGenerator _generator;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(generateObject);
    }

    private void generateObject()
    {
        Instantiate(_generator);
    }
}
