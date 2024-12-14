using System;

namespace EventBusScripts 
{
    public class Event : IEvent
    {
        private event Action _callback;

        public void UnsubscribeAll()
        {
            _callback = null;
        }

        public void Subscribe(Action action)
        {
            _callback -= action;
            _callback += action;
        }

        public void Unsubscribe(Action action)
        {
            _callback -= action;
        }

        public void Invoke()
        {
            _callback?.Invoke();
        }

    }

    public class Event<T> : IEvent<T>
    {
        private event Action<T> _callback;

        public void UnsubscribeAll()
        {
            _callback = null;
        }

        public void Subscribe(Action<T> action)
        {
            _callback -= action;
            _callback += action;
        }

        public void Unsubscribe(Action<T> action)
        {
            _callback -= action;
        }

        public void Invoke(T arg)
        {
            _callback?.Invoke(arg);
        }

    }
}