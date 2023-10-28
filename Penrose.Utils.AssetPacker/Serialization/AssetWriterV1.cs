using System.Numerics;
using System.Text;
using Penrose.Utils.AssetPacker.Common;
using Penrose.Utils.AssetPacker.Types;

namespace Penrose.Utils.AssetPacker.Serialization;

public class AssetWriterV1 : IAssetWriter
{
    private readonly BinaryWriter _writer;

    private AssetWriterV1(Stream stream)
    {
        Stream = stream;
        _writer = new BinaryWriter(stream, Encoding.UTF8, true);
    }

    public Stream Stream { get; }

    public void WriteHeader(Header header)
    {
        WriteMagic();

        _writer.Write((byte)header.Version);
        _writer.Write((byte)header.Type);
    }

    public void WriteMesh(Mesh mesh)
    {
        _writer.Write(BitConverter.GetBytes(mesh.Vertices.Count));
        _writer.Write(BitConverter.GetBytes(mesh.Indices.Count));

        foreach (Vertex vertex in mesh.Vertices)
        {
            WriteVertex(vertex);
        }

        foreach (uint index in mesh.Indices)
        {
            _writer.Write(BitConverter.GetBytes(index));
        }
    }

    public void WriteImage(Image image)
    {
        _writer.Write(BitConverter.GetBytes(image.Width));
        _writer.Write(BitConverter.GetBytes(image.Height));
        _writer.Write(image.Channels);
        _writer.Write(image.Data.Length);
        _writer.Write(image.Data);
    }

    public void WriteShaderInfo(Shader shader)
    {
        _writer.Write(BitConverter.GetBytes(shader.ContentLength));
    }

    public void WriteUILayoutInfo(UILayout uiLayout)
    {
        _writer.Write(BitConverter.GetBytes(uiLayout.ContentLength));
    }

    private void WriteMagic()
    {
        _writer.Write(new[] { 'P', 'n', 'r', 's' });
    }

    private void WriteVertex(Vertex vertex)
    {
        WriteVector3(vertex.Position);
        WriteVector3(vertex.Normal);
        WriteVector3(vertex.Color);
        WriteVector2(vertex.UV);
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
            throw new PackerException($"File {path} already exists");
        }

        Stream stream = new FileStream(path, overwrite ? FileMode.Create : FileMode.CreateNew);

        return new AssetWriterV1(stream);
    }

    public void Dispose()
    {
        _writer.Dispose();
        Stream.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _writer.DisposeAsync();
        await Stream.DisposeAsync();
    }
}