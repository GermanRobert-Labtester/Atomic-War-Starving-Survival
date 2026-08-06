using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Mental-break system: monitors each survivor's morale, rolls for a
    /// <see cref="MentalBreakSO"/> when morale stays below threshold for the
    /// configured window, applies the break, drives the BingeEater
    /// "consume 3x daily rations" path, and cures the break over time.
    ///
    /// Also owns the <see cref="InterpersonalAffinity"/> matrix and the
    /// hooks that EventRunner uses to mutate it from narrative choices.
    ///
    /// The system is data-driven: designers add MentalBreakSO assets with
    /// a snake_case id, register them via <see cref="RegisterBreak"/>, and
    /// the system reads trait weights off the asset. The same pattern as
    /// <c>AfflictionSO</c> / <c>MedicalSystem</c>.
    /// </summary>
    public class MentalBreakSystem
    {
        // -----------------------------------------------------------------
        // Public constants
        // -----------------------------------------------------------------

        /// <summary>Morale threshold below which a survivor starts accumulating
        /// <c>lowMoraleHours</c> toward a break roll. Spec: "Morale < 10".</summary>
        public const float LowMoraleBreakThreshold = 10f;

        /// <summary>Hours of continuous low morale that trigger the roll. Spec: 48h.</summary>
        public const float LowMoraleBreakWindowHours = 48f;

        /// <summary>Seconds per slot searched when looking for the highest-value
        /// food slot for a BingeEater survivor.</summary>
        public const int BingeEaterMaxSlotsScanned = 32;

        // -----------------------------------------------------------------
        // Events
        // -----------------------------------------------------------------

        /// <summary>Fired when a survivor enters a break. Args: (survivor, breakId).</summary>
        public event Action<Survivor, string> OnBreakStarted;

        /// <summary>Fired when a survivor is cured. Args: (survivor, breakId).</summary>
        public event Action<Survivor, string> OnBreakCured;

        // -----------------------------------------------------------------
        // State
        // -----------------------------------------------------------------

        private readonly Dictionary<string, MentalBreakSO> _breaksById = new Dictionary<string, MentalBreakSO>();

        /// <summary>The 2D affinity matrix; persisted via SaveSystem.</summary>
        public readonly InterpersonalAffinity Affinity = new InterpersonalAffinity();

        /// <summary>
        /// Optional host hook: binge-eat when a break has consumptionMultiplier &gt; 1.
        /// Injected by Core so Survivors stays free of Inventory assembly refs.
        /// Returns units consumed.
        /// </summary>
        public Func<Survivor, MentalBreakSO, int> BingeEatHandler;

        /// <summary>
        /// Optional host hook: comfort-item cure. Given a broken survivor,
        /// look up an appropriate comfort item in the bunker inventory,
        /// consume one, and return true if a unit was consumed. Injected by
        /// Core so Survivors stays free of Inventory assembly refs. Returning
        /// false means no comfort item was available; the cure attempt was
        /// a no-op.
        /// </summary>
        public Func<Survivor, MentalBreakSO, bool> ComfortCureHandler;

        /// <summary>
        /// Optional host hook: sabotage a shelter module (ViolentParanoia).
        /// Injected by Core so Survivors stays free of Shelter assembly refs.
        /// Returns the id of the sabotaged module (or null if none was hit).
        /// </summary>
        public Func<Survivor, MentalBreakSO, System.Random, string> SabotageHandler;

        /// <summary>
        /// Fired once a ViolentParanoia sabotage lands; the first arg is the best
        /// known module id (fallback to the break id when the host didn't supply one).
        /// Keeps the leaf assembly away from Shelter types.
        /// </summary>
        public event Action<string, object> ModuleSabotaged;

        private PersonalQuestSystem _personalQuests;
        private Func<IReadOnlyList<Survivor>> _getSurvivors;

        /// <summary>
        /// #249 Matriarch / #251 Pollyanna — block mental breaks when trait rules say so.
        /// #253 Psychopath — affinity drain while living among others.
        /// </summary>
        public void BindPersonalQuests(
            PersonalQuestSystem personalQuests,
            Func<IReadOnlyList<Survivor>> getSurvivors = null)
        {
            _personalQuests = personalQuests;
            _getSurvivors = getSurvivors;
        }

        // -----------------------------------------------------------------
        // Construction
        // -----------------------------------------------------------------

        public MentalBreakSystem() { }

        public MentalBreakSystem(IEnumerable<MentalBreakSO> breakAssets)
        {
            if (breakAssets == null) return;
            foreach (var br in breakAssets)
            {
                RegisterBreak(br);
            }
        }

        public void RegisterBreak(MentalBreakSO br)
        {
            if (br == null || string.IsNullOrEmpty(br.id)) return;
            _breaksById[br.id] = br;
        }

        public MentalBreakSO GetBreak(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _breaksById.TryGetValue(id, out var br) ? br : null;
        }

        public IReadOnlyCollection<MentalBreakSO> RegisteredBreaks => _breaksById.Values;

        // -----------------------------------------------------------------
        // Tick
        // -----------------------------------------------------------------

        /// <summary>
        /// Advance the system by <paramref name="gameHours"/>. Called from
        /// GameBootstrap.TickSystems. Updates low-morale hours, rolls for
        /// breaks at the threshold, drives the BingeEater consumption,
        /// advances cure progress, and applies passive drain to others
        /// sharing a room with a broken survivor.
        /// Inventory/Shelter side-effects run via <see cref="BingeEatHandler"/>
        /// and <see cref="SabotageHandler"/> so this assembly stays leaf-level.
        /// </summary>
        public void Tick(
            float gameHours,
            IReadOnlyList<Survivor> survivors,
            System.Random rng)
        {
            if (gameHours <= 0f || survivors == null) return;
            if (rng == null) rng = new System.Random();

            // 1. Update low-morale hours and roll for breaks at the threshold.
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                UpdateLowMoraleHours(sv, gameHours);
                if (!sv.HasMentalBreak && sv.lowMoraleHours >= LowMoraleBreakWindowHours)
                {
                    TryRollForBreak(sv, rng);
                }
            }

            // 2. Drive BingeEater / sabotage via host-injected handlers.
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive || !sv.HasMentalBreak) continue;
                var br = GetBreak(sv.currentMentalBreakId);
                if (br == null) continue;

                if (br.consumptionMultiplier > 1f && BingeEatHandler != null)
                {
                    BingeEatHandler(sv, br);
                }

                if (br.sabotageChancePerTick > 0f && SabotageHandler != null)
                {
                    if (rng.NextDouble() < br.sabotageChancePerTick * gameHours)
                    {
                        string moduleId = SabotageHandler(sv, br, rng);
                        ModuleSabotaged?.Invoke(moduleId ?? br.id ?? "unknown", null);
                    }
                }
            }

            // 3. Advance cure progress on every active break; auto-cure on time.
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive || !sv.HasMentalBreak) continue;
                var br = GetBreak(sv.currentMentalBreakId);
                if (br == null) continue;
                sv.mentalBreakCureProgress += gameHours;
                if (sv.mentalBreakCureProgress >= br.cureHours)
                {
                    Cure(sv);
                }
            }

            // 4. Passive morale drain + affinity drain on other survivors.
            ApplyPassiveDrain(gameHours, survivors, rng);
            ApplyBreakAffinityDrain(gameHours, survivors);

            // 5. #253 Psychopath InterpersonalAffinity drain + #254 Urge tick.
            TickBondBurdenQuirks(gameHours, survivors, rng);
        }

        /// <summary>
        /// #253 affinity drain around psychopaths; #254 Serial Killer Urge buildup.
        /// </summary>
        public void TickBondBurdenQuirks(
            float gameHours,
            IReadOnlyList<Survivor> survivors,
            System.Random rng)
        {
            if (gameHours <= 0f || survivors == null || _personalQuests == null) return;
            for (int i = 0; i < survivors.Count; i++)
            {
                var src = survivors[i];
                if (src == null || !src.IsAlive) continue;

                // #287 Invisible: no affinity gains or losses involving this survivor.
                // #309 Escapee Iron Will: also immune to affinity drain.
                if (_personalQuests.BlocksInterpersonalAffinity(src)
                    || _personalQuests.IsImmuneToInterpersonalAffinityDrain(src))
                    continue;

                float drain = _personalQuests.GetInterpersonalAffinityDrainPerHour(src);
                if (drain > 0f)
                {
                    float delta = -drain * gameHours;
                    for (int j = 0; j < survivors.Count; j++)
                    {
                        var other = survivors[j];
                        if (other == null || !other.IsAlive || other.Id == src.Id) continue;
                        if (_personalQuests.BlocksInterpersonalAffinity(other)
                            || _personalQuests.IsImmuneToInterpersonalAffinityDrain(other)) continue;
                        Affinity.Adjust(src.Id, other.Id, delta);
                    }
                }

                // Serial Killer Urge grows slowly every hour (~4/hour → max in ~25h).
                if (string.Equals(src.ArchetypeId, PersonalQuestSystem.SerialKillerId,
                        StringComparison.Ordinal))
                {
                    _personalQuests.TickUrge(src, 4f * gameHours, survivors);
                }

                _personalQuests.ClampMoraleToCap(src);
            }
        }

        // -----------------------------------------------------------------
        // Low-morale tracking + break roll
        // -----------------------------------------------------------------

        private void UpdateLowMoraleHours(Survivor sv, float gameHours)
        {
            if (sv.Needs.Morale < LowMoraleBreakThreshold)
            {
                sv.lowMoraleHours += gameHours;
            }
            else
            {
                // Climb back above threshold — reset the accumulator.
                if (sv.lowMoraleHours > 0f) sv.lowMoraleHours = 0f;
            }
        }

        /// <summary>Roll a weighted-random MentalBreakSO based on the survivor's
        /// <c>RiskBiasTrait</c>. Sets <c>currentMentalBreakId</c> and resets
        /// cure progress. No-op if no breaks are registered or all weights
        /// are zero.</summary>
        public bool TryRollForBreak(Survivor sv, System.Random rng)
        {
            if (sv == null || _breaksById.Count == 0 || rng == null) return false;
            // Gate: survivor must have accumulated enough low-morale hours.
            if (sv.lowMoraleHours < LowMoraleBreakWindowHours) return false;
            // #309 Escapee Iron Will: immune to all mental breaks.
            if (_personalQuests != null && _personalQuests.IsImmuneToAllMentalBreaks(sv))
                return false;

            var all = _getSurvivors != null ? _getSurvivors() : null;

            float totalWeight = 0f;
            foreach (var br in _breaksById.Values)
            {
                if (br == null) continue;
                // #249 Matriarch / #251 Pollyanna block specific or all breaks.
                if (_personalQuests != null
                    && _personalQuests.BlocksMentalBreak(sv, br.id, all))
                    continue;
                totalWeight += WeightForTrait(br, sv.RiskBias);
            }
            if (totalWeight <= 0f) return false;

            double roll = rng.NextDouble() * totalWeight;
            float accum = 0f;
            MentalBreakSO chosen = null;
            foreach (var br in _breaksById.Values)
            {
                if (br == null) continue;
                if (_personalQuests != null
                    && _personalQuests.BlocksMentalBreak(sv, br.id, all))
                    continue;
                accum += WeightForTrait(br, sv.RiskBias);
                if (roll <= accum) { chosen = br; break; }
            }
            if (chosen == null) return false;

            // Final gate (e.g. Matriarch while others live).
            if (_personalQuests != null
                && _personalQuests.BlocksMentalBreak(sv, chosen.id, all))
                return false;

            sv.currentMentalBreakId = chosen.id;
            sv.mentalBreakCureProgress = 0f;
            OnBreakStarted?.Invoke(sv, chosen.id);
            return true;
        }

        /// <summary>Per-trait weight lookup with safe fallback to 1.0
        /// if the break doesn't list the trait explicitly. Designers can
        /// leave the list empty and the break becomes trait-agnostic
        /// (equal weight for every survivor).</summary>
        public static float WeightForTrait(MentalBreakSO br, RiskBiasTrait trait)
        {
            if (br == null) return 0f;
            if (br.TraitWeights == null || br.TraitWeights.Count == 0) return 1f;
            for (int i = 0; i < br.TraitWeights.Count; i++)
            {
                if (br.TraitWeights[i] != null && br.TraitWeights[i].Trait == trait)
                {
                    return Mathf.Max(0f, br.TraitWeights[i].Weight);
                }
            }
            return 0f;
        }

        /// <summary>Force-cure a survivor (e.g. from a MedicalBed intervention).
        /// No-op if the survivor is not currently broken.</summary>
        public void Cure(Survivor sv)
        {
            if (sv == null || !sv.HasMentalBreak) return;
            string id = sv.currentMentalBreakId;
            sv.currentMentalBreakId = null;
            sv.mentalBreakCureProgress = 0f;
            sv.lowMoraleHours = 0f; // reset the trigger so a fresh break can accumulate
            OnBreakCured?.Invoke(sv, id);
        }

        /// <summary>
        /// Attempt to cure a broken survivor by consuming a high-value
        /// comfort item. The actual item lookup + consumption happens in
        /// <see cref="ComfortCureHandler"/> (host-supplied; needs Inventory
        /// access). If the handler returns true, the break's cure progress
        /// advances by <c>comfortItemCureAmount</c>; if the new progress
        /// meets or exceeds <c>cureHours</c>, the break resolves. Returns
        /// true if a cure was actually applied (item consumed + progress
        /// added); false if no comfort item was available, the survivor
        /// wasn't broken, or the break has <c>comfortItemCureAmount == 0</c>.
        /// </summary>
        public void ApplyComfortCure(Survivor sv) => TryCureWithComfortItem(sv);

        public bool TryCureWithComfortItem(Survivor sv)
        {
            if (sv == null || !sv.HasMentalBreak) return false;
            var br = GetBreak(sv.currentMentalBreakId);
            if (br == null || br.comfortItemCureAmount <= 0f) return false;
            if (ComfortCureHandler == null) return false;
            if (!ComfortCureHandler(sv, br)) return false;

            sv.mentalBreakCureProgress += br.comfortItemCureAmount;
            if (sv.mentalBreakCureProgress >= br.cureHours)
            {
                Cure(sv);
            }
            return true;
        }

        // -----------------------------------------------------------------
        // BingeEater consumption
        // -----------------------------------------------------------------

        // -----------------------------------------------------------------
        // Passive morale drain to other survivors
        // -----------------------------------------------------------------

        private void ApplyBreakAffinityDrain(
            float gameHours,
            IReadOnlyList<Survivor> survivors)
        {
            if (gameHours <= 0f || survivors == null) return;

            for (int b = 0; b < survivors.Count; b++)
            {
                var broken = survivors[b];
                if (broken == null || !broken.IsAlive || !broken.HasMentalBreak) continue;
                var br = GetBreak(broken.currentMentalBreakId);
                if (br == null || br.affinityDrainPerHour <= 0f) continue;

                float delta = -br.affinityDrainPerHour * gameHours;
                for (int o = 0; o < survivors.Count; o++)
                {
                    var other = survivors[o];
                    if (other == null || other == broken || !other.IsAlive) continue;
                    Affinity.Adjust(broken.Id, other.Id, delta);
                }
            }
        }

        private void ApplyPassiveDrain(
            float gameHours,
            IReadOnlyList<Survivor> survivors,
            System.Random rng)
        {
            if (gameHours <= 0f || survivors == null) return;

            for (int b = 0; b < survivors.Count; b++)
            {
                var broken = survivors[b];
                if (broken == null || !broken.IsAlive || !broken.HasMentalBreak) continue;
                var br = GetBreak(broken.currentMentalBreakId);
                if (br == null || br.passiveMoraleDrainPerHour <= 0f) continue;

                float drain = br.passiveMoraleDrainPerHour * gameHours;
                string brokenRoom = broken.CurrentRoomId;

                for (int o = 0; o < survivors.Count; o++)
                {
                    var other = survivors[o];
                    if (other == null || other == broken || !other.IsAlive) continue;
                    // Per-room drain: only hit survivors in the same room
                    // (or both unassigned, which acts as the "common area").
                    // Falls back to "whole shelter" when the broken survivor
                    // has no room assignment — keeps the previous broad
                    // behavior so unassigned survivors still feel the
                    // effect of a breakdown anywhere in the bunker.
                    if (!string.IsNullOrEmpty(brokenRoom)
                        && other.CurrentRoomId != brokenRoom)
                    {
                        continue;
                    }
                    other.Needs.Morale = Mathf.Max(0f, other.Needs.Morale - drain);
                }
            }
        }
    }
}
