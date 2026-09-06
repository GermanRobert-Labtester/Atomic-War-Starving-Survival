using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Ashfall.Core.Random;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    /// <summary>
    /// Task F24: Headless Lifecycle Smoke Test for Micro-Locations.
    ///
    /// Validates:
    /// 1. 8-tick seeded organic selection trace using ExpeditionEncounterBridge + ExpeditionSystem.
    /// 2. Deterministic crashed-truck lifecycle:
    ///    - Selection of micro_crashed_truck
    ///    - Resolution via choice search_truck_cargo
    ///    - Grant of +2 canned_food to active sortie pack
    ///    - Depletion recording in NarrativeEncounterState.history and depletedEncounterIds
    ///    - Save to JSON envelope & restore into fresh session
    ///    - Verification that micro_crashed_truck is excluded from future eligibility and no duplicate is surfaced.
    /// </summary>
    public class MicroLocationLifecycleSmokeTests
    {
        private const string CrashedTruckId = "micro_crashed_truck";
        private const string SearchCargoChoiceId = "search_truck_cargo";
        private const string CannedFoodId = "canned_food";
        private const string SurvivorId = "survivor_lead_scavenger";
        private const string DestinationLocationId = "loc_denial_cut_substation";

        private static string GetDataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent!;
            return Path.Combine(dir!.FullName, "Assets", "StreamingAssets", "Data");
        }

        private static (NarrativeEncounterSystem narrative, ExpeditionSystem engine, ExpeditionEncounterBridge bridge) CreateProductionTrio(int seed)
        {
            string dataDir = GetDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var narrative = new NarrativeEncounterSystem();
            narrative.RegisterRange(NarrativeEncounterCatalogLoader.Load(dataDir, fileIO, json));

            var engine = new ExpeditionSystem();
            var scavenging = ScavengingTableCatalog.LoadFromDirectory(dataDir, fileIO, json);
            if (scavenging != null) engine.ScavengingCatalog = scavenging;

            var loaded = ExpeditionCatalogLoader.Load(dataDir, fileIO, json);
            Assert.NotNull(loaded);
            foreach (var def in loaded!)
            {
                if (def != null && !string.IsNullOrEmpty(def.id))
                    ExpeditionDefinitionRegistry.Register(def);
            }

            var rng = new SeededRng(seed);
            var bridge = new ExpeditionEncounterBridge(narrative, rng);
            engine.OnEncounterTriggered += bridge.Surface;

            return (narrative, engine, bridge);
        }

        private static void SetLastSurfaced(ExpeditionEncounterBridge bridge, ExpeditionEncounterBridge.EncounterSurfaced dto)
        {
            var field = typeof(ExpeditionEncounterBridge).GetField("_lastSurfaced", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(field);
            field!.SetValue(bridge, dto);
        }

        // ── Test A: 8-tick seeded organic selection trace ──────────────────

        [Fact]
        public void F24_TestA_EightTick_SeededOrganicSelectionTrace_ExecutesDeterministically()
        {
            const int seed = 42;
            var runA = MicroLocationDeterminismHarness.Run(seed, DestinationLocationId, 8);
            var runB = MicroLocationDeterminismHarness.Run(seed, DestinationLocationId, 8);

            Assert.Equal(8, runA.TicksRun);
            Assert.Equal(8, runB.TicksRun);
            Assert.NotEmpty(runA.Trace);

            // Verify traces match 100% identically
            MicroLocationDeterminismHarness.AssertTracesEqual(runA, runB, "8-tick organic trace determinism");

            // Verify that trace contains valid tick data and state progression
            for (int i = 0; i < runA.Trace.Count; i++)
            {
                var entry = runA.Trace[i];
                Assert.Equal(i + 1, entry.Tick);
                Assert.True(entry.RngDrawsAfter >= 0);
            }
        }

        // ── Test B: Deterministic crashed-truck lifecycle ───────────────────

        [Fact]
        public void F24_TestB_CrashedTruck_FullLifecycle_SelectResolveGrantDepleteSaveRestore()
        {
            const int seed = 101;
            var (narrative, engine, bridge) = CreateProductionTrio(seed);

            // Step 1: Start expedition
            var expDef = ExpeditionDefinitionRegistry.Get(DestinationLocationId);
            Assert.NotNull(expDef);
            engine.DiscoverLocation(DestinationLocationId);
            bool started = engine.Start(expDef!, SurvivorId, day: 1, ExpeditionStance.Stealth);
            Assert.True(started, "Failed to start expedition");

            var activeSortie = engine.Active[SurvivorId];
            Assert.NotNull(activeSortie);
            Assert.Empty(activeSortie.loot);

            // Step 2: Surface micro_crashed_truck
            var truckDef = narrative.Find(CrashedTruckId);
            Assert.NotNull(truckDef);
            Assert.True(truckDef!.isMicroLocation);

            // Simulate surface via bridge
            var surfacedDto = new ExpeditionEncounterBridge.EncounterSurfaced
            {
                encounter_id = truckDef.id,
                title = truckDef.title,
                description = truckDef.description,
                category = truckDef.category,
                is_micro_location = truckDef.isMicroLocation,
                choices = truckDef.choices,
                trigger = activeSortie
            };
            SetLastSurfaced(bridge, surfacedDto);
            narrative.RecordEncounterSelected(truckDef);

            // Verify surfaced choice exists
            var choice = surfacedDto.choices.FirstOrDefault(c => c.choiceId == SearchCargoChoiceId);
            Assert.NotNull(choice);
            Assert.Equal(CannedFoodId, choice!.grantItemId);
            Assert.Equal(2, choice.grantItemQuantity);
            Assert.Equal(1, choice.moraleDelta);
            Assert.True(choice.depletesOnResolve);

            // Step 3: Resolve choice through bridge
            bool resolved = bridge.ResolveChoice(CrashedTruckId, SearchCargoChoiceId, day: 1, DestinationLocationId);
            Assert.True(resolved, "Failed to resolve micro_crashed_truck choice via bridge");
            Assert.True(surfacedDto.resolved_at_lead);

            var res = bridge.LastResolution;
            Assert.NotNull(res);
            Assert.Equal(CrashedTruckId, res!.EncounterId);
            Assert.Equal(SearchCargoChoiceId, res.ChoiceId);
            Assert.Equal(CannedFoodId, res.GrantItemId);
            Assert.Equal(2, res.GrantItemQuantity);
            Assert.Equal(1, res.MoraleDelta);
            Assert.Equal(0, res.GuiltDelta);
            Assert.True(res.DepletesEncounter);

            // Step 4: Grant loot to active sortie pack via canonical host route
            var grantStatus = engine.TryGrantLoot(SurvivorId, res.GrantItemId, weightKgPerUnit: 0.5f, res.GrantItemQuantity);
            Assert.Equal(ExpeditionSystem.LootGrantStatus.Granted, grantStatus);

            // Verify loot was added to active sortie
            Assert.Single(activeSortie.loot);
            var lootEntry = activeSortie.loot[0];
            Assert.Equal(CannedFoodId, lootEntry.itemId);
            Assert.Equal(2, lootEntry.quantity);

            // Step 5: Depletion recording in narrative state
            Assert.True(narrative.IsDepleted(CrashedTruckId));
            Assert.Equal(1, narrative.TotalResolved);
            Assert.Single(narrative.State.history);
            Assert.Equal(CrashedTruckId, narrative.State.history[0].encounterId);
            Assert.Equal(SearchCargoChoiceId, narrative.State.history[0].choiceId);

            // Step 6: Save to JSON envelope & restore into fresh session
            var capturedState = narrative.CaptureState();
            Assert.Contains(CrashedTruckId, capturedState.depletedEncounterIds);

            var jsonSerializer = new SystemTextJsonSerializer();
            string stateJson = jsonSerializer.Serialize(capturedState);
            string checksum = SaveChecksum.Compute(capturedState);

            // Envelope shape { "State": ..., "Checksum": ... }
            var envelope = new ChecksumEnvelope<NarrativeEncounterState>
            {
                State = capturedState,
                Checksum = checksum
            };
            string envelopeJson = jsonSerializer.Serialize(envelope);

            // Restore from envelope into a completely fresh NarrativeEncounterSystem
            var restoredEnvelope = jsonSerializer.Deserialize<ChecksumEnvelope<NarrativeEncounterState>>(envelopeJson);
            Assert.NotNull(restoredEnvelope);
            Assert.NotNull(restoredEnvelope!.State);
            Assert.Equal(checksum, SaveChecksum.Compute(restoredEnvelope.State!));

            var freshNarrative = new NarrativeEncounterSystem();
            freshNarrative.RegisterRange(NarrativeEncounterCatalogLoader.Load(GetDataDir(), new FileSystemIO(), jsonSerializer));
            freshNarrative.RestoreState(restoredEnvelope.State!);

            // Step 7: Verification that micro_crashed_truck is depleted and excluded from eligibility
            Assert.True(freshNarrative.IsDepleted(CrashedTruckId), "Depleted state must persist across restore");
            Assert.Equal(1, freshNarrative.TotalResolved);
            Assert.Single(freshNarrative.State.history);

            // Check across all stances and danger levels that CrashedTruck is never eligible
            string[] stances = { "Stealth", "Speed", "Cautious", "Aggressive" };
            for (int danger = 1; danger <= 5; danger++)
            {
                foreach (var stance in stances)
                {
                    var candidates = freshNarrative.GetEligibleCandidates(stance, danger, DestinationLocationId);
                    Assert.DoesNotContain(candidates, c => c.def.id == CrashedTruckId);
                }
            }

            // Monte Carlo check: 100 random seeds never select the depleted micro-location
            for (int s = 0; s < 100; s++)
            {
                var selected = freshNarrative.SelectEncounter("Stealth", 2f, DestinationLocationId, new SeededRng(s));
                Assert.NotEqual(CrashedTruckId, selected?.id);
            }
        }

        // ── Test C: Duplicate resolution prevention ─────────────────────────

        [Fact]
        public void F24_TestC_DuplicateResolution_CannotReGrantOrReResolve()
        {
            const int seed = 202;
            var (narrative, engine, bridge) = CreateProductionTrio(seed);

            var truckDef = narrative.Find(CrashedTruckId);
            Assert.NotNull(truckDef);

            var state = new ExpeditionState
            {
                survivorId = SurvivorId,
                locationId = DestinationLocationId,
                stance = "Stealth",
                dangerLevel = 1
            };

            var surfacedDto = new ExpeditionEncounterBridge.EncounterSurfaced
            {
                encounter_id = truckDef!.id,
                title = truckDef.title,
                description = truckDef.description,
                category = truckDef.category,
                is_micro_location = truckDef.isMicroLocation,
                choices = truckDef.choices,
                trigger = state
            };
            SetLastSurfaced(bridge, surfacedDto);

            // First resolution succeeds
            bool firstResolve = bridge.ResolveChoice(CrashedTruckId, SearchCargoChoiceId, day: 1, DestinationLocationId);
            Assert.True(firstResolve);
            Assert.True(surfacedDto.resolved_at_lead);
            Assert.True(narrative.IsDepleted(CrashedTruckId));

            // Immediate second resolution attempt through bridge on same encounter
            bool secondResolve = bridge.ResolveChoice(CrashedTruckId, SearchCargoChoiceId, day: 1, DestinationLocationId);
            Assert.False(secondResolve, "Bridge must reject re-resolving an already resolved surfaced encounter");
            Assert.Null(bridge.LastResolution);

            // Ensure narrative history only recorded the choice once
            Assert.Single(narrative.State.history);
        }

        private sealed class ChecksumEnvelope<T> where T : class
        {
            public T? State { get; set; }
            public string Checksum { get; set; } = string.Empty;
        }
    }
}
