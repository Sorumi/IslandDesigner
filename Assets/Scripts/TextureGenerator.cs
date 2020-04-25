using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureGenerator : MonoBehaviour
{
    public static TextureGenerator Instance
    {
        get
        {
            if (instance == null)
            {
                if (!Application.isPlaying) return null;
                var objects = FindObjectsOfType<TextureGenerator>();
                if (objects.Length == 0)
                {
                    var go = new GameObject("TextureGenerator Instance");
                    instance = go.AddComponent<TextureGenerator>();
                }
                else if (objects.Length >= 1)
                {
                    instance = objects[0];
                }
            }
            return instance;
        }
    }

    private static TextureGenerator instance;
    private static Dictionary<TerrainViewFeature, Texture2D> textureDictionary = new Dictionary<TerrainViewFeature, Texture2D>(32);
    public int width = 64;
    public int edge = 16;
    public Color WaterColor;
    public Color GrassColor;

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
        TerrainViewFeature feature = new TerrainViewFeature(false, false, false, false, false, false, false, false, false);
        textureDictionary.Add(feature, generateTerrain(feature));
    }

    public Texture2D GetTerrainTexture(TerrainViewFeature feature)
    {
        Texture2D texture;
        if (textureDictionary.TryGetValue(feature, out texture))
            return texture;
        texture = generateTerrain(feature);
        textureDictionary.Add(feature, texture);
        return texture;
    }

    private Texture2D generateTerrain(TerrainViewFeature feature)
    {
        Texture2D texture = new Texture2D(width, width);

        int p0 = 0;
        int p1 = edge;
        int p2 = width - edge;
        int p3 = width;

        // Top
        setPixel(texture, p1, p2, p2, p3, feature.WaterT ? WaterColor : GrassColor);

        // Down
        setPixel(texture, p1, p2, p0, p1, feature.WaterD ? WaterColor : GrassColor);

        // Left
        setPixel(texture, p0, p1, p1, p2, feature.WaterL ? WaterColor : GrassColor);

        // Right
        setPixel(texture, p2, p3, p1, p2, feature.WaterR ? WaterColor : GrassColor);


        // TopLeft
        setPixel(texture, p0, p1, p2, p3, feature.WaterTL ? WaterColor : GrassColor);

        // TopRight
        setPixel(texture, p2, p3, p2, p3, feature.WaterTR ? WaterColor : GrassColor);

        // DownLeft
        setPixel(texture, p0, p1, p0, p1, feature.WaterDL ? WaterColor : GrassColor);

        // DownRight
        setPixel(texture, p2, p3, p0, p1, feature.WaterDR ? WaterColor : GrassColor);

        // Center
        setPixel(texture, p1, p2, p1, p2, feature.WaterCenter ? WaterColor : GrassColor);

        texture.Apply();
        return texture;
    }

    private void setPixel(Texture2D texture, int xStart, int xEnd, int yStart, int yEnd, Color color)
    {
        for (int x = xStart; x < xEnd; x++)
            for (int y = yStart; y < yEnd; y++)
                texture.SetPixel(x, y, color);
    }
}
