using System;
using System.IO;
using System.Text;
using HomeAssistantTypesSourceGenerator.Resources;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Scriban;


namespace HomeAssistantTypesSourceGenerator.Framework;

internal sealed class FileSourceGeneratorContext
{
    private readonly SourceProductionContext _context;

    public FileSourceGeneratorContext(SourceProductionContext context)
    {
        _context = context;
    }

    public void AddSource(string filename, string content)
    {
        var sourceFile = new SourceFile(filename, content);
        _context.AddSource(sourceFile.Filename, SourceText.From(sourceFile.Content, Encoding.UTF8));
    }

    public static string? GetResourceFileContent(string resourceFilename)
    {
        var resourcePath = $"{typeof(ResourcesNamespaceToken).Namespace}.{resourceFilename}";
        var resourceStream = typeof(FileSourceGeneratorContext).Assembly.GetManifestResourceStream(resourcePath);
        if (resourceStream == null)
        {
            Console.Error.WriteLine("Unable to find resource file {0}.", resourcePath);
            return null;
        }

        using (resourceStream)
        using (var reader = new StreamReader(resourceStream))
        {
            return reader.ReadToEnd();
        }
    }

    public void RenderAndAddToSource(object model, string filename, string contentTemplate)
    {
        var message = $"Generating: {filename}";
        Console.WriteLine(message);
        _context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.InfoMessage, null, message));

        var template = Template.Parse(contentTemplate);
        if (template.HasErrors)
        {
            foreach (var error in template.Messages)
            {
                Console.Error.WriteLine("Error generating {0}: {1}", filename, error);
                _context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.ScribanParsingError, null, filename, error));
            }

            return;
        }

        var content = template.Render(model, member => member.Name);
        AddSource(filename, content);
    }
}