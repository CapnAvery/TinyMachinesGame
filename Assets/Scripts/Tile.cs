using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Vector2Int Center;
    public Vector3[] Vertices;
    public Vector2[] UVs;
    public bool Border;

    public Tile(int X, int Y)
    {
        Center = new Vector2Int(X, Y);
    }

    public void SetData(int Index, int NextIndex, Vector3[] Vertx, Vector2[] UV)
    {
        Vertices = new Vector3[4];
        Vertices[0] = Vertx[Index];
        Vertices[1] = Vertx[Index + 1];
        Vertices[2] = Vertx[NextIndex];
        Vertices[3] = Vertx[NextIndex + 1];
        UVs = new Vector2[4];
        UVs[0] = UV[Index];
        UVs[1] = UV[Index + 1];
        UVs[2] = UV[NextIndex];
        UVs[3] = UV[NextIndex + 1];
    }

    //public void AddNormals(List<Vector3> Normals)
    //{
    //    Mesh Temp = new Mesh();
    //    Temp.vertices = Vertices;
    //    int[] Triangles = new int[6];
    //    Triangles[0] = 0;
    //    Triangles[1] = 1;
    //    Triangles[2] = 2;

    //    Triangles[3] = 1;
    //    Triangles[4] = 3;
    //    Triangles[5] = 2;
    //    Temp.triangles = Triangles;
    //    Temp.RecalculateNormals();
    //    for (int i = 0; i < Temp.normals.Length; i++)
    //    {
    //        Normals.Add(Temp.normals[i]);
    //    }
    //}

    public void AddData(List<Vector3> Verts, List<int> Triangles, List<Vector2> UV)
    {
        Triangles.Add(Verts.Count);
        Triangles.Add(Verts.Count + 1);
        Triangles.Add(Verts.Count + 2);

        Triangles.Add(Verts.Count + 1);
        Triangles.Add(Verts.Count + 3);
        Triangles.Add(Verts.Count + 2);
        Verts.Add(Vertices[0]);
        Verts.Add(Vertices[1]);
        Verts.Add(Vertices[2]);
        Verts.Add(Vertices[3]);

        for (int i = 0; i < UVs.Length; i++)
        {
            UV.Add(UVs[i]);
        }
    }
} 
