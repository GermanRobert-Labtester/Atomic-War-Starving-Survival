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
        /// EventRunner.OnChoiceApplied listener: resolves side effects of the
        /// Safe Haven Broadcast event.
        /// </summary>
        private void HandleSafeHavenChoiceApplied(GameEvent ev, EventChoice choice, EventContext ctx)
        {
            if (ev == null || choice == null) return;
            if (ev.id != EventRunner.SafeHavenBroadcastEventId) return;

            switch (choice.ChoiceId)
            {
                case "warn_others":
                    ApplySafeHavenWarnOthers();
                    break;
                case "analyze_audio":
                case "analyze_audio_science":
                    // Reliability flip is applied by EventRunner.ApplySafeHavenIntelEffects
                    // during ApplyChoice (before this handler). Log only here.
                    GameLog.Log("[Safe Haven] Audio analyzed: the scrubber hum is a recorded loop. Trap confirmed.");
                    break;
                case "send_expedition":
                    ApplySafeHavenSendExpedition(ctx);
                    break;
            }
        }

        private void ApplySafeHavenWarnOthers()
        {
            // Transmission cost: pull from the radio tuner's fuel reserve.
            if (RadioTunerSystem?.State != null)
            {
                RadioTunerSystem.State.AvailableFuel = Mathf.Max(
                    0f, RadioTunerSystem.State.AvailableFuel - 5f);
            }

            // Karma/trust boost: every registered faction gets +3 trust.
            if (EconomySystem?.Factions != null)
            {
                foreach (var fac in EconomySystem.Factions.Values)
                {
                    if (fac == null) continue;
                    EconomySystem.ModifyTrust(fac.id, 3f);
                }
            }
            GameLog.Log("[Safe Haven] Broadcast warning transmitted. Radio fuel -5, all factions +3 trust.");
        }

        private void ApplySafeHavenSendExpedition(EventContext ctx)
        {
            // Prompt #47 — radio intel reliability drives which location
            // encounter is injected for the Safe Haven grid.
            if (EventRunner.ShouldInjectSafeHavenAmbush(ctx))
            {
                InjectSafeHavenAmbushEncounter();
                GameLog.Log("[Safe Haven] Unverified intel accepted. Sniper ambush injected at grid 4-7-North.");
            }
            else
            {
                InjectSafeHavenEmptyCacheEncounter();
                GameLog.Log("[Safe Haven] Trap confirmed. Empty-cache encounter injected — no sniper.");
            }
        }
    }
}
