using System.Globalization;
using System.Reflection;


namespace Net2HassMqtt.Tests.UnitTests.Framework;

internal class PropertyInfoStub(string name, Type propertyType, bool canRead) : PropertyInfo
{
    private Type _propertyType = propertyType;

    public override PropertyAttributes Attributes { get; }

    public override bool CanRead { get; } = canRead;

    public override bool CanWrite { get; }

    public override Type? DeclaringType { get; }

    public override string Name { get; } = name;

    public override Type PropertyType => _propertyType;

    public void SetPropertyType(Type type)
    {
        _propertyType = type;
    }

    public override Type? ReflectedType { get; }

    public object? StubbedValue { get; set; }

    public override MethodInfo[] GetAccessors(bool nonPublic)
    {
        throw new NotImplementedException();
    }

    public override object[] GetCustomAttributes(bool inherit)
    {
        throw new NotImplementedException();
    }

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
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
        return StubbedValue;
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
        throw new NotImplementedException();
    }

    public override void SetValue(object? obj, object? value, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture)
    {
        StubbedValue = value;
    }
}