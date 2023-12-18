using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region VARIABLES
    //SINGLETON
    public static PlayerManager Instance;

    //CONTROLS
    private readonly KeyCode[] circleKeys = new KeyCode[2] {KeyCode.A, KeyCode.H};
    private readonly KeyCode[] squareKeys = new KeyCode[2] {KeyCode.S, KeyCode.J};
    private readonly KeyCode[] triangleKeys = new KeyCode[2] {KeyCode.D, KeyCode.K};
    private readonly KeyCode[] diamondKeys = new KeyCode[2] {KeyCode.F, KeyCode.L};

    //HIT MARKER RENDERERS
    private SpriteRenderer[] circleMarkRenderers = new SpriteRenderer[2];
    private SpriteRenderer[] squareMarkRenderers = new SpriteRenderer[2];
    private SpriteRenderer[] triangleMarkRenderers = new SpriteRenderer[2];
    private SpriteRenderer[] diamondMarkRenderers = new SpriteRenderer[2];

    //CONTROL VARIABLES
    private bool[] isCircleHeld = new bool[2] {false, false};
    private bool[] isSquareHeld = new bool[2] {false, false};
    private bool[] isTriangleHeld = new bool[2] {false, false};
    private bool[] isDiamondHeld = new bool[2] {false, false};
    #endregion VARIABLES

    #region CUSTOM FUNCTIONS
    private void GetMarkerSpriteRenderers()
    {
        for(int i = 0; i < circleMarkRenderers.Length; i++)
        {
            circleMarkRenderers[i] = GameManager.Instance.circleHitMarks[i].GetComponent<SpriteRenderer>();
            squareMarkRenderers[i] = GameManager.Instance.squareHitMarks[i].GetComponent<SpriteRenderer>();
            triangleMarkRenderers[i] = GameManager.Instance.triangleHitMarks[i].GetComponent<SpriteRenderer>();
            diamondMarkRenderers[i] = GameManager.Instance.diamondHitMarks[i].GetComponent<SpriteRenderer>();
        }
    }

    private void InputDown(int index, Color color, SpriteRenderer spriteRenderer, ref bool isInputHeld)
    {
        spriteRenderer.color = color;
        if(isInputHeld == false)
        {
            isInputHeld = true;
            if(color == HelperClass.red)
                GameManager.Instance.InputPressed(index, HelperClass.InputType.Circle);
            else if(color == HelperClass.blue)
                GameManager.Instance.InputPressed(index, HelperClass.InputType.Square);
            else if(color == HelperClass.green)
                GameManager.Instance.InputPressed(index, HelperClass.InputType.Triangle);
            else if(color == HelperClass.yellow)
                GameManager.Instance.InputPressed(index, HelperClass.InputType.Diamond);
            else
                print("Function InputDown received an invalid color: " + color);
        } 
    }
    private void InputUp(Color color, SpriteRenderer spriteRenderer, out bool isInputHeld)
    {
        spriteRenderer.color = color;
        isInputHeld = false;
    }

    #endregion CUSTOM FUNCTIONS

    #region UNITY FUNCTIONS
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
        for(int i = 0; i < circleMarkRenderers.Length; i++)
        {
            circleMarkRenderers[i].color = HelperClass.darkRed;
            squareMarkRenderers[i].color = HelperClass.darkBlue;
            triangleMarkRenderers[i].color = HelperClass.darkGreen;
            diamondMarkRenderers[i].color = HelperClass.darkYellow;
        }
    }
    void Update()
    {
        if(GameManager.Instance.started)
        {
            //PLAYER CONTROLS
            for(int i = 0; i < circleKeys.Length; i++)
            {
                //CIRcLE INPUT
                if(Input.GetKeyDown(circleKeys[i]))
                {
                    InputDown(i, HelperClass.red, circleMarkRenderers[i], ref isCircleHeld[i]);
                }
                else if(Input.GetKeyUp(circleKeys[i]))
                {
                    InputUp(HelperClass.darkRed, circleMarkRenderers[i], out isCircleHeld[i]);
                }
                //SQUARE INPUT
                if(Input.GetKeyDown(squareKeys[i]))
                {
                    InputDown(i, HelperClass.blue, squareMarkRenderers[i], ref isSquareHeld[i]);
                }
                else if(Input.GetKeyUp(squareKeys[i]))
                {
                    InputUp(HelperClass.darkBlue, squareMarkRenderers[i], out isSquareHeld[i]);
                }
                //TRIANGLE INPUT
                if(Input.GetKeyDown(triangleKeys[i]))
                {
                    InputDown(i, HelperClass.green, triangleMarkRenderers[i], ref isTriangleHeld[i]);
                }
                else if(Input.GetKeyUp(triangleKeys[i]))
                {
                    InputUp(HelperClass.darkGreen, triangleMarkRenderers[i], out isSquareHeld[i]);
                }
                //DIAMOND INPUT
                if(Input.GetKeyDown(diamondKeys[i]))
                {
                    InputDown(i, HelperClass.yellow, diamondMarkRenderers[i], ref isDiamondHeld[i]);
                }
                else if(Input.GetKeyUp(diamondKeys[i]))
                {
                    InputUp(HelperClass.darkYellow, diamondMarkRenderers[i], out isDiamondHeld[i]);
                }
            }
        }
    }
    #endregion UNITY FUNCTIONS
}