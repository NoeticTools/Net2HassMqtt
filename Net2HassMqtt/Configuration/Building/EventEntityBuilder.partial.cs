namespace NoeticTools.Net2HassMqtt.Configuration.Building;

public partial class EventEntityBuilder<T>
{
    /// <summary>
    ///     Optional event type to publish immediately after any other event is published.
    /// </summary>
    /// <param name="eventTypeName"></param>
    /// <remarks>
    /// <para>
    ///     If set, the given event type is used as an idle or clear state and may be best named something like "clear".
    ///     In this mode last event state will not be visible in Home Assistant's entity UI as the last event state will be momentary.
    /// </para>
    /// <para>
    ///     If the event type is not included in the configured event types it is added to the event's possible event types.
    /// </para>
    /// <para>
    ///     This is a workaround to prevent Home Assistant from resending last event on Home Assistant start-up or when device reconnects.
    /// </para>
    /// </remarks>
    public T WithEventTypeToSendAfterEachEvent(string eventTypeName)
    {
        EntityConfig.EventTypeToSendAfterEachEvent = eventTypeName;
        return (this as T)!;
    }

    public T WithEventTypes<TE>()
        where TE : Enum
    {
        return WithEventTypes(Enum.GetNames(typeof(TE)));
    }
}