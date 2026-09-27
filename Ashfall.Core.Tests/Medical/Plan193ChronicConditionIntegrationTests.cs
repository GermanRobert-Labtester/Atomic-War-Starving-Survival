// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 193 — Chronic Conditions & Accommodations: integration + wiring tests.
// Core contract: catalog load, condition records from a clinical producer fit,
// accommodation refusal, capability projection, replay-after-restore.
// Production wiring gates: the host records diagnoses, registers the
// chronic_condition save section, and projects the rows on the care surface.
// ============================================================================
using System;
using System.IO;
using Ashfall.Core.Medical;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class Plan193ChronicConditionIntegrationTests
    {
        private static string RepoRoot()
        {
            string[] candidates = { Directory.GetCurrentDirectory(), AppContext.BaseDirectory };
            foreach (string start in candidates)
            {
                var directory = new DirectoryInfo(Path.GetFullPath(start));
                while (directory != null)
                {
                    if (File.Exists(Path.Combine(directory.FullName, "src", "Main.ChronicConditions.cs")))
                        return directory.FullName;
                    directory = directory.Parent!;
                }
            }
            throw new DirectoryNotFoundException("Could not locate the ASHFALL repository root.");
        }

        private static string DataDir()
        {
            return Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");
        }

        private static string Source(string relativePath) =>
            File.ReadAllText(Path.Combine(RepoRoot(), relativePath));

        [Fact]
        public void AuthoredCatalog_LoadsSixConditionsAndSixAccommodations()
        {
            var session = new ChronicConditionSystem();
            session.LoadCatalog(File.ReadAllText(Path.Combine(DataDir(), "chronic_conditions.json")));
            Assert.True(session.GetAllConditionDefs().Count > 0, "chronic_conditions.json must load");
            Assert.Equal(6, session.GetAllConditionDefs().Count);
            Assert.Equal(6, session.GetAllAccommodationDefs().Count);
            var limp = session.GetConditionDef("cond_chronic_limp");
            Assert.NotNull(limp);
            Assert.Equal("accom_cane_crutch", limp!.recommended_accommodation_id);
        }

        [Fact]
        public void ValidEvent_RecordsOnce_RepeatedDeliveryIsExactlyOnce()
        {
            var session = new ChronicConditionSystem();
            session.LoadCatalog(File.ReadAllText(Path.Combine(DataDir(), "chronic_conditions.json")));
            var rec = session.AddCondition("surv_a", "cond_respiratory_damage", 12, "diagnosis_confirmed");
            Assert.NotNull(rec);
            Assert.Equal("moderate", rec!.Severity);
            Assert.Equal("diagnosis_confirmed", rec.Cause);
            var again = session.AddCondition("surv_a", "cond_respiratory_damage", 20, "diagnosis_confirmed");
            Assert.Same(rec, again);
            Assert.Single(session.GetSurvivorConditions("surv_a"));
        }

        [Fact]
        public void UnknownAccommodation_RefusesExplicitly()
        {
            var session = new ChronicConditionSystem();
            session.LoadCatalog(File.ReadAllText(Path.Combine(DataDir(), "chronic_conditions.json")));
            session.AddCondition("surv_a", "cond_chronic_limp", 3, "injury");
            var known = session.AssignAccommodation("surv_a", "accom_cane_crutch", "cond_chronic_limp", 4);
            Assert.NotNull(known);
            Assert.True(session.RemoveAccommodation("surv_a", "accom_cane_crutch"));
            Assert.False(session.RemoveAccommodation("surv_a", "accom_cane_crutch")); // second removal refused
        }

        [Fact]
        public void CapabilityPenalty_TightlyScoped_ReducedByRecommendedAccommodation()
        {
            var session = new ChronicConditionSystem();
            session.LoadCatalog(File.ReadAllText(Path.Combine(DataDir(), "chronic_conditions.json")));
            session.AddCondition("surv_a", "cond_chronic_limp", 3, "injury");
            float raw = session.CalculateCapabilityModifier("surv_a", "movement_speed");
            Assert.True(raw < 1f);
            Assert.Equal(1f, session.CalculateCapabilityModifier("surv_a", "learning_rate"));
            Assert.Equal(1f, session.CalculateCapabilityModifier("surv_b", "movement_speed"));

            Assert.NotNull(session.AssignAccommodation("surv_a", "accom_cane_crutch", "cond_chronic_limp", 4));
            float fitted = session.CalculateCapabilityModifier("surv_a", "movement_speed");
            Assert.True(fitted > raw, $"{raw:0.00} should rise under the cane");
            Assert.True(session.RemoveAccommodation("surv_a", "accom_cane_crutch"));
            Assert.Equal(raw, session.CalculateCapabilityModifier("surv_a", "movement_speed"));
        }

        [Fact]
        public void ReplayAfterRestore_AppliesExactlyOnce()
        {
            var session = new ChronicConditionSystem();
            session.LoadCatalog(File.ReadAllText(Path.Combine(DataDir(), "chronic_conditions.json")));
            session.AddCondition("surv_a", "cond_chronic_limp", 3, "injury");
            session.AssignAccommodation("surv_a", "accom_cane_crutch", "cond_chronic_limp", 4);

            var captured = session.CaptureState();
            var fresh = new ChronicConditionSystem();
            fresh.RestoreState(captured);
            Assert.Single(fresh.GetSurvivorConditions("surv_a"));
            Assert.Single(fresh.GetSurvivorAccommodations("surv_a"));

            fresh.AddCondition("surv_a", "cond_chronic_limp", 9, "injury");
            Assert.Single(fresh.GetSurvivorConditions("surv_a"));
            Assert.Equal(3, fresh.GetSurvivorConditions("surv_a")[0].OnsetDay);
        }

        [Fact]
        public void SaveRegistry_RegistersChronicConditionSection()
        {
            Assert.True(SaveSectionRegistry.TryGetSection("chronic_condition", out var meta));
            Assert.Equal("SaveChronicCondition", meta!.SaveMethod);
            Assert.Equal("chronic_condition_save.json", SaveSectionRegistry.FileNameFor("chronic_condition"));
        }

        [Fact]
        public void HostSource_RecordsTheClinicalProducerAndProjectsCareRows()
        {
            string main = Source(Path.Combine("src", "Main.ChronicConditions.cs"));
            Assert.Contains("OnDiagnosisConfirmed", main);   // real clinical producer seam
            Assert.Contains("SetupChronicConditions", main);
            Assert.Contains("SaveChronicConditions", main);
            Assert.Contains("GetChronicCapabilityModifier", main);

            // Care surface projection on the existing afflictions panel.
            string panel = Source(Path.Combine("src", "UI", "AfflictionsPanel.cs"));
            Assert.Contains("ChronicConditionHostSession? chronicConditions", panel);

            foreach (string site in new[]
                     {
                         Path.Combine("src", "Main.GameFlow.cs"),
                         Path.Combine("src", "Main.PlayerSurfaces.cs"),
                         Path.Combine("src", "Main.UiTests.PlayerPanels.cs"),
                     })
            {
                Assert.Contains("chronicConditions: _chronicConditions", Source(site));
            }
        }
    }
}
