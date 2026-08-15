using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Utilities;
using Ashfall.Core;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>
        /// The Day-30 atomic exchange cascade. Thinned: all mechanical
        /// side effects (EMP, weather force, radiation unpause, morale hit)
        /// run from the FlashpointChoreographer's 'emp' step, scheduled
        /// after the white flash. The choreographer is the single source
        /// of truth for the moment's timeline.
        /// </summary>
        private void HandleNuclearExchange()
        {
            if (FlashpointChoreographer == null)
            {
                // Fallback: if no choreographer is wired (test scene, broken
                // wiring), run the original cascade so the game still
                // advances to NuclearWinter. This matches the pre-Prompt-27
                // behavior and prevents soft-locks.
                var empResult = EMPEvent.ApplyGlobal(Inventory, Shelter, RadioTunerSystem?.State);
                GameLog.Log($"[GameBootstrap] Nuclear exchange (fallback): {empResult.DevicesBroken} devices broken, " +
                          $"{empResult.ModulesDisabled} modules disabled, radio destroyed={empResult.RadioDestroyed}.");

                if (WeatherSystem != null)
                {
                    WeatherSystem.RestrictToNonHazardWeather = false;
                    WeatherSystem.ForceWeather(WeatherKind.Ashfall);
                }
                if (RadiationSystem != null) RadiationSystem.IsPaused = false;

                if (Survivors != null)
                {
                    float hit = WorldPhaseSystem?.ExchangeMoraleHit ?? 25f;
                    foreach (var sv in Survivors)
                    {
                        if (sv == null || !sv.IsAlive) continue;
                        // Route through NeedsSystem.Modify so Selfless,
                        // Traumatized cap, and LivingSaint floor are honoured.
                        if (NeedsSystem != null)
                            NeedsSystem.Modify(sv, NeedKind.Morale, -hit);
                        else
                            sv.Needs.Morale = Mathf.Clamp(sv.Needs.Morale - hit, 0f, 100f);
                    }
                }
                // Prompt #19 — ghost bands appear in the static after EMP.
                GhostStationSystem?.NotifyEmpOccurred();
                return;
            }

            FlashpointChoreographer.OnNuclearExchange();
        }

        /// <summary>Flashpoint EMP step → unlock ghost stations (Prompt #19).</summary>
        private void OnFlashpointEmp_UnlockGhosts(FlashpointEmptiedDevices _)
        {
            GhostStationSystem?.NotifyEmpOccurred();
        }

        private void HandleLifeboatContactOffered(GameEvent ev)
        {
            if (ev == null || EventRunner == null) return;
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
            var ctx = BuildEventContext(day);
            ctx.SetEventFlag(LifeboatTransmissionSystem.FlagContacted, true);
            EventRunner.Run(ev, ctx);
            GameLog.Log("[Lifeboat] Two-way contact. One seat. Choose who walks.");
        }

        private void HandleLifeboatChoiceApplied(GameEvent ev, EventChoice choice, EventContext ctx)
        {
            if (ev == null || choice == null || LifeboatTransmissionSystem == null) return;
            if (!string.Equals(ev.id, LifeboatTransmissionSystem.EventId, StringComparison.Ordinal))
                return;
            if (LifeboatTransmissionSystem.ApplyChoiceFromEvent(ev, choice, ctx))
            {
                GameLog.Log(
                    $"[Lifeboat] Sent {LifeboatTransmissionSystem.ExtractedSurvivorName}. " +
                    $"{LifeboatTransmissionSystem.LeftBehindIds.Count} left behind.");
                // VictoryProject.OnEndgameTriggered → ApplyEndgame already wired.
            }
        }

        /// <summary>
        /// Inject location-bound sniper ambush (Unverified send). forceOnArrival
        /// guarantees the beat fires when the expedition reaches the grid.
        /// </summary>
        private void InjectSafeHavenAmbushEncounter()
        {
            if (ExpeditionSystem == null) return;
            ExpeditionSystem.AddEncounter(SafeHavenEncounters.CreateAmbush());
        }

        /// <summary>
        /// Inject empty-cache discovery after the player analyzed the loop.
        /// </summary>
        private void InjectSafeHavenEmptyCacheEncounter()
        {
            if (ExpeditionSystem == null) return;
            ExpeditionSystem.AddEncounter(SafeHavenEncounters.CreateEmptyCache());
        }

        /// <summary>
        /// EventRunner.OnChoiceApplied listener for the Blood for Water
        /// event. Inflicts <c>BloodLoss</c> on the donor survivor (resolved
        /// via <see cref="EventRunner.FindBloodDonor"/>) and, on a forced
        /// bleed, slams the donor's affinity with the bunker leader to
        /// <see cref="EventRunner.ForcedBleedAffinityFloor"/> so
        /// MentalBreakSystem can fire a ViolentParanoia break.
        /// </summary>

        /// <summary>
        /// Buried Alive / faction dig-out choice side effects (Prompt #48).
        /// DigOut spikes entry-room CO2; faction rescue clears the hatch.
        /// </summary>



        /// <summary>
        /// Faction dig-out accepted → schedule collector for day + 20.
        /// Short-term dig-out debt flag is already set by HatchEntrapmentSystem.
        /// </summary>



        /// <summary>
        /// After a hatch repel that did not auto-surrender, present a modal:
        /// demand parley now, open trade, or dismiss. Wired from Economy.OnRaidResolved.
        /// </summary>

        /// <summary>Build + run the parley offer GameEvent; start soft timeout.</summary>


        /// <summary>Resolve a parley-offer choice id from the event modal.</summary>

        /// <summary>
        /// Hatch-dilemma prompt timeout: auto-apply the timeout resolution
        /// (default ForceDeconOutside) by raising the resolved signal.
        /// The ExpeditionSystem listens and applies the consequence.
        /// </summary>

        /// <summary>
        /// Player (or AI) made a hatch-dilemma choice via the event
        /// modal. The OnChoiceApplied lambda already raised the
        /// HatchDilemmaResolvedSignal; here we just cancel the prompt
        /// timeout so the survivor doesn't wait indefinitely after the
        /// player has already chosen.
        /// </summary>

    }
}
