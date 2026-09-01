// SPDX-License-Identifier: MIT
// Plan 60 / D1 + D5 — clinical stage, band and palliative plan are DERIVED from
// the authored catalog, and the sick list records which authority named a row.
// Before this, ResolveOutcomes ignored everything but illness_days + lethality,
// the sick list only ever meant "dose band", and palliativePlan had no writer.
// These tests pin the derived model and reject an invented parallel timeline.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public class DiseaseTriageBridgeTests
    {
        // ── fixture helpers ────────────────────────────────────────────

        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("data authority not found from " + start);
        }

        private static DiseaseCatalog LoadCatalog() =>
            DiseaseCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

        private static DiseaseDefinition Def(
            int incubation = 2, int illness = 8, float lethality = 0.4f) =>
            new DiseaseDefinition
            {
                id = "disease_test_fixture",
                display_name = "Fixture illness",
                vector = DiseaseVectorNames.Water,
                incubation_days = incubation,
                illness_days = illness,
                lethality = lethality,
                infectivity = 0.3f,
            };

        // ── D1: stages are derived, never authored ────────────────────

        [Fact]
        public void StageOf_DerivesIncubationIllnessTerminalFromCatalogBounds()
        {
            var def = Def(incubation: 2, illness: 8, lethality: 0.6f);

            Assert.Equal(DiseaseClinicalStage.None, DiseaseTriage.StageOf(null, 5));
            Assert.Equal(DiseaseClinicalStage.Incubating, DiseaseTriage.StageOf(def, 0));
            Assert.Equal(DiseaseClinicalStage.Incubating, DiseaseTriage.StageOf(def, 1));
            Assert.Equal(DiseaseClinicalStage.Ill, DiseaseTriage.StageOf(def, 2));
            Assert.Equal(DiseaseClinicalStage.Terminal, DiseaseTriage.StageOf(def, 7));
            Assert.Equal(DiseaseClinicalStage.OutcomePending, DiseaseTriage.StageOf(def, 8));
        }

        [Theory]
        [InlineData(-5)]
        [InlineData(int.MinValue)]
        public void StageOf_NegativeDays_IsNeverAPrognosis(int daysSick)
        {
            Assert.Equal(DiseaseClinicalStage.None, DiseaseTriage.StageOf(Def(), daysSick));
            Assert.False(DiseaseTriage.IsTerminalPrognosis(Def(), daysSick));
            Assert.Null(DiseaseTriage.PalliativePlanFor(Def(), daysSick));
            Assert.Equal(DoseLedgerSystem.BandGreen, DiseaseTriage.SickBandFor(Def(), daysSick));
        }

        [Fact]
        public void TerminalPrognosis_NeverAppliesToLowLethalityDisease()
        {
            // Self-limiting: 10% lethal. Late illness is still not a comfort-care case.
            var mild = Def(incubation: 1, illness: 10, lethality: 0.10f);

            Assert.False(DiseaseTriage.IsTerminalPrognosis(mild, 10));
            Assert.Null(DiseaseTriage.PalliativePlanFor(mild, 9));
            Assert.Equal(DiseaseClinicalStage.Ill, DiseaseTriage.StageOf(mild, 9));
        }

        [Fact]
        public void TerminalPrognosis_KeepsOneDayOfAcuteTreatmentRoom()
        {
            // Even a maximally lethal disease must be treatable as acute for at
            // least one ill day before comfort care becomes the honest plan.
            var harsh = Def(incubation: 0, illness: 1, lethality: 0.9f);

            Assert.Equal(DiseaseClinicalStage.Ill, DiseaseTriage.StageOf(harsh, 0));
            Assert.Equal(DiseaseClinicalStage.OutcomePending, DiseaseTriage.StageOf(harsh, 1));
            Assert.False(DiseaseTriage.IsTerminalPrognosis(harsh, 0));
        }

        // ── D5: one band ladder, illness-named rows ────────────────────

        [Fact]
        public void SickBandFor_MonotonicThroughIllnessToOutcomePending()
        {
            var def = Def(incubation: 2, illness: 10, lethality: 0.8f);

            int ill = DiseaseTriage.SickBandFor(def, 3);
            int terminal = DiseaseTriage.SickBandFor(def, 9);
            int pending = DiseaseTriage.SickBandFor(def, 10);

            Assert.Equal(DoseLedgerSystem.BandAmber, ill);
            Assert.Equal(DoseLedgerSystem.BandRed, terminal);
            Assert.Equal(DoseLedgerSystem.BandBlack, pending);
            Assert.True(ill < terminal && terminal < pending);
        }

        [Fact]
        public void ShouldNameToSickList_IncubationIsAQuarantineQuestionNotTriage()
        {
            var def = Def(incubation: 3, illness: 9, lethality: 0.9f);

            Assert.False(DiseaseTriage.ShouldNameToSickList(def, 2));
            Assert.True(DiseaseTriage.ShouldNameToSickList(def, 3));
            Assert.False(DiseaseTriage.ShouldNameToSickList(null, 5));
        }

        [Fact]
        public void PalliativePlan_UsesAuthoredRegisterPlanIdsOnly()
        {
            var heavy = Def(incubation: 1, illness: 8, lethality: 0.9f);
            var light = Def(incubation: 1, illness: 8, lethality: 0.3f);

            Assert.Equal(DiseaseTriage.Plans.MorphineTray,
                DiseaseTriage.PalliativePlanFor(heavy, 7));
            Assert.Equal(DiseaseTriage.Plans.ComfortRounds,
                DiseaseTriage.PalliativePlanFor(light, 7));

            // Every plan the triage can emit must exist in the register authority.
            var registerIds = RegisterPlanIds();
            Assert.Contains(DiseaseTriage.Plans.MorphineTray, registerIds);
            Assert.Contains(DiseaseTriage.Plans.ComfortRounds, registerIds);
        }

        private static List<string> RegisterPlanIds()
        {
            var doc = System.Text.Json.JsonDocument.Parse(
                new FileSystemIO().ReadAllText(Path.Combine(DataDir(), "dose_registers.json")));
            var ids = new List<string>();
            foreach (var p in doc.RootElement.GetProperty("plans").EnumerateArray())
                ids.Add(p.GetProperty("id").GetString());
            return ids;
        }

        // ── the real catalog must survive the derived model ────────────

        [Fact]
        public void AuthoredCatalog_ProducesSaneStagesBandsAndPlans()
        {
            var catalog = LoadCatalog();
            var diseases = catalog.Diseases;
            Assert.NotEmpty(diseases);

            int terminalCapable = 0;
            foreach (var def in diseases)
            {
                Assert.NotNull(def.id);

                // Stage is monotonic in days for every authored disease.
                var previous = DiseaseTriage.StageOf(def, 0);
                for (int d = 1; d <= def.illness_days + 2; d++)
                {
                    var now = DiseaseTriage.StageOf(def, d);
                    Assert.True((int)now >= (int)previous,
                        $"{def.id} stage regressed from {previous} to {now} at day {d}");
                    previous = now;
                }

                Assert.Equal(DoseLedgerSystem.BandBlack,
                    DiseaseTriage.SickBandFor(def, def.illness_days));

                var lastIllDay = Math.Max(def.incubation_days, def.illness_days - 1);
                var plan = DiseaseTriage.PalliativePlanFor(def, lastIllDay);
                if (plan != null)
                {
                    terminalCapable++;
                    Assert.Contains(plan, RegisterPlanIds());
                }

                Assert.DoesNotContain(DiseaseTriage.StageToken(
                    DiseaseTriage.StageOf(def, lastIllDay)), new[] { "", "none" });
            }

            // A catalog where nothing is ever terminal is as unreadable as one
            // where everything is: at least one authored path must reach comfort care.
            Assert.True(terminalCapable > 0,
                "no authored disease ever reaches a terminal prognosis");
        }

        // ── sick list: named source, additive, round-trips ─────────────

        [Fact]
        public void Diagnose_LegacySignatureStaysDoseSourced()
        {
            var list = new SickListSystem();
            list.Diagnose("sv_a", DoseLedgerSystem.BandRed, day: 10);

            var band = list.GetBand("sv_a");
            Assert.Equal(SickListSystem.SourceDose, band.severitySource);
            Assert.Equal(string.Empty, band.sourceId);
        }

        [Fact]
        public void Diagnose_IllnessSourceRecordsDiseaseProvenance()
        {
            var list = new SickListSystem();
            list.Diagnose("sv_a", DoseLedgerSystem.BandAmber, day: 10,
                SickListSystem.SourceIllness, "disease_cholera");

            var band = list.GetBand("sv_a");
            Assert.Equal(SickListSystem.SourceIllness, band.severitySource);
            Assert.Equal("disease_cholera", band.sourceId);
        }

        [Fact]
        public void SickList_NewFieldsRoundTripThroughCapture()
        {
            var list = new SickListSystem();
            list.Diagnose("sv_b", DoseLedgerSystem.BandBlack, day: 40,
                SickListSystem.SourceIllness, "disease_spore_blight");
            list.AssignPalliative("sv_b", DiseaseTriage.Plans.MorphineTray);

            var restored = new SickListSystem();
            restored.RestoreState(list.CaptureState());

            var band = restored.GetBand("sv_b");
            Assert.Equal(SickListSystem.SourceIllness, band.severitySource);
            Assert.Equal("disease_spore_blight", band.sourceId);
            Assert.Equal(DiseaseTriage.Plans.MorphineTray, band.palliativePlan);
            Assert.Equal(DoseLedgerSystem.BandBlack, band.band);
        }

        [Fact]
        public void SickList_PreD5RowsLoadAsDoseSourced()
        {
            // A save written before the fields existed has null severitySource.
            var legacy = new SickListSystemState();
            legacy.bands.Add(new SickBand
            {
                survivorId = "sv_c",
                band = DoseLedgerSystem.BandRed,
                diagnosedDay = 12,
                releaseDay = -1,
                palliativePlan = string.Empty,
            });

            var list = new SickListSystem();
            list.RestoreState(legacy);

            Assert.Equal(SickListSystem.SourceDose, list.GetBand("sv_c").severitySource);
        }

        [Fact]
        public void Release_OnlyTouchesRowsTheDiseaseBridgeNamed()
        {
            var list = new SickListSystem();
            list.Diagnose("sv_ill", DoseLedgerSystem.BandRed, day: 5,
                SickListSystem.SourceIllness, "disease_cholera");
            list.Diagnose("sv_dose", DoseLedgerSystem.BandBlack, day: 5);

            Assert.True(list.Release("sv_ill", day: 9));
            var doseRow = list.GetBand("sv_dose");

            // Dose-named rows are the dose ledger's business, not triage's.
            Assert.Equal(SickListSystem.SourceDose, doseRow.severitySource);
            Assert.Equal(-1, doseRow.releaseDay);
        }

        [Fact]
        public void CaptureState_IsOrdinalAndIndependentOfLiveState()
        {
            var list = new SickListSystem();
            list.Diagnose("sv_z", 1, 2, SickListSystem.SourceIllness, "disease_a");
            list.Diagnose("sv_a", 1, 2, SickListSystem.SourceIllness, "disease_b");

            var capture = list.CaptureState();
            Assert.Equal(new[] { "sv_a", "sv_z" }, capture.bands.Select(b => b.survivorId).ToArray());

            capture.bands[0].band = DoseLedgerSystem.BandBlack;
            Assert.NotEqual(DoseLedgerSystem.BandBlack, list.GetBand("sv_a").band);
        }
    }
}
