// SPDX-License-Identifier: MIT
// ASHFALL End-to-End Player Journey Test Suite (Task 110).
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Journeys;
using Ashfall.Core.Medical;
using Ashfall.Core.Save;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests.Journeys
{
    public sealed class EndToEndPlayerJourneyTests
    {
        private sealed class TestSurvivorAuthor : ISurvivorAuthor
        {
            public string Id { get; }
            public string DisplayName { get; }
            public RiskBiasTrait RiskBias { get; }

            public TestSurvivorAuthor(string id, string displayName, RiskBiasTrait riskBias = RiskBiasTrait.Realist)
            {
                Id = id;
                DisplayName = displayName;
                RiskBias = riskBias;
            }
        }

        public EndToEndPlayerJourneyTests()
        {
            PanelRegistryBootstrap.RegisterAll();
        }

        [Fact]
        public void ShelterSurvivalJourney_Seed42_RunsSavesReloadsAndCompletes()
        {
            const ulong seed = 42;
            var ctx = new JourneyExecutionContext("ShelterSurvivalJourney", seed);
            var rng = new SeededRng((int)seed);
            var log = NullLog.Instance;
            var json = new SystemTextJsonSerializer();

            // ── Day 1: New Game Bootstrap ──────────────────────────────────────────
            ctx.Navigate("main_menu", "New Game Bootstrap");
            var survivors = new List<SurvivorNeedsState>
            {
                new() { Id = "survivor_alice", Health = 100f, Hunger = 10f, Thirst = 10f },
                new() { Id = "survivor_bob", Health = 100f, Hunger = 15f, Thirst = 15f },
                new() { Id = "survivor_clara", Health = 100f, Hunger = 10f, Thirst = 10f },
                new() { Id = "survivor_david", Health = 100f, Hunger = 20f, Thirst = 20f }
            };

            var inventory = new Ashfall.Core.Inventory.Inventory();
            inventory.AddById("item_ration_standard", 12);
            inventory.AddById("item_water_purified", 12);
            inventory.AddById("item_scrap_metal", 20);
            inventory.AddById("item_cloth", 10);

            var needs = new NeedsSystem();
            foreach (var s in survivors) needs.Register(s);

            var water = new WaterTreatmentSystem(log);
            var dutyRoster = new DutyRosterSystem();
            var skills = new SkillProgressionSystem();
            var relations = new SurvivorRelationsSystem(rng);
            var apprenticeship = new ApprenticeshipSystem(rng, skills, dutyRoster, relations, log);

            // ── Day 1 Actions: Shelter Allocation & Water Treatment ─────────────────
            ctx.Navigate("duty_roster", "Assign Labor Shifts");
            dutyRoster.State.expansionUnlocked = true;
            dutyRoster.State.assignments.Add(new DutyRosterAssignmentEntry { role = "role_cook", survivorId = "survivor_bob" });
            dutyRoster.State.assignments.Add(new DutyRosterAssignmentEntry { role = "role_crafter", survivorId = "survivor_clara" });

            ctx.Navigate("water_treatment", "Start Charcoal Filtration");
            water.State.charcoalSupply = 10f;
            water.AddWater(WaterType.Raw, 20f);
            var waterStart = water.StartTreatment(TreatmentMode.CharcoalFiltration, 10f);
            Assert.True(waterStart.IsSuccess, ctx.FormatFailureDiagnostic("Water treatment failed to start."));

            ctx.Navigate("crafting", "Queue Bandage Craft");
            inventory.RemoveById("item_cloth", 2);
            inventory.AddById("item_bandage", 2);

            // ── Advance to Day 2 ───────────────────────────────────────────────────
            ctx.AdvanceDay(2);
            water.TickDay(2);
            needs.Tick(24f);
            foreach (var s in survivors)
            {
                needs.Modify(s.Id, NeedKind.Hunger, -15f);
                needs.Modify(s.Id, NeedKind.Thirst, -20f);
            }
            inventory.RemoveById("item_ration_standard", 4);
            inventory.RemoveById("item_water_purified", 4);

            // ── Advance to Day 3 ───────────────────────────────────────────────────
            ctx.AdvanceDay(3);

            // ── Midpoint Save: Capture Payloads & Build Aggregate Envelope ───────────
            ctx.Navigate("save", "Midpoint Campaign Save");
            var payloads = new Dictionary<string, string>
            {
                ["survivors"] = json.Serialize(survivors),
                ["inventory"] = json.Serialize(inventory.CaptureState()),
                ["duty_roster"] = json.Serialize(dutyRoster.CaptureState()),
                ["water_treatment"] = json.Serialize(water.CaptureState()),
                ["campaign_day"] = json.Serialize(new { day = 3 })
            };

            var manifest = new SaveManifest { manifestVersion = 2, currentDay = 3, seed = (int)seed };
            var envelope = CampaignEnvelopeBuilder.Build(payloads, manifest);

            Assert.NotNull(envelope);
            Assert.Equal(2, envelope.manifestVersion);
            Assert.Equal(5, envelope.sections.Count);

            // ── Day 3: Reload into completely fresh host session ───────────────────
            ctx.Navigate("save", "Reload from Campaign Envelope");
            var freshSurvivors = json.Deserialize<List<SurvivorNeedsState>>(
                envelope.sections.First(s => s.sectionName == "survivors").payloadJson)!;
            var freshInventory = new Ashfall.Core.Inventory.Inventory();
            freshInventory.RestoreState(
                json.Deserialize<InventorySaveState>(
                    envelope.sections.First(s => s.sectionName == "inventory").payloadJson)!,
                id => new ItemDefinition { id = id, displayName = id, stackMax = 99 });
            var freshDutyRoster = new DutyRosterSystem();
            freshDutyRoster.RestoreState(json.Deserialize<DutyRosterSystemState>(
                envelope.sections.First(s => s.sectionName == "duty_roster").payloadJson)!);
            var freshWater = new WaterTreatmentSystem(log);
            freshWater.RestoreState(json.Deserialize<WaterTreatmentState>(
                envelope.sections.First(s => s.sectionName == "water_treatment").payloadJson)!);

            var freshNeeds = new NeedsSystem();
            foreach (var s in freshSurvivors) freshNeeds.Register(s);

            // Assert exact midpoint state restoration
            Assert.Equal(4, freshSurvivors.Count);
            Assert.Equal(8, freshInventory.CountById("item_ration_standard"));
            Assert.Equal(8, freshInventory.CountById("item_water_purified"));
            Assert.Equal(2, freshInventory.CountById("item_bandage"));
            Assert.True(freshDutyRoster.State.expansionUnlocked);
            Assert.Equal(2, freshDutyRoster.State.assignments.Count);

            // ── Day 3 -> 5: Concluding Phase of Journey ────────────────────────────
            ctx.Navigate("apprenticeship", "Start Medical Apprenticeship");
            skills.RecordAction(new SimpleSkillActor("survivor_alice"), "medical", 50f, 3, rng);
            var freshApprenticeship = new ApprenticeshipSystem(rng, skills, freshDutyRoster, relations, log);
            var appResult = freshApprenticeship.StartPair("survivor_alice", "survivor_david", "medical");
            Assert.True(appResult.IsSuccess, ctx.FormatFailureDiagnostic("Failed to start apprenticeship pair."));

            ctx.AdvanceDay(4);
            freshApprenticeship.TickDay(4);
            freshNeeds.Tick(24f);

            ctx.AdvanceDay(5);
            freshApprenticeship.TickDay(5);

            // ── Final Journey Assertions ───────────────────────────────────────────
            Assert.All(freshSurvivors, s => Assert.True(s.IsAliveState, ctx.FormatFailureDiagnostic($"Survivor {s.Id} died unexpectedly.")));
            Assert.True(freshApprenticeship.State.activePairs[0].progressXp > 0f, ctx.FormatFailureDiagnostic("Apprentice gained zero XP."));
            Assert.Equal(5, ctx.Day);
        }

        [Fact]
        public void ExpeditionCombatJourney_Seed1986_RunsSavesReloadsAndCompletes()
        {
            const ulong seed = 1986;
            var ctx = new JourneyExecutionContext("ExpeditionCombatJourney", seed);
            var log = NullLog.Instance;
            var json = new SystemTextJsonSerializer();

            // ── Day 1: Expedition Gear Provisioning ────────────────────────────────
            ctx.Navigate("expeditions", "Provision Expedition Party");
            var inventory = new Ashfall.Core.Inventory.Inventory();
            inventory.AddById("item_ammo_standard", 50);
            inventory.AddById("item_medkit", 4);
            inventory.AddById("item_ration_standard", 10);

            var expSystem = new ExpeditionSystem();
            var journal = new JournalSystem();

            var expState = new ExpeditionState
            {
                expeditionId = "exp_alpha",
                survivorId = "survivor_scout",
                locationId = "loc_scavenge_ruins",
                startedDay = 1,
                distanceTicks = 8,
                phase = (int)ExpeditionPhase.Outbound
            };
            expSystem.RestoreState(new List<ExpeditionState> { expState });

            // ── Day 2: Arrival at Ruins & Combat Ambush ─────────────────────────────
            ctx.AdvanceDay(2);
            expState.travelTicksCompleted = 8;
            expState.phase = (int)ExpeditionPhase.Looting;

            ctx.Navigate("combat", "Resolve Hostile Ambush");
            int ammoUsed = 12;
            inventory.RemoveById("item_ammo_standard", ammoUsed);

            // Deterministic combat outcome: victory with collected loot
            expState.loot.Add(new ExpeditionLootEntry { itemId = "item_scrap_metal", quantity = 25, weightKg = 12.5f });
            expState.loot.Add(new ExpeditionLootEntry { itemId = "item_electronics", quantity = 2, weightKg = 1.0f });
            expSystem.RestoreState(new List<ExpeditionState> { expState });

            // ── Midpoint Save: Capture Expedition & Journal State ──────────────────
            ctx.Navigate("save", "Midpoint Campaign Save");
            var payloads = new Dictionary<string, string>
            {
                ["expedition"] = json.Serialize(expSystem.CaptureState()),
                ["inventory"] = json.Serialize(inventory.CaptureState()),
                ["journal"] = json.Serialize(journal.CaptureState())
            };

            var manifest = new SaveManifest { manifestVersion = 2, currentDay = 2, seed = (int)seed };
            var envelope = CampaignEnvelopeBuilder.Build(payloads, manifest);

            Assert.NotNull(envelope);
            Assert.Equal(3, envelope.sections.Count);

            // ── Day 2: Reload into Fresh Session ───────────────────────────────────
            ctx.Navigate("save", "Reload into Fresh Expedition Host");
            var freshExpList = json.Deserialize<List<ExpeditionState>>(
                envelope.sections.First(s => s.sectionName == "expedition").payloadJson)!;
            var freshInventory = new Ashfall.Core.Inventory.Inventory();
            freshInventory.RestoreState(
                json.Deserialize<InventorySaveState>(
                    envelope.sections.First(s => s.sectionName == "inventory").payloadJson)!,
                id => new ItemDefinition { id = id, displayName = id, stackMax = 99 });

            var reloadedExp = freshExpList.First();
            Assert.Equal("exp_alpha", reloadedExp.expeditionId);
            Assert.Equal(25, reloadedExp.loot.First(l => l.itemId == "item_scrap_metal").quantity);
            Assert.Equal(38, freshInventory.CountById("item_ammo_standard"));

            // ── Day 3 -> 4: Return Journey & Loot Unload ───────────────────────────
            ctx.AdvanceDay(3);
            reloadedExp.phase = (int)ExpeditionPhase.Inbound;

            ctx.AdvanceDay(4);
            reloadedExp.phase = (int)ExpeditionPhase.Completed;

            // Unload loot into base shelter inventory
            freshInventory.AddById("item_scrap_metal", 25);
            freshInventory.AddById("item_electronics", 2);

            ctx.Navigate("journal", "Record Expedition Return Log");
            var author = new TestSurvivorAuthor("survivor_scout", "Scout Miller", RiskBiasTrait.Realist);
            var entry = journal.TryDiscover("flag_expedition_ruins_concluded", author, 4);

            // ── Final Journey Assertions ───────────────────────────────────────────
            Assert.Equal(25, freshInventory.CountById("item_scrap_metal"));
            Assert.Equal(2, freshInventory.CountById("item_electronics"));
            Assert.NotNull(entry);
            Assert.Equal(4, ctx.Day);
        }

        [Fact]
        public void FactionMedicalJourney_Seed2026_RunsSavesReloadsAndCompletes()
        {
            const ulong seed = 2026;
            var ctx = new JourneyExecutionContext("FactionMedicalJourney", seed);
            var rng = new SeededRng((int)seed);
            var log = NullLog.Instance;
            var json = new SystemTextJsonSerializer();

            // ── Day 1: Medical Triage Bootstrap ────────────────────────────────────
            ctx.Navigate("medical_ward", "Admit Patient to Medical Ward");
            var wardState = new MedicalWardState();
            var beds = new List<MedicalBed>
            {
                new() { BedId = "bed_icu_1", Category = MedicalBedCategory.General, Isolation = false },
                new() { BedId = "bed_gen_1", Category = MedicalBedCategory.General, Isolation = false }
            };
            var procs = new List<MedicalProcedureDef>
            {
                new("proc_detox_stabilize", "Detox Stabilization", "ChemicalDependencySystem", null, 48f)
            };

            var medWard = new MedicalWardSystem(wardState, beds, procs);
            var chem = new ChemicalDependencySystem();
            var airlock = new AirlockSecuritySystem(rng, log);
            var inventory = new Ashfall.Core.Inventory.Inventory();
            inventory.AddById("item_herbs_medicinal", 15);

            // Admit injured/dependent patient
            var admitRes = medWard.Admit("survivor_patient", "bed_icu_1", 1);
            Assert.True(admitRes.Succeeded, ctx.FormatFailureDiagnostic("Failed to admit patient to medical ward."));
            chem.OnSubstanceConsumed("survivor_patient", "item_sedative_pill", ChemicalDependencyKind.Sedative);
            chem.OnSubstanceConsumed("survivor_patient", "item_sedative_pill", ChemicalDependencyKind.Sedative);
            bool detoxStarted = chem.BeginManagedDetox("survivor_patient", "item_sedative_pill");
            Assert.True(detoxStarted, ctx.FormatFailureDiagnostic("Failed to start managed detox."));

            // ── Day 2: Airlock Visitor Encounter from Meridian Compact ─────────────
            ctx.AdvanceDay(2);
            ctx.Navigate("airlock_security", "Handle Meridian Emissary Trade");
            airlock.VisitorArrives("visitor_meridian_trader", "Meridian Merchant");
            var resIncident = airlock.ResolveIncident(VisitorDecision.Admit);
            Assert.True(resIncident.IsSuccess);

            // Barter herbs for sterile antiseptics
            inventory.RemoveById("item_herbs_medicinal", 10);
            inventory.AddById("item_antiseptic", 10);

            // Advance detox & treatment
            chem.TickHours("survivor_patient", 24f);

            // ── Midpoint Save: Capture Medical & Airlock State ─────────────────────
            ctx.Navigate("save", "Midpoint Campaign Save");
            var payloads = new Dictionary<string, string>
            {
                ["medical"] = json.Serialize(medWard.CaptureState()),
                ["chemical_dependency"] = json.Serialize(chem.CaptureState()),
                ["airlock_security"] = json.Serialize(airlock.CaptureState()),
                ["inventory"] = json.Serialize(inventory.CaptureState())
            };

            var manifest = new SaveManifest { manifestVersion = 2, currentDay = 2, seed = (int)seed };
            var envelope = CampaignEnvelopeBuilder.Build(payloads, manifest);

            Assert.NotNull(envelope);
            Assert.Equal(4, envelope.sections.Count);

            // ── Day 2: Reload into Fresh Session ───────────────────────────────────
            ctx.Navigate("save", "Reload into Fresh Medical Host");
            var freshWardState = json.Deserialize<MedicalWardState>(
                envelope.sections.First(s => s.sectionName == "medical").payloadJson)!;
            var freshChem = new ChemicalDependencySystem();
            freshChem.RestoreState(json.Deserialize<ChemicalDependencyLedgerState>(
                envelope.sections.First(s => s.sectionName == "chemical_dependency").payloadJson)!);
            var freshInventory = new Ashfall.Core.Inventory.Inventory();
            freshInventory.RestoreState(
                json.Deserialize<InventorySaveState>(
                    envelope.sections.First(s => s.sectionName == "inventory").payloadJson)!,
                id => new ItemDefinition { id = id, displayName = id, stackMax = 99 });

            var freshMedWard = new MedicalWardSystem(freshWardState, beds, procs);

            // Assert exact restored state
            Assert.Single(freshWardState.Admissions);
            Assert.Equal("survivor_patient", freshWardState.Admissions[0].PatientId);
            Assert.Equal(10, freshInventory.CountById("item_antiseptic"));
            Assert.Equal(5, freshInventory.CountById("item_herbs_medicinal"));

            // ── Day 3 -> 4: Complete Treatment & Discharge Patient ──────────────────
            ctx.AdvanceDay(3);
            freshChem.TickHours("survivor_patient", 24f);

            ctx.AdvanceDay(4);
            freshChem.TickHours("survivor_patient", 24f);

            ctx.Navigate("medical_ward", "Discharge Healed Patient");
            var dischargeRes = freshMedWard.Discharge("survivor_patient", 4);
            Assert.True(dischargeRes.Succeeded, ctx.FormatFailureDiagnostic("Failed to discharge patient."));

            // ── Final Journey Assertions ───────────────────────────────────────────
            Assert.Equal(MedicalAdmissionStatus.Discharged, freshWardState.Admissions[0].Status);
            Assert.Equal(10, freshInventory.CountById("item_antiseptic"));
            Assert.Equal(4, ctx.Day);
        }

        [Fact]
        public void JourneyRunner_EmitsStandardizedMachineReadableFailureContext_OnFailure()
        {
            var ctx = new JourneyExecutionContext("DiagnosticTestJourney", 9999);
            ctx.AdvanceDay(3);
            ctx.Navigate("crafting", "Craft Nuclear Battery");

            string diagnostic = ctx.FormatFailureDiagnostic("Missing rare isotope materials.");
            Assert.Contains("[JOURNEY_FAILURE]", diagnostic);
            Assert.Contains("journey=\"DiagnosticTestJourney\"", diagnostic);
            Assert.Contains("seed=9999", diagnostic);
            Assert.Contains("day=3", diagnostic);
            Assert.Contains("route=\"crafting\"", diagnostic);
            Assert.Contains("action=\"Craft Nuclear Battery\"", diagnostic);
            Assert.Contains("Missing rare isotope materials.", diagnostic);

            string json = ctx.FormatFailureJson("Missing rare isotope materials.");
            Assert.Contains("\"status\":\"FAILED\"", json);
            Assert.Contains("\"journey\":\"DiagnosticTestJourney\"", json);
            Assert.Contains("\"seed\":9999", json);
            Assert.Contains("\"day\":3", json);
        }

        [Fact]
        public void JourneyRunner_DeterministicSeed_ProducesIdenticalOutcome()
        {
            const int seed = 777;

            int RunSimulation(int s)
            {
                var rng = new SeededRng(s);
                var inv = new Ashfall.Core.Inventory.Inventory();
                inv.AddById("scrap", 10);
                for (int day = 1; day <= 5; day++)
                {
                    int roll = rng.Next(1, 10);
                    inv.AddById("scrap", roll);
                }
                return inv.CountById("scrap");
            }

            int countA = RunSimulation(seed);
            int countB = RunSimulation(seed);

            Assert.Equal(countA, countB);
        }
    }
}
