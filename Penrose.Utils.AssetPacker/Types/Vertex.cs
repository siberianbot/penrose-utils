using System.Numerics;

namespace Penrose.Utils.AssetPacker.Types;

public record struct Vertex(Vector3 Position, Vector3 Normal, Vector3 Color, Vector2 UV);