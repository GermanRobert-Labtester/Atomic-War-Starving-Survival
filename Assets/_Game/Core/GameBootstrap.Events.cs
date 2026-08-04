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
        /// Safe Haven Broadcast event. Specifically:
        ///  - <c>warn_others</c>: drains 5 fuel from the radio tuner (transmission
        ///    cost) and boosts trust with every registered faction by +3.
        ///  - <c>send_expedition</c>: if the broadcast was NOT verified as a
        ///    trap first, injects the Safe Haven ambush encounter into the
        ///    ExpeditionSystem so the next expedition to grid 4-7-North hits
        ///    a pre-positioned sniper. If the broadcast WAS verified, the
        ///    encounter pool is left clean — the player can scavenge the
        ///    empty cache without casualties.
        ///  - <c>analyze_audio</c> / <c>analyze_audio_science</c>: flips the
        ///    EventContext's ActiveIntelReliability to Trap on the running
        ///    context so downstream choices inherit the new reliability.
        /// </summary>
        private void HandleSafeHavenChoiceApplied(GameEvent ev, EventChoice choice, EventContext ctx)
        {
            if (ev == null || choice == null) return;
            if (ev.id != EventRunner.SafeHavenBroadcastEventId) return;

            if (choice.ChoiceId == "warn_others")
            {
                // Transmission cost: pull from the radio tuner's fuel reserve.
                if (RadioTunerSystem != null && RadioTunerSystem.State != null)
                {
                    RadioTunerSystem.State.AvailableFuel = Mathf.Max(
                        0f, RadioTunerSystem.State.AvailableFuel - 5f);
                }
                // Karma/trust boost: every registered faction gets +3 trust.
                if (EconomySystem != null && EconomySystem.Factions != null)
                {
                    foreach (var fac in EconomySystem.Factions.Values)
                    {
                        if (fac == null) continue;
                        EconomySystem.ModifyTrust(fac.id, 3f);
                    }
                }
                Debug.Log("[Safe Haven] Broadcast warning transmitted. Radio fuel -5, all factions +3 trust.");
                return;
            }

            if (choice.ChoiceId == "analyze_audio" || choice.ChoiceId == "analyze_audio_science")
            {
                // Reliability flip is applied by EventRunner.ApplySafeHavenIntelEffects
                // during ApplyChoice (before this handler). Log only here.
                Debug.Log("[Safe Haven] Audio analyzed: the scrubber hum is a recorded loop. Trap confirmed.");
                return;
            }

            if (choice.ChoiceId == "send_expedition")
            {
                // Prompt #47 — radio intel reliability drives which location
                // encounter is injected for the Safe Haven grid.
                if (EventRunner.ShouldInjectSafeHavenAmbush(ctx))
                {
                    InjectSafeHavenAmbushEncounter();
                    Debug.Log("[Safe Haven] Unverified intel accepted. Sniper ambush injected at grid 4-7-North.");
                }
                else
                {
                    InjectSafeHavenEmptyCacheEncounter();
                    Debug.Log("[Safe Haven] Trap confirmed. Empty-cache encounter injected — no sniper.");
                }
                return;
            }
        }

    }
}
