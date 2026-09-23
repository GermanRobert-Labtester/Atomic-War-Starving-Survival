// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 177: Dream & Sleep Cycle Integration Tests
// Verifies dream template catalog loading, trauma-driven nightmare triggers,
// peaceful sleep rest bonuses, compounding insomnia, dream interpretation,
// and state persistence.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Random;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests.Plan177DreamSleep
{
    public sealed class Plan177DreamSleepIntegrationTests
    {
        private static string ResolveDataPath(string filename)
        {
            var candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates)
            {
                if (File.Exists(c)) return Path.GetFullPath(c);
            }
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename));
        }

        [Fact]
        public void LoadCatalog_LoadsAllAuthoredDreamTemplates()
        {
            var sys = new DreamSystem();
            string path = ResolveDataPath("dream_templates.json");
            Assert.True(File.Exists(path), $"dream_templates.json must exist at {path}");

            string json = File.ReadAllText(path);
            sys.LoadCatalog(json);

            var templates = sys.GetAllTemplates();
            Assert.True(templates.Count >= 8, $"Expected >= 8 templates, found {templates.Count}");
            Assert.Contains(templates, t => t.template_id == "dream_prairie_sunlight");
            Assert.Contains(templates, t => t.template_id == "dream_collapsing_bunker");
            Assert.Contains(templates, t => t.template_id == "dream_black_rain_flash");
            Assert.Contains(templates, t => t.template_id == "dream_distant_beacon");
        }

        [Fact]
        public void ProcessSleepCycle_HighTraumaTriggersNightmare()
        {
            var sys = new DreamSystem();
            string path = ResolveDataPath("dream_templates.json");
            sys.LoadCatalog(File.ReadAllText(path));

            // Survivor with severe trauma (85) and low morale (10)
            var rng = new SeededRng(101);
            var result = sys.ProcessSleepCycle("survivor_hollow", trauma: 85.0f, morale: 10.0f, currentDay: 12, rng: rng);

            Assert.True(result.HadDream);
            Assert.NotNull(result.Record);
            Assert.Equal("nightmare", result.Record.dream_type);
            Assert.True(result.EffectiveRestBonus < 0f, "Nightmare should inflict rest penalty");
            Assert.True(result.TraumaDelta > 0f, "Nightmare should increase trauma");
            Assert.True(result.MoraleDelta < 0f, "Nightmare should decrease morale");
        }

        [Fact]
        public void ProcessSleepCycle_PeacefulDreamGrantsRestBonus()
        {
            var sys = new DreamSystem();
            string path = ResolveDataPath("dream_templates.json");
            sys.LoadCatalog(File.ReadAllText(path));

            // Survivor with very low trauma (5) and high morale (85)
            var rng = new SeededRng(555);
            var result = sys.ProcessSleepCycle("survivor_serene", trauma: 5.0f, morale: 85.0f, currentDay: 3, rng: rng);

            Assert.True(result.HadDream);
            Assert.NotNull(result.Record);
            Assert.Equal("peaceful", result.Record.dream_type);
            Assert.True(result.EffectiveRestBonus > 0f, "Peaceful dream should give rest bonus");
            Assert.True(result.TraumaDelta < 0f, "Peaceful dream should decrease trauma");
            Assert.True(result.MoraleDelta > 0f, "Peaceful dream should boost morale");
        }

        [Fact]
        public void ConsecutiveNightmares_CompoundingInsomniaPenalty()
        {
            var sys = new DreamSystem();
            sys.RegisterTemplate(new DreamTemplate
            {
                template_id = "test_nightmare",
                display_name = "Endless Abyss",
                dream_type = "nightmare",
                min_trauma = 0.0f,
                max_trauma = 100.0f,
                min_morale = 0.0f,
                max_morale = 100.0f,
                rest_bonus = -0.20f,
                trauma_delta = 5.0f,
                morale_delta = -10.0f
            });

            var rng = new SeededRng(77);
            string survivorId = "survivor_cursed";

            var res1 = sys.ProcessSleepCycle(survivorId, 70f, 20f, 1, rng, forceDream: true);
            var res2 = sys.ProcessSleepCycle(survivorId, 75f, 15f, 2, rng, forceDream: true);
            var res3 = sys.ProcessSleepCycle(survivorId, 80f, 10f, 3, rng, forceDream: true);

            Assert.Equal(3, sys.GetConsecutiveNightmares(survivorId));
            // Third nightmare suffers compounding insomnia: extra -0.15 rest and +4.0 trauma
            Assert.Equal(-0.35f, res3.EffectiveRestBonus, 2);
            Assert.Equal(9.0f, res3.TraumaDelta, 2);
        }

        [Fact]
        public void InterpretDream_MitigatesTraumaAndFiresEvent()
        {
            var sys = new DreamSystem();
            sys.RegisterTemplate(new DreamTemplate
            {
                template_id = "test_nightmare",
                display_name = "Falling Tower",
                dream_type = "nightmare",
                min_trauma = 0.0f,
                max_trauma = 100.0f,
                min_morale = 0.0f,
                max_morale = 100.0f,
                rest_bonus = -0.20f,
                trauma_delta = 6.0f,
                morale_delta = -8.0f,
                interpretation_insight = "Overcoming the fear of shelter collapse"
            });

            DreamRecord? interpretedEventRecord = null;
            sys.OnDreamInterpreted += (record) => interpretedEventRecord = record;

            var rng = new SeededRng(42);
            var res = sys.ProcessSleepCycle("survivor_maya", 50f, 30f, 5, rng);
            Assert.NotNull(res.Record);

            bool success = sys.InterpretDream("survivor_maya", res.Record.record_id, "Discussed with counselor at clinic");
            Assert.True(success);
            Assert.NotNull(interpretedEventRecord);
            Assert.True(interpretedEventRecord.is_interpreted);
            Assert.Equal("Discussed with counselor at clinic", interpretedEventRecord.interpretation_choice);
            // Nightmare interpretation reduces trauma by 3 and boosts morale by 5
            Assert.Equal(3.0f, interpretedEventRecord.trauma_delta, 2);
            Assert.Equal(-3.0f, interpretedEventRecord.morale_delta, 2);

            // Re-interpretation of the same record should fail
            bool repeat = sys.InterpretDream("survivor_maya", res.Record.record_id, "Another interpretation");
            Assert.False(repeat);
        }

        [Fact]
        public void SaveRestoreState_PreservesDreamHistoryAndNightmareCounters()
        {
            var sys = new DreamSystem();
            string path = ResolveDataPath("dream_templates.json");
            sys.LoadCatalog(File.ReadAllText(path));

            var rng = new SeededRng(888);
            sys.ProcessSleepCycle("survivor_a", 75f, 20f, 1, rng);
            var resB = sys.ProcessSleepCycle("survivor_b", 10f, 90f, 1, rng);
            if (resB.HadDream && resB.Record != null)
            {
                sys.InterpretDream("survivor_b", resB.Record.record_id, "Recorded in journal");
            }

            var captured = sys.CaptureState();
            Assert.NotNull(captured);

            var restoredSys = new DreamSystem();
            restoredSys.RestoreState(captured);

            var historyA = restoredSys.GetDreamHistory("survivor_a");
            var historyB = restoredSys.GetDreamHistory("survivor_b");
            Assert.Single(historyA);
            Assert.Single(historyB);
            Assert.True(historyB[0].is_interpreted);
            Assert.Equal("Recorded in journal", historyB[0].interpretation_choice);
            Assert.Equal(sys.GetConsecutiveNightmares("survivor_a"), restoredSys.GetConsecutiveNightmares("survivor_a"));
        }
    }
}
