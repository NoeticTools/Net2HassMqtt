using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Reflection;


namespace Net2HassMqtt.Tests.UnitTests.Framework;

internal class PropertyInfoStub(string name, Type propertyType, bool canRead) : PropertyInfo
{
    public object? StubedValue { get; set; }

    public override object[] GetCustomAttributes(bool inherit)
    {
        throw new NotImplementedException();
    }

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
        throw new NotImplementedException();
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
        throw new NotImplementedException();
    }

    public override Type? DeclaringType { get; }

    public override string Name { get; } = name;

    public override Type? ReflectedType { get; }

    public override MethodInfo[] GetAccessors(bool nonPublic)
    {
        throw new NotImplementedException();
    }

    public override MethodInfo? GetGetMethod(bool nonPublic)
    {
        throw new NotImplementedException();
    }

    public override ParameterInfo[] GetIndexParameters()
    {
        throw new NotImplementedException();
    }

    public override MethodInfo? GetSetMethod(bool nonPublic)
    {
        throw new NotImplementedException();
    }

    public override object? GetValue(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture)
    {
        return StubedValue;
    }

    public override void SetValue(object? obj, object? value, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture)
    {
        StubedValue = value;
    }

    public override PropertyAttributes Attributes { get; }

    public override bool CanRead { get; } = canRead;

    public override bool CanWrite { get; }

    public override Type PropertyType { get; } = propertyType;
}