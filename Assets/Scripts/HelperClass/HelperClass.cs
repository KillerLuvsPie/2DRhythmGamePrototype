using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperClass
{
    #region ENUMS
    public enum InputType {Circle, Square, Triangle, Diamond}

    #endregion ENUMS

    #region COLORS
    //HIT MARKER COLORS
    public static Color red = new Color(1, 0, 0, 1);
    public static Color darkRed = new Color(0.25f, 0, 0, 1);
    public static Color blue = new Color(0, 0, 1, 1);
    public static Color darkBlue = new Color(0, 0, 0.25f, 1);
    public static Color green = new Color(0, 1, 0, 1);
    public static Color darkGreen = new Color(0, 0.25f, 0, 1);
    public static Color yellow = new Color(1, 1, 0, 1);
    public static Color darkYellow = new Color(0.25f, 0.25f, 0, 1);
    public static Color[] playerColors = new Color[2] {blue, green};
    public static Color[] playerDarkColors = new Color[2] {darkBlue, darkGreen};
    public static Color playerBGEffectAlphaSlide = new Color(0,0,0,0.05f);
    public static Color[] playerBGEffectColorSlide = new Color[2] {new Color(0,0,0.1f,0), new Color(0,0.1f,0,0)};
    //OUTLINE COLORS
    public static Color lightBlue = new Color(0, 0.75f, 1, 1);
    public static Color orange = new Color(1, 0.5f, 0, 1);
    public static Color lightGreen = new Color(0.5f, 1, 0, 1);
    public static Color[] outlineColors = new Color[2] {lightBlue, lightGreen};
    public static Color white = new Color(1, 1, 1, 1);
    //BACKGROUND EFFECT ALPHA
    public static Color bgAlphaChange = new Color(0, 0, 0, 0.1f);

    #endregion COLORS
}
