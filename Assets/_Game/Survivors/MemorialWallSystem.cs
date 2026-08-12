using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Memorial Wall — tracks fallen bunker members and dog-tag memorial entries.
    /// Paying respects grants temporary morale comfort hours.
    /// </summary>
    public class MemorialWallSystem
    {
        public const float PayRespectsDurationHours = 1f;
        public const float MemorialComfortBuffHours = 8f;

        public event Action<MemorialEntry> OnMemorialEntryAdded;

        private readonly List<MemorialEntry> _entries = new List<MemorialEntry>();

        public IReadOnlyList<MemorialEntry> Entries => _entries;

        public void ClearEntries() => _entries.Clear();

        public void AddEntry(MemorialEntry entry)
        {
            if (string.IsNullOrEmpty(entry.SurvivorId)) return;
            for (int i = 0; i < _entries.Count; i++)
            {
                if (_entries[i].SurvivorId == entry.SurvivorId)
                {
                    _entries[i] = entry;
                    OnMemorialEntryAdded?.Invoke(entry);
                    return;
                }
            }
            _entries.Add(entry);
            OnMemorialEntryAdded?.Invoke(entry);
        }

        public void SyncDeadSurvivors(IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || sv.IsAlive) continue;
                AddEntry(new MemorialEntry
                {
                    SurvivorId = sv.Id,
                    DisplayName = sv.DisplayName,
                    DeathDay = 0,
                    HasDogTag = true
                });
            }
        }

        public bool PayRespects(Survivor survivor)
        {
            if (survivor == null || !survivor.IsAlive) return false;
            if (survivor.HasPaidRespectsAtMemorial) return false;
            survivor.HasPaidRespectsAtMemorial = true;
            survivor.MemorialComfortHours = MemorialComfortBuffHours;
            return true;
        }
    }

    [Serializable]
    public struct MemorialEntry
    {
        public string SurvivorId;
        public string DisplayName;
        public int DeathDay;
        public bool HasDogTag;
    }
}
