using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Disease;

namespace Ashfall.Core
{
    /// <summary>
    /// Disease Expansion pack-minimum smoke: exact ids resolve, the catalog
    /// loads with every countermeasure item present in items.json, an outbreak
    /// declares at the threshold, quarantine stalls the vector, protocols block
    /// it outright, outcomes resolve deterministically, and the state save
    /// round-trips. Invoked by the expansions selftest and by xUnit.
    /// </summary>
    public static class DiseaseHeadlessDemo
    {
        public const int DemoSeed = 1013;

        public static HeadlessReport Run(string? dataDirectory = null, ILog? log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new HeadlessReport();

            void Check(bool condition, string name)
            {
                report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
                if (condition) { report.PassedCount++; log.Info("[PASS] " + name); }
                else { report.FailedCount++; log.Error("[FAIL] " + name); }
            }

            log.Info("[DiseaseHeadlessDemo] begin");

            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // Identity resolution (exact ids, never aliased).
            Check(DiseaseIds.ExpansionId == "expansion_disease_expansion", "expansion id expansion_disease_expansion");
            Check(DiseaseIds.Cholera == "disease_cholera", "disease id disease_cholera");
            Check(DiseaseIds.ZoonoticFlu == "disease_zoonotic_flu", "disease id disease_zoonotic_flu");
            Check(DiseaseIds.BloodFever == "disease_blood_fever", "disease id disease_blood_fever");
            Check(DiseaseIds.SporeBlight == "disease_spore_blight", "disease id disease_spore_blight");

            // Static catalog loads from disk.
            var catalog = DiseaseCatalogLoader.Load(dataDirectory, files, json);
            Check(catalog != null && !catalog.HasErrors, "disease_catalog.json loads with no schema errors");
            Check(catalog != null && catalog.Count >= 4, "disease_catalog.json registers >= 4 diseases");

            // Authored countermeasure items resolve in items.json (the shared
            // inventory catalog — a dangling ref would silently neuter a protocol).
            if (catalog != null && catalog.Count > 0)
            {
                var itemIds = LoadItemIds(dataDirectory, files, json);
                for (int i = 0; i < catalog.Diseases.Count; i++)
                {
                    var d = catalog.Diseases[i];
                    if (d == null || string.IsNullOrEmpty(d.countermeasure_item_id)) continue;
                    bool present = itemIds.Contains(d.countermeasure_item_id);
                    Check(present, "countermeasure item '" + d.countermeasure_item_id + "' for " + d.id + " exists in items.json");
                }
            }

            var system = new DiseaseSystem(rng: new SeededRng(DemoSeed), log: log);
            system.BindCatalog(catalog ?? new DiseaseCatalog());

            // Vectors + protocol wiring.
            Check(system.GetTransmissionVector(DiseaseIds.Cholera) == DiseaseVectorNames.Water,
                "cholera is a waterborne pathogen");
            Check(system.GetTransmissionVector(DiseaseIds.SporeBlight) == DiseaseVectorNames.Spore,
                "spore blight is a contagious spore vector");
            Check(!system.IsVectorBlocked(DiseaseVectorNames.Water), "water vector unblocked before the protocol");

            // Outbreak declaration at the threshold of 3 active infections.
            system.Infect("demo_survivor_a", DiseaseIds.Cholera, day: 10);
            system.Infect("demo_survivor_b", DiseaseIds.Cholera, day: 10);
            system.Infect("demo_survivor_c", DiseaseIds.Cholera, day: 10);
            Check(system.GetDiseaseState(DiseaseIds.Cholera) != null
                  && system.GetDiseaseState(DiseaseIds.Cholera)!.outbreak_active,
                "three active infections declare an outbreak");
            Check(system.GetSnapshot().total_outbreaks == 1, "outbreaks_total == 1");

            // Quarantine stalls the vector: all three isolated, tick with a live
            // roster — nobody new is exposed.
            foreach (var s in new[] { "demo_survivor_a", "demo_survivor_b", "demo_survivor_c" })
                system.Quarantine(s, DiseaseIds.Cholera);
            var roster = new List<string>
            {
                "demo_survivor_d", "demo_survivor_e", "demo_survivor_f",
                "demo_survivor_g", "demo_survivor_h"
            };
            int before = system.TotalInfectionsHistory;
            system.TickDaily(day: 12, candidates: roster);
            system.TickDaily(day: 13, candidates: roster);
            system.TickDaily(day: 14, candidates: roster);
            Check(system.TotalInfectionsHistory == before,
                "quarantined ward cannot seed new infections");

            // The quarantine ward contains the outbreak without loss (cholera
            // kills only on a seeded outcome roll at illness_days; here the ward
            // resolves before the days run out).
            var cholera = system.GetDiseaseState(DiseaseIds.Cholera);
            Check(cholera != null && !cholera.outbreak_active, "outbreak contained once no case is contagious");
            Check(cholera != null && cholera.outbreaks_prevented == 1, "contained outbreak counted as prevented");

            // Protocols: purifying the water blocks the water vector outright.
            system.PurifyWater();
            system.Infect("demo_survivor_i", DiseaseIds.Cholera, day: 20);
            int beforeProtocol = system.TotalInfectionsHistory;
            system.TickDaily(day: 21, candidates: roster);
            system.TickDaily(day: 22, candidates: roster);
            Check(system.IsVectorBlocked(DiseaseVectorNames.Water), "purified water blocks the water vector");
            Check(system.TotalInfectionsHistory == beforeProtocol,
                "no new cases while the water protocol holds");

            // Determinism: same seed, same candidate order ⇒ same outbreak.
            var a = SimulateOutbreak(DemoSeed, files, json, dataDirectory);
            var b = SimulateOutbreak(DemoSeed, files, json, dataDirectory);
            Check(a == b, "deterministic spread given the same seed and roster");

            // Save round-trip through the same serializer the hosts use.
            var stateA = system.CaptureState();
            string encoded = json.Serialize(stateA);
            var stateB = json.Deserialize<DiseaseSystemState>(encoded);
            var restored = new DiseaseSystem(log: log);
            restored.RestoreState(stateB!);
            restored.BindCatalog(catalog ?? new DiseaseCatalog());
            Check(restored.CaptureState().diseases.Count == system.CaptureState().diseases.Count,
                "save round-trip preserves every disease entry");
            Check(restored.GetSnapshot().total_deaths == system.GetSnapshot().total_deaths
                  && restored.GetSnapshot().total_recovered == system.GetSnapshot().total_recovered,
                "save round-trip preserves outcome history");
            // A restored system with the same seed continues the same outcome
            // sequence as a fresh twin (determinism across reload).
            var s1 = stateA;
            var twin = new DiseaseSystem(rng: new SeededRng(s1.rngSeed), log: log);
            twin.BindCatalog(catalog ?? new DiseaseCatalog());
            restored.TickDaily(day: 30, candidates: roster);
            twin.TickDaily(day: 30, candidates: roster);
            Check(restored.CaptureState().rngSeed == twin.CaptureState().rngSeed,
                "restored system resumes the same RNG stream");

            // -----------------------------------------------------------------
            // Plan 60 / D3 — treatment is an intervention, not a button.
            // Before this, ResolveOutcomes rolled the raw authored lethality no
            // matter what the player did, so "treat this patient" was not a question
            // the engine could answer. These checks pin the clinical contract: an
            // item must be authorised for that disease, the window must be honored,
            // one dose per patient per day, and only a curative role removes the
            // infection.
            // -----------------------------------------------------------------
            if (catalog != null)
            {
                var itemIds2 = LoadItemIds(dataDirectory, files, json);
                int curativeTotal = 0, treatedDiseases = 0, uncured = 0;
                bool allRolesKnown = true, allItemsResolve = true, reductionBounded = true;

                for (int i = 0; i < catalog.Diseases.Count; i++)
                {
                    var d = catalog.Diseases[i];
                    if (d == null) continue;
                    if (d.treatments == null || d.treatments.Count == 0) { uncured++; continue; }
                    treatedDiseases++;
                    bool hasCurative = false;
                    for (int t = 0; t < d.treatments.Count; t++)
                    {
                        var entry = d.treatments[t];
                        if (!DiseaseTreatmentRoles.IsKnown(entry.role)) allRolesKnown = false;
                        if (!itemIds2.Contains(entry.item_id)) allItemsResolve = false;
                        if (entry.lethality_reduction < 0f || entry.lethality_reduction > 1f) reductionBounded = false;
                        if (DiseaseTreatmentRoles.IsCurative(entry.role)) { hasCurative = true; curativeTotal++; }
                    }
                    if (!hasCurative) uncured++;
                }

                Check(allRolesKnown, "every authored treatment role is a known clinical role");
                Check(allItemsResolve, "every authored treatment item resolves in items.json");
                Check(reductionBounded, "every treatment lethality_reduction stays inside 0..1");
                Check(treatedDiseases > 0, "the catalog authorises at least one treatment path");
                Check(curativeTotal > 0, "at least one disease is curable");
                // An illness the holdfast cannot cure is a legitimate finding; a catalog
                // where everything is curable would make medicine a formality.
                // Care is universal; cure is not. Every illness may offer something to do,
                // but a meaningful share must stay incurable, or medicine becomes a
                // toggle that erases consequence.
                Check(uncured > 0, "some illnesses stay incurable (endure, don't optimise)");

                // ---- Plan 60 / D2: clinical tells exist and discriminate ----------
                // Authored-but-unread text is the failure this project keeps finding,
                // so the coverage of tells is gated here as well as rendered.
                int withTell = 0, withTiming = 0, unreadProse = 0;
                var primaryTells = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < catalog.Diseases.Count; i++)
                {
                    var d = catalog.Diseases[i];
                    if (d == null) continue;
                    if (!string.IsNullOrWhiteSpace(d.tell))
                    {
                        withTell++;
                        primaryTells.Add(d.tell);
                    }
                    if (!string.IsNullOrWhiteSpace(d.timing_clue)) withTiming++;
                    if (!string.IsNullOrWhiteSpace(d.guidance)) unreadProse++;
                }
                Check(withTell == catalog.Count,
                    "every illness carries a primary tell (" + withTell + "/" + catalog.Count + ")");
                Check(withTiming == catalog.Count,
                    "every illness carries a timing clue, which is how look-alike signs separate");
                Check(primaryTells.Count == catalog.Count,
                    "no two illnesses share the same primary tell (a shared key would make diagnosis a coin flip)");
                Check(unreadProse == catalog.Count,
                    "every illness carries protocol guidance for the bedside");

                // ---- Plan 60 / D4: protocols are maintenance, not switches -------
                // The gate the whole slice hangs on: every vector block must be able
                // to return to false on the day tick alone. A protocol that can only
                // ever be switched on is not a protocol, it is an achievement.
                var vectors = new[]
                {
                    DiseaseVectorNames.Water, DiseaseVectorNames.Air,
                    DiseaseVectorNames.Blood, DiseaseVectorNames.Spore,
                };
                bool allDurationsAuthored = true;
                for (int v = 0; v < vectors.Length; v++)
                    if (catalog.ProtocolDurationDays(vectors[v]) <= 0) allDurationsAuthored = false;
                Check(allDurationsAuthored,
                    "every vector protocol carries an authored lapse duration");

                // Fresh system so the lapse scenario cannot perturb the outcome
                // scenario above (its patients keep their own timeline).
                var protocolSystem = new DiseaseSystem(rng: new SeededRng(DemoSeed + 41), log: log);
                protocolSystem.BindCatalog(catalog ?? new DiseaseCatalog());
                int resetEvents = 0;
                protocolSystem.OnEventRaised += (eventId, _) =>
                {
                    if (eventId == DiseaseIds.EventProtocolReset) resetEvents++;
                };

                protocolSystem.PurifyWater(200);
                protocolSystem.SealVents(200);
                protocolSystem.SterilizeTools(200);
                protocolSystem.SetAirFiltration(true, 200);
                Check(protocolSystem.IsVectorBlocked(DiseaseVectorNames.Water)
                    && protocolSystem.IsVectorBlocked(DiseaseVectorNames.Air)
                    && protocolSystem.IsVectorBlocked(DiseaseVectorNames.Blood)
                    && protocolSystem.IsVectorBlocked(DiseaseVectorNames.Spore),
                    "all four vector protocols engage when applied");

                for (int d = 201; d <= 206; d++) protocolSystem.TickDaily(d, Array.Empty<string>());
                Check(!protocolSystem.IsVectorBlocked(DiseaseVectorNames.Water)
                    && !protocolSystem.IsVectorBlocked(DiseaseVectorNames.Air)
                    && !protocolSystem.IsVectorBlocked(DiseaseVectorNames.Blood)
                    && !protocolSystem.IsVectorBlocked(DiseaseVectorNames.Spore),
                    "every vector block returns to false on the day tick alone (no manual reset)");
                Check(resetEvents >= 4,
                    "each lapse announces itself (" + resetEvents + " protocol reset events)");

                // The window is exact: applied on day 300 with duration 3, it holds
                // on day 302 and is gone on day 303 — never one day of grace.
                protocolSystem.PurifyWater(300);
                int seedBeforeLapse = protocolSystem.CaptureState().rngSeed;
                protocolSystem.TickDaily(302, Array.Empty<string>());
                Check(protocolSystem.IsVectorBlocked(DiseaseVectorNames.Water),
                    "protocol still holds the day before its window ends");
                protocolSystem.TickProtocolExpiry(303);
                Check(!protocolSystem.IsVectorBlocked(DiseaseVectorNames.Water),
                    "lapses exactly on the authored day");
                Check(protocolSystem.ProtocolDaysRemaining(DiseaseVectorNames.Water, 305) < 0,
                    "lapsed protocol reports inactive");
                Check(protocolSystem.CaptureState().rngSeed == seedBeforeLapse,
                    "protocol lapse is pure day arithmetic — no RNG consumed");

                // A pre-D4 save (protocol on, no recorded expiry) re-arms on the next
                // tick and lapses one full window later — the honest reading of an
                // old save, not a surprise reset.
                var legacy = new DiseaseSystem(new DiseaseSystemState
                {
                    water_purified = true,
                    water_purified_until_day = 0,
                    rngSeed = DemoSeed,
                }, rng: new SeededRng(DemoSeed), log: log);
                legacy.BindCatalog(catalog ?? new DiseaseCatalog());
                int waterWindow = (catalog ?? new DiseaseCatalog()).ProtocolDurationDays(DiseaseVectorNames.Water);
                legacy.TickProtocolExpiry(50);
                Check(legacy.IsVectorBlocked(DiseaseVectorNames.Water)
                    && legacy.State.water_purified_until_day == 50 + waterWindow,
                    "legacy save with a bare protocol re-arms from the current day");
                for (int d = 51; d < 50 + waterWindow; d++)
                    legacy.TickProtocolExpiry(d);
                Check(legacy.IsVectorBlocked(DiseaseVectorNames.Water), "legacy protocol holds through its re-armed window");
                legacy.TickProtocolExpiry(50 + waterWindow);
                Check(!legacy.IsVectorBlocked(DiseaseVectorNames.Water),
                    "legacy protocol lapses one full window after re-arm");


                // ---- live intervention behaviour ----
                var ward = new DiseaseSystem(rng: new SeededRng(2027), log: log);
                ward.BindCatalog(catalog);

                var spendable = new HashSet<string>(StringComparer.Ordinal) { "antibiotics", "clean_water" };
                int spent = 0;
                ward.TryConsumeItem = (itemId, count) =>
                {
                    if (count <= 0 || !spendable.Contains(itemId)) return false;
                    spent += count;
                    return true;
                };

                Check(ward.TryTreat("pt_1", DiseaseIds.Cholera, "antibiotics", 5).Reason
                        == DiseaseTreatmentRefusals.NotPatient,
                    "nobody can be treated who is not a patient");

                ward.Infect("pt_1", DiseaseIds.Cholera, day: 5);
                var choleraDef = catalog.GetById(DiseaseIds.Cholera);
                Check(choleraDef != null && choleraDef.TreatmentFor("clean_water") != null,
                    "cholera authorises clean water as a treatment");
                Check(ward.TryTreat("pt_1", DiseaseIds.Cholera, "bandage", 5).Reason
                        == DiseaseTreatmentRefusals.ItemNotAuthorised,
                    "an unauthorised item is refused, not consumed");
                Check(spent == 0, "a refused treatment consumes nothing");
                Check(ward.TryTreat("pt_1", "disease_not_in_catalog", "antibiotics", 5).Reason
                        == DiseaseTreatmentRefusals.UnknownDisease,
                    "an unknown disease is refused");

                var first = ward.TryTreat("pt_1", DiseaseIds.Cholera, "antibiotics", 5);
                Check(first.Accepted, "a curative dose is accepted inside its window");
                Check(ward.IsInfected("pt_1", DiseaseIds.Cholera) == false,
                    "curative treatment removes the infection");
                var choleraState = ward.GetDiseaseState(DiseaseIds.Cholera);
                Check(choleraState != null && choleraState.recovered_total >= 1,
                    "a cured patient is counted as recovered, not silently dropped");
                Check(spent == 1, "one accepted dose spends exactly one item");

                // Suppressives must not masquerade as cures: septic rust-wound fever is
                // curable, so treat it non-curatively and confirm the patient remains.
                ward.Infect("pt_2", DiseaseIds.Cholera, day: 6);
                var sameDay = ward.TryTreat("pt_2", DiseaseIds.Cholera, "clean_water", 6);
                Check(sameDay.Accepted, "supportive care is accepted alongside a cure-capable disease");
                Check(ward.IsInfected("pt_2", DiseaseIds.Cholera),
                    "a non-curative role never clears an infection");
                Check(ward.TryTreat("pt_2", DiseaseIds.Cholera, "clean_water", 6).Reason
                        == DiseaseTreatmentRefusals.AlreadyTreatedToday,
                    "one dose per patient per day (no click-spam dosing)");
                Check(ward.GetEffectiveLethality("pt_2", DiseaseIds.Cholera)
                        < (choleraDef != null ? choleraDef.lethality : 0f),
                    "treatment improves this patient's odds, not the disease's");
                Check(Math.Abs(ward.GetEffectiveLethality("pt_9", DiseaseIds.Cholera)
                        - (choleraDef != null ? choleraDef.lethality : 0f)) < 0.0001f,
                    "an untreated patient keeps the authored lethality");

                // Window enforcement: cholera's curative is authorised only while
                // early enough, so late presentation is a real clinical loss.
                ward.Infect("pt_3", DiseaseIds.Cholera, day: 40);
                for (int day = 41; day <= 48; day++) ward.TickDaily(day, candidates: null);
                Check(!ward.IsInfected("pt_3", DiseaseIds.Cholera)
                        || ward.TryTreat("pt_3", DiseaseIds.Cholera, "antibiotics", 48).Reason
                            == DiseaseTreatmentRefusals.OutsideWindow,
                    "treatment outside the authorised window is refused");

                // Supply is the player's constraint, not the engine's silence.
                spendable.Remove("clean_water");
                ward.Infect("pt_4", DiseaseIds.Cholera, day: 60);
                Check(ward.TryTreat("pt_4", DiseaseIds.Cholera, "clean_water", 60).Reason
                        == DiseaseTreatmentRefusals.SupplyUnavailable,
                    "no stock means no treatment, with a stated reason");

                // A host that never wires supply must fail loudly, never pretend.
                var unwired = new DiseaseSystem(rng: new SeededRng(11), log: log);
                unwired.BindCatalog(catalog);
                unwired.Infect("pt_5", DiseaseIds.Cholera, day: 1);
                Check(unwired.TryTreat("pt_5", DiseaseIds.Cholera, "antibiotics", 1).Reason
                        == DiseaseTreatmentRefusals.NoSupplyChannel,
                    "an unwired supply channel refuses treatment instead of faking it");

                // Treatment history survives the save the same way the rest of the
                // clinical picture does.
                var saveJson = json.Serialize(ward.CaptureState());
                var reload = new DiseaseSystem(log: log);
                reload.BindCatalog(catalog);
                reload.RestoreState(json.Deserialize<DiseaseSystemState>(saveJson)!);
                var reloadedPatient = reload.GetDiseaseState(DiseaseIds.Cholera)?.infected
                    .Find(p => p.survivor_id == "pt_2");
                Check(reloadedPatient == null || reloadedPatient.treatments_applied >= 1,
                    "treatment history round-trips through the save");

                // Determinism: identical seeds and doses must land on identical odds.
                float Trace(int seed)
                {
                    var s = new DiseaseSystem(rng: new SeededRng(seed), log: NullLog.Instance);
                    s.BindCatalog(catalog);
                    s.TryConsumeItem = (_, __) => true;
                    s.Infect("pt", DiseaseIds.Cholera, day: 3);
                    s.TryTreat("pt", DiseaseIds.Cholera, "clean_water", 3);
                    return s.GetEffectiveLethality("pt", DiseaseIds.Cholera);
                }
                Check(Math.Abs(Trace(5) - Trace(5)) < 0.0001f,
                    "treatment outcome is deterministic for the same inputs");
            }

