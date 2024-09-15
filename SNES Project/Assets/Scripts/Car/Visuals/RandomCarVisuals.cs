using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCarVisuals : MonoBehaviour
{
    [SerializeField] private Sprite[] carSprites;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.color = Color.white;

        spriteRenderer.sprite = carSprites[Random.Range(0, carSprites.Length)];
    }
}
