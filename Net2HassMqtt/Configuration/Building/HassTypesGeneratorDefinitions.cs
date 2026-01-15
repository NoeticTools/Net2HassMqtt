using HomeAssistantTypesSourceGenerator;


namespace NoeticTools.Net2HassMqtt.Configuration.Building;

//// todo:
//    // button|None: Generic button.This is the default and does not need to be set.
//    // button|identify: The button is used to identify a device.
//    // button|restart: The button restarts the device.
//    // button|update: The button updates the software of the device.
//    // event|None: Generic event. This is the default and does not need to be set.
//    // event|button: For remote control buttons.
//    // event|doorbell: Specifically for buttons that are used as a doorbell.
//    // event|motion: For motion events detected by a motion sensor.

[HassEntityDomainGeneratorAttribute(
                                       """
                                       {
                                         "domains": [
                                           {
                                             "name": "binary_sensor",
                                             "read_only": true,
                                             "requires_command_handler": false,
                                             "additional_options": [],
                                             "domain_classes": [
                                               {
                                                 "name": "None",
                                                 "description": "Generic on/off. This is the default and does not need to be set.",
                                                 "units": []
                                               },
                                               {
                                                 "name": "battery",
                                                 "description": "on means low, off means normal",
                                                 "units": []
                                               },
                                               {
                                                 "name": "battery_charging",
                                                 "description": "on means charging, off means not charging",
                                                 "units": []
                                               },
                                               {
                                                 "name": "carbon_monoxide",
                                                 "description": "on means carbon monoxide detected, off no carbon monoxide (clear)",
                                                 "units": []
                                               },
                                               {
                                                 "name": "cold",
                                                 "description": "on means cold, off means normal",
                                                 "units": []
                                               },
                                               {
                                                 "name": "connectivity",
                                                 "description": "on means connected, off means disconnected",
                                                 "units": []
                                               },
                                               {
                                                 "name": "door",
                                                 "description": "on means open, off means closed",
                                                 "units": []
                                               },
                                               {
                                                 "name": "garage_door",
                                                 "description": "on means open, off means closed",
                                                 "units": []
                                               },
                                               {
                                                 "name": "gas",
                                                 "description": "on means gas detected, off means no gas (clear)",
                                                 "units": []
                                               },
                                               {
                                                 "name": "heat",
                                                 "description": "on means hot, off means normal",
                                                 "units": []
                                               },
                                               {
                                                 "name": "light",
                                                 "description": "on means light detected, off means no light",
                                                 "units": []
                                               },
                                               {
                                                 "name": "lock",
                                                 "description": "on means open (unlocked), off means closed (locked)",
                                                 "units": []
                                               },
                                               {
                                                 "name": "moisture",
                                                 "description": "on means moisture detected (wet), off means no moisture (dry)",
                                                 "units": []
                                               },
                                               {
                                                 "name": "motion",
                                                 "description": "on means motion detected, off means no motion (clear)",
                                                 "units": []
                                               },
                                               {
                                                 "name": "moving",
                                                 "description": "on means moving, off means not moving (stopped)",
                                                 "units": []
                                               },
                                               {
                                                 "name": "occupancy",
                                                 "description": "on means occupied (detected), off means not occupied (clear)",
                                                 "units": []
                                               },
                                               {
                                                 "name": "opening",
                                                 "description": "on means open, off means closed",
                                                 "units": []
                                               },
                                               {
                                                 "name": "plug",
                                                 "description": "on means device is plugged in, off means device is unplugged",
                                                 "units": []
                                               },
                                               {
                                                 "name": "power",
                                                 "description": "on means power detected, off means no power",
                                                 "units": []
                                               },
                                               {
                                                 "name": "presence",
                                                 "description": "on means home, off means away",
                                                 "units": []
                                               },
                                               {
                                                 "name": "problem",
                                                 "description": "on means problem detected, off means no problem (OK)",
                                                 "units": []
                                               },
                                               {
                                                 "name": "running",
                                                 "description": "on means running, off means not running",
                                                 "units": []
                                               },
                                               {
                                                 "name": "safety",
                                                 "description": "on means unsafe, off means safe",
                                                 "units": []
                                               },
                                               {
                                                 "name": "smoke",
                                                 "description": "on means smoke detected, off means no smoke (clear)",
                                                 "units": []
                                               },
                                               {
                                                 "name": "sound",
                                                 "description": "on means sound detected, off means no sound (clear)",
                                                 "units": []
                                               },
                                               {
                                                 "name": "tamper",
                                                 "description": "on means tampering detected, off means no tampering (clear)",
                                                 "units": []
                                               },
                                               {
                                                 "name": "update",
                                                 "description": "on means update available, off means up-to-date",
                                                 "units": []
                                               },
                                               {
                                                 "name": "vibration",
                                                 "description": "on means vibration detected, off means no vibration (clear)",
                                                 "units": []
                                               },
                                               {
                                                 "name": "window",
                                                 "description": "on means open, off means closed",
                                                 "units": []
                                               }
                                             ]
                                           },
                                           {
                                             "name": "button",
                                             "read_only": false,
                                             "requires_command_handler": true,
                                             "additional_options": [],
                                             "domain_classes": [
                                               {
                                                 "name": "None",
                                                 "description": "Generic button. This is the default and does not need to be set.",
                                                 "units": []
                                               },
                                               {
                                                 "name": "identify",
                                                 "description": "The button is used to identify a device.",
                                                 "units": []
                                               },
                                               {
                                                 "name": "restart",
                                                 "description": "The button restarts the device.",
                                                 "units": []
                                               },
                                               {
                                                 "name": "update",
                                                 "description": "The button updates the software of the device.",
                                                 "units": []
                                               },
                                            ]
                                           },
                                           {
                                             "name": "event",
                                             "read_only": true,
                                             "requires_command_handler": false,
                                             "additional_options": [
                                               {
                                                 "name": "EventTypes",
                                                 "mqtt_name": "event_types",
                                                 "description": "A list of valid event_type strings.",
                                                 "type": "string[]",
                                                 "default": "[]",
                                                 "is_optional": false
                                               }
                                             ],
                                             "no_retain_option": true,
                                             "domain_classes": [
                                               {
                                                 "name": "None",
                                                 "description": "Generic button. This is the default and does not need to be set.",
                                                 "units": []
                                               },
                                               {
                                                 "name": "button",
                                                 "description": "For remote control buttons.",
                                                 "units": []
                                               },
                                               {
                                                 "name": "doorbell",
                                                 "description": "Specifically for buttons that are used as a doorbell.",
                                                 "units": []
                                               },
                                               {
                                                 "name": "motion",
                                                 "description": "For motion events detected by a motion sensor.",
                                                 "units": []
                                               },
                                            ]
                                           },
                                           {
                                             "name": "cover",
                                             "read_only": false,
                                             "requires_command_handler": false,
                                             "additional_options": [],
                                             "domain_classes": [
                                               {
                                       
                                                 "name": "None",
                                                 "description": "Generic cover. This is the default and does not need to be set.",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "awning",
                                                 "description": "Control of an awning, such as an exterior retractable window, door, or patio cover.",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "blind",
                                                 "description": "Control of blinds, which are linked slats that expand or collapse to cover an opening or may be tilted to partially covering an opening, such as window blinds.",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "curtain",
                                                 "description": "Control of curtains or drapes, which is often fabric hung above a window or door that can be drawn open.",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "damper",
                                                 "description": "Control of a mechanical damper that reduces airflow, sound, or light.",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "door",
                                                 "description": "Control of a door or gate that provides access to an area.",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "garage",
                                                 "description": "Control of a garage door that provides access to a garage.",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "gate",
                                                 "description": "Control of a gate. Gates are found outside of a structure and are typically part of a fence.",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "shade",
                                                 "description": "Control of shades, which are a continuous plane of material or connected cells that expanded or collapsed over an opening, such as window shades.",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "shutter",
                                                 "description": "Control of shutters, which are linked slats that swing out/in to covering an opening or may be tilted to partially cover an opening, such as indoor or exterior window shutters.",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "window",
                                                 "description": "Control of a physical window that opens and closes or may tilt.",
                                                 "units": []
                                               }
                                             ]
                                           },
                                           {
                                             "name": "humidifier",
                                             "read_only": false,
                                             "requires_command_handler": false,
                                             "additional_options": [],
                                             "domain_classes": [
                                               {
                                                 "name": "Humidifier",
                                                 "description": "Adds humidity to the air around it.",
                                                 "units": []
                                               },
                                               {
                                                 "name": "Dehumidifier",
                                                 "description": "Removes humidity from the air around it.",
                                                 "units": []
                                               }
                                             ]
                                           },
                                           {
                                             "name": "number",
                                             "read_only": false,
                                             "requires_command_handler": true,
                                             "additional_options": [
                                                {
                                                    "name": "Minimum",
                                                    "mqtt_name": "min",
                                                    "description": "Minimum value.",
                                                    "type": "double",
                                                    "default": "1",
                                                    "is_optional": true
                                                },
                                                {
                                                    "name": "Maximum",
                                                    "mqtt_name": "max",
                                                    "description": "Maximum value.",
                                                    "type": "double",
                                                    "default": "100",
                                                    "is_optional": true
                                                },
                                                {
                                                    "name": "Step",
                                                    "mqtt_name": "step",
                                                    "description": "Step value. Smallest value 0.001.",
                                                    "type": "double",
                                                    "default": "1",
                                                    "is_optional": true
                                                },
                                                {
                                                    "name": "DisplayMode",
                                                    "mqtt_name": "mode",
                                                    "description": "Control how the number should be displayed in the UI. Can be set to `box` or `slider` to force a display mode.",
                                                    "type": "string",
                                                    "default": "auto",
                                                    "is_optional": true
                                                }
                                             ],
                                             "domain_classes": [
                                               {
                                       
                                                 "name": "None",
                                                 "description": "Generic number. This is the default and does not need to be set.",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "apparent_power",
                                                 "description": "Apparent power|",
                                                 "units": [ "VA." ]
                                               },
                                               {
                                       
                                                 "name": "aqi",
                                                 "description": "Air Quality Index (unitless).",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "atmospheric_pressure",
                                                 "description": "Atmospheric pressure|",
                                                 "units": [ "cbar", "bar", "hPa", "inHg", "kPa", "mbar", "Pa", "psi" ]
                                               },
                                               {
                                       
                                                 "name": "battery",
                                                 "description": "Percentage of battery that is left",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "carbon_dioxide",
                                                 "description": "Carbon Dioxide CO2 (Smoke)",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "carbon_monoxide",
                                                 "description": "Carbon Monoxide CO (Gas CNG/LPG)",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "current",
                                                 "description": "Current|",
                                                 "units": [ "A", "mA" ]
                                               },
                                               {
                                       
                                                 "name": "data_rate",
                                                 "description": "Data rate|",
                                                 "units": [ "bit/s", "kbit/s", "Mbit/s", "Gbit/s", "B/s", "kB/s", "MB/s", "GB/s", "KiB/s", "MiB/s", "GiB/s" ]
                                       
                                               },
                                               {
                                       
                                                 "name": "data_size",
                                                 "description": "Data size|",
                                                 "units": [ "bit", "kbit", "Mbit", "Gbit", "B", "kB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB" ]
                                       
                                               },
                                               {
                                       
                                                 "name": "distance",
                                                 "description": "Generic distance|",
                                                 "units": [ "km", "m", "cm", "mm", "mi", "yd", "in" ]
                                       
                                               },
                                               {
                                       
                                                 "name": "energy",
                                                 "description": "Energy|",
                                                 "units": [ "Wh", "kWh", "MWh", "MJ", "GJ" ]
                                       
                                               },
                                               {
                                       
                                                 "name": "energy_storage",
                                                 "description": "Stored energy|",
                                                 "units": [ "Wh", "kWh", "MWh", "MJ", "GJ" ]
                                       
                                               },
                                               {
                                       
                                                 "name": "frequency",
                                                 "description": "Frequency|",
                                                 "units": [ "Hz", "kHz", "MHz", "GHz" ]
                                       
                                               },
                                               {
                                       
                                                 "name": "gas",
                                                 "description": "Gas volume|",
                                                 "units": [ "m³", "ft³", "CCF" ]
                                       
                                               },
                                               {
                                       
                                                 "name": "humidity",
                                                 "description": "Percentage of humidity in the air",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "illuminance",
                                                 "description": "The current light level|",
                                                 "units": [ "lx" ]
                                       
                                               },
                                               {
                                       
                                                 "name": "irradiance",
                                                 "description": "Irradiance|",
                                                 "units": [ "W/m²", "BTU/(h⋅ft²)" ]
                                       
                                               },
                                               {
                                       
                                                 "name": "moisture",
                                                 "description": "Percentage of water in a substance",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "monetary",
                                                 "description": "The monetary value",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "nitrogen_dioxide",
                                                 "description": "Concentration of Nitrogen Dioxide|",
                                                 "units": [ "µg/m³" ]
                                               },
                                               {
                                       
                                                 "name": "nitrogen_monoxide",
                                                 "description": "Concentration of Nitrogen Monoxide|",
                                                 "units": [ "µg/m³" ]
                                               },
                                               {
                                       
                                                 "name": "nitrous_oxide",
                                                 "description": "Concentration of Nitrous Oxide|",
                                                 "units": [ "µg/m³" ]
                                               },
                                               {
                                       
                                                 "name": "ozone",
                                                 "description": "Concentration of Ozone|",
                                                 "units": [ "µg/m³" ]
                                               },
                                               {
                                       
                                                 "name": "ph",
                                                 "description": "Potential hydrogen (pH) value of a water solution",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "pm1",
                                                 "description": "Concentration of particulate matter less than 1 micrometer|",
                                                 "units": [ "µg/m³" ]
                                               },
                                               {
                                       
                                                 "name": "pm10",
                                                 "description": "Concentration of particulate matter less than 10 micrometers|",
                                                 "units": [ "µg/m³" ]
                                               },
                                               {
                                       
                                                 "name": "pm25",
                                                 "description": "Concentration of particulate matter less than 2.5 micrometers|",
                                                 "units": [ "µg/m³" ]
                                               },
                                               {
                                       
                                                 "name": "power_factor",
                                                 "description": "Power factor(unitless), unit may be None or %",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "power",
                                                 "description": "Power|",
                                                 "units": [ "W", "kW" ]
                                               },
                                               {
                                       
                                                 "name": "precipitation",
                                                 "description": "Accumulated precipitation|",
                                                 "units": [ "cm", "in", "mm" ]
                                               },
                                               {
                                       
                                                 "name": "precipitation_intensity",
                                                 "description": "Precipitation intensity|",
                                                 "units": [ "in/d", "in/h", "mm/d", "mm/h" ]
                                               },
                                               {
                                       
                                                 "name": "pressure",
                                                 "description": "Pressure|",
                                                 "units": [ "Pa", "kPa", "hPa", "bar", "cbar", "mbar", "mmHg", "inHg", "psi" ]
                                               },
                                               {
                                       
                                                 "name": "reactive_power",
                                                 "description": "Reactive power|",
                                                 "units": [ "var" ]
                                               },
                                               {
                                       
                                                 "name": "signal_strength",
                                                 "description": "Signal strength|",
                                                 "units": [ "dB", "dBm" ]
                                               },
                                               {
                                       
                                                 "name": "sound_pressure",
                                                 "description": "Sound pressure|",
                                                 "units": [ "dB", "dBA" ]
                                               },
                                               {
                                       
                                                 "name": "speed",
                                                 "description": "Generic speed|",
                                                 "units": [ "ft/s", "in/d", "in/h", "km/h", "kn", "m/s", "mph", "mm/d" ]
                                               },
                                               {
                                       
                                                 "name": "sulphur_dioxide",
                                                 "description": "Concentration of sulphur dioxide|",
                                                 "units": [ "µg/m³" ]
                                               },
                                               {
                                       
                                                 "name": "temperature",
                                                 "description": "Temperature|",
                                                 "units": [ "°C", "°F", "K" ]
                                               },
                                               {
                                       
                                                 "name": "volatile_organic_compounds",
                                                 "description": "Concentration of volatile organic compounds|",
                                                 "units": [ "µg/m³" ]
                                               },
                                               {
                                       
                                                 "name": "voltage",
                                                 "description": "Voltage|",
                                                 "units": [ "V", "mV" ]
                                               },
                                               {
                                       
                                                 "name": "volume",
                                                 "description": "Generic volume|",
                                                 "units": [ "L", "mL", "gal", "fl. oz.", "m³", "ft³", "CCF" ]
                                               },
                                               {
                                       
                                                 "name": "volume_flow_rate",
                                                 "description": "Volume flow rate|",
                                                 "units": [ "m³/h", "ft³/min", "L/min", "gal/min" ]
                                               },
                                               {
                                       
                                                 "name": "volume_storage",
                                                 "description": "Generic stored volume|",
                                                 "units": [ "L", "mL", "gal", "fl. oz.", "m³", "ft³", "CCF" ]
                                       
                                               },
                                               {
                                       
                                                 "name": "water",
                                                 "description": "Water consumption|",
                                                 "units": [ "L", "gal", "m³", "ft³", "CCF" ]
                                       
                                               },
                                               {
                                       
                                                 "name": "weight",
                                                 "description": "Generic mass|",
                                                 "units": [ "kg", "g", "mg", "µg", "oz", "lb", "st" ]
                                       
                                               },
                                               {
                                       
                                                 "name": "wind_speed",
                                                 "description": "Wind speed|",
                                                 "units": [ "ft/s", "km/h", "kn", "m/s", "mph" ]
                                       
                                               }
                                             ]
                                           },
                                           {
                                             "name": "sensor",
                                             "read_only": true,
                                             "requires_command_handler": false,
                                             "additional_options": [
                                               {
                                                  "name": "ExpiresAfter",
                                                  "mqtt_name": "expire_after",
                                                  "description": "If set, it defines the number of seconds after the sensor’s state expires, if it’s not updated. After expiry, the sensor’s state becomes `unavailable`. Default the sensors state never expires.",
                                                  "type": "int",
                                                  "default": "0",
                                                  "is_optional": true
                                                },
                                                {
                                                   "name": "SuggestedDisplayPrecision",
                                                   "mqtt_name": "suggested_display_precision",
                                                   "description": "The number of decimals which should be used in the sensor’s state after rounding.",
                                                   "type": "int",
                                                   "default": "\"None\"",
                                                   "is_optional": true
                                                 },
                                                 {
                                                   "name": "Options",
                                                   "mqtt_name": "options",
                                                   "description": "In case this sensor provides a textual state, this property can be used to provide a list of possible states. Requires the enum device class to be set. Cannot be combined with state_class or native_unit_of_measurement.",
                                                   "type": "string",
                                                   "default": "\"None\"",
                                                   "is_optional": false
                                                 },
                                                 {
                                                   "name": "StateClass",
                                                   "mqtt_name": "state_class",
                                                   "description": "The <a href=\"https://developers.home-assistant.io/docs/core/entity/sensor#available-state-classes\">state_class</a> of the sensor.",
                                                   "type": "string",
                                                   "default": "",
                                                   "is_optional": true
                                                  }
                                              ],
                                             "domain_classes": [
                                               {
                                                 "name": "None",
                                                 "description": "Generic sensor. This is the default.",
                                                 "units": []
                                               },
                                               {
                                                 "name": "apparent_power",
                                                 "description": "Apparent power|",
                                                 "units": [ "VA." ]
                                               },
                                               {
                                       
                                                 "name": "aqi",
                                                 "description": "Air Quality Index (unitless).",
                                                 "units": []
                                               },
                                               {
                                                 "name": "atmospheric_pressure",
                                                 "description": "Atmospheric pressure|",
                                                 "units": [ "cbar", "bar", "hPa", "mmHg", "inHg", "kPa", "mbar", "Pa or psi" ]
                                               },
                                               {
                                                 "name": "battery",
                                                 "description": "Percentage of battery that is left|",
                                                 "units": [ "%" ]
                                               },
                                               {
                                                 "name": "carbon_dioxide",
                                                 "description": "Carbon Dioxide in CO2 (Smoke)|",
                                                 "units": [ "ppm" ]
                                               },
                                               {
                                                 "name": "carbon_monoxide",
                                                 "description": "Carbon Monoxide in CO (Gas CNG/LPG)|",
                                                 "units": [ "ppm" ]
                                               },
                                               {
                                                 "name": "current",
                                                 "description": "Current|",
                                                 "units": [ "A", "mA" ]
                                               },
                                               {
                                                 "name": "data_rate",
                                                 "description": "Data rate|",
                                                 "units": [ "bit/s", "kbit/s", "Mbit/s", "Gbit/s", "B/s", "kB/s", "MB/s", "GB/s", "KiB/s", "MiB/s or GiB/s" ]
                                               },
                                               {
                                                 "name": "data_size",
                                                 "description": "Data size|",
                                                 "units": [ "bit", "kbit", "Mbit", "Gbit", "B", "kB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB or YiB" ]
                                               },
                                               {
                                                 "name": "date",
                                                 "description": "Date string (ISO 8601)",
                                                 "units": []
                                               },
                                               {
                                                 "name": "distance",
                                                 "description": "Generic distance|",
                                                 "units": [ "km", "m", "cm", "mm", "mi", "yd", "in" ]
                                               },
                                               {
                                                 "name": "duration",
                                                 "description": "Duration|",
                                                 "units": [ "d", "h", "min", "s" ]
                                               },
                                               {
                                       
                                                 "name": "energy",
                                                 "description": "Energy|",
                                                 "units": [ "Wh", "kWh", "MWh", "MJ", "GJ" ]
                                               },
                                               {
                                                 "name": "energy_storage",
                                                 "description": "Stored energy|",
                                                 "units": [ "Wh", "kWh", "MWh", "MJ", "GJ" ]
                                               },
                                               {
                                                 "name": "enum",
                                                 "description": "Has a limited set of (non-numeric) states",
                                                 "units": []
                                               },
                                               {
                                                 "name": "frequency",
                                                 "description": "Frequency|",
                                                 "units": [ "Hz", "kHz", "MHz", "GHz" ]
                                               },
                                               {
                                                 "name": "gas",
                                                 "description": "Gas volume|",
                                                 "units": [ "m³", "ft³", "CCF" ]
                                               },
                                               {
                                                 "name": "humidity",
                                                 "description": "Percentage of humidity in the air|",
                                                 "units": [ "%" ]
                                               },
                                               {
                                                 "name": "illuminance",
                                                 "description": "The current light level|",
                                                 "units": [ "lx" ]
                                               },
                                               {
                                                 "name": "irradiance",
                                                 "description": "Irradiance|",
                                                 "units": [ "W/m²", "BTU/(h⋅ft²)" ]
                                               },
                                               {
                                                 "name": "moisture",
                                                 "description": "Percentage of water in a substance|",
                                                 "units": [ "%" ]
                                               },
                                               {
                                                 "name": "monetary",
                                                 "description": "The monetary value (ISO 4217)",
                                                 "units": []
                                               },
                                               {
                                                 "name": "nitrogen_dioxide",
                                                 "description": "Concentration of Nitrogen Dioxide|",
                                                 "units": [ "µg/m³" ]
                                               },
                                               {
                                                 "name": "nitrogen_monoxide",
                                                 "description": "Concentration of Nitrogen Monoxide|",
                                                 "units": [ "µg/m³" ]
                                               },
                                               {
                                                 "name": "nitrous_oxide",
                                                 "description": "Concentration of Nitrous Oxide|",
                                                 "units": [ "µg/m³" ]
                                               },
                                               {
                                                 "name": "ozone",
                                                 "description": "Concentration of Ozone|",
                                                 "units": [ "µg/m³" ]
                                               },
                                               {
                                                 "name": "ph",
                                                 "description": "Potential hydrogen (pH) value of a water solution",
                                                 "units": []
                                               },
                                               {
                                                 "name": "pm1",
                                                 "description": "Concentration of particulate matter less than 1 micrometer|",
                                                 "units": [ "µg/m³" ]
                                               },
                                               {
                                                 "name": "pm25",
                                                 "description": "Concentration of particulate matter less than 2.5 micrometers|",
                                                 "units": [ "µg/m³" ]
                                               },
                                               {
                                                 "name": "pm10",
                                                 "description": "Concentration of particulate matter less than 10 micrometers|",
                                                 "units": [ "µg/m³" ]
                                               },
                                               {
                                                 "name": "power_factor",
                                                 "description": "Power factor (unitless), unit may be None or %",
                                                 "units": []
                                               },
                                               {
                                                 "name": "power",
                                                 "description": "Power|",
                                                 "units": [ "W", "kW" ]
                                               },
                                               {
                                                 "name": "precipitation",
                                                 "description": "Accumulated precipitation|",
                                                 "units": [ "cm", "in", "mm" ]
                                               },
                                               {
                                                 "name": "precipitation_intensity",
                                                 "description": "Precipitation intensity|",
                                                 "units": [ "in/d", "in/h", "mm/d or mm/h" ]
                                               },
                                               {
                                                 "name": "pressure",
                                                 "description": "Pressure|",
                                                 "units": [ "Pa", "kPa", "hPa", "bar", "cbar", "mbar", "mmHg", "inHg or psi" ]
                                               },
                                               {
                                                 "name": "reactive_power",
                                                 "description": "Reactive power|",
                                                 "units": [ "var" ]
                                               },
                                               {
                                                 "name": "signal_strength",
                                                 "description": "Signal strength|",
                                                 "units": [ "dB", "dBm" ]
                                               },
                                               {
                                                 "name": "sound_pressure",
                                                 "description": "Sound pressure|",
                                                 "units": [ "dB", "dBA" ]
                                               },
                                               {
                                                 "name": "speed",
                                                 "description": "Generic speed|",
                                                 "units": [ "ft/s", "in/d", "in/h", "km/h", "kn", "m/s", "mph or mm/d" ]
                                               },
                                               {
                                                 "name": "sulphur_dioxide",
                                                 "description": "Concentration of sulphur dioxide|",
                                                 "units": [ "µg/m³" ]
                                               },
                                               {
                                                 "name": "temperature",
                                                 "description": "Temperature|",
                                                 "units": [ "°C", "°F", "K" ]
                                               },
                                               {
                                                 "name": "timestamp",
                                                 "description": "Datetime object or timestamp string (ISO 8601)",
                                                 "units": []
                                               },
                                               {
                                                 "name": "volatile_organic_compounds",
                                                 "description": "Concentration of volatile organic compounds|",
                                                 "units": [ "µg/m³" ]
                                               },
                                               {
                                                 "name": "volatile_organic_compounds_parts",
                                                 "description": "Ratio of volatile organic compounds|",
                                                 "units": [ "ppm", "ppb" ]
                                               },
                                               {
                                                 "name": "voltage",
                                                 "description": "Voltage|",
                                                 "units": [ "V", "mV" ]
                                               },
                                               {
                                                 "name": "volume",
                                                 "description": "Generic volume|",
                                                 "units": [ "L", "mL", "gal", "fl. oz.", "m³", "ft³", "CCF" ]
                                               },
                                               {
                                                 "name": "volume_flow_rate",
                                                 "description": "Volume flow rate|",
                                                 "units": [ "m³/h", "ft³/min", "L/min", "gal/min" ]
                                               },
                                               {
                                                 "name": "volume_storage",
                                                 "description": "Generic stored volume|",
                                                 "units": [ "L", "mL", "gal", "fl. oz.", "m³", "ft³", "CCF" ]
                                               },
                                               {
                                                 "name": "water",
                                                 "description": "Water consumption|",
                                                 "units": [ "L", "gal", "m³", "ft³", "CCF" ]
                                               },
                                               {
                                                 "name": "weight",
                                                 "description": "Generic mass|",
                                                 "units": [ "kg", "g", "mg", "µg", "oz", "lb", "st" ]
                                               },
                                               {
                                                 "name": "wind_speed",
                                                 "description": "Wind speed|",
                                                 "units": [ "Beaufort", "ft/s", "km/h", "kn", "m/s", "mph" ]
                                               }
                                             ]
                                           },
                                           {
                                             "name": "switch",
                                             "read_only": false,
                                             "requires_command_handler": false,
                                             "additional_options": [],
                                             "domain_classes": [
                                               {
                                       
                                                 "name": "outlet",
                                                 "description": "A switch for a power outlet.",
                                                 "units": []
                                               },
                                               {
                                       
                                                 "name": "switch",
                                                 "description": "A generic switch.",
                                                 "units": []
                                               }
                                             ]
                                           },
                                           {
                                             "name": "update",
                                             "read_only": false,
                                             "additional_options": [],
                                             "domain_classes": [
                                               {
                                                 "name": "None",
                                                 "description": "A generic software update. This is the default and does not need to be set.",
                                                 "units": []
                                               },
                                               {
                                                 "name": "firmware",
                                                 "description": "This update integration provides firmwares.",
                                                 "units": []
                                               }
                                             ]
                                           },
                                           {
                                             "name": "valve",
                                             "read_only": false,
                                             "requires_command_handler": false,
                                             "additional_options": [],
                                             "domain_classes": [
                                               {
                                                 "name": "none",
                                                 "description": "Generic. This is the default.",
                                                 "units": []
                                               },
                                               {
                                                 "name": "water",
                                                 "description": "Valve that controls the flow of water through a system.",
                                                 "units": []
                                               },
                                               {
                                                 "name": "gas",
                                                 "description": "Valve that controls the flow of gas through a system.",
                                                 "units": []
                                               }
                                             ]
                                           }
                                         ]
                                       }
                                       """
                                   )]
internal class HassTypesGeneratorDefinitions
{
    // Do nothing. This class's only purpose is to house the attribute for code generator discovery.
}