using System;
using System.Collections.Generic;

namespace Ashfall.Core.Flags
{
    public interface IFlagLedger
    {
        bool IsSet(string flagId);
        void Set(string flagId, string originSystem = "", string sourceEvent = "", int day = 0, string subjectId = "");
        void Clear(string flagId);
        int GetCounter(string counterId);
        void Increment(string counterId, int amount = 1, string originSystem = "", string sourceEvent = "", int day = 0, string subjectId = "");
        void SetCounter(string counterId, int value, string originSystem = "", string sourceEvent = "", int day = 0, string subjectId = "");
    }

    public sealed class InMemoryFlagLedger : IFlagLedger
    {
        private static string Normalize(string id) => id == null ? string.Empty : id.Trim().ToLowerInvariant();

        private readonly HashSet<string> _flags = new HashSet<string>(StringComparer.Ordinal);
        private readonly Dictionary<string, int> _counters = new Dictionary<string, int>(StringComparer.Ordinal);

        public bool IsSet(string flagId)
        {
            if (string.IsNullOrEmpty(flagId)) return false;
            return _flags.Contains(Normalize(flagId));
        }

        public void Set(string flagId, string originSystem = "", string sourceEvent = "", int day = 0, string subjectId = "")
        {
            if (string.IsNullOrEmpty(flagId)) return;
            _flags.Add(Normalize(flagId));
        }

        public void Clear(string flagId)
        {
            if (string.IsNullOrEmpty(flagId)) return;
            _flags.Remove(Normalize(flagId));
        }

        public int GetCounter(string counterId)
        {
            if (string.IsNullOrEmpty(counterId)) return 0;
            _counters.TryGetValue(Normalize(counterId), out var val);
            return val;
        }

        public void Increment(string counterId, int amount = 1, string originSystem = "", string sourceEvent = "", int day = 0, string subjectId = "")
        {
            if (string.IsNullOrEmpty(counterId)) return;
            string n = Normalize(counterId);
            _counters.TryGetValue(n, out var cur);
            _counters[n] = cur + amount;
        }

        public void SetCounter(string counterId, int value, string originSystem = "", string sourceEvent = "", int day = 0, string subjectId = "")
        {
            if (string.IsNullOrEmpty(counterId)) return;
            _counters[Normalize(counterId)] = value;
        }
    }
}
