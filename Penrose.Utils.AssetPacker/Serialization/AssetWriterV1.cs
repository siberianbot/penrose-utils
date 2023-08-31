using System.Numerics;
using System.Text;
using Penrose.Utils.AssetPacker.Types;

namespace Penrose.Utils.AssetPacker.Serialization;

public class AssetWriterV1 : IAssetWriter
{
    private readonly BinaryWriter _writer;

    private AssetWriterV1(Stream stream)
    {
        _writer = new BinaryWriter(stream, Encoding.UTF8);
    }

    public void WriteMesh(Header header, Mesh mesh)
    {
        WriteMagic();
        WriteHeader(header);

        _writer.Write(BitConverter.GetBytes(mesh.Vertices.Count));
        _writer.Write(BitConverter.GetBytes(mesh.Faces.Count));

        foreach (Vertex vertex in mesh.Vertices)
        {
            WriteVertex(vertex);
        }

        foreach (Face face in mesh.Faces)
        {
            WriteFace(face);
        }
    }

    private void WriteMagic()
    {
        _writer.Write(new[] { 'P', 'n', 'r', 's' });
    }

    private void WriteHeader(Header header)
    {
        _writer.Write((byte)header.Version);
        _writer.Write((byte)header.Type);
    }

    private void WriteVertex(Vertex vertex)
    {
        WriteVector3(vertex.Position);
        WriteVector3(vertex.Normal);
        WriteVector3(vertex.Color);
        WriteVector2(vertex.UV);
    }

    private void WriteFace(Face face)
    {
        _writer.Write(BitConverter.GetBytes(face.Indices[0]));
        _writer.Write(BitConverter.GetBytes(face.Indices[1]));
        _writer.Write(BitConverter.GetBytes(face.Indices[2]));
    }

    private void WriteVector2(Vector2 value)
    {
        _writer.Write(BitConverter.GetBytes(value.X));
        _writer.Write(BitConverter.GetBytes(value.Y));
    }

    private void WriteVector3(Vector3 value)
    {
        _writer.Write(BitConverter.GetBytes(value.X));
        _writer.Write(BitConverter.GetBytes(value.Y));
        _writer.Write(BitConverter.GetBytes(value.Z));
    }

    public static IAssetWriter FromPath(string path, bool overwrite)
    {
        if (!overwrite && File.Exists(path))
        {
            throw new Exception($"File {path} already exists"); // TODO: introduce exception
        }

        Stream stream = new FileStream(path, overwrite ? FileMode.Create : FileMode.CreateNew);

        return new AssetWriterV1(stream);
    }

    public void Dispose()
    {
        _writer.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _writer.DisposeAsync();
    }
}