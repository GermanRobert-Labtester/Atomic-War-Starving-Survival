// SPDX-License-Identifier: MIT
// Plan 60 / D3 — treatment as an intervention. The disease engine used to roll the
// raw authored lethality no matter what the player did, so medicine could not be
// practised: only vector prevention existed. These tests pin the clinical contract
// on the Core side (the host side is covered by --dose-uitest): an item must be
// authorised for that disease, the window must be honoured, one dose per patient
// per day, only a curative role clears an infection, supply is spent exactly once,
// and an unwired host fails loudly instead of pretending.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public class DiseaseTreatmentTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("data authority not found from " + start);
        }

        private static DiseaseCatalog LoadCatalog() =>
            DiseaseCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

        private static DiseaseCatalog CatalogWith(
            string diseaseId, string vector, int incubation, int illness, float lethality,
            params (string item, string role, int maxDays, float reduction)[] treatments)
        {
            var catalog = new DiseaseCatalog();
            var def = new DiseaseDefinition
            {
                id = diseaseId,
                display_name = "Fixture illness",
                vector = vector,
                incubation_days = incubation,
                illness_days = illness,
                lethality = lethality,
                infectivity = 0f,
            };
            foreach (var t in treatments)
                def.treatments.Add(new DiseaseTreatment
                {
                    item_id = t.item, role = t.role, max_days = t.maxDays,
                    lethality_reduction = t.reduction,
                });
            catalog.Add(def);
            return catalog;
        }

        private static DiseaseSystem SystemWith(
            DiseaseCatalog catalog, Func<string, int, bool> supply = null, int seed = 2027)
        {
            var system = new DiseaseSystem(rng: new SeededRng(seed), log: NullLog.Instance);
            system.BindCatalog(catalog);
            system.TryConsumeItem = supply ?? ((_, __) => true);
            return system;
        }

        private static DiseaseCatalog Fixture(float lethality = 0.6f) => CatalogWith(
            "disease_fixture_ars", DiseaseVectorNames.Water, incubation: 0, illness: 12,
            lethality: lethality,
            ("antibiotics", DiseaseTreatmentRoles.Curative, maxDays: 2, reduction: 0.4f),
            ("rad_away", DiseaseTreatmentRoles.Suppressive, maxDays: 5, reduction: 0.15f),
            ("bandage", DiseaseTreatmentRoles.Symptomatic, maxDays: 0, reduction: 0f));

        // ── refusal is a stated reason, never a silent no-op ───────────

        [Fact]
        public void Treat_UnknownDisease_IsRefused()
        {
            var s = SystemWith(Fixture());
            s.Infect("sv_a", "disease_fixture_ars", day: 1);

            var r = s.TryTreat("sv_a", "disease_not_authored", "antibiotics", day: 1);

            Assert.False(r.Accepted);
            Assert.Equal(DiseaseTreatmentRefusals.UnknownDisease, r.Reason);
        }

        [Fact]
        public void Treat_NonPatient_IsRefused()
        {
            var s = SystemWith(Fixture());
            Assert.Equal(DiseaseTreatmentRefusals.NotPatient,
                s.TryTreat("sv_ghost", "disease_fixture_ars", "antibiotics", 1).Reason);
        }

        [Fact]
        public void Treat_UnauthorisedItem_IsRefusedWithoutSpending()
        {
            int spent = 0;
            var s = SystemWith(Fixture(), (_, __) => { spent++; return true; });
            s.Infect("sv_a", "disease_fixture_ars", day: 1);

            var r = s.TryTreat("sv_a", "disease_fixture_ars", "morphine", 1);

            Assert.False(r.Accepted);
            Assert.Equal(DiseaseTreatmentRefusals.ItemNotAuthorised, r.Reason);
            Assert.Equal(0, spent);
        }

        [Fact]
        public void Treat_DiseaseWithoutAuthoredTreatment_IsRefused()
        {
            var catalog = CatalogWith("disease_incurable", DiseaseVectorNames.Spore, 1, 6, 0.7f);
            var s = SystemWith(catalog);
            s.Infect("sv_a", "disease_incurable", day: 1);

            Assert.Equal(DiseaseTreatmentRefusals.NoTreatmentAuthorised,
                s.TryTreat("sv_a", "disease_incurable", "antibiotics", 1).Reason);
        }

        [Fact]
        public void Treat_WithoutSupplyChannel_RefusesInsteadOfPretending()
        {
            var s = SystemWith(Fixture(), supply: null);
            s.TryConsumeItem = null;
            s.Infect("sv_a", "disease_fixture_ars", day: 1);

            Assert.Equal(DiseaseTreatmentRefusals.NoSupplyChannel,
                s.TryTreat("sv_a", "disease_fixture_ars", "antibiotics", 1).Reason);
        }

        [Fact]
        public void Treat_OutOfStock_RefusesWithSupplyUnavailable()
        {
            var s = SystemWith(Fixture(), (_, __) => false);
            s.Infect("sv_a", "disease_fixture_ars", day: 1);

            Assert.Equal(DiseaseTreatmentRefusals.SupplyUnavailable,
                s.TryTreat("sv_a", "disease_fixture_ars", "antibiotics", 1).Reason);
            Assert.True(s.IsInfected("sv_a", "disease_fixture_ars"));
        }

        // ── windows ────────────────────────────────────────────────────

        [Fact]
        public void Treat_LateBeyondMaxDays_IsRefusedButSupportiveCareStillCounts()
        {
            var s = SystemWith(Fixture());
            s.Infect("sv_a", "disease_fixture_ars", day: 1);
            s.Infect("sv_b", "disease_fixture_ars", day: 1);

            // Antibiotics are authorised to day 2 only; bandaging is always allowed.
            Assert.True(s.TryTreat("sv_a", "disease_fixture_ars", "antibiotics", day: 1).Accepted);
            for (int day = 2; day <= 5; day++) s.TickDaily(day, candidates: null);

            Assert.Equal(DiseaseTreatmentRefusals.OutsideWindow,
                s.TryTreat("sv_b", "disease_fixture_ars", "antibiotics", day: 5).Reason);
            Assert.True(s.TryTreat("sv_b", "disease_fixture_ars", "bandage", day: 5).Accepted);
        }

        // ── dosing discipline ──────────────────────────────────────────

        [Fact]
        public void Treat_OneAcceptedDosePerPatientPerDay()
        {
            int spent = 0;
            var s = SystemWith(Fixture(), (_, __) => { spent++; return true; });
            s.Infect("sv_a", "disease_fixture_ars", day: 1);

            Assert.True(s.TryTreat("sv_a", "disease_fixture_ars", "rad_away", 1).Accepted);
            var repeat = s.TryTreat("sv_a", "disease_fixture_ars", "rad_away", 1);

            Assert.False(repeat.Accepted);
            Assert.Equal(DiseaseTreatmentRefusals.AlreadyTreatedToday, repeat.Reason);
            Assert.Equal(1, spent);

            // The next day a dose is available again — the limit is spam, not scarcity.
            Assert.True(s.TryTreat("sv_a", "disease_fixture_ars", "rad_away", 2).Accepted);
            Assert.Equal(2, spent);
        }

        // ── roles do what their names say ──────────────────────────────

        [Fact]
        public void CurativeRemovesInfection_AndCountsAsRecovered()
        {
            var s = SystemWith(Fixture());
            s.Infect("sv_a", "disease_fixture_ars", day: 1);
            List<(string, string, bool)> outcomes = new List<(string, string, bool)>();
            s.OnOutcomeResolved += (sv, d, rec) => outcomes.Add((sv, d, rec));

            var r = s.TryTreat("sv_a", "disease_fixture_ars", "antibiotics", 1);

            Assert.True(r.Accepted);
            Assert.True(r.Cured);
            Assert.False(s.IsInfected("sv_a", "disease_fixture_ars"));
            Assert.Single(outcomes);
            Assert.True(outcomes[0].Item3);
            Assert.Equal(1, s.GetDiseaseState("disease_fixture_ars").recovered_total);
        }

        [Theory]
        [InlineData(DiseaseTreatmentRoles.Suppressive)]
        [InlineData(DiseaseTreatmentRoles.Symptomatic)]
        [InlineData(DiseaseTreatmentRoles.Supportive)]
        public void NonCurativeRoles_NeverClearAnInfection(string role)
        {
            var catalog = CatalogWith("disease_fixture_ars", DiseaseVectorNames.Water,
                0, 12, 0.6f, ("the_only_drug", role, 0, 0.2f));
            var s = SystemWith(catalog);
            s.Infect("sv_a", "disease_fixture_ars", day: 1);

            var r = s.TryTreat("sv_a", "disease_fixture_ars", "the_only_drug", 1);

            Assert.True(r.Accepted);
            Assert.False(r.Cured);
            Assert.True(s.IsInfected("sv_a", "disease_fixture_ars"));
        }

        [Fact]
        public void TreatmentImprovesThisPatientsOddsOnly()
        {
            var s = SystemWith(Fixture(lethality: 0.6f));
            s.Infect("sv_a", "disease_fixture_ars", day: 1);
            s.Infect("sv_b", "disease_fixture_ars", day: 1);

            Assert.Equal(0.6f, s.GetEffectiveLethality("sv_a", "disease_fixture_ars"), 3);
            s.TryTreat("sv_a", "disease_fixture_ars", "rad_away", 1);

            Assert.Equal(0.45f, s.GetEffectiveLethality("sv_a", "disease_fixture_ars"), 3);
            Assert.Equal(0.6f, s.GetEffectiveLethality("sv_b", "disease_fixture_ars"), 3);
        }

        [Fact]
        public void CumulativeReduction_IsCappedSoTreatmentCannotGrantImmortality()
        {
            var catalog = CatalogWith("disease_fixture_ars", DiseaseVectorNames.Water,
                0, 40, 0.95f, ("sedative", DiseaseTreatmentRoles.Supportive, 0, 0.2f));
            var s = SystemWith(catalog);
            s.Infect("sv_a", "disease_fixture_ars", day: 1);

            for (int day = 1; day <= 60; day++)
                s.TryTreat("sv_a", "disease_fixture_ars", "sedative", day);

            float effective = s.GetEffectiveLethality("sv_a", "disease_fixture_ars");
            Assert.True(effective > 0f, "repeated care must not make death impossible");
            Assert.InRange(effective, 0.95f - DiseaseSystem.MaxLethalityReduction - 0.001f,
                0.95f - DiseaseSystem.MaxLethalityReduction + 0.001f);
        }

        // ── persistence + the shipped catalog ──────────────────────────

        [Fact]
        public void TreatmentHistory_RoundTripsThroughTheSave()
        {
            var s = SystemWith(Fixture());
            s.Infect("sv_a", "disease_fixture_ars", day: 1);
            s.TryTreat("sv_a", "disease_fixture_ars", "rad_away", 1);

            var state = new SystemTextJsonSerializer()
                .Deserialize<DiseaseSystemState>(
                    new SystemTextJsonSerializer().Serialize(s.CaptureState()));

            var reloaded = SystemWith(Fixture(), supply: null, seed: 5);
            reloaded.RestoreState(state);

            var patient = reloaded.GetDiseaseState("disease_fixture_ars").infected
                .Single(p => p.survivor_id == "sv_a");
            Assert.Equal(1, patient.treatments_applied);
            Assert.Equal(1, patient.last_treatment_day);
            Assert.Equal(0.45f, reloaded.GetEffectiveLethality("sv_a", "disease_fixture_ars"), 3);
        }

        [Fact]
        public void AuthoredCatalog_DoesNotDefineMorphineAsAnItem()
        {
            // Guard against the register plan id "plan_morphine_tray" being mistaken
            // for a consumable item id in a treatment entry.
            var catalog = LoadCatalog();
            var items = new HashSet<string>(StringComparer.Ordinal);
            foreach (var line in File.ReadLines(Path.Combine(DataDir(), "items.json")))
            {
                int idx = line.IndexOf("\"id\"", StringComparison.Ordinal);
                if (idx < 0) continue;
                int start = line.IndexOf('"', idx + 5);
                int end = line.IndexOf('"', start + 1);
                if (start > 0 && end > start) items.Add(line.Substring(start + 1, end - start - 1));
            }

            foreach (var d in catalog.Diseases)
                foreach (var t in d.treatments)
                    Assert.True(items.Contains(t.item_id),
                        $"{d.id} authorises treatment item '{t.item_id}' that is not in items.json");
        }

        [Fact]
        public void AuthoredCatalog_KeepsRoleAndWindowDiscipline()
        {
            var catalog = LoadCatalog();

            Assert.All(catalog.Diseases, d => Assert.All(d.treatments, t =>
            {
                Assert.True(DiseaseTreatmentRoles.IsKnown(t.role), $"unknown role {t.role} on {d.id}");
                Assert.InRange(t.lethality_reduction, 0f, 1f);
                Assert.True(t.max_days >= 0);
                Assert.False(string.IsNullOrEmpty(t.item_id));
            }));

            // Windows must be tighter than the illness itself, or they are decoration.
            foreach (var d in catalog.Diseases)
                foreach (var t in d.treatments.Where(t => t.max_days > 0))
                    Assert.True(t.max_days < d.illness_days,
                        $"{d.id}: treatment window {t.max_days} is not narrower than illness {d.illness_days}");

            // Care is universal; cure is not. Every illness may offer something to do,
            // but a meaningful share must stay incurable, or medicine becomes a
            // toggle that erases consequence.
            Assert.Contains(catalog.Diseases, d =>
                !d.treatments.Any(t => DiseaseTreatmentRoles.IsCurative(t.role)));
            Assert.Contains(catalog.Diseases, d =>
                d.treatments.Any(t => DiseaseTreatmentRoles.IsCurative(t.role)));
        }
    }
}
