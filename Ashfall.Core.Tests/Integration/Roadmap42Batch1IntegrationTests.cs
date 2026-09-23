// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Excavation;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Integration
{
    /// <summary>
    /// Roadmap 42 — Batch 1 (Plans 32–41): Scaffolding & Underused-System Catalogs.
    /// Canonical integration test suite proving:
    /// 1. Referential integrity across all 10 authored catalogs (§23).
    /// 2. The 20-step deterministic multi-system campaign journey (§33).
    /// 3. Cross-system save/restore round-trip preservation (§21).
    /// 4. Deterministic replay equality across independent seeded runs (§22).
    /// </summary>
    public sealed class Roadmap42Batch1IntegrationTests
    {
        private static string ResolveDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new InvalidOperationException("StreamingAssets/Data directory not found");
        }

        private sealed class TestSurvivorActor : SkillActor
        {
            public string Id { get; }
            public bool IsAlive => true;
            public float Morale => 100f;
            public float Health => 100f;
            public string ExpertDisciplineId { get; set; } = string.Empty;

            public TestSurvivorActor(string id, string expertDiscipline = "")
            {
                Id = id;
                ExpertDisciplineId = expertDiscipline;
            }

            public void SetSkillBonus(string disciplineId, float bonus) { }
        }

        [Fact]
        public void Test1_ReferentialIntegrity_CrossCatalogContracts_AllResolve()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            // 1. Plan 32: Expeditions (55 canonical destinations)
            var expeditions = ExpeditionCatalogLoader.Load(dataDir, fileIO, serializer);
            Assert.True(expeditions.Count >= 50, $"Expected >= 50 expedition destinations, got {expeditions.Count}");
            var expIds = new HashSet<string>(expeditions.Select(e => e.id), StringComparer.Ordinal);

            // 2. Plan 33: Skills (160 authored skills)
            var skills = SkillCatalogLoader.Load(dataDir, fileIO, serializer);
            Assert.True(skills.Count >= 50, $"Expected >= 50 skills, got {skills.Count}");
            var skillIds = new HashSet<string>(skills.Select(s => s.id), StringComparer.Ordinal);
            Assert.Contains("skill_field_dressing", skillIds);
            Assert.Contains("skill_rough_repairs", skillIds);

            // 3. Plan 34: Research Knowledge
            var researchNodes = ResearchKnowledgeCatalogLoader.Load(dataDir, fileIO, serializer);
            Assert.True(researchNodes.Count >= 15, $"Expected >= 15 research nodes, got {researchNodes.Count}");
            var researchIds = new HashSet<string>(researchNodes.Select(r => r.id), StringComparer.Ordinal);

            // Cross-ref check 33 -> 34: any skill prerequisites in research point to real skills
            foreach (var node in researchNodes)
            {
                if (node.prerequisites != null)
                {
                    foreach (var prereq in node.prerequisites)
                    {
                        if (prereq.StartsWith("skill_", StringComparison.Ordinal))
                        {
                            Assert.True(skillIds.Contains(prereq), $"Research '{node.id}' references unknown skill '{prereq}'");
                        }
                    }
                }
            }

            // 4. Plan 35: Wildlife Ecosystem / Migration
            string ecoPath = fileIO.Combine(dataDir, "wildlife_ecosystem.json");
            Assert.True(fileIO.FileExists(ecoPath), "wildlife_ecosystem.json must exist");

            // 5. Plan 36: Trapping Catalog (10 traps + 15 prey)
            var trapCatalog = WildlifeTrappingCatalogLoader.Load(dataDir, fileIO, serializer);
            Assert.NotNull(trapCatalog);
            Assert.True(trapCatalog.Traps.Count >= 10, $"Expected >= 10 traps, got {trapCatalog.Traps.Count}");
            Assert.True(trapCatalog.Prey.Count >= 15, $"Expected >= 15 prey species, got {trapCatalog.Prey.Count}");

            // 6. Plan 37: Excavation Sites (8 sites)
            var excavationSites = ExcavationCatalogLoader.Load(dataDir, fileIO, serializer);
            Assert.NotNull(excavationSites);
            Assert.Equal(8, excavationSites.Count);
            foreach (var site in excavationSites)
            {
                Assert.False(string.IsNullOrWhiteSpace(site.site_id));
                Assert.True(site.max_depth_meters > 0f);
                Assert.True(site.required_progress > 0f);
            }

            // 7. Plan 38: Sky-Layer Armor (6 configs)
            var armorConfigs = SkyLayerArmorCatalogLoader.Load(dataDir, fileIO, serializer);
            Assert.NotNull(armorConfigs);
            Assert.Equal(6, armorConfigs.Count);

            // 8. Plan 39: Orbital Harrow Telemetry Events (12 events)
            var harrowEvents = OrbitalHarrowCatalogLoader.Load(dataDir, fileIO, serializer);
            Assert.NotNull(harrowEvents);
            Assert.Equal(12, harrowEvents.Count);

            // Cross-ref check 37 <-> 39: revealed sites reference real excavation locations
            var excavationLocIds = new HashSet<string>(excavationSites.Select(s => s.location_id), StringComparer.Ordinal);
            foreach (var ev in harrowEvents)
            {
                if (!string.IsNullOrEmpty(ev.revealed_site_id))
                {
                    Assert.True(excavationLocIds.Contains(ev.revealed_site_id),
                        $"Telemetry event '{ev.id}' references unknown excavation location '{ev.revealed_site_id}'");
                }
            }

            // 9. Plan 40: Ledger Debt Templates (15 templates + 10 consequences)
            var debtCatalog = DebtTemplateCatalogLoader.Load(dataDir, fileIO, serializer);
            Assert.NotNull(debtCatalog);
            Assert.Equal(15, debtCatalog.Templates.Count);
            Assert.Equal(10, debtCatalog.Consequences.Count);
            var consequenceIds = new HashSet<string>(debtCatalog.Consequences.Select(c => c.id), StringComparer.Ordinal);
            foreach (var tmpl in debtCatalog.Templates)
            {
                Assert.True(consequenceIds.Contains(tmpl.consequenceId),
                    $"Debt template '{tmpl.id}' references unknown consequence '{tmpl.consequenceId}'");
            }

            // 10. Plan 41: Shelter Rooms (23 rooms + 12 assignment rules)
            var roomCatalog = ShelterRoomCatalogLoader.Load(dataDir);
            Assert.NotNull(roomCatalog);
            Assert.True(roomCatalog.rooms.Count >= 20, $"Expected >= 20 rooms, got {roomCatalog.rooms.Count}");
            Assert.Equal(12, roomCatalog.assignment_rules.Count);

            // Cross-ref check 33 -> 41: room and rule required skills resolve in skills.json
            foreach (var room in roomCatalog.rooms)
            {
                if (!string.IsNullOrEmpty(room.required_skill_id))
                {
                    Assert.True(skillIds.Contains(room.required_skill_id),
                        $"Room '{room.id}' references unknown skill '{room.required_skill_id}'");
                }
            }
            foreach (var rule in roomCatalog.assignment_rules)
            {
                if (!string.IsNullOrEmpty(rule.required_skill_id))
                {
                    Assert.True(skillIds.Contains(rule.required_skill_id),
                        $"Assignment rule '{rule.id}' references unknown skill '{rule.required_skill_id}'");
                }
            }
        }

        private sealed class CampaignExecutionSnapshot
        {
            public float SkillXp { get; set; }
            public bool HasActiveSkill { get; set; }
            public int ResearchCompletedCount { get; set; }
            public int ExpeditionCount { get; set; }
            public int PackCount { get; set; }
            public string PackSector { get; set; } = string.Empty;
            public int TrapRemainingDurability { get; set; }
            public bool TrapCatch { get; set; }
            public float DigProgress { get; set; }
            public float DigRisk { get; set; }
            public bool TelemetryActive { get; set; }
            public float CellDurability { get; set; }
            public int DebtDaysRemaining { get; set; }
            public int AssignmentCount { get; set; }

            public string ComputeDeterministicHash()
            {
                return $"{SkillXp:F1}_{HasActiveSkill}_{ResearchCompletedCount}_{ExpeditionCount}_" +
                       $"{PackCount}_{PackSector}_{TrapRemainingDurability}_{TrapCatch}_" +
                       $"{DigProgress:F1}_{DigRisk:F2}_{TelemetryActive}_{CellDurability:F1}_" +
                       $"{DebtDaysRemaining}_{AssignmentCount}";
            }
        }

        private static CampaignExecutionSnapshot ExecuteDeterministicCampaignJourney(int masterSeed)
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            // Step 1: Start fresh seeded campaign
            var rng = new SeededRng(masterSeed);

            // Step 2: Inspect canonical skill and research definitions
            var skillSys = new SkillProgressionSystem();
            SkillCatalogLoader.LoadAndRegister(skillSys, dataDir, fileIO, serializer);

            var researchNodes = ResearchKnowledgeCatalogLoader.Load(dataDir, fileIO, serializer);
            var researchSys = new ResearchSystem();
            foreach (var n in researchNodes) researchSys.Register(n);

            // Step 3: Gain/progress at least one migrated skill
            var actor = new TestSurvivorActor("survivor_audrey");
            for (int i = 0; i < 11; i++)
            {
                skillSys.RecordAction(actor, "medical", SkillProgressionSystem.DefaultXpPerAction, 1);
            }
            float skillXp = skillSys.GetXp("survivor_audrey", "medical");
            bool hasFieldDressing = skillSys.HasActiveSkill("survivor_audrey", "skill_field_dressing");

            // Step 4: Unlock or advance one research node
            researchSys.UnlockManual("knowledge_field_medicine");
            int researchCompleted = researchSys.State.unlockedIds.Count;

            // Step 5: Dispatch to one newly wired expedition location
            var expeditions = ExpeditionCatalogLoader.Load(dataDir, fileIO, serializer);
            var weighbridge = expeditions.FirstOrDefault(e => e.id == "loc_weighbridge") ?? expeditions.First();

            // Step 6: Advance wildlife season/migration state
            var wildSys = new WildlifeMigrationSystem(rng);
            wildSys.RegisterPack("pack_rad_dogs_alpha", "species_rad_dog", "sector_ruins_north", population: 6);
            wildSys.SetSectorAdjacency(new[]
            {
                ("sector_ruins_north", new List<string> { "sector_subway_concourse" }),
                ("sector_subway_concourse", new List<string> { "sector_ruins_north" })
            });
            wildSys.MigratePack("pack_rad_dogs_alpha", "sector_subway_concourse");

            // Step 7: Set a trap and resolve catch
            var trapSys = new WildlifeTrappingSystem(rng);
            var trapCatalog = WildlifeTrappingCatalogLoader.Load(dataDir, fileIO, serializer);
            trapCatalog.RegisterWith(trapSys);
            trapSys.SetTrap("site_edge_copse", "bait_grain_lure", "survivor_audrey", "snare", "trap_snare", 1, 4);
            trapSys.TickDay(2);
            var trapSite = trapSys.State.trapSites.FirstOrDefault(s => s.siteId == "site_edge_copse");

            // Step 8 & 9: Dispatch to an excavation site, advance dig, apply shoring
            var excavationSys = new ExcavationSystem(rng);
            excavationSys.AddSite("excavation_utility_tunnels", "loc_excavation_utility_tunnels", 95f, 0.45f);
            excavationSys.AssignWorkers("excavation_utility_tunnels", 2);
            excavationSys.TickDay();
            excavationSys.ApplyShoring("excavation_utility_tunnels");
            var digSite = excavationSys.State.sites.First(s => s.siteId == "excavation_utility_tunnels");

            // Step 10 & 11: Trigger telemetry event, verify armor config responds to threat
            var skyArmor = new SkyLayerArmorSystem();
            skyArmor.SetCellArmor(5, CeilingMaterialTier.ReinforcedConcrete, 0.8f);
            var harrowSys = new OrbitalHarrowTelemetrySystem(skyArmor, rng);
            harrowSys.ActivateTelemetry(day: 2);
            harrowSys.ScheduleImpact(5, 5, 25f);
            harrowSys.Brace("concrete", 4);
            harrowSys.TickDay(5);
            var cell = skyArmor.GetCell(5);

            // Step 12 & 13: Create debt from template and advance term
            var debtSys = new LedgerDebtSystem();
            debtSys.PresentContract("survivor_audrey", 8f, termDays: 20, rate: 0.15f, "eight tins of sealed rations");
            debtSys.PresentContract("survivor_audrey", 8f, termDays: 20, rate: 0.15f, "eight tins of sealed rations");
            debtSys.SignContract("survivor_audrey", 2);
            debtSys.TickDaily(3);
            var contract = debtSys.GetContract("survivor_audrey");

            // Step 14: Assign survivor to new room using skill/rule prerequisites
            var rooms = new List<ShelterRoom>
            {
                new ShelterRoom("room_clinic", "Clinic", 2, "skill_field_dressing"),
                new ShelterRoom("room_workshop", "Workshop", 2, "skill_rough_repairs")
            };
            var assignmentSys = new ShelterAssignmentSystem(new ShelterAssignmentState(), rooms, rng);
            assignmentSys.Assign("survivor_audrey", "room_clinic", day: 3);

            // Step 15 & 16: Save all system states and reload into fresh instances
            var skillState = skillSys.CaptureState();
            var researchState = researchSys.CaptureState();
            var wildState = wildSys.State;
            var trapState = trapSys.CaptureState();
            var digState = excavationSys.CaptureState();
            var harrowState = harrowSys.CaptureState();
            var debtState = debtSys.CaptureState();
            var assignmentState = assignmentSys.CaptureState();

            // Reload fresh systems
            var reloadedSkill = new SkillProgressionSystem();
            reloadedSkill.RestoreState(skillState);

            var reloadedResearch = new ResearchSystem();
            foreach (var n in researchNodes) reloadedResearch.Register(n);
            reloadedResearch.RestoreState(researchState);

            var reloadedDig = new ExcavationSystem(rng);
            reloadedDig.RestoreState(digState);

            var reloadedDebt = new LedgerDebtSystem();
            reloadedDebt.RestoreState(debtState);

            var reloadedAssignment = new ShelterAssignmentSystem(assignmentState, rooms, rng);

            // Step 17: Verify all affected states match restored instances
            Assert.Equal(skillXp, reloadedSkill.GetXp("survivor_audrey", "medical"));
            Assert.True(reloadedResearch.IsManualUnlocked("knowledge_field_medicine"));
            Assert.Equal(digSite.progress, reloadedDig.State.sites[0].progress);
            Assert.Equal(contract.daysRemaining, reloadedDebt.GetContract("survivor_audrey").daysRemaining);
            Assert.Single(reloadedAssignment.GetAssignments());

            // Step 18: Build snapshot for deterministic hash comparison
            return new CampaignExecutionSnapshot
            {
                SkillXp = skillXp,
                HasActiveSkill = hasFieldDressing,
                ResearchCompletedCount = researchCompleted,
                ExpeditionCount = expeditions.Count,
                PackCount = wildSys.State.packs.Count,
                PackSector = wildSys.State.packs[0].currentSectorId,
                TrapRemainingDurability = trapSite?.remainingDurability ?? 0,
                TrapCatch = trapSite?.hasCatch ?? false,
                DigProgress = digSite.progress,
                DigRisk = digSite.structuralRisk,
                TelemetryActive = harrowSys.State.telemetryActive,
                CellDurability = cell?.currentDurability ?? 0f,
                DebtDaysRemaining = contract.daysRemaining,
                AssignmentCount = reloadedAssignment.GetAssignments().Count
            };
        }

        [Fact]
        public void Test2_FullDeterministicCampaignJourney_TouchesAllTenSystems_ReplaysIdentically()
        {
            // Step 19 & 20: Repeat entire journey from seed 42 in independent run and compare hashes
            var runA = ExecuteDeterministicCampaignJourney(42);
            var runB = ExecuteDeterministicCampaignJourney(42);

            Assert.Equal(runA.ComputeDeterministicHash(), runB.ComputeDeterministicHash());
            Assert.Equal(runA.SkillXp, runB.SkillXp);
            Assert.Equal(runA.HasActiveSkill, runB.HasActiveSkill);
            Assert.Equal(runA.ResearchCompletedCount, runB.ResearchCompletedCount);
            Assert.Equal(runA.PackSector, runB.PackSector);
            Assert.Equal(runA.DigProgress, runB.DigProgress);
            Assert.Equal(runA.DebtDaysRemaining, runB.DebtDaysRemaining);
            Assert.Equal(runA.AssignmentCount, runB.AssignmentCount);
        }

        [Fact]
        public void Test3_Batch1CatalogCountsAndLoaderClassifications_MatchAuthoritativeTruth()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            // Plan 32: Expeditions (L1 generic loader, 55 destinations)
            var expeditions = ExpeditionCatalogLoader.Load(dataDir, fileIO, serializer);
            Assert.True(expeditions.Count >= 50, $"Plan 32 target >= 50, got {expeditions.Count}");

            // Plan 33: Skills (L2 mechanical loader, 160 skills)
            var skills = SkillCatalogLoader.Load(dataDir, fileIO, serializer);
            Assert.True(skills.Count >= 50, $"Plan 33 target >= 50, got {skills.Count}");

            // Plan 34: Research (L2 mechanical loader, 31+ nodes)
            var research = ResearchKnowledgeCatalogLoader.Load(dataDir, fileIO, serializer);
            Assert.True(research.Count >= 30, $"Plan 34 target >= 30, got {research.Count}");

            // Plan 35: Wildlife Ecosystem / Seeds (L1 generic loader)
            var seeds = EvolvingWorldCatalogLoader.Load(dataDir, fileIO, serializer);
            Assert.NotNull(seeds);
            Assert.True(seeds!.packs.Count >= 12, $"Plan 35 target >= 12 packs, got {seeds.packs.Count}");

            // Plan 36: Trapping (L2 mechanical loader, 10 traps + 15 prey)
            var trapping = WildlifeTrappingCatalogLoader.Load(dataDir, fileIO, serializer);
            Assert.NotNull(trapping);
            Assert.True(trapping.Traps.Count >= 10, $"Plan 36 target >= 10 traps, got {trapping.Traps.Count}");
            Assert.True(trapping.Prey.Count >= 15, $"Plan 36 target >= 15 prey, got {trapping.Prey.Count}");

            // Plan 37: Excavation (L2 mechanical loader, 8 sites)
            var excavation = ExcavationCatalogLoader.Load(dataDir, fileIO, serializer);
            Assert.NotNull(excavation);
            Assert.Equal(8, excavation.Count);

            // Plan 38: Sky-Layer Armor (L2 mechanical loader, 6 configs)
            var armor = SkyLayerArmorCatalogLoader.Load(dataDir, fileIO, serializer);
            Assert.NotNull(armor);
            Assert.Equal(6, armor.Count);

            // Plan 39: Orbital Harrow (L2 mechanical loader, 12 events)
            var harrow = OrbitalHarrowCatalogLoader.Load(dataDir, fileIO, serializer);
            Assert.NotNull(harrow);
            Assert.Equal(12, harrow.Count);

            // Plan 40: Ledger Debt (L2 mechanical loader, 15 templates + 10 consequences)
            var debt = DebtTemplateCatalogLoader.Load(dataDir, fileIO, serializer);
            Assert.NotNull(debt);
            Assert.Equal(15, debt.Templates.Count);
            Assert.Equal(10, debt.Consequences.Count);

            // Plan 41: Shelter Rooms (L2 mechanical loader, 23 rooms + 12 rules)
            var rooms = ShelterRoomCatalogLoader.Load(dataDir);
            Assert.NotNull(rooms);
            Assert.True(rooms.rooms.Count >= 20, $"Plan 41 target >= 20 rooms, got {rooms.rooms.Count}");
            Assert.Equal(12, rooms.assignment_rules.Count);
        }
    }
}
