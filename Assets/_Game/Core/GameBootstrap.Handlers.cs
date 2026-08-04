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
                Debug.Log($"[GameBootstrap] Nuclear exchange (fallback): {empResult.DevicesBroken} devices broken, " +
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
            Debug.Log("[Lifeboat] Two-way contact. One seat. Choose who walks.");
        }

        private void HandleLifeboatChoiceApplied(GameEvent ev, EventChoice choice, EventContext ctx)
        {
            if (ev == null || choice == null || LifeboatTransmissionSystem == null) return;
            if (!string.Equals(ev.id, LifeboatTransmissionSystem.EventId, StringComparison.Ordinal))
                return;
            if (LifeboatTransmissionSystem.ApplyChoiceFromEvent(ev, choice, ctx))
            {
                Debug.Log(
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
        private void HandleBloodForWaterChoiceApplied(GameEvent ev, EventChoice choice, EventContext ctx)
        {
            if (ev == null || choice == null) return;
            if (ev.id != EventRunner.BloodForWaterEventId) return;

            // Refuse / ignore: nothing to inflict, the trust delta was
            // already applied by the runner via the choice's FactionId +
            // TrustDelta path.
            if (choice.ChoiceId == "refuse_convoy") return;
            if (choice.ChoiceId == "ignore_summons") return;

            // Resolve the donor. The choice text doesn't carry a survivor
            // id (the bunker has multiple, the player can pick). For the
            // bootstrap, prefer the explicit PrimarySurvivor (the UI sets
            // it to the highlighted donor); fall back to the union of the
            // event's two gates (Fatalist first, then any non-Paranoid).
            var donor = ctx != null ? ctx.PrimarySurvivor : null;
            if (donor == null || !donor.IsAlive)
            {
                donor = EventRunner.FindBloodDonor(Survivors);
            }
            if (donor == null || !donor.IsAlive)
            {
                Debug.LogWarning("[Blood for Water] No eligible donor in the bunker; skipping BloodLoss inflict.");
                return;
            }

            // Inflict the affliction. MedicalSystem.Inflict is a no-op if
            // the def is unknown or the survivor already has it.
            if (MedicalSystem != null)
            {
                bool applied = MedicalSystem.Inflict(donor, AfflictionSO.Ids.BloodLoss);
                if (!applied)
                {
                    Debug.LogWarning($"[Blood for Water] MedicalSystem.Inflict returned false for {donor.Id}.");
                }
                else
                {
                    Debug.Log($"[Blood for Water] BloodLoss inflicted on {donor.DisplayName}.");
                }
            }

            // Forced bleed: slam the donor's affinity with the bunker leader
            // (the highest-trust living survivor, or donor themselves if
            // alone) to the ForcedBleedAffinityFloor. MentalBreakSystem
            // reads this matrix in its roll; -100 is the input that
            // maximises a Paranoid survivor's chance of a ViolentParanoia
            // break.
            if (choice.ChoiceId == "bleed_paranoid_force"
                && ctx != null
                && ctx.MentalBreak != null
                && MentalBreakSystem != null)
            {
                Survivor leader = ResolveBunkerLeader();
                if (leader != null && leader != donor)
                {
                    MentalBreakSystem.Affinity.Set(
                        donor.Id, leader.Id,
                        EventRunner.ForcedBleedAffinityFloor);
                    Debug.Log($"[Blood for Water] Affinity {donor.DisplayName}↔{leader.DisplayName} slammed to {EventRunner.ForcedBleedAffinityFloor}.");
                }
            }
        }

        /// <summary>
        /// Buried Alive / faction dig-out choice side effects (Prompt #48).
        /// DigOut spikes entry-room CO2; faction rescue clears the hatch.
        /// </summary>
        private void HandleHatchEntrapmentChoiceApplied(GameEvent ev, EventChoice choice, EventContext ctx)
        {
            if (ev == null || choice == null || HatchEntrapmentSystem == null) return;

            if (ev.id == EventRunner.BuriedAliveEventId
                && choice.ChoiceId == EventRunner.ChoiceDigOut)
            {
                if (_entryRoom == null)
                    _entryRoom = new ShelterRoom(HatchEntrapmentSystem.EntryRoomId, null);
                HatchEntrapmentSystem.DigOut(_entryRoom, ctx);
                SyncHatchExpeditionLock();
                Debug.Log($"[Hatch Entrapment] DigOut complete. Entry CO2={_entryRoom.Co2Ppm:F0} ppm.");
                return;
            }

            if (ev.id == EventRunner.FactionDigOutEventId
                && choice.ChoiceId == EventRunner.ChoiceAcceptFactionRescue)
            {
                HatchEntrapmentSystem.ApplyFactionRescue(ctx);
                SyncHatchExpeditionLock();
                Debug.Log("[Hatch Entrapment] Faction dug the hatch open. Debt recorded.");
            }
        }

        private void HandleRaidPlanInterceptOffered(FactionRaidPlan plan, GameEvent ev)
        {
            if (ev == null || EventRunner == null) return;
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
            var ctx = BuildEventContext(day);
            EventRunner.Run(ev, ctx);
            Debug.Log($"[Raid Plan] Wiretap offered: {plan?.AttackerFactionId} → {plan?.TargetFactionId}");
        }

        private void HandleRaidPlanChoiceApplied(GameEvent ev, EventChoice choice, EventContext ctx)
        {
            if (ev == null || choice == null || FactionRaidPlanSystem == null) return;
            if (string.IsNullOrEmpty(ev.id)
                || !ev.id.StartsWith(FactionRaidPlanSystem.EventIdPrefix, StringComparison.Ordinal))
                return;
            FactionRaidPlanSystem.ApplyChoiceFromEvent(ev, choice);
        }

        /// <summary>
        /// Faction dig-out accepted → schedule collector for day + 20.
        /// Short-term dig-out debt flag is already set by HatchEntrapmentSystem.
        /// </summary>
        private void HandleFactionRescueApplied_ScheduleDebt(string factionId)
        {
            if (DebtCollectorSystem == null || string.IsNullOrEmpty(factionId)) return;
            if (DebtCollectorSystem.HasPendingDebtFor(factionId)) return;
            var entry = DebtCollectorSystem.ScheduleDebt(factionId);
            if (entry != null)
                Debug.Log($"[Debt Collector] Scheduled for {factionId} on day {entry.CollectorDay}.");
        }

        private void HandleDebtCollectorArrived(DebtEntry debt, GameEvent ev)
        {
            if (ev == null || EventRunner == null) return;
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
            var ctx = BuildEventContext(day);
            EventRunner.Run(ev, ctx);
            Debug.Log($"[Debt Collector] {debt?.FactionId} demands half fuel + half clean water.");
        }

        private void HandleDebtCollectorChoiceApplied(GameEvent ev, EventChoice choice, EventContext ctx)
        {
            if (ev == null || choice == null || DebtCollectorSystem == null) return;
            if (string.IsNullOrEmpty(ev.id)
                || !ev.id.StartsWith(DebtCollectorSystem.EventIdPrefix, StringComparison.Ordinal))
                return;
            DebtCollectorSystem.ApplyChoiceFromEvent(ev, choice, ctx);
        }

        /// <summary>
        /// After a hatch repel that did not auto-surrender, present a modal:
        /// demand parley now, open trade, or dismiss. Wired from Economy.OnRaidResolved.
        /// </summary>
        private void OnFactionRaidResolved_Handle(FactionRaidResult result)
        {
            if (result == null || !result.Launched || !result.Repelled) return;
            // Second repel may auto-surrender — no parley gate left.
            if (result.SurrenderedAfter) return;
            if (EconomySystem == null || !EconomySystem.CanDemandParley(result.FactionId)) return;
            // Avoid stacking over an existing offer.
            if (ParleyOfferPromptField != null && ParleyOfferPromptField.IsActive) return;

            PresentParleyOffer(result.FactionId);
        }

        /// <summary>Build + run the parley offer GameEvent; start soft timeout.</summary>
        public void PresentParleyOffer(string factionId)
        {
            if (EconomySystem == null || EventRunner == null || string.IsNullOrEmpty(factionId))
                return;
            if (!EconomySystem.CanDemandParley(factionId)) return;

            string leader = EconomySystem.GetLeaderName(factionId);
            ParleyOfferPromptField?.Begin(factionId, leader);
            if (ParleyOfferPromptField != null)
            {
                ParleyOfferPromptField.OnTimeout -= OnParleyOfferTimeout_Handle;
                ParleyOfferPromptField.OnTimeout += OnParleyOfferTimeout_Handle;
            }

            var eventSo = EconomySystem.CreateParleyOfferEvent(factionId);
            string eventId = eventSo.id;
            string capturedFaction = factionId;

            Action<GameEvent, EventChoice, EventContext> onChoice = null;
            onChoice = (gameEvent, choice, ctx) =>
            {
                if (gameEvent == null || choice == null) return;
                if (gameEvent.id != eventId) return;
                ApplyParleyOfferChoice(capturedFaction, choice.ChoiceId);
                EventRunner.OnChoiceApplied -= onChoice;
            };
            EventRunner.OnChoiceApplied += onChoice;

            var primary = Survivors != null && Survivors.Count > 0 ? Survivors[0] : null;
            var ctx = new EventContext(primary, Shelter, Inventory, new System.Random(_worldSeed + 17));
            EventRunner.Run(eventSo, ctx);
        }

        private void OnParleyOfferTimeout_Handle(ParleyOfferPrompt.Resolution resolution)
        {
            // Soft dismiss — they can still open trade later via [P] if repels hold.
            ParleyOfferPromptField?.Cancel();
        }

        /// <summary>Resolve a parley-offer choice id from the event modal.</summary>
        public void ApplyParleyOfferChoice(string factionId, string choiceId)
        {
            if (string.IsNullOrEmpty(choiceId)) choiceId = "dismiss";

            ParleyOfferPrompt.Resolution res = ParleyOfferPrompt.Resolution.Dismiss;
            switch (choiceId)
            {
                case "parley_now":
                    res = ParleyOfferPrompt.Resolution.DemandParley;
                    DemandParleyForFaction(factionId);
                    break;
                case "open_trade":
                    res = ParleyOfferPrompt.Resolution.OpenTrade;
                    OpenTradeWithFaction(factionId);
                    break;
                default:
                    res = ParleyOfferPrompt.Resolution.Dismiss;
                    break;
            }

            ParleyOfferPromptField?.Resolve(res);
        }

        /// <summary>
        /// Hatch-dilemma prompt timeout: auto-apply the timeout resolution
        /// (default ForceDeconOutside) by raising the resolved signal.
        /// The ExpeditionSystem listens and applies the consequence.
        /// </summary>
        private void OnHatchDilemmaTimeout_Handle(HatchDilemmaResolvedSignal.Resolution resolution)
        {
            // Find the active expedition and raise the signal so the
            // ExpeditionSystem can apply the consequence. The prompt
            // already deactivated itself in Tick before firing OnTimeout.
            EventBus.Raise(new HatchDilemmaResolvedSignal(
                expeditionId: FindActiveHatchDilemmaExpeditionId(),
                choice: resolution));
        }

        /// <summary>
        /// Player (or AI) made a hatch-dilemma choice via the event
        /// modal. The OnChoiceApplied lambda already raised the
        /// HatchDilemmaResolvedSignal; here we just cancel the prompt
        /// timeout so the survivor doesn't wait indefinitely after the
        /// player has already chosen.
        /// </summary>
        private void OnHatchDilemmaChoiceApplied_Handle(HatchDilemmaResolvedSignal.Resolution resolution)
        {
            HatchDilemmaPromptField?.Cancel();
        }

        private void HandleChildFoundChoiceApplied(GameEvent ev, EventChoice choice, EventContext ctx)
        {
            if (ev == null || choice == null) return;
            if (ev.id != "child_found_in_ash") return;

            if (choice.ChoiceId == "take_the_child")
            {
                if (ChildSystem != null && !ChildSystem.WasChildFound)
                {
                    ChildSystem.CreateChild();
                    Debug.Log("[Child] The bunker has taken in the child. A fragile hope settles over the shelter.");
                }
            }

            // Either choice resolves the event — prevent re-triggering
            if (SaveSystem != null)
            {
                SaveSystem.SetWorldFlag(ChildDependentSystem.ChildFoundFlag, true);
            }
        }
    }
}
