using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingInputController : MonoBehaviour
{
    public FallingInput fallingInput;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = fallingInput.sprite;
        spriteRenderer.color = fallingInput.color;
        transform.localScale = fallingInput.scaleAdjust;
    }
}