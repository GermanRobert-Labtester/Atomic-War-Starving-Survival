// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Clock;
using Ashfall.Core.Crafting;
using Ashfall.Core.Economy;
using Ashfall.Core.Endgame;
using Ashfall.Core.Events;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Flags;
using Ashfall.Core.Legacy;
using Ashfall.Core.Medical;
using Ashfall.Core.Muster;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;
using Ashfall.Core.Settings;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.UtilityAI;
using Ashfall.Core.Verdict;
using Ashfall.Core.Warlords;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using AtomicWar.GodotApp.Narrative;
using AtomicWar.GodotApp.Settings;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.YearOfAsh;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {

        public static int RunEconomySelfTest(string dataDirectory)
        {
            // (method continues; DSE adapter probes replaced with core-backed
            // MarketSystem + FactionStanceEngine checks — see below)
            var report = EconomyHeadlessDemo.Run(dataDirectory, new GodotLog());
            // Save-integrity probe: tampered saves must be refused (checksum).
            string tmpPath = Path.Combine(
                Path.GetTempPath(), "ashfall_economy_selftest_" + Guid.NewGuid().ToString("N") + ".json"); // DETERMINISM_ALLOWLIST: Selftest scratch file path
            try
            {
                var session = new EconomyHostSession();
                var catalogResult = new GoodsCatalogLoadResult();
                catalogResult.Goods.Add(new GoodDefinition
                {
                    id = "probe_good", displayName = "Probe", category = "misc",
                    basePrice = 5f, volatility = 0.1f, elasticity = 1f
                });
                var probeCatalog = GoodsCatalogLoader.ToCatalog(catalogResult);
                session.Market.BindCatalog(probeCatalog);
                session.Market.TickDay(3, new SeededRng(9));
                if (EconomySaveStore.TrySave(session.CaptureSave(), tmpPath))
                    GD.Print("[PASS] economy save written to temp slot");
                else
                    GD.Print("[FAIL] economy save write failed");

                string raw = File.ReadAllText(tmpPath);
                // Flip the tick count in the payload, whatever its current value.
                string tampered = System.Text.RegularExpressions.Regex.Replace(
                    raw, "\"tickCount\":\\d+", "\"tickCount\":999");
                bool changed = tampered != raw;
                GD.Print(changed ? "[PASS] tamper changed the payload" : "[FAIL] tamper produced no change");
                if (changed)
                {
                    File.WriteAllText(tmpPath, tampered);
                    var loaded = EconomySaveStore.TryLoad(tmpPath);
                    GD.Print(loaded == null
                        ? "[PASS] tampered save refused (checksum)"
                        : "[FAIL] tampered save accepted (no checksum)");
                }
            }
            catch (Exception e)
            {
                GD.Print("[FAIL] save-integrity probe threw: " + e.Message);
            }
            finally
            {
                if (File.Exists(tmpPath)) File.Delete(tmpPath);
            }

            // Legacy-save probe: a bare MarketState (pre-checksum store shape)
            // must migrate, not be silently dropped as corrupt.
            string legacyPath = Path.Combine(
                Path.GetTempPath(), "ashfall_economy_legacy_" + Guid.NewGuid().ToString("N") + ".json"); // DETERMINISM_ALLOWLIST: Selftest scratch file path
            try
            {
                var legacy = new MarketState
                {
                    version = MarketState.Version,
                    day = 7,
                    tickCount = 7,
                    demand = new System.Collections.Generic.List<DemandEntry>
                    {
                        new DemandEntry { itemId = "legacy_good", multiplier = 1.4f }
                    }
                };
                File.WriteAllText(legacyPath, new SystemTextJsonSerializer().Serialize(legacy));
                var legacyLoaded = EconomySaveStore.TryLoad(legacyPath);
                bool legacyOk = legacyLoaded != null && legacyLoaded.day == 7
                    && legacyLoaded.tickCount == 7
                    && legacyLoaded.demand != null && legacyLoaded.demand.Count == 1;
                GD.Print(legacyOk
                    ? "[PASS] legacy bare save migrates (pre-checksum shape)"
                    : "[FAIL] legacy bare save dropped as corrupt");
            }
            catch (Exception e)
            {
                GD.Print("[FAIL] legacy-save probe threw: " + e.Message);
            }
            finally
            {
                if (File.Exists(legacyPath)) File.Delete(legacyPath);
            }

            // Corrupt-save probe: malformed JSON, truncated JSON, empty content, and missing files
            // must be caught safely by EconomySaveStore.TryLoad and return null without throwing.
            string corruptPath = Path.Combine(
                Path.GetTempPath(), "ashfall_economy_corrupt_" + Guid.NewGuid().ToString("N") + ".json"); // DETERMINISM_ALLOWLIST: Selftest scratch file path
            try
            {
                File.WriteAllText(corruptPath, "{ \"Checksum\": \"abc\", \"State\": malformed_json_syntax");
                var corruptLoaded = EconomySaveStore.TryLoad(corruptPath);
                GD.Print(corruptLoaded == null
                    ? "[PASS] malformed JSON save refused (error caught)"
                    : "[FAIL] malformed JSON save returned non-null state");

                File.WriteAllText(corruptPath, "{\"Checksum\":");
                var truncatedLoaded = EconomySaveStore.TryLoad(corruptPath);
                GD.Print(truncatedLoaded == null
                    ? "[PASS] truncated JSON save refused (error caught)"
                    : "[FAIL] truncated JSON save returned non-null state");

                File.WriteAllText(corruptPath, "   \n\t ");
                var emptyLoaded = EconomySaveStore.TryLoad(corruptPath);
                GD.Print(emptyLoaded == null
                    ? "[PASS] empty save file refused"
                    : "[FAIL] empty save file returned non-null state");

                var missingLoaded = EconomySaveStore.TryLoad(corruptPath + ".nonexistent");
                GD.Print(missingLoaded == null
                    ? "[PASS] missing save file refused"
                    : "[FAIL] missing save file returned non-null state");
            }
            catch (Exception e)
            {
                GD.Print("[FAIL] corrupt-save probe threw: " + e.Message);
            }
            finally
            {
                if (File.Exists(corruptPath)) File.Delete(corruptPath);
            }

            // Tuning-integration probe (Candidate A slice 4): the core overlay
            // loaded from the sample JSON must bind into the core market stack
            // and gate scarcity. (Legacy Unity DSE adapter removed; core is the
            // single source of truth.)
            try
            {
                var tuningLoad = Ashfall.Core.Economy.HardcoreEconomyTuningLoader.Load(
                    System.IO.File.ReadAllText(System.IO.Path.Combine(
                        dataDirectory, "hardcore_economy_tuning.json")));
                bool loaded = tuningLoad != null && tuningLoad.IsValid && tuningLoad.Bundle != null;
                GD.Print(loaded
                    ? "[PASS] hardcore tuning JSON loads via the core loader"
                    : "[FAIL] hardcore tuning JSON failed to load");

                var overlay = new Ashfall.Core.Economy.HardcoreEconomyTuning();
                overlay.Apply(tuningLoad!.Bundle!);
                float day5Water = overlay.GetScarcityMultiplier(5, "clean_water");
                bool gates = day5Water > 1.0f && day5Water <= 2.5f + 1e-6f;
                GD.Print(gates
                    ? $"[PASS] core overlay gates scarcity (day 5 clean_water x{day5Water:0.00})"
                    : $"[FAIL] core overlay scarcity gate wrong ({day5Water:0.00})");
            }
            catch (System.Exception e)
            {
                GD.Print("[FAIL] tuning-integration probe threw: " + e.Message);
            }

            // Core-market probe: demand nudges, save/restore round-trip, and the
            // shortage gate must all operate on the engine-agnostic MarketSystem.
            try
            {
                var market = new MarketSystem();
                var coreMarket = new MarketSystem();
                market.AdjustDemand("probe_water", 0.5f);
                bool nudged = market.GetDemandMultiplier("probe_water") == 1.5f;
                GD.Print(nudged
                    ? "[PASS] core MarketSystem demand nudge (AdjustDemand)"
                    : "[FAIL] MarketSystem demand nudge broken");

                var save = market.CaptureState();
                var fresh = new MarketSystem();
                fresh.RestoreState(save);
                bool roundtrip = fresh.GetDemandMultiplier("probe_water") == 1.5f;
                GD.Print(roundtrip
                    ? "[PASS] MarketSystem save/restore round-trips demand"
                    : "[FAIL] MarketSystem save/restore lost demand");
            }
            catch (System.Exception e)
            {
                GD.Print("[FAIL] core-market probe threw: " + e.Message);
            }

            // Reload-continuity probe: mid-sequence save via the REAL store slot,
            // reload, continue — the resumed trajectory must match an
            // uninterrupted run hash-for-hash.
            string continuityPath = Path.Combine(
                Path.GetTempPath(), "ashfall_economy_continuity_" + Guid.NewGuid().ToString("N") + ".json"); // DETERMINISM_ALLOWLIST: Selftest scratch file path
            try
            {
                var catalogResult = new GoodsCatalogLoadResult();
                catalogResult.Goods.Add(new GoodDefinition
                {
                    id = "cont_good", displayName = "Cont", category = "misc",
                    basePrice = 7f, volatility = 0.4f, elasticity = 1.4f
                });
                var contCatalog = GoodsCatalogLoader.ToCatalog(catalogResult);

                var uninterrupted = new MarketSystem();
                uninterrupted.BindCatalog(contCatalog);
                for (int day = 1; day <= 40; day++)
                    uninterrupted.TickDay(day, new SeededRng(31337));
                string expected = SaveChecksum.Compute(uninterrupted.CaptureState());

                var sliced = new MarketSystem();
                sliced.BindCatalog(contCatalog);
                for (int day = 1; day <= 20; day++)
                    sliced.TickDay(day, new SeededRng(31337));
                bool saved = EconomySaveStore.TrySave(sliced.CaptureState(), continuityPath);
                var reloaded = EconomySaveStore.TryLoad(continuityPath);
                var resumed = new MarketSystem();
                resumed.BindCatalog(contCatalog);
                if (reloaded != null) resumed.RestoreState(reloaded);
                for (int day = 21; day <= 40; day++)
                    resumed.TickDay(day, new SeededRng(31337));

                bool continuity = saved && reloaded != null
                    && SaveChecksum.Compute(resumed.CaptureState()) == expected;
                GD.Print(continuity
                    ? "[PASS] reload-continuity: resumed trajectory matches uninterrupted run"
                    : "[FAIL] reload-continuity: trajectory diverged");
            }
            catch (Exception e)
            {
                GD.Print("[FAIL] reload-continuity probe threw: " + e.Message);
            }
            finally
            {
                if (File.Exists(continuityPath)) File.Delete(continuityPath);
            }

            // EconomyHostSession TickDemo probe:
            try
            {
                var session = new EconomyHostSession();
                bool stateChangedInvoked = false;
                session.StateChanged += () => stateChangedInvoked = true;

                int initialDay = session.Market.Day;
                int initialTicks = (int)session.Market.TickCount;

                string eventMsg = session.TickDemo(3);
                bool dayUpdated = session.Market.Day == initialDay + 3;
                bool ticksUpdated = session.Market.TickCount == initialTicks + 3;
                bool eventUpdated = session.LastEvent == $"Advanced 3 days to Day {session.Market.Day}." && eventMsg == session.LastEvent;
                bool stateUpdated = stateChangedInvoked && session.IsDirty && session.StateVersion > 0;

                string eventMsg2 = session.TickDemo(2);
                bool day2Updated = session.Market.Day == initialDay + 5;
                bool ticks2Updated = session.Market.TickCount == initialTicks + 5;
                bool event2Updated = session.LastEvent == $"Advanced 2 days to Day {session.Market.Day}." && eventMsg2 == session.LastEvent;

                bool tickDemoOk = dayUpdated && ticksUpdated && eventUpdated && stateUpdated && day2Updated && ticks2Updated && event2Updated;
                if (tickDemoOk)
                {
                    report.PassedCount++;
                    report.Checks.Add(new HeadlessCheck { Name = "EconomyHostSession.TickDemo probe", Passed = true });
                    GD.Print($"[PASS] EconomyHostSession.TickDemo advances day ({session.Market.Day}) and updates LastEvent/StateChanged");
                }
                else
                {
                    report.FailedCount++;
                    report.Passed = false;
                    report.Checks.Add(new HeadlessCheck { Name = "EconomyHostSession.TickDemo probe", Passed = false });
                    GD.Print($"[FAIL] EconomyHostSession.TickDemo failed: day={dayUpdated} ticks={ticksUpdated} event={eventUpdated} state={stateUpdated}");
                }
            }
            catch (Exception e)
            {
                report.FailedCount++;
                report.Passed = false;
                report.Checks.Add(new HeadlessCheck { Name = "EconomyHostSession.TickDemo probe exception", Passed = false });
                GD.Print("[FAIL] EconomyHostSession.TickDemo probe threw: " + e.Message);
            }

            // EconomyHostSession BarterDemo probe:
            try
            {
                var session = EconomyHostSession.Create(dataDirectory);
                bool stateChangedInvoked = false;
                session.StateChanged += () => stateChangedInvoked = true;

                // 1. Accepted barter: 20 scrap_metal for clean_water
                string barterMsg = session.BarterDemo("scrap_metal", 20, "clean_water");
                bool barterAccepted = barterMsg.StartsWith("Bartered 20x scrap_metal for ")
                    && session.LastEvent == barterMsg
                    && stateChangedInvoked
                    && session.IsDirty;

                // 2. Rejected barter: unknown give good
                string rejMsg1 = session.BarterDemo("nonexistent_good_xyz", 5, "clean_water");
                bool rej1Ok = rejMsg1 == "Barter rejected: unknown give good." && session.LastEvent == rejMsg1;

                // 3. Rejected barter: non-positive quantity
                string rejMsg2 = session.BarterDemo("scrap_metal", 0, "clean_water");
                bool rej2Ok = rejMsg2 == "Barter rejected: quantity must be > 0." && session.LastEvent == rejMsg2;

                bool barterDemoOk = barterAccepted && rej1Ok && rej2Ok;
                if (barterDemoOk)
                {
                    report.PassedCount++;
                    report.Checks.Add(new HeadlessCheck { Name = "EconomyHostSession.BarterDemo probe", Passed = true });
                    GD.Print("[PASS] EconomyHostSession.BarterDemo exchanges goods, handles rejections, and notifies StateChanged");
                }
                else
                {
                    report.FailedCount++;
                    report.Passed = false;
                    report.Checks.Add(new HeadlessCheck { Name = "EconomyHostSession.BarterDemo probe", Passed = false });
                    GD.Print($"[FAIL] EconomyHostSession.BarterDemo probe failed: accepted={barterAccepted} rejUnknown={rej1Ok} rejZero={rej2Ok}");
                }
            }
            catch (Exception e)
            {
                report.FailedCount++;
                report.Passed = false;
                report.Checks.Add(new HeadlessCheck { Name = "EconomyHostSession.BarterDemo probe exception", Passed = false });
                GD.Print("[FAIL] EconomyHostSession.BarterDemo probe threw: " + e.Message);
            }

            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("economy_selftest", report);
        }

    }
}
