using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingInputController : MonoBehaviour
{
    public FallingInput fallingInput;
    public float inputTime;
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
        if(GameManager.Instance)
        {
            if(fallingInput.inputName == "Circle")
                GameManager.Instance.MissedCircle();
            else if(fallingInput.inputName == "Square")
                GameManager.Instance.MissedSquare();
            else if(fallingInput.inputName == "Triangle")
                GameManager.Instance.MissedTriangle();
            else if(fallingInput.inputName == "Diamond")
                GameManager.Instance.MissedDiamond();
        }
        
    }
}
/*
NOTE TIME FOR CHART:

REPEAT EVERY 7 SECONDS UNTIL 01:24
00:07
00:10.180
00:10.5

STARTS AT 00:14 AND REPEATS EVERY 3.5 SECONDS
00:14
00:14.40
00:14.50
00:14.90
00:15
00:15.975
00:16.075
00:16.425
00:16.65
00:16.75

*/