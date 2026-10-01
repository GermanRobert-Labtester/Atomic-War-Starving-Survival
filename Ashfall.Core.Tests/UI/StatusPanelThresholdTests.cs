// SPDX-License-Identifier: MIT
// P009–P012 survival-legibility wiring gate.
//
// The host UI is not compiled into this test project, so these are source-level
// contracts: each presentation surface must consume the owning authority
// (needs profile / RadiationSystem constants / host trend read model) rather
// than re-typing values, and each producer must have a call site.
using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public sealed class SurvivalLegibilityGateTests
    {
        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(Path.GetFullPath(Directory.GetCurrentDirectory()));
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, "src"))
                    && Directory.Exists(Path.Combine(dir.FullName, "Assets")))
                    return dir.FullName;
                dir = dir.Parent!;
            }
            throw new DirectoryNotFoundException("repository root not found");
        }

        private static string Read(string relativePath)
            => File.ReadAllText(Path.Combine(RepoRoot(), relativePath.Replace('/', Path.DirectorySeparatorChar)));

        [Fact]
        public void Hud_HasNeedsGlanceRow_AndStaysEventDriven()
        {
            string hud = Read("src/UI/GameHudOverlay.cs");
            Assert.Contains("UpdateNeeds", hud, StringComparison.Ordinal);
            Assert.Contains("_lblHunger", hud, StringComparison.Ordinal);
            Assert.Contains("_lblThirst", hud, StringComparison.Ordinal);
            Assert.Contains("_lblFatigue", hud, StringComparison.Ordinal);
            Assert.Contains("_lblMorale", hud, StringComparison.Ordinal);
            Assert.Contains("_lblWarmth", hud, StringComparison.Ordinal);
            Assert.Contains("ui.hud.needs.warmth", hud, StringComparison.Ordinal);
            // The event-driven contract must not regress to per-frame polling.
            Assert.DoesNotContain("_Process(", hud, StringComparison.Ordinal);
        }

        [Fact]
        public void Hud_NeedsProjection_IsWiredFromTheHudUpdate()
        {
            string gameFlow = Read("src/Main.GameFlow.cs");
            Assert.Contains("UpdateNeeds(", gameFlow, StringComparison.Ordinal);
            // The HUD projection must read the owning NeedsProfile (the exact
            // local/receiver name is free to refactor; the authority is not).
            Assert.Contains(".Needs.Profile", gameFlow, StringComparison.Ordinal);

            // The HUD critical band must come from the owning profile, not a
            // re-typed magic number (NeedsProfile owns hunger/thirst criticals).
            string hud = Read("src/UI/GameHudOverlay.cs");
            Assert.Contains("NeedsProfile", hud, StringComparison.Ordinal);
            Assert.Contains("profile?.hungerCritical", hud, StringComparison.Ordinal);
            Assert.Contains("profile?.thirstCritical", hud, StringComparison.Ordinal);
            // Task 3/6 — warn bands and labels are authored/localized.
            Assert.Contains("profile?.hungerWarn", hud, StringComparison.Ordinal);
            Assert.Contains("ui.hud.needs.hunger", hud, StringComparison.Ordinal);
            // Loop-2 hardening — the HUD resolves through the shared helper too.
            Assert.Contains("AshfallUiText", hud, StringComparison.Ordinal);
            Assert.DoesNotContain("=> AshfallLocalization.Tr(", hud, StringComparison.Ordinal);
        }

        [Fact]
        public void FeedbackThresholds_ReadTheProfile_NotHardcoded90()
        {
            string gameFlow = Read("src/Main.GameFlow.cs");
            Assert.Contains("needsProfile?.hungerCritical", gameFlow, StringComparison.Ordinal);
            Assert.Contains("needsProfile?.thirstCritical", gameFlow, StringComparison.Ordinal);
            Assert.Contains("needsProfile?.hungerWarn", gameFlow, StringComparison.Ordinal);
            Assert.DoesNotContain("Hunger >= 90", gameFlow, StringComparison.Ordinal);
            Assert.DoesNotContain("Thirst >= 90", gameFlow, StringComparison.Ordinal);
        }

        [Fact]
        public void Panels_RenderWarmthDriftAndMoraleDelta()
        {
            string status = Read("src/UI/StatusPanel.cs");
            Assert.Contains("Warmth Drift", status, StringComparison.Ordinal);
            Assert.Contains("TryGetNeedDayDelta(s.Id, NeedKind.Warmth", status, StringComparison.Ordinal);

            string roster = Read("src/UI/SurvivorsPanel.cs");
            Assert.Contains("DayDelta(survivor.Id, NeedKind.Morale)", roster, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorsHost_ConsumesOnNeedCritical()
        {
            string session = Read("src/Host/SurvivorsHostSession.cs");
            Assert.Contains("Needs.OnNeedCritical", session, StringComparison.Ordinal);
        }

        [Fact]
        public void HoldfastRuntime_FallbackDecaysAllFiveNeeds()
        {
            // Task 5 — the no-survivors fallback path must not freeze any of the
            // five need projections; a static HUD is a legibility lie.
            string runtime = Read("src/Host/HoldfastRuntimeSession.cs");
            Assert.Contains("_fallbackHunger = Math.Min(MaxHunger, _fallbackHunger + 8)", runtime, StringComparison.Ordinal);
            Assert.Contains("_fallbackThirst = Math.Min(MaxThirst, _fallbackThirst + 10)", runtime, StringComparison.Ordinal);
            Assert.Contains("_fallbackFatigue = Math.Min(100, _fallbackFatigue + 6)", runtime, StringComparison.Ordinal);
            Assert.Contains("_fallbackMorale = Math.Max(0, _fallbackMorale - 1)", runtime, StringComparison.Ordinal);
            Assert.Contains("_fallbackWarmth = Math.Max(0, _fallbackWarmth - 12)", runtime, StringComparison.Ordinal);
        }

        [Fact]
        public void GameHudSnapshotFixture_TracksEveryUpdateSignature()
        {
            // Task 6 — if a future UpdateX method is renamed or re-signatured the
            // fixture must fail loudly instead of silently rendering a stale HUD.
            string hud = Read("src/UI/GameHudOverlay.cs");
            foreach (string method in new[]
            {
                "public void UpdateState(", "public void UpdateHealth(", "public void UpdateRadiation(",
                "public void UpdateNeeds(", "public void UpdateOnboardingProgress(",
            })
                Assert.Contains(method, hud, StringComparison.Ordinal);

            string fixture = Read("src/UI/GameHudSnapshotFixture.cs");
            foreach (string call in new[]
            {
                "hud.UpdateState(", "hud.UpdateHealth(", "hud.UpdateRadiation(",
                "hud.UpdateNeeds(", "hud.UpdateOnboardingProgress(",
            })
                Assert.Contains(call, fixture, StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_CohortRows_AreLocalized()
        {
            // Task 1 — the four cohort rows must resolve from strings.csv.
            string status = Read("src/UI/StatusPanel.cs");
            foreach (string key in new[]
            {
                "ui.status.cohort.survivors", "ui.status.cohort.avg_health",
                "ui.status.cohort.morale", "ui.status.cohort.dose",
            })
                Assert.Contains(key, status, StringComparison.Ordinal);
            Assert.DoesNotContain("AddStatRow(\"Survivor Cohort\"", status, StringComparison.Ordinal);
            Assert.DoesNotContain("AddStatRow(\"Dosimetry Dose\"", status, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorsPanel_Headers_AreLocalized()
        {
            // Task 2 — roster/telemetry headers resolve from strings.csv.
            string panel = Read("src/UI/SurvivorsPanel.cs");
            Assert.Contains("ui.survivors.header.roster", panel, StringComparison.Ordinal);
            Assert.Contains("ui.survivors.header.telemetry", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("MakeSectionHeader(\"RESIDENT ROSTER\"", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("MakeSectionHeader(\"COHORT TELEMETRY\"", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void UiPanels_ResolveThroughSharedAshfallUiText()
        {
            // Task 4 — one shared helper owns UI string resolution.
            Assert.Contains("class AshfallUiText", Read("src/UI/AshfallUiText.cs"), StringComparison.Ordinal);
            foreach (string file in new[] { "src/UI/StatusPanel.cs", "src/UI/SurvivorsPanel.cs" })
                Assert.Contains("AshfallUiText.Tr(", Read(file), StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_HasMoraleDriftRow()
        {
            // Task 8 — morale is tracked by the day-delta owner but was omitted.
            string status = Read("src/UI/StatusPanel.cs");
            Assert.Contains("ui.status.drift.morale", status, StringComparison.Ordinal);
            Assert.Contains("TryGetNeedDayDelta(s.Id, NeedKind.Morale", status, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorsRail_ReadsSharedRadiationAndMoraleBands()
        {
            // Task 9 — rail cards colour from the shared bands, not 25/50/100 or 60/30.
            string panel = Read("src/UI/SurvivorsPanel.cs");
            Assert.Contains("RadiationSystem.WarnThreshold", panel, StringComparison.Ordinal);
            Assert.Contains("RadiationSystem.AcuteThreshold", panel, StringComparison.Ordinal);
            Assert.Contains("profile.moraleWarn", panel, StringComparison.Ordinal);
            Assert.Contains("profile.moraleCritical", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("avgMor >= 60", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorDetailPanel_RadiationRow_UsesSharedWarnBand()
        {
            // Task 7 — the per-survivor radiation row must colour from the shared
            // band. The authoritative implementation moved from an inline
            // WarnThreshold comparison to the shared AshfallUiBands.ForDose
            // helper (which reads the same RadiationSystem thresholds), so the
            // gate pins the helper the row actually consumes.
            string panel = Read("src/UI/SurvivorDetailPanel.cs");
            Assert.Contains("AshfallUiBands.ForDose", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("rad.RadiationDose >= 50", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void RadiationWarnToast_IsHostOwned_AndNotDuplicated()
        {
            // Task 11 — the host owns the radiation warn-band toast, deduped per
            // survivor; the player path no longer re-emits it.
            string host = Read("src/Host/SurvivorsHostSession.cs");
            Assert.Contains("Radiation.OnDoseChanged", host, StringComparison.Ordinal);
            Assert.Contains("radiation_high", host, StringComparison.Ordinal);
            Assert.Contains("_radWarnNotified", host, StringComparison.Ordinal);
            Assert.Contains("RadiationSystem.WarnThreshold", host, StringComparison.Ordinal);

            string gameFlow = Read("src/Main.GameFlow.cs");
            Assert.DoesNotContain("radiation_high_rate", gameFlow, StringComparison.Ordinal);
        }

        [Fact]
        public void UpdateHud_CapturesSurvivorsLocal_BeforeUse()
        {
            // Task 13 — lifecycle resets null the field; the local guard must stay.
            string gameFlow = Read("src/Main.GameFlow.cs");
            Assert.Contains("var survivors = _survivors;", gameFlow, StringComparison.Ordinal);
            Assert.Contains("if (survivors == null) return;", gameFlow, StringComparison.Ordinal);
        }

        [Fact]
        public void Hud_NeedChipTooltip_IsAssignedOnce_WithBandKeys()
        {
            // Loop-3 hardening — the authored .high/.low band tooltips must not
            // be overwritten by the generic key (a dead assignment).
            string hud = Read("src/UI/GameHudOverlay.cs");
            Assert.Contains("ui.hud.needs.tooltip.high", hud, StringComparison.Ordinal);
            Assert.Contains("ui.hud.needs.tooltip.low", hud, StringComparison.Ordinal);
            Assert.DoesNotContain("\"ui.hud.needs.tooltip\", tag", hud, StringComparison.Ordinal);
        }

        [Fact]
        public void RadiationWarnToast_RearmsOnRestore()
        {
            // Loop-4 hardening — a restored campaign must not inherit the
            // previous session's radiation-warn notified set.
            string host = Read("src/Host/SurvivorsHostSession.cs");
            Assert.Contains("_radWarnNotified.Clear();", host, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorsPanel_DayDelta_UsesSharedFormatter()
        {
            // Loop-5 hardening — the roster delta must not re-implement signed
            // formatting and reintroduce the "-0" / "NaN" leak.
            string panel = Read("src/UI/SurvivorsPanel.cs");
            Assert.Contains("NeedsDayDeltaFormat.Signed(delta)", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("(delta >= 0f ?", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void LowResourceToasts_PassLocalizedDisplayNames()
        {
            // Task 15 — the low-food/low-water toast args carry a localized
            // resource display name, not just a bare percentage.
            string gameFlow = Read("src/Main.GameFlow.cs");
            Assert.Contains("ui.resource.food", gameFlow, StringComparison.Ordinal);
            Assert.Contains("ui.resource.water", gameFlow, StringComparison.Ordinal);
            string catalog = Read("Assets/Ashfall.Core/Feedback/FeedbackMessageCatalogLoader.cs");
            Assert.Contains("\"food_low\", \"resource_warning\", \"warning\", \"WARNING: {1} at {0}%", catalog, StringComparison.Ordinal);
            Assert.Contains("\"water_low\", \"resource_warning\", \"warning\", \"WARNING: {1} at {0}%", catalog, StringComparison.Ordinal);
        }

        [Fact]
        public void AcuteRadiationLesson_IsEvaluatedOncePerJourney()
        {
            string medical = Read("src/Main.Medical.cs");
            Assert.Contains("_acuteRadLessonJourney", medical, StringComparison.Ordinal);
            Assert.Contains("ReferenceEquals(_acuteRadLessonJourney, _onboardingJourney)", medical, StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_ReadsRealThresholds_NotMagicNumbers()
        {
            string status = Read("src/UI/StatusPanel.cs");
            Assert.Contains("_thresholdData", status, StringComparison.Ordinal);
            Assert.Contains("RadiationSystem.AcuteThreshold", status, StringComparison.Ordinal);
            Assert.Contains("RadiationSystem.HealthLossPerHourAtAcute", status, StringComparison.Ordinal);
            Assert.Contains("profile.hungerCritical", status, StringComparison.Ordinal);
            Assert.Contains("profile.thirstCritical", status, StringComparison.Ordinal);
            Assert.Contains("profile.warmthCritical", status, StringComparison.Ordinal);
        }

        [Fact]
        public void NeedsTrend_HasProducerAndPanelConsumers()
        {
            string owner = Read("src/Main.CampaignOwners.cs");
            Assert.Contains("CaptureNeedsBaseline(day)", owner, StringComparison.Ordinal);

            string survivorsPanel = Read("src/UI/SurvivorsPanel.cs");
            Assert.Contains("TryGetNeedDayDelta", survivorsPanel, StringComparison.Ordinal);

            string statusPanel = Read("src/UI/StatusPanel.cs");
            Assert.Contains("TryGetNeedDayDelta", statusPanel, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorsPanel_StrainThresholds_ReadTheProfile()
        {
            // Sweep repair: the roster previously re-typed 90/20 while the
            // simulation profile was the authority. Pin the shared predicates.
            string survivorsPanel = Read("src/UI/SurvivorsPanel.cs");
            Assert.Contains("IsHungerCritical(s.Hunger)", survivorsPanel, StringComparison.Ordinal);
            Assert.Contains("IsThirstCritical(s.Thirst)", survivorsPanel, StringComparison.Ordinal);
            // The profile now exposes an explicit predicate; accept it as the
            // authoritative low-is-bad check instead of the raw field.
            Assert.Contains("IsWarmthCritical(s.Warmth)", survivorsPanel, StringComparison.Ordinal);
            Assert.DoesNotContain("Hunger >= 90f", survivorsPanel, StringComparison.Ordinal);
            Assert.DoesNotContain("Warmth <= 20f", survivorsPanel, StringComparison.Ordinal);
        }

        [Fact]
        public void AcuteRadiationLesson_HasATriggerInProduction()
        {
            string medical = Read("src/Main.Medical.cs");
            Assert.Contains("MaybeRequestAcuteRadiationLesson", medical, StringComparison.Ordinal);
            Assert.Contains("OnboardingLessonLocalization.AcuteRadiationId", medical, StringComparison.Ordinal);

            string gameFlow = Read("src/Main.GameFlow.cs");
            Assert.Contains("MaybeRequestAcuteRadiationLesson()", gameFlow, StringComparison.Ordinal);
        }

        [Fact]
        public void NeedsProfile_WarnBands_PrecedeCriticalBands()
        {
            var p = new NeedsProfile();

            // High-is-bad needs: warn < critical.
            Assert.True(p.hungerWarn < p.hungerCritical);
            Assert.True(p.thirstWarn < p.thirstCritical);
            Assert.True(p.fatigueWarn < p.fatigueCritical);
            // Low-is-bad needs: critical < warn.
            Assert.True(p.moraleCritical < p.moraleWarn);
            Assert.True(p.warmthCritical < p.warmthWarn);

            // Every band lives inside the authored 0..100 need range.
            foreach (var band in new[]
            {
                p.hungerWarn, p.hungerCritical, p.thirstWarn, p.thirstCritical,
                p.fatigueWarn, p.fatigueCritical, p.moraleWarn, p.moraleCritical,
                p.warmthWarn, p.warmthCritical
            })
            {
                Assert.InRange(band, 0f, 100f);
            }
        }

        [Fact]
        public void StatusPanel_ThresholdAndDriftLabels_AreLocalized()
        {
            string status = Read("src/UI/StatusPanel.cs");
            Assert.Contains("ui.status.threshold.starvation", status, StringComparison.Ordinal);
            Assert.Contains("ui.status.threshold.acute_radiation", status, StringComparison.Ordinal);
            Assert.Contains("ui.status.drift.hunger", status, StringComparison.Ordinal);
            Assert.Contains("ui.status.drift.warmth", status, StringComparison.Ordinal);
            Assert.Contains("ui.status.threshold.header", status, StringComparison.Ordinal);
            // Task 4 — panels now resolve through the shared AshfallUiText helper.
            Assert.Contains("AshfallUiText", status, StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_NoBaseline_ShowsTruthfulNote_AndAnnotatesSpan()
        {
            string status = Read("src/UI/StatusPanel.cs");
            Assert.Contains("ui.status.drift.pending", status, StringComparison.Ordinal);
            Assert.Contains("ui.status.drift.per_span", status, StringComparison.Ordinal);
            Assert.Contains("HasNeedsBaseline", status, StringComparison.Ordinal);
            Assert.Contains("NeedDaySpan", status, StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_BuildsThresholdDriftAndThermalSections()
        {
            string status = Read("src/UI/StatusPanel.cs");
            Assert.Contains("_thresholdData", status, StringComparison.Ordinal);
            Assert.Contains("_thermalData", status, StringComparison.Ordinal);
            Assert.Contains("_forecastData", status, StringComparison.Ordinal);
            Assert.Contains("RenderThresholds()", status, StringComparison.Ordinal);
            Assert.Contains("RenderThermal()", status, StringComparison.Ordinal);
            Assert.Contains("ui.status.threshold.value.starvation", status, StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_UsesSharedColdPredicate_NotRepeatedComparison()
        {
            string status = Read("src/UI/StatusPanel.cs");
            Assert.Contains("IsWarmthCritical", status, StringComparison.Ordinal);
            Assert.DoesNotContain("<= profile.warmthCritical", status, StringComparison.Ordinal);

            string roster = Read("src/UI/SurvivorsPanel.cs");
            Assert.Contains("IsWarmthCritical", roster, StringComparison.Ordinal);
            Assert.DoesNotContain("<= profile.warmthCritical", roster, StringComparison.Ordinal);
        }

        [Fact]
        public void ThresholdValues_AreLocalized_AndSignedRounds()
        {
            string status = Read("src/UI/StatusPanel.cs");
            foreach (var key in new[]
            {
                "ui.status.threshold.value.starvation",
                "ui.status.threshold.value.dehydration",
                "ui.status.threshold.value.hypothermia",
                "ui.status.threshold.value.acute_radiation",
                "ui.status.threshold.value.chronic_exposure"
            })
            {
                Assert.Contains(key, status, StringComparison.Ordinal);
            }
            // `Signed` is a shared Core helper: it must clamp to the need range,
            // round before formatting (no misleading "-0"), and reject non-finite deltas.
            Assert.Contains("NeedsDayDeltaFormat.Signed", status, StringComparison.Ordinal);
            string format = Read("Assets/Ashfall.Core/Survivors/NeedsDayDeltaFormat.cs");
            Assert.Contains("Math.Clamp((double)value, -100d, 100d)", format, StringComparison.Ordinal);
            Assert.Contains("Math.Round(clamped, MidpointRounding.AwayFromZero)", format, StringComparison.Ordinal);
            Assert.Contains("float.IsNaN(value) || float.IsInfinity(value)", format, StringComparison.Ordinal);
        }

        [Fact]
        public void Panels_UseProfileCriticalPredicates_NotRawComparisons()
        {
            // Hardening (5-loop): every critical comparison must go through the
            // NeedsProfile predicate so the threshold can never drift per panel.
            string roster = Read("src/UI/SurvivorsPanel.cs");
            Assert.Contains("profile.IsHungerCritical", roster, StringComparison.Ordinal);
            Assert.Contains("profile.IsThirstCritical", roster, StringComparison.Ordinal);
            Assert.DoesNotContain(">= profile.hungerCritical", roster, StringComparison.Ordinal);
            Assert.DoesNotContain(">= profile.thirstCritical", roster, StringComparison.Ordinal);

            string profile = Read("Assets/Ashfall.Core/Survivors/NeedsSystem.cs");
            Assert.Contains("public bool IsHungerCritical", profile, StringComparison.Ordinal);
            Assert.Contains("public bool IsThirstCritical", profile, StringComparison.Ordinal);
            Assert.Contains("public bool IsFatigueCritical", profile, StringComparison.Ordinal);
            Assert.Contains("public bool IsMoraleCritical", profile, StringComparison.Ordinal);

            // Every survival surface, not just the roster, must use the predicates.
            foreach (var path in new[] { "src/UI/StatusPanel.cs", "src/UI/SurvivorDetailPanel.cs" })
            {
                string panel = Read(path);
                Assert.DoesNotContain(">= profile.hungerCritical", panel, StringComparison.Ordinal);
                Assert.DoesNotContain(">= profile.thirstCritical", panel, StringComparison.Ordinal);
                Assert.DoesNotContain("<= profile.moraleCritical", panel, StringComparison.Ordinal);
                Assert.DoesNotContain("<= profile.warmthCritical", panel, StringComparison.Ordinal);
            }
        }

        [Fact]
        public void Hud_Keys_ArePresentInTheCatalog()
        {
            string csv = Read("assets/l10n/strings.csv");
            foreach (var key in new[]
            {
                "ui.hud.day", "ui.hud.health", "ui.hud.radiation", "ui.hud.value",
                "ui.hud.label.hp", "ui.hud.label.rad",
                "ui.hud.needs.hunger", "ui.hud.needs.thirst", "ui.hud.needs.fatigue",
                "ui.hud.needs.morale", "ui.hud.needs.warmth",
                "ui.hud.needs.tooltip.high", "ui.hud.needs.tooltip.low"
            })
            {
                Assert.Contains($"{key},", csv, StringComparison.Ordinal);
            }
        }

        [Fact]
        public void Hud_NeedChips_CarryBandTooltips()
        {
            string hud = Read("src/UI/GameHudOverlay.cs");
            Assert.Contains("ui.hud.needs.tooltip.high", hud, StringComparison.Ordinal);
            Assert.Contains("ui.hud.needs.tooltip.low", hud, StringComparison.Ordinal);
            Assert.Contains("TooltipText", hud, StringComparison.Ordinal);

            // Hardening: the high/low bands must not collapse to identical copy
            // (a low-is-bad chip has to read ≤, not ≥).
            string csv = Read("assets/l10n/strings.csv");
            Assert.Contains("ui.hud.needs.tooltip.high,Warn ≥ {0}", csv, StringComparison.Ordinal);
            Assert.Contains("ui.hud.needs.tooltip.low,Warn ≤ {0}", csv, StringComparison.Ordinal);
        }

        [Fact]
        public void Hud_CarriesWarmthProjection_FromTheRuntime()
        {
            string runtime = Read("src/Host/HoldfastRuntimeSession.cs");
            Assert.Contains("public int Warmth", runtime, StringComparison.Ordinal);
            Assert.Contains("_fallbackWarmth", runtime, StringComparison.Ordinal);

            string gameFlow = Read("src/Main.GameFlow.cs");
            Assert.Contains("_holdfastRuntime.Warmth", gameFlow, StringComparison.Ordinal);
        }

        [Fact]
        public void StringsCatalog_HasEverySurvivalLegibilityKey()
        {
            // T3 — the format gate checks shape; this pins the exact keys the
            // code calls so a renamed/removed row fails loudly.
            string csv = Read("assets/l10n/strings.csv");
            string[] keys =
            {
                "ui.hud.needs.hunger", "ui.hud.needs.thirst", "ui.hud.needs.fatigue",
                "ui.hud.needs.morale", "ui.hud.needs.warmth",
                "ui.hud.needs.tooltip.high", "ui.hud.needs.tooltip.low",
                "ui.hud.day", "ui.hud.health", "ui.hud.radiation", "ui.hud.value",
                "ui.hud.label.hp", "ui.hud.label.rad",
                "ui.status.threshold.header", "ui.status.threshold.starvation",
                "ui.status.threshold.dehydration", "ui.status.threshold.hypothermia",
                "ui.status.threshold.acute_radiation", "ui.status.threshold.chronic_exposure",
                "ui.status.drift.hunger", "ui.status.drift.thirst",
                "ui.status.drift.fatigue", "ui.status.drift.morale", "ui.status.drift.warmth",
                "ui.status.cohort.survivors", "ui.status.cohort.avg_health",
                "ui.status.cohort.morale", "ui.status.cohort.dose",
                "ui.survivors.header.roster", "ui.survivors.header.telemetry",
                "ui.survivors.filter.all", "ui.survivors.filter.living",
                "ui.survivors.filter.strained", "ui.survivors.filter.critical",
                "ui.resource.food", "ui.resource.water",
                "expedition.protection.title", "expedition.protection.body",
                "weather.storm_prep.title", "weather.storm_prep.body",
                "combat.basics.title", "combat.basics.body",
                "medical.acute_radiation.title", "medical.acute_radiation.body",
            };
            foreach (string key in keys)
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
        }

        [Fact]
        public void NeedsProfile_ExposesHealthBands()
        {
            var p = new NeedsProfile();
            Assert.Equal(30f, p.healthWarn, 3);
            Assert.Equal(25f, p.healthCritical, 3);
            Assert.True(p.healthCritical < p.healthWarn);
            // The host fallback thresholds derive from the same shared defaults.
            Assert.Equal(90f, NeedsProfile.DefaultHungerCritical, 3);
            Assert.Equal(90f, NeedsProfile.DefaultThirstCritical, 3);
        }

        [Fact]
        public void SurvivalPanels_ReadProfileBands_NotLiterals()
        {
            // Loop-1 hardening: the survival surfaces must colour from the one
            // authored profile, so a future edit cannot silently re-introduce a
            // drifting magic threshold.
            string[] files =
            {
                "src/UI/StatusPanel.cs", "src/UI/SurvivorsPanel.cs",
                "src/UI/SurvivalDetailPanel.cs", "src/UI/SurvivorDetailPanel.cs",
            };
            foreach (string file in files)
            {
                string src = Read(file);
                Assert.Contains("Needs.Profile", src, StringComparison.Ordinal);
                Assert.DoesNotContain("Health < 30f", src, StringComparison.Ordinal);
                Assert.DoesNotContain("Health < 30 ?", src, StringComparison.Ordinal);
                Assert.DoesNotContain("Hunger >= 90 ?", src, StringComparison.Ordinal);
                Assert.DoesNotContain("Thirst >= 90 ?", src, StringComparison.Ordinal);
                Assert.DoesNotContain("Warmth < 20 ?", src, StringComparison.Ordinal);
            }
        }

        [Fact]
        public void RadiationWatchBand_IsShared_NotRetyped()
        {
            // Loop-2 hardening: one authored watch band, consumed by every
            // radiation-colouring surface.
            Assert.Equal(50f, Ashfall.Core.Radiation.RadiationSystem.WarnThreshold, 3);
            Assert.True(Ashfall.Core.Radiation.RadiationSystem.WarnThreshold
                < Ashfall.Core.Radiation.RadiationSystem.AcuteThreshold);
            foreach (string file in new[]
            {
                "src/UI/StatusPanel.cs", "src/UI/SurvivalDetailPanel.cs", "src/Host/SurvivorsHostSession.cs",
            })
            {
                Assert.Contains("RadiationSystem.WarnThreshold", Read(file), StringComparison.Ordinal);
            }
        }

        [Fact]
        public void NeedCriticalAlert_UsesDisplayNameAndPerSurvivorDedupe()
        {
            // Loop-3 hardening: the visible toast must name the person and be
            // deduped per survivor+kind so it never double-fires with the player path.
            string session = Read("src/Host/SurvivorsHostSession.cs");
            Assert.Contains("OnNeedCritical", session, StringComparison.Ordinal);
            Assert.Contains("FindDefinition(s.Id)?.displayName", session, StringComparison.Ordinal);
            Assert.Contains("starvation_imminent", session, StringComparison.Ordinal);
            Assert.Contains("dehydration_imminent", session, StringComparison.Ordinal);
            Assert.Contains("hypothermia_risk", session, StringComparison.Ordinal);
            Assert.Contains("need_critical_", session, StringComparison.Ordinal);
        }

        [Fact]
        public void RadiationMessages_InterpolateTheSharedBand()
        {
            // Loop-4 hardening: the readout text must interpolate the shared
            // band so a future threshold change cannot leave a stale "50" in prose.
            foreach (string file in new[] { "src/UI/StatusPanel.cs", "src/UI/SurvivalDetailPanel.cs" })
            {
                string src = Read(file);
                Assert.Contains("WarnThreshold:0", src, StringComparison.Ordinal);
                Assert.DoesNotContain("above 50 mSv", src, StringComparison.Ordinal);
            }
        }

        // ── Third-wave task gates ──────────────────────────────────────────

        [Fact]
        public void NoUiTextHelper_CallsAshfallLocalizationDirectly()
        {
            // Task 1 — every private T/Tr helper delegates to AshfallUiText, so a
            // future localization change lands everywhere at once.
            string uiDir = Path.Combine(RepoRoot(), "src", "UI");
            foreach (string file in Directory.GetFiles(uiDir, "*.cs", SearchOption.AllDirectories))
            {
                string name = Path.GetFileName(file);
                if (name == "AshfallUiText.cs") continue;
                string src = File.ReadAllText(file);
                Assert.DoesNotContain("string T(string key, string fallback) => AshfallLocalization", src, StringComparison.Ordinal);
                Assert.DoesNotContain("string Tr(string key, string fallback) => AshfallLocalization", src, StringComparison.Ordinal);
                Assert.DoesNotContain("AshfallLocalization.Tr(key, fallback);", src, StringComparison.Ordinal);
            }
            Assert.Contains("class AshfallUiText", Read("src/UI/AshfallUiText.cs"), StringComparison.Ordinal);
        }

        // ── Third-wave loop gates ──────────────────────────────────────────

        [Fact]
        public void LocalizationHelpers_DoNotImportTheCatalogNamespace()
        {
            // Loop-1 hardening — panels resolve through AshfallUiText, not the catalog.
            foreach (string file in new[]
            {
                "src/UI/StatusPanel.cs", "src/UI/SurvivorsPanel.cs", "src/UI/GameHudOverlay.cs",
                "src/UI/ResearchPanel.cs", "src/UI/OnboardingHintPanel.cs",
            })
                Assert.DoesNotContain("using AtomicWar.GodotApp.Localization;", Read(file), StringComparison.Ordinal);
        }

        [Fact]
        public void ResearchPanel_FormatHelper_DoesNotReinitializeTheCatalog()
        {
            // Loop-3 hardening — AshfallUiText.Tr initializes; no explicit Initialize.
            Assert.DoesNotContain("AshfallLocalization.Initialize()", Read("src/UI/ResearchPanel.cs"), StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_ThermalAndForecastRows_AreLocalized()
        {
            // Loop-4 hardening — the remaining static rows resolve from strings.csv.
            string status = Read("src/UI/StatusPanel.cs");
            foreach (string key in new[]
            {
                "ui.status.thermal.roster_warmth", "ui.status.thermal.clothing_warmth",
                "ui.status.thermal.shelter_thermal", "ui.status.thermal.waterway",
                "ui.status.forecast.supply_projection", "ui.status.forecast.shortfall",
            })
                Assert.Contains(key, status, StringComparison.Ordinal);
        }

        [Fact]
        public void DriftFormatting_IsCentralized()
        {
            // Loop-5 hardening — no UI panel re-implements the signed drift format.
            foreach (string file in new[] { "src/UI/StatusPanel.cs", "src/UI/SurvivorsPanel.cs" })
            {
                string src = Read(file);
                Assert.Contains("NeedsDayDeltaFormat", src, StringComparison.Ordinal);
                Assert.DoesNotContain("(delta >= 0f ?", src, StringComparison.Ordinal);
            }
        }

        [Fact]
        public void Hud_ApplyNeedChip_AssignsTooltipExactlyOnce()
        {
            // Task 2 — a second assignment silently discards the band tooltip.
            string hud = Read("src/UI/GameHudOverlay.cs");
            int start = hud.IndexOf("private static void ApplyNeedChip", StringComparison.Ordinal);
            int end = hud.IndexOf("private static Label MakeNeedChip", StringComparison.Ordinal);
            Assert.True(start >= 0 && end > start, "ApplyNeedChip/MakeNeedChip not found");
            string body = hud.Substring(start, end - start);
            int assignments = 0, idx = 0;
            while ((idx = body.IndexOf("label.TooltipText =", idx, StringComparison.Ordinal)) >= 0) { assignments++; idx += 1; }
            Assert.Equal(1, assignments);
        }

        [Fact]
        public void ResourceToastNouns_AreLocalized()
        {
            // Task 5 — every feedback toast noun the host passes resolves.
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[] { "ui.resource.food", "ui.resource.water" })
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            string gameFlow = Read("src/Main.GameFlow.cs");
            Assert.Contains("AshfallUiText.Tr(\"ui.resource.food\"", gameFlow, StringComparison.Ordinal);
            Assert.Contains("AshfallUiText.Tr(\"ui.resource.water\"", gameFlow, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorsPanel_DayDeltaUnit_IsGuardedAgainstNonPositiveSpan()
        {
            // Task 7 — a zero/negative span must not render "/0d" or "/-1d".
            string panel = Read("src/UI/SurvivorsPanel.cs");
            Assert.Contains("int span = _day > 0 ? _survivorsHost.NeedDaySpan(_day) : 1;", panel, StringComparison.Ordinal);
            Assert.Contains("string unit = span > 1 ?", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void RadiationWarnToast_IsDedupedAndReArmedBelowBand()
        {
            // Task 8 — the host alert must dedupe per survivor and re-arm.
            string host = Read("src/Host/SurvivorsHostSession.cs");
            Assert.Contains("if (dose < RadiationSystem.WarnThreshold)", host, StringComparison.Ordinal);
            Assert.Contains("_radWarnNotified.Remove(radState.Id);", host, StringComparison.Ordinal);
            Assert.Contains("if (!_radWarnNotified.Add(radState.Id)) return;", host, StringComparison.Ordinal);
            Assert.Contains("dedupeKey: $\"radiation_warn_{radState.Id}\"", host, StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_MoraleDrift_IsPositiveIsGood()
        {
            // Task 9 — rising morale is good, unlike hunger/thirst/fatigue.
            string status = Read("src/UI/StatusPanel.cs");
            Assert.Contains("moraleDrift / driftSamples, suffix, positiveIsGood: true", status, StringComparison.Ordinal);
        }

        [Fact]
        public void PlayerPath_DoesNotEmitRadiationHighToast()
        {
            // Task 11 — the host owns the radiation_high toast; the player path
            // must not re-emit it.
            string gameFlow = Read("src/Main.GameFlow.cs");
            Assert.DoesNotContain("\"radiation_high\"", gameFlow, StringComparison.Ordinal);
            Assert.DoesNotContain("radiation_high_rate", gameFlow, StringComparison.Ordinal);
        }

        [Fact]
        public void NewLocalizedRows_HaveNonEmptyGerman()
        {
            // Task 12 — the new rows must carry a German string, not a copy/blank.
            string[] keys =
            {
                "ui.status.title", "ui.status.section.day_environment", "ui.status.section.directives",
                "ui.status.section.cohort", "ui.status.section.shelter", "ui.status.section.thermal",
                "ui.status.section.forecast", "ui.status.day.current_day", "ui.status.day.survivors",
                "ui.status.day.external_conditions", "ui.status.day.power_reserve", "ui.status.stores.water",
                "ui.status.stores.food", "ui.status.system.power_grid", "ui.status.system.radiation_shielding",
                "ui.status.system.outdoor_radiation", "ui.survivors.shell.title",
            };
            var lines = Read("assets/l10n/strings.csv").Split('\n');
            foreach (string key in keys)
            {
                string line = lines.FirstOrDefault(l => l.StartsWith(key + ",", StringComparison.Ordinal))!;
                Assert.True(line != null, $"missing key {key}");
                var cols = line!.Split(',');
                Assert.True(cols.Length >= 3 && !string.IsNullOrWhiteSpace(cols[2]), $"empty German for {key}");
            }
        }

        [Fact]
        public void GameHudSnapshotFixture_PinsWarmthChipState()
        {
            // Task 13 — the fixture must project a warmth value for the WARM chip.
            string fixture = Read("src/UI/GameHudSnapshotFixture.cs");
            Assert.Contains("UpdateNeeds(78, 62, 55, 44, 71", fixture, StringComparison.Ordinal);
            string hud = Read("src/UI/GameHudOverlay.cs");
            Assert.Contains("_lblWarmth", hud, StringComparison.Ordinal);
        }

        [Fact]
        public void StringsCsv_HasNoDuplicateKeys()
        {
            // Task 14 — duplicate keys are silently first-wins today.
            var seen = new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal);
            var duplicates = new System.Collections.Generic.List<string>();
            var lines = Read("assets/l10n/strings.csv").Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (line.Length == 0) continue;
                string key = line.Split(',')[0];
                if (!seen.Add(key)) duplicates.Add(key);
            }
            Assert.True(duplicates.Count == 0, "duplicate strings.csv keys: " + string.Join(", ", duplicates));
        }

        [Fact]
        public void StatusPanel_CohortSection_IsBoundedInsideTheScrollContent()
        {
            // Task 15 — bounded layout gate: the cohort section must stay inside
            // the fixed 680x560 panel's 640x440 scroll content (no overflow).
            string status = Read("src/UI/StatusPanel.cs");
            Assert.Contains("MakeSectionHeader(Tr(\"ui.status.section.cohort\"", status, StringComparison.Ordinal);
            Assert.Contains("_statsData = AshfallUiHelpers.MakeVBox", status, StringComparison.Ordinal);
            Assert.Contains("contentBox.AddChild(_statsData)", status, StringComparison.Ordinal);
            Assert.Contains("MakePanel(680, 560)", status, StringComparison.Ordinal);
            Assert.Contains("new Vector2(640, 440)", status, StringComparison.Ordinal);
        }

        // ── Fourth-wave task gates ─────────────────────────────────────────

        private static string[] SplitCsvLine(string line)
        {
            // Quote-aware split: fields may contain commas (and escaped quotes).
            var fields = new System.Collections.Generic.List<string>();
            var current = new System.Text.StringBuilder();
            bool quoted = false;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    if (quoted && i + 1 < line.Length && line[i + 1] == '"') { current.Append('"'); i++; }
                    else quoted = !quoted;
                }
                else if (c == ',' && !quoted) { fields.Add(current.ToString()); current.Clear(); }
                else current.Append(c);
            }
            fields.Add(current.ToString());
            return fields.ToArray();
        }

        [Fact]
        public void SurvivorsPanel_MetadataAndRailCaptions_AreLocalized()
        {
            // Tasks 1 & 4 — roster metadata + rail captions resolve from strings.csv.
            string panel = Read("src/UI/SurvivorsPanel.cs");
            foreach (string key in new[]
            {
                "ui.survivors.empty.no_session", "ui.survivors.empty.roster",
                "ui.survivors.empty.no_match", "ui.survivors.event.latest",
                "ui.survivors.rail.living", "ui.survivors.rail.avg_hp",
                "ui.survivors.rail.avg_rad", "ui.survivors.rail.avg_mor",
                "ui.survivors.rail.strained",
            })
                Assert.Contains(key, panel, StringComparison.Ordinal);
            Assert.DoesNotContain("MakeMetadata(\"No survivor session bound.\"", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("AddCard(\"living\",   \"LIVING\"", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorDetailPanel_NeedsAndTraits_AreLocalized()
        {
            // Task 2 — needs/trait labels resolve from strings.csv.
            string panel = Read("src/UI/SurvivorDetailPanel.cs");
            foreach (string key in new[]
            {
                "ui.survivor.need.health", "ui.survivor.need.hunger", "ui.survivor.need.thirst",
                "ui.survivor.need.fatigue", "ui.survivor.need.warmth", "ui.survivor.need.morale",
                "ui.survivor.need.hygiene", "ui.survivor.trait.radiation_dose",
                "ui.survivor.trait.lifetime_exposure", "ui.survivor.trait.rad_resistance",
                "ui.survivor.trait.acute_sickness", "ui.survivor.trait.chronic_illness",
            })
                Assert.Contains(key, panel, StringComparison.Ordinal);
            Assert.Contains("AshfallUiText.Tr", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_Keys_ResolveFromStringsCsv()
        {
            // Task 3 — the status key family is present in the catalog.
            string csv = Read("assets/l10n/strings.csv");
            string[] keys =
            {
                "ui.status.title", "ui.status.section.day_environment", "ui.status.section.directives",
                "ui.status.section.cohort", "ui.status.section.shelter", "ui.status.section.thermal",
                "ui.status.section.forecast", "ui.status.day.current_day", "ui.status.day.survivors",
                "ui.status.day.external_conditions", "ui.status.day.power_reserve", "ui.status.stores.water",
                "ui.status.stores.food", "ui.status.system.power_grid", "ui.status.system.radiation_shielding",
                "ui.status.system.outdoor_radiation", "ui.status.thermal.roster_warmth",
                "ui.status.thermal.clothing_warmth", "ui.status.thermal.shelter_thermal",
                "ui.status.thermal.waterway", "ui.status.forecast.supply_projection",
                "ui.status.forecast.shortfall", "ui.status.not_monitored", "ui.status.no_roster",
            };
            foreach (string key in keys)
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
        }

        [Fact]
        public void StringsCsv_SourceColumn_PointsAtExistingPaths()
        {
            // Task 5 — a stale source column is silent docs rot; every non-empty
            // source must name a real repository path.
            string root = RepoRoot();
            var lines = Read("assets/l10n/strings.csv").Split('\n');
            var missing = new System.Collections.Generic.List<string>();
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (line.Length == 0) continue;
                var cols = SplitCsvLine(line);
                if (cols.Length < 4) continue;
                string source = cols[3].Trim();
                if (source.Length == 0 || source == "unknown") continue;
                if (!File.Exists(Path.Combine(root, source.Replace('/', Path.DirectorySeparatorChar))))
                    missing.Add(cols[0] + " -> " + source);
            }
            Assert.True(missing.Count == 0, "stale source column: " + string.Join("; ", missing));
        }

        [Fact]
        public void StatusPanel_RosterWarmth_HasWarnBand()
        {
            // Task 6 / Loop-3 repair — the warmth readout resolves through the
            // shared AshfallUiBands helper with the authored profile bands.
            string status = Read("src/UI/StatusPanel.cs");
            Assert.Contains("profile.warmthWarn", status, StringComparison.Ordinal);
            Assert.Contains("AshfallUiBands.ForLow(minWarmth", status, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorDetailPanel_Needs_ReadProfilePredicates()
        {
            // Task 8 — no re-typed 90/20 literals; the profile owns the bands.
            string panel = Read("src/UI/SurvivorDetailPanel.cs");
            Assert.Contains("profile.IsMoraleCritical(s.Morale)", panel, StringComparison.Ordinal);
            Assert.Contains("profile.IsHungerCritical(s.Hunger)", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("Morale < 20", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void NeedCriticalDedupe_FiresOncePerRisingEdge()
        {
            // Task 9 — the Core rising-edge guard re-arms below the band, and the
            // host key is per survivor+kind.
            string needs = Read("Assets/Ashfall.Core/Survivors/NeedsSystem.cs");
            Assert.Contains("if (isCritical && !wasCritical)", needs, StringComparison.Ordinal);
            string host = Read("src/Host/SurvivorsHostSession.cs");
            Assert.Contains("need_critical_", host, StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_NotMonitored_IsLocalized()
        {
            // Task 10 — the shared NOT MONITORED marker resolves from the catalog.
            string status = Read("src/UI/StatusPanel.cs");
            Assert.Contains("ui.status.not_monitored", status, StringComparison.Ordinal);
            Assert.Contains("ui.status.no_roster", status, StringComparison.Ordinal);
            Assert.DoesNotContain("MakeDataRow(\"Roster Warmth\", \"NOT MONITORED\"", status, StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_ForecastKeys_ArePresent()
        {
            // Task 11 — forecast labels are catalogued.
            string csv = Read("assets/l10n/strings.csv");
            Assert.Contains("ui.status.forecast.supply_projection,", csv, StringComparison.Ordinal);
            Assert.Contains("ui.status.forecast.shortfall,", csv, StringComparison.Ordinal);
        }

        [Fact]
        public void AshfallUiText_StaysPresentationOnly()
        {
            // Task 12 — the helper must not pull in simulation or Godot value types.
            string helper = Read("src/UI/AshfallUiText.cs");
            Assert.DoesNotContain("using Godot;", helper, StringComparison.Ordinal);
            Assert.DoesNotContain("using Ashfall.Core.Survivors", helper, StringComparison.Ordinal);
            Assert.DoesNotContain("Ashfall.Core", helper, StringComparison.Ordinal);
        }

        [Fact]
        public void GameHudFixture_LivesInUiAndIsRegistered()
        {
            // Task 13 — a moved/renamed fixture must fail loudly.
            Assert.True(File.Exists(Path.Combine(RepoRoot(), "src", "UI", "GameHudSnapshotFixture.cs")));
            Assert.Contains("GameHudSnapshotFixture.Bind", Read("src/UI/SnapshotHarness.cs"), StringComparison.Ordinal);
        }

        [Fact]
        public void UiSources_HaveNoConsecutiveDuplicateTooltipAssignment()
        {
            // Task 14 — a second assignment on the same receiver within a few
            // lines silently discards the first (the bug fixed in ApplyNeedChip).
            string uiDir = Path.Combine(RepoRoot(), "src", "UI");
            var offenders = new System.Collections.Generic.List<string>();
            foreach (string file in Directory.GetFiles(uiDir, "*.cs", SearchOption.AllDirectories))
            {
                string[] lines = File.ReadAllLines(file);
                string? lastReceiver = null;
                int lastLine = -10;
                for (int i = 0; i < lines.Length; i++)
                {
                    var m = System.Text.RegularExpressions.Regex.Match(lines[i], @"([A-Za-z_][A-Za-z0-9_]*)\.TooltipText\s*=");
                    if (!m.Success) continue;
                    string receiver = m.Groups[1].Value;
                    if (receiver == lastReceiver && i - lastLine <= 4)
                        offenders.Add(Path.GetFileName(file) + ":" + (i + 1) + " (" + receiver + ")");
                    lastReceiver = receiver;
                    lastLine = i;
                }
            }
            Assert.True(offenders.Count == 0, "duplicate tooltip assignments: " + string.Join(", ", offenders));
        }

        [Fact]
        public void UiLayoutCheckScript_ExistsAndIsBounded()
        {
            // Task 15 — the layout check has a single bounded entry point.
            string script = Read("scripts/ci/ui-layout-check.sh");
            Assert.Contains("run-godot-bounded.sh", script, StringComparison.Ordinal);
            Assert.Contains("--ui-layout-selftest", script, StringComparison.Ordinal);
        }

        // ── Fifth-wave loop gates ─────────────────────────────────────────

        [Fact]
        public void StatusPanel_DayInfoValues_AreLocalized()
        {
            // Loop-2 hardening — the day/environment values resolve from the catalog.
            string status = Read("src/UI/StatusPanel.cs");
            foreach (string key in new[]
            {
                "ui.status.day.value", "ui.status.day.alive", "ui.status.day.hazard",
                "ui.status.day.nominal", "ui.status.day.battery",
            })
                Assert.Contains(key, status, StringComparison.Ordinal);
            Assert.DoesNotContain("$\"Day {_simDay}\"", status, StringComparison.Ordinal);
            Assert.DoesNotContain("\" — HAZARD WATCH\"", status, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorDetailPanel_IdentityRows_AreLocalized()
        {
            // Loop-3 hardening — identity rows resolve from the catalog.
            string panel = Read("src/UI/SurvivorDetailPanel.cs");
            foreach (string key in new[]
            {
                "ui.survivor.info.name", "ui.survivor.info.profession",
                "ui.survivor.info.unspecified", "ui.survivor.info.alive",
                "ui.survivor.info.max_health", "ui.survivor.info.tenure",
            })
                Assert.Contains(key, panel, StringComparison.Ordinal);
            Assert.DoesNotContain("$\"Name: ", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorsPanel_StatusLabels_AreLocalized()
        {
            // Loop-4 hardening — the internal status code stays stable for
            // FilterPass, but the displayed label is localized.
            string panel = Read("src/UI/SurvivorsPanel.cs");
            foreach (string key in new[]
            {
                "ui.survivors.status.stable", "ui.survivors.status.strained",
                "ui.survivors.status.critical", "ui.survivors.status.dead",
            })
                Assert.Contains(key, panel, StringComparison.Ordinal);
            Assert.Contains("StatusLabel(status)", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorsPanel_CohortSummary_IsLocalized()
        {
            // Loop-5 hardening — the cohort summary sentence resolves from the catalog.
            string panel = Read("src/UI/SurvivorsPanel.cs");
            Assert.Contains("ui.survivors.cohort.summary", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("Cohort morale reads ", panel, StringComparison.Ordinal);
        }

        // ── Sixth-wave task gates ─────────────────────────────────────────

        [Fact]
        public void SurvivorsPanel_FilterHintsAndSidebar_AreLocalized()
        {
            // Tasks 1 & 11 — hints and the sidebar title resolve from the catalog.
            string panel = Read("src/UI/SurvivorsPanel.cs");
            foreach (string key in new[]
            {
                "ui.survivors.filter.all.hint", "ui.survivors.filter.living.hint",
                "ui.survivors.filter.strained.hint", "ui.survivors.filter.critical.hint",
                "ui.survivors.sidebar.title",
            })
                Assert.Contains(key, panel, StringComparison.Ordinal);
            Assert.DoesNotContain("Hint = \"every survivor\"", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorDetailPanel_StatusRows_AreLocalized()
        {
            // Task 2 — origin/fitness/critical-flags rows resolve from the catalog.
            string panel = Read("src/UI/SurvivorDetailPanel.cs");
            foreach (string key in new[]
            {
                "ui.survivor.trait.origin", "ui.survivor.status.critical_flags",
                "ui.survivor.status.fitness", "ui.survivor.status.factors",
                "ui.survivor.fitness.fit", "ui.survivor.fitness.impaired", "ui.survivor.fitness.unfit",
            })
                Assert.Contains(key, panel, StringComparison.Ordinal);
            Assert.DoesNotContain("$\"Origin: ", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_Objectives_AreLocalized()
        {
            // Task 3 — objective strings resolve from the catalog.
            string status = Read("src/UI/StatusPanel.cs");
            foreach (string key in new[]
            {
                "ui.status.objective.critical_health", "ui.status.objective.dosed",
                "ui.status.objective.acute_rad", "ui.status.objective.brownout",
                "ui.status.objective.hazard_weather", "ui.status.objective.water_critical",
                "ui.status.objective.standing",
            })
                Assert.Contains(key, status, StringComparison.Ordinal);
            Assert.DoesNotContain("triage immediately\"", status, StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_SystemValues_AreLocalized()
        {
            // Task 4 — system-status values resolve from the catalog.
            string status = Read("src/UI/StatusPanel.cs");
            foreach (string key in new[]
            {
                "ui.status.system.power_brownout", "ui.status.system.power_online",
                "ui.status.system.shielding_value", "ui.status.system.outdoor_value",
                "ui.status.system.outdoor_none",
            })
                Assert.Contains(key, status, StringComparison.Ordinal);
            Assert.DoesNotContain("BROWNOUT · Gen ", status, StringComparison.Ordinal);
        }

        [Fact]
        public void UiSurvivorKeys_ResolveFromStringsCsv()
        {
            // Task 5 — the ui.survivor.* family is catalogued.
            string csv = Read("assets/l10n/strings.csv");
            string[] keys =
            {
                "ui.survivor.need.health", "ui.survivor.need.hunger", "ui.survivor.need.thirst",
                "ui.survivor.need.fatigue", "ui.survivor.need.warmth", "ui.survivor.need.morale",
                "ui.survivor.need.hygiene", "ui.survivor.trait.radiation_dose",
                "ui.survivor.trait.lifetime_exposure", "ui.survivor.trait.rad_resistance",
                "ui.survivor.trait.acute_sickness", "ui.survivor.trait.chronic_illness",
                "ui.survivor.trait.origin", "ui.survivor.status.critical_flags",
                "ui.survivor.status.fitness", "ui.survivor.status.factors",
                "ui.survivor.info.name", "ui.survivor.info.profession", "ui.survivor.info.unspecified",
                "ui.survivor.info.alive", "ui.survivor.info.max_health", "ui.survivor.info.tenure",
            };
            foreach (string key in keys)
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
        }

        [Fact]
        public void UiSurvivorsKeys_ResolveFromStringsCsv()
        {
            // Task 6 — the ui.survivors.* family is catalogued.
            string csv = Read("assets/l10n/strings.csv");
            string[] keys =
            {
                "ui.survivors.filter.all", "ui.survivors.filter.living",
                "ui.survivors.filter.strained", "ui.survivors.filter.critical",
                "ui.survivors.filter.all.hint", "ui.survivors.filter.living.hint",
                "ui.survivors.filter.strained.hint", "ui.survivors.filter.critical.hint",
                "ui.survivors.header.roster", "ui.survivors.header.telemetry",
                "ui.survivors.shell.title", "ui.survivors.sidebar.title",
                "ui.survivors.empty.no_session", "ui.survivors.empty.roster", "ui.survivors.empty.no_match",
                "ui.survivors.event.latest", "ui.survivors.rail.living", "ui.survivors.rail.avg_hp",
                "ui.survivors.rail.avg_rad", "ui.survivors.rail.avg_mor", "ui.survivors.rail.strained",
                "ui.survivors.status.stable", "ui.survivors.status.strained",
                "ui.survivors.status.critical", "ui.survivors.status.dead",
                "ui.survivors.cohort.summary",
            };
            foreach (string key in keys)
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
        }

        [Fact]
        public void UiSources_UseProfilePredicates_NotRawCriticals()
        {
            // Task 7 — high-is-bad comparisons go through the profile predicates.
            foreach (string file in new[]
            {
                "src/UI/StatusPanel.cs", "src/UI/SurvivorsPanel.cs", "src/UI/SurvivorDetailPanel.cs",
            })
            {
                string src = Read(file);
                Assert.DoesNotContain(">= profile.hungerCritical", src, StringComparison.Ordinal);
                Assert.DoesNotContain(">= profile.thirstCritical", src, StringComparison.Ordinal);
                Assert.DoesNotContain(">= profile.fatigueCritical", src, StringComparison.Ordinal);
            }
        }

        [Fact]
        public void StringsCsv_IdenticalEnglishGerman_StaysBounded()
        {
            // Task 8 — legitimate shared terms exist, but the count must not grow
            // unnoticed (a copied English row hiding a missing translation).
            int identical = 0;
            var lines = Read("assets/l10n/strings.csv").Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (line.Length == 0) continue;
                var cols = SplitCsvLine(line);
                if (cols.Length >= 3 && cols[1] == cols[2]) identical++;
            }
            Assert.True(identical <= 20, $"identical en/de rows={identical} (pin 20)");
        }

        [Fact]
        public void SurvivorDetailPanel_DoesNotImportCatalogNamespace()
        {
            // Task 9 — the panel localizes only through AshfallUiText.
            string panel = Read("src/UI/SurvivorDetailPanel.cs");
            Assert.DoesNotContain("using AtomicWar.GodotApp.Localization;", panel, StringComparison.Ordinal);
            Assert.Contains("AshfallUiText", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void UiLayoutCheckScript_ParsesFailures()
        {
            // Task 10 — the wrapper fails on a non-zero failure count even if the
            // runner exits 0.
            string script = Read("scripts/ci/ui-layout-check.sh");
            Assert.Contains("Failures: [0-9]+", script, StringComparison.Ordinal);
            Assert.Contains("exit 1", script, StringComparison.Ordinal);
        }

        [Fact]
        public void HudNeedChipLabels_ResolveFromStringsCsv()
        {
            // Task 12 — the five HUD need-chip labels are catalogued and used.
            string csv = Read("assets/l10n/strings.csv");
            string hud = Read("src/UI/GameHudOverlay.cs");
            foreach (string key in new[]
            {
                "ui.hud.needs.hunger", "ui.hud.needs.thirst", "ui.hud.needs.fatigue",
                "ui.hud.needs.morale", "ui.hud.needs.warmth",
            })
            {
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
                Assert.Contains(key, hud, StringComparison.Ordinal);
            }
        }

        [Fact]
        public void StringsCsv_KeyPrefixCollisions_StayBounded()
        {
            // Task 13 — a key that is a dot-prefix of another is legal but must be
            // intentional (lookup order can surprise).
            var keys = new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal);
            var lines = Read("assets/l10n/strings.csv").Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (line.Length == 0) continue;
                keys.Add(SplitCsvLine(line)[0]);
            }
            int collisions = 0;
            foreach (string key in keys)
            {
                foreach (string other in keys)
                {
                    if (key != other && other.StartsWith(key + ".", StringComparison.Ordinal))
                    {
                        collisions++;
                        break;
                    }
                }
            }
            Assert.True(collisions <= 10, "key prefix collisions=" + collisions + " (pin 10)");
        }

        [Fact]
        public void CiReadme_DocumentsLayoutCheck()
        {
            // Task 14 — the bounded layout wrapper is documented.
            string readme = Read("scripts/ci/README.md");
            Assert.Contains("ui-layout-check.sh", readme, StringComparison.Ordinal);
            Assert.Contains("180", readme, StringComparison.Ordinal);
        }

        [Fact]
        public void StringsCsv_LongLines_AreAllowlisted()
        {
            // Task 15 — long prose rows are allowed only for known body suffixes.
            string[] allowed = { ".body", ".lesson", ".summary", ".intro", ".desc", ".prose", ".text", ".brief" };
            var offenders = new System.Collections.Generic.List<string>();
            var lines = Read("assets/l10n/strings.csv").Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (line.Length == 0 || line.Length <= 400) continue;
                string key = SplitCsvLine(line)[0];
                if (!allowed.Any(suffix => key.Contains(suffix, StringComparison.Ordinal)))
                    offenders.Add(key + " (" + line.Length + " chars)");
            }
            Assert.True(offenders.Count == 0, "long non-prose rows: " + string.Join(", ", offenders));
        }

        // ── Seventh-wave loop gates ───────────────────────────────────────

        [Fact]
        public void SurvivorDetailPanel_Factors_KeyHasNoTrailingSpace()
        {
            // Loop-2 hardening — a trailing space in the CSV field is fragile; the
            // separator is added in code instead.
            string panel = Read("src/UI/SurvivorDetailPanel.cs");
            Assert.Contains("Tr(\"ui.survivor.status.factors\", \"Factors:\") + \" \"", panel, StringComparison.Ordinal);
            Assert.Contains("ui.survivor.status.factors,Factors:,Faktoren:", Read("assets/l10n/strings.csv"), StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_ObjectiveTags_AreLocalized()
        {
            // Loop-3 hardening — objective tags resolve from the catalog.
            string status = Read("src/UI/StatusPanel.cs");
            foreach (string key in new[]
            {
                "ui.status.objective.tag.primary", "ui.status.objective.tag.daily",
                "ui.status.objective.tag.secondary", "ui.status.objective.tag.standing",
            })
                Assert.Contains(key, status, StringComparison.Ordinal);
            Assert.Contains("ObjectiveTag(obj.type)", status, StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_ForecastValues_AreLocalized()
        {
            // Loop-4 hardening — forecast values resolve from the catalog.
            // Loop-3 (tenth wave) — `ui.status.forecast.short` is dead (superseded
            // by `day_value_short`); the old assertion passed only because the
            // dead key is a substring of `ui.status.forecast.shortfall`.
            string status = Read("src/UI/StatusPanel.cs");
            foreach (string key in new[]
            {
                "ui.status.forecast.day_value", "ui.status.forecast.day_value_short",
                "ui.status.forecast.shortfall_value",
            })
                Assert.Contains(key, status, StringComparison.Ordinal);
            Assert.DoesNotContain("$\"food {row.FoodAfter}", status, StringComparison.Ordinal);
            Assert.DoesNotContain("ui.status.forecast.short,", Read("assets/l10n/strings.csv"), StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_ThermalValues_AreLocalized()
        {
            // Loop-5 hardening — thermal values resolve from the catalog.
            string status = Read("src/UI/StatusPanel.cs");
            foreach (string key in new[]
            {
                "ui.status.thermal.roster_value", "ui.status.thermal.clothing_value",
                "ui.status.thermal.shelter_value", "ui.status.thermal.boiler_on",
                "ui.status.thermal.boiler_off",
            })
                Assert.Contains(key, status, StringComparison.Ordinal);
            Assert.DoesNotContain("cold loss (worn layers)", status, StringComparison.Ordinal);
        }

        // ── Seventh-wave task gates ───────────────────────────────────────

        [Fact]
        public void StatusPanel_ExpeditionInjuryValue_IsLocalized()
        {
            // Task 1 — the injury value resolves from the catalog, not a raw label.
            string status = Read("src/UI/StatusPanel.cs");
            Assert.Contains("ui.status.expedition_injury.value", status, StringComparison.Ordinal);
            Assert.DoesNotContain("$\"{name} — Day {latest.EventDay}", status, StringComparison.Ordinal);
            Assert.Contains("ui.status.expedition_injury.value,", Read("assets/l10n/strings.csv"), StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_DayWeatherValue_IsLocalized()
        {
            // Task 2 — the weather condition wrapper resolves from the catalog.
            string status = Read("src/UI/StatusPanel.cs");
            Assert.Contains("ui.status.day.weather_value", status, StringComparison.Ordinal);
            Assert.DoesNotContain("$\"{kind}{(hazard ?", status, StringComparison.Ordinal);
            Assert.Contains("ui.status.day.weather_value,", Read("assets/l10n/strings.csv"), StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_ObjectiveTagKeys_ResolveFromStringsCsv()
        {
            // Task 3 — the four objective tags are catalogued and consumed.
            string csv = Read("assets/l10n/strings.csv");
            string status = Read("src/UI/StatusPanel.cs");
            foreach (string key in new[]
            {
                "ui.status.objective.tag.primary", "ui.status.objective.tag.daily",
                "ui.status.objective.tag.secondary", "ui.status.objective.tag.standing",
            })
            {
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
                Assert.Contains(key, status, StringComparison.Ordinal);
            }
        }

        [Fact]
        public void StatusPanel_ThermalKeys_ResolveFromStringsCsv()
        {
            // Task 4 — the full thermal family is catalogued.
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.status.thermal.roster_warmth", "ui.status.thermal.clothing_warmth",
                "ui.status.thermal.shelter_thermal", "ui.status.thermal.waterway",
                "ui.status.thermal.roster_value", "ui.status.thermal.clothing_value",
                "ui.status.thermal.shelter_value", "ui.status.thermal.boiler_on",
                "ui.status.thermal.boiler_off",
            })
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorDetailPanel_BodyClothingSpecialization_AreLocalized()
        {
            // Task 5 — the role/clothing/body rows resolve from the catalog.
            string panel = Read("src/UI/SurvivorDetailPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.survivor.info.specialization", "ui.survivor.info.clothing", "ui.survivor.info.body",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.DoesNotContain("$\"Specialization: ", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("$\"Clothing: ", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("$\"Body: ", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorDetailPanel_NeedsAndTraits_RowsRouteThroughTr()
        {
            // Task 6 — every needs/traits row label comes from a catalog key.
            string panel = Read("src/UI/SurvivorDetailPanel.cs");
            int needsStart = panel.IndexOf("// ── Needs ──", StringComparison.Ordinal);
            int statusStart = panel.IndexOf("// ── Status ──", StringComparison.Ordinal);
            Assert.True(needsStart >= 0 && statusStart > needsStart, "needs/status sections not found");
            string region = panel.Substring(needsStart, statusStart - needsStart);
            var offenders = new System.Collections.Generic.List<string>();
            foreach (string line in region.Split('\n'))
            {
                if (!line.Contains("AddRow(_needsList,", StringComparison.Ordinal)
                    && !line.Contains("AddRow(_traitsList,", StringComparison.Ordinal))
                    continue;
                if (!line.Contains("Tr(", StringComparison.Ordinal)
                    && !line.Contains("TrFormat(", StringComparison.Ordinal))
                    offenders.Add(line.Trim());
            }
            Assert.True(offenders.Count == 0, "needs/traits rows without a catalog key: " + string.Join(" | ", offenders));
        }

        [Fact]
        public void SurvivorsPanel_FilterHintKeys_ResolveFromStringsCsv()
        {
            // Task 7 — the four filter hints are catalogued and consumed.
            string csv = Read("assets/l10n/strings.csv");
            string panel = Read("src/UI/SurvivorsPanel.cs");
            foreach (string key in new[]
            {
                "ui.survivors.filter.all.hint", "ui.survivors.filter.living.hint",
                "ui.survivors.filter.strained.hint", "ui.survivors.filter.critical.hint",
            })
            {
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
                Assert.Contains(key, panel, StringComparison.Ordinal);
            }
        }

        [Fact]
        public void SurvivorsPanel_EventLine_IsLocalized()
        {
            // Task 8 — the event line + truthful empty variant resolve from the catalog.
            string panel = Read("src/UI/SurvivorsPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            Assert.Contains("ui.survivors.event.line", panel, StringComparison.Ordinal);
            Assert.Contains("ui.survivors.event.none", panel, StringComparison.Ordinal);
            Assert.Contains("ui.survivors.event.line,", csv, StringComparison.Ordinal);
            Assert.Contains("ui.survivors.event.none,", csv, StringComparison.Ordinal);
            Assert.DoesNotContain("$\"{Tr(\"ui.survivors.event.latest\"", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void StringsCsv_HasNoEdgeWhitespace()
        {
            // Task 9 — a trailing/leading space in a field is a fragile separator;
            // the separator belongs in code (sixth-wave loop-2 precedent).
            var offenders = new System.Collections.Generic.List<string>();
            var lines = Read("assets/l10n/strings.csv").Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].TrimEnd('\r');
                if (line.Length == 0) continue;
                var cols = SplitCsvLine(line);
                for (int c = 0; c < cols.Length; c++)
                {
                    if (cols[c] != cols[c].Trim())
                        offenders.Add($"line {i + 1} col {c}: {cols[c]}");
                }
            }
            Assert.True(offenders.Count == 0, "edge whitespace: " + string.Join("; ", offenders));
        }

        [Fact]
        public void UiStringsCsv_HasNoEmptyGerman_AndOnlyAllowlistedIdenticalRows()
        {
            // Task 10 — a ui.* row with empty German or a copied English row hides
            // a missing translation. The allowlist is the reviewed set of terms
            // that are identical by design (codes, units, proper names).
            var allowlisted = new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal)
            {
                "ui.common.ok", "ui.hud.needs.hunger", "ui.hud.needs.morale",
                "ui.survivor.need.hunger", "ui.survivor.need.hygiene",
                "ui.survivor.info.name", "ui.survivor.status.fitness",
                "ui.survivor.fitness.fit", "ui.hud.radiation", "ui.hud.label.rad",
                "ui.status.day.weather_value", "ui.survivors.event.line",
                "ui.survivor.status.modifier_row", "ui.survivor.status.recent_row",
                "ui.shelter_hud.condition.hunger",
            };
            var errors = new System.Collections.Generic.List<string>();
            var lines = Read("assets/l10n/strings.csv").Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].TrimEnd('\r');
                if (line.Length == 0) continue;
                var cols = SplitCsvLine(line);
                if (cols.Length < 3 || !cols[0].StartsWith("ui.", StringComparison.Ordinal)) continue;
                if (string.IsNullOrWhiteSpace(cols[2])) errors.Add(cols[0] + ": empty German");
                else if (cols[1] == cols[2] && !allowlisted.Contains(cols[0]))
                    errors.Add(cols[0] + ": German copies English and is not allowlisted");
            }
            Assert.True(errors.Count == 0, string.Join("; ", errors));
        }

        [Fact]
        public void ObjectiveTag_MapsEveryCode_AndFallsBackToStanding()
        {
            // Task 11 — all four codes are mapped; an unknown code must fall back
            // to the standing tag, never render the raw code.
            string status = Read("src/UI/StatusPanel.cs");
            foreach (string code in new[] { "\"PRIMARY\"", "\"DAILY\"", "\"SECONDARY\"" })
                Assert.Contains(code, status, StringComparison.Ordinal);
            Assert.Contains("_ => Tr(\"ui.status.objective.tag.standing\"", status, StringComparison.Ordinal);
        }

        [Fact]
        public void NeedsProfile_ExposesHealthCriticalPredicate()
        {
            // Task 12 — health joins the symmetric predicate family so every panel
            // classifies a critical survivor identically.
            var p = new NeedsProfile();
            Assert.True(p.IsHealthCritical(p.healthCritical));
            Assert.False(p.IsHealthCritical(p.healthCritical + 1f));
            Assert.True(p.healthCritical < p.healthWarn);
        }

        [Fact]
        public void Panels_UseHealthCriticalPredicate_NotRawComparison()
        {
            // Task 12 (hardening) — no survival panel re-compares healthCritical.
            foreach (string file in new[]
            {
                "src/UI/StatusPanel.cs", "src/UI/SurvivorsPanel.cs", "src/UI/SurvivorDetailPanel.cs",
            })
            {
                string src = Read(file);
                Assert.DoesNotContain("< profile.healthCritical", src, StringComparison.Ordinal);
            }
            Assert.Contains("profile.IsHealthCritical", Read("src/UI/SurvivorsPanel.cs"), StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_AddStatRowLabels_ResolveFromStringsCsv()
        {
            // Task 13 — every AddStatRow label key resolves.
            string csv = Read("assets/l10n/strings.csv");
            string status = Read("src/UI/StatusPanel.cs");
            foreach (string key in new[]
            {
                "ui.status.cohort.survivors", "ui.status.cohort.avg_health",
                "ui.status.cohort.morale", "ui.status.cohort.dose",
                "ui.status.drift.header", "ui.status.drift.hunger",
                "ui.status.drift.thirst", "ui.status.drift.fatigue",
                "ui.status.drift.morale", "ui.status.drift.warmth",
                "ui.status.stores.water", "ui.status.stores.food",
                "ui.status.expedition_injury",
            })
            {
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
                Assert.Contains(key, status, StringComparison.Ordinal);
            }
        }

        [Fact]
        public void CiReadme_DocumentsL10nGate()
        {
            // Task 14 — the drift gate and its German-parity contract are documented.
            string readme = Read("scripts/ci/README.md");
            Assert.Contains("l10n_drift_gate.py", readme, StringComparison.Ordinal);
            Assert.Contains("German", readme, StringComparison.Ordinal);
        }

        [Fact]
        public void UiLayoutCheckScript_VerifiesArtifact()
        {
            // Task 15 — the wrapper verifies the machine-readable artifact, not
            // just stdout, so a stale/absent verdict fails.
            string script = Read("scripts/ci/ui-layout-check.sh");
            Assert.Contains("ui-layout-selftest.json", script, StringComparison.Ordinal);
            Assert.Contains("\"status\":\"PASS\"", script, StringComparison.Ordinal);
            string host = Read("src/Host/HostCli.Command.RunUiLayoutSelfTest.cs");
            Assert.Contains("ui-layout-selftest.json", host, StringComparison.Ordinal);
        }

        // ── Seventh-wave gates: shared band seam (tasks 7 & 9) ────────────

        [Fact]
        public void AshfallUiBands_IsTheSingleBandAuthority()
        {
            // Loop-1 — the three-band rule was inlined in the HUD while two
            // panels re-derived weaker two-band versions. The helper must exist
            // and every band call site must route through it.
            string bands = Read("src/UI/AshfallUiBands.cs");
            Assert.Contains("public static class AshfallUiBands", bands, StringComparison.Ordinal);
            Assert.Contains("public static (float r, float g, float b, float a) ForNeed(", bands, StringComparison.Ordinal);
            Assert.Contains("public static (float r, float g, float b, float a) ForLow(", bands, StringComparison.Ordinal);
            Assert.Contains("public static (float r, float g, float b, float a) ForDose(", bands, StringComparison.Ordinal);

            // ForDose must read the owning radiation constants, never a re-typed
            // number, so the panel band cannot disagree with the host toast.
            Assert.Contains("RadiationSystem.WarnThreshold", bands, StringComparison.Ordinal);
            Assert.Contains("RadiationSystem.AcuteThreshold", bands, StringComparison.Ordinal);

            foreach (string panel in new[]
            {
                "src/UI/StatusPanel.cs", "src/UI/SurvivorDetailPanel.cs", "src/UI/GameHudOverlay.cs",
            })
                Assert.Contains("AshfallUiBands.", Read(panel), StringComparison.Ordinal);
        }

        [Fact]
        public void AshfallUiBands_HasExactlyOneLowIsBadImplementation()
        {
            // Loop-1 finding 2 — ForNeed(..., highIsBad:false, ...) was a second,
            // independent copy of the ForLow rule, which is exactly the drift
            // this helper exists to remove. It must delegate, not re-implement.
            string bands = Read("src/UI/AshfallUiBands.cs");
            Assert.Contains("return ForLow(value, warnAt, criticalAt);", bands, StringComparison.Ordinal);
            Assert.Contains("return ForHigh(value, warnAt, criticalAt);", bands, StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_CohortBands_PassWarnBeforeCritical()
        {
            // Loop-1 finding 1 — a real bug: avg health was passed
            // (critical, warn) into (warnAt, criticalAt), which killed the warn
            // band and rendered every health <= 30 as hard-critical. Named
            // arguments make the order self-documenting and greppable.
            string panel = Read("src/UI/StatusPanel.cs");
            Assert.Contains("AshfallUiBands.ForNeed(avgHealth, highIsBad: false, warnAt: profile.healthWarn, criticalAt: profile.healthCritical)",
                panel, StringComparison.Ordinal);
            Assert.Contains("AshfallUiBands.ForLow(avgMorale, warnAt: profile.moraleWarn, criticalAt: profile.moraleCritical)",
                panel, StringComparison.Ordinal);
            Assert.Contains("AshfallUiBands.ForDose(avgDose)", panel, StringComparison.Ordinal);

            // The bare positional form is the failure mode; it must not reappear.
            Assert.DoesNotContain("ForNeed(avgHealth, highIsBad: false, profile.", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("ForLow(avgMorale, profile.", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorDetailPanel_DoseRow_UsesTheSharedThreeBand()
        {
            // Task 7 — the row used to jump straight from nominal to Critical at
            // the warn threshold, so a survivor sitting in the warn band looked
            // identical to one at zero dose.
            string panel = Read("src/UI/SurvivorDetailPanel.cs");
            Assert.Contains("AshfallUiBands.ForDose(rad.RadiationDose)", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("rad.RadiationDose >= Ashfall.Core.Radiation.RadiationSystem.WarnThreshold ?",
                panel, StringComparison.Ordinal);
        }

        [Fact]
        public void AshfallUiBands_TreatsUnreadableReadingsAsCritical()
        {
            // Loop-1 hardening — a NaN/Infinity reading must never read as
            // nominal; a corrupt baseline should look like an alarm. Both the
            // need path and the dose path go through the one shared guard.
            string bands = Read("src/UI/AshfallUiBands.cs");
            Assert.Contains("float.IsNaN(value) || float.IsInfinity(value)", bands, StringComparison.Ordinal);
            Assert.Contains("private static bool IsUnreadable(float value)", bands, StringComparison.Ordinal);
            Assert.Contains("if (IsUnreadable(msv)) return Critical;", bands, StringComparison.Ordinal);
            Assert.Contains("if (IsUnreadable(value)) return Critical;", bands, StringComparison.Ordinal);
        }

        [Fact]
        public void HudRadiation_UsesTheSharedDoseBand()
        {
            // Loop-2 finding — the HUD label re-derived its own dose thresholds
            // (>= 100 red, >= 50 amber) against the same RadiationDose field the
            // survivor detail row and the host toast band at 80/50, so at 85 mSv
            // the HUD read "amber" while the panel read "critical". The re-typed
            // literals must not return.
            string hud = Read("src/UI/GameHudOverlay.cs");
            Assert.Contains("AshfallUiBands.ForDose(msv)", hud, StringComparison.Ordinal);
            Assert.DoesNotContain("if (msv >= 100)", hud, StringComparison.Ordinal);
            Assert.DoesNotContain("else if (msv >= 50)", hud, StringComparison.Ordinal);
        }

        [Fact]
        public void AshfallUiBands_DoseNominalStaysOnTheShippedToken()
        {
            // Loop-3 finding — routing the HUD dose label through the helper
            // initially flattened its nominal band from Lethe to Pale. The
            // game_hud_default fixture projects 41 mSv (a nominal reading), so
            // that change silently invalidated the shipped golden. The dose
            // nominal token is now named (Calm) and gated so a future edit
            // cannot quietly re-break the visual contract.
            string bands = Read("src/UI/AshfallUiBands.cs");
            Assert.Contains("public static (float r, float g, float b, float a) Calm => Ashfall.Core.UI.Theme.Lethe;",
                bands, StringComparison.Ordinal);
            Assert.Contains("public static (float r, float g, float b, float a) Nominal => Ashfall.Core.UI.Theme.Pale;",
                bands, StringComparison.Ordinal);

            // ForDose must return Calm (not Nominal) below the warn threshold.
            Assert.Contains("return Calm;", bands, StringComparison.Ordinal);

            // The fixture must keep projecting a nominal dose, which is what
            // makes the golden sensitive to this token in the first place.
            string fixture = Read("src/UI/GameHudSnapshotFixture.cs");
            Assert.Contains("hud.UpdateRadiation(41f);", fixture, StringComparison.Ordinal);
        }

        [Fact]
        public void CohortDoseRows_ShareTheSameBandHelper()
        {
            // Loop-4 finding — three surfaces render the same cohort
            // RadiationDose average. Fixing only the detail row (task 7) left
            // the survival twin still on a 2-band test, so at 60 mSv one panel
            // said "critical" and the other said "warn". All of them must agree.
            Assert.Contains("AshfallUiBands.ForDose(rad.RadiationDose)",
                Read("src/UI/SurvivorDetailPanel.cs"), StringComparison.Ordinal);
            Assert.Contains("AshfallUiBands.ForDose(avgDose)",
                Read("src/UI/SurvivalDetailPanel.cs"), StringComparison.Ordinal);
            Assert.Contains("AshfallUiBands.ForDose(avgDose)",
                Read("src/UI/StatusPanel.cs"), StringComparison.Ordinal);
            Assert.Contains("AshfallUiBands.ForDose(msv)",
                Read("src/UI/GameHudOverlay.cs"), StringComparison.Ordinal);

            // The 2-band form is the specific failure mode.
            Assert.DoesNotContain("avgDose >= RadiationSystem.WarnThreshold ?",
                Read("src/UI/SurvivalDetailPanel.cs"), StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivalDetailPanel_UsesSharedBands_NotMagicThresholds()
        {
            // Loop-3 repair (eighth wave) — the concurrent band sweep replaced the
            // hardcoded 50/80 thresholds with the shared AshfallUiBands helper fed
            // by the owning NeedsProfile, so the cohort twin can no longer teach
            // numbers the simulation does not enforce. Re-pinned to current evidence.
            string panel = Read("src/UI/SurvivalDetailPanel.cs");
            Assert.Contains("AshfallUiBands.ForNeed", panel, StringComparison.Ordinal);
            Assert.Contains("profile.healthWarn", panel, StringComparison.Ordinal);
            Assert.Contains("profile.hungerCritical", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("avgHealth < 50 ?", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("avgHunger >= 80 ?", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void GodotBoundedRunner_PassesGameFlagsAfterTheSeparator()
        {
            // Loop-5 finding — run-godot-bounded.sh splits engine args from game
            // args at a literal "--". Invoked WITHOUT that separator the selftest
            // flag is swallowed as an engine arg: the game just boots normally,
            // the selftest never runs, no artifact is written, and the runner
            // still exits 0. That is a false green, and it is exactly the shape
            // of mistake that makes a runtime check look like it passed.
            // The contract is pinned here so the split cannot be "simplified"
            // away; the correct form is:
            //   run-godot-bounded.sh --path . --headless -- --ui-layout-selftest
            string script = Read("scripts/ci/run-godot-bounded.sh");
            Assert.Contains("separator_index", script, StringComparison.Ordinal);
            Assert.Contains("engine_args", script, StringComparison.Ordinal);
            Assert.Contains("game_args", script, StringComparison.Ordinal);
            // Game args must be appended after the engine args, not before.
            Assert.Contains("\"${engine_args[@]}\" --fixed-fps 15 --max-fps 15 \"${game_args[@]}\"",
                script, StringComparison.Ordinal);
        }

        // ── Eighth-wave task gates ────────────────────────────────────────

        [Fact]
        public void SurvivorDetailPanel_StatusListLabels_AreLocalized()
        {
            // Task 1 — the modifier-list headers and overflow resolve from the catalog.
            string panel = Read("src/UI/SurvivorDetailPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.survivor.status.top_contributors",
                "ui.survivor.status.recent_contributors",
                "ui.survivor.status.other_contributors",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.DoesNotContain("AddRow(_statusList, \"Top active need contributors\"", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("AddRow(_statusList, \"Recent need contributors\"", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorDetailPanel_DimLines_AreLocalized()
        {
            // Task 2 — the empty/not-found/no-radiation states resolve from the catalog.
            string panel = Read("src/UI/SurvivorDetailPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.survivor.info.no_selection", "ui.survivor.info.not_found",
                "ui.survivor.info.no_radiation", "ui.survivor.info.unknown",
                "ui.survivor.info.unknown_source",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.DoesNotContain("MakeDimLine(\"No survivor selected.\")", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("MakeDimLine(\"No radiation state tracked.\")", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_CriticalHealthObjective_UsesCriticalBand()
        {
            // Task 3 — the objective text says "critical health", so it must count
            // the critical band, not the wider warn band.
            string status = Read("src/UI/StatusPanel.cs");
            Assert.Contains("healthProfile.IsHealthCritical(s.Health)", status, StringComparison.Ordinal);
            Assert.DoesNotContain("s.Health < healthWarn", status, StringComparison.Ordinal);
        }

        [Fact]
        public void NeedsProfile_ExposesHealthWarnPredicate()
        {
            // Task 4 — the health warn band joins the predicate family.
            var p = new NeedsProfile();
            Assert.True(p.IsHealthWarn(p.healthWarn));
            Assert.False(p.IsHealthWarn(p.healthWarn + 1f));
            Assert.True(p.healthCritical < p.healthWarn);
        }

        [Fact]
        public void Panels_UseHealthWarnPredicate_NotRawComparison()
        {
            // Task 4 (hardening) — no survival panel re-compares healthWarn directly.
            foreach (string file in new[]
            {
                "src/UI/StatusPanel.cs", "src/UI/SurvivorsPanel.cs", "src/UI/SurvivorDetailPanel.cs",
            })
            {
                string src = Read(file);
                Assert.DoesNotContain("< profile.healthWarn", src, StringComparison.Ordinal);
            }
            Assert.Contains("profile.IsHealthWarn", Read("src/UI/SurvivorsPanel.cs"), StringComparison.Ordinal);
            Assert.Contains("profile.IsHealthWarn", Read("src/UI/SurvivorDetailPanel.cs"), StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorDetailPanel_WorldviewAndIdentityRows_AreLocalized()
        {
            // Task 5 — worldview/keepsake/duty/traits rows resolve from the catalog.
            string panel = Read("src/UI/SurvivorDetailPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.survivor.info.worldview", "ui.survivor.info.keepsake",
                "ui.survivor.info.duty", "ui.survivor.info.duty_bonus",
                "ui.survivor.info.traits", "ui.survivor.info.ideological_tension",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.DoesNotContain("$\"Worldview: ", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("$\"Traits: ", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorDetailPanel_RelationshipRows_AreLocalized()
        {
            // Task 6 — faction/romance/family/lineage/bloc rows resolve from the catalog.
            string panel = Read("src/UI/SurvivorDetailPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.survivor.info.faction", "ui.survivor.info.relationship",
                "ui.survivor.info.soulmate", "ui.survivor.info.family",
                "ui.survivor.info.lineage", "ui.survivor.info.political_bloc",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.DoesNotContain("$\"Bunker Faction: ", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("$\"Relationship: ", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorDetailPanel_EffectsAndRecordsRows_AreLocalized()
        {
            // Task 7 — demographic/personal-effects/records rows resolve from the catalog.
            string panel = Read("src/UI/SurvivorDetailPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.survivor.info.demographic", "ui.survivor.info.retired_elder",
                "ui.survivor.info.retirement_eligible", "ui.survivor.info.personal_effects",
                "ui.survivor.info.personal_effect_row", "ui.survivor.info.favorite",
                "ui.survivor.info.more", "ui.survivor.info.authored_records",
                "ui.survivor.info.record_row",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.DoesNotContain("$\"Personal effects: ", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("$\"Authored records: ", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorDetailPanel_ModifierRows_AreLocalized()
        {
            // Task 8 — the modifier-row formats resolve from the catalog.
            string panel = Read("src/UI/SurvivorDetailPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.survivor.status.modifier_row", "ui.survivor.status.recent_row",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.DoesNotContain("$\"  {FormatModifierSource", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorDetailPanel_HasNoRawInterpolatedLabel()
        {
            // Task 9 — every human-readable label resolves from the catalog. A
            // `$"` whose first character after the quote is a letter is a raw
            // label; `$"{...}` and value-only formatters (leading space) pass.
            string panel = Read("src/UI/SurvivorDetailPanel.cs");
            var offenders = new System.Collections.Generic.List<string>();
            int idx = 0;
            while ((idx = panel.IndexOf("$\"", idx, StringComparison.Ordinal)) >= 0)
            {
                int after = idx + 2;
                if (after < panel.Length && char.IsLetter(panel[after]))
                {
                    int line = 1;
                    for (int i = 0; i < idx; i++) if (panel[i] == '\n') line++;
                    offenders.Add("line " + line);
                }
                idx = after;
            }
            Assert.True(offenders.Count == 0, "raw interpolated labels: " + string.Join(", ", offenders));
        }

        [Fact]
        public void StringsCsv_PlaceholderNames_AreNumeric()
        {
            // Task 10 — positional placeholders only; a named {count} placeholder
            // would silently depend on an argument name the caller never supplies.
            var named = new System.Collections.Generic.List<string>();
            var lines = Read("assets/l10n/strings.csv").Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].TrimEnd('\r');
                if (line.Length == 0) continue;
                var cols = SplitCsvLine(line);
                if (cols.Length < 2) continue;
                foreach (System.Text.RegularExpressions.Match m in
                    System.Text.RegularExpressions.Regex.Matches(cols[1], @"\{([A-Za-z_][A-Za-z0-9_]*)(?::[^}]*)?\}"))
                    named.Add(cols[0] + ":" + m.Groups[1].Value);
            }
            Assert.True(named.Count == 0, "named placeholders: " + string.Join(", ", named));
        }

        [Fact]
        public void StringsCsv_SourceColumn_IsCsOrJson()
        {
            // Task 11 — a source path must point at a code or data file, never a
            // stray fragment produced by an unquoted comma.
            var bad = new System.Collections.Generic.List<string>();
            var lines = Read("assets/l10n/strings.csv").Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].TrimEnd('\r');
                if (line.Length == 0) continue;
                var cols = SplitCsvLine(line);
                if (cols.Length < 4) continue;
                string source = cols[3].Trim();
                if (source.Length == 0) continue;
                if (!source.EndsWith(".cs", StringComparison.Ordinal)
                    && !source.EndsWith(".json", StringComparison.Ordinal))
                    bad.Add(cols[0] + " -> " + source);
            }
            Assert.True(bad.Count == 0, "non-code/data source columns: " + string.Join("; ", bad));
        }

        [Fact]
        public void StatusPanel_SectionHeaders_AreLocalized()
        {
            // Task 12 — every StatusPanel section header resolves from the catalog.
            string status = Read("src/UI/StatusPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.status.section.day_environment", "ui.status.section.directives",
                "ui.status.section.cohort", "ui.status.section.shelter",
                "ui.status.section.thermal", "ui.status.section.forecast",
                "ui.status.threshold.header",
            })
            {
                Assert.Contains(key, status, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
        }

        [Fact]
        public void SurvivorsPanel_RailCaptions_AreLocalized()
        {
            // Task 13 — every rail caption resolves from the catalog.
            string panel = Read("src/UI/SurvivorsPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.survivors.rail.living", "ui.survivors.rail.avg_hp",
                "ui.survivors.rail.avg_rad", "ui.survivors.rail.avg_mor",
                "ui.survivors.rail.strained",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
        }

        [Fact]
        public void CiReadme_DocumentsLayoutArtifact()
        {
            // Task 14 — the machine-readable layout artifact and its parse contract
            // are documented next to the check.
            string readme = Read("scripts/ci/README.md");
            Assert.Contains("ui-layout-selftest.json", readme, StringComparison.Ordinal);
            Assert.Contains("mtime", readme, StringComparison.Ordinal);
        }

        [Fact]
        public void UiLayoutCheckScript_RejectsStaleArtifact()
        {
            // Task 15 — the wrapper records the run start and rejects an artifact
            // older than it, so a stale PASS cannot mask a missing write.
            string script = Read("scripts/ci/ui-layout-check.sh");
            Assert.Contains("start_epoch=", script, StringComparison.Ordinal);
            Assert.Contains("artifact_epoch", script, StringComparison.Ordinal);
            Assert.Contains("-lt \"$start_epoch\"", script, StringComparison.Ordinal);
        }

        // ── Eighth-wave loop gates ────────────────────────────────────────

        [Fact]
        public void UiSources_DoNotRetypeHealthBandLiterals()
        {
            // Loop-1 hardening (eighth wave) — the health warn/critical bands live
            // in NeedsProfile. A panel must not re-type 30/25 for survivor health;
            // `filterHealth` is a different domain and is excluded by the word
            // boundary before `Health`.
            string uiDir = Path.Combine(RepoRoot(), "src", "UI");
            var offenders = new System.Collections.Generic.List<string>();
            var pattern = new System.Text.RegularExpressions.Regex(@"\bHealth\s*<\s*(30|25)f?\b");
            foreach (string file in Directory.GetFiles(uiDir, "*.cs", SearchOption.AllDirectories))
            {
                string src = File.ReadAllText(file);
                foreach (System.Text.RegularExpressions.Match m in pattern.Matches(src))
                    offenders.Add(Path.GetFileName(file) + ":" + m.Value);
            }
            Assert.True(offenders.Count == 0, "re-typed health band literals: " + string.Join(", ", offenders));
        }

        [Fact]
        public void NeedsProfile_ExposesSharedHealthBandDefaults()
        {
            // Loop-1 hardening — the literal 30/25 has one home; the instance
            // fields and the panels both derive from it.
            Assert.Equal(30f, NeedsProfile.DefaultHealthWarn, 3);
            Assert.Equal(25f, NeedsProfile.DefaultHealthCritical, 3);
            var p = new NeedsProfile();
            Assert.Equal(NeedsProfile.DefaultHealthWarn, p.healthWarn, 3);
            Assert.Equal(NeedsProfile.DefaultHealthCritical, p.healthCritical, 3);
        }

        [Fact]
        public void StatusPanel_HasNoRawInterpolatedLabel()
        {
            // Loop-2 hardening — every StatusPanel label resolves from the catalog;
            // a `$"` starting with a letter is a raw label.
            string panel = Read("src/UI/StatusPanel.cs");
            var offenders = new System.Collections.Generic.List<string>();
            int idx = 0;
            while ((idx = panel.IndexOf("$\"", idx, StringComparison.Ordinal)) >= 0)
            {
                int after = idx + 2;
                if (after < panel.Length && char.IsLetter(panel[after]))
                {
                    int line = 1;
                    for (int i = 0; i < idx; i++) if (panel[i] == '\n') line++;
                    offenders.Add("line " + line);
                }
                idx = after;
            }
            Assert.True(offenders.Count == 0, "raw interpolated labels: " + string.Join(", ", offenders));
        }

        [Fact]
        public void UiSources_DoNotRetypeRadiationWarnLiteral()
        {
            // Loop-2 hardening — the radiation warn band lives in RadiationSystem.
            // A dose/radiation comparison against a literal 50 drifts the moment
            // the authority changes.
            string uiDir = Path.Combine(RepoRoot(), "src", "UI");
            var offenders = new System.Collections.Generic.List<string>();
            var pattern = new System.Text.RegularExpressions.Regex(
                @"(Dose|dose|Msv|msv|Radiation|radiation|radClamped|safeRadiation)\s*>=\s*50f?\b");
            foreach (string file in Directory.GetFiles(uiDir, "*.cs", SearchOption.AllDirectories))
            {
                string src = File.ReadAllText(file);
                foreach (System.Text.RegularExpressions.Match m in pattern.Matches(src))
                    offenders.Add(Path.GetFileName(file) + ":" + m.Value);
            }
            Assert.True(offenders.Count == 0, "re-typed radiation warn literals: " + string.Join(", ", offenders));
        }

        [Fact]
        public void StatusPanel_ForecastDayLabel_IsLocalized()
        {
            // Loop-2 repair — the forecast row label resolves from the catalog.
            string panel = Read("src/UI/StatusPanel.cs");
            Assert.Contains("ui.status.forecast.day_label", panel, StringComparison.Ordinal);
            Assert.Contains("ui.status.forecast.day_label,", Read("assets/l10n/strings.csv"), StringComparison.Ordinal);
            Assert.DoesNotContain("$\"Day +", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void UiSources_DoNotRetypeCohortNeedLiterals()
        {
            // Loop-3 hardening — the cohort rows read the owning NeedsProfile via
            // the shared bands; a literal avgX < N comparison is drift.
            string uiDir = Path.Combine(RepoRoot(), "src", "UI");
            var offenders = new System.Collections.Generic.List<string>();
            var pattern = new System.Text.RegularExpressions.Regex(
                @"avg(Hunger|Thirst|Fatigue|Morale|Warmth|Health)\s*(<|>=|<=)\s*[0-9]");
            foreach (string file in Directory.GetFiles(uiDir, "*.cs", SearchOption.AllDirectories))
            {
                string src = File.ReadAllText(file);
                foreach (System.Text.RegularExpressions.Match m in pattern.Matches(src))
                    offenders.Add(Path.GetFileName(file) + ":" + m.Value);
            }
            Assert.True(offenders.Count == 0, "re-typed cohort literals: " + string.Join(", ", offenders));
        }

        // ── Cohort truth + localization wave (2026-10-02) ────────────────
        // Tasks 1, 3, 9, 12, 13 and 15 of this wave are already gated by the
        // re-typed-literal ratchets above; these cover the remainder so the
        // wave is not double-tested in two different wordings.

        [Fact]
        public void SurvivalDetailPanel_RowsResolveThroughTheSharedTextHelper()
        {
            // Task 2 — the panel was the last cohort surface with no Tr calls at
            // all: every row was a hardcoded English interpolation. It now
            // routes through the shared helper, and the l10n drift gate has the
            // panel enrolled so a new untranslated row fails CI rather than
            // shipping silently.
            string panel = Read("src/UI/SurvivalDetailPanel.cs");
            foreach (string key in new[]
            {
                "ui.survival_detail.no_roster", "ui.survival_detail.roster",
                "ui.survival_detail.avg_health", "ui.survival_detail.avg_hunger",
                "ui.survival_detail.avg_thirst", "ui.survival_detail.avg_fatigue",
                "ui.survival_detail.avg_morale", "ui.survival_detail.avg_dose",
                "ui.survival_detail.above_threshold", "ui.survival_detail.critical_health",
                "ui.survival_detail.weakest_ceiling",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key, Read("assets/l10n/strings.csv"), StringComparison.Ordinal);
            }

            Assert.Contains("private static string Tr(string key, string fallback) => AshfallUiText.Tr(key, fallback);",
                panel, StringComparison.Ordinal);
            Assert.Contains("private static string TrFormat(string key, params object[] args) => AshfallUiText.TrFormat(key, args);",
                panel, StringComparison.Ordinal);

            // No row may be built from a hardcoded English interpolation again.
            Assert.DoesNotContain("$\"Avg ", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("$\"Roster: ", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("$\"Critical health: ", panel, StringComparison.Ordinal);

            // Enforcement, not merely intent: the gate must list the panel.
            Assert.Contains("SurvivalDetailPanel.cs", Read("scripts/ci/l10n_drift_gate.py"), StringComparison.Ordinal);
        }

        [Fact]
        public void HudRadiationMeter_SharesTheLabelBand()
        {
            // Task 4 — the bar's fill was latched once at construction, so at
            // 85 mSv the radiation bar stayed calm cyan while the label beside
            // it had already gone red. Bar and label now read one band value.
            string hud = Read("src/UI/GameHudOverlay.cs");
            Assert.Contains("var band = AshfallUiBands.ForDose(msv);", hud, StringComparison.Ordinal);
            Assert.Contains("AshfallUiHelpers.ToColor(band)", hud, StringComparison.Ordinal);
            Assert.Contains("SetMeterFill(_barRad, band)", hud, StringComparison.Ordinal);
            Assert.Contains("private static void SetMeterFill(", hud, StringComparison.Ordinal);
        }

        [Fact]
        public void DoseLedgerMapBand_UsesItsOwnCumulativeScale()
        {
            // Task 5 — DoseLedgerPanel bands a cumulative lifetime total off
            // DoseLedgerSystem (Amber/Red/Black), which is a genuinely different
            // authority and scale from RadiationSystem's instantaneous dose.
            // Pinned so a future "consolidation" cannot quietly point the ledger
            // at the wrong threshold and make a 400 mSv lifetime read nominal.
            string ledger = Read("src/UI/DoseLedgerPanel.cs");
            Assert.Contains("DoseLedgerSystem.BlackMsv", ledger, StringComparison.Ordinal);
            Assert.Contains("DoseLedgerSystem.RedMsv", ledger, StringComparison.Ordinal);
            Assert.Contains("DoseLedgerSystem.AmberMsv", ledger, StringComparison.Ordinal);
            Assert.DoesNotContain("RadiationSystem.WarnThreshold", ledger, StringComparison.Ordinal);
            Assert.DoesNotContain("AshfallUiBands.ForDose", ledger, StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_StoresAndCohortRowsAreBanded()
        {
            // Tasks 6 & 7 — the stores rows were the only unbanded measurements
            // on the cohort card, and the cohort row reported "survivors lost"
            // in neutral text while RenderDayInfo raised the same fact in
            // Critical. One fact, one signal.
            //
            // Loop-1 of the verification pass — the stores rows first used a
            // local BandStores helper returning Lethe for nominal, making them
            // the only banded rows on the card off the shared nominal token.
            // They are low-is-bad like morale, so the shared helper is the
            // correct one and no second band rule is warranted.
            string panel = Read("src/UI/StatusPanel.cs");
            Assert.Contains("AshfallUiBands.ForLow(water, warnAt: WaterWarnStores, criticalAt: WaterCriticalStores)", panel, StringComparison.Ordinal);
            Assert.Contains("AshfallUiBands.ForLow(food, warnAt: FoodWarnStores, criticalAt: FoodCriticalStores)", panel, StringComparison.Ordinal);
            Assert.Contains("alive < roster.Count ? AshfallUiBands.Critical : null", panel, StringComparison.Ordinal);
            // The water-critical figure must stay the one RenderObjectives uses.
            Assert.Contains("private const int WaterCriticalStores = 3;", panel, StringComparison.Ordinal);
            // No second band rule may reappear on this card.
            Assert.DoesNotContain("BandStores(", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivalDetailPanel_HealthyStatesAreNotRenderedAsMuted()
        {
            // Loop-2 — three rows used Theme.Dim, this panel's muted token for
            // absent secondary detail, to render states that are actually GOOD
            // news: "0 survivors above threshold", "0 critically ill", and a
            // shelter figure that read identically whether sound or collapsed.
            // A healthy cohort read as broken, and a collapsing shelter read as
            // fine. Healthy states now use the calm token and the shelter
            // figure is banded.
            string panel = Read("src/UI/SurvivalDetailPanel.cs");
            Assert.Contains("dosed > 0 ? Ashfall.Core.UI.Theme.Warm : AshfallUiBands.Calm", panel, StringComparison.Ordinal);
            Assert.Contains("critical > 0 ? Ashfall.Core.UI.Theme.Critical : AshfallUiBands.Calm", panel, StringComparison.Ordinal);
            Assert.Contains("weakestCeiling <= 0f ? AshfallUiBands.Critical", panel, StringComparison.Ordinal);
            Assert.Contains("AshfallUiBands.ForLow(weakestCeiling, warnAt: ShelterWarnCeilingPct, criticalAt: ShelterCriticalCeilingPct)",
                panel, StringComparison.Ordinal);

            // Dim must not come back as a "nothing to report" health state.
            Assert.DoesNotContain("? Ashfall.Core.UI.Theme.Dim", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void AshfallUiBands_ExposesTheMetricCardVocabulary()
        {
            // Loop-3 — a static sweep found the UI tree still carries a second
            // band vocabulary: status-rail metric cards are driven by
            // AshfallMetricCard.Criticality, and 123 UI files still band them
            // against re-typed literals. Folding all of that in is its own
            // package, so the sanctioned bridge is pinned here: a future panel
            // must be able to adopt the owning thresholds through this helper
            // rather than re-derive them.
            string bands = Read("src/UI/AshfallUiBands.cs");
            Assert.Contains("public static AshfallMetricCard.Criticality ToCriticality(", bands, StringComparison.Ordinal);
            Assert.Contains("public static AshfallMetricCard.Criticality CriticalityForNeed(", bands, StringComparison.Ordinal);
            Assert.Contains("public static AshfallMetricCard.Criticality CriticalityForDose(float msv)", bands, StringComparison.Ordinal);
            // The bridge must reuse the band rules, not restate them.
            Assert.Contains("=> ToCriticality(ForNeed(value, highIsBad, warnAt, criticalAt));", bands, StringComparison.Ordinal);
            Assert.Contains("=> ToCriticality(ForDose(msv));", bands, StringComparison.Ordinal);
            // Criticality is defined by the card owner, not duplicated here.
            Assert.Contains("public enum Criticality { Normal = 0, Caution = 1, Warn = 2, Critical = 3 }",
                Read("src/UI/AshfallMetricCard.cs"), StringComparison.Ordinal);
        }

        [Fact]
        public void StatusPanel_InjuryAndForecastSuffixAreCatalogKeys()
        {
            // Tasks 8 & 10 — the nameless-survivor placeholder was a bare
            // "[UNNAMED]" and the " — SHORT" suffix was glued on from a
            // hardcoded separator, so a German player saw English fragments
            // spliced onto translated values.
            string panel = Read("src/UI/StatusPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            Assert.Contains("TrFormat(\"ui.status.forecast.day_value_short\", row.FoodAfter, row.WaterAfter)",
                panel, StringComparison.Ordinal);
            Assert.Contains("Tr(\"ui.status.expedition_injury.unnamed\", \"[UNNAMED]\")", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("\" — \" + Tr(\"ui.status.forecast.short\"", panel, StringComparison.Ordinal);
            foreach (string key in new[]
            {
                "ui.status.forecast.day_value_short", "ui.status.expedition_injury.unnamed",
                "ui.status.cohort.alive_of_total", "ui.status.stores.water_value", "ui.status.stores.food_value",
            })
                Assert.Contains(key, csv, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorDetailPanel_DoseRowsCarryBandTooltips()
        {
            // Task 11 — an amber number with no way to learn that 50 mSv is the
            // warn line teaches the player nothing about the band they are in.
            string panel = Read("src/UI/SurvivorDetailPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            Assert.Contains("doseRow.TooltipText", panel, StringComparison.Ordinal);
            Assert.Contains("lifetimeRow.TooltipText", panel, StringComparison.Ordinal);
            // The tooltip figures must come from the owning constants.
            Assert.Contains("RadiationSystem.WarnThreshold:0}", panel, StringComparison.Ordinal);
            Assert.Contains("RadiationSystem.AcuteThreshold:0}", panel, StringComparison.Ordinal);
            foreach (string key in new[] { "ui.survivor.trait.dose_tooltip", "ui.survivor.trait.lifetime_tooltip" })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key, csv, StringComparison.Ordinal);
            }
        }

        // ── Ninth-wave task gates ─────────────────────────────────────────

        [Fact]
        public void AchievementsPanel_StatLabels_AreLocalized()
        {
            // Task 1 — the four stat rows resolve from the catalog.
            string panel = Read("src/UI/AchievementsPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.achievements.days_survived", "ui.achievements.roster_alive",
                "ui.achievements.avg_health", "ui.achievements.avg_dose",
                "ui.achievements.no_roster", "ui.achievements.no_milestones",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.DoesNotContain("$\"Days Survived: ", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("$\"Average Dose: ", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void AfflictionsPanel_CriticalHealth_IsLocalized()
        {
            // Task 2 — the critical-health affliction row resolves from the catalog.
            string panel = Read("src/UI/AfflictionsPanel.cs");
            Assert.Contains("ui.afflictions.critical_health", panel, StringComparison.Ordinal);
            Assert.Contains("ui.afflictions.critical_health,", Read("assets/l10n/strings.csv"), StringComparison.Ordinal);
            Assert.DoesNotContain("Critical health (", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void MedicalPanel_RowLabelsAndConditionProse_AreLocalized()
        {
            // Task 3 — the survivor-row labels and respiratory prose resolve.
            string panel = Read("src/UI/MedicalPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.medical.hp", "ui.medical.rad", "ui.medical.resist_suffix",
                "ui.medical.hunger", "ui.medical.thirst", "ui.medical.lung_row",
                "ui.medical.last_event", "ui.medical.reserved", "ui.medical.severe_cough",
                "ui.medical.lung.clear", "ui.medical.lung.mild", "ui.medical.lung.permanent",
                "ui.medical.lung.critical", "ui.medical.lung.terminal",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.DoesNotContain("$\"HP ", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("\"SEVERE COUGH", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivalDetailPanel_IsLocalized()
        {
            // Task 4 — the cohort twin routes every row through the catalog.
            string panel = Read("src/UI/SurvivalDetailPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.survival_detail.no_roster", "ui.survival_detail.roster",
                "ui.survival_detail.avg_health", "ui.survival_detail.avg_hunger",
                "ui.survival_detail.avg_thirst", "ui.survival_detail.avg_fatigue",
                "ui.survival_detail.avg_morale", "ui.survival_detail.avg_dose",
                "ui.survival_detail.above_threshold", "ui.survival_detail.critical_health",
                "ui.survival_detail.weakest_ceiling",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.Contains("AshfallUiText.Tr", panel, StringComparison.Ordinal);
        }

        private static string[] SurvivalPanelFiles() => new[]
        {
            "src/UI/StatusPanel.cs", "src/UI/SurvivorsPanel.cs",
            "src/UI/SurvivorDetailPanel.cs", "src/UI/SurvivalDetailPanel.cs",
        };

        [Fact]
        public void SurvivalPanels_DoNotRetypeWarmthBandLiteral()
        {
            // Task 5 — warmth bands come from the authored profile.
            foreach (string file in SurvivalPanelFiles())
                Assert.DoesNotMatch(new System.Text.RegularExpressions.Regex(@"[Ww]armth\s*(<=|<)\s*[0-9]"), Read(file));
        }

        [Fact]
        public void SurvivalPanels_DoNotRetypeMoraleBandLiteral()
        {
            // Task 6 — morale bands come from the authored profile.
            foreach (string file in SurvivalPanelFiles())
                Assert.DoesNotMatch(new System.Text.RegularExpressions.Regex(@"[Mm]orale\s*(<=|<)\s*[0-9]"), Read(file));
        }

        [Fact]
        public void SurvivalPanels_DoNotRetypeHungerThirstBandLiteral()
        {
            // Task 7 — hunger/thirst bands come from the authored profile.
            foreach (string file in SurvivalPanelFiles())
                Assert.DoesNotMatch(new System.Text.RegularExpressions.Regex(@"([Hh]unger|[Tt]hirst)\s*(>=|>)\s*[0-9]"), Read(file));
        }

        [Fact]
        public void StringsCsv_KeysAreSnakeCaseDotted()
        {
            // Task 8 — every key is lower_snake.dotted so the loader and gates
            // can rely on the shape.
            var bad = new System.Collections.Generic.List<string>();
            var pattern = new System.Text.RegularExpressions.Regex(@"^[a-z0-9_]+(\.[a-z0-9_]+)+$");
            var lines = Read("assets/l10n/strings.csv").Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].TrimEnd('\r');
                if (line.Length == 0) continue;
                string key = SplitCsvLine(line)[0];
                if (!pattern.IsMatch(key)) bad.Add(key);
            }
            Assert.True(bad.Count == 0, "non-snake_case keys: " + string.Join(", ", bad));
        }

        [Fact]
        public void StringsCsv_NoSpaceAdjacentToQuote()
        {
            // Task 9 — a space between the delimiter and a quote (or the reverse)
            // is a raw-line smell the parsed edge-whitespace gate cannot name.
            var bad = new System.Collections.Generic.List<string>();
            var lines = Read("assets/l10n/strings.csv").Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].TrimEnd('\r');
                if (line.Length == 0) continue;
                if (line.Contains(", \"", StringComparison.Ordinal)
                    || line.Contains("\" ,", StringComparison.Ordinal))
                    bad.Add("line " + (i + 1));
            }
            Assert.True(bad.Count == 0, "space adjacent to quote: " + string.Join(", ", bad));
        }

        [Fact]
        public void StatusPanel_ForecastRowCount_Bounded()
        {
            // Task 10 — the forecast strip is bounded by the host projection and
            // the Core 3-day horizon; it can never grow unbounded.
            Assert.Equal(3, Ashfall.Core.Campaign.SupplyForecast.DefaultHorizonDays);
            string status = Read("src/UI/StatusPanel.cs");
            Assert.Contains("for (int i = 0; i < rows.Count; i++)", status, StringComparison.Ordinal);
            Assert.Contains("ui.status.forecast.day_label", status, StringComparison.Ordinal);
            Assert.Contains("new Vector2(640, 440)", status, StringComparison.Ordinal);
        }

        [Fact]
        public void SurvivorDetailPanel_RenderedRowCount_Accounting()
        {
            // Task 11 — the panel resets its row count and increments it with the
            // fixed blocks, so a future row cannot be added uncounted.
            string panel = Read("src/UI/SurvivorDetailPanel.cs");
            Assert.Contains("RenderedRowCount = 0;", panel, StringComparison.Ordinal);
            Assert.Contains("RenderedRowCount += 2;", panel, StringComparison.Ordinal);
            Assert.Contains("RenderedRowCount += 7;", panel, StringComparison.Ordinal);
            Assert.Contains("RenderedRowCount += 5;", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void StringsCsv_DuplicateSource_StaysBounded()
        {
            // Task 12 — one source can legitimately own many keys (a data catalog),
            // but a runaway reuse is a data-entry smell; pin the current ceiling.
            var counts = new System.Collections.Generic.Dictionary<string, int>(StringComparer.Ordinal);
            var lines = Read("assets/l10n/strings.csv").Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].TrimEnd('\r');
                if (line.Length == 0) continue;
                var cols = SplitCsvLine(line);
                if (cols.Length < 4) continue;
                string source = cols[3].Trim();
                if (source.Length == 0) continue;
                counts.TryGetValue(source, out int n);
                counts[source] = n + 1;
            }
            int max = 0;
            string top = "";
            foreach (var pair in counts)
                if (pair.Value > max) { max = pair.Value; top = pair.Key; }
            Assert.True(max <= 160, $"keys-per-source ceiling exceeded: {top}={max} (pin 160)");
        }

        [Fact]
        public void CiReadme_DocumentsL10nKeyCount()
        {
            // Task 13 — the README explains that the gate prints the catalog size.
            string readme = Read("scripts/ci/README.md");
            Assert.Contains("catalog size", readme, StringComparison.Ordinal);
            Assert.Contains("German", readme, StringComparison.Ordinal);
        }

        [Fact]
        public void GodotBoundedRunner_SupportsExpectedArtifact()
        {
            // Task 14 — the bounded runner can promise an artifact and fail when
            // it is not written, so a false green is impossible.
            string script = Read("scripts/ci/run-godot-bounded.sh");
            Assert.Contains("ASHFALL_EXPECT_ARTIFACT", script, StringComparison.Ordinal);
            Assert.Contains("! -s", script, StringComparison.Ordinal);
            string wrapper = Read("scripts/ci/ui-layout-check.sh");
            Assert.Contains("ASHFALL_EXPECT_ARTIFACT=", wrapper, StringComparison.Ordinal);
        }

        [Fact]
        public void UiLayoutArtifact_HasRequiredFields()
        {
            // Task 15 — the artifact schema is pinned at the writer so a reader
            // can rely on test/status/failures.
            string host = Read("src/Host/HostCli.Command.RunUiLayoutSelfTest.cs");
            Assert.Contains("\\\"test\\\":\\\"ui_layout_selftest\\\"", host, StringComparison.Ordinal);
            Assert.Contains("\\\"status\\\"", host, StringComparison.Ordinal);
            Assert.Contains("\\\"failures\\\"", host, StringComparison.Ordinal);
        }

        // ── Ninth-wave loop gates ─────────────────────────────────────────

        [Fact]
        public void UiSources_DoNotRetypeRadiationLowBandLiteral()
        {
            // Loop-1 hardening (ninth wave) — a dose ceiling comparison against a
            // literal 25/50/100 drifts from RadiationSystem.
            string uiDir = Path.Combine(RepoRoot(), "src", "UI");
            var offenders = new System.Collections.Generic.List<string>();
            var pattern = new System.Text.RegularExpressions.Regex(@"[Dd]ose\s*<\s*(25|50|100)f?\b");
            foreach (string file in Directory.GetFiles(uiDir, "*.cs", SearchOption.AllDirectories))
            {
                string src = File.ReadAllText(file);
                foreach (System.Text.RegularExpressions.Match m in pattern.Matches(src))
                    offenders.Add(Path.GetFileName(file) + ":" + m.Value);
            }
            Assert.True(offenders.Count == 0, "re-typed radiation low bands: " + string.Join(", ", offenders));
        }

        [Fact]
        public void MedicalPanel_ReadyLabels_AreLocalized()
        {
            // Loop-1 repair — the treatment readiness labels resolve from the catalog.
            string panel = Read("src/UI/MedicalPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[] { "ui.medical.ready_for_tick", "ui.medical.ready_in" })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.DoesNotContain("$\"READY IN ", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void AfflictionsPanel_RecentRecord_IsLocalized()
        {
            // Loop-1 repair — the recent-record dim line resolves from the catalog.
            string panel = Read("src/UI/AfflictionsPanel.cs");
            Assert.Contains("ui.afflictions.recent_record", panel, StringComparison.Ordinal);
            Assert.Contains("ui.afflictions.recent_record,", Read("assets/l10n/strings.csv"), StringComparison.Ordinal);
            Assert.DoesNotContain("$\"Recent medical record", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void AchievementsPanel_Chrome_IsLocalized()
        {
            // Loop-2 repair — title, section headers, and close button resolve.
            string panel = Read("src/UI/AchievementsPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.achievements.title", "ui.achievements.section.stats",
                "ui.achievements.section.earned", "ui.achievements.section.targets",
                "ui.achievements.close",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.DoesNotContain("MakeTitle(\"ACHIEVEMENTS", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("MakeSectionHeader(\"RUN STATISTICS\")", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void RegisteredLocalizedPanels_UseTheSharedHelper()
        {
            // Loop-3 hardening — the panels registered with the l10n drift gate
            // must resolve through AshfallUiText, not the catalog namespace.
            foreach (string file in new[]
            {
                "src/UI/AchievementsPanel.cs", "src/UI/AfflictionsPanel.cs",
                "src/UI/MedicalPanel.cs", "src/UI/SurvivorDetailPanel.cs",
                "src/UI/SurvivalDetailPanel.cs",
            })
            {
                Assert.DoesNotContain("AshfallLocalization.Tr(", Read(file), StringComparison.Ordinal);
            }
        }

        [Fact]
        public void L10nDriftGate_RegistersTheNinthWaveSurfaces()
        {
            // Loop-3 hardening — the gate must keep checking the newly-localized
            // panels, so an unregistered rename cannot silently drop coverage.
            string gate = Read("scripts/ci/l10n_drift_gate.py");
            foreach (string name in new[]
            {
                "AchievementsPanel.cs", "AfflictionsPanel.cs", "MedicalPanel.cs",
                "SurvivorDetailPanel.cs", "SurvivalDetailPanel.cs",
            })
                Assert.Contains(name, gate, StringComparison.Ordinal);
        }

        // ── Tenth-wave task gates ─────────────────────────────────────────

        [Fact]
        public void AchievementsPanel_MilestoneProse_IsLocalized()
        {
            // Task 1 — derived milestone + next-target prose resolve from the catalog.
            string panel = Read("src/UI/AchievementsPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.achievements.milestone.first_week", "ui.achievements.milestone.two_week",
                "ui.achievements.milestone.month_ash", "ui.achievements.milestone.no_casualties",
                "ui.achievements.milestone.low_exposure", "ui.achievements.milestone.healthy_cohort",
                "ui.achievements.target.day7", "ui.achievements.target.day7_done",
                "ui.achievements.target.day14", "ui.achievements.target.day14_done",
                "ui.achievements.target.day30", "ui.achievements.target.day30_done",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
        }

        [Fact]
        public void AfflictionsPanel_AccommodationFeedback_IsLocalized()
        {
            // Task 2 — the accommodation tooltips and feedback resolve.
            string panel = Read("src/UI/AfflictionsPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.afflictions.fit_tooltip", "ui.afflictions.missing_tooltip",
                "ui.afflictions.cannot_fit", "ui.afflictions.fitted",
                "ui.afflictions.cannot_fit_short", "ui.afflictions.cannot_remove",
                "ui.afflictions.removed", "ui.afflictions.cannot_remove_short",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.DoesNotContain("$\"Fit {def.display_name}", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void MedicalPanel_TreatmentButtons_AreLocalized()
        {
            // Task 3 — the treatment buttons and respiratory action row resolve.
            string panel = Read("src/UI/MedicalPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.medical.bandage", "ui.medical.iodine", "ui.medical.anti_rad",
                "ui.medical.inhaler_apply", "ui.medical.inhaler_plain", "ui.medical.herbal_tea",
                "ui.medical.inhaler_relief", "ui.medical.inhaler_no_item", "ui.medical.inhaler_no_damage",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.DoesNotContain("$\"BANDAGE (+25 HP)", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void MedicalPanel_CohortAndCare_AreLocalized()
        {
            // Task 4 — the cohort penalty summary and care-cancel entry resolve.
            string panel = Read("src/UI/MedicalPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[] { "ui.medical.cohort_penalty", "ui.medical.care_cancelled" })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.DoesNotContain("$\"Crafting {_medicalHost.ActiveCraftingPenalty", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void MedicalPanel_UnnamedFallback_IsLocalized()
        {
            // Task 5 — the unnamed fallback resolves from the catalog.
            string panel = Read("src/UI/MedicalPanel.cs");
            Assert.Contains("ui.medical.unnamed", panel, StringComparison.Ordinal);
            Assert.Contains("ui.medical.unnamed,", Read("assets/l10n/strings.csv"), StringComparison.Ordinal);
            Assert.DoesNotContain("return \"[UNNAMED]\"", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void GameDashboardPanel_UsesProfileNeedBands()
        {
            // Task 6 — the hunger/thirst gauges read the profile defaults.
            string panel = Read("src/UI/GameDashboardPanel.cs");
            Assert.Contains("NeedsProfile.DefaultHungerCritical", panel, StringComparison.Ordinal);
            Assert.Contains("NeedsProfile.DefaultThirstWarn", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("safeHunger >= 80", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("safeThirst >= 50", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void ShelterHudPanel_UsesProfileNeedBands()
        {
            // Task 7 — the shelter HUD condition rows read the profile defaults.
            string panel = Read("src/UI/ShelterHudPanel.cs");
            Assert.Contains("NeedsProfile.DefaultHungerCritical", panel, StringComparison.Ordinal);
            Assert.Contains("NeedsProfile.DefaultThirstWarn", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("hungerClamped >= 80", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void MedicalPanel_HealthRail_UsesProfileBands()
        {
            // Task 8 — the health rail anchors its alarming levels to the profile.
            string panel = Read("src/UI/MedicalPanel.cs");
            Assert.Contains("NeedsProfile.DefaultHealthCritical", panel, StringComparison.Ordinal);
            Assert.Contains("NeedsProfile.DefaultHealthWarn", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("avgHp > 0 ? AshfallMetricCard.Criticality.Warn", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void AchievementsPanel_HasCatalogLocalizationPath()
        {
            // Task 9 — the achievement row resolves per-id catalog keys with the
            // authored name/description as fallback.
            string panel = Read("src/UI/AchievementsPanel.cs");
            Assert.Contains("AchievementName(def)", panel, StringComparison.Ordinal);
            Assert.Contains("AchievementDescription(def)", panel, StringComparison.Ordinal);
            Assert.Contains("$\"achievement.{def.Id}.name\"", panel, StringComparison.Ordinal);
            Assert.Contains("$\"achievement.{def.Id}.description\"", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void StringsCsv_IdenticalEnglishGerman_AllowlistComplete()
        {
            // Task 10 — the ui.* identical set is reviewed and pinned exactly.
            var expected = new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal)
            {
                "ui.common.ok", "ui.hud.needs.hunger", "ui.hud.needs.morale",
                "ui.survivor.need.hunger", "ui.survivor.need.hygiene",
                "ui.survivor.info.name", "ui.survivor.status.fitness",
                "ui.survivor.fitness.fit", "ui.hud.radiation", "ui.hud.label.rad",
                "ui.status.day.weather_value", "ui.survivors.event.line",
                "ui.survivor.status.modifier_row", "ui.survivor.status.recent_row",
                "ui.shelter_hud.condition.hunger",
            };
            var actual = new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal);
            var lines = Read("assets/l10n/strings.csv").Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].TrimEnd('\r');
                if (line.Length == 0) continue;
                var cols = SplitCsvLine(line);
                if (cols.Length < 3 || !cols[0].StartsWith("ui.", StringComparison.Ordinal)) continue;
                if (cols[1] == cols[2]) actual.Add(cols[0]);
            }
            var unexpected = actual.Where(k => !expected.Contains(k)).ToList();
            var stale = expected.Where(k => !actual.Contains(k)).ToList();
            Assert.True(unexpected.Count == 0, "unallowlisted identical rows: " + string.Join(", ", unexpected));
            Assert.True(stale.Count == 0, "stale allowlist entries: " + string.Join(", ", stale));
        }

        [Fact]
        public void StringsCsv_SourceFileExistsForUiKeys()
        {
            // Task 11 — every ui.* row points at an existing host src/ file.
            string root = RepoRoot();
            var bad = new System.Collections.Generic.List<string>();
            var lines = Read("assets/l10n/strings.csv").Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].TrimEnd('\r');
                if (line.Length == 0) continue;
                var cols = SplitCsvLine(line);
                if (cols.Length < 4 || !cols[0].StartsWith("ui.", StringComparison.Ordinal)) continue;
                string source = cols[3].Trim();
                if (!source.StartsWith("src/", StringComparison.Ordinal))
                    bad.Add(cols[0] + " -> " + source);
                else if (!File.Exists(Path.Combine(root, source.Replace('/', Path.DirectorySeparatorChar))))
                    bad.Add(cols[0] + " -> missing " + source);
            }
            Assert.True(bad.Count == 0, "ui.* source problems: " + string.Join("; ", bad));
        }

        [Fact]
        public void LocalizedSurfaces_HaveNoRawTextAssignment()
        {
            // Task 12 — the registered panels have no bare Text/TooltipText literal.
            var pattern = new System.Text.RegularExpressions.Regex("\\b(Text|TooltipText)\\s*=\\s*\"[^\"$]*\"");
            foreach (string file in new[]
            {
                "src/UI/AchievementsPanel.cs", "src/UI/AfflictionsPanel.cs",
                "src/UI/MedicalPanel.cs", "src/UI/SurvivorDetailPanel.cs",
                "src/UI/SurvivalDetailPanel.cs",
            })
            {
                Assert.DoesNotMatch(pattern, Read(file));
            }
        }

        [Fact]
        public void CiReadme_DocumentsArtifactContract()
        {
            // Task 13 — the README documents ASHFALL_EXPECT_ARTIFACT.
            string readme = Read("scripts/ci/README.md");
            Assert.Contains("ASHFALL_EXPECT_ARTIFACT", readme, StringComparison.Ordinal);
            Assert.Contains("ui-layout-selftest.json", readme, StringComparison.Ordinal);
        }

        [Fact]
        public void GodotBoundedRunner_DocumentsEnvVars()
        {
            // Task 14 — the runner documents both environment contracts.
            string script = Read("scripts/ci/run-godot-bounded.sh");
            Assert.Contains("ASHFALL_SKIP_BUILD_STALENESS", script, StringComparison.Ordinal);
            Assert.Contains("ASHFALL_EXPECT_ARTIFACT", script, StringComparison.Ordinal);
            Assert.Contains("Environment:", script, StringComparison.Ordinal);
        }

        [Fact]
        public void UiLayoutArtifact_MtimeAfterRunStart()
        {
            // Task 15 — the wrapper captures the start epoch before the run and
            // compares the artifact mtime after it.
            string script = Read("scripts/ci/ui-layout-check.sh");
            int start = script.IndexOf("start_epoch=", StringComparison.Ordinal);
            int artifact = script.IndexOf("artifact_epoch=", StringComparison.Ordinal);
            Assert.True(start >= 0 && artifact > start, "wrapper does not order start_epoch before artifact_epoch");
            Assert.Contains("-lt \"$start_epoch\"", script, StringComparison.Ordinal);
        }

        // ── Tenth-wave loop gates ─────────────────────────────────────────

        [Fact]
        public void MedicalPanel_HasNoRawMetadataOrCareLiteral()
        {
            // Loop-1 hardening — empty-state metadata and care-log entries must
            // resolve from the catalog, never a raw literal.
            string panel = Read("src/UI/MedicalPanel.cs");
            Assert.DoesNotMatch(new System.Text.RegularExpressions.Regex("MakeMetadata\\(\"[A-Z]"), panel);
            Assert.DoesNotMatch(new System.Text.RegularExpressions.Regex("AddCareEntry\\([^,]+, \"[A-Z]"), panel);
        }

        [Fact]
        public void MedicalPanel_CareEntriesAndEmptyStates_AreLocalized()
        {
            string panel = Read("src/UI/MedicalPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.medical.care_bandage", "ui.medical.care_iodine", "ui.medical.care_anti_rad",
                "ui.medical.care_inhaler", "ui.medical.care_herbal_tea",
                "ui.medical.no_session", "ui.medical.no_treatment_ledger", "ui.medical.no_inventory",
                "ui.medical.no_health_readout", "ui.medical.no_chem_dependencies",
                "ui.medical.inventory_not_bound", "ui.medical.no_procedures",
                "ui.medical.disease_ward_offline", "ui.medical.no_infections",
                "ui.medical.active_penalties",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
        }

        [Fact]
        public void StatusPanel_ForecastKeys_AreLive()
        {
            // Loop-3 hardening — every ui.status.forecast.* catalog row must be
            // referenced by StatusPanel; a superseded row is dead weight.
            string status = Read("src/UI/StatusPanel.cs");
            var dead = new System.Collections.Generic.List<string>();
            var lines = Read("assets/l10n/strings.csv").Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].TrimEnd('\r');
                if (line.Length == 0) continue;
                var cols = SplitCsvLine(line);
                if (cols.Length < 1 || !cols[0].StartsWith("ui.status.forecast.", StringComparison.Ordinal)) continue;
                if (!status.Contains("\"" + cols[0] + "\"", StringComparison.Ordinal)) dead.Add(cols[0]);
            }
            Assert.True(dead.Count == 0, "dead forecast keys: " + string.Join(", ", dead));
        }

        // ── Eleventh-wave task gates ──────────────────────────────────────

        [Fact]
        public void MedicalVigilText_IsLocalized()
        {
            // Task 1 — the vigil line is formatted in the panels from the public
            // state machine; the host method is no longer consumed by the UI.
            string helper = Read("src/UI/MedicalVigilText.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.medical.vigil_idle", "ui.medical.vigil_left_early",
                "ui.medical.vigil_kept", "ui.medical.vigil_active",
                "ui.medical.vigil_phantom",
            })
            {
                Assert.Contains(key, helper, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.Contains("MedicalVigilText.Format", Read("src/UI/MedicalPanel.cs"), StringComparison.Ordinal);
            Assert.Contains("MedicalVigilText.Format", Read("src/UI/MedicalWardPanel.cs"), StringComparison.Ordinal);
            Assert.DoesNotContain("VigilStatusLine()", Read("src/UI/MedicalPanel.cs"), StringComparison.Ordinal);
        }

        [Fact]
        public void MedicalPanel_DiseaseRowsAndButtons_AreLocalized()
        {
            // Task 2 — disease rows, treatment buttons, and protocol rows resolve.
            string panel = Read("src/UI/MedicalPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.medical.identify", "ui.medical.isolate", "ui.medical.release",
                "ui.medical.apply", "ui.medical.affliction_unidentified",
                "ui.medical.affliction_row", "ui.medical.clinical_note",
                "ui.medical.treatment_ready", "ui.medical.no_supplies",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.DoesNotContain("MakeButton(\"IDENTIFY\"", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void AfflictionsPanel_SupplyRows_AreLocalized()
        {
            // Task 3 — treatment supply labels resolve from the catalog.
            string panel = Read("src/UI/AfflictionsPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.afflictions.supply_bandage", "ui.afflictions.supply_iodine",
                "ui.afflictions.supply_anti_rad", "ui.afflictions.supply_inhaler",
                "ui.afflictions.supply_herbal_tea", "ui.afflictions.supply_antibiotics",
                "ui.afflictions.supply_row",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
        }

        [Fact]
        public void AfflictionsPanel_ConditionNames_AreLocalized()
        {
            // Task 4 — the hardcoded condition rows resolve from the catalog.
            string panel = Read("src/UI/AfflictionsPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.afflictions.acute_rad", "ui.afflictions.severe_respiratory",
                "ui.afflictions.respiratory_irritation", "ui.afflictions.clinical_note",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.DoesNotContain("Acute radiation sickness (dose", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void StartingCohortSetupPanel_ChromeAndNotes_AreLocalized()
        {
            // Task 5 — title, headers, buttons, and condition notes resolve.
            string panel = Read("src/UI/StartingCohortSetupPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.cohort.title", "ui.cohort.subtitle", "ui.cohort.section.cohort",
                "ui.cohort.section.stores", "ui.cohort.section.difficulty",
                "ui.cohort.cancel", "ui.cohort.start", "ui.cohort.note.acute_rad",
                "ui.cohort.note.injured", "ui.cohort.note.strained", "ui.cohort.note.stable",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.DoesNotContain("MakeTitle(\"EXPEDITION ROSTER COMMISSION\")", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void StartingCohortSetupPanel_UsesNamedStrainThreshold()
        {
            // Task 6 — the setup-preview band is a named constant, not a literal.
            string panel = Read("src/UI/StartingCohortSetupPanel.cs");
            Assert.Contains("StartingStrainPreviewThreshold", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("hunger >= 36f", panel, StringComparison.Ordinal);
            Assert.DoesNotContain("thirst >= 36f", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void ShelterHudPanel_ConditionLabels_AreLocalized()
        {
            // Task 7 — the condition grid labels resolve from the catalog.
            string panel = Read("src/UI/ShelterHudPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.shelter_hud.condition.health", "ui.shelter_hud.condition.radiation",
                "ui.shelter_hud.condition.hunger", "ui.shelter_hud.condition.thirst",
                "ui.shelter_hud.condition.roster",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.DoesNotContain("BuildConditionRow(\"HEALTH\"", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void GameDashboardPanel_DirectiveAndStatus_AreLocalized()
        {
            // Task 8 — the directive and shelter-status strings resolve.
            string panel = Read("src/UI/GameDashboardPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.dashboard.memorialized", "ui.dashboard.youth", "ui.dashboard.unassigned",
                "ui.dashboard.duty_intake", "ui.dashboard.hold_hatch", "ui.dashboard.air_warning",
                "ui.dashboard.rations_next", "ui.dashboard.keep_quiet",
                "ui.dashboard.next_shift.hold", "ui.dashboard.next_shift.open",
                "ui.dashboard.hatch.sealed_hazard", "ui.dashboard.hatch.sealed",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
        }

        [Fact]
        public void UiSources_DoNotRetypeSetupBandLiteral()
        {
            // Task 9 — the cohort-setup strain band is a named constant.
            string uiDir = Path.Combine(RepoRoot(), "src", "UI");
            var offenders = new System.Collections.Generic.List<string>();
            var pattern = new System.Text.RegularExpressions.Regex(@"([Hh]unger|[Tt]hirst)\s*(>=|>)\s*36");
            foreach (string file in Directory.GetFiles(uiDir, "*.cs", SearchOption.AllDirectories))
            {
                string src = File.ReadAllText(file);
                foreach (System.Text.RegularExpressions.Match m in pattern.Matches(src))
                    offenders.Add(Path.GetFileName(file) + ":" + m.Value);
            }
            Assert.True(offenders.Count == 0, "re-typed setup band literals: " + string.Join(", ", offenders));
        }

        [Fact]
        public void StringsCsv_CuratedPrefixesHaveNoDeadKey()
        {
            // Task 10 / loop — the curated prefix families are fully live in the
            // file their own source column names, so a superseded row in any of
            // them fails instead of lingering unnoticed. Keys required by the
            // expedition presence guard are exempt (reserved, not superseded).
            string[] prefixes =
            {
                "ui.cohort.", "ui.shelter_hud.", "ui.dashboard.", "ui.ward.",
                "ui.medical.section.", "ui.expedition.",
            };
            var reserved = new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal);
            foreach (System.Text.RegularExpressions.Match m in
                System.Text.RegularExpressions.Regex.Matches(
                    Read("Ashfall.Core.Tests/Localization/ExpeditionLocaleKeysTests.cs"),
                    "\\\"(ui\\.expedition\\.[A-Za-z0-9_.]+)\\\""))
                reserved.Add(m.Groups[1].Value);
            var dead = new System.Collections.Generic.List<string>();
            var lines = Read("assets/l10n/strings.csv").Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].TrimEnd('\r');
                if (line.Length == 0) continue;
                var cols = SplitCsvLine(line);
                if (cols.Length < 4) continue;
                foreach (string prefix in prefixes)
                {
                    if (!cols[0].StartsWith(prefix, StringComparison.Ordinal)) continue;
                    if (reserved.Contains(cols[0])) continue;
                    string source = cols[3].Trim();
                    if (!source.StartsWith("src/", StringComparison.Ordinal)) continue;
                    if (!Read(source).Contains("\"" + cols[0] + "\"", StringComparison.Ordinal))
                        dead.Add(cols[0] + " -> " + source);
                }
            }
            Assert.True(dead.Count == 0, "dead curated keys: " + string.Join("; ", dead));
        }

        [Fact]
        public void LocalizedSurfaces_RegisteredInDriftGate()
        {
            // Task 11 — every UI file that resolves through AshfallUiText is
            // registered with the drift gate.
            string gate = Read("scripts/ci/l10n_drift_gate.py");
            string uiDir = Path.Combine(RepoRoot(), "src", "UI");
            var missing = new System.Collections.Generic.List<string>();
            foreach (string file in Directory.GetFiles(uiDir, "*.cs", SearchOption.TopDirectoryOnly))
            {
                string name = Path.GetFileName(file);
                if (name == "AshfallUiText.cs") continue;
                string src = File.ReadAllText(file);
                if (!src.Contains("AshfallUiText.Tr", StringComparison.Ordinal)) continue;
                if (!gate.Contains(name, StringComparison.Ordinal)) missing.Add(name);
            }
            Assert.True(missing.Count == 0, "unregistered localized surfaces: " + string.Join(", ", missing));
        }

        [Fact]
        public void StringsCsv_GermanDuplicates_StayBounded()
        {
            // Task 12 — a German string legitimately repeats across panels, but a
            // runaway copy-paste is a smell; pin the current ceiling.
            var counts = new System.Collections.Generic.Dictionary<string, int>(StringComparer.Ordinal);
            var lines = Read("assets/l10n/strings.csv").Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].TrimEnd('\r');
                if (line.Length == 0) continue;
                var cols = SplitCsvLine(line);
                if (cols.Length < 3 || cols[2].Length == 0) continue;
                counts.TryGetValue(cols[2], out int n);
                counts[cols[2]] = n + 1;
            }
            int max = 0;
            string top = "";
            foreach (var pair in counts)
                if (pair.Value > max) { max = pair.Value; top = pair.Key; }
            Assert.True(max <= 4, $"German duplicate ceiling exceeded: {top}={max} (pin 4)");
        }

        [Fact]
        public void CiReadme_DocumentsLocalizedSurfaceRegistry()
        {
            // Task 13 — the README names the registry and its enforcement gate.
            string readme = Read("scripts/ci/README.md");
            Assert.Contains("LOCALIZED_SURFACES", readme, StringComparison.Ordinal);
            Assert.Contains("LocalizedSurfaces_RegisteredInDriftGate", readme, StringComparison.Ordinal);
        }

        [Fact]
        public void GodotBoundedRunner_HasStalenessOnlyMode()
        {
            // Task 14 — the runner can check staleness without booting Godot.
            string script = Read("scripts/ci/run-godot-bounded.sh");
            Assert.Contains("--check-staleness-only", script, StringComparison.Ordinal);
            Assert.Contains("STALENESS_ONLY", script, StringComparison.Ordinal);
        }

        [Fact]
        public void UiLayoutArtifact_FailureCountField()
        {
            // Task 15 — the artifact's failures field is an unquoted integer.
            string host = Read("src/Host/HostCli.Command.RunUiLayoutSelfTest.cs");
            Assert.Contains("\\\"failures\\\":{failures}", host, StringComparison.Ordinal);
        }

        // ── Eleventh-wave loop gates ──────────────────────────────────────

        [Fact]
        public void MedicalPanel_HasNoRawChromeLiteral()
        {
            // Loop-1 hardening — the medical panel chrome resolves from the catalog.
            string panel = Read("src/UI/MedicalPanel.cs");
            Assert.DoesNotMatch(new System.Text.RegularExpressions.Regex(
                "(MakeMetadata|MakeDimLine|MakeSmall|MakeBody|MakeSectionHeader|MakeTitle|MakeButton)\\(\\\"[A-Za-z]"), panel);
        }

        [Fact]
        public void MedicalWardPanel_Chrome_IsLocalized()
        {
            // Loop-2 repair — the ward chrome resolves from the catalog.
            string panel = Read("src/UI/MedicalWardPanel.cs");
            string csv = Read("assets/l10n/strings.csv");
            foreach (string key in new[]
            {
                "ui.ward.section.matrix", "ui.ward.section.telemetry", "ui.ward.section.catalog",
                "ui.ward.no_events", "ui.ward.bed_unit", "ui.ward.run", "ui.ward.vacant",
                "ui.ward.select_bed", "ui.ward.section.procedures", "ui.ward.system",
                "ui.ward.section.recent", "ui.ward.no_procedures", "ui.ward.discharge",
            })
            {
                Assert.Contains(key, panel, StringComparison.Ordinal);
                Assert.Contains(key + ",", csv, StringComparison.Ordinal);
            }
            Assert.DoesNotContain("MakeSectionHeader(\"WARD BED MATRIX\")", panel, StringComparison.Ordinal);
        }

        [Fact]
        public void MedicalWardPanel_HasNoRawChromeLiteral()
        {
            // Loop-2 hardening — no bare chrome literal remains in the ward.
            string panel = Read("src/UI/MedicalWardPanel.cs");
            Assert.DoesNotMatch(new System.Text.RegularExpressions.Regex(
                "(MakeMetadata|MakeDimLine|MakeSmall|MakeBody|MakeSectionHeader|MakeSubsectionHeader|MakeTitle|MakeButton)\\(\\\"[A-Za-z]"), panel);
        }

        [Fact]
        public void CiReadme_DocumentsStalenessOnlyMode()
        {
            // Loop-3 — the CI-friendly staleness-only mode is documented.
            string readme = Read("scripts/ci/README.md");
            Assert.Contains("--check-staleness-only", readme, StringComparison.Ordinal);
        }

        // ── Twelfth-wave task gates ───────────────────────────────────────

        [Fact]
        public void UiSources_DoNotRetypeWardBandLiteral()
        {
            // Task 9 — the ward reads no re-typed health/radiation bands.
            string panel = Read("src/UI/MedicalWardPanel.cs");
            Assert.DoesNotMatch(new System.Text.RegularExpressions.Regex(
                "(Health|health|Radiation|radiation|[Dd]ose)\\s*(<|<=|>=|>)\\s*[0-9]"), panel);
        }

        [Fact]
        public void StringsCsv_SourceColumnExistsForRegisteredPanels()
        {
            // Task 11 — every panel registered with the drift gate exists, so a
            // renamed panel leaves no dangling gate entry.
            string gate = Read("scripts/ci/l10n_drift_gate.py");
            string root = Path.Combine(RepoRoot(), "src", "UI");
            var missing = new System.Collections.Generic.List<string>();
            foreach (System.Text.RegularExpressions.Match m in
                System.Text.RegularExpressions.Regex.Matches(
                    gate, "ROOT\\s*/\\s*\\\"src\\\"\\s*/\\s*\\\"UI\\\"\\s*/\\s*\\\"([A-Za-z0-9_]+\\.cs)\\\""))
            {
                string name = m.Groups[1].Value;
                if (!File.Exists(Path.Combine(root, name))) missing.Add(name);
            }
            Assert.True(missing.Count == 0, "dangling registered panels: " + string.Join(", ", missing));
        }

        [Fact]
        public void LocalizedSurfaces_CountStaysBounded()
        {
            // Task 12 — the registered-surface registry must not shrink silently.
            string gate = Read("scripts/ci/l10n_drift_gate.py");
            int count = System.Text.RegularExpressions.Regex.Matches(gate, "ROOT / \\\"src\\\" / \\\"UI\\\" /").Count;
            Assert.True(count >= 20, $"registered localized surfaces shrank to {count} (floor 20)");
        }

        [Fact]
        public void CiReadme_DocumentsGermanDuplicateBound()
        {
            // Task 13 — the German-duplicate bound is documented.
            string readme = Read("scripts/ci/README.md");
            Assert.Contains("StringsCsv_GermanDuplicates_StayBounded", readme, StringComparison.Ordinal);
        }

        [Fact]
        public void UiLayoutArtifact_FailureCountMatchesStatus()
        {
            // Task 15 — the artifact status is derived from the failure count, so
            // PASS always means failures==0.
            string host = Read("src/Host/HostCli.Command.RunUiLayoutSelfTest.cs");
            Assert.Contains("failures == 0 ? \"PASS\" : \"FAIL\"", host, StringComparison.Ordinal);
        }
    }
}
