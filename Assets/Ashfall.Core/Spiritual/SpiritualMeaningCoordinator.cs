using System;
using System.Collections.Generic;
using Ashfall.Core.Memorial;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Spiritual
{
    [Serializable]
    public sealed class SpiritualCoordinatorSaveState
    {
        public List<MourningArcRecord> MourningArcs = new List<MourningArcRecord>();
        public Dictionary<string, int> RitualLastPerformedDay = new Dictionary<string, int>();
    }

    /// <summary>
    /// Plan 30 Core Coordinator — bridges spiritual, mourning, and ritual content
    /// into existing simulation authorities (MemorialSystem, NeedsSystem, IdeologicalFrictionSystem,
    /// GuiltInsomniaSystem, LeadershipSystem) without introducing any parallel faith or piety meters.
    /// </summary>
    public sealed class SpiritualMeaningCoordinator
    {
        private readonly SpiritualCatalog _catalog;
        private readonly Dictionary<string, MourningArcRecord> _mourningArcs =
            new Dictionary<string, MourningArcRecord>(StringComparer.Ordinal);
        private readonly Dictionary<string, int> _ritualLastPerformedDay =
            new Dictionary<string, int>(StringComparer.Ordinal);

        public event Action<string, float>? OnRitualPerformed; // ritualId, moraleDelta
        public event Action<string, string>? OnMemorialRitePerformed; // deceasedId, riteId
        public event Action<string, int>? OnMourningStageAdvanced; // deceasedId, newStage

        public SpiritualMeaningCoordinator(SpiritualCatalog catalog)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        }

        public IReadOnlyDictionary<string, MourningArcRecord> MourningArcs => _mourningArcs;

        // ── Ritual & Comfort Execution (Anti-Exploit / Cooldown Guarded) ──

        public bool CanPerformRitual(string ritualId, int currentDay)
        {
            var def = _catalog.GetRitual(ritualId);
            if (def == null) return false;

            if (_ritualLastPerformedDay.TryGetValue(ritualId, out int lastDay))
            {
                if (currentDay - lastDay < def.CooldownDays)
                    return false;
            }
            return true;
        }

        public bool TryPerformRitual(string ritualId, int currentDay, Action<float>? applyMoraleCallback = null)
        {
            if (!CanPerformRitual(ritualId, currentDay)) return false;

            var def = _catalog.GetRitual(ritualId);
            if (def == null) return false;

            _ritualLastPerformedDay[ritualId] = currentDay;

            if (def.MoraleDelta != 0f && applyMoraleCallback != null)
            {
                applyMoraleCallback(def.MoraleDelta);
            }

            OnRitualPerformed?.Invoke(ritualId, def.MoraleDelta);
            return true;
        }

        // ── Staged Mourning & Memorial Rites ───────────────────────

        public void RegisterDeath(string deceasedId, int day)
        {
            if (string.IsNullOrEmpty(deceasedId)) return;
            if (_mourningArcs.ContainsKey(deceasedId)) return;

            var arc = new MourningArcRecord
            {
                DeceasedId = deceasedId,
                DeathDay = day,
                CurrentStage = 1, // Acute Shock
                LastUpdateDay = day
            };
            _mourningArcs[deceasedId] = arc;
        }

        public bool PerformMemorialRite(string deceasedId, string riteId, int day)
        {
            if (string.IsNullOrEmpty(deceasedId) || string.IsNullOrEmpty(riteId)) return false;
            if (!_mourningArcs.TryGetValue(deceasedId, out var arc)) return false;

            var riteDef = _catalog.GetMemorialRite(riteId);
            if (riteDef == null) return false;

            arc.PerformedRiteId = riteId;
            arc.RiteCompleted = true;
            arc.LastUpdateDay = day;

            OnMemorialRitePerformed?.Invoke(deceasedId, riteId);
            return true;
        }

        public void SkipMemorialRite(string deceasedId, int day)
        {
            if (string.IsNullOrEmpty(deceasedId)) return;
            if (!_mourningArcs.TryGetValue(deceasedId, out var arc)) return;

            arc.RiteSkipped = true;
            arc.LastUpdateDay = day;
        }

        public void TickMourning(int currentDay)
        {
            foreach (var kvp in _mourningArcs)
            {
                var arc = kvp.Value;
                int daysSinceDeath = currentDay - arc.DeathDay;
                int expectedStage = arc.CurrentStage;

                if (daysSinceDeath >= 30) expectedStage = 5;      // Long-Tail Echo / Anniversary
                else if (daysSinceDeath >= 7) expectedStage = 4;  // Memorial Observance
                else if (daysSinceDeath >= 3) expectedStage = 3;  // Return of the Ordinary
                else if (daysSinceDeath >= 1) expectedStage = 2;  // Empty Shift
                else expectedStage = 1;                           // Acute Shock

                if (expectedStage > arc.CurrentStage)
                {
                    arc.CurrentStage = expectedStage;
                    arc.LastUpdateDay = currentDay;
                    OnMourningStageAdvanced?.Invoke(arc.DeceasedId, arc.CurrentStage);
                }
            }
        }

        public MourningArcRecord? GetMourningArc(string deceasedId) =>
            _mourningArcs.TryGetValue(deceasedId, out var arc) ? arc : null;

        // ── Save / Load ───────────────────────────────────────────

        public SpiritualCoordinatorSaveState CaptureState()
        {
            var save = new SpiritualCoordinatorSaveState();
            foreach (var kvp in _mourningArcs)
            {
                var a = kvp.Value;
                save.MourningArcs.Add(new MourningArcRecord
                {
                    DeceasedId = a.DeceasedId,
                    DeathDay = a.DeathDay,
                    CurrentStage = a.CurrentStage,
                    PerformedRiteId = a.PerformedRiteId,
                    RiteCompleted = a.RiteCompleted,
                    RiteSkipped = a.RiteSkipped,
                    LastUpdateDay = a.LastUpdateDay
                });
            }

            foreach (var kvp in _ritualLastPerformedDay)
            {
                save.RitualLastPerformedDay[kvp.Key] = kvp.Value;
            }

            return save;
        }

        public void RestoreState(SpiritualCoordinatorSaveState? save)
        {
            _mourningArcs.Clear();
            _ritualLastPerformedDay.Clear();
            if (save == null) return;

            if (save.MourningArcs != null)
            {
                foreach (var a in save.MourningArcs)
                {
                    if (a == null || string.IsNullOrEmpty(a.DeceasedId)) continue;
                    _mourningArcs[a.DeceasedId] = new MourningArcRecord
                    {
                        DeceasedId = a.DeceasedId,
                        DeathDay = a.DeathDay,
                        CurrentStage = a.CurrentStage,
                        PerformedRiteId = a.PerformedRiteId,
                        RiteCompleted = a.RiteCompleted,
                        RiteSkipped = a.RiteSkipped,
                        LastUpdateDay = a.LastUpdateDay
                    };
                }
            }

            if (save.RitualLastPerformedDay != null)
            {
                foreach (var kvp in save.RitualLastPerformedDay)
                {
                    if (!string.IsNullOrEmpty(kvp.Key))
                        _ritualLastPerformedDay[kvp.Key] = kvp.Value;
                }
            }
        }
    }
}
