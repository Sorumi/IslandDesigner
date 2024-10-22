using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesignerController : MonoBehaviour
{
    public enum ToolState
    {
        Water,
        Tall
    }

    public int IslandWidth = 16;
    public int IslandHeight = 16;

    public TextureGenerator textureGenerator;
    public MeshFilter meshFilter;

    public GameObject IslandPrefab;

    public HoverGrid grid;

    public ToolPanel toolPanel;

    ToolState state;

    Island island;
    IslandView islandView;

    void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 100, 50), "Init Island"))
            initIsland();

        // if (GUI.Button(new Rect(50, 160, 100, 50), "Test Mesh"))
        // TestMesh();
    }

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
                TerrainViewFeature terrainViewFeature;
                TerrainTallFeature terrainTallFeature;
                terrainViewFeatureOfPosition(position, out terrainViewFeature, out terrainTallFeature);
                islandView.SetTerrainFeature(position, terrainViewFeature, terrainTallFeature);
            }
        }

        // UI
        grid.Init();

        toolPanel.Init();
        toolPanel.OnWaterClick = () => state = ToolState.Water;
        toolPanel.OnTallClick = () => state = ToolState.Tall;
    }

    private void enterTerrainCellHandler(Vector2Int position)
    {
        // Debug.Log(position);

        Vector2 posWS = new Vector2(position.x - IslandWidth / 2, -position.y + IslandHeight / 2);
        grid.SetPosition(posWS);
    }

    private void releaseTerrainCellHandler(Vector2Int position)
    {
        if (state == ToolState.Water)
            createTerrainWater(position);
        else if (state == ToolState.Tall)
            createTerrainTall(position);
    }

    private void createTerrainTall(Vector2Int position)
    {
        TerrainTall terrainTall = island.TerrainTallOfCell(position);

        if (terrainTall == TerrainTall.Floor)
            terrainTall = TerrainTall.Second;
        else if (terrainTall == TerrainTall.Second)
            terrainTall = TerrainTall.Floor;

        bool result = island.ChangeTerrainTall(position, terrainTall);

        if (result)
        {
            TerrainViewFeature terrainViewFeature;
            TerrainTallFeature terrainTallFeature;

            terrainViewFeatureOfPosition(position, out terrainViewFeature, out terrainTallFeature);
            islandView.SetTerrainFeature(position, terrainViewFeature, terrainTallFeature);
        }
    }
    private void createTerrainWater(Vector2Int position)
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
                        TerrainViewFeature terrainViewFeature;
                        TerrainTallFeature terrainTallFeature;

                        terrainViewFeatureOfPosition(newPosition, out terrainViewFeature, out terrainTallFeature);
                        islandView.SetTerrainFeature(newPosition, terrainViewFeature, terrainTallFeature);
                    }
                }
        }
    }

    private void terrainViewFeatureOfPosition(Vector2Int position, out TerrainViewFeature terrainViewFeature, out TerrainTallFeature terrainTallFeature)
    {
        int x = position.x;
        int y = position.y;
        terrainViewFeature = new TerrainViewFeature(false, false, false, false, false, false, false, false, false);
        terrainTallFeature = new TerrainTallFeature(0, false, false, false, false);

        bool waterCenter = island.TerrainTypeOfCell(position) == TerrainType.Water;
        if (waterCenter)
        {
            bool waterFront = island.TerrainTypeOfCell(new Vector2Int(x, y - 1)) == TerrainType.Water;
            bool waterLeft = island.TerrainTypeOfCell(new Vector2Int(x - 1, y)) == TerrainType.Water;
            bool waterBack = island.TerrainTypeOfCell(new Vector2Int(x, y + 1)) == TerrainType.Water;
            bool waterRight = island.TerrainTypeOfCell(new Vector2Int(x + 1, y)) == TerrainType.Water;
            bool waterFrontLeft = island.TerrainTypeOfCell(new Vector2Int(x - 1, y - 1)) == TerrainType.Water;
            bool waterBackLeft = island.TerrainTypeOfCell(new Vector2Int(x - 1, y + 1)) == TerrainType.Water;
            bool waterBackRight = island.TerrainTypeOfCell(new Vector2Int(x + 1, y + 1)) == TerrainType.Water;
            bool waterFrontRight = island.TerrainTypeOfCell(new Vector2Int(x + 1, y - 1)) == TerrainType.Water;

            terrainViewFeature.WaterCenter = waterCenter;
            terrainViewFeature.WaterF = waterFront;
            terrainViewFeature.WaterL = waterLeft;
            terrainViewFeature.WaterB = waterBack;
            terrainViewFeature.WaterR = waterRight;
            terrainViewFeature.WaterFL = waterFront && waterLeft && waterFrontLeft;
            terrainViewFeature.WaterBL = waterBack && waterLeft && waterBackLeft;
            terrainViewFeature.WaterBR = waterBack && waterRight && waterBackRight;
            terrainViewFeature.WaterFR = waterFront && waterRight && waterFrontRight;
        }

        TerrainTall tall = island.TerrainTallOfCell(position);
        bool tallFront = island.TerrainTallOfCell(new Vector2Int(x, y - 1)) < tall;
        bool tallLeft = island.TerrainTallOfCell(new Vector2Int(x - 1, y)) < tall;
        bool tallBack = island.TerrainTallOfCell(new Vector2Int(x, y + 1)) < tall;
        bool tallRight = island.TerrainTallOfCell(new Vector2Int(x + 1, y)) < tall;
        terrainTallFeature.Front = tallFront;
        terrainTallFeature.Left = tallLeft;
        terrainTallFeature.Back = tallBack;
        terrainTallFeature.Right = tallRight;

        terrainTallFeature.Tall = (int)tall;

        return;
    }
}