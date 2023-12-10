using System;
using System.Collections.Generic;
using PereViader.Utils.Common.Generators;

namespace PereViader.Utils.Common.ActiveStatuses
{
    [GenerateEventTaskWaits]
    public sealed class ActiveStatus<TId>
    {
        public event Action<TId, bool> OnChanged;

        private readonly Dictionary<TId, HashSet<object>> _activeStatuses = new Dictionary<TId, HashSet<object>>();

        public bool DefaultActiveState { get; }

        public ActiveStatus(bool defaultActiveState = false)
        {
            DefaultActiveState = defaultActiveState;
        }

        public bool IsActive(TId id)
        {
            if (!_activeStatuses.TryGetValue(id, out var registry))
            {
                return DefaultActiveState;
            }
            
            return (registry.Count > 0) ^ DefaultActiveState;
        }

        public bool UpdateStatus(object owner, TId id, bool active)
        {
            var previousActive = IsActive(id);
            
            if (!_activeStatuses.TryGetValue(id, out var registry))
            {
                registry = new HashSet<object>();
                _activeStatuses.Add(id, registry);
            }
            
            if (active != DefaultActiveState)
            {
                registry.Add(owner);
            }
            else
            {
                registry.Remove(owner);
            }

            var currentIsActive = IsActive(id);
            var hasChanged = previousActive != currentIsActive;

            if (hasChanged)
            {
                OnChanged?.Invoke(id, currentIsActive);
            }
            
            return hasChanged;
        }

        public void Forget(TId id)
        {
            _activeStatuses.Remove(id);
        }

        public void ForgetAll()
        {
            _activeStatuses.Clear();
        }
    }
}