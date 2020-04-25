using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMeshGenerator : MonoBehaviour
{
    public static TerrainMeshGenerator Instance
    {
        get
        {
            if (instance == null)
            {
                if (!Application.isPlaying) return null;
                var objects = FindObjectsOfType<TerrainMeshGenerator>();
                if (objects.Length == 0)
                {
                    var go = new GameObject("TerrainMeshGenerator Instance");
                    instance = go.AddComponent<TerrainMeshGenerator>();
                }
                else if (objects.Length >= 1)
                {
                    instance = objects[0];
                }
            }
            return instance;
        }
    }

    private static TerrainMeshGenerator instance;
    public float edge = 0.16f;
    public Mesh Plane
    {
        get
        {
            return colliderMesh;
        }
    }
    private static Dictionary<TerrainViewFeature, Mesh> meshDictionary = new Dictionary<TerrainViewFeature, Mesh>(32);

    private Vector3[] vertices;
    private Vector2[] uvs;
    private int[][] trianglesCell;

    private Mesh colliderMesh;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
            return;
        }

        gameObject.hideFlags = HideFlags.HideInHierarchy;
        DontDestroyOnLoad(gameObject);

        init();
    }

    private void init()
    {
        float p0 = 0.0f;
        float p1 = edge;
        float p2 = 1.0f - edge;
        float p3 = 1.0f;
        float[] points = new float[] { p0, p1, p2, p3 };

        vertices = new Vector3[16];
        uvs = new Vector2[16];

        Vector3 offset = new Vector3(0.5f, 0.0f, 0.5f);

        for (int y = 0; y < 4; y++)
            for (int x = 0; x < 4; x++)
            {
                vertices[y * 4 + x] = new Vector3(points[x], 0, points[y]) - offset;
                uvs[y * 4 + x] = new Vector2(points[x], points[y]);
            }

        trianglesCell = new int[9][];
        trianglesCell[0] = new int[6] { 0, 4, 1, 1, 4, 5 };
        trianglesCell[1] = new int[6] { 1, 5, 2, 2, 5, 6 };
        trianglesCell[2] = new int[6] { 2, 6, 3, 3, 6, 7 };
        trianglesCell[3] = new int[6] { 4, 8, 5, 5, 8, 9 };
        trianglesCell[4] = new int[6] { 5, 9, 6, 6, 9, 10 };
        trianglesCell[5] = new int[6] { 6, 10, 7, 7, 10, 11 };
        trianglesCell[6] = new int[6] { 8, 12, 9, 9, 12, 13 };
        trianglesCell[7] = new int[6] { 9, 13, 10, 10, 13, 14 };
        trianglesCell[8] = new int[6] { 10, 14, 11, 11, 14, 15 };

        colliderMesh = new Mesh();
        colliderMesh.vertices = new Vector3[]{
            new Vector3(0, 0, 0) - offset,
            new Vector3(1, 0, 0) - offset,
            new Vector3(1, 0, 1) - offset,
            new Vector3(0, 0, 1) - offset,
        };
        colliderMesh.triangles = new int[] { 0, 3, 1, 1, 3, 2 };
    }

    public Mesh GetTerrainMesh(TerrainViewFeature feature)
    {
        Mesh mesh;
        if (meshDictionary.TryGetValue(feature, out mesh))
            return mesh;
        mesh = generateMesh(feature);
        meshDictionary.Add(feature, mesh);
        return mesh;
    }

    private Mesh generateMesh(TerrainViewFeature feature)
    {
        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.uv = uvs;

        List<int> triangleList = new List<int>();

        if (!feature.WaterCenter)
            triangleList.AddRange(trianglesCell[4]);
        if (!feature.WaterT)
            triangleList.AddRange(trianglesCell[7]);
        if (!feature.WaterL)
            triangleList.AddRange(trianglesCell[3]);
        if (!feature.WaterD)
            triangleList.AddRange(trianglesCell[1]);
        if (!feature.WaterR)
            triangleList.AddRange(trianglesCell[5]);
        if (!feature.WaterTL)
            triangleList.AddRange(trianglesCell[6]);
        if (!feature.WaterDL)
            triangleList.AddRange(trianglesCell[0]);
        if (!feature.WaterDR)
            triangleList.AddRange(trianglesCell[2]);
        if (!feature.WaterTR)
            triangleList.AddRange(trianglesCell[8]);

        mesh.triangles = triangleList.ToArray();

        return mesh;
    }

}