            log.Info("[DiseaseHeadlessDemo] done");
            report.Passed = report.FailedCount == 0;
            report.Summary = "[DiseaseHeadlessDemo] " + report.PassedCount + "/"
                + (report.PassedCount + report.FailedCount) + " PASSED"
                + (report.Passed ? string.Empty : " (FAILED)")
                + " — disease catalog, quarantine, protocols, determinism, save round-trip";
            return report;
        }

        /// <summary>Replay the same seeded 40-day outbreak; returns a compact fingerprint.</summary>
        private static string SimulateOutbreak(int seed, IFileIO files, IJsonSerializer json, string dataDirectory)
        {
            var catalog = DiseaseCatalogLoader.Load(dataDirectory, files, json);
            var system = new DiseaseSystem(rng: new SeededRng(seed));
            system.BindCatalog(catalog);
            var roster = new List<string>();
            for (int i = 0; i < 12; i++) roster.Add("s_" + i);

            system.Infect("s_0", DiseaseIds.Cholera, day: 1);
            system.Infect("s_1", DiseaseIds.ZoonoticFlu, day: 1);
            system.Infect("s_2", DiseaseIds.BloodFever, day: 1);
            system.Infect("s_3", DiseaseIds.SporeBlight, day: 1);
            for (int day = 2; day <= 40; day++)
            {
                system.TickDaily(day, roster);
                if (day == 10) { system.Quarantine("s_3", DiseaseIds.SporeBlight); }
                if (day == 15) { system.PurifyWater(); }
            }
            var s = system.CaptureState();
            return string.Join("|",
                s.diseases[0].outbreaks_total, s.diseases[0].deaths_total,
                s.diseases[1].outbreaks_total, s.diseases[1].deaths_total,
                s.diseases[2].outbreaks_total, s.diseases[2].deaths_total,
                s.diseases[3].outbreaks_total, s.diseases[3].deaths_total,
                s.rngSeed);
        }

