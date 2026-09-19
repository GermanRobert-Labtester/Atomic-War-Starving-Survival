// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Headless selftest for Plan 182 (Relationship Decay & Social Drift System).
    /// </summary>
    internal static class RelationshipDecaySelfTest
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
                GD.Print("[RelationshipDecaySelfTest] Starting Plan 182 verification...");

                // 1. Core System & HostSession
                var system = new RelationshipDecaySystem();
                var session = new RelationshipDecayHostSession(system);
                Check(session != null, "HostSession: Created successfully");
                Check(session!.TrackedPairCount == 0, "Core: Initial TrackedPairCount is 0");

                // 2. Register Pair Bond
                var pair = session.RegisterOrUpdatePair(
                    survivorA: "surv_sarah",
                    survivorB: "surv_tom",
                    bond: SurvivorBondType.Friend,
                    initialAffinity: 60f,
                    initialTrust: 55f,
                    currentDay: 1
                );

                Check(pair != null, "HostSession: Pair registered");
                Check(session.TrackedPairCount == 1, "HostSession: TrackedPairCount is 1");
                Check(session.GetPair("surv_sarah", "surv_tom") != null, "HostSession: Pair retrieved by survivor IDs");

                // 3. Record Positive Interaction (Strengthens bond)
                bool interacted = session.RecordInteraction("surv_sarah", "surv_tom", "shared_meal", 20f, 1);
                Check(interacted, "HostSession: Interaction recorded");
                Check(pair!.Affinity == 80f, "Core: Affinity increased to 80");
                Check(pair.Bond == SurvivorBondType.CloseFriend, "Core: Bond upgraded to CloseFriend (affinity >= 75, trust >= 60)");

                // 4. Tick Day without interaction -> Bond Decay & Drift
                // Tick 10 days to trigger drift
                for (int day = 2; day <= 10; day++)
                {
                    session.TickDay(day);
                }

                Check(pair.DaysWithoutInteraction == 9, "Core: DaysWithoutInteraction tracked accurately");
                Check(pair.Affinity < 80f, "Core: Affinity decayed over time");

                // 5. Persistence Roundtrip
                var state = session.CaptureState();
                Check(state != null && state.Pairs.Count == 1, "SaveStore: State captured");

                var newSession = new RelationshipDecayHostSession(new RelationshipDecaySystem());
                newSession.RestoreState(state!);
                Check(newSession.TrackedPairCount == 1, "SaveStore: Restored TrackedPairCount matches");
                Check(newSession.Pairs[0].SurvivorA == pair.SurvivorA, "SaveStore: Restored survivor A matches");
                Check(newSession.Pairs[0].SurvivorB == pair.SurvivorB, "SaveStore: Restored survivor B matches");

                // 6. UI Construction
                var panel = new UI.RelationshipDecayPanel();
                panel.Bind(session);
                panel.RefreshView();
                Check(panel != null, "UI: RelationshipDecayPanel instantiated and bound cleanly");

                GD.Print($"[RelationshipDecaySelfTest] Complete with {failures} failure(s).");
                return failures == 0 ? 0 : 1;
            }
            catch (Exception ex)
            {
                GD.PrintErr("[RelationshipDecaySelfTest] Unhandled exception: " + ex);
                return 1;
            }
        }
    }
}
