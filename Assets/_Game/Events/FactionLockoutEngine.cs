using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Events
{
    /// <summary>
    /// Expansion VI — The Faction Lockout Engine. You cannot be everyone's savior.
    /// Allying with one faction actively destroys your standing with another,
    /// triggering Hard Lockouts on rival questlines and initiating Retaliatory
    /// Mechanics. Monitors FactionHegemony thresholds and permanently alters
    /// the questline graph.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class FactionLockoutEngine
    {
        // ── Lockout pair definitions ──────────────────────────────────
        public const string Quest_GarrisonCensus = "quest_garrison_census";
        public const string Quest_MilitiaSmugglers = "quest_militia_smugglers_route";
        public const string Quest_CultPurity = "quest_cult_purity";
        public const string Quest_RebuildersThirst = "quest_rebuilders_thirst";

        // ── Hegemony thresholds for lockout ───────────────────────────
        public const int LockoutThreshold = 40;

        // ── Retaliation event ids ─────────────────────────────────────
        public const string Retaliation_RadioArrayShot = "retaliation_radio_array_shot";
        public const string Retaliation_ArtilleryTargeting = "retaliation_artillery_targeting";
        public const string Retaliation_PsychologicalWarfare = "retaliation_psychological_warfare";
        public const string Retaliation_MilitiaEmbargo = "retaliation_militia_embargo";
        public const string Retaliation_GarrisonCheckpoint = "retaliation_garrison_checkpoint";
        public const string Retaliation_RebuildersDeath = "retaliation_rebuilders_death";
        public const string Retaliation_CultHeretics = "retaliation_cult_heretics";

        // ── Retaliation constants ─────────────────────────────────────
        public const float ArtilleryHatchTargetChance = 0.30f;
        public const float PsychWarfareMoraleDrop = 20f;
        public const int RadioArrayRepairHours = 4;
        public const int RebuildersWaterDays = 6;

        // ── Events ────────────────────────────────────────────────────
        public event Action<string, string> OnQuestlineLocked;       // lockedId, reasonId
        public event Action<string> OnRetaliationTriggered;          // retaliationId
        public event Action<string, string> OnLockoutPairResolved;   // winner, loser
        public event Action<string> OnQuestlineCompleted;            // questId

        private readonly HashSet<string> _lockedQuestlines = new HashSet<string>();
        private readonly HashSet<string> _completedQuestlines = new HashSet<string>();
        private readonly List<RetaliationEvent> _retaliationLog = new List<RetaliationEvent>();
        private readonly Dictionary<string, int> _retaliationCounters = new Dictionary<string, int>();

        public IReadOnlyCollection<string> LockedQuestlines => _lockedQuestlines;
        public IReadOnlyCollection<string> CompletedQuestlines => _completedQuestlines;
        public IReadOnlyList<RetaliationEvent> RetaliationLog => _retaliationLog;

        // ── Questline completion with lockout ─────────────────────────

        /// <summary>
        /// Complete a questline and trigger lockout of its rival plus retaliation.
        /// </summary>
        public QuestlineResult CompleteQuestline(string questId, int currentDay)
        {
            if (_completedQuestlines.Contains(questId) || _lockedQuestlines.Contains(questId))
                return new QuestlineResult { Success = false, AlreadyResolved = true };

            _completedQuestlines.Add(questId);
            var result = new QuestlineResult { Success = true, QuestId = questId };

            switch (questId)
            {
                case Quest_GarrisonCensus:
                    result = ResolveGarrisonCensus(currentDay);
                    break;
                case Quest_MilitiaSmugglers:
                    result = ResolveMilitiaSmugglers(currentDay);
                    break;
                case Quest_CultPurity:
                    result = ResolveCultPurity(currentDay);
                    break;
                case Quest_RebuildersThirst:
                    result = ResolveRebuildersThirst(currentDay);
                    break;
            }

            OnQuestlineCompleted?.Invoke(questId);
            return result;
        }

        // ── Questline resolutions ─────────────────────────────────────

        private QuestlineResult ResolveGarrisonCensus(int day)
        {
            // Lock out Militia Smuggler's Route
            LockQuestline(Quest_MilitiaSmugglers, Quest_GarrisonCensus);

            // Militia retaliation: sniper shoots radio array
            TriggerRetaliation(Retaliation_RadioArrayShot, day,
                "The Militia intercepts the broadcast. A sniper shoots your radio array off the surface.");

            // Militia embargo begins
            TriggerRetaliation(Retaliation_MilitiaEmbargo, day,
                "The Militia embargo begins. seed_envelope_wheat and fertilizer removed from traders.");

            return new QuestlineResult
            {
                Success = true,
                QuestId = Quest_GarrisonCensus,
                LockedQuestId = Quest_MilitiaSmugglers,
                RetaliationId = Retaliation_RadioArrayShot,
                RewardDescription = "ammo_762x51_jhp_ap x100, body_armour_military x2, Garrison Hegemony +40.",
                ConsequenceDescription = "Militia sniper destroys radio array. 4-hour surface repair. Militia embargo."
            };
        }

        private QuestlineResult ResolveMilitiaSmugglers(int day)
        {
            // Lock out Garrison Census
            LockQuestline(Quest_GarrisonCensus, Quest_MilitiaSmugglers);

            // Garrison retaliation: artillery targeting
            TriggerRetaliation(Retaliation_ArtilleryTargeting, day,
                "The Garrison triangulates your broadcast. Siege_Artillery now targets your hatch.");

            // Garrison stops sending traders
            TriggerRetaliation(Retaliation_GarrisonCheckpoint, day,
                "The Garrison marks your bunker as a Rebel Sympathizer Node. Passive traders stop coming.");

            return new QuestlineResult
            {
                Success = true,
                QuestId = Quest_MilitiaSmugglers,
                LockedQuestId = Quest_GarrisonCensus,
                RetaliationId = Retaliation_ArtilleryTargeting,
                RewardDescription = "fertilizer x20, vegetable_potato x30, Militia Hegemony +40.",
                ConsequenceDescription = "Garrison artillery targets hatch (30% chance). Garrison traders stop."
            };
        }

        private QuestlineResult ResolveCultPurity(int day)
        {
            // Lock out Rebuilders Thirst
            LockQuestline(Quest_RebuildersThirst, Quest_CultPurity);

            // Rebuilders die in 6 days
            TriggerRetaliation(Retaliation_RebuildersDeath, day,
                "The Rebuilders' settlement will run out of water in 6 days. " +
                "The the_empath intercepts their dying broadcast.");

            return new QuestlineResult
            {
                Success = true,
                QuestId = Quest_CultPurity,
                LockedQuestId = Quest_RebuildersThirst,
                RetaliationId = Retaliation_RebuildersDeath,
                RewardDescription = "HallucinogenicTea x10, Cult Hegemony +50. Cult intercepts Warlord raids.",
                ConsequenceDescription = "Rebuilders die. Empath suffers GriefCascade. May sabotage water purifier."
            };
        }

        private QuestlineResult ResolveRebuildersThirst(int day)
        {
            // Lock out Cult Purity
            LockQuestline(Quest_CultPurity, Quest_RebuildersThirst);

            // Cult psychological warfare
            TriggerRetaliation(Retaliation_PsychologicalWarfare, day,
                "The Cult marks your bunker as Heretics. Mutilated corpses at your hatch. " +
                "They broadcast your survivors' names and pre-war addresses.");

            // Cult heretics designation
            TriggerRetaliation(Retaliation_CultHeretics, day,
                "The Cult designates your bunker as Heretics. Global Morale -20.");

            return new QuestlineResult
            {
                Success = true,
                QuestId = Quest_RebuildersThirst,
                LockedQuestId = Quest_CultPurity,
                RetaliationId = Retaliation_PsychologicalWarfare,
                RewardDescription = "water_purification_tablets x3, Rebuilder Hegemony +50.",
                ConsequenceDescription = "Cult psychological warfare. Corpses at hatch. Names broadcast. Morale -20."
            };
        }

        // ── Lockout helpers ───────────────────────────────────────────

        private void LockQuestline(string questId, string reasonId)
        {
            if (_lockedQuestlines.Add(questId))
            {
                OnQuestlineLocked?.Invoke(questId, reasonId);
                OnLockoutPairResolved?.Invoke(
                    _completedQuestlines.Contains(Quest_GarrisonCensus) || _completedQuestlines.Contains(Quest_MilitiaSmugglers)
                        ? "faction" : "faction",
                    questId);
            }
        }

        private void TriggerRetaliation(string retaliationId, int day, string description)
        {
            _retaliationLog.Add(new RetaliationEvent
            {
                RetaliationId = retaliationId,
                Day = day,
                Description = description
            });

            _retaliationCounters.TryGetValue(retaliationId, out var count);
            _retaliationCounters[retaliationId] = count + 1;

            OnRetaliationTriggered?.Invoke(retaliationId);
        }

        // ── Retaliation queries ───────────────────────────────────────

        public bool IsQuestlineLocked(string questId) => _lockedQuestlines.Contains(questId);
        public bool IsQuestlineCompleted(string questId) => _completedQuestlines.Contains(questId);

        public int GetRetaliationCount(string retaliationId)
        {
            return _retaliationCounters.TryGetValue(retaliationId, out var c) ? c : 0;
        }

        public bool HasRetaliation(string retaliationId) => GetRetaliationCount(retaliationId) > 0;

        /// <summary>Check if artillery should target the hatch this siege.</summary>
        public bool ShouldArtilleryTargetHatch(System.Random rng)
        {
            if (!HasRetaliation(Retaliation_ArtilleryTargeting)) return false;
            return rng.NextDouble() < ArtilleryHatchTargetChance;
        }

        /// <summary>Check if traders have been blocked.</summary>
        public bool AreTradersBlocked(string factionId)
        {
            if (factionId == "faction_upland_militia" && HasRetaliation(Retaliation_MilitiaEmbargo))
                return true;
            if (factionId == "faction_central_garrison" && HasRetaliation(Retaliation_GarrisonCheckpoint))
                return true;
            return false;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public FactionLockoutSave CaptureState()
        {
            var locked = new string[_lockedQuestlines.Count];
            _lockedQuestlines.CopyTo(locked);
            var completed = new string[_completedQuestlines.Count];
            _completedQuestlines.CopyTo(completed);
            var log = new RetaliationEventSave[_retaliationLog.Count];
            for (int i = 0; i < _retaliationLog.Count; i++)
            {
                log[i] = new RetaliationEventSave
                {
                    RetaliationId = _retaliationLog[i].RetaliationId,
                    Day = _retaliationLog[i].Day,
                    Description = _retaliationLog[i].Description
                };
            }
            var counters = new RetaliationCounterSave[_retaliationCounters.Count];
            int j = 0;
            foreach (var kv in _retaliationCounters)
                counters[j++] = new RetaliationCounterSave { Id = kv.Key, Count = kv.Value };

            return new FactionLockoutSave
            {
                LockedQuestlines = locked,
                CompletedQuestlines = completed,
                RetaliationLog = log,
                RetaliationCounters = counters
            };
        }

        public void RestoreState(FactionLockoutSave save)
        {
            _lockedQuestlines.Clear();
            _completedQuestlines.Clear();
            _retaliationLog.Clear();
            _retaliationCounters.Clear();
            if (save == null) return;
            if (save.LockedQuestlines != null)
                for (int i = 0; i < save.LockedQuestlines.Length; i++)
                    if (!string.IsNullOrEmpty(save.LockedQuestlines[i]))
                        _lockedQuestlines.Add(save.LockedQuestlines[i]);
            if (save.CompletedQuestlines != null)
                for (int i = 0; i < save.CompletedQuestlines.Length; i++)
                    if (!string.IsNullOrEmpty(save.CompletedQuestlines[i]))
                        _completedQuestlines.Add(save.CompletedQuestlines[i]);
            if (save.RetaliationLog != null)
                for (int i = 0; i < save.RetaliationLog.Length; i++)
                    if (save.RetaliationLog[i] != null)
                        _retaliationLog.Add(new RetaliationEvent
                        {
                            RetaliationId = save.RetaliationLog[i].RetaliationId,
                            Day = save.RetaliationLog[i].Day,
                            Description = save.RetaliationLog[i].Description
                        });
            if (save.RetaliationCounters != null)
                for (int i = 0; i < save.RetaliationCounters.Length; i++)
                    if (save.RetaliationCounters[i] != null)
                        _retaliationCounters[save.RetaliationCounters[i].Id] = save.RetaliationCounters[i].Count;
        }
    }

    public class RetaliationEvent
    {
        public string RetaliationId;
        public int Day;
        public string Description;
    }

    [Serializable]
    public class QuestlineResult
    {
        public bool Success;
        public bool AlreadyResolved;
        public string QuestId;
        public string LockedQuestId;
        public string RetaliationId;
        public string RewardDescription;
        public string ConsequenceDescription;
    }

    [Serializable]
    public class FactionLockoutSave
    {
        public string[] LockedQuestlines;
        public string[] CompletedQuestlines;
        public RetaliationEventSave[] RetaliationLog;
        public RetaliationCounterSave[] RetaliationCounters;
    }

    [Serializable]
    public class RetaliationEventSave
    {
        public string RetaliationId;
        public int Day;
        public string Description;
    }

    [Serializable]
    public class RetaliationCounterSave
    {
        public string Id;
        public int Count;
    }
}
