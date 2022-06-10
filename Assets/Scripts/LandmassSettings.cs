using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandmassSettings : MonoBehaviour
{
    public static LandmassSettings current;
    public LandmassSettings ()
    {
        current = this;
    }
    public int TerrainTextureScale;
    public Biome[] Biomes;
    public Biome BorderBiome;

    public Texture2D GenerateTerrainTexture(Tile[,] Tiles)
    {
        int Width = Tiles.GetLength(0);
        int Height = Tiles.GetLength(1);
        Debug.Log(Width.ToString() + " | " + Height.ToString());
        TerrainData TD = new TerrainData(Tiles);
        Texture2D Result = new Texture2D(Width*TerrainTextureScale, Height* TerrainTextureScale);
        float MaxHeight = 0;
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Tiles[x, y].Center.y > MaxHeight)
                {
                    if (Tiles[x,y].Border)
                    {
                        continue;
                    }
                    MaxHeight = Tiles[x, y].Center.y;
                }
            }
        }
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Tiles[x, y].Border)
                {
                    Texture2D BorderTexture = BorderBiome.GetText(x, y, TD);
                    for (int w = 0; w < TerrainTextureScale; w++)
                    {
                        for (int h = 0; h < TerrainTextureScale; h++)
                        {
                            Result.SetPixel(x * TerrainTextureScale + w, y * TerrainTextureScale + h, BorderTexture.GetPixel(w, h));
                        }
                    }
                    continue;
                }
                Biome MyBiome = Biomes[0];
                for (int i = 0; i < Biomes.Length; i++)
                {
                    if (Biomes[i].Evaluate(x,y,TD))
                    {
                        MyBiome = Biomes[i];
                    }
                }
                Texture2D MyText = MyBiome.GetText(x,y,TD);
                for (int w = 0; w < TerrainTextureScale; w++)
                {
                    for (int h = 0; h < TerrainTextureScale; h++)
                    {
                        Result.SetPixel(x * TerrainTextureScale + w, y * TerrainTextureScale + h, MyText.GetPixel(w, h));
                    }
                }
            }
        }
        Result.filterMode = FilterMode.Point;
        Result.Apply();
        return Result;
    }
}
