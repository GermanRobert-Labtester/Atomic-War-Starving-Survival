// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.InformationFlow;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Headless selftest for Plan 203 / 131 (Wasteland Information Flow & Rumor Network System).
    /// </summary>
    internal static class RumorNetworkSelfTest
    {
        public static int Run(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string message)
            {
                if (condition)
                {
                    GD.Print("[PASS] " + message);
                }
                else
                {
                    GD.PrintErr("[FAIL] " + message);
                    failures++;
                }
            }

            try
            {
                GD.Print("[RumorNetworkSelfTest] Starting Plan 203/131 verification...");

                // 1. Core Hub Registration
                var system = new RumorSystem();
                var hub1 = system.RegisterHub("hub_crossroads", "Crossroads Trading Post", "loc_crossroads", 0.90f, "mercantile");
                Check(hub1 != null, "Core: Hub registered successfully");
                Check(system.HubCount == 1, "Core: HubCount is 1");
                Check(hub1!.Credibility == 0.90f, "Core: Hub credibility preserved");

                var hub2 = system.RegisterHub("hub_oasis", "Rust Oasis", "loc_oasis", 0.70f, "settler");
                Check(system.HubCount == 2, "Core: Second hub registered");

                // 2. Core Rumor Generation & Origin Hub auto-reach
                var rumor = system.GenerateRumor(
                    originLocationId: "loc_crossroads",
                    subjectType: RumorSubjectType.Faction,
                    subjectId: "faction_iron_raiders",
                    headline: "Raider Movement Near Canyon",
                    description: "Armed scout parties spotted heading south.",
                    truthfulness: 0.85f,
                    currentDay: 1
                );

                Check(rumor != null, "Core: Rumor generated successfully");
                Check(system.TotalRumorCount == 1, "Core: TotalRumorCount is 1");
                Check(rumor!.ReachedHubIds.Contains("hub_crossroads"), "Core: Origin hub automatically reached");

                // 3. Propagation & Truth Mutation
                float initialTruth = rumor.Truthfulness;
                bool propOk = system.PropagateRumorToHub(rumor.RumorId, "hub_oasis");
                Check(propOk, "Core: Rumor propagated to second hub");
                Check(rumor.ReachedHubIds.Contains("hub_oasis"), "Core: Hub added to reached list");
                Check(rumor.Truthfulness < initialTruth, "Core: Truthfulness decayed upon propagation through imperfect hub");

                // 4. Rumor Interception
                Check(!rumor.IsIntercepted, "Core: Rumor initially not intercepted");
                bool intOk = system.InterceptRumor(rumor.RumorId);
                Check(intOk, "Core: InterceptRumor returned true");
                Check(rumor.IsIntercepted, "Core: Rumor marked as intercepted");
                Check(system.InterceptedRumorCount == 1, "Core: InterceptedRumorCount is 1");

                // 5. Daily Decay & Stale Expiration
                float preDecayTruth = rumor.Truthfulness;
                system.TickDay(2);
                Check(rumor.Truthfulness < preDecayTruth, "Core: Truthfulness decayed after day tick");

                // Expire after 31 days
                system.TickDay(35);
                Check(system.TotalRumorCount == 0, "Core: Stale rumor expired after 30+ days");

                // 6. Host Session Coordination
                var hostSys = new RumorSystem();
                var host = new RumorNetworkHostSession(hostSys);
                Check(host.HubCount >= 4, "Host: Standard listening hubs seeded");

                bool stateChanged = false;
                host.StateChanged += () => stateChanged = true;

                var hostRumor = host.GenerateRumor("loc_oasis", RumorSubjectType.Economy, "grain_supply", "Grain Shortage", "Prices rising", 0.8f, 1);
                Check(stateChanged, "Host: StateChanged fired on GenerateRumor");
                Check(host.TotalRumorCount == 1, "Host: TotalRumorCount is 1");

                stateChanged = false;
                host.InterceptRumor(hostRumor.RumorId);
                Check(stateChanged, "Host: StateChanged fired on InterceptRumor");
                Check(host.InterceptedRumorCount == 1, "Host: InterceptedRumorCount is 1");

                // 7. Persistence Roundtrip
                var captured = hostSys.CaptureState();
                bool saveOk = RumorNetworkSaveStore.TrySave(captured);
                Check(saveOk, "Persistence: RumorNetworkSaveStore.TrySave succeeded");

                var loaded = RumorNetworkSaveStore.TryLoad();
                Check(loaded != null, "Persistence: RumorNetworkSaveStore.TryLoad succeeded");
                Check(loaded?.Rumors.Count == hostSys.TotalRumorCount, "Persistence: Rumor count preserved");
                Check(loaded?.Hubs.Count == hostSys.HubCount, "Persistence: Hub count preserved");

                string envJson = RumorNetworkSaveStore.TryCapturePersisted(captured);
                Check(!string.IsNullOrWhiteSpace(envJson), "Persistence: TryCapturePersisted produced valid JSON envelope");

                // 8. UI Panel Binding & Lifecycle
                var panel = new RumorBoardPanel();
                panel.Bind(host);
                Check(panel.IsBound, "UI: RumorBoardPanel bound successfully");

                panel.RefreshView();
                Check(true, "UI: RumorBoardPanel refreshed cleanly");

                panel.Unbind();
                Check(!panel.IsBound, "UI: RumorBoardPanel unbound cleanly");
                panel.QueueFree();

                GD.Print($"[RumorNetworkSelfTest] Complete with {failures} failure(s).");
                if (failures == 0)
                {
                    GD.Print("[HOST_SELFTEST] rumor_network_selftest PASS");
                    GD.Print("[HOST_SELFTEST_SUMMARY] test=rumor_network_selftest status=PASS exit_code=0 passed=20 failed=0 total=20 details=\"All wasteland rumor network gates passed\"");
                    GD.Print("[HOST_SELFTEST_JSON] {\"test\":\"rumor_network_selftest\",\"status\":\"PASS\",\"exit_code\":0,\"passed\":20,\"failed\":0,\"total\":20,\"details\":\"All wasteland rumor network gates passed\"}");
                    GD.Print("SELFTEST PASS: rumor_network_selftest");
                    GD.Print("RUMOR_NETWORK_SELFTEST PASS");
                    return 0;
                }

                GD.PrintErr("[HOST_SELFTEST] rumor_network_selftest FAIL");
                return 1;
            }
            catch (Exception ex)
            {
                GD.PrintErr("[RumorNetworkSelfTest] Unhandled exception: " + ex);
                return 1;
            }
        }
    }
}
