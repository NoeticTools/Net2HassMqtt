using Microsoft.CodeAnalysis;


namespace HomeAssistantTypesSourceGenerator;

public static class DiagnosticDescriptors
{
    public static readonly DiagnosticDescriptor InfoMessage = new("HTSGEN000",
                                                                  "Source generator",
                                                                  "{0}",
                                                                  "HomeAssistantTypesSourceGenerator",
                                                                  DiagnosticSeverity.Info,
                                                                  true);

    public static readonly DiagnosticDescriptor ExceptionThrownError = new("HTSGEN001",
                                                                           "Exception thrown",
                                                                           "Exception: {0}",
                                                                           "HomeAssistantTypesSourceGenerator",
                                                                           DiagnosticSeverity.Error,
                                                                           true);

    public static readonly DiagnosticDescriptor ScribanParsingError = new("HTSGEN002",
                                                                          "Scriban error",
                                                                          "Error generating {0}: {1}",
                                                                          "HomeAssistantTypesSourceGenerator",
                                                                          DiagnosticSeverity.Error,
                                                                          true);

    public static readonly DiagnosticDescriptor ConfigurationError = new("HTSGEN003",
                                                                         "Configuration error",
                                                                         "{0}",
                                                                         "HomeAssistantTypesSourceGenerator",
                                                                         DiagnosticSeverity.Error,
                                                                         true);
}