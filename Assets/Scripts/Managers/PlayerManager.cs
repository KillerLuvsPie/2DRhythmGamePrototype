using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //SINGLETON
    public static PlayerManager Instance;

    //CONTROLS
    //PLAYER 1
    private KeyCode circleKeyP1 = KeyCode.A;
    private KeyCode squareKeyP1 = KeyCode.S;
    private KeyCode triangleKeyP1 = KeyCode.D;
    private KeyCode diamondKeyP1 = KeyCode.F;
    //PLAYER 2
    private KeyCode circleKeyP2 = KeyCode.H;
    private KeyCode squareKeyP2 = KeyCode.J;
    private KeyCode triangleKeyP2 = KeyCode.K;
    private KeyCode diamondKeyP2 = KeyCode.L;
    //HIT MARKER RENDERERS
    //PLAYER 1
    private SpriteRenderer circleRenderer_P1;
    private SpriteRenderer squareRenderer_P1;
    private SpriteRenderer triangleRenderer_P1;
    private SpriteRenderer diamondRenderer_P1;
    //PLAYER 2
    private SpriteRenderer circleRenderer_P2;
    private SpriteRenderer squareRenderer_P2;
    private SpriteRenderer triangleRenderer_P2;
    private SpriteRenderer diamondRenderer_P2;

    //HIT MARKER COLORS
    private Color red = new Color(1,0,0,1);
    private Color darkRed = new Color(0.25f,0,0,1);
    private Color blue = new Color(0,0,1,1);
    private Color darkBlue = new Color(0,0,0.25f,1);
    private Color green = new Color(0,1,0,1);
    private Color darkGreen = new Color(0,0.25f,0,1);
    private Color yellow = new Color(1,1,0,1);
    private Color darkYellow = new Color(0.25f,0.25f,0,1);
    private Color white = new Color(1,1,1,1);

    //CONTROL VARIABLES
    //PLAYER 1
    private bool isCircleHeldP1 = false;
    private bool isSquareHeldP1 = false;
    private bool isTriangleHeldP1 = false;
    private bool isDiamondHeldP1 = false;
    //PLAYER 2
    private bool isCircleHeldP2 = false;
    private bool isSquareHeldP2 = false;
    private bool isTriangleHeldP2 = false;
    private bool isDiamondHeldP2 = false;

    //FUNCTIONS
    private void GetMarkerSpriteRenderers()
    {
        circleRenderer_P1 = GameManager.Instance.circleHitMark_P1.GetComponent<SpriteRenderer>();
        squareRenderer_P1 = GameManager.Instance.squareHitMark_P1.GetComponent<SpriteRenderer>();
        triangleRenderer_P1 = GameManager.Instance.triangleHitMark_P1.GetComponent<SpriteRenderer>();
        diamondRenderer_P1 = GameManager.Instance.diamondHitMark_P1.GetComponent<SpriteRenderer>();
        circleRenderer_P2 = GameManager.Instance.circleHitMark_P2.GetComponent<SpriteRenderer>();
        squareRenderer_P2 = GameManager.Instance.squareHitMark_P2.GetComponent<SpriteRenderer>();
        triangleRenderer_P2 = GameManager.Instance.triangleHitMark_P2.GetComponent<SpriteRenderer>();
        diamondRenderer_P2 = GameManager.Instance.diamondHitMark_P2.GetComponent<SpriteRenderer>();
    }

    private void CircleDown(bool p1)
    {
        if(p1)
        {
            circleRenderer_P1.color = red;
            if(isCircleHeldP1 == false)
            {
                isCircleHeldP1 = true;
                GameManager.Instance.CirclePressed();
            }
        }
        else
        {
            circleRenderer_P2.color = red;
            if(isCircleHeldP2 == false)
            {
                isCircleHeldP2 = true;
                GameManager.Instance.CirclePressed();
            }
        }
        
    }
    private void CircleUp(bool p1)
    {
        if(p1)
        {
            circleRenderer_P1.color = darkRed;
            isCircleHeldP1 = false;
        }
        else
        {
            circleRenderer_P2.color = darkRed;
            isCircleHeldP2 = false;
        }
    }

    private void SquareDown(bool p1)
    {
        if(p1)
        {
            squareRenderer_P1.color = blue;
            if(isSquareHeldP1 == false)
            {
                isSquareHeldP1 = true;
                GameManager.Instance.SquarePressed();
            }
        }
        else
        {
            squareRenderer_P2.color = blue;
            if(isSquareHeldP2 == false)
            {
                isSquareHeldP2 = true;
                GameManager.Instance.SquarePressed();
            }
        }
        
    }
    private void SquareUp(bool p1)
    {
        if(p1)
        {
            squareRenderer_P1.color = darkBlue;
            isSquareHeldP1 = false;
        }
        else
        {
            squareRenderer_P2.color = darkBlue;
            isSquareHeldP2 = false;
        }
    }

    private void TriangleDown(bool p1)
    {
        if(p1)
        {
            triangleRenderer_P1.color = green;
            if(isTriangleHeldP1 == false)
            {
                isTriangleHeldP1 = true;
                GameManager.Instance.TrianglePressed();
            }
        }
        else
        {
            triangleRenderer_P2.color = green;
            if(isTriangleHeldP2 == false)
            {
                isTriangleHeldP2 = true;
                GameManager.Instance.TrianglePressed();
            }
        }
    }
    private void TriangleUp(bool p1)
    {
        if(p1)
        {
            triangleRenderer_P1.color = darkGreen;
            isTriangleHeldP1 = false;
        }
        else
        {
            triangleRenderer_P2.color = darkGreen;
            isTriangleHeldP2 = false;
        }
        
    }

    private void DiamondDown(bool p1)
    {
        if(p1)
        {
            diamondRenderer_P1.color = yellow;
            if(isDiamondHeldP1 == false)
            {
                isDiamondHeldP1 = true;
                GameManager.Instance.DiamondPressed();
            }
        }
        else
        {
            diamondRenderer_P2.color = yellow;
            if(isDiamondHeldP2 == false)
            {
                isDiamondHeldP2 = true;
                GameManager.Instance.DiamondPressed();
            }
        }
        
    }
    private void DiamondUp(bool p1)
    {
        if(p1)
        {
            diamondRenderer_P1.color = darkYellow;
            isDiamondHeldP1 = false;
        }
        else
        {
            diamondRenderer_P2.color = darkYellow;
            isDiamondHeldP2 = false;
        }
    }

    //UNITY FUNCTIONS
    void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    void Start()
    {
        GetMarkerSpriteRenderers();
        circleRenderer_P1.color = darkRed;
        squareRenderer_P1.color = darkBlue;
        triangleRenderer_P1.color = darkGreen;
        diamondRenderer_P1.color = darkYellow;
        circleRenderer_P2.color = darkRed;
        squareRenderer_P2.color = darkBlue;
        triangleRenderer_P2.color = darkGreen;
        diamondRenderer_P2.color = darkYellow;
    }
    void Update()
    {
        if(GameManager.Instance.started)
        {
            //PLAYER 1 CONTROLS
            //CIRcLE INPUT
            if(Input.GetKeyDown(circleKeyP1))
            {
                CircleDown(true);
            }
            else if(Input.GetKeyUp(circleKeyP1))
            {
                CircleUp(true);
            }
            //SQUARE INPUT
            if(Input.GetKeyDown(squareKeyP1))
            {
                SquareDown(true);
            }
            else if(Input.GetKeyUp(squareKeyP1))
            {
                SquareUp(true);
            }
            //TRIANGLE INPUT
            if(Input.GetKeyDown(triangleKeyP1))
            {
                TriangleDown(true);
            }
            else if(Input.GetKeyUp(triangleKeyP1))
            {
                TriangleUp(true);
            }
            //DIAMOND INPUT
            if(Input.GetKeyDown(diamondKeyP1))
            {
                DiamondDown(true);
            }
            else if(Input.GetKeyUp(diamondKeyP1))
            {
                DiamondUp(true);
            }

            //PLAYER 2 CONTROLS
            if(Input.GetKeyDown(circleKeyP2))
            {
                CircleDown(false);
            }
            else if(Input.GetKeyUp(circleKeyP2))
            {
                CircleUp(false);
            }
            //SQUARE INPUT
            if(Input.GetKeyDown(squareKeyP2))
            {
                SquareDown(false);
            }
            else if(Input.GetKeyUp(squareKeyP2))
            {
                SquareUp(false);
            }
            //TRIANGLE INPUT
            if(Input.GetKeyDown(triangleKeyP2))
            {
                TriangleDown(false);
            }
            else if(Input.GetKeyUp(triangleKeyP2))
            {
                TriangleUp(false);
            }
            //DIAMOND INPUT
            if(Input.GetKeyDown(diamondKeyP2))
            {
                DiamondDown(false);
            }
            else if(Input.GetKeyUp(diamondKeyP2))
            {
                DiamondUp(false);
            }
        }
    }
}
