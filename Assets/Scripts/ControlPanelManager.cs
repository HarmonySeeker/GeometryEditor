using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanelManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button colorButton;
    [SerializeField] private GameObject sliderPanel;
    
    private List<GameObject> sliderList;
    private GameObject currentObject;
    private Color randColor;

    private void Start()
    {
        deleteButton.onClick.AddListener(RemoveObject);
        colorButton.onClick.AddListener(ChangeColor);
        randColor = Random.ColorHSV();
        colorButton.image.color = randColor;
    }

    private void Update()
    {
        // Проверяем клик левой кнопкой мыши
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Если луч попадает в объект с коллайдером
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("GeometryObject"))
                {
                    if (currentObject != hit.collider.gameObject)
                    {
                        Debug.Log(hit.collider.gameObject.name);
                        currentObject = hit.collider.gameObject;
                        SetUpSliders();
                    }
                }
            }
        }
    }

    private void ChangeColor()
    {
        if (currentObject != null)
        {
            currentObject.GetComponent<MeshRenderer>().material.color = randColor;
            randColor = Random.ColorHSV();
            colorButton.image.color = randColor;
        }
    }

    private void SetUpSliders()
    {
        RemoveSliders();

        sliderList = currentObject.GetComponent<ShapeGenerator>().GetSliders();

        foreach (GameObject slider in sliderList)
        {
            slider.SetActive(true);
            slider.transform.SetParent(sliderPanel.transform);
        }
    }

    private void RemoveObject()
    {
        if (currentObject != null)
        {
            Destroy(currentObject);
            RemoveSliders();
            currentObject = null;
        }
    }

    private void RemoveSliders()
    {
        if (sliderList != null)
        {
            foreach (GameObject slider in sliderList)
            {
                slider.transform.SetParent(slider.transform);
                slider.SetActive(false);
            }

            sliderList = null;
        }
    }
}
