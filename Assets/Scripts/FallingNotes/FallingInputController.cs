using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FallingInputController : MonoBehaviour
{
    public FallingInput fallingInput;
    public float inputTime;
    public Vector3 moveDirection;
    public int playerNum;
    public bool isActive = true;
    public bool canMove = false;
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer outlineRenderer;
    
    void PrepareNote()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = fallingInput.sprite;
        spriteRenderer.color = fallingInput.color;
        transform.localScale = fallingInput.scaleAdjust;
        outlineRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void GetDirection()
    {
        if(GameManager.Instance)
        {
            switch(fallingInput.inputName)
            {
                case "Circle":
                    moveDirection = (GameManager.Instance.circleHitMarks[playerNum].position - transform.position).normalized;
                    break;
                case "Square":
                    moveDirection = (GameManager.Instance.squareHitMarks[playerNum].position - transform.position).normalized;
                    break;
                case "Triangle":
                    moveDirection = (GameManager.Instance.triangleHitMarks[playerNum].position - transform.position).normalized;
                    break;
                case "Diamond":
                    moveDirection = (GameManager.Instance.diamondHitMarks[playerNum].position - transform.position).normalized;
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
                    transform.position = Vector2.Lerp(Vector2.zero, GameManager.Instance.circleHitMarks[playerNum].position * 2, Mathf.Abs(CalculateTimeWithDistance()));
                    break;
                case "Square":
                    transform.position = Vector2.Lerp(Vector2.zero, GameManager.Instance.squareHitMarks[playerNum].position * 2, Mathf.Abs(CalculateTimeWithDistance()));
                    break;
                case "Triangle":
                    transform.position = Vector2.Lerp(Vector2.zero, GameManager.Instance.triangleHitMarks[playerNum].position * 2, Mathf.Abs(CalculateTimeWithDistance()));
                    break;
                case "Diamond":
                    transform.position = Vector2.Lerp(Vector2.zero, GameManager.Instance.diamondHitMarks[playerNum].position * 2, Mathf.Abs(CalculateTimeWithDistance()));
                    break;
            }  
        }
        else
        {
           transform.Translate(GameManager.Instance.scrollSpeed * Time.deltaTime * moveDirection); 
           StartCoroutine(DisableScript());
        }
    }

    private IEnumerator DisableScript()
    {
        yield return new WaitForSeconds(5);
        enabled = false;
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
                MoveTowards();
                if(fallingInput.inputName == "Circle")
                {
                    if(isActive)
                        GameManager.Instance.MissedInput(playerNum, HelperClass.InputType.Circle, transform, moveDirection);
                }
                else if(fallingInput.inputName == "Square")
                {
                    if(isActive)
                        GameManager.Instance.MissedInput(playerNum, HelperClass.InputType.Square, transform, moveDirection);
                }
                else if(fallingInput.inputName == "Triangle")
                {
                    if(isActive)
                        GameManager.Instance.MissedInput(playerNum, HelperClass.InputType.Triangle, transform, moveDirection);
                }
                else if(fallingInput.inputName == "Diamond")
                {
                    if(isActive)
                        GameManager.Instance.MissedInput(playerNum, HelperClass.InputType.Diamond, transform, moveDirection);
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