using System;
using System.Collections.Generic;
using PereViader.Utils.Common.Generators;

namespace PereViader.Utils.Common.ActiveStatuses
{
    [GenerateEventTaskWaits]
    public sealed class ActiveStatus
    {
        public event Action<bool>? OnChanged;

        private readonly HashSet<object> _registry = new HashSet<object>();

        public bool DefaultActiveState { get; }

        public ActiveStatus(bool defaultActiveState)
        {
            DefaultActiveState = defaultActiveState;
        }

        public bool IsActive()
        {
            return (_registry.Count > 0) ^ DefaultActiveState;
        }
        
        public bool IsActive(object owner)
        {
            return _registry.Contains(owner) ^ DefaultActiveState;
        }

        public bool Update(object owner, bool active)
        {
            var previousActive = IsActive();

            if (active != DefaultActiveState)
            {
                _registry.Add(owner);
            }
            else
            {
                _registry.Remove(owner);
            }

            var currentIsActive = IsActive();
            var hasChanged = previousActive != currentIsActive;

            if (hasChanged)
            {
                OnChanged?.Invoke(currentIsActive);
            }
            
            return hasChanged;
        }

        public bool Toggle(object owner)
        {
            var previousActive = IsActive(owner);
            return Update(owner, !previousActive);
        }

        public void ForgetAll()
        {
            _registry.Clear();
        }
    }
    
    [GenerateEventTaskWaits]
    public sealed class ActiveStatus<TId>
    {
        public event Action<TId, bool>? OnChanged;

        private readonly Dictionary<TId, HashSet<object>> _activeStatuses = new Dictionary<TId, HashSet<object>>();

        public bool DefaultActiveState { get; }

        public ActiveStatus(bool defaultActiveState)
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
        
        public bool IsActive(object owner, TId id)
        {
            if (!_activeStatuses.TryGetValue(id, out var registry))
            {
                return DefaultActiveState;
            }
            
            return registry.Contains(owner) ^ DefaultActiveState;
        }

        public bool Update(object owner, TId id, bool active)
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
        
        public bool Toggle(object owner, TId id)
        {
            var previousActive = IsActive(owner, id);
            return Update(owner, id, !previousActive);
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