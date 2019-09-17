using System;
using UnityEngine;


public static class Utility
{
    public static Color HexToColor(string hex)
    {
        Color result = new Color(0f, 0f, 0f, 1f);
        hex = hex.ToLower();
        result.r = (float)(Utility.HexToInt(hex[0]) * 16 + Utility.HexToInt(hex[1])) / 255f;
        result.g = (float)(Utility.HexToInt(hex[2]) * 16 + Utility.HexToInt(hex[3])) / 255f;
        result.b = (float)(Utility.HexToInt(hex[4]) * 16 + Utility.HexToInt(hex[5])) / 255f;
        return result;
    }
    
    private static int HexToInt(char hex)
    {
        switch (hex)
        {
            case 'a':
                return 10;
            case 'b':
                return 11;
            case 'c':
                return 12;
            case 'd':
                return 13;
            case 'e':
                return 14;
            case 'f':
                return 15;
            default:
                return int.Parse(hex.ToString());
        }
    }
    
    public static float GetNote(int i)
    {
        switch (i)
        {
            case 1:
                return 1.125f;
            case 2:
                return 1.25f;
            case 3:
                return 1.333f;
            case 4:
                return 1.5f;
            case 5:
                return 1.666f;
            case 6:
                return 1.875f;
            default:
                return 1f;
        }
    }
}
