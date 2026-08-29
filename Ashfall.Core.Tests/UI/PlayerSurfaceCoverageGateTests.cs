// SPDX-License-Identifier: MIT
// ASHFALL CI Gate: Player-Surface Contract & Coverage Manifest (Task 109).
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;
using Ashfall.Core.UI;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public sealed class PlayerSurfaceCoverageGateTests
    {
        public PlayerSurfaceCoverageGateTests()
        {
            PanelRegistryBootstrap.RegisterAll();
        }

        [Fact]
        public void Manifest_GeneratesFromPanelRegistry_MatchesTotalCount()
        {
            var manifest = PlayerSurfaceManifest.Generate();

            Assert.NotNull(manifest);
            Assert.Equal(PanelRegistry.Count, manifest.TotalSurfaces);
            Assert.True(manifest.TotalSurfaces >= 70, $"Expected >= 70 player surfaces, found {manifest.TotalSurfaces}");
        }

        [Fact]
        public void Manifest_AllSurfaces_HaveReachablePlayerRoute()
        {
            var manifest = PlayerSurfaceManifest.Generate();

            var unrouted = manifest.Contracts
                .Where(c => c.ReachableRoutes == null || c.ReachableRoutes.Length == 0)
                .Select(c => c.PanelId)
                .ToList();

            Assert.Empty(unrouted);
            Assert.Equal(manifest.TotalSurfaces, manifest.RoutedSurfaces);
        }

        [Fact]
        public void Manifest_AllSurfaces_HaveBindingTargetOrSetupDeps()
        {
            var manifest = PlayerSurfaceManifest.Generate();

            var unbound = manifest.Contracts
                .Where(c => string.IsNullOrWhiteSpace(c.BindingTarget) && (c.SetupDependencies == null || c.SetupDependencies.Length == 0))
                .Select(c => c.PanelId)
                .ToList();

            Assert.Empty(unbound);
            Assert.Equal(manifest.TotalSurfaces, manifest.BoundSurfaces);
        }

        [Fact]
        public void Manifest_AllSurfaces_HaveDesignatedCloseBehavior()
        {
            var manifest = PlayerSurfaceManifest.Generate();

            var uncloseable = manifest.Contracts
                .Where(c => c.CloseBehavior == SurfaceCloseBehavior.EscKeyOnly)
                .Select(c => c.PanelId)
                .ToList();

            Assert.Empty(uncloseable);
            Assert.Equal(manifest.TotalSurfaces, manifest.CloseableSurfaces);
        }

        [Fact]
        public void Manifest_TracksActionCoverageSeparatelyFromRendering()
        {
            var manifest = PlayerSurfaceManifest.Generate();

            Assert.True(manifest.InteractiveActionSurfaces > 0, "Expected interactive action surfaces");
            Assert.True(manifest.ReadOnlySurfaces > 0, "Expected read-only observational surfaces");
            Assert.Equal(manifest.TotalSurfaces, manifest.InteractiveActionSurfaces + manifest.ReadOnlySurfaces);

            // All surfaces are production rendered
            var unrendered = manifest.Contracts
                .Where(c => c.RenderCoverage == SurfaceRenderCoverage.Stub)
                .Select(c => c.PanelId)
                .ToList();

            Assert.Empty(unrendered);
        }

        [Fact]
        public void Manifest_SerializesValidJsonAndMarkdown()
        {
            var manifest = PlayerSurfaceManifest.Generate();

            string md = manifest.ToMarkdown();
            Assert.False(string.IsNullOrWhiteSpace(md));
            Assert.Contains("# ASHFALL Player-Surface Contract & Coverage Manifest", md);
            Assert.Contains("| `inventory` |", md);
            Assert.Contains("| `status` |", md);

            string json = manifest.ToJson();
            Assert.False(string.IsNullOrWhiteSpace(json));
            Assert.Contains("\"totalSurfaces\":", json);
            Assert.Contains("\"panelId\":\"inventory\"", json);

            // Publish to docs artifact path if running in workspace root
            string? dir = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(dir) && !Directory.Exists(Path.Combine(dir, "docs")))
            {
                var parent = Directory.GetParent(dir);
                dir = parent?.FullName;
            }
            if (!string.IsNullOrEmpty(dir) && Directory.Exists(Path.Combine(dir, "docs")))
            {
                File.WriteAllText(Path.Combine(dir, "docs", "player_surface_manifest.json"), json);
            }
        }

        [Fact]
        public void DeterministicFixtures_InstantiateExpandedDomainSystems_CleanState()
        {
            var rng = new SeededRng(42);
            var log = NullLog.Instance;
            var skills = new SkillProgressionSystem();
            var roster = new DutyRosterSystem();
            var relations = new SurvivorRelationsSystem(rng);
            var inv = new Ashfall.Core.Inventory.Inventory();
            var exp = new ExpeditionSystem();

            // 1. Apprenticeship
            var apprenticeship = new ApprenticeshipSystem(rng, skills, roster, relations, log);
            Assert.NotNull(apprenticeship);
            Assert.Empty(apprenticeship.CaptureState().activePairs);

            // 2. Airlock Security
            var airlock = new AirlockSecuritySystem(rng, log);
            Assert.NotNull(airlock);
            Assert.Equal(100f, airlock.CaptureState().alertness);

            // 3. Caregiving
            var caregiving = new CaregivingSystem();
            Assert.NotNull(caregiving);
            Assert.Empty(caregiving.CaptureState().Assignments);

            // 4. Contractor Roster
            var contractors = new ContractorRosterSystem(rng, inv, roster, exp, log);
            Assert.NotNull(contractors);
            Assert.Empty(contractors.CaptureState().contractors);

            // 5. Excavation
            var excavation = new ExcavationSystem(rng, log);
            Assert.NotNull(excavation);
            Assert.Empty(excavation.State.sites);

            // 6. Kitchen Nutrition
            var needs = new NeedsSystem();
            var kitchen = new KitchenNutritionSystem(rng, inv, needs, log);
            Assert.NotNull(kitchen);
            Assert.Empty(kitchen.State.pantry);

            // 7. Water Treatment
            var water = new WaterTreatmentSystem(log);
            Assert.NotNull(water);
            Assert.Equal(TreatmentMode.Idle, water.State.activeMode);

            // 8. Chemical Dependency
            var chem = new Ashfall.Core.Medical.ChemicalDependencySystem();
            Assert.NotNull(chem);
            Assert.Empty(chem.CaptureState().survivors);

            // 9. Decontamination
            var rad = new Radiation.RadiationSystem();
            var startLvl = new StartingLevel.StartingLevelSystem();
            var decontam = new DecontaminationSystem(rng, rad, inv, airlock, startLvl, log);
            Assert.NotNull(decontam);
            Assert.Empty(decontam.State.queue);

            // 10. Library Study
            var research = new ResearchSystem(log);
            var journal = new JournalSystem();
            var library = new LibraryStudySystem(skills, research, journal, roster, log);
            Assert.NotNull(library);
            Assert.Empty(library.State.activeJobs);

            // 11. Archive Desk
            var knowledge = new KnowledgeBase();
            var archive = new ArchiveDeskSystem(journal, knowledge, inv, roster, log);
            Assert.NotNull(archive);
            Assert.Empty(archive.State.queue);

            // 12. Equipment Condition
            var crafting = new Crafting.CraftingSystem(inv);
            var equip = new EquipmentConditionSystem(rng, inv, crafting, log);
            Assert.NotNull(equip);
            Assert.Empty(equip.CaptureState().items);

            // 13. Medical Ward
            var medWard = new Ashfall.Core.Medical.MedicalWardSystem(
                new Ashfall.Core.Medical.MedicalWardState(),
                new[] { new Ashfall.Core.Medical.MedicalBed { BedId = "bed_1" } },
                new[] { new Ashfall.Core.Medical.MedicalProcedureDef { ProcedureId = "proc_1" } });
            Assert.NotNull(medWard);
            Assert.Empty(medWard.CaptureState().Admissions);

            // 14. Mental Health Crisis
            var mental = new MentalHealthCrisisSystem(rng, needs, medWard, chem, roster, log);
            Assert.NotNull(mental);
            Assert.Empty(mental.State.activeCases);

            // 15. Phantom Memory
            var phantom = new PhantomMemoryEngine();
            Assert.NotNull(phantom);
            Assert.Empty(phantom.CaptureState().records);

            // 16. Traveling Caravan
            var caravan = new TravelingCaravanSystem();
            Assert.NotNull(caravan);
            Assert.Empty(caravan.State.activeCaravans);
        }
    }
}
