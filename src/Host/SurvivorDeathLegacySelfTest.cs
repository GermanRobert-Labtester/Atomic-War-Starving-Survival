// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Headless selftest for Plan 206 (Survivor Death Records, Wills & Estates System).
    /// </summary>
    internal static class SurvivorDeathLegacySelfTest
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
                GD.Print("[SurvivorDeathLegacySelfTest] Starting Plan 206 verification...");

                // 1. Core System & HostSession
                var system = new SurvivorDeathLegacySystem();
                var session = new SurvivorDeathLegacyHostSession(system);
                Check(session != null, "HostSession: Created successfully");
                Check(session!.DeathCount == 0, "Core: Initial DeathCount is 0");
                Check(session.WillCount == 0, "Core: Initial WillCount is 0");

                // 2. Draft Last Will
                var beneficiaries = new List<BeneficiaryEntry>
                {
                    new BeneficiaryEntry { BeneficiaryId = "surv_lucas", Category = InheritanceCategory.All, Percentage = 100f }
                };
                var bequests = new List<SpecialBequest>
                {
                    new SpecialBequest { ItemId = "rifle_hunting", RecipientId = "surv_lucas" }
                };

                var will = session.CreateWill(
                    survivorId: "surv_old_hunter",
                    beneficiaries: beneficiaries,
                    specialBequests: bequests,
                    residuaryBeneficiary: "commons",
                    witnesses: new[] { "surv_elder", "surv_doc" },
                    currentDay: 1
                );

                Check(will != null, "HostSession: Will created");
                Check(session.WillCount == 1, "HostSession: WillCount is 1");
                Check(session.GetActiveWill("surv_old_hunter") != null, "HostSession: Active will retrieved for survivor");

                // 3. Record Death
                var death = session.RecordDeath(
                    survivorId: "surv_old_hunter",
                    survivorName: "Old Hunter Joe",
                    cause: DeathCause.OldAge,
                    deathDay: 12,
                    location: "Quarters B4",
                    lastWords: "Keep the watch.",
                    witnesses: new[] { "surv_elder" },
                    circumstances: "Passed peacefully during the night."
                );

                Check(death != null, "HostSession: Death record created");
                Check(session.DeathCount == 1, "HostSession: DeathCount is 1");
                Check(session.GetDeathRecord("surv_old_hunter") != null, "HostSession: Death record retrieved");

                // 4. Distribute Inheritance
                var items = session.DistributeInheritance(death!.RecordId, new[] { "rifle_hunting", "ammo_box_308" });
                Check(items.Count == 2, "HostSession: 2 items distributed");
                Check(items[0].RecipientId == "surv_lucas", "Core: Special bequest honored (rifle -> surv_lucas)");
                Check(items[1].RecipientId == "surv_lucas", "Core: Primary beneficiary received general item");

                // 5. Raise and Resolve Dispute
                var dispute = session.RaiseDispute(will!.WillId, "surv_rival", "Claims the rifle was promised to them in trade.");
                Check(dispute != null, "HostSession: Dispute raised");
                Check(session.ActiveDisputeCount == 1, "HostSession: ActiveDisputeCount is 1");

                bool resolved = session.ResolveDispute(dispute!.DisputeId, DisputeResolution.Upheld);
                Check(resolved, "HostSession: Dispute resolved as Upheld");
                Check(session.ActiveDisputeCount == 0, "HostSession: ActiveDisputeCount is 0");

                // 6. Persistence Roundtrip
                var state = session.CaptureState();
                Check(state != null && state.DeathRecords.Count == 1 && state.Wills.Count == 1, "SaveStore: State captured");

                var newSession = new SurvivorDeathLegacyHostSession(new SurvivorDeathLegacySystem());
                newSession.RestoreState(state!);
                Check(newSession.DeathCount == 1, "SaveStore: Restored DeathCount matches");
                Check(newSession.WillCount == 1, "SaveStore: Restored WillCount matches");
                Check(newSession.DeathRecords[0].SurvivorName == "Old Hunter Joe", "SaveStore: Restored survivor name matches");
                Check(newSession.InheritedItems.Count == 2, "SaveStore: Restored inherited items count matches");

                // 7. UI Construction
                var panel = new UI.SurvivorDeathLegacyPanel();
                panel.Bind(session);
                panel.RefreshView();
                Check(panel != null, "UI: SurvivorDeathLegacyPanel instantiated and bound cleanly");

                GD.Print($"[SurvivorDeathLegacySelfTest] Complete with {failures} failure(s).");
                return failures == 0 ? 0 : 1;
            }
            catch (Exception ex)
            {
                GD.PrintErr("[SurvivorDeathLegacySelfTest] Unhandled exception: " + ex);
                return 1;
            }
        }
    }
}
