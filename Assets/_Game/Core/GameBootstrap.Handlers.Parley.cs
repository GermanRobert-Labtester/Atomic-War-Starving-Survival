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
            var ctx = new EventContext(primary, Shelter, Inventory, CreateSaltedRng(_worldSeed, "parley_event"));
            EventRunner.Run(eventSo, ctx);
        }

        private void OnParleyOfferTimeout_Handle(ParleyOfferPrompt.Resolution resolution)
        {
            // Soft dismiss — they can still open trade later via [P] if repels hold.
            ParleyOfferPromptField?.Cancel();
        }

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

        private void OnHatchDilemmaTimeout_Handle(HatchDilemmaResolvedSignal.Resolution resolution)
        {
            // Find the active expedition and raise the signal so the
            // ExpeditionSystem can apply the consequence. The prompt
            // already deactivated itself in Tick before firing OnTimeout.
            EventBus.Raise(new HatchDilemmaResolvedSignal(
                expeditionId: FindActiveHatchDilemmaExpeditionId(),
                choice: resolution));
        }

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
                    GameLog.Log("[Child] The bunker has taken in the child. A fragile hope settles over the shelter.");
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
