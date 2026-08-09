using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Core
{
    public partial class ExpeditionSystem
    {
        private void HandleFlashpointIntercept(FlashpointInterceptSignal signal)
        {
            if (signal.InterceptedExpeditions == null) return;

            for (int i = 0; i < signal.InterceptedExpeditions.Count; i++)
            {
                var exp = signal.InterceptedExpeditions[i];
                if (exp == null || exp.isCommsSevered) continue;
                ApplyFlashpointIntercept(exp);
            }
        }

        private void ApplyFlashpointIntercept(ExpeditionState exp)
        {
            if (exp.Survivor == null) return;

            // 1. Sever comms
            exp.isCommsSevered = true;

            // 2. Acute-dose spike, scaled by how long the survivor has been in the
            // field. RAD-3C: a flat spike gave a survivor caught an hour from the
            // hatch the same acute window as one three days deep, which made the
            // stance/timing decisions around a flashpoint meaningless.
            exp.Survivor.AcuteDoseWindow += FlashpointAcuteDoseSpike * GetFlashpointDoseScale(exp);

            // 3. Trauma affliction (broken bones / shockwave)
            if (_medicalSystem != null)
            {
                _medicalSystem.Inflict(exp.Survivor, AfflictionSO.Ids.BrokenBone);
            }

            // 4. Trait-driven resolution
            ResolveFlashpointBehavior(exp);

            // 5. Cache original ETA so the UI / save can show "halved" / "delayed"
            exp.originalEtaTicks = exp.TravelTicksCompleted;

            // 6. Force into the Inbound phase so the survivor begins the return.
            // If they were Looting, abandon the loot site immediately; if they
            // were Outbound, the inbound leg is the full distance.
            exp.Phase = ExpeditionPhase.Inbound;
            if (exp.IsPushingLuck) exp.IsPushingLuck = false;

            OnFlashpointIntercepted?.Invoke(exp);
        }

        /// <summary>
        /// RAD-3C: duration multiplier for the flashpoint acute-dose spike. One
        /// expedition tick is one game hour (see <c>Tick</c>), so
        /// <see cref="ExpeditionState.CurrentTick"/> is elapsed field hours.
        /// Clamped so a very short run still hurts and a very long one cannot
        /// deliver an unbounded window.
        /// </summary>
        internal static float GetFlashpointDoseScale(ExpeditionState exp)
        {
            if (exp == null) return MinFlashpointDoseScale;
            return Mathf.Clamp(
                exp.CurrentTick / FlashpointDoseReferenceHours,
                MinFlashpointDoseScale,
                MaxFlashpointDoseScale);
        }

        private void ResolveFlashpointBehavior(ExpeditionState exp)
        {
            var sv = exp.Survivor;
            if (sv == null) return;

            switch (sv.RiskBias)
            {
                case RiskBiasTrait.Paranoid:
                    // Drop all loot, sprint home
                    exp.DropLoot(1.0f);
                    exp.returnSpeedMultiplier = ParanoidSprintMultiplier;
                    exp.flashpointBehavior = FlashpointBehavior.ParanoidSprint;
                    // Halve the visual ETA so the UI shows progress
                    if (exp.originalEtaTicks <= 0f) exp.originalEtaTicks = exp.TravelTicksCompleted;
                    exp.TravelTicksCompleted = Mathf.Max(0, Mathf.RoundToInt(exp.TravelTicksCompleted * 0.5f));
                    break;

                case RiskBiasTrait.Cautious:
                    // Take temporary shelter, gain radiation anxiety
                    exp.shelterDelayTicksRemaining = DefaultCautiousShelterDelayTicks;
                    exp.flashpointBehavior = FlashpointBehavior.CautiousShelter;
                    sv.HasRadiationAnxietyStatus = true;
                    break;

                case RiskBiasTrait.Fatalist:
                    // Numb walk: keep loot, half speed, gain Numbness
                    exp.returnSpeedDivisor = FatalistSlowWalkDivisor;
                    exp.flashpointBehavior = FlashpointBehavior.FatalistNumbWalk;
                    sv.IsNumb = true;
                    break;

                case RiskBiasTrait.Reckless:
                case RiskBiasTrait.Realist:
                case RiskBiasTrait.Denialist:
                default:
                    // Keep all loot, normal return speed, full rad accumulation
                    exp.flashpointBehavior = FlashpointBehavior.RecklessPushThrough;
                    break;
            }
        }

        /// <summary>
        /// Resolve a hatch dilemma choice. Called by the GameBootstrap (or
        /// any handler that runs the dilemma GameEventSO through the
        /// EventRunner) once the player picks a choice.
        ///
        /// Applies both the state-machine consequence (complete/fail the
        /// expedition, kill the survivor) and the bunker-wide side effects
        /// (contamination spike on let-them-in / force-decon; morale
        /// propagation to the rest of the bunker on deny-entry). The side
        /// effects are intentionally co-located here so they are testable
        /// without a full GameBootstrap setup.
        /// </summary>
        public void ApplyHatchDilemmaChoice(string expeditionId, HatchDilemmaResolvedSignal.Resolution choice)
        {
            var exp = FindExpeditionById(expeditionId);
            if (exp == null || exp.Phase != ExpeditionPhase.AtHatchDilemma) return;

            string survivorName = exp.Survivor != null ? exp.Survivor.DisplayName : "the survivor";

            switch (choice)
            {
                case HatchDilemmaResolvedSignal.Resolution.LetThemIn:
                    // Complete the expedition. The bunker's ambient contamination
                    // spikes by the configured amount (their gear and clothes are
                    // soaked in fallout). The survivor is sick but alive.
                    if (_shelter != null)
                    {
                        _shelter.AddBunkerContamination(LetThemInContaminationRadsPerHour);
                        GameLog.Log($"[Flashpoint] LetThemIn: bunker contamination +{LetThemInContaminationRadsPerHour} rph " +
                                  $"(now {_shelter.BunkerContamination:F1}) after admitting {survivorName}.");
                    }
                    CompleteExpedition(exp);
                    RemoveExpedition(exp);
                    break;

                case HatchDilemmaResolvedSignal.Resolution.ForceDeconOutside:
                    // Strip outside in the ash: 2 hours of rad damage, big morale
                    // hit, plus a small contamination spill (the gear is set
                    // down just inside the airlock for decon).
                    if (exp.Survivor != null && _radSystem != null)
                    {
                        _radSystem.Expose(exp.Survivor, 10f, 2f);
                        if (_needsSystem != null)
                            _needsSystem.Modify(exp.Survivor, NeedKind.Morale, -15f);
                        else
                            exp.Survivor.Needs.Morale = Mathf.Clamp(exp.Survivor.Needs.Morale - 15f, 0f, 100f);
                    }
                    if (_shelter != null)
                    {
                        _shelter.AddBunkerContamination(ForceDeconContaminationRadsPerHour);
                        GameLog.Log($"[Flashpoint] ForceDecon: bunker contamination +{ForceDeconContaminationRadsPerHour} rph " +
                                  $"(now {_shelter.BunkerContamination:F1}) from {survivorName}'s strip-down.");
                    }
                    CompleteExpedition(exp);
                    RemoveExpedition(exp);
                    break;

                case HatchDilemmaResolvedSignal.Resolution.DenyEntry:
                    // Survivor dies outside. Massive morale penalty propagates
                    // to every OTHER living survivor in the bunker.
                    if (exp.Survivor != null)
                    {
                        exp.Survivor.State = SurvivorState.Dead;
                    }
                    int affected = PropagateDenyEntryMoralePenalty(exp.SurvivorId);
                    exp.Phase = ExpeditionPhase.Failed;
                    OnExpeditionFailed?.Invoke(exp, "denied_entry");
                    RemoveExpedition(exp);
                    GameLog.Log($"[Flashpoint] DenyEntry: {survivorName} died outside the hatch; " +
                              $"{affected} other survivor(s) lost {DenyEntryMoralePenaltyForOtherSurvivors} morale.");
                    break;
            }

            OnHatchDilemmaResolved?.Invoke(exp);
        }

        /// <summary>
        /// Apply the deny-entry morale hit to every other living survivor in
        /// the configured survivor list. Returns the count of survivors
        /// affected. Skips the dying survivor (by id) and any non-alive or
        /// null entries. Logs a one-liner so designers can tune the magnitude.
        /// </summary>
        private int PropagateDenyEntryMoralePenalty(string dyingSurvivorId)
        {
            if (_survivors == null) return 0;
            int affected = 0;
            for (int i = 0; i < _survivors.Count; i++)
            {
                var sv = _survivors[i];
                if (sv == null || !sv.IsAlive) continue;
                if (!string.IsNullOrEmpty(dyingSurvivorId) && sv.Id == dyingSurvivorId) continue;
                if (_needsSystem != null)
                    _needsSystem.Modify(sv, NeedKind.Morale, -DenyEntryMoralePenaltyForOtherSurvivors);
                else
                    sv.Needs.Morale = Mathf.Clamp(
                        sv.Needs.Morale - DenyEntryMoralePenaltyForOtherSurvivors,
                        0f, 100f);
                affected++;
            }
            return affected;
        }

        private void HandleHatchDilemmaResolved(HatchDilemmaResolvedSignal signal)
        {
            ApplyHatchDilemmaChoice(signal.ExpeditionId, signal.Choice);
        }
    }
}
