using System;


namespace HomeAssistantTypesSourceGenerator;

/// <summary>
/// Code generator attribute to generate Home Asssitant domain, <a href="https://www.home-assistant.io/integrations/homeassistant/#device-class"> device class</a>,
/// and unit of measurment code.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class HassEntityDomainGeneratorAttribute : Attribute
{
    /// <summary>
    /// The Home Assistant (HASS) domain, device class, and units of measurement data for code generation.
    /// </summary>
    /// <remarks>
    /// Requires a constructor argument with JSON text defining Home Assistant (HASS) domain, device class, and units of measurement.
    /// </remarks>
    public HassEntityDomainGeneratorAttribute(string hassDefinitionText)
    {
    }
}
