using System;
using UnityEngine;


public enum TerrainTall
{
    None,
    Floor,
    Second,
    Third
}
public enum TerrainType
{
    Earth,
    Water
}

public class TerrainCell
{

    public Vector2Int Position
    {
        get
        {
            return cellPosition;
        }
    }

    public TerrainTall Tall
    {
        get
        {
            return tall;
        }
        set
        {
            tall = value;
        }
    }

    public TerrainType Type
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
        }
    }

    Vector2Int cellPosition;
    TerrainTall tall;
    TerrainType type;


    public TerrainCell(int x, int y)
    {
        cellPosition = new Vector2Int(x, y);
        tall = TerrainTall.Floor;
        type = TerrainType.Earth;
    }

}