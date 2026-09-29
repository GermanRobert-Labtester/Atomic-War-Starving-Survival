// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Clock;
using Ashfall.Core.Crafting;
using Ashfall.Core.Economy;
using Ashfall.Core.Endgame;
using Ashfall.Core.Events;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Flags;
using Ashfall.Core.Legacy;
using Ashfall.Core.Medical;
using Ashfall.Core.Muster;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;
using Ashfall.Core.Settings;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.UtilityAI;
using Ashfall.Core.Verdict;
using Ashfall.Core.Warlords;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using AtomicWar.GodotApp.Narrative;
using AtomicWar.GodotApp.Settings;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.YearOfAsh;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {

        /// <summary>Smoke-test ExpeditionEncounterBridge: bare-notice path + resolved path surface count.</summary>
        public static int RunExpeditionEncounterBridgeSelfTest()
        {
            int errors = 0;
            var log = new GodotLog();

            // Bare-notice path: no eligible encounter in catalog.
            var bareNarrative = new NarrativeEncounterSystem();
            var bareBridge = new ExpeditionEncounterBridge(bareNarrative, new SeededRng(1));
            int bareCount = 0;
            bareBridge.OnSurfaced += dto =>
            {
                bareCount++;
                if (dto.encounter_id != null || dto.resolved_at_lead != false || dto.choices.Count != 0)
                {
                    log.Error("[bridge-selftest] bare-notice DTO malformed.");
                    errors++;
                }
            };
            bareBridge.Surface(new ExpeditionState
            {
                survivorId = "sv",
                locationId = "loc",
                displayName = "Loc",
                stance = "Stealth",
                phase = (int)ExpeditionPhase.Outbound,
                encounterCount = 1,
                dangerLevel = 1
            });
            if (bareCount != 1) { log.Error("[bridge-selftest] expected 1 bare surfaced, got " + bareCount); errors++; }

            // Resolved path: catalog has one eligible encounter.
            var resolvedNarrative = new NarrativeEncounterSystem();
            resolvedNarrative.RegisterEncounter(new EncounterDefinition
            {
                id = "enc_bridge_smoke",
                title = "Bridge Smoke",
                description = "Smoke on the horizon.",
                category = "Discovery",
                baseWeight = 1f,
                minDangerLevel = 0f,
                choices = new System.Collections.Generic.List<EncounterChoiceDefinition>
                {
                    new EncounterChoiceDefinition { choiceId = "investigate", text = "Investigate", moraleDelta = 1, guiltDelta = 0 }
                }
            });
            var resolvedBridge = new ExpeditionEncounterBridge(resolvedNarrative, new SeededRng(42));
            int resolvedCount = 0;
            resolvedBridge.OnSurfaced += dto =>
            {
                resolvedCount++;
                if (dto.encounter_id != "enc_bridge_smoke" || dto.choices.Count != 1)
                {
                    log.Error("[bridge-selftest] resolved DTO malformed.");
                    errors++;
                }
            };
            resolvedBridge.Surface(new ExpeditionState
            {
                survivorId = "sv",
                locationId = "loc",
                displayName = "Loc",
                stance = "Stealth",
                phase = (int)ExpeditionPhase.Outbound,
                encounterCount = 1,
                dangerLevel = 1
            });
            if (resolvedCount != 1) { log.Error("[bridge-selftest] expected 1 resolved surfaced, got " + resolvedCount); errors++; }

            return EmitSummary("expedition_encounter_bridge_selftest", errors == 0, errors == 0 ? 0 : 1, details: errors == 0 ? "PASS" : $"FAIL ({errors})");
        }

    }
}
