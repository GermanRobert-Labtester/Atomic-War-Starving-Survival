// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 139: Combat → Faction Standing Bridge — Integration Tests
// Verifies threshold catalog loading, consequence evaluation (kills/downed/
// assisted/friendly-fire), self-defence multiplier, exactly-once application,
// and threshold lookup by casualty count.
// ============================================================================
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests.Plan139CombatFactionBridge
{
    public sealed class Plan139CombatFactionBridgeIntegrationTests
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
            foreach (var c in candidates) { if (File.Exists(c)) return Path.GetFullPath(c); }
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename));
        }

        private static CombatState MakeWonState(string encounterId, int day, List<CombatantState> combatants)
        {
            return new CombatState
            {
                EncounterId = encounterId,
                Day = day,
                Phase = (int)CombatPhase.Won,
                Combatants = combatants,
                AppliedFactionConsequenceIds = new List<string>()
            };
        }

        [Fact]
        public void LoadCatalog_LoadsThresholdsAndFactionRules()
        {
            var bridge = new CombatFactionStandingBridge();
            string path = ResolveDataPath("faction_combat_thresholds.json");
            Assert.True(File.Exists(path), $"faction_combat_thresholds.json must exist at {path}");

            bridge.LoadCatalog(File.ReadAllText(path));

            var thresholds = bridge.GetAllThresholds();
            Assert.Equal(3, thresholds.Count);
            Assert.Contains(thresholds, t => t.threshold_id == "thresh_minor_skirmish");
            Assert.Contains(thresholds, t => t.threshold_id == "thresh_major_battle");

            var rules = bridge.GetAllFactionRules();
            Assert.Equal(3, rules.Count);
            Assert.Contains(rules, r => r.faction_id == "faction_wardens");
        }

        [Fact]
        public void GetThresholdForCasualties_ReturnsCorrectBand()
        {
            var bridge = new CombatFactionStandingBridge();
            bridge.LoadCatalog(File.ReadAllText(ResolveDataPath("faction_combat_thresholds.json")));

            var minor = bridge.GetThresholdForCasualties(1);
            Assert.NotNull(minor);
            Assert.Equal("thresh_minor_skirmish", minor!.threshold_id);

            var standard = bridge.GetThresholdForCasualties(4);
            Assert.NotNull(standard);
            Assert.Equal("thresh_standard_engagement", standard!.threshold_id);

            var major = bridge.GetThresholdForCasualties(10);
            Assert.NotNull(major);
            Assert.Equal("thresh_major_battle", major!.threshold_id);
        }

        [Fact]
        public void EvaluateConsequences_KillsProducePenaltyDelta()
        {
            var bridge = new CombatFactionStandingBridge();
            var state = MakeWonState("enc_001", 5, new List<CombatantState>
            {
                new CombatantState { Id = "e1", FactionId = "faction_wardens", IsPlayer = false, Health = 0f },
                new CombatantState { Id = "e2", FactionId = "faction_wardens", IsPlayer = false, Health = 0f },
            });

            var consequences = CombatFactionStandingBridge.EvaluateConsequences(state, isSelfDefense: false);

            Assert.NotEmpty(consequences);
            var wardensConsequence = consequences.FirstOrDefault(c => c.FactionId == "faction_wardens");
            Assert.NotNull(wardensConsequence);
            Assert.True(wardensConsequence!.StandingDelta < 0f, "Kills should produce negative standing delta");
            Assert.Equal(2, wardensConsequence.Kills);
        }

        [Fact]
        public void EvaluateConsequences_SelfDefenseHalvesThePenalty()
        {
            var bridge = new CombatFactionStandingBridge();
            var state = MakeWonState("enc_002", 6, new List<CombatantState>
            {
                new CombatantState { Id = "e1", FactionId = "faction_scavengers", IsPlayer = false, Health = 0f },
            });

            var normal = CombatFactionStandingBridge.EvaluateConsequences(state, isSelfDefense: false);
            var selfDef = CombatFactionStandingBridge.EvaluateConsequences(state, isSelfDefense: true);

            float normalDelta = normal.First(c => c.FactionId == "faction_scavengers").StandingDelta;
            float selfDefDelta = selfDef.First(c => c.FactionId == "faction_scavengers").StandingDelta;

            Assert.True(selfDefDelta > normalDelta, "Self-defense should produce a smaller (less negative) penalty");
            Assert.Equal(normalDelta * CombatFactionStandingBridge.SelfDefenseMultiplier, selfDefDelta, 2);
        }

        [Fact]
        public void ApplyConsequences_ExactlyOnce_PreventsDoubleApplication()
        {
            var bridge = new CombatFactionStandingBridge();
            var state = MakeWonState("enc_003", 7, new List<CombatantState>
            {
                new CombatantState { Id = "e1", FactionId = "faction_traders_guild", IsPlayer = false, Health = 0f },
            });

            var consequences = CombatFactionStandingBridge.EvaluateConsequences(state).ToList();
            var appliedIds = new HashSet<string>(state.AppliedFactionConsequenceIds);
            var standingLog = new Dictionary<string, int>();

            int first = bridge.ApplyConsequences(consequences,
                (fId, delta) => standingLog[fId] = standingLog.GetValueOrDefault(fId) + delta,
                appliedIds);

            int second = bridge.ApplyConsequences(consequences,
                (fId, delta) => standingLog[fId] = standingLog.GetValueOrDefault(fId) + delta,
                appliedIds);

            Assert.True(first > 0, "First application should apply consequences");
            Assert.Equal(0, second);   // already applied — no double-dip
        }

        [Fact]
        public void EvaluateConsequences_WonWithAlly_GrantsAssistanceBonus()
        {
            var bridge = new CombatFactionStandingBridge();
            var state = MakeWonState("enc_004", 8, new List<CombatantState>
            {
                new CombatantState { Id = "p1", FactionId = "faction_militia", IsPlayer = true, Health = 80f },
                new CombatantState { Id = "e1", FactionId = "faction_raiders", IsPlayer = false, Health = 0f },
            });

            var consequences = CombatFactionStandingBridge.EvaluateConsequences(state, isSelfDefense: false);

            var assistBonus = consequences.FirstOrDefault(c =>
                c.FactionId == "faction_militia" && c.Reason == "combat_assistance");
            Assert.NotNull(assistBonus);
            Assert.True(assistBonus!.StandingDelta > 0f, "Allied player faction should get assistance bonus");
        }
    }
}
