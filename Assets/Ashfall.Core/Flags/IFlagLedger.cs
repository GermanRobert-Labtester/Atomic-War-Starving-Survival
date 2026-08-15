using System;
using System.Collections.Generic;

namespace Ashfall.Core.Flags
{
    public interface IFlagLedger
    {
        bool IsSet(string flagId);
        void Set(string flagId);
        void Clear(string flagId);
        int GetCounter(string counterId);
        void Increment(string counterId, int amount = 1);
        void SetCounter(string counterId, int value);
    }

    public sealed class InMemoryFlagLedger : IFlagLedger
    {
        private readonly HashSet<string> _flags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, int> _counters = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        public bool IsSet(string flagId)
        {
            if (string.IsNullOrEmpty(flagId)) return false;
            return _flags.Contains(flagId);
        }

        public void Set(string flagId)
        {
            if (string.IsNullOrEmpty(flagId)) return;
            _flags.Add(flagId);
        }

        public void Clear(string flagId)
        {
            if (string.IsNullOrEmpty(flagId)) return;
            _flags.Remove(flagId);
        }

        public int GetCounter(string counterId)
        {
            if (string.IsNullOrEmpty(counterId)) return 0;
            _counters.TryGetValue(counterId, out var val);
            return val;
        }

        public void Increment(string counterId, int amount = 1)
        {
            if (string.IsNullOrEmpty(counterId)) return;
            int cur = GetCounter(counterId);
            _counters[counterId] = cur + amount;
        }

        public void SetCounter(string counterId, int value)
        {
            if (string.IsNullOrEmpty(counterId)) return;
            _counters[counterId] = value;
        }
    }
}
