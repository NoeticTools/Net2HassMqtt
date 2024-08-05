namespace NoeticTools.Net2HassMqtt.Configuration.Building;

public partial class EventEntityBuilder<T>
{
    /// <summary>
    /// Causes the first event type to be published immediately after any other event is published.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     This is a workaround to prevent Home Assistant from resending last event on Home Assistant start-up or when device reconnects.
    /// </para>
    /// <para>
    ///     If used the first configured event type is used as an idle or clear state and may be best named something like "clear".
    ///     In this mode last event state will not be visible in Home Assistant's entity UI as the last event state will be momentary.
    /// </para>
    /// </remarks>
    public T WithFirstEventTypeSentAfterEachEvent()
    {
        EntityConfig.SendFirstEventTypeSentAfterEachEvent = true;
        return (this as T)!;
    }

    public T WithEventTypes<TE>()
        where TE : Enum
    {
        return WithEventTypes(Enum.GetNames(typeof(TE)));
    }
}