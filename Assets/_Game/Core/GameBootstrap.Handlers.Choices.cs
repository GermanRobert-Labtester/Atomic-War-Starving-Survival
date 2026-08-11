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
                    GameLog.Log($"[Blood for Water] BloodLoss inflicted on {donor.DisplayName}.");
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
                    GameLog.Log($"[Blood for Water] Affinity {donor.DisplayName}↔{leader.DisplayName} slammed to {EventRunner.ForcedBleedAffinityFloor}.");
                }
            }
        }

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
                GameLog.Log($"[Hatch Entrapment] DigOut complete. Entry CO2={_entryRoom.Co2Ppm:F0} ppm.");
                return;
            }

            if (ev.id == EventRunner.FactionDigOutEventId
                && choice.ChoiceId == EventRunner.ChoiceAcceptFactionRescue)
            {
                HatchEntrapmentSystem.ApplyFactionRescue(ctx);
                SyncHatchExpeditionLock();
                GameLog.Log("[Hatch Entrapment] Faction dug the hatch open. Debt recorded.");
            }
        }

        private void HandleRaidPlanInterceptOffered(FactionRaidPlan plan, GameEvent ev)
        {
            if (ev == null || EventRunner == null) return;
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
            var ctx = BuildEventContext(day);
            EventRunner.Run(ev, ctx);
            GameLog.Log($"[Raid Plan] Wiretap offered: {plan?.AttackerFactionId} → {plan?.TargetFactionId}");
        }

        private void HandleRaidPlanChoiceApplied(GameEvent ev, EventChoice choice, EventContext ctx)
        {
            if (ev == null || choice == null || FactionRaidPlanSystem == null) return;
            if (string.IsNullOrEmpty(ev.id)
                || !ev.id.StartsWith(FactionRaidPlanSystem.EventIdPrefix, StringComparison.Ordinal))
                return;
            FactionRaidPlanSystem.ApplyChoiceFromEvent(ev, choice);
        }

        private void HandleFactionRescueApplied_ScheduleDebt(string factionId)
        {
            if (DebtCollectorSystem == null || string.IsNullOrEmpty(factionId)) return;
            if (DebtCollectorSystem.HasPendingDebtFor(factionId)) return;
            var entry = DebtCollectorSystem.ScheduleDebt(factionId);
            if (entry != null)
                GameLog.Log($"[Debt Collector] Scheduled for {factionId} on day {entry.CollectorDay}.");
        }

        private void HandleDebtCollectorArrived(DebtEntry debt, GameEvent ev)
        {
            if (ev == null || EventRunner == null) return;
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
            var ctx = BuildEventContext(day);
            EventRunner.Run(ev, ctx);
            GameLog.Log($"[Debt Collector] {debt?.FactionId} demands half fuel + half clean water.");
        }

        private void HandleDebtCollectorChoiceApplied(GameEvent ev, EventChoice choice, EventContext ctx)
        {
            if (ev == null || choice == null || DebtCollectorSystem == null) return;
            if (string.IsNullOrEmpty(ev.id)
                || !ev.id.StartsWith(DebtCollectorSystem.EventIdPrefix, StringComparison.Ordinal))
                return;
            DebtCollectorSystem.ApplyChoiceFromEvent(ev, choice, ctx);
        }

        /// <summary>Rations offered to buy off a mutiny's followers (Audit H-6g).</summary>
        private const int MutinyYieldRationUnits = 10;

        /// <summary>
        /// Audit H-6g: MutinySystem.ResolveNegotiate/ResolveYieldResources/ResolveExecute
        /// existed and were fully tested but nothing could ever call them — once morale
        /// crashed for a week and a mutiny started, MutinyActive had no path back to
        /// false. Presents the standoff as a GameEvent, reusing the existing choice
        /// pipeline instead of a bespoke panel.
        /// </summary>
        private void HandleMutinyStarted(Survivor leader)
        {
            if (EventRunner == null) return;
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
            var ctx = BuildEventContext(day);
            EventRunner.Run(EncounterEventFactory.CreateMutinyStandoff(), ctx);
        }

        private void HandleMutinyChoiceApplied(GameEvent ev, EventChoice choice, EventContext ctx)
        {
            if (ev == null || choice == null || BunkerSocial == null) return;
            if (ev.id != "shelter_mutiny_standoff") return;

            switch (choice.ChoiceId)
            {
                case "mutiny_negotiate":
                    BunkerSocial.ResolveMutinyNegotiate();
                    break;
                case "mutiny_yield":
                    if (!BunkerSocial.ResolveMutinyYield(MutinyYieldRationUnits))
                        GameLog.Log("[Mutiny] Not enough rations to buy them off — the standoff continues.");
                    break;
                case "mutiny_execute":
                    BunkerSocial.ResolveMutinyExecute();
                    break;
            }
        }

        /// <summary>
        /// Audit H-6g: ShelterAtmosphereSystem.ResolveCoLeakEvent existed and was
        /// fully tested but nothing ever called it — EventRunner applied the
        /// generic health/morale deltas for "shelter_co_leak" and stopped there,
        /// so The Canary questline could never actually complete.
        /// </summary>
        private void HandleCOLeakChoiceApplied(GameEvent ev, EventChoice choice, EventContext ctx)
        {
            if (ev == null || choice == null || AtmosphereSystem == null) return;
            if (ev.id != "shelter_co_leak") return;

            // "ventilate" fixes the leak before it poisons the rest of the bunker;
            // "ignore_co" rides it out and leaves everyone else at risk.
            bool ventilated = choice.ChoiceId == "ventilate";

            float healthLost = 0f;
            if (choice.Effects != null)
            {
                for (int i = 0; i < choice.Effects.Count; i++)
                {
                    var eff = choice.Effects[i];
                    if (eff != null && eff.TargetNeed == "health" && eff.NeedDelta < 0f)
                        healthLost += -eff.NeedDelta;
                }
            }

            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
            AtmosphereSystem.ResolveCoLeakEvent(healthLost, minerSavedAnother: ventilated, day);
        }

    }
}
