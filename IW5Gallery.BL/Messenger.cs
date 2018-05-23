using System;
using System.Collections.Concurrent;
using System.Linq;

namespace IW5Gallery.BL
{
    public class Messenger:IMessenger
    {
        private readonly ConcurrentDictionary<Type, ConcurrentBag<Delegate>> _registeredActions = new ConcurrentDictionary<Type, ConcurrentBag<Delegate>>();
        private readonly object _bagLock = new object();

        public void Register<TMessage>(Action<TMessage> action)
        {
            var key = typeof(TMessage);

            lock (_bagLock)
            {
                ConcurrentBag<Delegate> actions;
                if (!_registeredActions.TryGetValue(typeof(TMessage), out actions))
                {
                    actions = new ConcurrentBag<Delegate>();
                    _registeredActions[key] = actions;
                }

                actions.Add(action);
            }

        }

        public void Send<TMessage>(TMessage message)
        {
            ConcurrentBag<Delegate> actions;
            if (_registeredActions.TryGetValue(typeof(TMessage), out actions))
            {
                foreach (var action in actions.Select(a => a as Action<TMessage>).Where(a => a != null))
                {
                    action(message);
                }
            }
        }

        public void UnRegister<TMessage>(Action<TMessage> action)
        {
            var key = typeof(TMessage);

            ConcurrentBag<Delegate> actions;
            if (_registeredActions.TryGetValue(typeof(TMessage), out actions))
            {
                lock (_bagLock)
                {
                    var actionsList = actions.ToList();
                    actionsList.Remove(action);
                    _registeredActions[key] = new ConcurrentBag<Delegate>(actionsList);
                }
            }
        }
    }
}
