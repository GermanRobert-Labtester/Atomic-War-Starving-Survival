// SPDX-License-Identifier: MIT
// ASHFALL Moral Choice Journey & Host-UI Action Path Verification (REM-004 / R07).
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Flags;
using Ashfall.Core.Journal;
using Ashfall.Core.MoralChoice;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Journeys
{
    public sealed class MoralChoiceJourneyTests
    {
        [Fact]
        public void MoralChoiceJourney_Encounter_Resolve_SaveReload_PreventsDuplicateResolution()
        {
            var flags = new InMemoryFlagLedger();
            var rng = new SeededRng(20260904);
            var system = new MoralChoiceSystem(rng, flags: flags);

            // Authored dilemma definition
            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_water_share",
                DisplayName = "The Thirsty Wanderer",
                Category = "share",
                Discovery = "A desperate wanderer collapses against the exterior decontamination hatch, begging for a single canteen of water.",
                LocationId = "loc_surface_airlock",
                MinDay = 1,
                MaxDay = 10,
                Choices = new List<MoralChoiceOption>
                {
                    new()
                    {
                        Label = "Share a clean water ration",
                        MoralDelta = 15,
                        EmpathyDelta = 10,
                        OutcomeText = "The wanderer drinks with trembling gratitude and promises to spread word of the shelter's humanity.",
                        Epitaph = "Gave water to the dying."
                    },
                    new()
                    {
                        Label = "Drive them away into the ash storm",
                        MoralDelta = -15,
                        EmpathyDelta = -5,
                        OutcomeText = "The wanderer curses the bunker door before vanishing into the particulate haze.",
                        Epitaph = "Hoarded water behind sealed iron."
                    }
                }
            };

            // Track events
            var resolvedEvents = new List<MoralChoiceResolution>();
            system.OnQuestResolved += r => resolvedEvents.Add(r);

            // Step 1: Initial state verification
            Assert.False(system.IsResolved(quest.Id));
            Assert.Equal(0, system.QuestsResolved);
            Assert.Equal(0, system.MoralScore);
            Assert.Equal(0, system.EmpathyPoints);
            Assert.Equal(MoralPathBand.Neutral, system.CurrentBand);

            // Step 2: Resolve Option 0 (Compassion / Share)
            var resolution = system.Resolve(quest, choiceIndex: 0, quest.LocationId, day: 1);

            Assert.NotNull(resolution);
            Assert.Equal(quest.Id, resolution.questId);
            Assert.Equal(0, resolution.choiceIndex);
            Assert.Single(resolvedEvents);
            Assert.True(system.IsResolved(quest.Id));
            Assert.Equal(1, system.QuestsResolved);
            Assert.Equal(15, system.MoralScore);
            Assert.Equal(10, system.EmpathyPoints);
            Assert.Equal(MoralPathBand.SlightlyPositive, system.CurrentBand);

            // Step 3: Save to state DTO and serialize
            var savedState = system.CaptureState();
            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(savedState);
            Assert.False(string.IsNullOrWhiteSpace(json));

            // Step 4: Restore in a fresh system instance
            var reloadedFlags = new InMemoryFlagLedger();
            var reloadedRng = new SeededRng(20260904);
            var reloadedSystem = new MoralChoiceSystem(reloadedRng, flags: reloadedFlags);
            var restoredState = serializer.Deserialize<MoralChoiceState>(json);
            Assert.NotNull(restoredState);
            reloadedSystem.RestoreState(restoredState);

            // Step 5: Verify restored state integrity
            Assert.True(reloadedSystem.IsResolved(quest.Id));
            Assert.Equal(1, reloadedSystem.QuestsResolved);
            Assert.Equal(15, reloadedSystem.MoralScore);
            Assert.Equal(10, reloadedSystem.EmpathyPoints);
            Assert.Equal(MoralPathBand.SlightlyPositive, reloadedSystem.CurrentBand);

            // Step 6: Verify lockout against duplicate selection / double application
            // Host pattern: check IsResolved before invoking Resolve
            bool canResolveAgain = !reloadedSystem.IsResolved(quest.Id);
            Assert.False(canResolveAgain, "Host must reject already resolved moral choices");

            // Even if Resolve is called directly on Core, it returns stored resolution and does not re-apply deltas
            var duplicateRes = reloadedSystem.Resolve(quest, choiceIndex: 1, quest.LocationId, day: 2);
            Assert.Equal(0, duplicateRes.choiceIndex); // Stored original choice preserved
            Assert.Equal(15, reloadedSystem.MoralScore); // Score NOT corrupted by second call
            Assert.Equal(1, reloadedSystem.QuestsResolved); // Count remains exactly 1
        }

        [Fact]
        public void Journey_J1_MoralChoice_DecisionSpine_JournalConsequence_SaveReload_Idempotent()
        {
            // Journey J1 specification from Flagship Remediation Plan (Section 7):
            // 1. Load one unresolved authored choice.
            // 2. Open decision UI / surface options.
            // 3. Resolve an option.
            // 4. Verify moral state changes.
            // 5. Verify journal consequence.
            // 6. Save.
            // 7. Reload.
            // 8. Verify it cannot resolve twice.

            var flags = new InMemoryFlagLedger();
            var rng = new SeededRng(20260905);
            var system = new MoralChoiceSystem(rng, flags: flags);
            var journal = new JournalSystem();

            int journalEntriesCount = 0;
            string? lastJournalEntryText = null;

            system.OnQuestResolved += r =>
            {
                string arrow = r.impactMark == "up" ? "🔺" : r.impactMark == "down" ? "🔻" : "⚪";
                journal.TryAddRawEntry(r.questId, $"{arrow} {r.epitaph}", null!, r.resolvedDay);
                journalEntriesCount++;
                lastJournalEntryText = $"{arrow} {r.epitaph}";
            };

            // 1. Authored choice: The Caloric Deficit
            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_share_child",
                DisplayName = "The Caloric Deficit",
                Category = "share",
                Trigger = "An unaccompanied minor requests rations at a ruin crossing.",
                Discovery = "A child sits against the concrete. Their caloric math is visibly failing.",
                LocationId = "loc_sector_ruins",
                MinDay = 0,
                MaxDay = 0,
                Choices = new List<MoralChoiceOption>
                {
                    new()
                    {
                        Label = "Give all your food",
                        MoralDelta = 10,
                        EmpathyDelta = 1,
                        OutcomeText = "You hand over the rations. The child consumes half and pockets the rest.",
                        Epitaph = "Transferred all rations to the minor. Received charcoal schematic in return."
                    },
                    new()
                    {
                        Label = "Refuse and walk on",
                        MoralDelta = -5,
                        EmpathyDelta = 0,
                        OutcomeText = "You shake your head. The child goes back to conserving energy.",
                        Epitaph = "Refused the minor's requisition. Conserved my own supply."
                    }
                }
            };

            // 2. Initial state verification before player decision
            Assert.False(system.IsResolved(quest.Id));
            Assert.Equal(0, system.QuestsResolved);
            Assert.Equal(0, system.MoralScore);
            Assert.Equal(0, system.EmpathyPoints);
            Assert.Equal(0, journalEntriesCount);

            // 3. Player resolves Option 0 ("Give all your food")
            var resolution = system.Resolve(quest, choiceIndex: 0, quest.LocationId, day: 3);

            // 4. Verify moral state mutated
            Assert.NotNull(resolution);
            Assert.True(system.IsResolved(quest.Id));
            Assert.Equal(1, system.QuestsResolved);
            Assert.Equal(10, system.MoralScore);
            Assert.Equal(1, system.EmpathyPoints);
            Assert.Equal("up", resolution.impactMark);

            // 5. Verify journal consequence was recorded
            Assert.Equal(1, journalEntriesCount);
            Assert.NotNull(lastJournalEntryText);
            Assert.StartsWith("🔺", lastJournalEntryText!);
            Assert.Contains("Transferred all rations to the minor", lastJournalEntryText!);

            // 6. Save state to DTO
            var state = system.CaptureState();
            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(state);

            // 7. Reload in clean host session
            var reloadedSystem = new MoralChoiceSystem(new SeededRng(20260905));
            var restoredState = serializer.Deserialize<MoralChoiceState>(json);
            Assert.NotNull(restoredState);
            reloadedSystem.RestoreState(restoredState!);

            // 8. Verify persistence and impossibility of double resolution
            Assert.True(reloadedSystem.IsResolved(quest.Id));
            Assert.Equal(1, reloadedSystem.QuestsResolved);
            Assert.Equal(10, reloadedSystem.MoralScore);
            Assert.Equal(1, reloadedSystem.EmpathyPoints);

            // Attempting to resolve again returns stored resolution without mutating scores or firing duplicate events
            var secondRes = reloadedSystem.Resolve(quest, choiceIndex: 1, quest.LocationId, day: 4);
            Assert.Equal(0, secondRes.choiceIndex); // preserved original choice
            Assert.Equal(10, reloadedSystem.MoralScore); // untouched
            Assert.Equal(1, reloadedSystem.QuestsResolved); // untouched
        }

        [Fact]
        public void MoralChoice_SaveEnvelope_RoundTripsResolvedLedger()
        {
            // Audit #30 — checksum envelope over MoralChoiceState (host SaveStore shape).
            var system = new MoralChoiceSystem(new SeededRng(42));
            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_envelope_pin",
                DisplayName = "Envelope Pin",
                Category = "share",
                Discovery = "Fixture dilemma for save-envelope pin.",
                LocationId = "loc_surface_airlock",
                MinDay = 1,
                MaxDay = 10,
                Choices = new List<MoralChoiceOption>
                {
                    new() { Label = "Share", MoralDelta = 5, EmpathyDelta = 1, OutcomeText = "ok", Epitaph = "shared" },
                    new() { Label = "Refuse", MoralDelta = -5, EmpathyDelta = -1, OutcomeText = "no", Epitaph = "refused" }
                }
            };
            Assert.NotNull(system.Resolve(quest, 0, quest.LocationId, 2));

            string envelope = SaveEnvelopeHelper.CaptureEnvelope(system.CaptureState());
            var (ok, restored, error) = SaveEnvelopeHelper.RestoreEnvelope<MoralChoiceState>(
                envelope, allowBareFallback: false);
            Assert.True(ok, error);
            Assert.NotNull(restored);

            var reloaded = new MoralChoiceSystem(new SeededRng(99));
            reloaded.RestoreState(restored!);
            Assert.True(reloaded.IsResolved(quest.Id));
            Assert.Equal(5, reloaded.MoralScore);

            var again = reloaded.Resolve(quest, 1, quest.LocationId, 3);
            Assert.Equal(0, again.choiceIndex);
            Assert.Equal(5, reloaded.MoralScore);
        }

        [Fact]
        public void HostWiring_SaveAllAndProcessFlush_EnrollMoralChoice()
        {
            string? srcRoot = FindSrcRoot();
            Assert.NotNull(srcRoot);

            string orch = File.ReadAllText(Path.Combine(srcRoot!, "Main.SaveOrchestrator.cs"));
            string app = File.ReadAllText(Path.Combine(srcRoot!, "Main.Application.cs"));
            Assert.Contains("SaveMoralChoice()", orch);
            Assert.Contains("FlushMoralChoiceIfDirty()", app);
        }

        private static string? FindSrcRoot()
        {
            string current = Directory.GetCurrentDirectory();
            while (!string.IsNullOrEmpty(current))
            {
                string candidate = Path.Combine(current, "src");
                if (Directory.Exists(candidate)) return candidate;
                string parent = Path.GetDirectoryName(current)!;
                if (parent == current) break;
                current = parent;
            }
            return null;
        }
    }
}
