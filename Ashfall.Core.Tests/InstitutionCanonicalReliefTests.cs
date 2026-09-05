using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Host-binding wave 2: the canonical relief APIs the sanatorium port
    /// consumes, and the vinyl merge path the culture archive cutting uses.
    /// </summary>
    public class InstitutionCanonicalReliefTests
    {
        [Fact]
        public void CombatTrauma_TherapyRelief_ScalesHypervigilanceDown()
        {
            var sys = new CombatTraumaSystem();
            sys.RegisterSurvivor("sv");
            sys.OnCombatSurvived("sv");
            sys.OnCombatSurvived("sv");
            sys.OnCombatSurvived("sv");
            float before = sys.GetHypervigilanceLevel("sv");
            Assert.True(before > 0f);

            sys.ApplyTherapyRelief("sv", 0.5f);
            Assert.Equal(before * 0.5f, sys.GetHypervigilanceLevel("sv"), 4);

            sys.ApplyTherapyRelief("sv", 1f);
            Assert.Equal(0f, sys.GetHypervigilanceLevel("sv"), 4);

            // unknown survivor / zero fraction are no-ops
            sys.ApplyTherapyRelief("ghost", 1f);
            sys.ApplyTherapyRelief("sv", 0f);
            Assert.Equal(0f, sys.GetHypervigilanceLevel("sv"), 4);
        }

        [Fact]
        public void Flashback_ReduceSusceptibility_FloorsAtZero()
        {
            var sys = new SomaticFlashbackSystem();
            sys.IncreaseSusceptibility("sv", 0.4f);
            Assert.Equal(0.4f, sys.GetSusceptibility("sv"), 4);

            sys.ReduceSusceptibility("sv", 0.15f);
            Assert.Equal(0.25f, sys.GetSusceptibility("sv"), 4);

            sys.ReduceSusceptibility("sv", 9f);
            Assert.Equal(0f, sys.GetSusceptibility("sv"), 4);

            // unknown survivor no-op, negative amounts rejected
            sys.ReduceSusceptibility("ghost", 1f);
            sys.ReduceSusceptibility("sv", -1f);
            Assert.Equal(0f, sys.GetSusceptibility("sv"), 4);
        }

        [Fact]
        public void GuiltInsomnia_TherapyRelief_ScalesInsomniaDown()
        {
            var sys = new GuiltInsomniaSystem();
            sys.RecordGuilt("sv", "source_a", 0.8f, currentDay: 1);
            float before = sys.GetInsomniaSeverity("sv");
            Assert.True(before > 0f);

            sys.ApplyTherapyRelief("sv", 0.25f);
            Assert.Equal(before * 0.75f, sys.GetInsomniaSeverity("sv"), 4);

            sys.ApplyTherapyRelief("ghost", 1f);
            Assert.Equal(before * 0.75f, sys.GetInsomniaSeverity("sv"), 4);
        }

        [Fact]
        public void Vinyl_MergeRecord_AddsWithoutReplacing_Catalog()
        {
            var sys = new VinylMoraleSystem();
            var preWar = new VinylRecordDefinition { record_id = "pre_war_a", display_name = "Pre-War" };
            sys.LoadCatalog(new List<VinylRecordDefinition> { preWar });

            sys.MergeRecord(new VinylRecordDefinition { record_id = "archive_disc_dream_sv", display_name = "Cut Disc" });

            // both resolvable — replace-all semantics were not triggered
            Assert.NotNull(sys.GetRecord("pre_war_a"));
            Assert.NotNull(sys.GetRecord("archive_disc_dream_sv"));

            // re-cutting the same id overwrites (no duplicate), pre-war intact
            sys.MergeRecord(new VinylRecordDefinition { record_id = "archive_disc_dream_sv", display_name = "Recut" });
            Assert.Equal("Recut", sys.GetRecord("archive_disc_dream_sv")!.display_name);
            Assert.NotNull(sys.GetRecord("pre_war_a"));

            // null / empty-id merges are no-ops
            sys.MergeRecord(null);
            sys.MergeRecord(new VinylRecordDefinition());
            Assert.NotNull(sys.GetRecord("pre_war_a"));
        }
    }
}
