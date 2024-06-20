// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

using System;
using System.Globalization;
using System.IO;
using System.Text;


namespace LightJson.Serialization;

using ErrorType = JsonParseException.ErrorType;


/// <summary>
///     Represents a reader that can read JsonValues.
/// </summary>
internal sealed class JsonReader
{
    private readonly TextScanner scanner;

    private JsonReader(TextReader reader)
    {
        scanner = new TextScanner(reader);
    }

    /// <summary>
    ///     Creates a JsonValue by using the given TextReader.
    /// </summary>
    /// <param name="reader">The TextReader used to read a JSON message.</param>
    /// <returns>The parsed <see cref="JsonValue" />.</returns>
    public static JsonValue Parse(TextReader reader)
    {
        if (reader == null)
        {
            throw new ArgumentNullException(nameof(reader));
        }

        return new JsonReader(reader).Parse();
    }

    /// <summary>
    ///     Creates a JsonValue by reader the JSON message in the given string.
    /// </summary>
    /// <param name="source">The string containing the JSON message.</param>
    /// <returns>The parsed <see cref="JsonValue" />.</returns>
    public static JsonValue Parse(string source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        using (var reader = new StringReader(source))
        {
            return Parse(reader);
        }
    }

    private string ReadJsonKey()
    {
        return ReadString();
    }

    private JsonValue ReadJsonValue()
    {
        scanner.SkipWhitespace();

        var next = scanner.Peek();

        if (char.IsNumber(next))
        {
            return ReadNumber();
        }

        switch (next)
        {
            case '{':
                return ReadObject();

            case '[':
                return ReadArray();

            case '"':
                return ReadString();

            case '-':
                return ReadNumber();

            case 't':
            case 'f':
                return ReadBoolean();

            case 'n':
                return ReadNull();

            default:
                throw new JsonParseException(
                                             ErrorType.InvalidOrUnexpectedCharacter,
                                             scanner.Position);
        }
    }

    private JsonValue ReadNull()
    {
        scanner.Assert("null");
        return JsonValue.Null;
    }

    private JsonValue ReadBoolean()
    {
        switch (scanner.Peek())
        {
            case 't':
                scanner.Assert("true");
                return true;

            default:
                scanner.Assert("false");
                return false;
        }
    }

    private void ReadDigits(StringBuilder builder)
    {
        while (true)
        {
            var next = scanner.Peek(false);
            if (next == -1 || !char.IsNumber((char)next))
            {
                return;
            }

            builder.Append(scanner.Read());
        }
    }

    private JsonValue ReadNumber()
    {
        var builder = new StringBuilder();

        if (scanner.Peek() == '-')
        {
            builder.Append(scanner.Read());
        }

        if (scanner.Peek() == '0')
        {
            builder.Append(scanner.Read());
        }
        else
        {
            ReadDigits(builder);
        }

        if (scanner.Peek(false) == '.')
        {
            builder.Append(scanner.Read());
            ReadDigits(builder);
        }

        if (scanner.Peek(false) == 'e' || scanner.Peek(false) == 'E')
        {
            builder.Append(scanner.Read());

            var next = scanner.Peek();

            switch (next)
            {
                case '+':
                case '-':
                    builder.Append(scanner.Read());
                    break;
            }

            ReadDigits(builder);
        }

        return double.Parse(
                            builder.ToString(),
                            CultureInfo.InvariantCulture);
    }

    private string ReadString()
    {
        var builder = new StringBuilder();

        scanner.Assert('"');

        while (true)
        {
            var errorPosition = scanner.Position;
            var c = scanner.Read();

            if (c == '\\')
            {
                errorPosition = scanner.Position;
                c = scanner.Read();

                switch (char.ToLower(c))
                {
                    case '"':
                    case '\\':
                    case '/':
                        builder.Append(c);
                        break;
                    case 'b':
                        builder.Append('\b');
                        break;
                    case 'f':
                        builder.Append('\f');
                        break;
                    case 'n':
                        builder.Append('\n');
                        break;
                    case 'r':
                        builder.Append('\r');
                        break;
                    case 't':
                        builder.Append('\t');
                        break;
                    case 'u':
                        builder.Append(ReadUnicodeLiteral());
                        break;
                    default:
                        throw new JsonParseException(
                                                     ErrorType.InvalidOrUnexpectedCharacter,
                                                     errorPosition);
                }
            }
            else if (c == '"')
            {
                break;
            }
            else
            {
                if (char.IsControl(c))
                {
                    throw new JsonParseException(
                                                 ErrorType.InvalidOrUnexpectedCharacter,
                                                 errorPosition);
                }

                builder.Append(c);
            }
        }

        return builder.ToString();
    }

