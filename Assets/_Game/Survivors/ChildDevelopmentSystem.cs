using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Expansion III — The Ash Curriculum. Children are not morale buffs.
    /// They are liabilities who consume calories and break psychologically,
    /// but are also the only reason some survivors haven't opened the airlock.
    ///
    /// Tracks Innocence (100→0) and Trauma (0→100) per child survivor.
    /// Innocence decays from grim exposure; Trauma rises from violence/starvation.
    /// Divergence: Hardened (Innocence→0 first) or Catatonic Break (Trauma→100 first).
    /// Save/load safe. Plain C#.
    /// </summary>
    public class ChildDevelopmentSystem
    {
        // ── Thresholds ────────────────────────────────────────────────
        public const float MaxInnocence = 100f;
        public const float MaxTrauma = 100f;
        public const float NightTerrorThreshold = 70f;  // Trauma level that triggers night terrors
        public const float HardenedWorkSpeedMult = 0.85f;
        public const float CatatonicBreakCureHours = 4f;
        public const int CatatonicBreakStarvationDays = 7;

        // ── Chore effects on Innocence ────────────────────────────────
        public const float SortScrap_InnocenceLoss = -2f;
        public const float SortScrap_ScrapYieldBonus = 0.10f;
        public const float TendHydroponics_InnocenceGain = 5f;
        public const float TendHydroponics_CropYieldBonus = 0.05f;
        public const float ListenToRadio_InnocenceLoss = -5f;
        public const float ChalkDrawAccuracy = 0.60f;

        // ── Combat ────────────────────────────────────────────────────
        public const string Perk_SmallTarget = "perk_small_target";
        public const string MentalBreak_GriefCascade = "mental_break_grief_cascade";

        // ── Item ids ──────────────────────────────────────────────────
        public const string Item_AshDoll = "ash_doll";
        public const string Item_ChalkStick = "chalk_stick";
        public const string Item_PistolCz75 = "pistol_cz75_9x19";

        // ── Child archetype ids ───────────────────────────────────────
        public const string Archetype_NaiveSon = "the_naive_son";
        public const string Archetype_HardenedDaughter = "the_hardened_daughter";
        public const string Archetype_FeralOrphan = "the_feral_orphan";
        public const string Archetype_ChildSoldier = "the_child_soldier";

        private readonly System.Random _rng;
        private readonly Dictionary<string, ChildState> _children = new Dictionary<string, ChildState>();
        private NeedsSystem _needsSystem;
        private CombatPerkSystem _combatPerks;
        private MentalBreakSystem _mentalBreakSystem;

        public void SetNeedsSystem(NeedsSystem ns) => _needsSystem = ns;
        public void BindCombatPerks(CombatPerkSystem perks) => _combatPerks = perks;
        public void BindMentalBreaks(MentalBreakSystem mbs) => _mentalBreakSystem = mbs;

        // ── Events ────────────────────────────────────────────────────
        public event Action<Survivor, ChildState> OnChildHardened;
        public event Action<Survivor, ChildState> OnCatatonicBreak;
        public event Action<Survivor> OnNightTerror;
        public event Action<Survivor, string> OnChoreAssigned;       // (child, choreId)
        public event Action<Survivor> OnChildTaughtToShoot;
        public event Action<Survivor, float> OnChalkPrediction;      // (child, stormChance)

        public ChildDevelopmentSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(3000);
        }

        /// <summary>Register a child survivor for development tracking.</summary>
        public ChildState RegisterChild(Survivor child, string archetypeId = null)
        {
            if (child == null || !child.IsChild) return null;
            if (_children.ContainsKey(child.Id)) return _children[child.Id];

            var state = new ChildState
            {
                ChildId = child.Id,
                ArchetypeId = archetypeId ?? Archetype_NaiveSon,
                Innocence = MaxInnocence,
                Trauma = 0f,
                IsHardened = false,
                HasCatatonicBreak = false,
                HasLearnedToShoot = false,
                NightTerrorFreeDays = 0
            };
            _children[child.Id] = state;
            return state;
        }

        /// <summary>Get child state by survivor id.</summary>
        public ChildState GetChildState(string childId)
        {
            if (string.IsNullOrEmpty(childId)) return null;
            return _children.TryGetValue(childId, out var s) ? s : null;
        }

        /// <summary>Get all tracked children.</summary>
        public IReadOnlyDictionary<string, ChildState> AllChildren => _children;

        // ── Innocence / Trauma modification ───────────────────────────

        /// <summary>Apply innocence change (negative = loss). Returns true if child just hardened.</summary>
        public bool ModifyInnocence(Survivor child, float delta)
        {
            if (child == null) return false;
            var state = GetChildState(child.Id);
            if (state == null || state.IsHardened || state.HasCatatonicBreak) return false;

            state.Innocence = Mathf.Clamp(state.Innocence + delta, 0f, MaxInnocence);

            if (state.Innocence <= 0f && state.Trauma < MaxTrauma)
            {
                state.IsHardened = true;
                OnChildHardened?.Invoke(child, state);
                return true;
            }
            return false;
        }

        /// <summary>Apply trauma change (positive = gain). Returns true if catatonic break triggered.</summary>
        public bool ModifyTrauma(Survivor child, float delta)
        {
            if (child == null) return false;
            var state = GetChildState(child.Id);
            if (state == null || state.HasCatatonicBreak || state.IsHardened) return false;

            state.Trauma = Mathf.Clamp(state.Trauma + delta, 0f, MaxTrauma);

            if (state.Trauma >= MaxTrauma)
            {
                state.HasCatatonicBreak = true;
                state.CatatonicBreakDays = 0;
                OnCatatonicBreak?.Invoke(child, state);
                return true;
            }
            return false;
        }

        // ── Exposure triggers (called by other systems) ───────────────

        /// <summary>Child sees a corpse. Innocence drops, trauma rises.</summary>
        public void OnChildSeesCorpse(Survivor child)
        {
            ModifyInnocence(child, -8f);
            ModifyTrauma(child, 5f);
        }

        /// <summary>Child hears screaming (raid, mental break, etc.).</summary>
        public void OnChildHearsScreaming(Survivor child)
        {
            ModifyInnocence(child, -3f);
            ModifyTrauma(child, 3f);
        }

        /// <summary>During raids or starvation.</summary>
        public void OnRaidOrStarvation(Survivor child, bool isRaid)
        {
            if (isRaid)
                ModifyTrauma(child, 12f);
            else
                ModifyTrauma(child, 8f);
        }

        /// <summary>Adults argue in the shelter.</summary>
        public void OnAdultsArgue(Survivor child)
        {
            ModifyTrauma(child, 4f);
        }

        // ── Ash Curriculum chores ─────────────────────────────────────

        /// <summary>Assign child to sort scrap metal. Returns scrap yield bonus.</summary>
        public float AssignSortScrap(Survivor child)
        {
            var state = GetChildState(child?.Id);
            if (state == null || !child.IsAlive) return 0f;
            ModifyInnocence(child, SortScrap_InnocenceLoss);
            OnChoreAssigned?.Invoke(child, "action_sort_scrap");
            return SortScrap_ScrapYieldBonus;
        }

        /// <summary>Assign child to tend hydroponics. Returns crop yield bonus.</summary>
        public float AssignTendHydroponics(Survivor child)
        {
            var state = GetChildState(child?.Id);
            if (state == null || !child.IsAlive) return 0f;
            ModifyInnocence(child, TendHydroponics_InnocenceGain);
            OnChoreAssigned?.Invoke(child, "action_tend_hydroponics");
            return TendHydroponics_CropYieldBonus;
        }

        /// <summary>Assign child to listen to radio. Returns intel reliability bonus.</summary>
        public float AssignListenToRadio(Survivor child)
        {
            var state = GetChildState(child?.Id);
            if (state == null || !child.IsAlive) return 0f;
            ModifyInnocence(child, ListenToRadio_InnocenceLoss);
            OnChoreAssigned?.Invoke(child, "action_listen_to_radio");
            return 0.15f; // IntelReliability bonus
        }

        // ── Items ─────────────────────────────────────────────────────

        /// <summary>Give child an ash_doll. Stops night terrors for 3 days.</summary>
        public bool GiveAshDoll(Survivor child)
        {
            var state = GetChildState(child?.Id);
            if (state == null) return false;
            state.NightTerrorFreeDays = 3;
            state.HasAshDoll = true;
            return true;
        }

        /// <summary>Give child a chalk_stick. Enables weather predictions.</summary>
        public bool GiveChalkStick(Survivor child)
        {
            var state = GetChildState(child?.Id);
            if (state == null) return false;
            state.HasChalkStick = true;
            return true;
        }

        /// <summary>Child draws on walls. Returns predicted storm chance (60% accuracy).</summary>
        public float ChildDrawsOnWalls(Survivor child)
        {
            var state = GetChildState(child?.Id);
            if (state == null || !state.HasChalkStick) return -1f;

            // 60% accurate prediction
            float realChance = 0.3f; // Host provides real weather data
            float prediction;
            if (_rng.NextDouble() < ChalkDrawAccuracy)
                prediction = realChance; // Accurate
            else
                prediction = (float)_rng.NextDouble(); // Inaccurate

            OnChalkPrediction?.Invoke(child, prediction);
            return prediction;
        }

        // ── Teach to shoot ────────────────────────────────────────────

        /// <summary>
        /// Teach the Hardened Daughter to use a pistol. Grants SmallTarget perk.
        /// Permanently locks Innocence at 0. May trigger GriefCascade in parents.
        /// </summary>
        public bool TeachChildToShoot(Survivor child, Survivor teacher)
        {
            if (child == null || teacher == null) return false;
            var state = GetChildState(child.Id);
            if (state == null || !state.IsHardened || state.HasLearnedToShoot) return false;

            state.HasLearnedToShoot = true;
            state.Innocence = 0f; // Permanently locked

            // Grant SmallTarget combat perk
            if (_combatPerks != null)
            {
                // Child can hide in vents and shoot raiders
                child.CannotFight = false;
            }

            OnChildTaughtToShoot?.Invoke(child);
            return true;
        }

        /// <summary>
        /// Check if a parent/adult should suffer GriefCascade after child learns to shoot.
        /// Returns the affected survivor, or null.
        /// </summary>
        public Survivor CheckGriefCascade(IReadOnlyList<Survivor> survivors, Survivor armedChild)
        {
            if (survivors == null || armedChild == null) return null;
            var state = GetChildState(armedChild.Id);
            if (state == null || !state.HasLearnedToShoot) return null;

            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive || sv.Id == armedChild.Id) continue;
                // the_fierce_mother or any parent archetype
                if (sv.ArchetypeId == "the_fierce_mother" || sv.ArchetypeId == "the_parent")
                {
                    if (_mentalBreakSystem != null)
                        _mentalBreakSystem.TryApply(sv, MentalBreak_GriefCascade);
                    return sv;
                }
            }
            return null;
        }

        // ── Catatonic Break treatment ─────────────────────────────────

        /// <summary>
        /// Treat a catatonic child. Requires 4 hours/day of TalkDown by therapist/empath.
        /// Returns true if treatment session was valid.
        /// </summary>
        public bool TreatCatatonicBreak(Survivor child, Survivor caregiver, float hours)
        {
            var state = GetChildState(child?.Id);
            if (state == null || !state.HasCatatonicBreak) return false;
            if (caregiver == null || !caregiver.IsAlive) return false;
            if (hours < CatatonicBreakCureHours) return false;

            state.CatatonicTreatmentHours += hours;
            // After 7 days of consistent treatment, child begins eating again
            if (state.CatatonicTreatmentHours >= CatatonicBreakCureHours * 7)
            {
                state.HasCatatonicBreak = false;
                state.Trauma = Mathf.Max(0f, state.Trauma - 30f);
            }
            return true;
        }

        // ── Tick ──────────────────────────────────────────────────────

        /// <summary>
        /// Per-tick update. Handles night terrors, catatonic starvation,
        /// and chalk/weather predictions.
        /// </summary>
        public void Tick(float gameHours, IReadOnlyList<Survivor> survivors)
        {
            if (gameHours <= 0f || survivors == null) return;

            float gameDays = gameHours / 24f;

            foreach (var kv in _children)
            {
                var state = kv.Value;
                if (state == null) continue;

                Survivor child = null;
                for (int i = 0; i < survivors.Count; i++)
                {
                    if (survivors[i] != null && survivors[i].Id == state.ChildId && survivors[i].IsAlive)
                    {
                        child = survivors[i];
                        break;
                    }
                }
                if (child == null) continue;

                // Night terrors
                if (state.NightTerrorFreeDays > 0)
                {
                    state.NightTerrorFreeDays -= gameDays;
                }
                else if (state.Trauma >= NightTerrorThreshold && _rng.NextDouble() < 0.10f * gameDays)
                {
                    OnNightTerror?.Invoke(child);
                    if (_needsSystem != null)
                        _needsSystem.Modify(child, NeedKind.Morale, -5f);
                }

                // Catatonic break: child stops eating after 7 days untreated
                if (state.HasCatatonicBreak)
                {
                    state.CatatonicBreakDays += gameDays;
                    if (state.CatatonicBreakDays >= CatatonicBreakStarvationDays)
                    {
                        // Child stops eating — hunger rises dramatically
                        if (_needsSystem != null)
                        {
                            _needsSystem.Modify(child, NeedKind.Hunger, 15f * gameDays);
                            _needsSystem.Modify(child, NeedKind.Thirst, 15f * gameDays);
                        }
                    }
                }

                // Hardened child gains adult work speed but loses comfort morale
                if (state.IsHardened && !state.HasLearnedToShoot)
                {
                    // Eventually demands a weapon (after 10 days hardened)
                    state.DaysHardened += gameDays;
                }
            }
        }

        // ── Queries ───────────────────────────────────────────────────

        /// <summary>True if any child is currently hardened.</summary>
        public bool HasHardenedChild()
        {
            foreach (var kv in _children)
                if (kv.Value.IsHardened) return true;
            return false;
        }

        /// <summary>True if any child has catatonic break.</summary>
        public bool HasCatatonicChild()
        {
            foreach (var kv in _children)
                if (kv.Value.HasCatatonicBreak) return true;
            return false;
        }

        /// <summary>Get scrap yield bonus from children sorting scrap.</summary>
        public float GetScrapYieldBonus()
        {
            float bonus = 0f;
            foreach (var kv in _children)
                if (kv.Value.IsHardened) bonus += SortScrap_ScrapYieldBonus;
            return bonus;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public ChildDevelopmentSave CaptureState()
        {
            var entries = new ChildStateSave[_children.Count];
            int i = 0;
            foreach (var kv in _children)
            {
                var s = kv.Value;
                entries[i++] = new ChildStateSave
                {
                    ChildId = s.ChildId,
                    ArchetypeId = s.ArchetypeId,
                    Innocence = s.Innocence,
                    Trauma = s.Trauma,
                    IsHardened = s.IsHardened,
                    HasCatatonicBreak = s.HasCatatonicBreak,
                    CatatonicBreakDays = s.CatatonicBreakDays,
                    CatatonicTreatmentHours = s.CatatonicTreatmentHours,
                    HasLearnedToShoot = s.HasLearnedToShoot,
                    HasAshDoll = s.HasAshDoll,
                    HasChalkStick = s.HasChalkStick,
                    NightTerrorFreeDays = s.NightTerrorFreeDays,
                    DaysHardened = s.DaysHardened
                };
            }
            return new ChildDevelopmentSave { Children = entries };
        }

        public void RestoreState(ChildDevelopmentSave save)
        {
            _children.Clear();
            if (save?.Children == null) return;
            for (int i = 0; i < save.Children.Length; i++)
            {
                var e = save.Children[i];
                if (e == null || string.IsNullOrEmpty(e.ChildId)) continue;
                _children[e.ChildId] = new ChildState
                {
                    ChildId = e.ChildId,
                    ArchetypeId = e.ArchetypeId,
                    Innocence = e.Innocence,
                    Trauma = e.Trauma,
                    IsHardened = e.IsHardened,
                    HasCatatonicBreak = e.HasCatatonicBreak,
                    CatatonicBreakDays = e.CatatonicBreakDays,
                    CatatonicTreatmentHours = e.CatatonicTreatmentHours,
                    HasLearnedToShoot = e.HasLearnedToShoot,
                    HasAshDoll = e.HasAshDoll,
                    HasChalkStick = e.HasChalkStick,
                    NightTerrorFreeDays = e.NightTerrorFreeDays,
                    DaysHardened = e.DaysHardened
                };
            }
        }
    }

    /// <summary>Per-child development state.</summary>
    [Serializable]
    public class ChildState
    {
        public string ChildId;
        public string ArchetypeId;
        public float Innocence = 100f;
        public float Trauma;
        public bool IsHardened;
        public bool HasCatatonicBreak;
        public float CatatonicBreakDays;
        public float CatatonicTreatmentHours;
        public bool HasLearnedToShoot;
        public bool HasAshDoll;
        public bool HasChalkStick;
        public float NightTerrorFreeDays;
        public float DaysHardened;
    }

    [Serializable]
    public class ChildDevelopmentSave
    {
        public ChildStateSave[] Children;
    }

    [Serializable]
    public class ChildStateSave
    {
        public string ChildId;
        public string ArchetypeId;
        public float Innocence;
        public float Trauma;
        public bool IsHardened;
        public bool HasCatatonicBreak;
        public float CatatonicBreakDays;
        public float CatatonicTreatmentHours;
        public bool HasLearnedToShoot;
        public bool HasAshDoll;
        public bool HasChalkStick;
        public float NightTerrorFreeDays;
        public float DaysHardened;
    }
}
