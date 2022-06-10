using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TerrainGeneration/PerlinSO", order = 1)]
public class PerlinGenerator : ScriptableObject
{
    public Vector2 InnerScale;
    public float HeightAmplifier;

    public float SetHeight(float Height, float XCord,float YCord)
    {
        return Mathf.PerlinNoise(XCord * InnerScale.x, YCord * InnerScale.y) * HeightAmplifier;
    }
}
