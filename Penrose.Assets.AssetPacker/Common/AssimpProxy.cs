using System.Numerics;
using Penrose.Assets.AssetPacker.Types;
using Silk.NET.Assimp;
using AssimpFace = Silk.NET.Assimp.Face;
using AssimpMesh = Silk.NET.Assimp.Mesh;
using Face = Penrose.Assets.AssetPacker.Types.Face;
using Mesh = Penrose.Assets.AssetPacker.Types.Mesh;

namespace Penrose.Assets.AssetPacker.Common;

public class AssimpProxy : IDisposable
{
    private readonly Assimp _assimp;

    public AssimpProxy()
    {
        _assimp = Assimp.GetApi();
    }

    public IReadOnlyCollection<Mesh> ReadMesh(string path)
    {
        unsafe
        {
            Scene* scene = _assimp.ImportFile(path, (uint)PostProcessSteps.Triangulate);

            if (scene == null || scene->MFlags == Assimp.SceneFlagsIncomplete)
            {
                throw new Exception($"Failed to read mesh {path}: {_assimp.GetErrorStringS()}");
            }

            Context context = new Context(scene);

            ReadScene(context);

            return context.Meshes;
        }
    }

    private class Context
    {
        public unsafe Scene* Scene { get; }
        public List<Mesh> Meshes { get; }

        public unsafe Context(Scene* scene)
        {
            Scene = scene;
            Meshes = new List<Mesh>();
        }
    }

    private unsafe void ReadScene(Context context)
    {
        if (context.Scene->MRootNode == null)
        {
            return;
        }

        ReadNode(context, context.Scene->MRootNode);
    }

    private unsafe void ReadNode(Context context, Node* node)
    {
        for (uint idx = 0; idx < node->MNumMeshes; idx++)
        {
            Mesh mesh = ProcessMesh(context.Scene->MMeshes[node->MMeshes[idx]]);

            context.Meshes.Add(mesh);
        }

        for (uint idx = 0; idx < node->MNumChildren; idx++)
        {
            ReadNode(context, node->MChildren[idx]);
        }
    }

    private unsafe Mesh ProcessMesh(AssimpMesh* mesh)
    {
        List<Vertex> vertices = new List<Vertex>();
        List<Face> faces = new List<Face>();

        for (uint idx = 0; idx < mesh->MNumVertices; idx++)
        {
            Vector3 position = mesh->MVertices[idx];

            Vector3 normal = mesh->MNormals != null
                ? mesh->MNormals[idx]
                : Vector3.Zero;

            Vector4 color = mesh->MColors[0] != null
                ? mesh->MColors[0][idx]
                : Vector4.One;

            Vector3 uvw = mesh->MTextureCoords[0] != null
                ? mesh->MTextureCoords[0][idx]
                : Vector3.Zero;

            Vertex vertex = new Vertex(
                position,
                normal,
                new Vector3(color.X, color.Y, color.Z),
                new Vector2(uvw.X, uvw.Y)
            );

            vertices.Add(vertex);
        }

        for (uint idx = 0; idx < mesh->MNumFaces; idx++)
        {
            AssimpFace face = mesh->MFaces[idx];

            if (face.MNumIndices != 3)
            {
                throw new Exception($"Face #{idx} have {face.MNumIndices} vertices (supported only 3 vertices per face)");
            }

            List<uint> indices = new List<uint>(3);

            for (uint indexIdx = 0; indexIdx < face.MNumIndices; indexIdx++)
            {
                indices.Add(face.MIndices[indexIdx]);
            }

            faces.Add(new Face(indices));
        }

        return new Mesh(vertices, faces);
    }

    public void Dispose()
    {
        _assimp.Dispose();
    }
}