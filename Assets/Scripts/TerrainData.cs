using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainData 
{
    public Tile[,] Tiles;
    public Biome[,] Biomes;
    
    public  TerrainData(Tile[,]    _Tiles)
    {
        Tiles = _Tiles;
        Biomes = new Biome[Tiles.GetLength(0), Tiles.GetLength(1)];
    }
}
