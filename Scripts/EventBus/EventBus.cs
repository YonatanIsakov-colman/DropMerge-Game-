using System;
using System.Collections.Generic;

namespace EventBusScripts 
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, IEventBase> _events = new();

        public static T Get<T>() where T : IEventBase, new()
        {
            var eventType = typeof(T);
            return (T)Get(eventType);
        }

        private static IEventBase Get(Type eventType)
        {
            return _events.TryGetValue(eventType, out var concreteEvent) ? concreteEvent : Bind(eventType);
        }

        private static IEventBase Bind(Type eventType)
        {
            try
            {
                var concreteEvent = (IEventBase)Activator.CreateInstance(eventType);
                _events.Add(eventType, concreteEvent);
                return concreteEvent;
            }
            catch
            {
                throw new InvalidOperationException($"failed to create event of type {eventType}");
            }
        }
    }
}