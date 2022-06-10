using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public Vector2Int Center;
    public Vector2Int StartPos;
    public Vector2Int EndPos;
    int XSize => EndPos.x - StartPos.x;
    int YSize => EndPos.y - StartPos.y;
    public float[,] Heights;
    public GameObject MyObject;
    public Tile[,] MyTiles;
    public Vector2 UVStart;
    public Vector2 UVEnd;
    Vector2 UVSize => new Vector2(UVEnd.x - UVStart.x, UVEnd.y - UVStart.y);
    
    public  Chunk   (int    X,int   Y)
    {
        Center = new Vector2Int(X,Y);

    }

    public  void    GrabHeights(float[,]    AllHeights,int  XStart,int  YStart,int  ChunkSize)
    {
        Heights = new float[ChunkSize+1, ChunkSize+1];
        int Width = AllHeights.GetLength(0);
        int Height = AllHeights.GetLength(1);
        StartPos = new Vector2Int(XStart,YStart);
        EndPos = StartPos + new Vector2Int(ChunkSize,ChunkSize);

        for (int x = 0; x < ChunkSize+1; x++)
        {
            for (int y = 0; y < ChunkSize+1; y++)
            {
                int X = Mathf.Clamp(XStart + x, 0, Width - 1);
                int Y = Mathf.Clamp(YStart + y, 0, Height - 1);
                Heights[x, y] = AllHeights[X, Y];
            }
        }
    }

    public  void    CalculateUVs    (Vector2Int Size)
    {
        UVStart.x = StartPos.x / (float)Size.x;
        UVStart.y = StartPos.y / (float)Size.y;

        UVEnd.x = EndPos.x / (float)Size.x;
        UVEnd.y = EndPos.y / (float)Size.y;
    }

    public  Vector2Int GetClampedSize  (Vector2Int Size)
    {
        int _XSize = XSize;
        int _YSize = YSize;
        if (StartPos.x + XSize > Size.x)
        {
            _XSize = Mathf.Abs(Size.x - StartPos.x);
        }
        if (StartPos.y + YSize > Size.y)
        {
            _YSize = Mathf.Abs(Size.y - StartPos.y);
        }
        return new Vector2Int(_XSize, _YSize);
    }
    //This function takes a 2d array of tiles and a start x and start y, then adds all its tiles to the array
    public void FillTiles(Tile[,] AllTiles, int XStart, int YStart)
    {
        int _XSize = MyTiles.GetLength(0);
        int _YSize = MyTiles.GetLength(1);
        int Width = AllTiles.GetLength(0);
        int Height = AllTiles.GetLength(1);
        for (int x = 0; x < _XSize; x++)
        {
            for (int y = 0; y < _YSize; y++)
            {
                int X = Mathf.Clamp(XStart + x, 0, Width - 1);
                int Y = Mathf.Clamp(YStart + y, 0, Height - 1);
                AllTiles[X, Y] = MyTiles[x, y];
            }
        }
    }

    public void GenerateMesh(GameObject Prefab,Material Mat,Vector2Int  Size,List<Vector2Int>  BorderPoints)
    {
        int _XSize = XSize;
        int _YSize = YSize;
        if (StartPos.x+XSize>Size.x)
        {
            _XSize = Mathf.Abs(Size.x-StartPos.x);
        }
        if (StartPos.y + YSize > Size.y)
        {
            _YSize = Mathf.Abs(Size.y - StartPos.y);
        }
        MyTiles = new Tile[_XSize, _YSize];

        for (int x = 0; x < _XSize; x++)
        {
            for (int y = 0; y < _YSize; y++)
            {
                int X = StartPos.x + x;
                int Y = StartPos.y + y;
                if (X>=Size.x || Y>=Size.y)
                {
                    continue;
                }
                MyTiles[x, y] = new Tile(StartPos.x + x, StartPos.y + y);
                Vector3[] Vertices = new Vector3[4];
                Vertices[0] = new Vector3( x, Heights[x, y], y);
                Vertices[1] = new Vector3( x, Heights[x, y + 1],  y + 1);
                Vertices[2] = new Vector3(x + 1, Heights[x + 1, y],  y);
                Vertices[3] = new Vector3(x + 1, Heights[x + 1, y + 1], y + 1);
                bool Border = false;
                for (int i = 0; i < Vertices.Length; i++)
                {
                    if (BorderPoints.Contains(new Vector2Int(Mathf.RoundToInt(Vertices[i].x), Mathf.RoundToInt(Vertices[i].y))))
                    {
                        Border = true;
                        break;
                    }
                }
                MyTiles[x, y].Border = Border;
                MyTiles[x, y].Vertices  =   Vertices;
                Vector2[] UVs = new Vector2[4];
                for (int i = 0; i < UVs.Length; i++)
                {
                    UVs[i] = CalcUV(Vertices[i]);
                }
                MyTiles[x, y].UVs = UVs;
            }
        }

        List<Vector3> AllVertices = new List<Vector3>();
        List<Vector2> AllUVs = new List<Vector2>();
        List<int> Triangles = new List<int>();

        for (int x = 0; x < _XSize; x++)
        {
            for (int y = 0; y < _YSize; y++)
            {
                //MyTiles[x,y].AddNormals(Normals);
                MyTiles[x,y].AddData(AllVertices, Triangles, AllUVs);
            }
        }
        Mesh mesh = new Mesh();
        mesh.vertices = AllVertices.ToArray();
        mesh.uv = AllUVs.ToArray();
        mesh.triangles = Triangles.ToArray();
        mesh.RecalculateNormals();

        MyObject = GameObject.Instantiate(Prefab, new Vector3(Center.x * XSize, 0, Center.y * YSize), Quaternion.identity);
        MyObject.GetComponent<MeshFilter>().mesh = mesh;
        MyObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        MyObject.GetComponent<MeshRenderer>().material = Mat;
    }

    Vector2 CalcUV  (Vector2    Vert)
    {
        return new Vector2((Vert.x / XSize)* UVSize.x, (Vert.y / YSize) * UVSize.y) + UVStart;
    }
}
