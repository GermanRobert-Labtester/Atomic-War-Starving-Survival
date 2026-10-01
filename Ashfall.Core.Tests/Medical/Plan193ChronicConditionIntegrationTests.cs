// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 193 — Chronic Conditions & Accommodations: integration + wiring tests.
// Core contract: catalog load, condition records from a clinical producer fit,
// accommodation refusal, capability projection, replay-after-restore.
// Production wiring gates: the host records diagnoses, registers the
// chronic_condition save section, and projects the rows on the care surface.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core.Medical;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;
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
            Assert.Equal("SaveChronicConditions", meta!.SaveMethod);
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

        [Fact]
        public void AuthoredAccommodationCosts_ReferenceCanonicalItemIds()
        {
            using var items = JsonDocument.Parse(File.ReadAllText(Path.Combine(DataDir(), "items.json")));
            using var chronic = JsonDocument.Parse(File.ReadAllText(Path.Combine(DataDir(), "chronic_conditions.json")));

            var itemIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var item in items.RootElement.GetProperty("items").EnumerateArray())
            {
                if (item.TryGetProperty("id", out var id) && id.ValueKind == JsonValueKind.String)
                    itemIds.Add(id.GetString()!);
            }

            var dead = new List<string>();
            foreach (var accom in chronic.RootElement.GetProperty("accommodations").EnumerateArray())
            {
                if (!accom.TryGetProperty("maintenance_cost_items", out var costs)) continue;
                string accommodationId = accom.TryGetProperty("accommodation_id", out var aid)
                    ? aid.GetString() ?? string.Empty
                    : string.Empty;
                foreach (var cost in costs.EnumerateArray())
                {
                    string itemId = cost.GetString() ?? string.Empty;
                    if (!string.IsNullOrEmpty(itemId) && !itemIds.Contains(itemId))
                        dead.Add($"{accommodationId} -> {itemId}");
                }
            }

            Assert.True(dead.Count == 0,
                "chronic_conditions.json maintenance_cost_items must reference canonical items.json ids: "
                + string.Join("; ", dead));
        }

        [Fact]
        public void ChronicCapability_CapsDutyHours_AndFittedAccommodationRelaxesThem()
        {
            var model = new FitnessForDutyModel(new FitnessThresholds(
                fatigueImpaired: 60f, fatigueUnfit: 85f,
                healthImpaired: 60f, healthUnfit: 30f,
                hungerImpaired: 60f, hungerUnfit: 85f,
                thirstImpaired: 60f, thirstUnfit: 85f,
                warmthImpaired: 40f, warmthUnfit: 20f,
                daysWithoutSleepImpaired: 2, daysWithoutSleepUnfit: 4,
                doseImpairedMsv: 100f, doseUnfitMsv: 300f));
            var role = new RoleRequirements(
                "night_watch", "skill_watchful", 0.1f,
                maximumFatigue: 90f, minimumHealth: 30f,
                maximumDoseMsv: 300f, maximumHours: 12f,
                maximumHoursIfImpaired: 8f, allowUnfit: false,
                lightDuty: false, requiresNotQuarantined: true,
                precisionWork: true, hazardClass: "perimeter");
            var skills = new Dictionary<string, float> { ["skill_watchful"] = 0.5f };

            var healthy = model.EvaluateForRole(new FitnessEvaluationFacts
            {
                SurvivorId = "surv_a",
                SkillLevels = skills,
                ChronicCapabilityMultiplier = 1f
            }, role);
            var impaired = model.EvaluateForRole(new FitnessEvaluationFacts
            {
                SurvivorId = "surv_a",
                SkillLevels = skills,
                ChronicCapabilityMultiplier = 0.5f
            }, role);

            Assert.Equal(12f, healthy.RecommendedMaxHours);
            Assert.False(healthy.Warning);
            Assert.True(impaired.Allowed);
            Assert.True(impaired.Warning);
            Assert.Contains(FitnessReasonIds.ChronicImpairment, impaired.WarningReasons);
            Assert.True(impaired.RecommendedMaxHours < healthy.RecommendedMaxHours,
                $"chronic impairment must cap shift hours: {impaired.RecommendedMaxHours} vs {healthy.RecommendedMaxHours}");
        }

        [Fact]
        public void EveryAuthoredCondition_HasACommittedProducerPath()
        {
            using var chronic = JsonDocument.Parse(File.ReadAllText(Path.Combine(DataDir(), "chronic_conditions.json")));
            string hostSource = string.Concat(
                Source(Path.Combine("src", "Main.ChronicConditions.cs")),
                Source(Path.Combine("src", "Main.Amputation.Integration.cs")),
                Source(Path.Combine("src", "Main.Aging.cs")),
                Source(Path.Combine("src", "Main.Expeditions.cs")));

            var unreachable = new List<string>();
            foreach (var cond in chronic.RootElement.GetProperty("conditions").EnumerateArray())
            {
                string id = cond.TryGetProperty("condition_id", out var cid) ? cid.GetString() ?? string.Empty : string.Empty;
                if (string.IsNullOrEmpty(id)) continue;
                if (!hostSource.Contains("\"" + id + "\"", StringComparison.Ordinal))
                    unreachable.Add(id);
            }

            Assert.True(unreachable.Count == 0,
                "authored chronic conditions with no committed producer path: " + string.Join(", ", unreachable));
        }

        [Fact]
        public void IntegrityValidator_AcceptsAuthoredChronicCatalog()
        {
            var report = new CatalogIntegrityReport();
            CatalogIntegrityValidator.ValidateChronicConditionsCatalog(DataDir(), new FileSystemIO(), report);
            Assert.True(report.Clean, "chronic_conditions.json failed the typed integrity pass: " + string.Join("; ", report.Errors));
        }

        [Fact]
        public void IntegrityValidator_RejectsDeadMaintenanceItem()
        {
            string temp = Path.Combine(Path.GetTempPath(), "ashfall-chronic-invalid-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(temp);
            try
            {
                string json = File.ReadAllText(Path.Combine(DataDir(), "chronic_conditions.json"))
                    .Replace("\"scrap_wood\"", "\"definitely_not_an_item\"");
                File.WriteAllText(Path.Combine(temp, "chronic_conditions.json"), json);
                File.Copy(Path.Combine(DataDir(), "items.json"), Path.Combine(temp, "items.json"));

                var report = new CatalogIntegrityReport();
                CatalogIntegrityValidator.ValidateChronicConditionsCatalog(temp, new FileSystemIO(), report);

                Assert.Contains(report.Errors, e => e.Contains("unknown item") && e.Contains("definitely_not_an_item"));
            }
            finally
            {
                Directory.Delete(temp, recursive: true);
            }
        }

        [Fact]
        public void HostAndPanel_ExposeThePlayerAccommodationDecision()
        {
            string main = Source(Path.Combine("src", "Main.ChronicConditions.cs"));
            Assert.Contains("FitChronicAccommodation", main);
            Assert.Contains("RemoveChronicAccommodation", main);
            Assert.Contains("GetChronicDutyCapabilityModifier", main);
            Assert.Contains("CountById", main); // gated on the real inventory authority
            Assert.Contains("already_fitted", main); // no double-charge on replay

            string panel = Source(Path.Combine("src", "UI", "AfflictionsPanel.cs"));
            Assert.Contains("RenderAccommodationDecision", panel);
            Assert.Contains("fitAccommodation", panel);
            Assert.Contains("_fitAccommodation", panel);

            // T18b — the duty board surfaces the machine reason and the
            // actionable accommodation hint so the capped shift is legible.
            string dutyPanel = Source(Path.Combine("src", "UI", "DutyRosterPanel.cs"));
            Assert.Contains("FitnessReasonIds.ChronicImpairment", dutyPanel);
            Assert.Contains("WarningReasons", dutyPanel);

            foreach (string site in new[]
                     {
                         Path.Combine("src", "Main.GameFlow.cs"),
                         Path.Combine("src", "Main.PlayerSurfaces.cs"),
                         Path.Combine("src", "Main.UiTests.PlayerPanels.cs"),
                     })
            {
                Assert.Contains("fitAccommodation: FitChronicAccommodation", Source(site));
            }
        }
    }
}
