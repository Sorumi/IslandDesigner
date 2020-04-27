using System;
using System.Collections.Generic;
using UnityEngine;

public class IslandView : MonoBehaviour
{
    public TerrainCellView terrainView;

    int width;
    int height;
    int countOfCells;
    private List<TerrainCellView> terrainCellViews;

    public Action<Vector2Int> OnEnterTerrainCell;

    public Action<Vector2Int> OnReleaseTerrainCell;

    public void Init(int width, int height)
    {
        this.width = width;
        this.height = height;

        countOfCells = width * height;

        int left = width / 2;
        int top = height / 2;

        terrainCellViews = new List<TerrainCellView>(countOfCells);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TerrainCellView terrainCellView = Instantiate(terrainView, new Vector3(0, 0, 0), Quaternion.identity);
                float posWSX = x - left + 0.5f;
                float posWSZ = -(y - top + 1) + 0.5f;
                terrainCellView.transform.position = new Vector3(posWSX, 0, posWSZ);

                terrainCellView.Init(x, y);
                terrainCellView.OnEnter = enterTerrainCellHandler;
                terrainCellView.OnRelease = releaseTerrainCellHandler;

                terrainCellViews.Add(terrainCellView);
            }
        }
    }

    private void enterTerrainCellHandler(Vector2Int position)
    {
        if (OnEnterTerrainCell != null)
            OnEnterTerrainCell(position);
    }

    private void releaseTerrainCellHandler(Vector2Int position)
    {
        if (OnReleaseTerrainCell != null)
            OnReleaseTerrainCell(position);
    }

    public void SetTerrainFeature(Vector2Int position, TerrainViewFeature viewFeature, TerrainTallFeature tallFeature)
    {
        TerrainCellView terrainCellView = getCellView(position);
        terrainCellView.SetFeature(viewFeature, tallFeature);
    }


    private TerrainCellView getCellView(Vector2Int position)
    {
        int index = position.x * width + position.y;
        if (index < 0 || index > countOfCells)
            return null;
        return terrainCellViews[index];
    }
}