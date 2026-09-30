// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : PatrolEncounterIntegritySelfTest
// Subsystem          : Travel / patrol encounter data integrity.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AtomicWar.GodotApp
{
    public static class HostCliPatrolEncounterIntegrity
    {
        private static void Check(bool ok, string label)
        {
            if (ok) Console.WriteLine($"[PASS] {label}");
            else Console.WriteLine($"[FAIL] {label}");
        }

        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Patrol Encounter Integrity Self-Test ===");
            int passed = 0;
            const int total = 11;
            try
            {
                string root = !string.IsNullOrEmpty(dataDir) && System.IO.Directory.Exists(dataDir)
                    ? dataDir : CatalogPath.ResolveDataDir();
                var io = new FileSystemIO();
                var catalog = TravelEncounterCatalog.LoadFromDirectory(root, io);

                // 1. The live catalog the expedition owner plays from is non-empty.
                Check(catalog != null && catalog.Count > 0,
                    $"Check 1: live travel encounter catalog loaded ({catalog?.Count ?? 0} rows).");
                passed += catalog != null && catalog.Count > 0 ? 1 : 0;

                // 2. The authored data passes the previously-unconsumed validator.
                var session = new PatrolEncounterIntegrityHostSession(() => catalog);
                var errors = session.Validate();
                Check(errors.Count == 0 && session.IsClean,
                    $"Check 2: authored encounters are structurally clean ({session.EncountersValidated} rows).");
                passed += errors.Count == 0 ? 1 : 0;

                // 3. Validation covered every row, not a sample.
                Check(session.EncountersValidated == (catalog?.Count ?? 0) && session.ValidationCount == 1,
                    "Check 3: every row is validated exactly once per pass.");
                passed += session.EncountersValidated == (catalog?.Count ?? 0) ? 1 : 0;

                // 4. Reference sets are honoured when supplied.
                var factions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var e in catalog!.Encounters)
                    if (!string.IsNullOrEmpty(e.FactionId)) factions.Add(e.FactionId);
                var withRefs = new PatrolEncounterIntegrityHostSession(
                    () => catalog, () => factions, () => null);
                withRefs.Validate();
                Check(withRefs.IsClean,
                    "Check 4: faction references resolve against the real faction set.");
                passed += withRefs.IsClean ? 1 : 0;

                // 5. A duplicate encounter id is caught.
                TravelEncounterDefinition one = null!;
                foreach (var e in catalog.Encounters)
                {
                    if (e != null && e.Id != null && e.Id.StartsWith("enc_patrol_", StringComparison.OrdinalIgnoreCase))
                    { one = e; break; }
                }
                Check(one != null, $"Probe fixture source: patrol rows present ({session.PatrolRowsValidated}).");
                passed += one != null ? 1 : 0;
                var dupJson = System.Text.Json.JsonSerializer.Serialize(new Dictionary<string, object?>
                {
                    ["encounters"] = new List<Dictionary<string, object?>> { ToDict(one), ToDict(one) }
                });
                var dup = TravelEncounterCatalog.LoadFromJson(dupJson);
                var dupSession = new PatrolEncounterIntegrityHostSession(() => dup);
                dupSession.Validate();
                Check(!dupSession.IsClean && dupSession.HasErrorContaining("duplicate"),
                    $"Check 5: a duplicate encounter id is reported ({dupSession.ErrorCount} error(s)).");
                passed += !dupSession.IsClean ? 1 : 0;

                // 6. Patrol coverage is reported honestly (the validator is patrol-only).
                Check(session.PatrolRowsValidated > 0
                      && session.PatrolRowsValidated <= catalog.Count
                      && session.StatusLine().Contains("patrol row", StringComparison.Ordinal),
                    $"Check 6: {session.PatrolRowsValidated}/{catalog.Count} rows are patrol rows and were inspected.");
                passed += session.PatrolRowsValidated > 0 ? 1 : 0;

                // 6b. A patrol row stripped of its choices is caught.
                var stripped = PatrolRowJson(one, choices: new List<Dictionary<string, object?>>());
                var emptySession = new PatrolEncounterIntegrityHostSession(
                    () => TravelEncounterCatalog.LoadFromJson(stripped));
                emptySession.Validate();
                Check(!emptySession.IsClean && emptySession.HasErrorContaining("choices"),
                    "Check 6b: a patrol row with too few choices is reported.");
                passed += !emptySession.IsClean ? 1 : 0;

                // 7. A dangling faction reference is caught.
                var ghost = PatrolRowJson(one, null, "faction_that_was_never_authored");
                var ghostSession = new PatrolEncounterIntegrityHostSession(
                    () => TravelEncounterCatalog.LoadFromJson(ghost),
                    () => new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "iron_garrison" },
                    () => null);
                ghostSession.Validate();
                Check(ghostSession.HasErrorContaining("unknown faction"),
                    "Check 7: a patrol faction id outside the real set is reported.");
                passed += ghostSession.HasErrorContaining("unknown faction") ? 1 : 0;

                // 8. Validation is read-only: the catalog is unchanged.
                int rowsBefore = catalog.Count;
                session.Validate();
                session.Validate();
                Check(catalog.Count == rowsBefore && session.ValidationCount == 3,
                    "Check 8: validation never drops or edits a row.");
                passed += catalog.Count == rowsBefore ? 1 : 0;

                // 9. Repeat validation is deterministic and the status is truthful.
                var first = string.Join("|", session.Errors);
                session.Validate();
                Check(string.Join("|", session.Errors) == first
                      && session.StatusLine().StartsWith("encounter integrity:", StringComparison.Ordinal),
                    "Check 9: reports are deterministic and truthfully worded.");
                passed += 1;
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex}"); }
            Console.WriteLine($"=== Patrol Encounter Integrity Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }

        private static TravelEncounterDefinition CloneOf(TravelEncounterDefinition source)
            => Def(ToDict(source));

        /// <summary>
        /// Re-serialises the REAL patrol row, optionally with a replaced choice list
        /// or faction id, so injected defects stay inside the validator's patrol
        /// scope instead of being skipped as non-patrol data.
        /// </summary>
        private static string PatrolRowJson(
            TravelEncounterDefinition source,
            List<Dictionary<string, object?>>? choices = null,
            string? factionId = null)
        {
            var node = JsonSerializer.SerializeToNode(source)!.AsObject();
            node["id"] = source.Id;
            node["category"] = "Human";
            if (factionId != null) node["faction_id"] = factionId;
            if (choices != null) node["choices"] = JsonSerializer.SerializeToNode(choices);
            var clone = JsonNode.Parse(node.ToJsonString())!;
            var wrapper = new JsonObject { ["encounters"] = new JsonArray { clone } };
            return wrapper.ToJsonString(new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        private static Dictionary<string, object?> ToDict(TravelEncounterDefinition d)
            => new Dictionary<string, object?>
            {
                ["id"] = d.Id,
                ["title"] = d.Title,
                ["category"] = d.Category,
                ["faction_id"] = d.FactionId,
                ["cooldown_days"] = d.CooldownDays,
                ["choices"] = new List<Dictionary<string, object?>>(),
            };

        private static TravelEncounterDefinition Def(Dictionary<string, object?> row)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(new Dictionary<string, object?>
            {
                ["encounters"] = new List<Dictionary<string, object?>> { row }
            });
            var catalog = TravelEncounterCatalog.LoadFromJson(json);
            return catalog.Count > 0 ? catalog.Encounters[0] : new TravelEncounterDefinition();
        }

        private static TravelEncounterCatalog CatalogOf(params TravelEncounterDefinition[] rows)
        {
            var list = new List<Dictionary<string, object?>>();
            foreach (var r in rows) list.Add(ToDict(r));
            var json = System.Text.Json.JsonSerializer.Serialize(
                new Dictionary<string, object?> { ["encounters"] = list });
            return TravelEncounterCatalog.LoadFromJson(json);
        }
    }
}
