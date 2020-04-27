using System;
using System.Collections.Generic;
using UnityEngine;

public class Island
{
    int width;
    int height;
    int countOfCells;

    List<TerrainCell> cells;

    public int Width
    {
        get
        {
            return width;
        }
    }

    public int Height
    {
        get
        {
            return height;
        }
    }

    public Island(int width, int height)
    {
        this.width = width;
        this.height = height;

        countOfCells = width * height;

        cells = new List<TerrainCell>(countOfCells);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TerrainCell cell = new TerrainCell(x, y);
                cells.Add(cell);
            }
        }
    }

    public TerrainType TerrainTypeOfCell(Vector2Int position)
    {
        TerrainCell cell = getCell(position);
        if (cell != null)
            return cell.Type;
        return TerrainType.Earth;
    }

    public TerrainTall TerrainTallOfCell(Vector2Int position)
    {
        TerrainCell cell = getCell(position);
        if (cell != null)
            return cell.Tall;
        return TerrainTall.None;
    }

    public bool ChangeTerrainType(Vector2Int position, TerrainType type)
    {
        TerrainCell cell = getCell(position);
        cell.Type = type;

        // TODO
        return true;
    }

    public bool ChangeTerrainTall(Vector2Int position, TerrainTall tall)
    {
        TerrainCell cell = getCell(position);
        cell.Tall = tall;

        return true;
    }

    public int PositionIndex(Vector2Int position)
    {
        int index = position.x * width + position.y;
        if (index < 0 || index >= countOfCells)
            return -1;
        return index;
    }

    public bool ValidPosition(Vector2Int position)
    {
        int index = position.x * width + position.y;
        return index >= 0 && index < countOfCells;
    }

    private TerrainCell getCell(Vector2Int position)
    {
        int index = PositionIndex(position);
        if (index == -1)
            return null;
        return cells[index];
    }

}