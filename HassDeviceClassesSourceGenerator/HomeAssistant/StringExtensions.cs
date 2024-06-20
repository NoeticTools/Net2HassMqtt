using System;
using System.Collections.Generic;


namespace HomeAssistantTypesSourceGenerator.HomeAssistant;

internal static class StringExtensions
{
    public static string EnsureEndsWith(this string source, string suffix)
    {
        if (source.EndsWith(suffix))
        {
            return source;
        }

        return source + suffix;
    }

    public static string FirstCharToUpper(this string input)
    {
        return input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => input[0].ToString().ToUpper() + input.Substring(1)
        };
    }

    public static string ToUpperCamelCase(this string input)
    {
        var words = input.Split([' ', '_', '-']);
        var titleCasedWords = new List<string>();
        foreach (var word in words)
        {
            titleCasedWords.Add(word.FirstCharToUpper());
        }

        return string.Join("", titleCasedWords);
    }
}