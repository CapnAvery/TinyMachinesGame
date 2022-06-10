using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D GenerateTerrain(Tile[,] Tiles)
    {
        if (LandmassSettings.current!=null)
        {
            return LandmassSettings.current.GenerateTerrainTexture(Tiles);
        }

        int Width = Tiles.GetLength(0);
        int Height = Tiles.GetLength(1);
        Texture2D Result = new Texture2D(Width, Height);
        List<Color> Colors = new List<Color>();
        Colors.Add(Color.Lerp(Color.Lerp(Color.red, Color.yellow, 0.5f), Color.black, 0.5f));
        Colors.Add(Color.Lerp(Color.green, Color.black, 0.5f));
        Colors.Add(Color.Lerp(Color.green, Color.black, 0.5f));
        Color BorderColor = Color.red;
        float MaxHeight = 0;
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Tiles[x, y].Center.y > MaxHeight || !Tiles[x, y].Border)
                {
                    MaxHeight = Tiles[x, y].Center.y;
                }
            }
        }
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Tiles[x,y].Border)
                {
                    Result.SetPixel(x, y, BorderColor);
                    continue;
                }
                Color Col = Colors[Mathf.RoundToInt(Tiles[x, y].Center.y / MaxHeight * (Colors.Count - 1))];
                Result.SetPixel(x, y, Col);
            }
        }
        Result.filterMode = FilterMode.Point;
        Result.Apply();
        return Result;
    }
}