    private int ReadHexDigit()
    {
        var errorPosition = scanner.Position;
        switch (char.ToUpper(scanner.Read()))
        {
            case '0':
                return 0;

            case '1':
                return 1;

            case '2':
                return 2;

            case '3':
                return 3;

            case '4':
                return 4;

            case '5':
                return 5;

            case '6':
                return 6;

            case '7':
                return 7;

            case '8':
                return 8;

            case '9':
                return 9;

            case 'A':
                return 10;

            case 'B':
                return 11;

            case 'C':
                return 12;

            case 'D':
                return 13;

            case 'E':
                return 14;

            case 'F':
                return 15;

            default:
                throw new JsonParseException(
                                             ErrorType.InvalidOrUnexpectedCharacter,
                                             errorPosition);
        }
    }

    private char ReadUnicodeLiteral()
    {
        var value = 0;

        value += ReadHexDigit() * 4096; // 16^3
        value += ReadHexDigit() * 256; // 16^2
        value += ReadHexDigit() * 16; // 16^1
        value += ReadHexDigit(); // 16^0

        return (char)value;
    }

    private JsonObject ReadObject()
    {
        return ReadObject(new JsonObject());
    }

    private JsonObject ReadObject(JsonObject jsonObject)
    {
        scanner.Assert('{');

        scanner.SkipWhitespace();

        if (scanner.Peek() == '}')
        {
            scanner.Read();
        }
        else
        {
            while (true)
            {
                scanner.SkipWhitespace();

                var errorPosition = scanner.Position;
                var key = ReadJsonKey();

                if (jsonObject.ContainsKey(key))
                {
                    throw new JsonParseException(
                                                 ErrorType.DuplicateObjectKeys,
                                                 errorPosition);
                }

                scanner.SkipWhitespace();

                scanner.Assert(':');

                scanner.SkipWhitespace();

                var value = ReadJsonValue();

                jsonObject.Add(key, value);

                scanner.SkipWhitespace();

                errorPosition = scanner.Position;
                var next = scanner.Read();
                if (next == ',')
                {
                    // Allow trailing commas in objects
                    scanner.SkipWhitespace();
                    if (scanner.Peek() == '}')
                    {
                        next = scanner.Read();
                    }
                }

                if (next == '}')
                {
                    break;
                }

                if (next == ',')
                {
                    continue;
                }

                throw new JsonParseException(
                                             ErrorType.InvalidOrUnexpectedCharacter,
                                             errorPosition);
            }
        }

        return jsonObject;
    }

    private JsonArray ReadArray()
    {
        return ReadArray(new JsonArray());
    }

    private JsonArray ReadArray(JsonArray jsonArray)
    {
        scanner.Assert('[');

        scanner.SkipWhitespace();

        if (scanner.Peek() == ']')
        {
            scanner.Read();
        }
        else
        {
            while (true)
            {
                scanner.SkipWhitespace();

                var value = ReadJsonValue();

                jsonArray.Add(value);

                scanner.SkipWhitespace();

                var errorPosition = scanner.Position;
                var next = scanner.Read();
                if (next == ',')
                {
                    // Allow trailing commas in arrays
                    scanner.SkipWhitespace();
                    if (scanner.Peek() == ']')
                    {
                        next = scanner.Read();
                    }
                }

                if (next == ']')
                {
                    break;
                }

                if (next == ',')
                {
                    continue;
                }

                throw new JsonParseException(
                                             ErrorType.InvalidOrUnexpectedCharacter,
                                             errorPosition);
            }
        }

        return jsonArray;
    }

    private JsonValue Parse()
    {
        scanner.SkipWhitespace();
        return ReadJsonValue();
    }
}