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
        ///     If true the first event type to be published immediately after any other event is published.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     This is a workaround to prevent Home Assistant from resending last event on Home Assistant start-up or when device reconnects.
        /// </para>
        /// <para>
        ///     If true the first configured event type is used as an idle or clear state and may be best named something like "clear".
        ///     In this mode last event state will not be visible in Home Assistant's entity UI as the last event state will be momentary.
        /// </para>
        /// </remarks>
        public bool SendFirstEventTypeSentAfterEachEvent { get; set; }
    }
}
