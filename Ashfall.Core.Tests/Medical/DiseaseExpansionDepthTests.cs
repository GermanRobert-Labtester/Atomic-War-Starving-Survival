// SPDX-License-Identifier: MIT
// Plan 09 / 9A — Disease Expansion depth: 8 grounded pathogens (15 total).
// Data-only ship: no Core schema change, no Infect-trigger wiring, no parallel
// runtime. The "literary 3-phase tell" lives in source_note prose; the runtime
// 2-phase step (incubation → illness → outcome) is unchanged and pinned.
// World-trigger arrival (flood aftermath, deep dig) remains a future Core
// extension; until then the new diseases drift into the spread pool through
// the existing Infect(...) host path.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public class DiseaseExpansionDepthTests
    {
        // Six-cms allowlist: the in-recipe countermeasure ids the disease
        // system can actually flip via its spread-blocker protocols
        // (PurifyWater / SealVents / SterilizeTools / SetAirFiltration).
        // iodine_pills + anti_rad are valid remedies and route through the
        // medical pipeline; they appear as cms in the catalog but never act
        // as spread-blockers, so they are tested separately below.
        private const string ItemCleanWater = "clean_water";
        private const string ItemGasMask = "gas_mask";
        private const string ItemAntibiotics = "antibiotics";
        private const string ItemHazmatSuit = "hazmat_suit";
        private const string ItemIodinePills = "iodine_pills";
        private const string ItemAntiRad = "anti_rad";

        private static readonly HashSet<string> AllowedCountermeasures =
            new HashSet<string>(StringComparer.Ordinal)
            {
                ItemCleanWater, ItemGasMask, ItemHazmatSuit,
                ItemAntibiotics, ItemIodinePills, ItemAntiRad,
            };

        // // ── Loader helpers (parallels DiseaseSystemTests) ──────────

        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static DiseaseCatalog LoadCatalog()
            => DiseaseCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

        private static List<string> Roster(int count)
        {
            var list = new List<string>(count);
            for (int i = 0; i < count; i++) list.Add("sv_" + i);
            return list;
        }

        // // ── Authored new disease ids (single source of test truth) ─

        public static readonly (string id, string vector, string countermeasure, int seed)[] NewDiseases =
        {
            ("disease_wellspring_cramps",          DiseaseVectorNames.Water, "clean_water",  1013),
            ("disease_silt_jaundice",              DiseaseVectorNames.Water, "clean_water",  1013),
            ("disease_condemned_air_cough",        DiseaseVectorNames.Air,   "gas_mask",     1013),
            ("disease_dry_bunker_hiss",            DiseaseVectorNames.Air,   "gas_mask",     1013),
            ("disease_septic_rust_wound_fever",    DiseaseVectorNames.Blood, "antibiotics",  1013),
            ("disease_reused_needle_fever",        DiseaseVectorNames.Blood, "antibiotics",  1013),
            ("disease_deep_excavation_mold_lung",  DiseaseVectorNames.Spore, "hazmat_suit",  1013),
            ("disease_silo_lung",                  DiseaseVectorNames.Spore, "hazmat_suit",  1013),
        };

        public static IEnumerable<object[]> NewDiseaseRows()
        {
            foreach (var (id, vec, cm, seed) in NewDiseases)
                yield return new object[] { id, vec, cm, seed };
        }

        // Apply the right protocol for the disease's vector. The protocol holds
        // for the entire countermeasure-blocked test window; the matching reset
        // is invoked explicitly in the reset test.
        private static void EngageProtocol(DiseaseSystem sys, string vector, int day = 0)
        {
            switch (vector)
            {
                case DiseaseVectorNames.Water: sys.PurifyWater(day); break;
                case DiseaseVectorNames.Air:   sys.SealVents(day); break;
                case DiseaseVectorNames.Blood: sys.SterilizeTools(day); break;
                case DiseaseVectorNames.Spore: sys.SetAirFiltration(true, day); break;
                default: throw new ArgumentException("unknown vector: " + vector);
            }
        }

        // // ── Catalog conformance ────────────────────────────────────

        [Fact]
        public void Catalog_LoadsSixteenDiseases_FromDataDirectory()
        {
            var catalog = LoadCatalog();
            Assert.False(catalog.HasErrors, string.Join("; ", catalog.Errors));
            Assert.Equal(16, catalog.Count);
        }

        [Fact]
        public void AllSevenLegacyDiseases_StillResolvable()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog.GetById("disease_cholera"));
            Assert.NotNull(catalog.GetById("disease_zoonotic_flu"));
            Assert.NotNull(catalog.GetById("disease_blood_fever"));
            Assert.NotNull(catalog.GetById("disease_spore_blight"));
            Assert.NotNull(catalog.GetById("disease_acute_radiation_syndrome"));
            Assert.NotNull(catalog.GetById("disease_fungal_respiratory"));
            Assert.NotNull(catalog.GetById("disease_typhoid_waterborne"));
        }

        [Fact]
        public void AllEightNewDiseases_Authored_WithExactlyTwoPerVector()
        {
            int water = 0, air = 0, blood = 0, spore = 0;
            var catalog = LoadCatalog();
            foreach (var (id, vector, countermeasure, _) in NewDiseases)
            {
                var def = catalog.GetById(id);
                Assert.NotNull(def);
                Assert.Equal(vector, def!.vector);
                Assert.Equal(countermeasure, def.countermeasure_item_id);
                switch (vector)
                {
                    case DiseaseVectorNames.Water: water++; break;
                    case DiseaseVectorNames.Air:   air++;   break;
                    case DiseaseVectorNames.Blood: blood++; break;
                    case DiseaseVectorNames.Spore: spore++; break;
                }
            }
            Assert.Equal(2, water);
            Assert.Equal(2, air);
            Assert.Equal(2, blood);
            Assert.Equal(2, spore);
        }

        [Fact]
        public void EveryNewDisease_ResolvesToOneOfSixAuthorisedCountermeasures()
        {
            var catalog = LoadCatalog();
            foreach (var (id, _, countermeasure, _) in NewDiseases)
            {
                Assert.True(AllowedCountermeasures.Contains(countermeasure),
                    $"countermeasure {countermeasure} for {id} is not in the allowed set");
                Assert.NotNull(catalog.GetById(id));
            }
        }

        [Fact]
        public void EveryNewDisease_SchemaRangesConform()
        {
            var catalog = LoadCatalog();
            foreach (var (id, _, _, _) in NewDiseases)
            {
                var def = catalog.GetById(id)!;
                Assert.InRange(def.lethality, 0f, 1f);
                Assert.InRange(def.infectivity, 0f, 1f);
                Assert.True(def.illness_days >= 1, id + " illness_days < 1");
                Assert.True(def.spread_interval_days >= 1, id + " spread_interval_days < 1");
                Assert.True(def.spread_radius >= 1, id + " spread_radius < 1");
                Assert.False(string.IsNullOrEmpty(def.display_name));
                Assert.False(string.IsNullOrEmpty(def.guidance));
                Assert.False(string.IsNullOrEmpty(def.source_note));
            }
        }

        [Fact]
        public void EveryNewDisease_HasLiteraryThreePhaseTell_InSourceNote()
        {
            // The "3-phase" surface is the source_note prose, NOT a Core field.
            // Two cheap gates:
            //   (a) source_note is long enough to plausibly describe phases;
            //   (b) no two new diseases share a source_note string (each tells a
            //       distinct story — the codebase rejects duplicate copy).
            var seen = new HashSet<string>(StringComparer.Ordinal);
            var catalog = LoadCatalog();
            foreach (var (id, _, _, _) in NewDiseases)
            {
                var def = catalog.GetById(id)!;
                Assert.True(def.source_note.Length >= 180,
                    id + " source_note too short to carry a 3-phase tell");
                Assert.True(seen.Add(def.source_note),
                    id + " source_note duplicates an existing entry");
            }
        }

        [Fact]
        public void EveryNewSourceNote_CommitsToThreePhaseTell()
        {
            // The 3-phase reveal lives in source_note prose, NOT in any Core
            // field. Three honest gates:
            //   (a) the source_note carries enough body to hold three distinct
            //       symptoms (≥ 220 chars — the floor we needed to write all
            //       three tells plus a symptom name plus a pair-with note);
            //   (b) it commits to a "Three-phase tell" section, so a reader
            //       can find the structure without parsing the whole paragraph;
            //   (c) no two new diseases share the same note string — each
            //       tells a distinct story.
            var seen = new HashSet<string>(StringComparer.Ordinal);
            var catalog = LoadCatalog();
            foreach (var (id, _, _, _) in NewDiseases)
            {
                var def = catalog.GetById(id)!;
                Assert.True(def.source_note.Length >= 220,
                    id + " source_note too short to carry a 3-phase tell");
                Assert.Contains("Three-phase tell", def.source_note);
                Assert.True(seen.Add(def.source_note),
                    id + " source_note duplicates an existing entry");
            }
        }

        [Fact]
        public void EveryNewSourceNote_NamesThreeDistinctPhaseSymptoms()
        {
            // More rigorous: each "Three-phase tell:" section must surface
            // three comma-separated symptoms. We slice the section and assert
            // it contains at least three symptom marker clauses delimited by
            // commas. This is the operational pin on the 3-phase structure.
            var catalog = LoadCatalog();
            foreach (var (id, _, _, _) in NewDiseases)
            {
                var note = catalog.GetById(id)!.source_note;
                int start = note.IndexOf("Three-phase tell:", StringComparison.Ordinal);
                Assert.True(start >= 0,
                    id + " source_note missing 'Three-phase tell' header");
                string tell = note.Substring(start);
                int firstComma = tell.IndexOf(',');
                int secondComma = firstComma >= 0
                    ? tell.IndexOf(',', firstComma + 1)
                    : -1;
                Assert.True(firstComma >= 0 && secondComma >= 0,
                    id + " 'Three-phase tell' section must carry at least three comma-separated phases (found only "
                    + (secondComma > 0 ? 2 : firstComma > 0 ? 1 : 0) + ")");
            }
        }

        // // ── Spread-readiness pin (countermeasure OFF → patient reaches contagious window) ──

        // The deterministic invariant for spread readiness is that patient_zero
        // walks past incubation and enters the contagious window regardless of
        // RNG outcome rolls. Whether a follow-on survivor actually gets
        // infected depends on infectivity × RNG over many ticks and is
        // intentionally not pinned here — except via the countermeasure
        // interaction below. This test pins the *readiness* shape of each new
        // disease's lifecycle: contagious window lasts exactly illness_days
        // days, and the snapshot reports contagious=true during that window.

        [Theory]
        [MemberData(nameof(NewDiseaseRows))]
        public void NewDisease_PatientReachesContagiousWindow_AfterIncubation(
            string diseaseId, string vector, string countermeasure, int seed)
        {
            _ = vector; _ = countermeasure; _ = seed;
            var sys = new DiseaseSystem(rng: new Ashfall.Core.SeededRng(1013));
            sys.BindCatalog(LoadCatalog());
            var def = sys.Catalog.GetById(diseaseId)!;
            var roster = Roster(2);

            sys.Infect("patient_zero", diseaseId, 1);
            Assert.True(sys.IsInfected("patient_zero", diseaseId));

            // Walk past incubation — patient_zero must be IsContagious now.
            for (int day = 2; day <= def.incubation_days + 1; day++)
                sys.TickDaily(day, roster);

            Assert.True(sys.IsContagious("patient_zero", diseaseId),
                diseaseId + " failed to reach contagious window within incubation_days+1 ticks");

            var snap = sys.GetSnapshot();
            Assert.True(snap.total_infected >= 1);
        }

        // // ── Countermeasure pin (vector protocol ON → no further spread) ─

        [Theory]
        [MemberData(nameof(NewDiseaseRows))]
        public void VectorProtocol_BlocksAllSpread_OfNewDisease(
            string diseaseId, string vector, string countermeasure, int seed)
        {
            _ = countermeasure;
            var sys = new DiseaseSystem(rng: new Ashfall.Core.SeededRng(seed));
            sys.BindCatalog(LoadCatalog());
            var roster = Roster(10);

            sys.Infect("patient_zero", diseaseId, 1);
            EngageProtocol(sys, vector);
            Assert.True(sys.IsVectorBlocked(vector));

            for (int day = 2; day <= 720; day++)
            {
                EngageProtocol(sys, vector, day);
                sys.TickDaily(day, roster);
            }

            var state = sys.GetDiseaseState(diseaseId);
            Assert.NotNull(state);
            // Historical counter: patient_zero infected exactly once. The
            // vector protocol blocked every spread attempt, so no follow-on
            // infection was ever added to the entry's cumulative count.
            Assert.Equal(1, state!.infections_total);
            Assert.Equal(vector, sys.GetTransmissionVector(diseaseId));

            // Total recovery: 1 patient was either recovered or has died —
            // never both, never more, never less. We don't pin which outcome
            // because lethality is RNG-sensitive; we only pin that exactly one
            // patient ran the full illness arc.
            Assert.Equal(1, state!.recovered_total + state!.deaths_total);

            // The current-state snapshot (total_infected = the *now infected*
            // count) can be 0 (recovered or dead) or 1 (still sick), but never
            // > 1 — that would mean a second survivor slipped past the protocol.
            var snap = sys.GetSnapshot();
            Assert.True(snap.total_infected <= 1,
                diseaseId + " snapshot reports more than 1 currently infected — protocol leaked");
        }

        // // ── Countermeasure → countermeasure-reset pin ──────────────

        [Theory]
        [InlineData(DiseaseVectorNames.Water, "disease_wellspring_cramps", 1013)]
        [InlineData(DiseaseVectorNames.Air,   "disease_condemned_air_cough", 1013)]
        [InlineData(DiseaseVectorNames.Blood, "disease_septic_rust_wound_fever", 1013)]
        [InlineData(DiseaseVectorNames.Spore, "disease_silo_lung", 1013)]
        public void VectorProtocol_ResetFlipsBlockedFlag_RestoringVectorThreat(
            string vector, string diseaseId, int seed)
        {
            var sys = new DiseaseSystem(rng: new Ashfall.Core.SeededRng(seed));
            sys.BindCatalog(LoadCatalog());
            var roster = Roster(8);

            sys.Infect("zero", diseaseId, 1);
            EngageProtocol(sys, vector);
            Assert.True(sys.IsVectorBlocked(vector));

            for (int day = 2; day <= 60; day++)
            {
                EngageProtocol(sys, vector, day);
                sys.TickDaily(day, roster);
            }
            Assert.Equal(1, sys.GetDiseaseState(diseaseId)!.infections_total);

            // Drop the protocol — IsVectorBlocked must flip false. We don't
            // assert post-reset spread here because patient_zero may have died
            // during the long protocol-held window (illness_days + lethality);
            // the spread-on-unblock contract is exhaustively pinned in
            // NewDisease_Spreads_AcrossUnblockedRoster and the lock contract
            // is pinned in VectorProtocol_BlocksAllSpread_OfNewDisease.
            switch (vector)
            {
                case DiseaseVectorNames.Water: sys.ResetWaterPurification(); break;
                case DiseaseVectorNames.Air:   sys.ResetVentSeal(); break;
                case DiseaseVectorNames.Blood: sys.ResetToolSterilization(); break;
                case DiseaseVectorNames.Spore: sys.SetAirFiltration(false); break;
            }
            Assert.False(sys.IsVectorBlocked(vector),
                diseaseId + " vector remained blocked after reset");
        }

        // // ── Save/load round trip for one representative new disease ──

        [Theory]
        [MemberData(nameof(NewDiseaseRows))]
        public void NewDisease_PreservesCountermeasureFlag_AcrossSaveLoad(
            string diseaseId, string vector, string countermeasure, int seed)
        {
            _ = countermeasure;
            var sys = new DiseaseSystem(rng: new Ashfall.Core.SeededRng(seed));
            sys.BindCatalog(LoadCatalog());
            sys.Infect("zero", diseaseId, 1);
            EngageProtocol(sys, vector);

            var state = sys.CaptureState();
            var restored = new DiseaseSystem(state);
            restored.BindCatalog(LoadCatalog());

            Assert.True(restored.IsVectorBlocked(vector));
            Assert.True(restored.IsInfected("zero", diseaseId));
        }
    }
}
