namespace HomeAssistantTypesSourceGenerator.Framework;

internal interface IApiSourceFile
{
    string Content { get; }
    string Filename { get; }
}