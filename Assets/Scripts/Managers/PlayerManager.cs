using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            circleHitMarker.color = red;
        }
        else if(Input.GetKeyUp(KeyCode.A))
        {
            circleHitMarker.color = white;
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            squareHitMarker.color = blue;
        }
        else if(Input.GetKeyUp(KeyCode.S))
        {
            squareHitMarker.color = white;
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            triangleHitMarker.color = green;
        }
        else if(Input.GetKeyUp(KeyCode.G))
        {
            triangleHitMarker.color = white;
        }
        if(Input.GetKeyDown(KeyCode.H))
        {
            diamondHitMarker.color = yellow;
        }
        else if(Input.GetKeyUp(KeyCode.H))
        {
            diamondHitMarker.color = white;
        }
    }
}
