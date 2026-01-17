using System.Reflection;


namespace NoeticTools.Net2HassMqtt.Framework;

internal interface IPropertyInfoReader
{
    PropertyInfo? GetPropertyGetterInfo(object model, string? statusPropertyName);
}