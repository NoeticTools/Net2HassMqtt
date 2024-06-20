using System.Collections.Generic;
using System.Linq;
using HomeAssistantTypesSourceGenerator.HomeAssistant;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace HomeAssistantTypesSourceGenerator.Framework;

internal abstract class SourceGeneratorBase
{
    protected static string? ExtractName(NameSyntax? name)
    {
        return name switch
        {
            SimpleNameSyntax ins => ins.Identifier.Text,
            QualifiedNameSyntax qns => qns.Right.Identifier.Text,
            _ => null
        };
    }

    protected static string GetNamespaceText(INamespaceSymbol namespaceSymbol)
    {
        var elements = new List<string>();

        while (!string.IsNullOrEmpty(namespaceSymbol.Name))
        {
            elements.Add(namespaceSymbol.Name);
            namespaceSymbol = namespaceSymbol.ContainingNamespace;
        }

        elements.Reverse();
        return string.Join(".", elements);
    }

    protected static bool HasAttribute(MemberDeclarationSyntax classDeclaration, string attributeName)
    {
        var matchPrefix = attributeName + "<";
        return classDeclaration.AttributeLists
                               .Any(attributeList => attributeList.Attributes.Any(attribute =>
                                                                                      attribute.Name
                                                                                               .ToString()
                                                                                               .EnsureEndsWith("Attribute")
                                                                                               .StartsWith(matchPrefix)));
    }

    // Extension method?
    protected static bool InheritsFrom(INamedTypeSymbol symbol, string baseTypeName)
    {
        var baseSymbol = symbol.BaseType;
        if (baseSymbol == null)
        {
            return false;
        }

        return baseSymbol.Name == baseTypeName || InheritsFrom(baseSymbol, baseTypeName);
    }

    // Extension method?
    protected static bool IsAbstract(MemberDeclarationSyntax classSyntaxNode)
    {
        return classSyntaxNode.Modifiers.Any(m => m.IsKind(SyntaxKind.AbstractKeyword));
    }

    protected static bool IsPartial(ClassDeclarationSyntax classDeclaration)
    {
        return classDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));
    }
}