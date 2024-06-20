namespace HomeAssistantTypesSourceGenerator.Framework;

public struct SourceFile
{
    public SourceFile(string filename, string content)
    {
        Filename = filename;
        Content = content;
    }

    public string Filename { get; }

    public string Content { get; }
}