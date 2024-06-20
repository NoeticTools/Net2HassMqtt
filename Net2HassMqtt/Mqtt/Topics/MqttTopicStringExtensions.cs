using System.Text;
using System.Text.RegularExpressions;


namespace NoeticTools.Net2HassMqtt.Mqtt.Topics;

public static class MqttTopicStringExtensions
{
    public static string ToMqttTopicSnakeCase(this string text)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        if (text.Length < 2)
        {
            return text;
        }

        text = text.ToTopicFormat();

        return text.ToSnakeCase();
    }

    private static string ToSnakeCase(this string text, char delimiter = '_')
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(char.ToLowerInvariant(text[0]));
        for (var index = 1; index < text.Length; ++index)
        {
            var character = text[index];
            if (char.IsUpper(character))
            {
                stringBuilder.Append(delimiter);
                stringBuilder.Append(char.ToLowerInvariant(character));
            }
            else
            {
                stringBuilder.Append(character);
            }
        }

        return stringBuilder.ToString().Replace("__", "_");
    }

    private static string ToTopicFormat(this string topic)
    {
        var regex = new Regex("[^a-zA-Z0-9_/+#]");
        return regex.Replace(topic, "_").ToLower();
    }
}