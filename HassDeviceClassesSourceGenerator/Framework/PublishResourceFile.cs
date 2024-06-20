using System.IO;


namespace HomeAssistantTypesSourceGenerator.Framework;

internal class PublishResourceFile : IApiSourceFile
{
    public PublishResourceFile(string resourceFilename, string? subFolder = null)
    {
        Filename = $"{subFolder ?? ""}{Path.GetFileNameWithoutExtension(resourceFilename)}.g.{Path.GetExtension(resourceFilename)}";
        Content = FileSourceGeneratorContext.GetResourceFileContent(resourceFilename)!;
    }

    public string Content { get; }

    public string Filename { get; }
}