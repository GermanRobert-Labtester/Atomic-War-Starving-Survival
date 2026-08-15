using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Expansion VII — Internal Mythology. When humans are trapped in a concrete
    /// box for 100 days with no sunlight, they invent ghosts to explain the things
    /// they cannot control. Myths stabilize Morale but cost resources and create
    /// dangerous behavioral blind spots.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class InternalMythologySystem
    {
        // ── Myth ids ──────────────────────────────────────────────────
        public const string Myth_VentWalker = "myth_vent_walker";
        public const string Myth_ThirdHatch = "myth_third_hatch";
        public const string Myth_IronWorm = "myth_iron_worm";
        public const string Myth_AshDevil = "myth_ash_devil";

        // ── Myth effects ──────────────────────────────────────────────
        public const float VentWalker_MoraleStabilization = 5f;
        public const float VentWalker_FoodCostPerDay = 0.5f; // rations "offered" to vents
        public const float VentWalker_InnocenceDrop = -10f;

        public const float ThirdHatch_DelaySeconds = 3f;     // knock-knock-knock ritual
        public const float ThirdHatch_SiegeDeathChance = 0.15f;

        public const float DebunkMoraleDrop = -15f;
        public const float InstitutionalizeRationCost = 1f;

        // ── Events ────────────────────────────────────────────────────
        public event Action<string> OnMythBorn;
        public event Action<string> OnMythInstitutionalized;
        public event Action<string> OnMythDebunked;
        public event Action<string, float> OnMythResourceCost;
        public event Action<string> OnHatchDelayCaused;
        public event Action<string> OnScapegoatFromMyth;

        private readonly Dictionary<string, MythState> _myths = new Dictionary<string, MythState>();
        private readonly System.Random _rng;

        public IReadOnlyDictionary<string, MythState> AllMyths => _myths;
        public bool HasActiveMyth(string mythId) => _myths.TryGetValue(mythId, out var m) && m.IsActive;

        public InternalMythologySystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(6666);
        }

        // ── The Vent Walker ───────────────────────────────────────────

        /// <summary>
        /// The Insomniac hears the AshDriftSystem scraping against the surface
        /// intake grates. It sounds like footsteps. The myth spreads.
        /// </summary>
        public bool TriggerVentWalker(int currentDay)
        {
            if (_myths.ContainsKey(Myth_VentWalker)) return false;

            _myths[Myth_VentWalker] = new MythState
            {
                Id = Myth_VentWalker,
                BornDay = currentDay,
                IsActive = true,
                Description = "The ghost of the bunker's original architect, checking the seals."
            };

            OnMythBorn?.Invoke(Myth_VentWalker);
            return true;
        }

        /// <summary>
        /// Children start leaving food and water near the vents as "offerings."
        /// The Hoarder notices missing food and accuses the Feral Orphan.
        /// </summary>
        public VentWalkerResult ProcessVentWalkerOfferings(string childId, string hoarderId,
            string feralOrphanId)
        {
            var result = new VentWalkerResult();
            var myth = _myths.TryGetValue(Myth_VentWalker, out var m) ? m : null;
            if (myth == null || !myth.IsActive) return result;

            // Children leave food
            myth.DaysActive++;
            OnMythResourceCost?.Invoke(Myth_VentWalker, VentWalker_FoodCostPerDay);

            // Hoarder accuses Feral Orphan
            if (myth.DaysActive >= 3)
            {
                result.HoarderAccusesOrphan = true;
                OnScapegoatFromMyth?.Invoke(feralOrphanId);
            }

            return result;
        }

        // ── The Rule of the Third Hatch ───────────────────────────────

        /// <summary>
        /// A survivor dies during a surface expedition while the hatch is open.
        /// The Superstitious survivor declares the rule.
        /// </summary>
        public bool TriggerThirdHatch(int currentDay)
        {
            if (_myths.ContainsKey(Myth_ThirdHatch)) return false;

            _myths[Myth_ThirdHatch] = new MythState
            {
                Id = Myth_ThirdHatch,
                BornDay = currentDay,
                IsActive = true,
                Description = "The hatch must be knocked on three times before opening, or the Ash Devil follows."
            };

            OnMythBorn?.Invoke(Myth_ThirdHatch);
            return true;
        }

        /// <summary>
        /// Check if the Third Hatch ritual delays hatch response during a siege.
        /// Returns true if the delay causes a casualty.
        /// </summary>
        public bool CheckHatchDelay(string survivorId)
        {
            if (!HasActiveMyth(Myth_ThirdHatch)) return false;

            OnHatchDelayCaused?.Invoke(survivorId);
            return _rng.NextDouble() < ThirdHatch_SiegeDeathChance;
        }

        /// <summary>Get the delay in seconds for hatch operations.</summary>
        public float GetHatchDelay()
        {
            return HasActiveMyth(Myth_ThirdHatch) ? ThirdHatch_DelaySeconds : 0f;
        }

        // ── Myth management ───────────────────────────────────────────

        /// <summary>
        /// Debunk a myth. Crushes the children's coping mechanism.
        /// </summary>
        public bool DebunkMyth(string mythId)
        {
            if (!_myths.TryGetValue(mythId, out var myth) || !myth.IsActive) return false;
            myth.IsActive = false;
            myth.WasDebunked = true;
            OnMythDebunked?.Invoke(mythId);
            return true;
        }

        /// <summary>
        /// Institutionalize a myth. Assign someone to "bless" or manage it.
        /// Stabilizes Morale but costs rations.
        /// </summary>
        public bool InstitutionalizeMyth(string mythId, string assignedSurvivorId)
        {
            if (!_myths.TryGetValue(mythId, out var myth) || !myth.IsActive) return false;
            myth.IsInstitutionalized = true;
            myth.AssignedSurvivorId = assignedSurvivorId;
            OnMythInstitutionalized?.Invoke(mythId);
            return true;
        }

        /// <summary>Get morale stabilization from institutionalized myths.</summary>
        public float GetMoraleStabilization()
        {
            float total = 0f;
            foreach (var kv in _myths)
            {
                if (kv.Value.IsActive && kv.Value.IsInstitutionalized)
                    total += VentWalker_MoraleStabilization;
            }
            return total;
        }

        /// <summary>Get daily ration cost from institutionalized myths.</summary>
        public float GetDailyRationCost()
        {
            float total = 0f;
            foreach (var kv in _myths)
            {
                if (kv.Value.IsActive && kv.Value.IsInstitutionalized)
                    total += InstitutionalizeRationCost;
            }
            return total;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public MythologySave CaptureState()
        {
            var entries = new MythStateSave[_myths.Count];
            int i = 0;
            foreach (var kv in _myths)
            {
                var m = kv.Value;
                entries[i++] = new MythStateSave
                {
                    Id = m.Id,
                    BornDay = m.BornDay,
                    IsActive = m.IsActive,
                    WasDebunked = m.WasDebunked,
                    IsInstitutionalized = m.IsInstitutionalized,
                    AssignedSurvivorId = m.AssignedSurvivorId,
                    DaysActive = m.DaysActive,
                    Description = m.Description
                };
            }
            return new MythologySave { Myths = entries };
        }

        public void RestoreState(MythologySave save)
        {
            _myths.Clear();
            if (save?.Myths == null) return;
            for (int i = 0; i < save.Myths.Length; i++)
            {
                var e = save.Myths[i];
                if (e == null || string.IsNullOrEmpty(e.Id)) continue;
                _myths[e.Id] = new MythState
                {
                    Id = e.Id,
                    BornDay = e.BornDay,
                    IsActive = e.IsActive,
                    WasDebunked = e.WasDebunked,
                    IsInstitutionalized = e.IsInstitutionalized,
                    AssignedSurvivorId = e.AssignedSurvivorId,
                    DaysActive = e.DaysActive,
                    Description = e.Description
                };
            }
        }
    }

    public class MythState
    {
        public string Id;
        public int BornDay;
        public bool IsActive;
        public bool WasDebunked;
        public bool IsInstitutionalized;
        public string AssignedSurvivorId;
        public int DaysActive;
        public string Description;
    }

    [Serializable]
    public class VentWalkerResult
    {
        public bool HoarderAccusesOrphan;
    }

    [Serializable]
    public class MythologySave
    {
        public MythStateSave[] Myths;
    }

    [Serializable]
    public class MythStateSave
    {
        public string Id;
        public int BornDay;
        public bool IsActive;
        public bool WasDebunked;
        public bool IsInstitutionalized;
        public string AssignedSurvivorId;
        public int DaysActive;
        public string Description;
    }
}
