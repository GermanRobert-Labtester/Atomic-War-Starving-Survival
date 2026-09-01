// SPDX-License-Identifier: MIT
// Task #133 — Architecture gates: prevent regression toward split medical authority.
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    /// <summary>
    /// Source-scanned gates keeping the unified medical pipeline coherent
    /// (Phase 75 of the Task #133 plan). These read repository source the same
    /// way <see cref="SaveStoreCoverageGateTests"/> does; they own no gameplay.
    /// </summary>
    public class MedicalPipelineArchitectureGateTests
    {
        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string probe = Path.Combine(dir.FullName, "Ashfall.csproj");
                if (File.Exists(probe))
                    return dir.FullName;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate the repository root from the test run");
        }

        private static string ReadRepositoryFile(params string[] segments)
        {
            string path = Path.Combine(new[] { RepoRoot() }.Concat(segments).ToArray());
            Assert.True(File.Exists(path), $"expected repository file to exist: {path}");
            return File.ReadAllText(path);
        }

        [Fact]
        public void MedicalHostSession_HasNoTickDemo()
        {
            string source = ReadRepositoryFile("src", "Host", "MedicalHostSession.cs");
            Assert.DoesNotContain("TickDemo", source);
        }

        [Fact]
        public void Phase0_DoesNotTickSharedChemicalDependency()
        {
            string source = ReadRepositoryFile("src", "Host", "Phase0HostSession.cs");
            // The dependency tick must be guarded by instance ownership so the
            // shared MedicalHostSession ledger is advanced exactly once per day
            // (by MedicalDiseaseDayOwner) — never twice.
            Assert.Matches(new Regex(@"if\s*\(\s*_ownsDependency\s*\)\s*\r?\n\s*Dependency\.TickHours"), source);
        }

        [Fact]
        public void MedicalDiseaseDayOwner_AdvancesScheduledProcedures_BeforeTicks()
        {
            string source = ReadRepositoryFile("src", "Main.CampaignOwners.cs");
            int advanceIndex = source.IndexOf("AdvanceScheduled(24f", StringComparison.Ordinal);
            int chemTickIndex = source.IndexOf("_medical.TickHours(24f)", StringComparison.Ordinal);
            Assert.True(advanceIndex >= 0, "MedicalDiseaseDayOwner must call AdvanceScheduled");
            Assert.True(chemTickIndex >= 0);
            Assert.True(advanceIndex < chemTickIndex,
                "scheduled procedures must resolve before the dependency tick (documented phase order)");
        }

        [Fact]
        public void MedicalPanel_DoesNotConsumeItemsDirectly()
        {
            string source = ReadRepositoryFile("src", "UI", "MedicalPanel.cs");
            Assert.DoesNotContain("RemoveById", source);
            Assert.DoesNotContain(".ApplyInhaler(", source);
            Assert.DoesNotContain(".ApplyHerbalTea(", source);
            Assert.DoesNotContain(".HealSurvivor(", source);
        }

        [Fact]
        public void Phase0HostSession_AcceptsSharedDependencyInstance()
        {
            string source = ReadRepositoryFile("src", "Main.Phase0.cs");
            Assert.Contains("new Phase0HostSession(dependency: _medical.Engine)", source);
        }

        [Fact]
        public void AfflictionInventory_Exists_AndNamesCoreAfflictions()
        {
            string json = ReadRepositoryFile("docs", "architecture", "affliction_inventory.json");
            foreach (string expected in new[]
            {
                "affliction_respiratory_degeneration",
                "affliction_radiation_sickness",
                "affliction_health_deficit",
                "chemical_dependency",
                "disease_infections",
                "combat_trauma",
                "somatic_flashbacks",
                "guilt_insomnia",
                "dose_ledger_bands",
                "RespiratoryDegenerationSystem",
                "RadiationPhaseProgression",
                "DiseaseSystem",
                "ChemicalDependencySystem"
            })
            {
                Assert.Contains(expected, json);
            }
        }

        [Fact]
        public void PipelineHandlers_Exist_ForVerticalSliceAfflictions()
        {
            string respiratory = ReadRepositoryFile("Assets", "Ashfall.Core", "Medical", "RespiratoryAfflictionHandler.cs");
            Assert.Contains("IAfflictionHandler", respiratory);
            Assert.Contains("AfflictionId(MedicalTreatmentCatalog.RespiratoryDegenerationId)", respiratory);

            string radiation = ReadRepositoryFile("Assets", "Ashfall.Core", "Medical", "RadiationAfflictionHandlers.cs");
            Assert.Contains("AfflictionId(MedicalTreatmentCatalog.RadiationSicknessId)", radiation);
        }

        [Fact]
        public void PipelineSave_IsRegistered_AsSaveSection()
        {
            string registry = ReadRepositoryFile("Assets", "Ashfall.Core", "Save", "SaveSectionRegistry.cs");
            Assert.Contains("medical_pipeline", registry);
            Assert.Contains("SaveMedicalPipeline", registry);
        }

        // ── Task #133 P1 — disease write-path gates ──────────────────

        [Fact]
        public void UiPanels_NeverMutateDiseaseOrDetoxDomainsDirectly()
        {
            // All disease and chemical-dependency player actions must flow
            // through the pipeline. The host sessions' thin commands stay
            // available for headless/CLI selftests only — no UI panel may
            // call them.
            foreach (string file in Directory.GetFiles(
                Path.Combine(RepoRoot(), "src", "UI"), "*.cs", SearchOption.AllDirectories))
            {
                string source = File.ReadAllText(file);
                foreach (string forbidden in new[]
                {
                    ".PurifyWater(", ".SealVents(", ".SterilizeTools(",
                    ".SetAirFiltration(", ".Quarantine(", ".EndQuarantine(",
                    ".BeginManagedDetox(", ".BeginColdTurkey("
                })
                {
                    Assert.True(!source.Contains(forbidden),
                        $"{Path.GetFileName(file)} must not call {forbidden} directly (route through the medical pipeline).");
                }
            }
        }

        [Fact]
        public void DiseaseHandler_NeverTicksProgression()
        {
            string source = ReadRepositoryFile("Assets", "Ashfall.Core", "Medical", "DiseaseAfflictionHandler.cs");
            // Call-syntax scan: prose in doc comments may mention the domain
            // APIs; actual invocations may not exist.
            Assert.DoesNotContain(".TickDaily(", source);
            Assert.DoesNotContain(".Infect(", source);
        }

        [Fact]
        public void DiseaseHandler_IsRegistered_InHostWiring()
        {
            string source = ReadRepositoryFile("src", "Main.Medical.cs");
            Assert.Contains("DiseaseAfflictionHandler.RegisterAll", source);
            Assert.Contains("DiseaseProtocolHandler.RegisterAll", source);
            Assert.Contains("SuspectFromEvidence", source);
        }

        [Fact]
        public void MedicalPanel_DiseaseActions_RouteThroughPipeline()
        {
            string source = ReadRepositoryFile("src", "UI", "MedicalPanel.cs");
            Assert.Contains("ExecuteIdentify", source);
            Assert.Contains("ExecuteProtocol", source);
            Assert.Contains("TreatmentQuarantine", source);
            Assert.Contains("TreatmentRelease", source);
            // The panel must never name a disease the pipeline has not
            // confirmed: no hardcoded catalog disease ids in UI code.
            Assert.DoesNotContain("disease_cholera", source);
            Assert.DoesNotContain("disease_zoonotic_flu", source);
        }

        // ── Task #133 P1b — chemical-dependency + ward gates ─────────

        [Fact]
        public void ChemicalDependencyHandler_NeverTicksOrSchedules()
        {
            string source = ReadRepositoryFile(
                "Assets", "Ashfall.Core", "Medical", "ChemicalDependencyAfflictionHandler.cs");
            // Call-syntax scan: the dependency domain keeps every clock; the
            // handler starts programs and never advances or schedules one
            // (a second detox clock would double-advance withdrawal).
            Assert.DoesNotContain(".TickHours(", source);
            Assert.DoesNotContain(".TickDaily(", source);
            Assert.DoesNotContain(".Schedule(", source);
        }

        [Fact]
        public void ChemicalDependencyPanel_RoutesThroughPipeline()
        {
            string source = ReadRepositoryFile("src", "UI", "ChemicalDependencyPanel.cs");
            Assert.Contains("ExecuteTreatment", source);
            Assert.Contains("TreatmentManagedDetox", source);
            Assert.Contains("TreatmentColdTurkey", source);
        }

        [Fact]
        public void MedicalWardPanel_RoutesProceduresThroughHostWrapper()
        {
            string source = ReadRepositoryFile("src", "UI", "MedicalWardPanel.cs");
            Assert.Contains("_host.RunProcedure(", source);
            // The panel must not bypass the host wrapper (which runs the
            // pipeline before the ward log).
            Assert.DoesNotContain(".System.RunProcedure(", source);
            Assert.DoesNotContain("RemoveById", source);
        }

        [Fact]
        public void WardBridge_MapsOnlyExistingPipelineTreatments()
        {
            string source = ReadRepositoryFile("Assets", "Ashfall.Core", "Medical", "MedicalWardPipelineBridge.cs");
            // The bridge may only map procedures whose clinical effect exists
            // as a pipeline treatment; everything else stays log-only (the
            // doc comment may mention unmapped procedures, the switch may
            // not map them).
            Assert.Contains("TreatmentBandage", source);
            Assert.Contains("TreatmentAntiRad", source);
            Assert.DoesNotContain("\"proc_surgery\" =>", source);
        }

        [Fact]
        public void HostWiring_RegistersChemicalDependencyHandler()
        {
            string source = ReadRepositoryFile("src", "Main.Medical.cs");
            Assert.Contains("ChemicalDependencyAfflictionHandler", source);
        }

        // ── Task #133 P1c — psychology projection + Phase0 inhaler gates ──

        [Fact]
        public void PsychologyHandlers_AreObserveOnly()
        {
            string source = ReadRepositoryFile(
                "Assets", "Ashfall.Core", "Medical", "PsychologyAfflictionHandlers.cs");
            // Call-syntax scan: the handlers only read. Every Phase-0 clock,
            // interaction, and symptom trigger stays with the Phase-0 day
            // owner and the UI; no sedative economy exists in the pipeline.
            Assert.DoesNotContain(".Tick(", source);
            Assert.DoesNotContain(".TickHours(", source);
            Assert.DoesNotContain(".ApplySedative(", source);
            Assert.DoesNotContain(".OnCombatSurvived(", source);
            Assert.DoesNotContain(".IncreaseSusceptibility(", source);
            Assert.Contains("treatment_not_for_affliction", source);
        }

        [Fact]
        public void PsychologyHandlers_AreRegistered_InHostWiring()
        {
            string source = ReadRepositoryFile("src", "Main.Medical.cs");
            Assert.Contains("CombatTraumaAfflictionHandler", source);
            Assert.Contains("SomaticFlashbackAfflictionHandler", source);
            Assert.Contains("GuiltInsomniaAfflictionHandler", source);
        }

        [Fact]
        public void PsychologyDefinitions_HaveNoTreatmentEntries()
        {
            string source = ReadRepositoryFile("Assets", "Ashfall.Core", "Medical", "MedicalTreatmentCatalog.cs");
            // The three psychology definition ids exist as identity constants
            // only — no MedicalTreatmentDef may target them (no invented
            // psychology treatments or items).
            Assert.Contains("CombatTraumaId = \"affliction_combat_trauma\"", source);
            Assert.Contains("SomaticFlashbackId = \"affliction_somatic_flashback\"", source);
            Assert.Contains("GuiltInsomniaId = \"affliction_guilt_insomnia\"", source);
            Assert.DoesNotContain("TreatmentId = CombatTraumaId", source);
            Assert.DoesNotContain("TreatmentId = SomaticFlashbackId", source);
            Assert.DoesNotContain("TreatmentId = GuiltInsomniaId", source);
        }

        [Fact]
        public void Phase0Panel_InhalerRoutesThroughPipeline()
        {
            string source = ReadRepositoryFile("src", "UI", "Phase0Panel.cs");
            Assert.Contains("ExecuteTreatment", source);
            Assert.Contains("TreatmentInhaler", source);
            // The panel never calls the host's raw Phase-0 inhaler command
            // (that stays CLI/test-only); unbound panels keep the button off.
            Assert.DoesNotContain("_phase0.ApplyInhaler(", source);
        }

        [Fact]
        public void Phase0PanelOpen_BindsPipeline_InHostPaths()
        {
            string handlers = ReadRepositoryFile("src", "Main.UiHandlers.cs");
            Assert.Contains("EnsureMedicalPipeline();", handlers);
            Assert.Contains("_phase0Panel.Bind(_phase0, _survivors, _medical.Pipeline)", handlers);

            string uiTest = ReadRepositoryFile("src", "Main.UiTests.Phase0.cs");
            Assert.Contains("EnsureMedicalPipeline();", uiTest);
            Assert.Contains("_phase0Panel!.Bind(_phase0, _survivors, _medical.Pipeline)", uiTest);
        }

        [Fact]
        public void AfflictionsPanel_ShowsPsychologyRows_FromPatientRecord()
        {
            string source = ReadRepositoryFile("src", "UI", "AfflictionsPanel.cs");
            Assert.Contains("PatientRecordProjector", source);
            Assert.Contains("CombatTraumaId", source);
            Assert.Contains("SomaticFlashbackId", source);
            Assert.Contains("GuiltInsomniaId", source);
        }

        [Fact]
        public void PsychologyHandlers_NeverTick_BySource()
        {
            // Belt-and-braces with PsychologyHandlers_AreObserveOnly: no
            // Phase-0 psychology system may be driven from the handler file
            // under any method name.
            string source = ReadRepositoryFile(
                "Assets", "Ashfall.Core", "Medical", "PsychologyAfflictionHandlers.cs");
            Assert.DoesNotContain(".TickDay(", source);
            Assert.DoesNotContain(".TickAll(", source);
            Assert.DoesNotContain("ResetNightFlags", source);
        }

        [Fact]
        public void AfflictionInventory_MarksPsychologyObserveOnly()
        {
            string json = ReadRepositoryFile("docs", "architecture", "affliction_inventory.json");
            Assert.Contains("observe_only_projection", json);
        }
    }
}
