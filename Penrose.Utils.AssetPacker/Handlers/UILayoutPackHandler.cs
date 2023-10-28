using System.Text;
using System.Xml;
using Penrose.Utils.AssetPacker.Serialization;
using Penrose.Utils.AssetPacker.Types;

namespace Penrose.Utils.AssetPacker.Handlers;

public class UILayoutPackHandler : IPackHandler
{
    public async Task HandleAsync(string input, IAssetWriter assetWriter)
    {
        XmlDocument xml = new XmlDocument();
        xml.Load(input);

        XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
        {
            Async = true,
            Indent = false,
            OmitXmlDeclaration = true,
            Encoding = Encoding.UTF8,
            CloseOutput = false,
        };

        await using MemoryStream memoryStream = new MemoryStream();
        await using XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings);
        xml.Save(xmlWriter);

        assetWriter.WriteUILayoutInfo(new UILayout((int)memoryStream.Length));

        memoryStream.Seek(0, SeekOrigin.Begin);

        await memoryStream.CopyToAsync(assetWriter.Stream);
    }
}