using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesignerController : MonoBehaviour
{
    public int IslandWidth = 16;
    public int IslandHeight = 16;

    public TextureGenerator textureGenerator;
    public MeshFilter meshFilter;

    public GameObject IslandPrefab;

    public HoverGrid grid;

    Island island;
    IslandView islandView;

    void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 100, 50), "Init Island"))
            initIsland();

        // if (GUI.Button(new Rect(50, 160, 100, 50), "Test Texture"))
        //     TestMesh();
    }

    // private void TestMesh()
    // {
    //     TerrainViewFeature feature = new TerrainViewFeature(true, true, false, false, true, false, false, false, false);
    //     Mesh mesh = TerrainMeshGenerator.Instance.GetTerrainMesh(feature);
    //     meshFilter.sharedMesh = mesh;
    // }

    private void initIsland()
    {
        island = new Island(IslandWidth, IslandHeight);

        GameObject islandGameObject = Instantiate(IslandPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        islandView = islandGameObject.GetComponent<IslandView>();
        islandView.Init(IslandWidth, IslandHeight);
        islandView.OnEnterTerrainCell = enterTerrainCellHandler;
        islandView.OnReleaseTerrainCell = releaseTerrainCellHandler;

        for (int x = 0; x < IslandWidth; x++)
        {
            for (int y = 0; y < IslandHeight; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                TerrainViewFeature terrainViewFeature = terrainViewFeatureOfPosition(position);


                islandView.SetTerrainFeature(position, terrainViewFeature);
            }
        }

        grid.Init();
    }

    private void enterTerrainCellHandler(Vector2Int position)
    {
        Debug.Log(position);

        Vector2 posWS = new Vector2(position.x - IslandWidth / 2, - position.y + IslandHeight / 2);
        grid.SetPosition(posWS);
    }

    private void releaseTerrainCellHandler(Vector2Int position)
    {
        TerrainType terrainType = island.TerrainTypeOfCell(position);

        if (terrainType == TerrainType.Water)
            terrainType = TerrainType.Earth;
        else if (terrainType == TerrainType.Earth)
            terrainType = TerrainType.Water;

        bool result = island.ChangeTerrainType(position, terrainType);
        if (result)
        {
            for (int x = position.x - 1; x <= position.x + 1; x++)
                for (int y = position.y - 1; y <= position.y + 1; y++)
                {
                    Vector2Int newPosition = new Vector2Int(x, y);
                    if (island.ValidPosition(newPosition))
                    {
                        TerrainViewFeature terrainViewFeature = terrainViewFeatureOfPosition(newPosition);
                        islandView.SetTerrainFeature(newPosition, terrainViewFeature);
                    }
                }
        }
    }


    private TerrainViewFeature terrainViewFeatureOfPosition(Vector2Int position)
    {
        int x = position.x;
        int y = position.y;

        bool waterCenter = island.TerrainTypeOfCell(position) == TerrainType.Water;
        TerrainViewFeature terrainViewFeature = new TerrainViewFeature(false, false, false, false, false, false, false, false, false);

        if (!waterCenter)
            return terrainViewFeature;

        bool waterTop = island.TerrainTypeOfCell(new Vector2Int(x, y - 1)) == TerrainType.Water;
        bool waterLeft = island.TerrainTypeOfCell(new Vector2Int(x - 1, y)) == TerrainType.Water;
        bool waterDown = island.TerrainTypeOfCell(new Vector2Int(x, y + 1)) == TerrainType.Water;
        bool waterRight = island.TerrainTypeOfCell(new Vector2Int(x + 1, y)) == TerrainType.Water;
        bool waterTopLeft = island.TerrainTypeOfCell(new Vector2Int(x - 1, y - 1)) == TerrainType.Water;
        bool waterDownLeft = island.TerrainTypeOfCell(new Vector2Int(x - 1, y + 1)) == TerrainType.Water;
        bool waterDownRight = island.TerrainTypeOfCell(new Vector2Int(x + 1, y + 1)) == TerrainType.Water;
        bool waterTopRight = island.TerrainTypeOfCell(new Vector2Int(x + 1, y - 1)) == TerrainType.Water;

        terrainViewFeature.WaterCenter = waterCenter;
        terrainViewFeature.WaterT = waterTop;
        terrainViewFeature.WaterL = waterLeft;
        terrainViewFeature.WaterD = waterDown;
        terrainViewFeature.WaterR = waterRight;
        terrainViewFeature.WaterTL = waterTop && waterLeft && waterTopLeft;
        terrainViewFeature.WaterDL = waterDown && waterLeft && waterDownLeft;
        terrainViewFeature.WaterDR = waterDown && waterRight && waterDownRight;
        terrainViewFeature.WaterTR = waterTop && waterRight && waterTopRight;

        return terrainViewFeature;
    }
}