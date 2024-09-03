using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInfo
{
    [SerializeField] private string itemName;
    public GameObject itemPrefab;
}
public class ItemManager : MonoBehaviour
{
    public List<ItemInfo> items;
}
