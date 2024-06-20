namespace HomeAssistantTypesSourceGenerator.HomeAssistant;

internal sealed class HassUnitOfMeasurementInfo
{
    public HassUnitOfMeasurementInfo(string hassUnitOfMeasurement)
    {
        HassUoM = hassUnitOfMeasurement;
        if (!string.IsNullOrEmpty(hassUnitOfMeasurement))
        {
            PropertyName = UoMSnakeCaseTransformer.ToUpperCamelCase(hassUnitOfMeasurement);
        }
    }

    /// <summary>
    ///     Home Assistant unit of measurement (UoM) string.
    /// </summary>
    public string HassUoM { get; }

    /// <summary>
    ///     The name of the unit of measurement property to be generated.
    /// </summary>
    public string PropertyName { get; } = "";
}