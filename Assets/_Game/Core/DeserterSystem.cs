using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Deserter Mechanic / Spies (Prompt #75). A soldier from a hostile faction
    /// begs to join the bunker. If accepted, they provide combat bonuses + intel.
    /// Hidden 30% chance they're a spy — 10 days later they sabotage the air
    /// filter and unlock the hatch from inside during a raid.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class DeserterSystem
    {
        /// <summary>Chance (0..1) that a deserter is actually a spy.</summary>
        public const float SpyChance = 0.30f;

        /// <summary>Days until a spy executes their sabotage.</summary>
        public const float SpySabotageDelayDays = 10f;

        /// <summary>Combat bonus provided by a genuine deserter.</summary>
        public const float DeserterCombatBonus = 15f;

        /// <summary>Intel nodes revealed by a genuine deserter.</summary>
        public const int DeserterIntelCount = 3;

        /// <summary>Event id for the deserter arrival.</summary>
        public const string DeserterEventId = "deserter_at_hatch";

        /// <summary>Event id for spy sabotage.</summary>
        public const string SpySabotageEventId = "spy_sabotage";

        /// <summary>Trait id applied to deserter survivors.</summary>
        public const string DeserterTraitId = "deserter";

        /// <summary>Trait id applied to spies (hidden from player).</summary>
        public const string SpyTraitId = "spy";

        /// <summary>Deserter entries.</summary>
        public class DeserterEntry
        {
            public string SurvivorId;
            public string OriginFactionId;
            public bool IsSpy;
            public bool SpyRevealed;
            public float SpyDaysUntilSabotage;
            public bool SabotageExecuted;
        }

        private readonly List<DeserterEntry> _deserters = new List<DeserterEntry>();
        private readonly System.Random _rng;

        // -- Events --
        public event Action<DeserterEntry> OnDeserterAccepted;
        public event Action<DeserterEntry> OnSpyRevealed;
        public event Action<DeserterEntry> OnSabotageExecuted;

        public IReadOnlyList<DeserterEntry> Deserters => _deserters;

        public DeserterSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(75);
        }

        /// <summary>
        /// Accept a deserter into the bunker. Rolls for spy chance.
        /// Returns the entry with IsSpy flag set.
        /// </summary>
        public DeserterEntry AcceptDeserter(string survivorId, string originFactionId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;

            bool isSpy = _rng.NextDouble() < SpyChance;

            var entry = new DeserterEntry
            {
                SurvivorId = survivorId,
                OriginFactionId = originFactionId ?? string.Empty,
                IsSpy = isSpy,
                SpyRevealed = false,
                SpyDaysUntilSabotage = isSpy ? SpySabotageDelayDays : 0f,
                SabotageExecuted = false
            };
            _deserters.Add(entry);
            OnDeserterAccepted?.Invoke(entry);
            return entry;
        }

        /// <summary>
        /// Check if a survivor is a spy (for internal logic only — hidden from player).
        /// </summary>
        public bool IsSpy(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            for (int i = 0; i < _deserters.Count; i++)
                if (_deserters[i].SurvivorId == survivorId && _deserters[i].IsSpy)
                    return true;
            return false;
        }

        /// <summary>
        /// Whether a deserter provides genuine combat bonuses (not a spy or spy not yet revealed).
        /// </summary>
        public float GetCombatBonus(string survivorId)
        {
            if (IsSpy(survivorId)) return 0f;
            for (int i = 0; i < _deserters.Count; i++)
                if (_deserters[i].SurvivorId == survivorId)
                    return DeserterCombatBonus;
            return 0f;
        }

        /// <summary>
        /// Daily tick. Advances spy countdown and executes sabotage.
        /// </summary>
        public void TickDaily(Shelter.Shelter shelter,
            Action<string> scheduleEvent = null)
        {
            for (int i = _deserters.Count - 1; i >= 0; i--)
            {
                var entry = _deserters[i];
                if (!entry.IsSpy || entry.SpyRevealed || entry.SabotageExecuted) continue;

                entry.SpyDaysUntilSabotage -= 1f;
                if (entry.SpyDaysUntilSabotage <= 0f)
                {
                    ExecuteSabotage(entry, shelter, scheduleEvent);
                }
            }
        }

        private void ExecuteSabotage(DeserterEntry entry, Shelter.Shelter shelter,
            Action<string> scheduleEvent)
        {
            entry.SabotageExecuted = true;
            entry.SpyRevealed = true;

            // Sabotage air filter.
            if (shelter != null)
            {
                var airFilter = shelter.GetModule("air_filtration");
                if (airFilter != null)
                {
                    airFilter.FilterHealth = Mathf.Max(0f, airFilter.FilterHealth - 60f);
                    airFilter.IsEnabled = false;
                }
            }

            // Schedule the narrative event.
            scheduleEvent?.Invoke(SpySabotageEventId);

            OnSpyRevealed?.Invoke(entry);
            OnSabotageExecuted?.Invoke(entry);
        }

        /// <summary>
        /// Reveal a spy through investigation/interrogation (player action).
        /// </summary>
        public bool RevealSpy(string survivorId)
        {
            var entry = FindEntry(survivorId);
            if (entry == null || !entry.IsSpy || entry.SpyRevealed) return false;
            entry.SpyRevealed = true;
            OnSpyRevealed?.Invoke(entry);
            return true;
        }

        private DeserterEntry FindEntry(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            for (int i = 0; i < _deserters.Count; i++)
                if (_deserters[i].SurvivorId == survivorId) return _deserters[i];
            return null;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public DeserterSave CaptureState()
        {
            var entries = new DeserterEntrySave[_deserters.Count];
            for (int i = 0; i < _deserters.Count; i++)
            {
                var d = _deserters[i];
                entries[i] = new DeserterEntrySave
                {
                    SurvivorId = d.SurvivorId,
                    OriginFactionId = d.OriginFactionId,
                    IsSpy = d.IsSpy,
                    SpyRevealed = d.SpyRevealed,
                    SpyDaysUntilSabotage = d.SpyDaysUntilSabotage,
                    SabotageExecuted = d.SabotageExecuted
                };
            }
            return new DeserterSave { Entries = entries };
        }

        public void RestoreState(DeserterSave save)
        {
            _deserters.Clear();
            if (save?.Entries == null) return;
            for (int i = 0; i < save.Entries.Length; i++)
            {
                var e = save.Entries[i];
                if (e == null || string.IsNullOrEmpty(e.SurvivorId)) continue;
                _deserters.Add(new DeserterEntry
                {
                    SurvivorId = e.SurvivorId,
                    OriginFactionId = e.OriginFactionId,
                    IsSpy = e.IsSpy,
                    SpyRevealed = e.SpyRevealed,
                    SpyDaysUntilSabotage = e.SpyDaysUntilSabotage,
                    SabotageExecuted = e.SabotageExecuted
                });
            }
        }
    }

    [Serializable]
    public class DeserterSave
    {
        public DeserterEntrySave[] Entries;
    }

    [Serializable]
    public class DeserterEntrySave
    {
        public string SurvivorId;
        public string OriginFactionId;
        public bool IsSpy;
        public bool SpyRevealed;
        public float SpyDaysUntilSabotage;
        public bool SabotageExecuted;
    }
}
