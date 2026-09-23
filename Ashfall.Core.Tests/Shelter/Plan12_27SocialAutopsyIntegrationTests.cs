// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Ashfall.Core.Radiation;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public class Plan12_27SocialAutopsyIntegrationTests
    {
        private static AutopsySystem CreateAutopsySystem(out Inventory.Inventory inv, out ResearchSystem res, out VentilationSystem vent, int seed = 42)
        {
            inv = new Inventory.Inventory();
            var rad = new RadiationSystem(seed: seed);
            var starting = new StartingLevelSystem();
            vent = new VentilationSystem(starting);
            res = new ResearchSystem();
            var wardState = new MedicalWardState();
            var bed = new MedicalBed("bed_1", "Bed 1", MedicalBedCategory.General);
            var proc = new MedicalProcedureDef("proc_1", "Procedure 1", "MedicalSystem");
            var medical = new MedicalWardSystem(wardState, new[] { bed }, new[] { proc });
            var sys = new AutopsySystem(new SeededRng(seed), inv, rad, vent, res, medical);
            return sys;
        }

        [Fact]
        public void Plan12_ShelterDecorAndIdeologicalFriction_EndToEnd()
        {
            // 1. ShelterDecorSystem: room decor mounting, localized morale delta & memorial plaques
            var decorSys = new ShelterDecorSystem();
            decorSys.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_decor_poster_ration",
                LocalizedMoraleDelta = 3.5f,
                Category = "poster"
            });
            decorSys.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_decor_memorial_plaque_generic",
                LocalizedMoraleDelta = 5.0f,
                Category = "plaque"
            });
            decorSys.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_decor_memorial_plaque_carving",
                LocalizedMoraleDelta = 7.0f,
                Category = "plaque"
            });

            // Assign poster to north wall of bunk room 1
            bool assignedPoster = decorSys.Assign("bunk_room_1", "north_wall", "item_decor_poster_ration", dayInstalled: 1);
            Assert.True(assignedPoster);
            Assert.Equal(3.5f, decorSys.GetRoomMoraleDelta("bunk_room_1"));

            // Resolve and assign memorial plaque for fallen survivor
            var resolvedPlacement = decorSys.ResolvePlaqueSlot("dweller_reid", "item_personal_keepsake_reid_carving", "bunk_room_1", "memorial_peg", dayInstalled: 2);
            Assert.NotNull(resolvedPlacement);
            Assert.Equal("item_decor_memorial_plaque_carving", resolvedPlacement.ItemId);

            bool assignedPlaque = decorSys.Assign(resolvedPlacement.RoomId, resolvedPlacement.SlotId, resolvedPlacement.ItemId,
                resolvedPlacement.DayInstalled, resolvedPlacement.IsMemorialPlaque, resolvedPlacement.MemorialSurvivorId, resolvedPlacement.PlaqueSourceHeirloomId);
            Assert.True(assignedPlaque);

            // Combined room morale delta: 3.5 + 7.0 = 10.5
            Assert.Equal(10.5f, decorSys.GetRoomMoraleDelta("bunk_room_1"));

            // Verify decor save/restore round-trip
            var decorCapture = decorSys.CaptureState();
            var restoredDecorSys = new ShelterDecorSystem();
            restoredDecorSys.RegisterItemModifier(decorSys.GetItemModifier("item_decor_poster_ration")!);
            restoredDecorSys.RegisterItemModifier(decorSys.GetItemModifier("item_decor_memorial_plaque_carving")!);
            restoredDecorSys.RestoreState(decorCapture);

            Assert.Equal(10.5f, restoredDecorSys.GetRoomMoraleDelta("bunk_room_1"));
            var restoredPlaque = restoredDecorSys.GetSlot("bunk_room_1", "memorial_peg");
            Assert.NotNull(restoredPlaque);
            Assert.True(restoredPlaque.IsMemorialPlaque);
            Assert.Equal("dweller_reid", restoredPlaque.MemorialSurvivorId);

            // 2. IdeologicalFrictionSystem: belief profiles, roommate friction vs synergy
            var frictionSys = new IdeologicalFrictionSystem();
            frictionSys.RegisterBelief("survivor_collectivist", "belief_ration_collectivist");
            frictionSys.RegisterBelief("survivor_individualist", "belief_every_soul_alone");
            frictionSys.RegisterBelief("survivor_faithful", "belief_faith_in_rebuild");

            bool frictionFired = false;
            frictionSys.OnFrictionDetected += (a, b, penalty) =>
            {
                frictionFired = true;
                Assert.Equal(0.80f, penalty, 2); // 1f - 0.20f
            };

            // Conflicting pair: collectivist vs individualist
            float conflictMult = frictionSys.GetRoommateCompatibilityMultiplier("survivor_collectivist", "survivor_individualist");
            Assert.True(frictionFired);
            Assert.Equal(0.80f, conflictMult, 2);

            // Ticking conflicting roommates drains affinity
            frictionSys.TickRoommates("survivor_collectivist", "survivor_individualist", 24f);
            float affinityAfterConflict = frictionSys.GetAffinity("survivor_collectivist", "survivor_individualist");
            Assert.Equal(-2.0f, affinityAfterConflict, 2);

            // Matching belief roommates: synergy bonus
            var frictionSys2 = new IdeologicalFrictionSystem();
            frictionSys2.RegisterBelief("believer_a", "belief_faith_in_rebuild");
            frictionSys2.RegisterBelief("believer_b", "belief_faith_in_rebuild");

            bool synergyFired = false;
            frictionSys2.OnRoommateSynergy += (a, b) => synergyFired = true;

            float synergyMult = frictionSys2.GetRoommateCompatibilityMultiplier("believer_a", "believer_b");
            Assert.True(synergyFired);
            Assert.Equal(1.10f, synergyMult, 2); // 1f + 0.10f

            // Ticking synergy roommates gains affinity
            frictionSys2.TickRoommates("believer_a", "believer_b", 24f);
            Assert.Equal(1.0f, frictionSys2.GetAffinity("believer_a", "believer_b"), 2);

            // Save/Restore affinity round-trip
            var affinitySave = frictionSys.CaptureState();
            var restoredFriction = new IdeologicalFrictionSystem();
            restoredFriction.RestoreState(affinitySave);
            Assert.Equal(-2.0f, restoredFriction.GetAffinity("survivor_collectivist", "survivor_individualist"), 2);
        }

        [Fact]
        public void Plan27_AutopsyProcedureAndVentilationRisk_EndToEnd()
        {
            var autopsySys = CreateAutopsySystem(out var inv, out var res, out var vent, seed: 1234);
            autopsySys.LoadCatalog(new List<AutopsyProcedure>
            {
                new AutopsyProcedure
                {
                    procedure_id = "proc_rad_tissue_biopsy",
                    display_name = "Radiation Tissue Biopsy",
                    procedureHours = 16,
                    airborneRisk = 0.5f,
                    pathogenRisk = 0.2f,
                    requiredTools = new List<string> { "scalpel", "forceps" },
                    requiredConsumables = new List<string> { "antiseptic" },
                    possibleFindings = new List<string> { "finding_cellular_crystallization", "finding_heavy_metal_retention" },
                    researchUnlocks = new List<string> { "knowledge_radiation_chelation" }
                }
            });

            // Stock required inventory
            inv.AddById("scalpel", 1);
            inv.AddById("forceps", 1);
            inv.AddById("antiseptic", 1);

            // Queue autopsy on deceased survivor specimen
            var queueResult = autopsySys.QueueAutopsy("specimen_dweller_42", "proc_rad_tissue_biopsy", "medic_dr_cross");
            Assert.True(queueResult.IsSuccess);
            Assert.Single(autopsySys.State.cases);
            var autopsyCase = autopsySys.State.cases[0];
            Assert.Equal(AutopsyStatus.Queued, autopsyCase.status);

            // Begin autopsy: atomically consumes surgical tools & consumables
            var beginResult = autopsySys.BeginAutopsy(autopsyCase.caseId);
            Assert.True(beginResult.IsSuccess);
            Assert.Equal(AutopsyStatus.InProgress, autopsyCase.status);
            Assert.Equal(0, inv.CountById("scalpel"));
            Assert.Equal(0, inv.CountById("antiseptic"));

            // Day 1: advances 8 hours (8 / 16 hours done)
            autopsySys.TickDay(1);
            Assert.Single(autopsySys.State.cases);
            Assert.Equal(8f, autopsySys.State.cases[0].progressHours);
            Assert.Equal(AutopsyStatus.InProgress, autopsySys.State.cases[0].status);

            // Day 2: advances another 8 hours (16 / 16 hours done -> completes)
            bool caseCompletedFired = false;
            autopsySys.OnCaseCompleted += completedCase =>
            {
                caseCompletedFired = true;
                Assert.Equal("specimen_dweller_42", completedCase.specimenId);
                Assert.False(string.IsNullOrEmpty(completedCase.finding));
            };

            autopsySys.TickDay(2);
            Assert.True(caseCompletedFired);
            Assert.Contains("specimen_dweller_42", autopsySys.State.completedSpecimenIds);
            Assert.Contains("knowledge_radiation_chelation", res.State.unlockedIds);

            // Save/Restore roundtrip preserves completed specimen records
            var autopsyState = autopsySys.CaptureState();
            var restoredAutopsy = CreateAutopsySystem(out _, out _, out _);
            restoredAutopsy.RestoreState(autopsyState);

            Assert.Contains("specimen_dweller_42", restoredAutopsy.State.completedSpecimenIds);
            // Queued duplicate on already processed specimen rejected
            var dupeQueue = restoredAutopsy.QueueAutopsy("specimen_dweller_42", "proc_rad_tissue_biopsy", "medic_dr_cross");
            Assert.False(dupeQueue.IsSuccess);
            Assert.Equal("already_processed", dupeQueue.FailureCode);
        }

        [Fact]
        public void Plan27_CombatTraumaAndTraumaBond_EndToEnd()
        {
            var traumaSys = new CombatTraumaSystem();
            var bondSys = new TraumaBondSystem();

            traumaSys.Rng = new SeededRng(777);
            float moraleDeltaReceived = 0f;
            traumaSys.ApplyMoraleDelta = (survivorId, delta) => moraleDeltaReceived += delta;

            // 1. Combat trauma & hypervigilance
            traumaSys.RegisterSurvivor("survivor_veteran");
            Assert.Equal(0f, traumaSys.GetHypervigilanceLevel("survivor_veteran"));
            Assert.Equal(1f, traumaSys.GetDefenseMultiplier("survivor_veteran"));

            // Survivor survives 2 combat encounters
            traumaSys.OnCombatSurvived("survivor_veteran");
            traumaSys.OnCombatSurvived("survivor_veteran");

            Assert.Equal(2, traumaSys.GetCombatEncountersSurvived("survivor_veteran"));
            Assert.Equal(0.10f, traumaSys.GetHypervigilanceLevel("survivor_veteran"), 2);
            // Defense multiplier: 1f + (0.10 * 0.15) = 1.015f
            Assert.True(traumaSys.GetDefenseMultiplier("survivor_veteran") > 1.01f);

            // Night tick triggers false alarm when hypervigilant
            bool falseAlarmFired = false;
            traumaSys.OnFalseAlarmTriggered += id => falseAlarmFired = true;

            // Tick at night for 12 hours with ungrounded survivor
            traumaSys.Tick("survivor_veteran", 12f, isNightTime: true);
            if (falseAlarmFired)
            {
                Assert.True(moraleDeltaReceived < 0f);
            }

            // Companion grounding reduces false alarm chance
            traumaSys.SetGroundedByCompanion("survivor_veteran", true);

            // Therapy relief reduces hypervigilance
            traumaSys.ApplyTherapyRelief("survivor_veteran", 0.5f);
            Assert.True(traumaSys.GetHypervigilanceLevel("survivor_veteran") < 0.06f);

            // 2. TraumaBondSystem: shared hazard endurance
            bondSys.GetDay = () => 5f;
            float affinityGiven = 0f;
            bondSys.AdjustAffinity = (a, b, delta) => affinityGiven += delta;

            var participants = new List<string> { "survivor_veteran", "survivor_scout" };
            bondSys.OnSharedHazardEndured(participants, "rad_storm_cataclysm");

            Assert.True(bondSys.GetBondStrength("survivor_veteran", "survivor_scout") >= 0.30f);
            Assert.True(affinityGiven >= 15f);

            // Co-shift efficiency bonus applied
            float bonus = bondSys.GetCoShiftEfficiencyBonus("survivor_veteran", "survivor_scout");
            Assert.True(bonus >= 0.07f); // 0.25 * 0.30 = 0.075

            // Bonds decay over time without shared activity
            bondSys.Tick("survivor_veteran", 240f); // 10 days
            float decayedStrength = bondSys.GetBondStrength("survivor_veteran", "survivor_scout");
            Assert.True(decayedStrength < 0.30f);
        }

        [Fact]
        public void Plan12_Plan27_CombinedEcosystem_MemorialAutopsyTrauma()
        {
            // Combined scenario:
            // 1. Two survivors endure shared radiation storm -> form trauma bond
            // 2. One survivor perishes in combat -> squadmate gains combat trauma
            // 3. Medical team executes autopsy -> unlocks clinical knowledge
            // 4. Shelter crafts & mounts memorial plaque in survivor's room -> boosts morale to buffer trauma
            // 5. Roommate ideological friction is checked and tempered by high room decor morale

            var bondSys = new TraumaBondSystem { GetDay = () => 10f };
            var traumaSys = new CombatTraumaSystem { Rng = new SeededRng(999) };
            var autopsySys = CreateAutopsySystem(out var inv, out var res, out var vent, seed: 999);
            var decorSys = new ShelterDecorSystem();

            // Register decor catalog
            decorSys.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_decor_memorial_plaque_drawing",
                LocalizedMoraleDelta = 6.0f,
                Category = "plaque"
            });
            decorSys.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_decor_poster_warning",
                LocalizedMoraleDelta = 2.0f,
                Category = "poster"
            });

            // Register autopsy catalog
            autopsySys.LoadCatalog(new List<AutopsyProcedure>
            {
                new AutopsyProcedure
                {
                    procedure_id = "proc_post_mortem_combat",
                    display_name = "Combat Trauma Post-Mortem",
                    procedureHours = 8,
                    airborneRisk = 0.0f,
                    pathogenRisk = 0.0f,
                    requiredTools = new List<string> { "scalpel" },
                    possibleFindings = new List<string> { "finding_ballistic_fragment" },
                    researchUnlocks = new List<string> { "knowledge_combat_trauma_care" }
                }
            });

            string fallenId = "dweller_alder";
            string survivorId = "dweller_mira";

            // Step 1: Pre-fall bond
            bondSys.OnSharedHazardEndured(new List<string> { fallenId, survivorId }, "hazard_rad_pulse");
            Assert.True(bondSys.GetBondStrength(survivorId, fallenId) >= 0.30f);

            // Step 2: Combat loss
            traumaSys.OnCombatSurvived(survivorId);
            Assert.Equal(0.05f, traumaSys.GetHypervigilanceLevel(survivorId), 2);

            // Step 3: Autopsy conducted on fallen dweller
            inv.AddById("scalpel", 1);
            var q = autopsySys.QueueAutopsy(fallenId, "proc_post_mortem_combat", survivorId);
            Assert.True(q.IsSuccess);
            autopsySys.BeginAutopsy(autopsySys.State.cases[0].caseId);
            autopsySys.TickDay(11);
            Assert.Contains("knowledge_combat_trauma_care", res.State.unlockedIds);

            // Step 4: Mount memorial plaque in communal quarters
            var plaquePlacement = decorSys.ResolvePlaqueSlot(fallenId, "item_personal_keepsake_alder_drawing", "quarters_communal", "slot_west_plaque", dayInstalled: 11);
            Assert.NotNull(plaquePlacement);
            decorSys.Assign(plaquePlacement.RoomId, plaquePlacement.SlotId, plaquePlacement.ItemId, plaquePlacement.DayInstalled,
                isMemorialPlaque: true, memorialSurvivorId: fallenId, plaqueSourceHeirloomId: plaquePlacement.PlaqueSourceHeirloomId);
            decorSys.Assign("quarters_communal", "slot_east_poster", "item_decor_poster_warning", dayInstalled: 11);

            // Room morale delta compensates for grief/trauma: 6.0 + 2.0 = 8.0
            float roomMorale = decorSys.GetRoomMoraleDelta("quarters_communal");
            Assert.Equal(8.0f, roomMorale);

            // Step 5: Roommate ideological friction tempered by decor
            var frictionSys = new IdeologicalFrictionSystem();
            frictionSys.RegisterBelief(survivorId, "belief_faith_in_rebuild");
            frictionSys.RegisterBelief("dweller_cynic", "belief_ash_nihilist");

            float frictionMult = frictionSys.GetRoommateCompatibilityMultiplier(survivorId, "dweller_cynic");
            Assert.Equal(0.80f, frictionMult, 2); // 20% sleep penalty exists, but room morale is +8.0
        }
    }
}
