// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Communication;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Headless selftest for Plan 212 (Time Capsule & Legacy Messages System).
    /// </summary>
    internal static class TimeCapsuleSelfTest
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
                GD.Print("[TimeCapsuleSelfTest] Starting Plan 212 verification...");

                // 1. Core System & HostSession
                var system = new TimeCapsuleSystem();
                var session = new TimeCapsuleHostSession(system);
                Check(session != null, "HostSession: Created successfully");
                Check(session!.TotalCapsuleCount == 0, "Core: Initial TotalCapsuleCount is 0");

                // 2. Create Capsule
                var contents = new List<CapsuleContent>
                {
                    new CapsuleContent
                    {
                        ContentId = "item_watch",
                        ContentType = CapsuleContentType.Artifact,
                        ItemId = "prewar_pocketwatch",
                        Text = "Passed down from grandfather.",
                        AuthorId = "surv_elena",
                        SentimentalValue = 85f
                    },
                    new CapsuleContent
                    {
                        ContentId = "letter_1",
                        ContentType = CapsuleContentType.Letter,
                        Text = "If you are reading this, our shelter survived.",
                        AuthorId = "surv_elena"
                    }
                };

                var capsule = session.CreateCapsule(
                    capsuleName: "Founders Vault",
                    creatorId: "surv_elena",
                    conditionType: OpenConditionType.DateBased,
                    createdDay: 1,
                    targetOpenDay: 10,
                    location: "Sublevel 3 Maintenance Shaft",
                    message: "Open only when the dust has cleared.",
                    contents: contents
                );

                Check(capsule != null, "HostSession: Capsule created");
                Check(session.TotalCapsuleCount == 1, "HostSession: TotalCapsuleCount is 1");
                Check(session.UnopenedCapsuleCount == 1, "HostSession: UnopenedCapsuleCount is 1");
                Check(!capsule!.IsOpen, "Core: Capsule is unopened");

                // 3. Create Legacy Message
                var msg = session.WriteMessage(
                    authorId: "surv_elena",
                    recipientId: "surv_marcus",
                    content: "Keep the generator running, no matter what.",
                    condition: DeliveryCondition.OnDate,
                    deliveryDay: 5,
                    currentDay: 1
                );

                Check(msg != null, "HostSession: Message written");
                Check(session.PendingMessageCount == 1, "HostSession: PendingMessageCount is 1");
                Check(!msg!.IsDelivered, "Core: Message is not yet delivered");

                // 4. Tick Day progression
                session.TickDay(5);
                Check(msg!.IsDelivered, "Core: Message delivered on day 5");
                Check(session.PendingMessageCount == 0, "HostSession: PendingMessageCount is 0");
                Check(!capsule!.IsOpen, "Core: Capsule not yet open on day 5 (target is day 10)");

                session.TickDay(10);
                Check(capsule!.IsOpen, "Core: Date-based capsule opened automatically on day 10");
                Check(session.UnopenedCapsuleCount == 0, "HostSession: UnopenedCapsuleCount is 0");

                // 5. Persistence Roundtrip
                var state = session.CaptureState();
                Check(state != null && state.Capsules.Count == 1 && state.Messages.Count == 1, "SaveStore: State captured");

                var newSession = new TimeCapsuleHostSession(new TimeCapsuleSystem());
                newSession.RestoreState(state!);
                Check(newSession.TotalCapsuleCount == 1, "SaveStore: Restored TotalCapsuleCount matches");
                Check(newSession.Capsules[0].CapsuleName == "Founders Vault", "SaveStore: Restored capsule name matches");
                Check(newSession.Capsules[0].IsOpen, "SaveStore: Restored capsule open status matches");
                Check(newSession.Messages[0].IsDelivered, "SaveStore: Restored message delivery status matches");

                // 6. UI Construction
                var panel = new UI.TimeCapsulePanel();
                panel.Bind(session);
                panel.RefreshView();
                Check(panel != null, "UI: TimeCapsulePanel instantiated and bound cleanly");

                GD.Print($"[TimeCapsuleSelfTest] Complete with {failures} failure(s).");
                return failures == 0 ? 0 : 1;
            }
            catch (Exception ex)
            {
                GD.PrintErr("[TimeCapsuleSelfTest] Unhandled exception: " + ex);
                return 1;
            }
        }
    }
}
