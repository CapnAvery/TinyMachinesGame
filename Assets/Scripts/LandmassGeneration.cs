using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandmassGeneration : MonoBehaviour
{
    public GameObject ChunkPrefab;
    public Vector2Int Size;                     //The amount of tiles that the entire landmass will exist out of
    public Vector2Int Seed;                     //The offset for the perlin noise
    public bool RandomSeed;                     //Make this true if you want the offset to be random
    public PerlinGenerator[] Generators;        //These will be the generators that will determine the heights of the landmass
    public float BorderRange;                   //How big the border of the terrain is. It is deducted from size. That means if your landmass is 100x100, and your border is 10, the size of the play area will be 80x80
    public  AnimationCurve  MinOuter;           //The height of the border will be a value between these two curves
    public  AnimationCurve  MaxOuter;
    public float BorderAmplifier;               //This amplifies the value gained from the curves
    public float Spacing;                       //This is the size of an individual tile
    public bool Snapping;                       //Use snapping or not?
    public float SnappingAmount;                //How snappy the snapping should be
    public int ChunkSize;                       //The size of the chunks
    public Material MyMat;
    public RawImage Display;
    public Texture2D Text;
    private void Start()
    {
        StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
        //This works out how many chunks we are going to cut our terrain into
        int XChunks = Mathf.CeilToInt(Mathf.Clamp(Size.x / (float)ChunkSize, 1, Mathf.Infinity));
        int YChunks = Mathf.CeilToInt(Mathf.Clamp(Size.y / (float)ChunkSize, 1, Mathf.Infinity));

        Vector2Int TotalSize = new Vector2Int(XChunks * ChunkSize, YChunks * ChunkSize);

        //This intializes the chunks
        Chunk[,] Chunks = new Chunk[XChunks, YChunks];
        for (int x = 0; x < XChunks; x++)
        {
            for (int y = 0; y < YChunks; y++)
            {
                Chunks[x, y] = new Chunk(x, y);
            }
        }

        //Randomize the seed if true
        if (RandomSeed)
        {
            Seed = new Vector2Int(Random.Range(0, 2000), Random.Range(0, 2000));
        }

        //This initializes the heights
        float[,] heights = new float[Size.x, Size.y];
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                heights[x, y] = 0;
            }
        }

        //This calculates the heights
        List<Vector2Int> BorderPoints = new List<Vector2Int>();
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                float xCoord = (float)x / (float)Size.x + Seed.x;
                float yCoord = (float)y / (float)Size.y + Seed.y;
                float Height = 0;

                for (int i = 0; i < Generators.Length; i++)
                {
                    Height += Generators[i].SetHeight(Height, xCoord, yCoord);
                }

                heights[x, y] = Height;
                if (x < BorderRange)
                {
                    heights[x, y] = BorderHeight(heights[x, y], x / (float)BorderRange);
                    BorderPoints.Add(new Vector2Int(x, y));
                }
                if (y < BorderRange)
                {
                    heights[x, y] = BorderHeight(heights[x, y], y / (float)BorderRange);
                    BorderPoints.Add(new Vector2Int(x, y));
                }
                if (x > Size.x - 1 - BorderRange)
                {
                    heights[x, y] = BorderHeight(heights[x, y], 1.0f - (x - (Size.x - 1 - BorderRange)) / (float)BorderRange);
                    BorderPoints.Add(new Vector2Int(x, y));
                }
                if (y > Size.y - 1 - BorderRange)
                {
                    heights[x, y] = BorderHeight(heights[x, y], 1.0f - (y - (Size.y - 1 - BorderRange)) / (float)BorderRange);
                    BorderPoints.Add(new Vector2Int(x, y));
                }
                if (Snapping)
                {
                    heights[x, y] = Mathf.RoundToInt(heights[x, y] / SnappingAmount) * SnappingAmount;
                }
            }
        }

        //This gives the height data to each of the chunks
        for (int x = 0; x < XChunks; x++)
        {
            for (int y = 0; y < YChunks; y++)
            {
                Chunks[x, y].GrabHeights(heights, x * ChunkSize, y * ChunkSize, ChunkSize);
                Chunks[x, y].CalculateUVs(TotalSize);
                Chunks[x, y].GenerateMesh(ChunkPrefab, MyMat, Size, BorderPoints);
                yield return null;
            }
        }

        Tile[,] Tiles = new Tile[Size.x, Size.y];
        for (int x = 0; x < XChunks; x++)
        {
            for (int y = 0; y < YChunks; y++)
            {
                Chunks[x, y].FillTiles(Tiles, x * ChunkSize, y * ChunkSize);
            }
        }

        if (Text == null)
        {
            MyMat.mainTexture = TextureGenerator.GenerateTerrain(Tiles);
            Display.texture = MyMat.mainTexture;
        }
        else
        {
            MyMat.mainTexture = Text;
        }

        //Vector3[] Vertices = new Vector3[Size.x * Size.y];
        //Vector2[] UVs = new Vector2[Size.x * Size.y];
        ////List<int> Triangles = new List<int>();
        //for (int x = 0; x < Size.x; x++)
        //{
        //    for (int y = 0; y < Size.y; y++)
        //    {
        //        int Index = x * Size.x + y;
        //        Vertices[Index] = new Vector3(x*Spacing, heights[x, y], y*Spacing);
        //        UVs[Index] = new Vector2((float)x / Size.x, (float)y / Size.y);
        //    }
        //}
        //List<Tile> AllTiles = new List<Tile>();
        //for (int x = 0; x < Size.x-1; x++)
        //{
        //    for (int y = 0; y < Size.y-1; y++)
        //    {
        //        int Index = x * Size.x + y;
        //        int NextIndex = (x+1) * Size.x + y;
        //        //Triangles.Add(Index);
        //        //Triangles.Add(Index + 1);
        //        //Triangles.Add(NextIndex);

        //        //Triangles.Add(Index+1);
        //        //Triangles.Add(NextIndex+1);
        //        //Triangles.Add(NextIndex);
        //        AllTiles.Add(new Tile(Index, NextIndex, Vertices, UVs));
        //    }
        //}
        //List<Vector3> Normals = new List<Vector3>();
        //List<Vector3> NewVerts = new List<Vector3>();
        //List<Vector2> NewUVs = new List<Vector2>();
        //List<int> Triangles = new List<int>();
        //for (int i = 0; i < AllTiles.Count; i++)
        //{
        //    AllTiles[i].AddNormals(Normals);dawawd
        //    AllTiles[i].AddData(NewVerts,Triangles,NewUVs);
        //}
        //Debug.Log(Normals.Count);
        //Mesh mesh = new Mesh();
        //mesh.vertices = NewVerts.ToArray();
        //mesh.uv = NewUVs.ToArray();
        //mesh.triangles = Triangles.ToArray();
        //mesh.normals = Normals.ToArray();
        //mesh.RecalculateNormals();
    }

    float   BorderHeight    (float  InputHeight,float   T)
    {
        return InputHeight +Random.Range(MinOuter.Evaluate(1.0f-T),MaxOuter.Evaluate(1.0f-T))*BorderAmplifier;
    }
}
