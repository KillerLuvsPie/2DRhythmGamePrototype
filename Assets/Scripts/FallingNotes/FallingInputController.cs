using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FallingInputController : MonoBehaviour
{
    public FallingInput fallingInput;
    public float inputTime;
    public Vector3 moveDirection;
    public bool isActive = true;
    public bool canMove = false;
    private SpriteRenderer spriteRenderer;
    
    void PrepareNote()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = fallingInput.sprite;
        spriteRenderer.color = fallingInput.color;
        transform.localScale = fallingInput.scaleAdjust;
    }

    private void GetDirection()
    {
        if(GameManager.Instance)
        {
            switch(fallingInput.inputName)
            {
                case "Circle":
                    moveDirection = (GameManager.Instance.circleHitZone.position - transform.position).normalized;
                    break;
                case "Square":
                    moveDirection = (GameManager.Instance.squareHitZone.position - transform.position).normalized;
                    break;
                case "Triangle":
                    moveDirection = (GameManager.Instance.triangleHitZone.position - transform.position).normalized;
                    break;
                case "Diamond":
                    moveDirection = (GameManager.Instance.diamondHitZone.position - transform.position).normalized;
                    break;
            }
        }
    }

    private float CalculateTimeWithDistance()
    {
        return (float)
        (
            (inputTime - GameManager.Instance.currentTime - (GameManager.Instance.baseTranslateDuration / GameManager.Instance.scrollSpeed)) /
            GameManager.Instance.baseTranslateDuration * (GameManager.Instance.scrollSpeed / 2)
        );
    }
    private void MoveTowards()
    {
        if(isActive)
        {
            switch(fallingInput.inputName)
            {
                case "Circle":
                    transform.position = Vector2.Lerp(Vector2.zero, GameManager.Instance.circleHitZone.position * 2, Mathf.Abs(CalculateTimeWithDistance()));
                    break;
                case "Square":
                    transform.position = Vector2.Lerp(Vector2.zero, GameManager.Instance.squareHitZone.position * 2, Mathf.Abs(CalculateTimeWithDistance()));
                    break;
                case "Triangle":
                    transform.position = Vector2.Lerp(Vector2.zero, GameManager.Instance.triangleHitZone.position * 2, Mathf.Abs(CalculateTimeWithDistance()));
                    break;
                case "Diamond":
                    transform.position = Vector2.Lerp(Vector2.zero, GameManager.Instance.diamondHitZone.position * 2, Mathf.Abs(CalculateTimeWithDistance()));
                    break;
            }  
        }
        else
            DisableScript();
            
    }

    private void DisableScript()
    {
        this.enabled = false;
    }
    
    void Awake()
    {
        PrepareNote();
    }

    void Start()
    {
        GetDirection();
    }

    void Update()
    {
        if(GameManager.Instance)
        {
            if(canMove /*&& GameManager.Instance.started*/)
            {
                if(fallingInput.inputName == "Circle")
                {
                    MoveTowards();
                    if(isActive)
                        GameManager.Instance.MissedCircle(transform, moveDirection);
                }
                else if(fallingInput.inputName == "Square")
                {
                    MoveTowards();
                    if(isActive)
                        GameManager.Instance.MissedSquare(transform, moveDirection);
                }
                else if(fallingInput.inputName == "Triangle")
                {
                    MoveTowards();
                    if(isActive)
                        GameManager.Instance.MissedTriangle(transform, moveDirection);
                }
                else if(fallingInput.inputName == "Diamond")
                {
                    MoveTowards();
                    if(isActive)
                        GameManager.Instance.MissedDiamond(transform, moveDirection);
                }
            }
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