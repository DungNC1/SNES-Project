using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacer : MonoBehaviour
{
    public ItemManager itemManager;
    public Camera mainCamera;
    [SerializeField] private int itemCount = 10;

    private ItemInfo selectedItem;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && selectedItem != null && itemCount >= 0)
        {
            PlaceItem();

            itemCount--;
        }
    }

    public void SelectItemByIndex(int index)
    {
        if (index >= 0 && index < itemManager.items.Count)
        {
            selectedItem = itemManager.items[index];
        }
        else
        {
            Debug.LogWarning("Invalid item index: " + index);
        }
    }

    void PlaceItem()
    {
        if (selectedItem != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f;  
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            Instantiate(selectedItem.itemPrefab, worldPosition, Quaternion.identity);
            // selectedItem = null;  
        }
    }
}