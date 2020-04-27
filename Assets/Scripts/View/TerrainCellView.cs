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

public struct TerrainTallFeature
{
    public int Tall;
    public bool Front;
    public bool Left;
    public bool Back;
    public bool Right;

    public TerrainTallFeature(int tall, bool front, bool left, bool back, bool right)
    {
        Tall = tall;
        Front = front;
        Left = left;
        Back = back;
        Right = right;
    }
}

public class TerrainCellView : MonoBehaviour
{
    const float TALL_HEIGHT = 1.5f;

    public GameObject surface;
    public GameObject inner;
    public GameObject outter;

    public Action<Vector2Int> OnEnter;

    public Action<Vector2Int> OnRelease;

    Vector2Int position;

    TerrainViewFeature viewFeature;
    TerrainTallFeature tallFeature;

    MeshCollider mc;
    MeshFilter mfSurface;
    MeshFilter mfInner;
    MeshFilter mfOutter;

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
        mfOutter = outter.GetComponent<MeshFilter>();
        mc = GetComponent<MeshCollider>();
        mfSurface.sharedMesh = null;
        mfInner.sharedMesh = null;
        mfOutter.sharedMesh = null;
        mc.sharedMesh = TerrainMeshGenerator.Instance.Plane;
    }

    public void SetFeature(TerrainViewFeature viewFeature, TerrainTallFeature tallFeature)
    {
        this.viewFeature = viewFeature;
        this.tallFeature = tallFeature;

        Vector3 position = transform.position;
        position.y = (this.tallFeature.Tall - 1) * TALL_HEIGHT;
        transform.position = position;

        tallFeature.Tall = 0;

        mfSurface.sharedMesh = TerrainMeshGenerator.Instance.GetTerrainSurfaceMesh(viewFeature);
        mfInner.sharedMesh = TerrainMeshGenerator.Instance.GetTerrainInnerMesh(viewFeature);
        mfOutter.sharedMesh = TerrainMeshGenerator.Instance.GetTerrainOutterMesh(tallFeature);

    }
}