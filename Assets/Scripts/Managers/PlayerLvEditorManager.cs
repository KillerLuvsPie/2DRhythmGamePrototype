using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLvEditorManager : MonoBehaviour
{
    #region VARIABLES
    //SINGLETON
    public static PlayerLvEditorManager Instance;
    //HIT MARKERS
    public SpriteRenderer circleHitMarker;
    public SpriteRenderer squareHitMarker;
    public SpriteRenderer triangleHitMarker;
    public SpriteRenderer diamondHitMarker;
    //CONTROL VARIABLES
    private bool isCircleHeld = false;
    private bool isSquareHeld = false;
    private bool isTriangleHeld = false;
    private bool isDiamondHeld = false;

    #endregion VARIABLES

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
        circleHitMarker.color = HelperClass.darkRed;
        squareHitMarker.color = HelperClass.darkBlue;
        triangleHitMarker.color = HelperClass.darkGreen;
        diamondHitMarker.color = HelperClass.darkYellow;
    }
    void Update()
    {
        if(LevelEditorManager.Instance.isPlaying)
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                circleHitMarker.color = HelperClass.red;
                if(isCircleHeld == false)
                {
                    isCircleHeld = true;
                    LevelEditorManager.Instance.CreateCircle();
                }
            }
            else if(Input.GetKeyUp(KeyCode.A))
            {
                circleHitMarker.color = HelperClass.darkRed;
                isCircleHeld = false;
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                squareHitMarker.color = HelperClass.blue;
                if(isSquareHeld == false)
                {
                    isSquareHeld = true;
                    LevelEditorManager.Instance.CreateSquare();
                }
            }
            else if(Input.GetKeyUp(KeyCode.S))
            {
                squareHitMarker.color = HelperClass.darkBlue;
                isSquareHeld = false;
            }
            if(Input.GetKeyDown(KeyCode.G))
            {
                triangleHitMarker.color = HelperClass.green;
                if(isTriangleHeld == false)
                {
                    isTriangleHeld = true;
                    LevelEditorManager.Instance.CreateTriangle();
                }
            }
            else if(Input.GetKeyUp(KeyCode.G))
            {
                triangleHitMarker.color = HelperClass.darkGreen;
                isTriangleHeld = false;
            }
            if(Input.GetKeyDown(KeyCode.H))
            {
                diamondHitMarker.color = HelperClass.yellow;
                if(isDiamondHeld == false)
                {
                    isDiamondHeld = true;
                    LevelEditorManager.Instance.CreateDiamond();
                }
            }
            else if(Input.GetKeyUp(KeyCode.H))
            {
                diamondHitMarker.color = HelperClass.darkYellow;
                isDiamondHeld = false;
            }
        }
    }

    #endregion UNITY FUNCTIONS
}
