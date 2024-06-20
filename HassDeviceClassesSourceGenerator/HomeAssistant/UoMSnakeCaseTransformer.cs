using System.Collections.Generic;


namespace HomeAssistantTypesSourceGenerator.HomeAssistant;

internal static class UoMSnakeCaseTransformer
{
    private static readonly Dictionary<string, string> KnownUnitTransforms = new()
    {
        { "%", "Percent" },
        { "m\u00b2", "SquareMeters" },
        { "m³", "CubicMeters" },
        { "ft³", "CubicFeet" },
        { "m\u00b3/h", "CubicMetersPerHour" },
        { "ft\u00b3/min", "CubicFeePerHour" },
        { "fl. oz", "FluidOunces" },

        { "Lx", "Lux" },

        { "W/m²", "WattsPerSquareMeter" },
        { "BTU/(h⋅ft²)", "BtusPerHourPerSquareFoot" },

        { "CO (Gas CNG/LPG)", "CarbonMonoxide" },
        { "CO2 (Smoke)", "CarbonDioxide" },

        { "µg/m³", "MicrogramsPerCubicMeter" },

        // Velocity
        { "ft/s", "FeetPerSecond" },
        { "in/d", "InchesPerDay" },
        { "in/h", "InchesPerHour" },
        { "km/h", "KilometersPerHour" },
        { "kn", "Knots" },
        { "m/s", "MetersPerSecond" },
        { "mph", "MilesPerHour" },
        { "mm/d", "MillimetersPerDay" },
        { "mm/h", "MillimetersPerHour" },

        // Temperature
        { "°C", "DegreesCelsius" },
        { "°F", "DegreesFahrenheit" },
        { "K", "DegreesKelvin" },

        { "dB", "Decibel" },

        // Electrical
        { "A", "Ampere" },
        { "V", "Volts" },
        { "mV", "Millivolts" },

        // apparent_power
        { "VA", "VoltAmps" },

        // distance
        { "km", "Kilometers" },
        { "m", "Meters" },
        { "cm", "Centimeters" },
        { "mm", "Millimeters" },
        { "mi", "Miles" },
        { "yd", "Yards" },
        { "in", "Inches" },

        // duration
        { "d", "Days" },
        { "h", "Hours" },
        { "min", "Minutes" },
        { "s", "Seconds" },

        // power
        { "W", "Watts" },
        { "kW", "KiloWatts" },

        // weight
        { "kg", "Kilograms" },
        { "g", "Grams" },
        { "mq", "Milligrams" },
        { "ug", "Micrograms" },
        { "oz", "Ounce" },
        { "lb", "Pounds" },
        { "st", "Stones" },

        // data rates
        { "bit/s", "BitsPerSecond" },
        { "kbit/s", "KilobitsPerSecond" },
        { "Mbit/s", "MegabitsPerSecond" },
        { "Gbit/s", "GigabitsPerSecond" },
        { "B/s", "BytesPerSecond" },
        { "kB/s", "KilobytesPerSecond" },
        { "MB/s", "MegabytesPerSecond" },
        { "GB/s", "GigabytesPerSecond" },
        { "KiB/s", "KiBs" },
        { "MiB/s", "MiBs" },
        { "GiB/s", "GiBs" }
    };

    public static string ToUpperCamelCase(string hassUnitOfMeasurement)
    {
        if (KnownUnitTransforms.TryGetValue(hassUnitOfMeasurement, out var name))
        {
            return name;
        }

        return hassUnitOfMeasurement.Replace("/", "per").ToUpperCamelCase();
    }
}