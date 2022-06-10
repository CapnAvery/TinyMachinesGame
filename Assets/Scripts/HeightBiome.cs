using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TerrainGeneration/Biomes/HeightBiome", order = 1)]
public class HeightBiome : Biome
{
    public Vector2 Heights;
    public Texture2D[] Textures;
    Texture2D Texture => Textures[Random.Range(0, Textures.Length)];

    public void OnValidate()
    {
        if (Heights.x>Heights.y)
        {
            Heights.x = Heights.y - 1;
        }
    }

    public override bool Evaluate(int X, int Y, TerrainData Data)
    {
        return Data.Tiles[X, Y].Center.y >= Heights.x && Data.Tiles[X, Y].Center.y <= Heights.y;
    }

    public override Texture2D GetText(int X, int Y, TerrainData Data)
    {
        return Texture;
    }
}
