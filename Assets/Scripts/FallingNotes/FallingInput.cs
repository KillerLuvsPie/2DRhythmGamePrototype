using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "NewFallingInput", menuName = "Scriptable Object/Falling Input")]
public class FallingInput : ScriptableObject
{
    public string inputName;
    public Sprite sprite;
    public Vector3 scaleAdjust = new Vector3(1,1,1);
}