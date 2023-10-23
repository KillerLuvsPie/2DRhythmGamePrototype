using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingInputController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void PrepareObject(FallingInput fallingInput)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = fallingInput.sprite;
        spriteRenderer.color = fallingInput.color;
        transform.localScale = fallingInput.scaleAdjust;
    }

    void Start()
    {
        //PrepareObject();
    }
}