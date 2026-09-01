// SPDX-License-Identifier: MIT
// Plan 60 / D2 + D6 — the clinical picture a surface is allowed to render, and the
// bedside vigil's one hard rule: presence may be measured in real time, but only a
// boolean may reach the simulation. Before this, nothing started a vigil and nothing
// ticked it, so the ward's vigil readout sat at "idle" forever, and the catalog's
// clinical prose (guidance / source_note) reached no surface at all.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.Flags;
using Ashfall.Core.Medical;
using Ashfall.Core.Memorial;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public class VigilAndClinicalPictureTests
    {
        // ── D6: what a kept vigil means ────────────────────────────────

        [Theory]
        [InlineData(false, false, false, DeathQuality.Unattended)]
        [InlineData(true, false, false, DeathQuality.Rushed)]     // medic present, nothing resolved
        [InlineData(true, true, false, DeathQuality.Peaceful)]    // medic present, wish resolved
        [InlineData(false, false, true, DeathQuality.Peaceful)]   // only a vigil — still care
        [InlineData(false, true, false, DeathQuality.Unattended)] // a wish alone is not presence
        [InlineData(true, false, true, DeathQuality.Peaceful)]
        public void ResolveQuality_MatchesTheCareMatrix(
            bool attended, bool wishResolved, bool vigilKept, DeathQuality expected)
        {
            Assert.Equal(expected, VigilCare.ResolveQuality(attended, wishResolved, vigilKept));
        }

        [Fact]
        public void KeptVigil_IsRecordedOnceOnTheConsequenceLedger()
        {
            var flags = new InMemoryFlagLedger();

            VigilCare.RecordKept(flags, "sv_a", day: 12);
            VigilCare.RecordKept(flags, "sv_a", day: 13);

            Assert.True(VigilCare.IsKept(flags, "sv_a"));
            Assert.False(VigilCare.IsKept(flags, "sv_b"));
            Assert.StartsWith("flag_vigil_kept_", VigilCare.FlagFor("sv_a"));
        }

        [Fact]
        public void VigilFlagConvention_IsStableAcrossHosts()
        {
            // The ledger normalises case; the id must not depend on it.
            Assert.Equal(VigilCare.FlagFor("sv_a").ToLowerInvariant(),
                VigilCare.FlagFor("SV_A").ToLowerInvariant());
            Assert.Equal(string.Empty, VigilCare.FlagFor(null));
            Assert.False(VigilCare.IsKept(null, "sv_a"));
        }

        [Fact]
        public void VigilDuration_CanNeverMoveTheSimulation()
        {
            // The bed-clock is real time, so it must be provably irrelevant to outcome.
            // Same vigil, two tick granularities, and the quality the campaign records is
            // identical — a 30 fps machine cannot give anyone a worse death than 144 fps.
            DeathQuality Run(double secondsPerTick, int ticks)
            {
                var vigil = new VigilStateMachine();
                vigil.StartVigil("sv_a", new[] { "one", "two" });
                for (int i = 0; i < ticks; i++) vigil.Tick((float)secondsPerTick);
                if (!vigil.IsCompleted) vigil.Skip();
                return VigilCare.ResolveQuality(
                    attended: false, wishResolved: false,
                    vigilKept: vigil.IsCompleted && !vigil.WasSkipped);
            }

            var fine = Run(0.5, (int)(VigilStateMachine.DefaultDuration / 0.5));
            var coarse = Run(3.0, (int)(VigilStateMachine.DefaultDuration / 3.0) + 2);

            Assert.Equal(fine, coarse);
            Assert.Equal(DeathQuality.Peaceful, fine);
        }

        [Fact]
        public void ALeftEarlyVigil_IsNotRecordedAsCare()
        {
            var vigil = new VigilStateMachine();
            vigil.StartVigil("sv_a", new[] { "one" });
            vigil.Tick(5f);
            vigil.Skip();

            Assert.True(vigil.IsCompleted);
            Assert.True(vigil.WasSkipped);
            Assert.Equal(DeathQuality.Unattended, VigilCare.ResolveQuality(
                attended: false, wishResolved: false,
                vigilKept: vigil.IsCompleted && !vigil.WasSkipped));
        }

        // ── D2: the clinical picture ───────────────────────────────────

        private static DiseaseDefinition Def(
            string tell = "yellow in the eyes before the skin",
            string secondary = "dark urine",
            string timing = "a week after flood water",
            string guidance = "Boil everything.",
            float lethality = 0.5f, int incubation = 2, int illness = 10,
            params (string item, string role)[] treatments)
        {
            var d = new DiseaseDefinition
            {
                id = "disease_fixture", display_name = "Fixture illness",
                vector = DiseaseVectorNames.Water, tell = tell,
                tell_secondary = secondary, timing_clue = timing, guidance = guidance,
                lethality = lethality, incubation_days = incubation, illness_days = illness,
            };
            foreach (var t in treatments)
                d.treatments.Add(new DiseaseTreatment
                {
                    item_id = t.item, role = t.role, max_days = 0,
                    lethality_reduction = 0.1f,
                });
            return d;
        }

        [Fact]
        public void PictureOf_CarriesTheAuthoredSignsAndStage()
        {
            var def = Def();
            var picture = DiseaseTriage.PictureOf(def, daysSick: 9);

            Assert.Equal("yellow in the eyes before the skin", picture.Tell);
            Assert.Equal("dark urine", picture.SecondaryTell);
            Assert.Equal("a week after flood water", picture.TimingClue);
            Assert.Equal("Boil everything.", picture.Guidance);
            Assert.Equal(DiseaseTriage.StageToken(picture.Stage), picture.StageToken);
            Assert.Equal(DiseaseClinicalStage.Terminal, picture.Stage);
            Assert.True(picture.Terminal);
            Assert.Equal(1, picture.DaysUntilOutcome);
        }

        [Theory]
        [InlineData(true, "Fixture illness")]
        [InlineData(false, "")]
        public void PictureOf_HidesTheNameBeforeItIsEarned(bool diagnosed, string expectedName)
        {
            // Signs are what a person at the bedside sees; the identification is the
            // diagnosis mechanic's business and must not leak from a projection helper.
            var picture = DiseaseTriage.PictureOf(Def(), 5, diagnosed: diagnosed);
            Assert.Equal(expectedName, picture.DisplayName);
            Assert.NotEmpty(picture.Tell);
        }

        [Fact]
        public void PictureOf_DistinguishesCurableFromIncurableIllness()
        {
            var curable = DiseaseTriage.PictureOf(
                Def(treatments: new (string, string)[] { ("antibiotics", "curative") }), 4);
            var careOnly = DiseaseTriage.PictureOf(
                Def(treatments: new (string, string)[] { ("bandage", "supportive") }), 4);
            var nothing = DiseaseTriage.PictureOf(Def(), 4);

            Assert.True(curable.HasCure);
            Assert.True(careOnly.HasTreatmentPath);
            Assert.False(careOnly.HasCure);
            Assert.False(nothing.HasTreatmentPath);
        }

        [Fact]
        public void PictureOf_ReportsOddsAfterTreatment_AndDefaultsToAuthoredLethality()
        {
            var untreated = DiseaseTriage.PictureOf(Def(lethality: 0.5f), 4);
            var treated = DiseaseTriage.PictureOf(Def(lethality: 0.5f), 4,
                effectiveLethality: 0.35f, dosesGiven: 2);

            Assert.Equal(0.5f, untreated.EffectiveLethality, 3);
            Assert.Equal(0, untreated.DosesGiven);
            Assert.Equal(0.35f, treated.EffectiveLethality, 3);
            Assert.Equal(2, treated.DosesGiven);
        }

        [Fact]
        public void PictureOf_SurvivesAnUnauthorisedOrEmptyDefinition()
        {
            Assert.NotNull(DiseaseTriage.PictureOf(null, 3));
            Assert.Equal(DiseaseClinicalStage.None, DiseaseTriage.PictureOf(null, 3).Stage);
            Assert.Equal(string.Empty, DiseaseTriage.PictureOf(new DiseaseDefinition(), 3).Tell);
        }

        // ── the shipped catalog keeps its clinical contract ────────────

        private static DiseaseCatalog LoadCatalog()
        {
            string start = Directory.GetCurrentDirectory();
            if (!CatalogLocator.TryFindDataDirectory(start, out string dir)
                && !CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out dir))
                throw new DirectoryNotFoundException("data authority not found");
            return DiseaseCatalogLoader.Load(dir, new FileSystemIO(), new SystemTextJsonSerializer());
        }

        [Fact]
        public void ShippedCatalog_EveryIllnessIsRecognisable()
        {
            var catalog = LoadCatalog();
            Assert.NotEmpty(catalog.Diseases);

            var primary = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var d in catalog.Diseases)
            {
                Assert.False(string.IsNullOrWhiteSpace(d.tell), $"{d.id} has no primary tell");
                Assert.False(string.IsNullOrWhiteSpace(d.timing_clue), $"{d.id} has no timing clue");
                Assert.False(string.IsNullOrWhiteSpace(d.guidance), $"{d.id} has no bedside guidance");
                Assert.True(primary.Add(d.tell), $"{d.id} reuses another illness's primary tell");
                Assert.All(d.treatments, t => Assert.False(string.IsNullOrWhiteSpace(t.item_id)));
            }
        }

        [Fact]
        public void ShippedCatalog_StillHasIllnessesThatCannotBeCured()
        {
            var catalog = LoadCatalog();
            Assert.Contains(catalog.Diseases, d => !d.treatments.Any(t => DiseaseTreatmentRoles.IsCurative(t.role)));
        }
    }
}
