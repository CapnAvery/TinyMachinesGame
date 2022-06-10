using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Biome : ScriptableObject
{
    public abstract bool Evaluate(int   X,int   Y,TerrainData   Data);
    public abstract Texture2D GetText(int    X,int   Y,TerrainData   Data);
}
