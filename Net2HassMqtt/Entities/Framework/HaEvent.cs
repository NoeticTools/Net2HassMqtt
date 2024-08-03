﻿namespace NoeticTools.Net2HassMqtt.Entities.Framework;

public class HaEvent(IEnumerable<string> eventTypes) {
    public event EventHandler<DictEventArgs>? Event;
    public List<string> EventTypes { get; } = eventTypes.ToList();

    public HaEvent(string eventType) : this(new List<string> { eventType }) {
        if (eventType.Length == 0) {
            throw new ArgumentException("Must specify at least one event type.", nameof(eventType));
        }
    }

    public void Fire(params (string key, string value)[]? args) {
        var dict = new Dictionary<string, string>();
        if (args != null) {
            foreach (var kvp in args) {
                if (!dict.TryAdd(kvp.key, kvp.value)) {
                    throw new ArgumentException($"Tuples must contain unique keys. Collision found: '{kvp.key}", nameof(args));
                }
            }
        }

        Fire(dict);
    }

    public void Fire(IDictionary<string, string>? args = null) {
        if (EventTypes.Count != 1) {
            throw
                new InvalidOperationException("Cannot call Fire with default event_type when there are is exactly one event type that can be used.");
        }

        Fire(EventTypes[0], args);
    }
    
    public void Fire(string eventType, params (string key, string value)[]? args) {
        var dict = new Dictionary<string, string>();
        if (args != null) {
            foreach (var kvp in args) {
                if (!dict.TryAdd(kvp.key, kvp.value)) {
                    throw new ArgumentException($"Tuples must contain unique keys. Collision found: '{kvp.key}", nameof(args));
                }
            }
        }

        Fire(eventType, dict);
    }

    public void Fire(string eventType, IDictionary<string, string>? args = null) {
        if (Event == null) {
            return;
        }

        if (!EventTypes.Contains(eventType)) {
            throw new ArgumentException($"'{eventType}' not found in constructed list of allowed event types", nameof(eventType));
        }

        var allArgs = new Dictionary<string, string> { { "event_type", eventType } };
        if (args != null) {
            foreach (var kvp in args) {
                allArgs.Add(kvp.Key, kvp.Value);
            }
        }

        Event.Invoke(this, new DictEventArgs(allArgs));
    }

    public class DictEventArgs : EventArgs {
        public readonly Dictionary<string, string> Arguments;

        public DictEventArgs(IDictionary<string, string> args) {
            Arguments = new Dictionary<string, string>();
            Arguments = args.ToDictionary(entry => entry.Key, entry => entry.Value);
        }
    }
}