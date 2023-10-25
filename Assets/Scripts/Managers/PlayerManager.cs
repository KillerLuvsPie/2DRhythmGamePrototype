using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //SINGLETON
    public PlayerManager Instance;
    //HIT MARKERS
    public SpriteRenderer circleHitMarker;
    public SpriteRenderer squareHitMarker;
    public SpriteRenderer triangleHitMarker;
    public SpriteRenderer diamondHitMarker;
    //HIT MARKER COLORS
    private Color red = new Color(1,0,0,1);
    private Color blue = new Color(0,0,1,1);
    private Color green = new Color(0,1,0,1);
    private Color yellow = new Color(1,1,0,1);
    private Color white = new Color(1,1,1,1);
    //CONTROL VARIABLES
    private bool isCircleHeld = false;
    private bool isSquareHeld = false;
    private bool isTriangleHeld = false;
    private bool isDiamondHeld = false;
    //UNITY FUNCTIONS
    void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            circleHitMarker.color = red;
            if(isCircleHeld == false)
            {
                isCircleHeld = true;
                GameManager.Instance.CirclePressed();
            }
        }
        else if(Input.GetKeyUp(KeyCode.A))
        {
            circleHitMarker.color = white;
            isCircleHeld = false;
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            squareHitMarker.color = blue;
            if(isSquareHeld == false)
            {
                isSquareHeld = true;
                GameManager.Instance.CirclePressed();
            }
        }
        else if(Input.GetKeyUp(KeyCode.S))
        {
            squareHitMarker.color = white;
            isSquareHeld = false;
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            triangleHitMarker.color = green;
            if(isTriangleHeld == false)
            {
                isTriangleHeld = true;
                GameManager.Instance.CirclePressed();
            }
        }
        else if(Input.GetKeyUp(KeyCode.G))
        {
            triangleHitMarker.color = white;
            isTriangleHeld = false;
        }
        if(Input.GetKeyDown(KeyCode.H))
        {
            diamondHitMarker.color = yellow;
            if(isDiamondHeld == false)
            {
                isDiamondHeld = true;
                GameManager.Instance.CirclePressed();
            }
        }
        else if(Input.GetKeyUp(KeyCode.H))
        {
            diamondHitMarker.color = white;
            isDiamondHeld = false;
        }
    }
}
