using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPlacer : MonoBehaviour
{
    [Header("Placement System")]
    public ItemManager itemManager;
    public Camera mainCamera;
    [SerializeField] private int nitroCount = 10;
    [SerializeField] private int coinCount = 10;

    [Header("Raycasting Settings")]
    public LayerMask placementLayerMask;  // Layer for valid placement areas
    public LayerMask propLayerMask;       // Layer for props that should block placement
    public float rayDistance = 100f;      // Max distance for the raycast

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI nitroText;
    [SerializeField] private TextMeshProUGUI coinText;

    private ItemInfo selectedItem;

    void Update()
    {
        nitroText.text = "Nitro: " + nitroCount.ToString();
        coinText.text = "Coin: " + coinCount.ToString();

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && coinCount > 0 && CanPlaceItem())
        {
            SelectItemByIndex(0);
            PlaceItem();

            coinCount--;
        } else if(Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject() && nitroCount > 0 && CanPlaceItem())
        {
            //&& !EventSystem.current.IsPointerOverGameObject()
            SelectItemByIndex(1);
            PlaceItem();

            nitroCount--;
        }
    }

    private void SelectItemByIndex(int index)
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
            mousePosition.z = mainCamera.nearClipPlane;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 10f;

            Instantiate(selectedItem.itemPrefab, worldPosition, Quaternion.identity);

            Debug.Log("Placing item at: " + worldPosition);

            if (selectedItem.itemPrefab != null)
            {
                Instantiate(selectedItem.itemPrefab, worldPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Selected item prefab is null!");
            }

            selectedItem = null;
        }
    }

    bool CanPlaceItem()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Perform a raycast to check if the target location is obstructed by a prop (2D)
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 0f, propLayerMask);

        if (hit.collider != null)
        {
            // If we hit a prop, block placement
            Debug.Log("Raycast hit prop: " + hit.collider.name);
            return false;
        }

        // If nothing is hit, allow placement
        return true;
    }
}