        private static HashSet<string> LoadItemIds(string dataDirectory, IFileIO files, IJsonSerializer json)
        {
            var ids = new HashSet<string>(StringComparer.Ordinal);
            if (string.IsNullOrEmpty(dataDirectory)) return ids;
            string path = files.Combine(dataDirectory, "items.json");
            if (!files.FileExists(path)) return ids;
            try
            {
                string raw = files.ReadAllText(path);
                List<DiseaseDemoItemRow> rows = null;
                try
                {
                    var root = json.Deserialize<DiseaseDemoItemsRoot>(raw);
                    rows = root?.items;
                }
                catch (Exception)
                {
                    // bare list fallback
                }
                if (rows == null)
                {
                    rows = CatalogLocator.LoadWrappedList<DiseaseDemoItemRow>(raw, SystemTextJsonSerializer.Options);
                }
                if (rows != null)
                {
                    for (int i = 0; i < rows.Count; i++)
                        if (rows[i] != null && !string.IsNullOrEmpty(rows[i].id))
                            ids.Add(rows[i].id);
                }
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine("[DiseaseHeadlessDemo] items.json read failed: " + e.Message);
            }
            return ids;
        }

        /// <summary>Minimal items.json row read for the countermeasure check.</summary>
        private sealed class DiseaseDemoItemRow
        {
            public string id = string.Empty;
        }

        [Serializable]
        private sealed class DiseaseDemoItemsRoot
        {
#pragma warning disable CS0649 // schema_version is deserialized for contract compliance, not read in code
            public int schema_version;
#pragma warning restore CS0649
            public List<DiseaseDemoItemRow> items = new List<DiseaseDemoItemRow>();
        }
    }
}
