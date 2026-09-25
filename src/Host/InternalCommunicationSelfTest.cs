// SPDX-License-Identifier: MIT
// ============================================================================
// Headless integration probe : InternalCommunicationSelfTest
// Plan 211 — catalog, identity refusal, public/private projection, expiry,
// save/restore, and separation from external radio communications.
// ============================================================================

using System;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core.Communication;

namespace AtomicWar.GodotApp
{
    internal static class InternalCommunicationSelfTest
    {
        public static int Run(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string message)
            {
                if (condition)
                    GD.Print("[PASS] " + message);
                else
                {
                    GD.PrintErr("[FAIL] " + message);
                    failures++;
                }
            }

            try
            {
                GD.Print("[InternalCommunicationSelfTest] Starting Plan 211 verification...");

                var system = new InternalCommunicationSystem();
                var session = new InternalCommunicationHostSession(
                    system,
                    survivorExists: id => id is "survivor_leader" or "survivor_reader",
                    canAuthor: id => id == "survivor_leader");

                Check(session.TryLoadCatalogFile(dataDirectory, out string catalogReason), "Authored communication catalog validates and loads (" + catalogReason + ")");
                Check(session.CatalogReady && session.System.Templates.Count == 7, "Seven authored communication templates are bound");

                var unknownAuthor = session.PostWaterAdvisory("survivor_unknown", 10);
                Check(!unknownAuthor.Accepted && unknownAuthor.ReasonKey == "author_unknown", "Unknown notice authors fail with a precise identity refusal");

                var refused = session.PostWaterAdvisory("survivor_reader", 10);
                Check(!refused.Accepted && refused.ReasonKey == "author_not_authorized", "Ordinary survivor cannot author a shelter notice");

                var posted = session.PostWaterAdvisory("survivor_leader", 10, "Canonical water advisory test fact.");
                Check(posted.Accepted && !string.IsNullOrWhiteSpace(posted.MessageId), "Leadership-authored water advisory posts through the host");
                Check(session.PublicNotices.Count == 1 && session.PublicNotices[0].MessageId == posted.MessageId, "Public read model exposes the committed notice");

                var mail = session.PostPersonalMail("survivor_leader", "survivor_reader", "Clinic note", "Bring the spare filters.", 10);
                Check(mail.Accepted, "Private mail posts through the same Core authority");
                Check(session.PublicNotices.All(message => message.RecipientId == string.Empty), "Private mail never enters the public notice projection");
                Check(session.GetInbox("survivor_reader").Any(message => message.MessageId == mail.MessageId), "Recipient inbox is resolved by canonical survivor id");

                var unknownBroadcast = session.BroadcastIntercom("survivor_unknown", "Unknown speaker test.", MessagePriority.Normal, 10);
                Check(!unknownBroadcast.Accepted && unknownBroadcast.ReasonKey == "author_unknown", "Unknown intercom authors fail with a precise identity refusal");

                var publicBoard = session.CreateBulletinBoard("survivor_reader", "Workshop Board", "room_workshop", 12, leadershipOnly: false);
                Check(publicBoard.Accepted, "A known survivor can create an ordinary bulletin board");
                var restrictedBoard = session.CreateBulletinBoard("survivor_reader", "Leadership Board", "room_office", 4, leadershipOnly: true);
                Check(!restrictedBoard.Accepted && restrictedBoard.ReasonKey == "author_not_authorized", "Ordinary survivors cannot create leadership-only boards");
                var leaderBoard = session.CreateBulletinBoard("survivor_leader", "Leadership Board", "room_office", 4, leadershipOnly: true);
                Check(leaderBoard.Accepted, "Leadership can create a leadership-only board");

                var wrongReader = session.MarkRead(mail.MessageId, "survivor_leader");
                Check(!wrongReader.Accepted && wrongReader.ReasonKey == "reader_not_recipient", "Non-recipient cannot read private mail");
                var read = session.MarkRead(mail.MessageId, "survivor_reader");
                var acknowledged = session.Acknowledge(mail.MessageId, "survivor_reader");
                Check(read.Accepted && acknowledged.Accepted, "Recipient read and acknowledgement markers commit");

                var state = session.CaptureState();
                string saveRoot = Path.Combine(Path.GetTempPath(), "ashfall-plan211-" + Guid.NewGuid().ToString("N"));
                SaveSlotRoot.ConfigureUserDataDirectory(saveRoot);
                bool persisted = InternalCommunicationSaveStore.TrySave(state);
                var loaded = InternalCommunicationSaveStore.TryLoad();
                Check(persisted && loaded != null && loaded.Messages.Count == state.Messages.Count, "Checksummed internal communication store round-trips through an isolated save root");
                SaveSlotRoot.ConfigureUserDataDirectory(null);
                try { if (Directory.Exists(saveRoot)) Directory.Delete(saveRoot, recursive: true); } catch { /* best-effort probe cleanup */ }

                var restored = new InternalCommunicationHostSession(new InternalCommunicationSystem());
                restored.SetIdentityProviders(
                    id => id is "survivor_leader" or "survivor_reader",
                    id => id == "survivor_leader");
                restored.RestoreState(state);
                Check(restored.PublicNotices.Count == 1 && restored.GetInbox("survivor_reader").Count == 1, "Capture/restore preserves public and private message state");

                restored.TickDay(15);
                Check(restored.PublicNotices.Count == 0, "Canonical day expiry removes the notice at its authored expiry day");
                Check(InternalCommunicationSaveStore.SectionName != "communications" &&
                      InternalCommunicationSaveStore.FileName != "communications_save.json", "Internal communication persistence is distinct from external antenna communications");

                GD.Print($"[InternalCommunicationSelfTest] Complete with {failures} failure(s).");
                return failures == 0 ? 0 : 1;
            }
            catch (Exception ex)
            {
                GD.PrintErr("[InternalCommunicationSelfTest] Unhandled exception: " + ex);
                return 1;
            }
        }
    }
}
