#pragma warning disable CS0067 // Public API event surface; subscribers arrive with feature wiring
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion V — Faction Doctrines. There is no XP bar. Progression is handled
    /// by adopting Doctrines: permanent faction alignments that grant passive buffs
    /// and lock you out of opposing Doctrines. Every Doctrine requires a sacrifice.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class DoctrineSystem
    {
        // ── Doctrine ids ──────────────────────────────────────────────
        public const string Doctrine_Iron = "doctrine_iron";           // Garrison
        public const string Doctrine_Roots = "doctrine_roots";         // Militia
        public const string Doctrine_Glow = "doctrine_glow";           // Cult
        public const string Doctrine_Toll = "doctrine_toll";           // Warlords
        public const string Doctrine_Ghost = "doctrine_ghost";         // Pariah

        // ── Doctrine conflicts (mutually exclusive) ───────────────────
        public static readonly string[][] DoctrineConflicts = new string[][]
        {
            new[] { Doctrine_Iron, Doctrine_Roots },    // Garrison vs Militia
            new[] { Doctrine_Iron, Doctrine_Glow },     // Garrison vs Cult
            new[] { Doctrine_Roots, Doctrine_Glow },    // Militia vs Cult
            new[] { Doctrine_Toll, Doctrine_Ghost },    // Warlords vs Pariah
        };

        // ── Unlock requirement constants ──────────────────────────────
        // Doctrine of Iron: Execute 3 prisoners; surrender 500 rounds
        public const int Iron_PrisonersExecuted = 3;
        public const int Iron_AmmoSurrendered = 500;

        // Doctrine of Roots: Sabotage 2 garrison convoys; hoard 100kg grain
        public const int Roots_ConvoysSabotaged = 2;
        public const float Roots_GrainHoardKg = 100f;

        // Doctrine of the Glow: Drink irradiated water 5 times; refuse treatment for dying
        public const int Glow_IrradiatedDrinks = 5;
        public const int Glow_TreatmentRefusals = 1;

        // Doctrine of the Toll: Extort 3 shelters; sell survivor into slavery
        public const int Toll_Extortions = 3;
        public const int Toll_SlavesSold = 1;

        // Doctrine of the Ghost: -100 hegemony with ALL factions
        // (checked via WorldStateConsequenceSystem.IsPariah)

        // ── Passive buff constants ────────────────────────────────────
        public const float Iron_MoraleLossReduction = 0.50f;    // 50% less morale loss during breaches
        public const float Roots_CalorieReduction = 0.20f;      // 20% fewer calories needed
        public const float Glow_RadThresholdMultiplier = 2.0f;  // Rad sickness thresholds doubled
        public const float Toll_FleeChance = 0.30f;             // 30% encounter flee chance
        public const float Ghost_NoiseReduction = 0.80f;        // 80% noise reduction

        // ── Hidden cost constants ─────────────────────────────────────
        public const string Iron_LocksOut = "peace_talks_militia";
        public const string Roots_LocksOut = "body_armour_military";
        public const string Glow_AfflictionId = "affliction_rad_hallucinations";
        public const string Toll_TargetFaction = "faction_black_ops";
        public const bool Ghost_NoTraderAccess = true;

        // ── Events ────────────────────────────────────────────────────
        public event Action<string> OnDoctrineUnlocked;
        public event Action<string> OnDoctrineAdopted;
        public event Action<string, string> OnDoctrineConflict;  // adoptedId, lockedOutId
        public event Action<string> OnHiddenCostApplied;

        private readonly Dictionary<string, DoctrineState> _doctrines = new Dictionary<string, DoctrineState>();
        private string _adoptedDoctrineId;
        private readonly Dictionary<string, int> _unlockCounters = new Dictionary<string, int>();

        public string AdoptedDoctrineId => _adoptedDoctrineId;
        public bool HasAdoptedDoctrine => !string.IsNullOrEmpty(_adoptedDoctrineId);
        public IReadOnlyDictionary<string, DoctrineState> AllDoctrines => _doctrines;

        public DoctrineSystem()
        {
            // Initialize all doctrines as locked
            _doctrines[Doctrine_Iron] = new DoctrineState { Id = Doctrine_Iron, Faction = "Garrison" };
            _doctrines[Doctrine_Roots] = new DoctrineState { Id = Doctrine_Roots, Faction = "Militia" };
            _doctrines[Doctrine_Glow] = new DoctrineState { Id = Doctrine_Glow, Faction = "Cult" };
            _doctrines[Doctrine_Toll] = new DoctrineState { Id = Doctrine_Toll, Faction = "Warlords" };
            _doctrines[Doctrine_Ghost] = new DoctrineState { Id = Doctrine_Ghost, Faction = "Pariah" };
        }

        // ── Unlock requirement tracking ───────────────────────────────

        public void RecordPrisonerExecuted()
        {
            IncrementCounter("prisoners_executed");
            CheckIronUnlock();
        }

        public void RecordAmmoSurrendered(int rounds)
        {
            AddToCounter("ammo_surrendered", rounds);
            CheckIronUnlock();
        }

        public void RecordConvoySabotaged()
        {
            IncrementCounter("convoys_sabotaged");
            CheckRootsUnlock();
        }

        public void RecordGrainHoard(float kg)
        {
            AddToCounterFloat("grain_hoarded_kg", kg);
            CheckRootsUnlock();
        }

        public void RecordIrradiatedDrink()
        {
            IncrementCounter("irradiated_drinks");
            CheckGlowUnlock();
        }

        public void RecordTreatmentRefusal()
        {
            IncrementCounter("treatment_refusals");
            CheckGlowUnlock();
        }

        public void RecordExtortion()
        {
            IncrementCounter("extortions");
            CheckTollUnlock();
        }

        public void RecordSlaveSold()
        {
            IncrementCounter("slaves_sold");
            CheckTollUnlock();
        }

        public void CheckGhostUnlock(int garrisonHegemony, int militiaHegemony, int cultHegemony)
        {
            if (garrisonHegemony <= -100 && militiaHegemony <= -100 && cultHegemony <= -100)
            {
                if (_doctrines[Doctrine_Ghost].State != DoctrineProgressState.Unlocked)
                {
                    _doctrines[Doctrine_Ghost].State = DoctrineProgressState.Unlocked;
                    OnDoctrineUnlocked?.Invoke(Doctrine_Ghost);
                }
            }
        }

        // ── Unlock checks ─────────────────────────────────────────────

        private void CheckIronUnlock()
        {
            int prisoners = GetCounter("prisoners_executed");
            int ammo = GetCounter("ammo_surrendered");
            if (prisoners >= Iron_PrisonersExecuted && ammo >= Iron_AmmoSurrendered)
            {
                _doctrines[Doctrine_Iron].State = DoctrineProgressState.Unlocked;
                OnDoctrineUnlocked?.Invoke(Doctrine_Iron);
            }
        }

        private void CheckRootsUnlock()
        {
            int convoys = GetCounter("convoys_sabotaged");
            float grain = GetCounterFloat("grain_hoarded_kg");
            if (convoys >= Roots_ConvoysSabotaged && grain >= Roots_GrainHoardKg)
            {
                _doctrines[Doctrine_Roots].State = DoctrineProgressState.Unlocked;
                OnDoctrineUnlocked?.Invoke(Doctrine_Roots);
            }
        }

        private void CheckGlowUnlock()
        {
            int drinks = GetCounter("irradiated_drinks");
            int refusals = GetCounter("treatment_refusals");
            if (drinks >= Glow_IrradiatedDrinks && refusals >= Glow_TreatmentRefusals)
            {
                _doctrines[Doctrine_Glow].State = DoctrineProgressState.Unlocked;
                OnDoctrineUnlocked?.Invoke(Doctrine_Glow);
            }
        }

        private void CheckTollUnlock()
        {
            int extortions = GetCounter("extortions");
            int slaves = GetCounter("slaves_sold");
            if (extortions >= Toll_Extortions && slaves >= Toll_SlavesSold)
            {
                _doctrines[Doctrine_Toll].State = DoctrineProgressState.Unlocked;
                OnDoctrineUnlocked?.Invoke(Doctrine_Toll);
            }
        }

        // ── Doctrine adoption ─────────────────────────────────────────

        /// <summary>
        /// Adopt a doctrine. Permanently locks out conflicting doctrines.
        /// Only one doctrine can be adopted per run.
        /// </summary>
        public bool AdoptDoctrine(string doctrineId)
        {
            if (HasAdoptedDoctrine) return false;
            if (!_doctrines.TryGetValue(doctrineId, out var doctrine)) return false;
            if (doctrine.State != DoctrineProgressState.Unlocked) return false;

            _adoptedDoctrineId = doctrineId;
            doctrine.State = DoctrineProgressState.Adopted;

            // Lock out conflicting doctrines
            for (int i = 0; i < DoctrineConflicts.Length; i++)
            {
                var conflict = DoctrineConflicts[i];
                if (conflict[0] == doctrineId && _doctrines.TryGetValue(conflict[1], out var locked))
                {
                    locked.State = DoctrineProgressState.LockedOut;
                    OnDoctrineConflict?.Invoke(doctrineId, conflict[1]);
                }
                else if (conflict[1] == doctrineId && _doctrines.TryGetValue(conflict[0], out var locked2))
                {
                    locked2.State = DoctrineProgressState.LockedOut;
                    OnDoctrineConflict?.Invoke(doctrineId, conflict[0]);
                }
            }

            OnDoctrineAdopted?.Invoke(doctrineId);
            return true;
        }

        // ── Passive buff queries ──────────────────────────────────────

        /// <summary>Get morale loss multiplier during hatch breaches.</summary>
        public float GetHatchBreachMoraleMultiplier()
        {
            return _adoptedDoctrineId == Doctrine_Iron ? Iron_MoraleLossReduction : 1f;
        }

        /// <summary>Get calorie requirement multiplier.</summary>
        public float GetCalorieMultiplier()
        {
            return _adoptedDoctrineId == Doctrine_Roots ? (1f - Roots_CalorieReduction) : 1f;
        }

        /// <summary>Get radiation sickness threshold multiplier.</summary>
        public float GetRadThresholdMultiplier()
        {
            return _adoptedDoctrineId == Doctrine_Glow ? Glow_RadThresholdMultiplier : 1f;
        }

        /// <summary>Get encounter flee chance from Toll doctrine.</summary>
        public float GetEncounterFleeChance()
        {
            return _adoptedDoctrineId == Doctrine_Toll ? Toll_FleeChance : 0f;
        }

        /// <summary>Get noise reduction multiplier from Ghost doctrine.</summary>
        public float GetNoiseMultiplier()
        {
            return _adoptedDoctrineId == Doctrine_Ghost ? (1f - Ghost_NoiseReduction) : 1f;
        }

        /// <summary>True if the adopted doctrine locks out peace talks with a faction.</summary>
        public bool LocksOutPeaceTalks(string factionId)
        {
            return _adoptedDoctrineId == Doctrine_Iron && factionId == "faction_upland_militia";
        }

        /// <summary>True if the adopted doctrine prevents trader access.</summary>
        public bool BlocksTraderAccess()
        {
            return _adoptedDoctrineId == Doctrine_Ghost;
        }

        /// <summary>True if survivors periodically hallucinate (Glow doctrine).</summary>
        public bool CausesPeriodicHallucinations()
        {
            return _adoptedDoctrineId == Doctrine_Glow;
        }

        /// <summary>True if black ops hunters are active (Toll doctrine).</summary>
        public bool AttractsBlackOpsHunters()
        {
            return _adoptedDoctrineId == Doctrine_Toll;
        }

        // ── Counter helpers ───────────────────────────────────────────

        private void IncrementCounter(string key)
        {
            _unlockCounters.TryGetValue(key, out var v);
            _unlockCounters[key] = v + 1;
        }

        private void AddToCounter(string key, int amount)
        {
            _unlockCounters.TryGetValue(key, out var v);
            _unlockCounters[key] = v + amount;
        }

        private int GetCounter(string key)
        {
            return _unlockCounters.TryGetValue(key, out var v) ? v : 0;
        }

        private void AddToCounterFloat(string key, float amount)
        {
            // Store as int (×10 for precision)
            string intKey = key + "_x10";
            _unlockCounters.TryGetValue(intKey, out var v);
            _unlockCounters[intKey] = v + Mathf.RoundToInt(amount * 10f);
        }

        private float GetCounterFloat(string key)
        {
            return GetCounter(key + "_x10") / 10f;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public DoctrineSave CaptureState()
        {
            var entries = new DoctrineStateSave[_doctrines.Count];
            int i = 0;
            foreach (var kv in _doctrines)
            {
                entries[i++] = new DoctrineStateSave
                {
                    Id = kv.Value.Id,
                    State = kv.Value.State,
                    Faction = kv.Value.Faction
                };
            }

            var counters = new CounterSave[_unlockCounters.Count];
            int j = 0;
            foreach (var kv in _unlockCounters)
                counters[j++] = new CounterSave { Key = kv.Key, Value = kv.Value };

            return new DoctrineSave
            {
                AdoptedDoctrineId = _adoptedDoctrineId,
                Doctrines = entries,
                Counters = counters
            };
        }

        public void RestoreState(DoctrineSave save)
        {
            _doctrines.Clear();
            _adoptedDoctrineId = null;
            _unlockCounters.Clear();
            if (save == null) return;
            _adoptedDoctrineId = save.AdoptedDoctrineId;
            if (save.Doctrines != null)
                for (int i = 0; i < save.Doctrines.Length; i++)
                    if (save.Doctrines[i] != null)
                        _doctrines[save.Doctrines[i].Id] = new DoctrineState
                        {
                            Id = save.Doctrines[i].Id,
                            State = save.Doctrines[i].State,
                            Faction = save.Doctrines[i].Faction
                        };
            if (save.Counters != null)
                for (int i = 0; i < save.Counters.Length; i++)
                    if (save.Counters[i] != null)
                        _unlockCounters[save.Counters[i].Key] = save.Counters[i].Value;
        }
    }

    public enum DoctrineProgressState
    {
        Locked,      // Requirements not met
        Unlocked,    // Requirements met, can adopt
        Adopted,     // Active doctrine
        LockedOut    // Permanently locked by conflicting doctrine
    }

    public class DoctrineState
    {
        public string Id;
        public string Faction;
        public DoctrineProgressState State;
    }

    [Serializable]
    public class DoctrineSave
    {
        public string AdoptedDoctrineId;
        public DoctrineStateSave[] Doctrines;
        public CounterSave[] Counters;
    }

    [Serializable]
    public class DoctrineStateSave
    {
        public string Id;
        public DoctrineProgressState State;
        public string Faction;
    }

    [Serializable]
    public class CounterSave
    {
        public string Key;
        public int Value;
    }
}
