namespace NoeticTools.Net2HassMqtt.Entities.Framework.StatusProperty;

internal interface IStatusPropertyReader
{
    /// <summary>
    ///     Can read entity value from the model.
    /// </summary>
    bool CanRead { get; }

    /// <summary>
    ///     Read value from the model's getter property.
    /// </summary>
    string Read();
}