using System;
using System.Collections.Generic;
using UnityEngine;

public struct TerrainViewFeature
{
    public bool WaterCenter;
    public bool WaterT;
    public bool WaterL;
    public bool WaterD;
    public bool WaterR;
    public bool WaterTL;
    public bool WaterDL;
    public bool WaterDR;
    public bool WaterTR;

    public TerrainViewFeature(bool waterCenter, bool waterT, bool waterL, bool waterD, bool waterR,
     bool waterTL, bool waterDL, bool waterDR, bool waterTR)
    {
        WaterCenter = waterCenter;
        WaterT = waterT;
        WaterL = waterL;
        WaterD = waterD;
        WaterR = waterR;
        WaterTL = waterTL;
        WaterDL = waterDL;
        WaterDR = waterDR;
        WaterTR = waterTR;
    }
}

public class TerrainCellView : MonoBehaviour
{

    public Action<Vector2Int> OnEnter;

    public Action<Vector2Int> OnRelease;

    Vector2Int position;

    TerrainViewFeature feature;

    MeshRenderer meshRenderer;

    MaterialPropertyBlock block;

    void OnMouseEnter()
    {
        if (OnEnter != null)
            OnEnter(position);
    }

    void OnMouseUp()
    {
        if (OnRelease != null)
            OnRelease(position);
    }

    public void Init(int x, int y)
    {

        this.position = new Vector2Int(x, y);
        meshRenderer = GetComponent<MeshRenderer>();
        block = new MaterialPropertyBlock();

        meshRenderer.SetPropertyBlock(block);
    }

    public void SetFeature(TerrainViewFeature feature)
    {
        this.feature = feature;

        Texture2D texture = TextureGenerator.Instance.GetTerrainTexture(feature);
        block.SetTexture("_MainTex", texture);

        meshRenderer.SetPropertyBlock(block);
    }
}