using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

public class ObjLoader
{
    public List<Vector3> Vertices { get; private set; } = new();
    public List<Vector3> Normals { get; private set; } = new();
    public List<Face> Faces { get; private set; } = new();

    public void Load(string path)
    {
        foreach (var line in File.ReadAllLines(path))
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) continue;

            switch (parts[0])
            {
                case "v":
                    Vertices.Add(ParseVec3(parts));
                    break;
                case "vn":
                    Normals.Add(ParseVec3(parts));
                    break;
                case "f":
                    Faces.Add(new Face(parts[1], parts[2], parts[3]));
                    break;
            }
        }
    }

    private Vector3 ParseVec3(string[] parts)
    {
        return new Vector3(
            float.Parse(parts[1], CultureInfo.InvariantCulture),
            float.Parse(parts[2], CultureInfo.InvariantCulture),
            float.Parse(parts[3], CultureInfo.InvariantCulture)
        );
    }

    public class Face
    {
        public (int v, int n)[] Indices = new (int, int)[3];

        public Face(string p1, string p2, string p3)
        {
            Indices[0] = ParseVertex(p1);
            Indices[1] = ParseVertex(p2);
            Indices[2] = ParseVertex(p3);
        }

        private (int v, int n) ParseVertex(string part)
        {
            var elems = part.Split("//"); // формат v//n
            return (int.Parse(elems[0]) - 1, int.Parse(elems[1]) - 1);
        }
    }

    public void DrawModel(ObjLoader model)
    {
        GL.Begin(PrimitiveType.Triangles);

        foreach (var face in model.Faces)
        {
            foreach (var (vIdx, nIdx) in face.Indices)
            {
                var normal = model.Normals[nIdx];
                var vertex = model.Vertices[vIdx];
                GL.Normal3(normal);
                GL.Vertex3(vertex);
            }
        }

        GL.End();
    }
}