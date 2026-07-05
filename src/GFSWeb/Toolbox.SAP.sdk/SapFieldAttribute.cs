namespace Toolbox.SAP.sdk;


/// <summary>
/// Specifies the SAP RFC field name to read when deserializing a row into this property.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class SapFieldAttribute : Attribute
{
    public string FieldName { get; }

    public SapFieldAttribute(string fieldName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fieldName);
        FieldName = fieldName;
    }
}
