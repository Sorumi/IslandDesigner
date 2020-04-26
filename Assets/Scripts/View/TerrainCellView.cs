using System;
using System.Collections.Generic;
using UnityEngine;

public struct TerrainViewFeature
{
    public bool WaterCenter;
    public bool WaterF;
    public bool WaterL;
    public bool WaterB;
    public bool WaterR;
    public bool WaterFL;
    public bool WaterBL;
    public bool WaterBR;
    public bool WaterFR;

    public TerrainViewFeature(bool waterCenter, bool waterF, bool waterL, bool waterB, bool waterR,
     bool waterFL, bool waterBL, bool waterBR, bool waterFR)
    {
        WaterCenter = waterCenter;
        WaterF = waterF;
        WaterL = waterL;
        WaterB = waterB;
        WaterR = waterR;
        WaterFL = waterFL;
        WaterBL = waterBL;
        WaterBR = waterBR;
        WaterFR = waterFR;
    }
}

public class TerrainCellView : MonoBehaviour
{

    public GameObject surface;
    public GameObject inner;

    public Action<Vector2Int> OnEnter;

    public Action<Vector2Int> OnRelease;

    Vector2Int position;

    TerrainViewFeature feature;

    MeshCollider mc;
    MeshFilter mfSurface;
    MeshFilter mfInner;

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
        mfSurface = surface.GetComponent<MeshFilter>();
        mfInner = inner.GetComponent<MeshFilter>();
        mc = GetComponent<MeshCollider>();
        mfSurface.sharedMesh = null;
        mfInner.sharedMesh = null;
        mc.sharedMesh = TerrainMeshGenerator.Instance.Plane;
    }

    public void SetFeature(TerrainViewFeature feature)
    {
        this.feature = feature;

        mfSurface.sharedMesh = TerrainMeshGenerator.Instance.GetTerrainSurfaceMesh(feature);
        mfInner.sharedMesh = TerrainMeshGenerator.Instance.GetTerrainInnerMesh(feature);
    }
}