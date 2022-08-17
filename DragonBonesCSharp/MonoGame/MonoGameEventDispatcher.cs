using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonBones
{
    public class MonoGameEventDispatcher<T>
    {
        private readonly Dictionary<string, ListenerDelegate<T>> _listeners = new Dictionary<string, ListenerDelegate<T>>();

        public MonoGameEventDispatcher() { }

        public void DispatchEvent(string type, T eventObject)
        {
            if (!HasEventListener(type))
            {
                return;
            }
            else
            {
                _listeners[type](type, eventObject);
            }
        }

        public bool HasEventListener(string type)
        {
            return _listeners.ContainsKey(type);
        }

        public void AddEventListener(string type, ListenerDelegate<T> listener)
        {
            if (HasEventListener(type))
            {
                var delegates = _listeners[type].GetInvocationList();
                for (int i = 0; i < delegates.Length; i++)
                {
                    if (listener == delegates[i] as ListenerDelegate<T>)
                    {
                        return;
                    }
                }

                _listeners[type] += listener;
            }
            else
            {
                _listeners.Add(type, listener);
            }
        }

        public void RemoveEventListener(string type, ListenerDelegate<T> listener)
        {
            if (!HasEventListener(type))
            {
                return;
            }

            var delegates = _listeners[type].GetInvocationList();
            for (int i = 0; i < delegates.Length; i++)
            {
                if (listener == delegates[i] as ListenerDelegate<T>)
                {
                    _listeners[type] -= listener;
                    break;
                }
            }

            if (_listeners[type] == null)
            {
                _listeners.Remove(type);
            }
        }
    }

    public class DragonBonesEventDispatcher : MonoGameEventDispatcher<EventObject>, IEventDispatcher<EventObject>
    {
        public void AddDBEventListener(string type, ListenerDelegate<EventObject> listener)
        {
            AddEventListener(type, listener);
        }

        public void DispatchDBEvent(string type, EventObject eventObject)
        {
            DispatchEvent(type, eventObject);
        }

        public bool HasDBEventListener(string type)
        {
            return HasEventListener(type);
        }

        public void RemoveDBEventListener(string type, ListenerDelegate<EventObject> listener)
        {
            RemoveEventListener(type, listener);
        }
    }
}
