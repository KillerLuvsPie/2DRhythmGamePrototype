using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingInputController : MonoBehaviour
{
    public FallingInput fallingInput;
    private SpriteRenderer spriteRenderer;

    void PrepareNote()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = fallingInput.sprite;
        spriteRenderer.color = fallingInput.color;
        transform.localScale = fallingInput.scaleAdjust;
    }

    void Start()
    {
        PrepareNote();
    }
    void Update()
    {
        if(fallingInput.inputName == "Circle")
            GameManager.Instance.MissedCircle();
        /*else if(fallingInput.name == "Square")
            GameManager.Instance.MissedSquare();
        else if(fallingInput.name == "Triangle")
            GameManager.Instance.MissedTriangle();
        else if(fallingInput.name == "Diamond")
            GameManager.Instance.MissedDiamond();*/
    }
}