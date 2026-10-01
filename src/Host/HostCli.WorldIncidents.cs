// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : WorldIncidentsSelfTest
// Subsystem          : World incidents (events.json) — third fallback of the
//                      per-day decision stream (arc -> echo -> incident)
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp
{
    public static class HostCliWorldIncidents
    {
        private sealed class RecordingPort : IWorldIncidentConsequencePort
        {
            public readonly List<string> Log = new List<string>();
            public readonly HashSet<string> Flags = new HashSet<string>(StringComparer.Ordinal);
            public string Weather = string.Empty;

            public bool CanApplyMorale(double delta, out string reason)
            { reason = string.Empty; return true; }
            public void ApplyMorale(double delta)
            { Log.Add($"morale:{delta}"); }
            public bool CanGrantItem(string itemId, int amount, out string reason)
            { reason = string.Empty; return true; }
            public void GrantItem(string itemId, int amount)
            { Log.Add($"item:{itemId}:{amount}"); }
            public bool CanApplyNeedDelta(string needId, double delta, out string reason)
            { reason = string.Empty; return true; }
            public void ApplyNeedDelta(string needId, double delta)
            { Log.Add($"need:{needId}:{delta}"); }
            public bool CanSetWorldFlag(string flagId, bool value, out string reason)
            { reason = string.Empty; return true; }
            public void SetWorldFlag(string flagId, bool value)
            {
                if (value) Flags.Add(flagId); else Flags.Remove(flagId);
                Log.Add($"flag:{flagId}:{value}");
            }
            public bool IsWorldFlagSet(string flagId) => Flags.Contains(flagId);
            public bool WeatherIs(string weatherKindName)
                => string.Equals(Weather, weatherKindName, StringComparison.Ordinal);
            public bool CanApplyFactionStanding(string canonicalFactionId, int delta, out string reason)
            { reason = string.Empty; return true; }
            public void ApplyFactionStanding(string canonicalFactionId, int delta)
            { Log.Add($"faction:{canonicalFactionId}:{delta}"); }
        }

        private static WorldIncidentSystem Engine(
            IReadOnlyList<WorldIncidentDefinition> catalog,
            RecordingPort port,
            params string[] only)
        {
            var subset = only.Length == 0
                ? catalog
                : catalog.Where(i => only.Contains(i.Id)).ToList();
            return new WorldIncidentSystem(subset) { Consequences = port };
        }

        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] World Incidents (events.json) Self-Test ===");
            int passed = 0;
            const int total = 12;

            try
            {
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir)
                    ? dataDir
                    : CatalogPath.ResolveDataDir());

                // Check 1: real catalog loads with zero errors.
                var load = WorldIncidentCatalogLoader.LoadDetailed(
                    dataRoot, new FileSystemIO(), new SystemTextJsonSerializer());
                if (load.IsSuccess && load.Incidents.Count == 240 && load.SchemaVersion == 1)
                {
                    Console.WriteLine($"[PASS] Check 1: Catalog loaded {load.Incidents.Count} world incidents, 0 errors, schema_version 1.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Catalog load mismatch (count={load.Incidents.Count}, errors={load.Errors.Count}, first='{(load.Errors.Count > 0 ? load.Errors[0] : "")}').");
                }

                var port = new RecordingPort();
                var catalog = load.Incidents;

                // Check 2: minDay gating on the real catalog.
                var engine = Engine(catalog, port);
                var dryPipes = engine.Find("water_shortage");
                if (dryPipes != null && !engine.IsEligible(dryPipes, 3) && engine.IsEligible(dryPipes, 4))
                {
                    Console.WriteLine("[PASS] Check 2: minDay gating holds (water_shortage ineligible day 3, eligible day 4).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: minDay gating failed for water_shortage.");
                }

                // Check 3: same-seed determinism across the day-8+ window.
                var idsA = new List<string>();
                var idsB = new List<string>();
                for (int day = 8; day <= 40; day++)
                {
                    var a = Engine(catalog, port);
                    var b = Engine(catalog, port);
                    var pickA = a.SelectForDay(day, new SeededRng(900 + day));
                    if (pickA != null) idsA.Add(pickA.Id);
                    var pickB = b.SelectForDay(day, new SeededRng(900 + day));
                    if (pickB != null) idsB.Add(pickB.Id);
                }
                if (idsA.Count > 0 && idsA.SequenceEqual(idsB))
                {
                    Console.WriteLine($"[PASS] Check 3: Same-seed draws are deterministic ({idsA.Count} surfaced days, ids identical).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: Same-seed divergence (A={idsA.Count}, B={idsB.Count}).");
                }

                // Check 4: non-executable typed effect fails closed at load.
                var trade = catalog.FirstOrDefault(i => i.Id == "contaminated_water_trade");
                var tradeChoice = trade?.Choices.FirstOrDefault(c => c.ChoiceId == "trade_water");
                if (tradeChoice != null && !tradeChoice.IsExecutable &&
                    tradeChoice.ValidationError.Contains("has no bound consequence owner"))
                {
                    Console.WriteLine("[PASS] Check 4: Unbound typed effect ('remove_water'/'add_item') marks the choice non-executable with a validation error.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: Typed-effect gate did not fail closed (found={trade != null}, choice={tradeChoice != null}, executable={tradeChoice?.IsExecutable}, error='{tradeChoice?.ValidationError}', effects={string.Join("|", tradeChoice?.Effects.Select(e => e.Type + "/" + e.ItemId) ?? Enumerable.Empty<string>())}).");
                }

                // Check 5: informational incident auto-resolves (never a modal).
                var informational = Engine(catalog, port, "fallout_storm");
                var surfaced = informational.SelectForDay(1, new SeededRng(1));
                if (surfaced != null && !informational.HasPendingIncident && informational.IsResolved("fallout_storm"))
                {
                    Console.WriteLine("[PASS] Check 5: Informational incident (no choices) auto-resolves on surface, never blocks the pending slot.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 5: Informational incident did not auto-resolve.");
                }

                // Check 6: pending decision + resolve through the recording port.
                port.Weather = "BlackRain";
                var decisions = Engine(catalog, port, "weather_black_rain_contamination");
                var pending = decisions.SelectForDay(4, new SeededRng(2));
                if (pending != null && decisions.HasPendingIncident)
                {
                    var result = decisions.Resolve(pending.Id, "pour_out", 4);
                    if (result.Succeeded &&
                        port.Log.Contains("morale:-5") &&
                        port.Log.Contains("item:dirty_water:-5"))
                    {
                        Console.WriteLine("[PASS] Check 6: Pending decision resolved; morale and item effects routed through the consequence port.");
                        passed++;
                    }
                    else
                    {
                        Console.WriteLine($"[FAIL] Check 6: Resolve mismatch (succeeded={result.Succeeded}, reason='{result.Reason}', log=[{string.Join(",", port.Log)}]).");
                    }
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 6: No pending decision surfaced.");
                }

                // Check 7: once-only resolution.
                var again = decisions.Resolve("weather_black_rain_contamination", "boil_it", 5);
                if (again.Status == WorldIncidentResolutionStatus.AlreadyResolved)
                {
                    Console.WriteLine("[PASS] Check 7: Resolved incidents are once-only (second resolve is AlreadyResolved).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 7: Re-resolve returned {again.Status} ('{again.Reason}').");
                }

                // Check 8: the silent_knock chain — dose exposure, world flag,
                // and a weight-0 scheduled follow-up that fires on its due day.
                port.Weather = "FalloutStorm";
                var scheduler = Engine(
                    catalog, port, "silent_knock_part1", "silent_knock_part2a_wakes");
                var knock = scheduler.SelectForDay(35, new SeededRng(3));
                var chainOk = false;
                if (knock != null && knock.Id == "silent_knock_part1")
                {
                    var opened = scheduler.Resolve(knock.Id, "open_hatch", 35);
                    var followUp = scheduler.SelectForDay(37, new SeededRng(4));
                    chainOk = opened.Succeeded &&
                        port.Log.Contains("need:radiation:12") &&
                        port.Log.Contains("flag:stranger_inside:True") &&
                        followUp != null && followUp.Id == "silent_knock_part2a_wakes";
                }
                if (chainOk)
                {
                    Console.WriteLine("[PASS] Check 8: silent_knock chain: radiation dose + flag routed, weight-0 follow-up surfaced on its scheduled day.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 8: silent_knock chain broke (dose/flag/schedule).");
                }

                // Check 9: RequiredFlagId fails closed until the flag is set.
                // A fresh port keeps this check independent of the flag writes
                // performed by the silent_knock chain above.
                var gatePort = new RecordingPort();
                var gated = Engine(catalog, gatePort, "silent_knock_part2a_wakes");
                var gatedRow = gated.Find("silent_knock_part2a_wakes");
                var blocked = gatedRow != null && !gated.IsEligible(gatedRow, 37);
                gatePort.Flags.Add("stranger_inside");
                var allowed = gatedRow != null && gated.IsEligible(gatedRow, 37);
                if (blocked && allowed)
                {
                    Console.WriteLine("[PASS] Check 9: RequiredFlagId gate is fail-closed until the flag is set.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 9: RequiredFlagId gating failed.");
                }

                // Check 10: unsupported authored conditions stay ineligible.
                var unsupportedEngine = Engine(catalog, port, "apprenticeship_completion_rough_repairs");
                var unsupportedRow = unsupportedEngine.Find("apprenticeship_completion_rough_repairs");
                if (unsupportedRow != null &&
                    !unsupportedEngine.IsEligible(unsupportedRow, 100) &&
                    unsupportedEngine.SelectForDay(100, new SeededRng(7)) == null)
                {
                    Console.WriteLine("[PASS] Check 10: Unsupported authored conditions (child cohort, ration state, ...) stay ineligible.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 10: Unsupported-condition row fired.");
                }

                // Check 11: save round-trip preserves pending and resolved state.
                port.Weather = "BlackRain";
                var persisted = Engine(catalog, port, "weather_black_rain_contamination");
                var rtPending = persisted.SelectForDay(4, new SeededRng(8));
                var captured = persisted.CaptureState();
                var restored = new WorldIncidentSystem(
                    persisted.Catalog) { Consequences = port };
                restored.RestoreState(captured);
                if (rtPending != null &&
                    restored.HasPendingIncident &&
                    restored.PendingIncident?.Id == rtPending.Id)
                {
                    var restoredResult = restored.Resolve(rtPending.Id, "pour_out", 5);
                    if (restoredResult.Succeeded && restored.IsResolved(rtPending.Id))
                    {
                        Console.WriteLine("[PASS] Check 11: Save round-trip preserves the pending decision and resolution history.");
                        passed++;
                    }
                    else
                    {
                        Console.WriteLine("[FAIL] Check 11: Restored engine could not resolve the pending decision.");
                    }
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 11: Save round-trip lost the pending decision.");
                }

                // Check 12: the week-1 pool has decision-capable incidents, so
                // the third fallback can never starve days 1-7.
                var weekOneDecisions = catalog
                    .Where(i => i.MinDay <= 7 && i.HasExecutableChoices)
                    .ToList();
                if (weekOneDecisions.Count >= 5)
                {
                    Console.WriteLine($"[PASS] Check 12: {weekOneDecisions.Count} decision-capable incidents are eligible inside days 1-7.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 12: Only {weekOneDecisions.Count} decision-capable incidents in days 1-7.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unexpected exception during WorldIncidentsSelfTest: {ex.Message}");
            }

            Console.WriteLine($"=== WorldIncidentsSelfTest: {passed}/{total} checks passed. ===");
            return passed == total ? 0 : 1;
        }
    }
}
