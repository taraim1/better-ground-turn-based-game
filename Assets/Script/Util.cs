using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PRS
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;


    public PRS(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
    }
}

public class Util
{
    static public float drag_time_standard = 0.13f;

    static public T StringToEnum<T>(string e)
    {
        return (T)Enum.Parse(typeof(T), e);
    } 
}

[System.Serializable]
public struct coordinate
{
    public static bool operator != (coordinate c1, coordinate c2) 
    {
        if (c1.x != c2.x || c1.y != c2.y) { return true; }
        else { return false; }
    }
    public static bool operator == (coordinate c1, coordinate c2) 
    {
        if (c1.x == c2.x && c1.y == c2.y) { return true; }
        else { return false; }
    }

    public static coordinate operator + (coordinate c1, coordinate c2)
    {
        return new coordinate(c1.x + c2.x, c1.y + c2.y);
    }

    public coordinate(int x = 0, int y = 0)
    {
        this.x = x;
        this.y = y;
    }
    public int x, y;
}

