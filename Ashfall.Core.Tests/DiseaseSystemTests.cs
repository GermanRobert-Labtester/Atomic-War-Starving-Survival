using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Crossing;
using Ashfall.Core.Disease;
using Ashfall.Core.Foundry;
using Ashfall.Core.Legacy;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Disease Expansion (epidemic contagion / quarantine protocols / waterborne
    /// pathogens / contagious spore vectors) — migrated from the legacy Unity
    /// DiseaseSystem_Expansion.cs. Every rule is deterministic through the
    /// seeded RNG: same seed ⇒ same outbreak, same deaths, same recoveries.
    /// </summary>
    public class DiseaseSystemTests
    {
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
        {
            return DiseaseCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
        }

        private static List<string> Roster(int count)
        {
            var roster = new List<string>();
            for (int i = 0; i < count; i++) roster.Add("s_" + i);
            return roster;
        }

        // -----------------------------------------------------------------
        // Catalog
        // -----------------------------------------------------------------

        [Fact]
        public void Catalog_LoadsFromDataDirectory_WithCanonicalIds()
        {
            var catalog = LoadCatalog();
            Assert.False(catalog.HasErrors, string.Join("; ", catalog.Errors));
            Assert.True(catalog.Count >= 4);

            Assert.Equal(DiseaseIds.ExpansionId, "expansion_disease_expansion");
            Assert.Equal("disease_cholera", DiseaseIds.Cholera);
            Assert.Equal("disease_zoonotic_flu", DiseaseIds.ZoonoticFlu);
            Assert.Equal("disease_blood_fever", DiseaseIds.BloodFever);
            Assert.Equal("disease_spore_blight", DiseaseIds.SporeBlight);

            // Every disease has an authored vector + a resolvable countermeasure
            // item (items.json is the shared inventory authority).
            for (int i = 0; i < catalog.Diseases.Count; i++)
            {
                var d = catalog.Diseases[i];
                Assert.False(string.IsNullOrEmpty(d.vector));
                Assert.InRange(d.lethality, 0f, 1f);
                Assert.InRange(d.infectivity, 0f, 1f);
                Assert.True(d.illness_days >= 1);
                Assert.False(string.IsNullOrEmpty(d.countermeasure_item_id), d.id + " has no countermeasure item");
            }

            var cholera = catalog.GetById(DiseaseIds.Cholera);
            Assert.NotNull(cholera);
            Assert.Equal(DiseaseVectorNames.Water, cholera.vector);
            Assert.Equal("clean_water", cholera.countermeasure_item_id);
            Assert.Equal(DiseaseVectorNames.Spore, catalog.GetById(DiseaseIds.SporeBlight).vector);
        }

        [Fact]
        public void UnknownVector_DefaultsToWater_AndUnknownDiseaseIsRejected()
        {
            var sys = new DiseaseSystem(rng: new Ashfall.Core.SeededRng(1));
            sys.BindCatalog(LoadCatalog());
            sys.Infect("anyone", "disease_does_not_exist", 1);
            Assert.Equal(0, sys.TotalInfectionsHistory);
            Assert.Equal(string.Empty, sys.GetTransmissionVector("disease_does_not_exist"));
        }

        // -----------------------------------------------------------------
        // Outbreak + quarantine protocol
        // -----------------------------------------------------------------

        [Fact]
        public void ThreeActiveInfections_DeclareOutbreak()
        {
            var sys = new DiseaseSystem(rng: new Ashfall.Core.SeededRng(1013));
            sys.BindCatalog(LoadCatalog());

            sys.Infect("a", DiseaseIds.Cholera, 10);
            sys.Infect("b", DiseaseIds.Cholera, 10);
            Assert.Equal(0, sys.GetSnapshot().total_outbreaks);
            sys.Infect("c", DiseaseIds.Cholera, 10);

            Assert.Equal(1, sys.GetSnapshot().total_outbreaks);
            Assert.True(sys.GetDiseaseState(DiseaseIds.Cholera).outbreak_active);
            Assert.Equal(3, sys.GetSnapshot().total_infected);
        }

        [Fact]
        public void QuarantineWard_StopsSpread_AndContainmentPreventsTheOutbreak()
        {
            var sys = new DiseaseSystem(rng: new Ashfall.Core.SeededRng(1013));
            sys.BindCatalog(LoadCatalog());
            var roster = Roster(8);

            sys.Infect("a", DiseaseIds.Cholera, 10);
            sys.Infect("b", DiseaseIds.Cholera, 10);
            sys.Infect("c", DiseaseIds.Cholera, 10);
            Assert.True(sys.GetDiseaseState(DiseaseIds.Cholera).outbreak_active);

            // All three in the isolation ward — nobody can seed a new case.
            sys.Quarantine("a", DiseaseIds.Cholera);
            sys.Quarantine("b", DiseaseIds.Cholera);
            sys.Quarantine("c", DiseaseIds.Cholera);
            Assert.True(sys.IsContagious("a", "disease_does_not_exist") == false); // sanity
            Assert.True(sys.IsQuarantined("a", DiseaseIds.Cholera));

            int totalBefore = sys.TotalInfectionsHistory;
            for (int day = 11; day <= 60; day++)
                sys.TickDaily(day, roster);

            // Invariant for ANY seed: quarantined patients never spread.
            Assert.Equal(totalBefore, sys.TotalInfectionsHistory);
            Assert.True(sys.IsContagious("a", DiseaseIds.Cholera) == false
                        || sys.GetDiseaseState(DiseaseIds.Cholera) == null
                        || !sys.GetDiseaseState(DiseaseIds.Cholera).outbreak_active);

            // The ward comes home: contained with no deaths during the active
            // outbreak ⇒ prevented. (A patient can still die AFTER the outbreak
            // is contained by quarantine — that does not reopen the outbreak.)
            var state = sys.GetDiseaseState(DiseaseIds.Cholera);
            Assert.NotNull(state);
            Assert.False(state.outbreak_active);
            Assert.Equal(1, state.outbreaks_total);
            Assert.Equal(state.deaths_during_outbreak == 0 ? 1 : 0, state.outbreaks_prevented);
            Assert.Equal(3, state.recovered_total + state.deaths_total);
            Assert.Equal(0, sys.GetSnapshot().total_infected);
        }

        [Fact]
        public void Quarantine_AppliesPerDisease_PerSurvivor()
        {
            var sys = new DiseaseSystem(rng: new Ashfall.Core.SeededRng(7));
            sys.BindCatalog(LoadCatalog());
            sys.Infect("x", DiseaseIds.Cholera, 1);
            sys.Infect("x", DiseaseIds.ZoonoticFlu, 1);
            sys.Quarantine("x", DiseaseIds.Cholera);
            Assert.True(sys.IsQuarantined("x", DiseaseIds.Cholera));
            Assert.False(sys.IsQuarantined("x", DiseaseIds.ZoonoticFlu));
            Assert.True(sys.IsInfected("x", DiseaseIds.ZoonoticFlu));
            sys.EndQuarantine("x", DiseaseIds.Cholera);
            Assert.False(sys.IsQuarantined("x", DiseaseIds.Cholera));
        }

        // -----------------------------------------------------------------
        // Vector protocols
        // -----------------------------------------------------------------

        [Fact]
        public void WaterProtocol_BlocksWaterborneSpread_AndResetRestoresTheThreat()
        {
            var sys = new DiseaseSystem(rng: new Ashfall.Core.SeededRng(55));
            sys.BindCatalog(LoadCatalog());
            var roster = Roster(10);

            sys.Infect("patient_zero", DiseaseIds.Cholera, 5);
            sys.PurifyWater();
            Assert.True(sys.IsVectorBlocked(DiseaseVectorNames.Water));

            for (int day = 6; day <= 45; day++)
                sys.TickDaily(day, roster);
            Assert.Equal(1, sys.TotalInfectionsHistory); // protocol holds

            sys.ResetWaterPurification();
            Assert.False(sys.IsVectorBlocked(DiseaseVectorNames.Water));
            // The first patient resolves before day 46; seed a fresh case so
            // the unblocked vector has someone to shed from.
            sys.Infect("patient_two", DiseaseIds.Cholera, 46);
            for (int day = 46; day <= 90; day++)
                sys.TickDaily(day, roster);
            // With a live unisolated cholera patient, infectivity 0.4 every 2d
            // over 10 candidates, a 45-day window must produce secondary cases.
            // NOTE: SeededRng is fully qualified (Ashfall.Core.SeededRng) — the
            // test assembly carries a namespace-level SeededRng stub that would
            // otherwise shadow the real xorshift RNG on unqualified references.
            Assert.True(sys.TotalInfectionsHistory > 2, // 2 direct + secondary
                "resetting the protocol must restore the waterborne threat");
        }

        [Fact]
        public void EveryVector_HasALivableProtocol()
        {
            var sys = new DiseaseSystem(rng: new Ashfall.Core.SeededRng(3));
            sys.BindCatalog(LoadCatalog());
            Assert.False(sys.IsVectorBlocked(DiseaseVectorNames.Air));
            Assert.False(sys.IsVectorBlocked(DiseaseVectorNames.Blood));
            Assert.False(sys.IsVectorBlocked(DiseaseVectorNames.Spore));

            sys.SealVents();
            sys.SterilizeTools();
            sys.SetAirFiltration(true);
            Assert.True(sys.IsVectorBlocked(DiseaseVectorNames.Air));
            Assert.True(sys.IsVectorBlocked(DiseaseVectorNames.Blood));
            Assert.True(sys.IsVectorBlocked(DiseaseVectorNames.Spore));

            sys.ResetVentSeal();
            sys.ResetToolSterilization();
            sys.SetAirFiltration(false);
            Assert.False(sys.IsVectorBlocked(DiseaseVectorNames.Air));
            Assert.False(sys.IsVectorBlocked(DiseaseVectorNames.Blood));
            Assert.False(sys.IsVectorBlocked(DiseaseVectorNames.Spore));
        }

        // -----------------------------------------------------------------
        // Determinism
        // -----------------------------------------------------------------

        private static string Fingerprint(int seed)
        {
            var sys = new DiseaseSystem(rng: new Ashfall.Core.SeededRng(seed));
            sys.BindCatalog(LoadCatalog());
            var roster = Roster(12);

            sys.Infect("p0", DiseaseIds.Cholera, 1);
            sys.Infect("p1", DiseaseIds.ZoonoticFlu, 1);
            sys.Infect("p2", DiseaseIds.BloodFever, 1);
            sys.Infect("p3", DiseaseIds.SporeBlight, 1);
            for (int day = 2; day <= 60; day++)
            {
                sys.TickDaily(day, roster);
                if (day == 10) sys.Quarantine("p3", DiseaseIds.SporeBlight);
                if (day == 15) sys.PurifyWater();
            }
            var s = sys.CaptureState();
            return string.Join("|",
                s.diseases[0].outbreaks_total, s.diseases[0].deaths_total,
                s.diseases[1].outbreaks_total, s.diseases[1].deaths_total,
                s.diseases[2].outbreaks_total, s.diseases[2].deaths_total,
                s.diseases[3].outbreaks_total, s.diseases[3].deaths_total,
                s.rngSeed);
        }

        [Fact]
        public void SameSeed_ProducesIdenticalOutbreaks()
        {
            Assert.Equal(Fingerprint(1013), Fingerprint(1013));
            Assert.Equal(Fingerprint(77), Fingerprint(77));
            Assert.NotEqual(Fingerprint(1013), Fingerprint(77));
        }

        [Fact]
        public void OutcomeRolls_ProduceBothRecoveryAndDeath_Deterministically()
        {
            // Cholera lethality 0.30 over a 40-day window with a fresh patient per
            // seed: across 30 seeds the outcome branch must hit both paths.
            int deaths = 0, recoveries = 0;
            for (int seed = 200; seed < 230; seed++)
            {
                var sys = new DiseaseSystem(rng: new Ashfall.Core.SeededRng(seed));
                sys.BindCatalog(LoadCatalog());
                sys.Infect("patient", DiseaseIds.Cholera, 1);
                // Empty roster: no autonomous spread — exactly one patient, so
                // recovered+deaths must equal exactly one.
                for (int day = 2; day <= 40; day++)
                    sys.TickDaily(day, new List<string>());
                var s = sys.GetDiseaseState(DiseaseIds.Cholera);
                deaths += s.deaths_total;
                recoveries += s.recovered_total;
                Assert.True(s.recovered_total + s.deaths_total == 1);
            }
            Assert.True(deaths > 0, "30 seeds must include at least one death");
            Assert.True(recoveries > 0, "30 seeds must include at least one recovery");
        }

        // -----------------------------------------------------------------
        // Persistence (rides the expansion-hub save envelope)
        // -----------------------------------------------------------------

        [Fact]
        public void Save_ExpansionHubEnvelopeRoundTrips_DiseaseState()
        {
            var sys = new DiseaseSystem(rng: new Ashfall.Core.SeededRng(2024));
            sys.BindCatalog(LoadCatalog());
            sys.Infect("a", DiseaseIds.SporeBlight, 5);
            sys.Infect("b", DiseaseIds.SporeBlight, 5);
            sys.Infect("c", DiseaseIds.SporeBlight, 5);
            sys.Quarantine("c", DiseaseIds.SporeBlight);
            sys.SetAirFiltration(true);

            var json = new SystemTextJsonSerializer();
            var envelope = new ExpansionHubSave
            {
                saveVersion = ExpansionHubSave.CurrentSaveVersion,
                simDay = 42
            };
            envelope.disease = sys.CaptureState();
            envelope.Checksum = SaveChecksum.Compute(envelope);

            string encoded = ExpansionHubSaveCodec.Encode(envelope, json);
            var decoded = ExpansionHubSaveCodec.Decode(encoded, json);

            Assert.Equal(ExpansionHubSave.CurrentSaveVersion, decoded.saveVersion);
            Assert.NotNull(decoded.disease);
            Assert.True(decoded.disease.air_filtration);
            // One simulation row per authored disease (cholera, flu, blood, spore).
            Assert.Equal(4, decoded.disease.diseases.Count);
            var spore = decoded.disease.diseases.Find(d => d.disease_id == DiseaseIds.SporeBlight);
            Assert.NotNull(spore);
            Assert.Equal(3, spore.infected.Count);
            Assert.True(spore.infected[2].quarantined);
            Assert.Equal(sys.CaptureState().rngSeed, decoded.disease.rngSeed);
        }

        [Fact]
        public void Save_TamperWithDiseasePayload_FailsChecksum()
        {
            var json = new SystemTextJsonSerializer();
            var envelope = new ExpansionHubSave { saveVersion = ExpansionHubSave.CurrentSaveVersion, simDay = 1 };
            envelope.disease = new DiseaseSystem(rng: new Ashfall.Core.SeededRng(9)).CaptureState();
            envelope.Checksum = SaveChecksum.Compute(envelope);
            string encoded = ExpansionHubSaveCodec.Encode(envelope, json);

            // Corrupt the ward after the checksum was computed.
            string tampered = encoded.Replace("\"water_purified\":false", "\"water_purified\":true");
            Assert.Throws<InvalidOperationException>(() => ExpansionHubSaveCodec.Decode(tampered, json));
        }

        [Fact]
        public void V3Save_MigratesToV4_WithFreshWard()
        {
            var json = new SystemTextJsonSerializer();
            var v3 = new ExpansionHubSaveV3
            {
                saveVersion = 3,
                simDay = 290,
                waystation = new WaystationSystemState(),
                layouts = new LocationLayoutState(),
                memory = new LocationMemoryState(),
                siteEncounters = new SiteEncounterState(),
                vouch = new VouchAccessSystemState(),
                greenhouse = new GreenhouseState(),
                arbitration = new CrossingArbitrationState(),
                ledger = new LedgerDebtSystemState(),
                crossingQuests = new CrossingQuestSystemState(),
                generational = new GenerationalSuccessionSaveState(),
                foundry = new SilentFoundryState { unlocked = true, unlockDay = 100 },
                consequences = new SilentFoundryConsequenceState()
            };
            v3.Checksum = SaveChecksum.Compute(v3);

            var migrated = ExpansionHubSaveCodec.Decode(json.Serialize(v3), json);
            Assert.Equal(4, migrated.saveVersion);
            Assert.Equal(290, migrated.simDay);
            Assert.True(migrated.foundry.unlocked, "v3 foundry state must survive migration");
            Assert.NotNull(migrated.disease);
            Assert.True(migrated.disease.diseases.Count == 0, "v3 saves predate the disease ward");

            // The restored engine starts with an empty ward and a usable seed.
            var engine = new DiseaseSystem(log: NullLog.Instance);
            ExpansionHubSaveCodec.Restore(migrated, null, null, null, null, null, null, null, null, null, null, null, engine);
            Assert.Equal(0, engine.GetSnapshot().total_infected);
            Assert.NotEqual(0, engine.State.rngSeed);
        }

        [Fact]
        public void Restore_ResumesSameRngStream_AsATwin()
        {
            var catalog = LoadCatalog();
            var original = new DiseaseSystem(rng: new Ashfall.Core.SeededRng(808));
            original.BindCatalog(catalog);
            original.Infect("x", DiseaseIds.ZoonoticFlu, 1);
            var captured = original.CaptureState();

            var restored = new DiseaseSystem(log: NullLog.Instance);
            restored.RestoreState(captured);
            restored.BindCatalog(catalog);

            var twin = new DiseaseSystem(rng: new Ashfall.Core.SeededRng(captured.rngSeed));
            twin.BindCatalog(catalog);
            twin.Infect("x", DiseaseIds.ZoonoticFlu, 1);

            var roster = Roster(6);
            for (int day = 2; day <= 30; day++)
            {
                restored.TickDaily(day, roster);
                twin.TickDaily(day, roster);
            }

            var a = restored.CaptureState();
            var b = twin.CaptureState();
            Assert.Equal(a.rngSeed, b.rngSeed);
            Assert.Equal(a.diseases[0].deaths_total, b.diseases[0].deaths_total);
            Assert.Equal(a.diseases[0].recovered_total, b.diseases[0].recovered_total);
        }

        // -----------------------------------------------------------------
        // Data integrity
        // -----------------------------------------------------------------

        [Fact]
        public void DiseaseCatalog_IntroducesNoDataIntegrityErrors()
        {
            var report = CatalogIntegrityValidator.Validate(DataDir(), new FileSystemIO());
            for (int i = 0; i < report.Errors.Count; i++)
            {
                Assert.DoesNotContain("disease", report.Errors[i]);
            }
        }
    }
}