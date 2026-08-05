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
        // -----------------------------------------------------------------
        // Day-30 Flashpoint: Hatch Dilemma
        // -----------------------------------------------------------------

        /// <summary>
        /// Day-30 hatch dilemma handler. Called by the ExpeditionSystem when
        /// a comms-severed expedition enters the AtHatchDilemma phase. Builds
        /// a forced GameEventSO with three player choices, runs it through
        /// the EventRunner, and forwards the choice back via the typed
        /// HatchDilemmaResolvedSignal (which the ExpeditionSystem listens to).
        /// </summary>
        private void OnHatchDilemmaReady_Handle(ExpeditionState exp)
        {
            if (exp == null || EventRunner == null) return;

            string survivorName = exp.Survivor != null ? exp.Survivor.DisplayName : "the survivor";
            var eventSo = ScriptableObject.CreateInstance<GameEvent>();
            eventSo.id = $"evt_hatch_dilemma_{exp.ExpeditionId}";
            eventSo.title = "Knock at the Hatch";
            eventSo.bodyText =
                $"{survivorName} is at the door. Their suit and gear are soaked in fallout, " +
                "and the dosimeter on their chest is screaming. You have one chance to " +
                "decide what happens next.";
            eventSo.weight = 1f;
            eventSo.conditions = new EventConditions { MinDay = 1 };
            eventSo.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "let_them_in",
                    Text = "Open the hatch. They are family.",
                    MoraleDelta = +5f
                },
                new EventChoice
                {
                    ChoiceId = "force_decon",
                    Text = "Force them to strip and decontaminate outside.",
                    MoraleDelta = -15f
                },
                new EventChoice
                {
                    ChoiceId = "deny_entry",
                    Text = "Do not open the hatch.",
                    MoraleDelta = -30f
                }
            };

            // Start the prompt (begins the timeout). On timeout, the
            // prompt fires OnTimeout which auto-resolves with
            // ForceDeconOutside (the safest option).
            if (HatchDilemmaPromptField != null)
            {
                HatchDilemmaPromptField.Begin(exp);
                HatchDilemmaPromptField.OnTimeout -= OnHatchDilemmaTimeout_Handle;
                HatchDilemmaPromptField.OnTimeout += OnHatchDilemmaTimeout_Handle;
                HatchDilemmaPromptField.OnChoiceApplied -= OnHatchDilemmaChoiceApplied_Handle;
                HatchDilemmaPromptField.OnChoiceApplied += OnHatchDilemmaChoiceApplied_Handle;
            }

            // Translate the EventRunner's choice event into our typed signal.
            string expeditionId = exp.ExpeditionId;
            Action<GameEvent, EventChoice, EventContext> onChoice = null;
            onChoice = (gameEvent, choice, ctx) =>
            {
                if (gameEvent == null || choice == null) return;
                if (gameEvent.id != eventSo.id) return;
                HatchDilemmaResolvedSignal.Resolution resolution = HatchDilemmaResolvedSignal.Resolution.LetThemIn;
                switch (choice.ChoiceId)
                {
                    case "let_them_in":
                        resolution = HatchDilemmaResolvedSignal.Resolution.LetThemIn;
                        break;
                    case "force_decon":
                        resolution = HatchDilemmaResolvedSignal.Resolution.ForceDeconOutside;
                        break;
                    case "deny_entry":
                        resolution = HatchDilemmaResolvedSignal.Resolution.DenyEntry;
                        break;
                }
                EventBus.Raise(new HatchDilemmaResolvedSignal(expeditionId, resolution));
                // Stop the timeout so we do not double-resolve on Tick.
                HatchDilemmaPromptField?.ApplyChoice(resolution);
                EventRunner.OnChoiceApplied -= onChoice;
            };
            EventRunner.OnChoiceApplied += onChoice;

            // Build a minimal EventContext (no choices to apply directly; the
            // side effects are handled by the ExpeditionSystem on the resolve
            // signal). Run so the event modal is presented to the player.
            var ctx = new EventContext(exp.Survivor, Shelter, Inventory, CreateSaltedRng(_worldSeed, "hatch_event"));
            EventRunner.Run(eventSo, ctx);
        }

        /// <summary>
        /// Keep expedition hard-lock + map UI in sync with HatchState.
        /// </summary>
        private void SyncHatchExpeditionLock()
        {
            bool locked = HatchEntrapmentSystem != null && HatchEntrapmentSystem.AreExpeditionsLocked;
            if (ExpeditionSystem != null)
                ExpeditionSystem.HatchBlocksExpeditions = locked;
            if (_hud != null && _hud.MapScreenUI != null)
                _hud.MapScreenUI.IsExpeditionUiEnabled = !locked;
        }

    }
}
