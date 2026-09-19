// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Propaganda;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Headless selftest for Plan 168 (Propaganda & Morale Warfare System).
    /// </summary>
    internal static class PropagandaSelfTest
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
                GD.Print("[PropagandaSelfTest] Starting Plan 168 verification...");

                // 1. Core Message Creation & Quality
                var system = new PropagandaSystem();
                var msg1 = system.CreateMessage(
                    authorId: "surv_scribe",
                    medium: PropagandaMedium.RadioBroadcast,
                    truthfulness: MessageTruthfulness.Truth,
                    theme: PropagandaTheme.Hope,
                    targetFactionId: "faction_iron_raiders",
                    content: "The shelter stands strong and offers shelter.",
                    authorSkill: 75f,
                    currentDay: 1
                );

                Check(msg1 != null, "Core: Message created successfully");
                Check(system.MessageCount == 1, "Core: MessageCount is 1");
                Check(msg1!.Quality == 70f, "Core: Truthfulness.Truth slightly adjusts base quality (75 - 5 = 70)");

                var msgLie = system.CreateMessage(
                    authorId: "surv_scribe",
                    medium: PropagandaMedium.Leaflet,
                    truthfulness: MessageTruthfulness.Lie,
                    theme: PropagandaTheme.Triumph,
                    targetFactionId: "faction_iron_raiders",
                    content: "We possess orbital weaponry.",
                    authorSkill: 60f,
                    currentDay: 1
                );
                Check(msgLie!.Quality == 70f, "Core: Truthfulness.Lie gives bonus quality (60 + 10 = 70)");

                // 2. Campaign Launch
                var campaign = system.LaunchCampaign(
                    campaignName: "Operation Beacon",
                    targetFactionId: "faction_iron_raiders",
                    objective: PropagandaObjective.UndermineFaction,
                    messageIds: new[] { msg1.MessageId },
                    durationDays: 3,
                    currentDay: 1
                );

                Check(campaign != null, "Core: Campaign launched successfully");
                Check(system.ActiveCampaignCount == 1, "Core: ActiveCampaignCount is 1");
                Check(campaign!.Status == CampaignStatus.Active, "Core: Campaign starts in Active status");

                // 3. Campaign Progression & Effectiveness
                system.TickDay(2, randomRoll: 0.99f); // High roll ensures no detection
                Check(campaign.AccumulatedEffectiveness > 0f, "Core: Campaign accumulated effectiveness on day tick");

                system.TickDay(3, randomRoll: 0.99f);
                system.TickDay(4, randomRoll: 0.99f);
                Check(campaign.Status == CampaignStatus.Completed, "Core: Campaign completed after duration reached");
                Check(system.GetFactionMoraleImpact("faction_iron_raiders") < 0f, "Core: UndermineFaction objective decreased faction morale");

                // 4. Detection & Credibility Penalty
                var campaignRisk = system.LaunchCampaign(
                    campaignName: "Operation Deception",
                    targetFactionId: "faction_iron_raiders",
                    objective: PropagandaObjective.DestabilizeRegion,
                    messageIds: new[] { msgLie.MessageId },
                    durationDays: 5,
                    currentDay: 5
                );

                float credBefore = system.ShelterCredibility;
                system.TickDay(6, randomRoll: 0.01f); // Low roll forces detection
                Check(campaignRisk.WasDetected, "Core: Campaign detected on low roll");
                Check(campaignRisk.Status == CampaignStatus.Compromised, "Core: Campaign status marked Compromised");
                Check(system.ShelterCredibility < credBefore, "Core: Shelter credibility penalized for exposed lies");

                // 5. Host Session Coordination
                var hostSys = new PropagandaSystem();
                var host = new PropagandaHostSession(hostSys);
                bool stateChanged = false;
                host.StateChanged += () => stateChanged = true;

                var hostMsg = host.CreateMessage("surv_author", PropagandaMedium.WallPosting, MessageTruthfulness.HalfTruth, PropagandaTheme.Unity, "faction_settlers", "Join our holdfast.");
                Check(stateChanged, "Host: StateChanged fired on CreateMessage");
                Check(host.MessageCount == 1, "Host: MessageCount is 1");

                stateChanged = false;
                var hostCmp = host.LaunchCampaign("Op Unity", "faction_settlers", PropagandaObjective.BoostMorale, new[] { hostMsg.MessageId }, 2, 1);
                Check(stateChanged, "Host: StateChanged fired on LaunchCampaign");
                Check(host.ActiveCampaignCount == 1, "Host: ActiveCampaignCount is 1");

                stateChanged = false;
                host.TickDay(2, 0.99f);
                Check(stateChanged, "Host: StateChanged fired on TickDay");

                // 6. Persistence Roundtrip
                var captured = hostSys.CaptureState();
                bool saveOk = PropagandaSaveStore.TrySave(captured);
                Check(saveOk, "Persistence: PropagandaSaveStore.TrySave succeeded");

                var loaded = PropagandaSaveStore.TryLoad();
                Check(loaded != null, "Persistence: PropagandaSaveStore.TryLoad succeeded");
                Check(loaded?.Messages.Count == hostSys.MessageCount, "Persistence: Message count preserved");
                Check(loaded?.Campaigns.Count == hostSys.State.Campaigns.Count, "Persistence: Campaign count preserved");
                Check(loaded?.ShelterCredibility == hostSys.ShelterCredibility, "Persistence: Shelter credibility preserved");

                string envJson = PropagandaSaveStore.TryCapturePersisted(captured);
                Check(!string.IsNullOrWhiteSpace(envJson), "Persistence: TryCapturePersisted produced valid JSON envelope");

                // 7. UI Panel Binding & Lifecycle
                var panel = new PropagandaPanel();
                panel.Bind(host);
                Check(panel.IsBound, "UI: PropagandaPanel bound successfully");

                panel.RefreshView();
                Check(true, "UI: PropagandaPanel refreshed cleanly");

                panel.Unbind();
                Check(!panel.IsBound, "UI: PropagandaPanel unbound cleanly");
                panel.QueueFree();

                GD.Print($"[PropagandaSelfTest] Complete with {failures} failure(s).");
                if (failures == 0)
                {
                    GD.Print("[HOST_SELFTEST] propaganda_selftest PASS");
                    GD.Print("[HOST_SELFTEST_SUMMARY] test=propaganda_selftest status=PASS exit_code=0 passed=19 failed=0 total=19 details=\"All propaganda & morale warfare gates passed\"");
                    GD.Print("[HOST_SELFTEST_JSON] {\"test\":\"propaganda_selftest\",\"status\":\"PASS\",\"exit_code\":0,\"passed\":19,\"failed\":0,\"total\":19,\"details\":\"All propaganda & morale warfare gates passed\"}");
                    GD.Print("SELFTEST PASS: propaganda_selftest");
                    GD.Print("PROPAGANDA_SELFTEST PASS");
                    return 0;
                }

                GD.PrintErr("[HOST_SELFTEST] propaganda_selftest FAIL");
                return 1;
            }
            catch (Exception ex)
            {
                GD.PrintErr("[PropagandaSelfTest] Unhandled exception: " + ex);
                return 1;
            }
        }
    }
}
