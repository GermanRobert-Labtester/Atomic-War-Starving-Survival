using System;
using System.Collections.Generic;
using UnityEngine;
using Ashfall.Core.Journal;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// The Child / Dependent Mechanic (Prompt #9). An event where the player
    /// finds an 8-year-old in the ash. The child cannot scavenge, craft, or
    /// fight and consumes a full ration of food and water. Keeping them alive
    /// provides a massive, permanent Hope morale buff to the entire bunker.
    /// If the child dies, the subsequent morale crash will likely end the run.
    ///
    /// Plain C# system. Owns and writes the ChildHopeActive flag and applies
    /// the per-tick morale buff.
    /// </summary>
    public class ChildDependentSystem
    {
        private NeedsSystem _needsSystem;
        public void SetNeedsSystem(NeedsSystem ns) => _needsSystem = ns;

        // Sociopath immunity is a TRAIT (trait_sociopath), not a RiskBias value;
        // see EmpathSystem for the same correction. RiskBias is never Sociopath.
        private PersonalQuestSystem _personalQuests;
        public void SetPersonalQuestSystem(PersonalQuestSystem pqs) => _personalQuests = pqs;

        /// <summary>Morale per hour applied to every other survivor while the child is alive.</summary>
        public const float HopeBuffPerHour = 0.15f;

        /// <summary>Massive morale hit applied when the child dies.</summary>
        public const float ChildDeathMoralePenalty = 35f;

        /// <summary>Snake_case id for the child survivor.</summary>
        public const string ChildSurvivorId = "sv_child";

        /// <summary>Snake_case trait id applied to the child survivor.</summary>
        public const string ChildTraitId = "child";

        /// <summary>Event flag set when the child is found.</summary>
        public const string ChildFoundFlag = "child_found";

        /// <summary>Event flag set when the child dies.</summary>
        public const string ChildDiedFlag = "child_died";

        /// <summary>Child's daily food consumption in hunger points (inverted: they add to hunger drain).</summary>
        public const float ChildDailyFoodDrain = 20f; // per day, added to bunker food consumption

        /// <summary>Child's daily water consumption in thirst points.</summary>
        public const float ChildDailyWaterDrain = 20f; // per day, added to bunker water consumption

        /// <summary>Fired when the child is found and joins the bunker.</summary>
        public event Action<Survivor> OnChildFound;

        /// <summary>Fired when the child dies. Other systems listen for morale crash.</summary>
        public event Action<Survivor> OnChildDied;

        /// <summary>The child survivor instance, or null if not yet found.</summary>
        public Survivor Child { get; private set; }

        /// <summary>True while the child is alive in the bunker.</summary>
        public bool IsChildAlive => Child != null && Child.IsAlive;

        /// <summary>True if the child was ever found (persists across save/load).
        /// MUST be a plain field (not auto-property) — JsonUtility doesn't serialize properties.</summary>
        public bool WasChildFound;

        /// <summary>
        /// Serializable state for save/load. Captures everything needed to
        /// restore the ChildDependentSystem after deserialization.
        /// </summary>
        [System.Serializable]
        public struct SaveState
        {
            public bool wasChildFound;
            public string childId;
        }

        /// <summary>Capture current state for serialization.</summary>
        public SaveState CaptureState()
        {
            return new SaveState
            {
                wasChildFound = WasChildFound,
                childId = Child != null ? Child.Id : null
            };
        }

        /// <summary>Restore state after deserialization.</summary>
        public void RestoreState(SaveState state, IReadOnlyList<Survivor> survivors)
        {
            WasChildFound = state.wasChildFound;
            if (!string.IsNullOrEmpty(state.childId) && survivors != null)
            {
                for (int i = 0; i < survivors.Count; i++)
                {
                    if (survivors[i] != null && survivors[i].Id == state.childId && survivors[i].IsAlive)
                    {
                        Child = survivors[i];
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Host hook: consume extra food/water from inventory for the child.
        /// Injected by GameBootstrap. Signature: (survivor, foodUnits, waterUnits).
        /// </summary>
        public Action<float, float> ConsumeChildRationsHandler;

        // -----------------------------------------------------------------
        // Child creation
        // -----------------------------------------------------------------

        /// <summary>
        /// Create and register the child survivor. Called when the player
        /// accepts the child into the bunker via the GameEvent choice.
        /// </summary>
        public Survivor CreateChild()
        {
            if (Child != null) return Child;

            Child = new Survivor
            {
                Id = ChildSurvivorId,
                DisplayName = "The Child",
                State = SurvivorState.Idle,
                RiskBias = RiskBiasTrait.Realist,
                IsChild = true,
                CannotScavenge = true,
                CannotCraft = true,
                CannotFight = true,
                MedicalSkill = 0f,
                ScienceSkill = 0f,
                CraftingSkill = 0f
            };
            Child.Needs.Hunger = 50f;
            Child.Needs.Thirst = 50f;
            Child.Needs.Fatigue = 30f;
            Child.Needs.Warmth = 80f;
            Child.Needs.Morale = 40f;
            SurvivorNeedWrite.SetHealth(Child, 80f);
            Child.Traits.Add(ChildTraitId);

            WasChildFound = true;
            OnChildFound?.Invoke(Child);

            return Child;
        }

        // -----------------------------------------------------------------
        // Tick
        // -----------------------------------------------------------------

        /// <summary>
        /// Advance the system. Applies the Hope buff to all non-child survivors
        /// while the child is alive. Drains extra food/water for the child.
        /// </summary>
        public void Tick(float gameHours, IReadOnlyList<Survivor> survivors)
        {
            if (gameHours <= 0f || survivors == null) return;

            // Lazy-find the child from the survivors list after save/load restore.
            // Runs regardless of WasChildFound — if a living IsChild survivor exists,
            // they ARE the child and we should recognize them.
            if (Child == null && survivors != null)
            {
                for (int i = 0; i < survivors.Count; i++)
                {
                    var sv = survivors[i];
                    if (sv != null && sv.IsAlive && sv.IsChild)
                    {
                        Child = sv;
                        WasChildFound = true;
                        break;
                    }
                }
            }

            if (!IsChildAlive) return;

            float gameDays = gameHours / 24f;

            // Apply Hope buff to all other survivors
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive || sv == Child) continue;
                if (_needsSystem != null)
                    _needsSystem.Modify(sv, NeedKind.Morale, HopeBuffPerHour * gameHours);
                else
                    sv.Needs.Morale = Mathf.Clamp(
                        sv.Needs.Morale + HopeBuffPerHour * gameHours, 0f, 100f);
            }

            // Consume child's rations (daily drain prorated)
            float foodDrain = ChildDailyFoodDrain * gameDays;
            float waterDrain = ChildDailyWaterDrain * gameDays;

            if (ConsumeChildRationsHandler != null)
            {
                ConsumeChildRationsHandler(foodDrain, waterDrain);
            }
            else
            {
                // Fallback: increase child's hunger/thirst directly
                if (_needsSystem != null)
                {
                    _needsSystem.Modify(Child, NeedKind.Hunger, foodDrain);
                    _needsSystem.Modify(Child, NeedKind.Thirst, waterDrain);
                }
                else
                {
                    Child.Needs.Hunger = Mathf.Clamp(Child.Needs.Hunger + foodDrain, 0f, 100f);
                    Child.Needs.Thirst = Mathf.Clamp(Child.Needs.Thirst + waterDrain, 0f, 100f);
                }
            }

            // NOTE: Child needs are also decayed by NeedsSystem.Tick (the child
            // is registered). The rations handler above covers the EXTRA consumption
            // on top of baseline needs decay. We do NOT apply additional decay here.
        }

        // -----------------------------------------------------------------
        // Death hook
        // -----------------------------------------------------------------

        /// <summary>
        /// Call when the child dies. Applies the catastrophic morale crash.
        /// </summary>
        public void OnChildPerished(IReadOnlyList<Survivor> survivors)
        {
            if (!IsChildAlive) return;

            string childId = Child.Id;
            Child = null; // clear reference

            // Apply morale crash to all survivors
            if (survivors != null)
            {
                for (int i = 0; i < survivors.Count; i++)
                {
                    var sv = survivors[i];
                    if (sv == null || !sv.IsAlive || sv.Id == childId) continue;

                    // Sociopath is immune to the child's death too.
                    bool sociopathImmune = _personalQuests != null
                        ? _personalQuests.HasSociopath(sv)
                        : sv.HasTrait(PersonalQuestSystem.SociopathId);
                    if (sociopathImmune) continue;

                    if (_needsSystem != null)
                        _needsSystem.Modify(sv, NeedKind.Morale, -ChildDeathMoralePenalty);
                    else
                        sv.Needs.Morale = Mathf.Clamp(
                            sv.Needs.Morale - ChildDeathMoralePenalty, 0f, 100f);
                }
            }

            OnChildDied?.Invoke(null);
        }

        /// <summary>
        /// Check if the child survivor has died (via NeedsSystem or other means)
        /// and trigger the death consequence if so.
        /// </summary>
        public bool CheckChildDeath(IReadOnlyList<Survivor> survivors)
        {
            if (!IsChildAlive) return false;

            if (!Child.IsAlive)
            {
                OnChildPerished(survivors);
                return true;
            }
            return false;
        }
    }
}
