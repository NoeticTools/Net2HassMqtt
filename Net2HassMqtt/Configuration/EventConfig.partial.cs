using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoeticTools.Net2HassMqtt.Configuration
{
    public partial class EventConfig
    {
        /// <summary>
        ///     Optional event type to publish immediately after any other event type is published.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     This is a workaround to prevent Home Assistant from resending last event on Home Assistant start-up or when device reconnects.
        ///     It also allows for triggering on repeat events.
        /// </para>
        /// <para>
        ///     It is expected to typically be an event type name like "clear".
        /// </para>
        /// </remarks>
        public string EventTypeToSendAfterEachEvent { get; set; } = string.Empty;
    }
}
