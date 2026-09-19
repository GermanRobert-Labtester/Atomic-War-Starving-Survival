// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Reputation;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Headless selftest for Plan 207 (Shelter Reputation & External Perception System).
    /// </summary>
    internal static class ShelterReputationSelfTest
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
                GD.Print("[ShelterReputationSelfTest] Starting Plan 207 verification...");

                // 1. Core Reputation Recording & Reach
                var system = new ShelterReputationSystem();
                bool rec1 = system.RecordEvidence("ev_trade_01", ReputationDimension.Wealth, 20f, InformationMedium.Witness, 1, "Fair market trade") != null;
                Check(rec1, "Core: First evidence recorded successfully");
                Check(system.EvidenceCount == 1, "Core: Evidence count is 1");
                Check(system.GetScore(ReputationDimension.Wealth) == 20f, "Core: Wealth score reflects witness multiplier 1.0");

                // 2. Anti-Farming Protection
                bool recDup = system.RecordEvidence("ev_trade_01", ReputationDimension.Wealth, 20f, InformationMedium.Witness, 2, "Duplicate trade") != null;
                Check(!recDup, "Core: Anti-farming rejected immediate duplicate eventId");
                Check(system.EvidenceCount == 1, "Core: Evidence count unchanged on duplicate reject");

                // Radio Broadcast with 2.0x reach
                bool recRadio = system.RecordEvidence("ev_radio_01", ReputationDimension.Reliability, 15f, InformationMedium.RadioBroadcast, 2, "Weather beacon") != null;
                Check(recRadio, "Core: Radio broadcast evidence recorded");
                Check(system.GetScore(ReputationDimension.Reliability) == 15f, "Core: Reliability score updated with delta 15");

                // 3. Notoriety Accumulation & Capping
                float prevNotoriety = system.Notoriety;
                Check(prevNotoriety > 0f, "Core: Notoriety increased from recorded evidence");
                for (int i = 0; i < 25; i++)
                {
                    system.RecordEvidence($"ev_war_{i}", ReputationDimension.Strength, 50f, InformationMedium.Propaganda, 2);
                }
                Check(system.Notoriety == 100f, "Core: Notoriety capped at 100%");

                // 4. Tag Evaluation
                Check(system.HasTag(ReputationTag.Fortress), "Core: High strength triggered Fortress tag");

                system.RecordEvidence("ev_charity_01", ReputationDimension.Generosity, 45f, InformationMedium.Witness, 3);
                Check(system.HasTag(ReputationTag.Sanctuary), "Core: High generosity triggered Sanctuary tag");

                system.RecordEvidence("ev_ruthless_01", ReputationDimension.Ruthlessness, 45f, InformationMedium.Witness, 3);
                Check(system.HasTag(ReputationTag.RaiderBane), "Core: High strength + ruthlessness triggered RaiderBane tag");

                system.RecordEvidence("ev_betray_01", ReputationDimension.Reliability, -80f, InformationMedium.Witness, 3);
                Check(system.HasTag(ReputationTag.Treacherous), "Core: Negative reliability triggered Treacherous tag");

                // 5. Daily Decay
                float preDecayWealth = system.GetScore(ReputationDimension.Wealth);
                for (int d = 4; d <= 13; d++)
                {
                    system.TickDay(d);
                }
                Check(system.GetScore(ReputationDimension.Wealth) < preDecayWealth, "Core: Wealth decayed toward zero after 10 days");

                // 6. Host Session Orchestration
                var hostSys = new ShelterReputationSystem();
                var host = new ShelterReputationHostSession(hostSys);
                bool stateChangedFired = false;
                host.StateChanged += () => stateChangedFired = true;

                host.RecordEvidence("ev_host_01", ReputationDimension.Wealth, 25f, InformationMedium.TraderWord, 1);
                Check(stateChangedFired, "Host: StateChanged fired on RecordEvidence");
                Check(host.GetScore(ReputationDimension.Wealth) == 25f, "Host: Wealth score updated to 25");

                stateChangedFired = false;
                host.TickDay(2);
                Check(stateChangedFired, "Host: StateChanged fired on TickDay");

                // 7. Persistence Roundtrip via ShelterReputationSaveStore
                var saveState = hostSys.CaptureState();
                bool saveOk = ShelterReputationSaveStore.TrySave(saveState);
                Check(saveOk, "Persistence: ShelterReputationSaveStore.TrySave succeeded");

                var loadedState = ShelterReputationSaveStore.TryLoad();
                Check(loadedState != null, "Persistence: ShelterReputationSaveStore.TryLoad succeeded");
                if (loadedState != null)
                {
                    Check(loadedState.EvidenceHistory.Count == saveState.EvidenceHistory.Count, "Persistence: Evidence history count preserved");
                    Check(loadedState.Notoriety == saveState.Notoriety, "Persistence: Notoriety value preserved");
                    Check(loadedState.ActiveTags.Count == saveState.ActiveTags.Count, "Persistence: Active tags count preserved");
                }

                string persisted = ShelterReputationSaveStore.TryCapturePersisted(saveState);
                Check(!string.IsNullOrEmpty(persisted), "Persistence: ShelterReputationSaveStore.TryCapturePersisted produced valid envelope payload");

                // 8. UI Control Lifecycle
                var panel = new ShelterReputationPanel();
                panel.Bind(host);
                Check(panel.IsBound, "UI: ShelterReputationPanel bound successfully");
                panel.RefreshView();
                Check(true, "UI: ShelterReputationPanel refreshed cleanly");
                panel.Unbind();
                Check(!panel.IsBound, "UI: ShelterReputationPanel unbound cleanly");
                panel.QueueFree();

                GD.Print($"[ShelterReputationSelfTest] Complete with {failures} failure(s).");
                return HostCli.EmitSummary("shelter_reputation_selftest", failures == 0, failures == 0 ? 0 : 1, passedCount: 26 - failures, failedCount: failures,
                    details: failures == 0 ? "All shelter reputation & external perception gates passed" : $"{failures} failures detected");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[ShelterReputationSelfTest] Unexpected exception: {ex}");
                return HostCli.EmitSummary("shelter_reputation_selftest", false, 1, 0, 26, ex.Message);
            }
        }
    }
}
