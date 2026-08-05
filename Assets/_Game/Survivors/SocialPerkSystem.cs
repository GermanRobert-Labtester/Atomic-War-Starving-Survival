using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Social / leadership milestone perks (Prompts #211–#213).
    /// Earned through peaceful de-escalation, internal hauling, and sustained
    /// high morale — not XP grind. Plain C#, save/load safe. Inventory-free
    /// (Survivors asmdef has no Inventory/Core ref).
    /// </summary>
    public class SocialPerkSystem
    {
        // ── Perk ids ─────────────────────────────────────────────────────
        public const string DeEscalatorId = "perk_de_escalator";
        public const string QuartermasterId = "perk_quartermaster";
        public const string TaskmasterId = "perk_taskmaster";

        // ── Thresholds ───────────────────────────────────────────────────
        /// <summary>#211 — one successful non-force breakup of a ViolentParanoia fight.</summary>
        public const int PeacefulDeEscalationsForPerk = 1;

        /// <summary>#212 — items moved via InternalHauling (#173).</summary>
        public const int ItemsHauledForQuartermaster = 100;

        /// <summary>#213 — consecutive days with Morale &gt; 90.</summary>
        public const int HighMoraleDaysForTaskmaster = 14;

        /// <summary>Morale must strictly exceed this each day for Taskmaster streak.</summary>
        public const float TaskmasterMoraleThreshold = 90f;

        // ── Effect constants ─────────────────────────────────────────────
        /// <summary>#212 — items in the same room degrade at half rate.</summary>
        public const float QuartermasterDegradationMult = 0.5f;

        /// <summary>#213 — Utility AI work rate under Pacing Aura (+15%).</summary>
        public const float TaskmasterActionSpeedMult = 1.15f;

        private SkillProgressionSystem _progression;
        private readonly Dictionary<string, SocialCounters> _bySurvivor =
            new Dictionary<string, SocialCounters>();

        public event Action<Survivor, string> OnSocialPerkEarned;
        public event Action<Survivor, string, int> OnMilestoneProgress;

        public void Bind(SkillProgressionSystem progression)
        {
            _progression = progression;
            _progression?.RegisterSocialPerks();
        }

        public void RegisterCatalog() => _progression?.RegisterSocialPerks();

        // ── Queries ──────────────────────────────────────────────────────

        public bool Has(string survivorId, string perkId)
        {
            if (_progression == null || string.IsNullOrEmpty(survivorId)) return false;
            return _progression.HasActivePerk(survivorId, perkId);
        }

        public bool Has(Survivor sv, string perkId) =>
            sv != null && Has(sv.Id, perkId);

        public SocialCounters GetCounters(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return new SocialCounters();
            return GetOrCreate(survivorId).Clone();
        }

        public bool AnyLivingHas(IReadOnlyList<Survivor> survivors, string perkId)
        {
            if (survivors == null) return false;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv != null && sv.IsAlive && Has(sv, perkId))
                    return true;
            }
            return false;
        }

        // ── #211 De-Escalator ────────────────────────────────────────────

        /// <summary>
        /// Record a successful non-force resolution of a ViolentParanoia fight
        /// (comfort/talk — not medical bed, meds, or isolation). Grants
        /// De-Escalator after the first success.
        /// </summary>
        public void RecordPeacefulDeEscalation(Survivor intervener, int currentDay = 0)
        {
            if (intervener == null || !intervener.IsAlive) return;
            var c = GetOrCreate(intervener.Id);
            c.PeacefulDeEscalations++;
            OnMilestoneProgress?.Invoke(intervener, "peaceful_de_escalations", c.PeacefulDeEscalations);
            if (c.PeacefulDeEscalations >= PeacefulDeEscalationsForPerk)
                TryGrant(intervener, DeEscalatorId, currentDay);
        }

        public bool HasDeEscalator(Survivor sv) => Has(sv, DeEscalatorId);

        /// <summary>
        /// Talk Down: instantly end ViolentParanoia without meds or isolation.
        /// Requires De-Escalator on the speaker. Returns true if the break ended.
        /// </summary>
        public bool TryTalkDown(
            Survivor speaker,
            Survivor target,
            MentalBreakSystem mentalBreak,
            int currentDay = 0)
        {
            if (speaker == null || !speaker.IsAlive || !HasDeEscalator(speaker)) return false;
            if (target == null || !target.IsAlive || !target.HasMentalBreak) return false;
            if (mentalBreak == null) return false;
            if (!IsViolentParanoia(target.currentMentalBreakId)) return false;

            mentalBreak.Cure(target);
            // Talk Down itself is also a peaceful de-escalation (idempotent grant).
            RecordPeacefulDeEscalation(speaker, currentDay);
            return true;
        }

        public static bool IsViolentParanoia(string breakId)
        {
            if (string.IsNullOrEmpty(breakId)) return false;
            return string.Equals(breakId, MentalBreakSO.Ids.ViolentParanoia, StringComparison.OrdinalIgnoreCase)
                   || string.Equals(breakId, "violent_paranoia", StringComparison.OrdinalIgnoreCase);
        }

        // ── #212 Quartermaster ───────────────────────────────────────────

        /// <summary>
        /// Record items moved via InternalHauling. Weight kg maps 1:1 to item
        /// units (haul capacity is kg; bunker loot is treated as unit weight).
        /// Fractional moves still count at least 1 when any mass was moved.
        /// </summary>
        public void RecordItemsHauled(Survivor hauler, float weightKgMoved, int currentDay = 0)
        {
            if (hauler == null || !hauler.IsAlive || weightKgMoved <= 0f) return;
            int items = Mathf.Max(1, Mathf.FloorToInt(weightKgMoved));
            var c = GetOrCreate(hauler.Id);
            c.ItemsHauled += items;
            OnMilestoneProgress?.Invoke(hauler, "items_hauled", c.ItemsHauled);
            if (c.ItemsHauled >= ItemsHauledForQuartermaster)
                TryGrant(hauler, QuartermasterId, currentDay);
        }

        /// <summary>Direct item-count path for tests / precise tallies.</summary>
        public void RecordItemsHauledCount(Survivor hauler, int itemCount, int currentDay = 0)
        {
            if (hauler == null || !hauler.IsAlive || itemCount <= 0) return;
            var c = GetOrCreate(hauler.Id);
            c.ItemsHauled += itemCount;
            OnMilestoneProgress?.Invoke(hauler, "items_hauled", c.ItemsHauled);
            if (c.ItemsHauled >= ItemsHauledForQuartermaster)
                TryGrant(hauler, QuartermasterId, currentDay);
        }

        public bool HasQuartermaster(Survivor sv) => Has(sv, QuartermasterId);

        /// <summary>
        /// Degradation rate multiplier for items stored in <paramref name="roomId"/>.
        /// 0.5 when a living Quartermaster occupies that room; else 1.
        /// </summary>
        public float GetItemDegradationMultiplier(
            string roomId,
            IReadOnlyList<Survivor> survivors)
        {
            if (string.IsNullOrEmpty(roomId) || survivors == null) return 1f;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive || !HasQuartermaster(sv)) continue;
                if (string.Equals(sv.CurrentRoomId, roomId, StringComparison.Ordinal))
                    return QuartermasterDegradationMult;
            }
            return 1f;
        }

        /// <summary>
        /// True when a living Quartermaster is currently in <paramref name="roomId"/>.
        /// </summary>
        public bool IsQuartermasterInRoom(string roomId, IReadOnlyList<Survivor> survivors)
        {
            return GetItemDegradationMultiplier(roomId, survivors) < 1f;
        }

        // ── #213 Taskmaster ──────────────────────────────────────────────

        /// <summary>
        /// Daily tick: survivors with Morale &gt; 90 advance their high-morale
        /// streak; others reset. Grants Taskmaster at 14 consecutive days.
        /// </summary>
        public void TickDailyMorale(IReadOnlyList<Survivor> survivors, int currentDay = 0)
        {
            if (survivors == null) return;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                var c = GetOrCreate(sv.Id);
                if (sv.Needs.Morale > TaskmasterMoraleThreshold)
                {
                    c.HighMoraleDays++;
                    OnMilestoneProgress?.Invoke(sv, "high_morale_days", c.HighMoraleDays);
                    if (c.HighMoraleDays >= HighMoraleDaysForTaskmaster)
                        TryGrant(sv, TaskmasterId, currentDay);
                }
                else
                {
                    c.HighMoraleDays = 0;
                }
            }
        }

        public bool HasTaskmaster(Survivor sv) => Has(sv, TaskmasterId);

        /// <summary>
        /// Pacing Aura: work-rate multiplier for Utility AI craft/repair/dig.
        /// 1.15 when a living Taskmaster is in the same or adjacent room.
        /// </summary>
        public float GetPacingAuraMultiplier(
            Survivor worker,
            IReadOnlyList<Survivor> survivors,
            Func<string, string, bool> areRoomsAdjacent)
        {
            if (worker == null || !worker.IsAlive || survivors == null) return 1f;
            string workerRoom = worker.CurrentRoomId;
            if (string.IsNullOrEmpty(workerRoom)) return 1f;

            for (int i = 0; i < survivors.Count; i++)
            {
                var tm = survivors[i];
                if (tm == null || !tm.IsAlive || !HasTaskmaster(tm)) continue;
                string tmRoom = tm.CurrentRoomId;
                if (string.IsNullOrEmpty(tmRoom)) continue;

                if (string.Equals(tmRoom, workerRoom, StringComparison.Ordinal))
                    return TaskmasterActionSpeedMult;

                if (areRoomsAdjacent != null && areRoomsAdjacent(tmRoom, workerRoom))
                    return TaskmasterActionSpeedMult;
            }
            return 1f;
        }

        // ── Grant helper ─────────────────────────────────────────────────

        private bool TryGrant(Survivor sv, string perkId, int currentDay)
        {
            if (_progression == null || sv == null) return false;
            if (_progression.HasActivePerk(sv.Id, perkId)
                || _progression.HasDormantPerk(sv.Id, perkId))
                return false;

            bool granted = _progression.TryGrantPerk(sv, perkId, currentDay);
            if (granted)
                OnSocialPerkEarned?.Invoke(sv, perkId);
            return granted;
        }

        private SocialCounters GetOrCreate(string survivorId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var c))
            {
                c = new SocialCounters();
                _bySurvivor[survivorId] = c;
            }
            return c;
        }

        // ── Save / Load ──────────────────────────────────────────────────

        public SocialPerkSave CaptureState()
        {
            var save = new SocialPerkSave { Entries = new List<SocialCounterSave>() };
            foreach (var kv in _bySurvivor)
            {
                var c = kv.Value;
                save.Entries.Add(new SocialCounterSave
                {
                    SurvivorId = kv.Key,
                    PeacefulDeEscalations = c.PeacefulDeEscalations,
                    ItemsHauled = c.ItemsHauled,
                    HighMoraleDays = c.HighMoraleDays
                });
            }
            return save;
        }

        public void RestoreState(SocialPerkSave save)
        {
            _bySurvivor.Clear();
            if (save?.Entries == null) return;
            for (int i = 0; i < save.Entries.Count; i++)
            {
                var e = save.Entries[i];
                if (e == null || string.IsNullOrEmpty(e.SurvivorId)) continue;
                _bySurvivor[e.SurvivorId] = new SocialCounters
                {
                    PeacefulDeEscalations = e.PeacefulDeEscalations,
                    ItemsHauled = e.ItemsHauled,
                    HighMoraleDays = e.HighMoraleDays
                };
            }
        }

        public sealed class SocialCounters
        {
            public int PeacefulDeEscalations;
            public int ItemsHauled;
            public int HighMoraleDays;

            public SocialCounters Clone() => new SocialCounters
            {
                PeacefulDeEscalations = PeacefulDeEscalations,
                ItemsHauled = ItemsHauled,
                HighMoraleDays = HighMoraleDays
            };
        }
    }

    [Serializable]
    public class SocialPerkSave
    {
        public List<SocialCounterSave> Entries = new List<SocialCounterSave>();
    }

    [Serializable]
    public class SocialCounterSave
    {
        public string SurvivorId;
        public int PeacefulDeEscalations;
        public int ItemsHauled;
        public int HighMoraleDays;
    }
}
