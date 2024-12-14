using System;

namespace EventBusScripts 
{
    public interface IEventBase
    {
        public void UnsubscribeAll();
    }
    public interface IEvent : IEventBase
    {
        public void Subscribe(Action action);
        public void Unsubscribe(Action action);
        public void Invoke();
    }

    public interface IEvent<T> : IEventBase
    {
        public void Subscribe(Action<T> action);
        public void Unsubscribe(Action<T> action);
        public void Invoke(T arg);
    }
}