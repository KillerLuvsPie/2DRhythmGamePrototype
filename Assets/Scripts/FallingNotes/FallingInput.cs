using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "NewFallingInput", menuName = "Scriptable Object/Falling Input")]
public class FallingInput : ScriptableObject
{
    public Sprite sprite;
    public Color color;
    public Vector3 scaleAdjust = new Vector3(1,1,1);
    public float inputTime;
}