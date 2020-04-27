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
    public float bottom = 1.5f;
    public float tall = 1.5f;
    public Mesh Plane
    {
        get
        {
            return plane;
        }
    }
    private static Dictionary<TerrainViewFeature, Mesh> surfaceDictionary = new Dictionary<TerrainViewFeature, Mesh>(32);
    private static Dictionary<TerrainViewFeature, Mesh> innerDictionary = new Dictionary<TerrainViewFeature, Mesh>(32);
    private static Dictionary<TerrainTallFeature, Mesh> outterDictionary = new Dictionary<TerrainTallFeature, Mesh>(9);

    private Vector3[] surfaceVertices;
    private Vector3[] surfaceNormals;
    private Vector2[] surfaceUVs;
    private int[][] surfaceTriangles;

    private Vector3[] innerVertices;
    private Vector3[] innerNormals;
    private Vector2[] innerUVs;
    private int[][] innerTriangles;

    private Vector3[] outterVertices;
    private Vector3[] outterNormals;
    private Vector2[] outterUVs;
    private int[][] outterTriangles;

    private Mesh plane;

    Vector3 normalUp = new Vector3(0, 1, 0);
    Vector3 normalFront = new Vector3(0, 0, 1);
    Vector3 normalBack = new Vector3(0, 0, -1);
    Vector3 normalLeft = new Vector3(-1, 0, 0);
    Vector3 normalRight = new Vector3(1, 0, 0);

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
        Vector3 offset = new Vector3(-0.5f, 0.0f, -0.5f);
        Vector3 bottomOffset = new Vector3(0, -bottom, 0);

        // plane
        plane = new Mesh();
        plane.vertices = new Vector3[]{
            new Vector3(0, 0, 0) + offset,
            new Vector3(1, 0, 0) + offset,
            new Vector3(1, 0, 1) + offset,
            new Vector3(0, 0, 1) + offset,
        };
        plane.triangles = new int[] { 0, 3, 1, 1, 3, 2 };

        // Terrain
        float p0 = 0.0f;
        float p1 = edge;
        float p2 = 1.0f - edge;
        float p3 = 1.0f;
        float[] points = new float[] { p0, p1, p2, p3 };

        // Surface
        surfaceVertices = new Vector3[16];
        surfaceNormals = new Vector3[16];
        surfaceUVs = new Vector2[16];

        for (int y = 0; y < 4; y++)
            for (int x = 0; x < 4; x++)
            {
                surfaceVertices[y * 4 + x] = new Vector3(points[x], 0, points[y]) + offset;
                surfaceUVs[y * 4 + x] = new Vector2(points[x], points[y]);
                surfaceNormals[y * 4 + x] = normalUp;
            }

        surfaceTriangles = new int[9][];
        surfaceTriangles[0] = new int[6] { 0, 4, 1, 1, 4, 5 };
        surfaceTriangles[1] = new int[6] { 1, 5, 2, 2, 5, 6 };
        surfaceTriangles[2] = new int[6] { 2, 6, 3, 3, 6, 7 };
        surfaceTriangles[3] = new int[6] { 4, 8, 5, 5, 8, 9 };
        surfaceTriangles[4] = new int[6] { 5, 9, 6, 6, 9, 10 };
        surfaceTriangles[5] = new int[6] { 6, 10, 7, 7, 10, 11 };
        surfaceTriangles[6] = new int[6] { 8, 12, 9, 9, 12, 13 };
        surfaceTriangles[7] = new int[6] { 9, 13, 10, 10, 13, 14 };
        surfaceTriangles[8] = new int[6] { 10, 14, 11, 11, 14, 15 };

        // Inner
        innerVertices = new Vector3[32];
        innerNormals = new Vector3[32];

        innerVertices[0] = new Vector3(p0, 0, p1);
        innerVertices[1] = new Vector3(p1, 0, p1);
        innerVertices[2] = new Vector3(p1, 0, p1);
        innerVertices[3] = new Vector3(p1, 0, p0);
        innerVertices[4] = new Vector3(p2, 0, p0);
        innerVertices[5] = new Vector3(p2, 0, p1);
        innerVertices[6] = new Vector3(p2, 0, p1);
        innerVertices[7] = new Vector3(p3, 0, p1);
        innerVertices[8] = new Vector3(p3, 0, p2);
        innerVertices[9] = new Vector3(p2, 0, p2);
        innerVertices[10] = new Vector3(p2, 0, p2);
        innerVertices[11] = new Vector3(p2, 0, p3);
        innerVertices[12] = new Vector3(p1, 0, p3);
        innerVertices[13] = new Vector3(p1, 0, p2);
        innerVertices[14] = new Vector3(p1, 0, p2);
        innerVertices[15] = new Vector3(p0, 0, p2);

        innerNormals[0] = normalFront;
        innerNormals[1] = normalFront;
        innerNormals[2] = normalRight;
        innerNormals[3] = normalRight;
        innerNormals[4] = normalLeft;
        innerNormals[5] = normalLeft;
        innerNormals[6] = normalFront;
        innerNormals[7] = normalFront;
        innerNormals[8] = normalBack;
        innerNormals[9] = normalBack;
        innerNormals[10] = normalLeft;
        innerNormals[11] = normalLeft;
        innerNormals[12] = normalRight;
        innerNormals[13] = normalRight;
        innerNormals[14] = normalBack;
        innerNormals[15] = normalBack;

        for (int i = 0; i < 16; i++)
        {
            innerVertices[i] = innerVertices[i] + offset;
            innerVertices[i + 16] = innerVertices[i] + bottomOffset;
            innerNormals[i + 16] = innerNormals[i];
        }

        innerTriangles = new int[12][];
        innerTriangles[0] = new int[6] { 1, 0, 16, 1, 16, 17 };
        innerTriangles[1] = new int[6] { 3, 2, 18, 3, 18, 19 };
        innerTriangles[2] = new int[6] { 5, 4, 20, 5, 20, 21 };
        innerTriangles[3] = new int[6] { 7, 6, 22, 7, 22, 23 };
        innerTriangles[4] = new int[6] { 9, 8, 24, 9, 24, 25 };
        innerTriangles[5] = new int[6] { 11, 10, 26, 11, 26, 27 };
        innerTriangles[6] = new int[6] { 13, 12, 28, 13, 28, 29 };
        innerTriangles[7] = new int[6] { 15, 14, 30, 15, 30, 31 };
        innerTriangles[8] = new int[6] { 1, 14, 30, 1, 30, 17 };
        innerTriangles[9] = new int[6] { 5, 2, 18, 5, 18, 21 };
        innerTriangles[10] = new int[6] { 9, 6, 22, 9, 22, 25 };
        innerTriangles[11] = new int[6] { 13, 10, 26, 13, 26, 29 };

        // Outter
        outterVertices = new Vector3[16];
        outterNormals = new Vector3[16];
        outterUVs = new Vector2[16];

        outterVertices[0] = new Vector3(p0, 0, p0);
        outterVertices[1] = new Vector3(p0, 0, p0);
        outterVertices[2] = new Vector3(p3, 0, p0);
        outterVertices[3] = new Vector3(p3, 0, p0);
        outterVertices[4] = new Vector3(p3, 0, p3);
        outterVertices[5] = new Vector3(p3, 0, p3);
        outterVertices[6] = new Vector3(p0, 0, p3);
        outterVertices[7] = new Vector3(p0, 0, p3);

        outterNormals[0] = normalLeft;
        outterNormals[1] = normalBack;
        outterNormals[2] = normalBack;
        outterNormals[3] = normalRight;
        outterNormals[4] = normalRight;
        outterNormals[5] = normalFront;
        outterNormals[6] = normalFront;
        outterNormals[7] = normalLeft;

        outterUVs[0] = new Vector2(1, 1);
        outterUVs[1] = new Vector2(0, 1);
        outterUVs[2] = new Vector2(1, 1);
        outterUVs[3] = new Vector2(0, 1);
        outterUVs[4] = new Vector2(1, 1);
        outterUVs[5] = new Vector2(0, 1);
        outterUVs[6] = new Vector2(1, 1);
        outterUVs[7] = new Vector2(0, 1);


        for (int i = 0; i < 8; i++)
        {
            outterVertices[i] = outterVertices[i] + offset;
            outterVertices[i + 8] = outterVertices[i] + bottomOffset;
            outterNormals[i + 8] = outterNormals[i];
            outterUVs[i + 8] = outterUVs[i] - new Vector2(0, 1);
        }

        outterTriangles = new int[4][];
        outterTriangles[0] = new int[6] { 1, 2, 10, 1, 10, 9 };
        outterTriangles[1] = new int[6] { 3, 4, 12, 3, 12, 11 };
        outterTriangles[2] = new int[6] { 5, 6, 14, 5, 14, 13 };
        outterTriangles[3] = new int[6] { 7, 0, 8, 7, 8, 15 };
    }

    public Mesh GetTerrainSurfaceMesh(TerrainViewFeature feature)
    {
        Mesh mesh;
        if (surfaceDictionary.TryGetValue(feature, out mesh))
            return mesh;
        mesh = generateSurfaceMesh(feature);
        surfaceDictionary.Add(feature, mesh);
        return mesh;
    }

    public Mesh GetTerrainInnerMesh(TerrainViewFeature feature)
    {
        Mesh mesh;
        if (innerDictionary.TryGetValue(feature, out mesh))
            return mesh;
        mesh = generateInnerMesh(feature);
        innerDictionary.Add(feature, mesh);
        return mesh;
    }

    public Mesh GetTerrainOutterMesh(TerrainTallFeature feature)
    {
        Mesh mesh;
        if (outterDictionary.TryGetValue(feature, out mesh))
            return mesh;
        mesh = generateOutterMesh(feature);
        outterDictionary.Add(feature, mesh);
        return mesh;
    }

    private Mesh generateInnerMesh(TerrainViewFeature feature)
    {
        if (!feature.WaterCenter)
            return null;

        Mesh mesh = new Mesh();

        mesh.vertices = innerVertices;

        Vector3[] normals = new Vector3[32];
        System.Array.Copy(innerNormals, normals, 32);

        List<int> triangleList = new List<int>();


        if (!feature.WaterF)
            triangleList.AddRange(innerTriangles[11]);
        if (!feature.WaterL)
            triangleList.AddRange(innerTriangles[8]);
        if (!feature.WaterB)
            triangleList.AddRange(innerTriangles[9]);
        if (!feature.WaterR)
            triangleList.AddRange(innerTriangles[10]);

        if (!feature.WaterFL && feature.WaterF)
            triangleList.AddRange(innerTriangles[6]);
        if (!feature.WaterFL && feature.WaterL)
            triangleList.AddRange(innerTriangles[7]);
        if (!feature.WaterBL && feature.WaterB)
            triangleList.AddRange(innerTriangles[1]);
        if (!feature.WaterBL && feature.WaterL)
            triangleList.AddRange(innerTriangles[0]);
        if (!feature.WaterBR && feature.WaterB)
            triangleList.AddRange(innerTriangles[2]);
        if (!feature.WaterBR && feature.WaterR)
            triangleList.AddRange(innerTriangles[3]);
        if (!feature.WaterFR && feature.WaterF)
            triangleList.AddRange(innerTriangles[5]);
        if (!feature.WaterFR && feature.WaterR)
            triangleList.AddRange(innerTriangles[4]);

        if (!feature.WaterF)
            normals[10] = normals[13] = normals[26] = normals[29] = normalBack;
        if (!feature.WaterL)
            normals[1] = normals[14] = normals[17] = normals[30] = normalRight;
        if (!feature.WaterB)
            normals[2] = normals[5] = normals[18] = normals[21] = normalFront;
        if (!feature.WaterR)
            normals[6] = normals[9] = normals[22] = normals[25] = normalLeft;


        mesh.triangles = triangleList.ToArray();
        mesh.normals = normals;

        return mesh;

    }

    private Mesh generateSurfaceMesh(TerrainViewFeature feature)
    {
        Mesh mesh = new Mesh();

        mesh.vertices = surfaceVertices;
        mesh.normals = surfaceNormals;
        mesh.uv = surfaceUVs;

        List<int> triangleList = new List<int>();

        if (!feature.WaterCenter)
            triangleList.AddRange(surfaceTriangles[4]);
        if (!feature.WaterF)
            triangleList.AddRange(surfaceTriangles[7]);
        if (!feature.WaterL)
            triangleList.AddRange(surfaceTriangles[3]);
        if (!feature.WaterB)
            triangleList.AddRange(surfaceTriangles[1]);
        if (!feature.WaterR)
            triangleList.AddRange(surfaceTriangles[5]);
        if (!feature.WaterFL)
            triangleList.AddRange(surfaceTriangles[6]);
        if (!feature.WaterBL)
            triangleList.AddRange(surfaceTriangles[0]);
        if (!feature.WaterBR)
            triangleList.AddRange(surfaceTriangles[2]);
        if (!feature.WaterFR)
            triangleList.AddRange(surfaceTriangles[8]);

        mesh.triangles = triangleList.ToArray();

        return mesh;
    }

    private Mesh generateOutterMesh(TerrainTallFeature feature)
    {
        if (!feature.Front && !feature.Left && !feature.Back && !feature.Right)
            return null;

        Mesh mesh = new Mesh();

        mesh.vertices = outterVertices;
        mesh.normals = outterNormals;
        mesh.uv = outterUVs;

        List<int> triangleList = new List<int>();

        if (feature.Front)
            triangleList.AddRange(outterTriangles[2]);
        if (feature.Left)
            triangleList.AddRange(outterTriangles[3]);
        if (feature.Back)
            triangleList.AddRange(outterTriangles[0]);
        if (feature.Right)
            triangleList.AddRange(outterTriangles[1]);


        mesh.triangles = triangleList.ToArray();

        return mesh;
    }
}
