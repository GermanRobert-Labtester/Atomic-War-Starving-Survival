// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 42 / C2[18] — survivor voice host-integration gate.
//
// Pins the production wiring contract added by the host integration:
//   * the `survivor_voice` save section is registered with its projection file,
//   * the authored catalog is the only line source (8 authored lines, 5 triggers),
//   * the host composes selection (SurvivorVoiceSystem) with playback
//     arbitration (VoiceLineDispatchCoordinator) — one selection authority,
//   * every authored trigger with a reachable producer is wired to that owner,
//   * the CLI probe is registered.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Voice;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public sealed class Plan42SurvivorVoiceHostIntegrationTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (File.Exists(Path.Combine(dir, "Ashfall.csproj")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        private static string ReadRepoFile(params string[] parts)
            => File.ReadAllText(Path.Combine(new[] { RepoRoot() }.Concat(parts).ToArray()));

        private static SurvivorVoiceSystem LoadAuthoredCatalog()
        {
            string path = Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data", "survivor_voice_lines.json");
            var system = new SurvivorVoiceSystem();
            system.LoadCatalog(File.ReadAllText(path));
            return system;
        }

        [Fact]
        public void SurvivorVoiceSection_IsRegisteredWithProjectionFile()
        {
            Assert.True(SaveSectionRegistry.TryGetSection("survivor_voice", out var section));
            Assert.NotNull(section);
            Assert.Equal("SaveSurvivorVoice", section!.SaveMethod);
            Assert.Equal("SetupSurvivorVoice", section.SetupMethod);
            Assert.Equal("survivor_voice_save.json", SaveSectionRegistry.FileNameFor("survivor_voice"));
        }

        [Fact]
        public void AuthoredCatalog_IsTheOnlyLineSource()
        {
            var system = LoadAuthoredCatalog();
            Assert.Equal(8, system.Catalog.Count);
            Assert.Contains(system.Catalog, l => l.Trigger == "survivor_perished");
            Assert.Contains(system.Catalog, l => l.Trigger == "ration_cut");
            Assert.Contains(system.Catalog, l => l.Trigger == "radiation_spike");
            Assert.Contains(system.Catalog, l => l.Trigger == "season_changed");
            Assert.Contains(system.Catalog, l => l.Trigger == "visitor_arrived");
            // Every line carries authored English text and a stable text key.
            Assert.All(system.Catalog, l =>
                Assert.False(string.IsNullOrWhiteSpace(l.TextEnglish) && string.IsNullOrWhiteSpace(l.TextKey)));
        }

        [Fact]
        public void HostSession_UsesOneSelectionAuthorityAndOneBus()
        {
            string session = ReadRepoFile("src", "Host", "SurvivorVoiceHostSession.cs");
            // Catalog authority selects...
            Assert.Contains("Voice.TrySelectVoiceLine(", session);
            // ...the bus only arbitrates the selected line (no second selection).
            Assert.Contains("Dispatch.TryDispatch(selection, priority, TickForDay(day), durationTicks);", session);
            Assert.DoesNotContain("VoiceLineSelectionEngine.SelectLine(", session);
        }

        [Fact]
        public void ReachableProducers_AreWiredToTheirCanonicalOwners()
        {
            string voice = ReadRepoFile("src", "Main.SurvivorVoice.cs");
            Assert.Contains("_campaignDay.Calendar.OnSeasonChanged += TriggerSurvivorVoiceSeasonChanged;", voice);
            Assert.Contains("_survivors.Radiation.OnDoseChanged +=", voice);

            string fate = ReadRepoFile("src", "Main.SurvivorFate.cs");
            Assert.Contains("TriggerSurvivorVoicePerished(fate.survivorId);", fate);

            string economyMain = ReadRepoFile("src", "Main.Economy.cs");
            Assert.Contains("TriggerSurvivorVoiceRationCut(target);", economyMain);
        }

        [Fact]
        public void VisitorTrigger_IsHonestlyUnbound()
        {
            // visitor_arrived has no reachable host producer: the visitor authority
            // is itself an orphan. The separator must stay unbound rather than
            // invent a visitor.
            string voice = ReadRepoFile("src", "Main.SurvivorVoice.cs");
            Assert.Contains("visitor_arrived", voice);
            Assert.DoesNotContain("TriggerSurvivorVoiceVisitorArrived", voice);
        }

        [Fact]
        public void SelectionAndDispatch_ComposeAsDocumented()
        {
            // The documented composition at Core level: the catalog authority
            // selects, the bus arbitrates. The host performs no second selection.
            var system = LoadAuthoredCatalog();
            var dispatch = new VoiceLineDispatchCoordinator();

            var context = new SurvivorSpeechContext
            {
                SurvivorId = "srv_medic",
                Profession = "medic",
                Morale = 70f,
                Fatigue = 20f,
                PreferredRegister = string.Empty
            };

            Assert.True(system.TrySelectVoiceLine(context, "radiation_spike", 5, null, out var payload));
            Assert.Equal("radiation_spike", payload.Trigger);
            Assert.Equal("srv_medic", payload.SpeakerSurvivorId);
            Assert.Single(system.History);

            var selection = new VoiceLineSelectionResult(
                true, payload.LineId, payload.Register, payload.TextKey, string.Empty, payload.SpeakerSurvivorId);
            var dispatched = dispatch.TryDispatch(selection, VoiceLinePriority.NeedsWarning, 500, 5);
            Assert.True(dispatched.Dispatched);
            Assert.Equal("srv_medic", dispatch.ActivePlayback!.SpeakerSurvivorId);

            // Per-survivor day cooldown blocks a second line that day...
            Assert.False(system.TrySelectVoiceLine(context, "radiation_spike", 5, null, out _));

            // ...and capture/restore preserves history and cooldowns.
            var captured = system.CaptureState();
            var restored = LoadAuthoredCatalog();
            restored.RestoreState(captured);
            Assert.Single(restored.History);
            Assert.False(restored.TrySelectVoiceLine(context, "radiation_spike", 5, null, out _));
        }

        [Fact]
        public void RationCutReadModel_OnlyFiresOnAReduction()
        {
            string session = ReadRepoFile("src", "Host", "SurvivorVoiceHostSession.cs");
            Assert.Contains("known && tier < prior", session);
            Assert.Contains("public bool IsRationCut(string resourceId, RationingTier tier)", session);
        }

        [Fact]
        public void HostCliProbe_IsRegistered()
        {
            string actions = ReadRepoFile("src", "Host", "HostCli.cs");
            Assert.Contains("SurvivorVoiceSelfTest", actions);
            Assert.Contains("--survivor-voice-selftest", actions);
            string probe = ReadRepoFile("src", "Host", "HostCli.SurvivorVoice.cs");
            Assert.Contains("public static int RunSurvivorVoiceSelfTest(", probe);
        }
    }
}
