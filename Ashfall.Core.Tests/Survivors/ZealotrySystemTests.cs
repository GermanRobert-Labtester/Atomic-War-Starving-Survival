// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 175 Phase 1 — ZealotrySystem contract tests (§7.20).
// Covers: strict catalog validation (fictional-only guard), no-belief
// baseline, deterministic conversion + resistance + leader modifier,
// fervor gain/decay, ritual resource demand + failure cost, despair
// resistance bounded (never bypasses morale), crisis reversal, dissent
// growth above the fanaticism threshold, escalation ladder with NO instant
// lethal purge (stops at AssaultThreat), de-escalation, multi-belief
// coexistence, PsyOps broadcast modifier, save/load, and old-save baseline.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.IO;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Plan175Social
{
    public sealed class ZealotrySystemTests
    {
        private static ZealotryBeliefProfile Witnesses() => new ZealotryBeliefProfile
        {
            belief_id = "belief_ash_witnesses",
            display_name = "The Ash Witnesses",
            doctrine_tags = { "memory", "penance" },
            conversion_base_bp = 1400,
            fervor_daily_decay_bp = 450,
            fervor_ritual_gain_bp = 1800,
            cohesion_bonus_bp = 600,
            despair_resistance_bp = 1600,
            fanaticism_threshold = 78,
            dissent_tolerance = "low",
            ritual_resource_item_ids = { "canned_food", "clean_water" },
            shrine_room_tags = { "social", "morale" },
            broadcast_profile = "neutral",
            tags = { "fictional" }
        };

        private static ZealotryBeliefProfile Rebuilders() => new ZealotryBeliefProfile
        {
            belief_id = "belief_rebuilders",
            display_name = "The Rebuilders",
            doctrine_tags = { "labor", "craft" },
            conversion_base_bp = 1100,
            fervor_daily_decay_bp = 380,
            fervor_ritual_gain_bp = 1400,
            cohesion_bonus_bp = 1200,
            despair_resistance_bp = 900,
            fanaticism_threshold = 85,
            dissent_tolerance = "medium",
            ritual_resource_item_ids = { "scrap_metal" },
            shrine_room_tags = { "social" },
            broadcast_profile = "receptive",
            tags = { "fictional" }
        };

        private static ZealotrySystem Create(params ZealotryBeliefProfile[] profiles) => new ZealotrySystem(profiles);

        private static string FindDataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string candidate = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data");
                if (File.Exists(Path.Combine(candidate, "wasteland_religions.json"))) return candidate;
                dir = dir.Parent;
            }
            return string.Empty;
        }

        // ── catalog ────────────────────────────────────────────────────

        [Fact]
        public void Loader_AuthoredCatalog_LoadsWithoutErrors()
        {
            string dataDir = FindDataDir();
            Assert.True(dataDir.Length > 0, "wasteland_religions.json not found");
            var result = ZealotryCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(result.HasErrors, string.Join("; ", result.Errors));
            Assert.True(result.Religions.Count >= 3);
        }

        [Fact]
        public void Loader_EnforcesFictionalOnlyGuard()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_plan175_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            try
            {
                // A row missing the fictional marker is rejected outright (§1.6).
                File.WriteAllText(Path.Combine(dir, ZealotryCatalogLoader.FileName),
                    "{\"schema_version\":1,\"religions\":[" +
                    "{\"belief_id\":\"belief_test\",\"display_name\":\"X\",\"conversion_base_bp\":1000," +
                    "\"ritual_resource_item_ids\":[\"canned_food\"],\"tags\":[\"not_marked\"]}]}");
                var r = ZealotryCatalogLoader.Load(dir, fileIO, json);
                Assert.True(r.HasErrors);
                Assert.Contains(r.Errors, e => e.Contains("fictional"));
            }
            finally
            {
                Directory.Delete(dir, recursive: true);
            }
        }

        // ── no-belief baseline ─────────────────────────────────────────

        [Fact]
        public void NoBeliefBaseline_ZeroModifiers()
        {
            var system = Create(Witnesses());
            Assert.Equal(0, system.GetDespairResistanceBp("survivor_a"));
            Assert.Equal(0, system.GetCohesionBonusBp("belief_ash_witnesses"));
            Assert.Null(system.Believer("survivor_a"));
        }

        // ── conversion (§7.4) ──────────────────────────────────────────

        private static ConversionContext Ctx(float stress = 0.5f, float charisma = 0.5f,
            float shrine = 0f, float bond = 0f, bool leaderActive = true) => new ConversionContext
        {
            Stress01 = stress, LeaderCharisma01 = charisma, ShrineInfluence01 = shrine,
            RelationshipBond01 = bond, LeaderOfSameBelief = leaderActive
        };

        [Fact]
        public void Conversion_Deterministic_SameSeedSameOutcome()
        {
            var run = (int seed) =>
            {
                var system = Create(Witnesses());
                var r = system.TryConvert("survivor_a", "belief_ash_witnesses", 5, Ctx(), new SeededRng(seed));
                return (r.Converted, r.ReasonCode, r.Belief?.conviction ?? -1, r.Belief?.fervor ?? -1);
            };
            Assert.Equal(run(11), run(11));
        }

        [Fact]
        public void Conversion_ResistanceExists_CommittedBelieverResistsHard()
        {
            var system = Create(Witnesses(), Rebuilders());
            // Establish a committed rebuilder (conviction 100 via leader register + rituals).
            system.RegisterLeader("survivor_a", "belief_rebuilders");
            for (int i = 0; i < 6; i++) system.RecordRitual("belief_rebuilders", i);
            var a = system.Believer("survivor_a")!;
            a.conviction = 100;

            // High-stress recruitment still mostly resisted (agency preserved).
            int converted = 0;
            for (int seed = 1; seed <= 20; seed++)
            {
                var fresh = Create(Witnesses(), Rebuilders());
                fresh.RegisterLeader("survivor_a", "belief_rebuilders");
                var st = fresh.Believer("survivor_a")!;
                st.conviction = 100;
                var r = fresh.TryConvert("survivor_a", "belief_ash_witnesses", 5,
                    Ctx(stress: 0.9f, charisma: 0.9f), new SeededRng(seed));
                if (r.Converted) converted++;
            }
            Assert.True(converted <= 10, $"a committed believer must resist hard (converted {converted}/20)");
        }

        [Fact]
        public void Conversion_AlreadyMember_DeepensConviction_NeverDoubleConverts()
        {
            var system = Create(Witnesses());
            system.RegisterLeader("survivor_a", "belief_ash_witnesses");
            var before = system.Believer("survivor_a")!.conviction;

            var r = system.TryConvert("survivor_a", "belief_ash_witnesses", 5, Ctx(), new SeededRng(1));
            Assert.False(r.Converted);
            Assert.Equal("already_member", r.ReasonCode);
            Assert.True(system.Believer("survivor_a")!.conviction >= before);
        }

        [Fact]
        public void Conversion_LeaderFactor_RaisesChance()
        {
            // Low-charisma, no-leader context converts rarely across seeds.
            int withLeader = 0, withoutLeader = 0;
            for (int seed = 1; seed <= 40; seed++)
            {
                var a = Create(Witnesses());
                if (a.TryConvert("s", "belief_ash_witnesses", 5, Ctx(charisma: 0.9f, leaderActive: true), new SeededRng(seed)).Converted) withLeader++;
                var b = Create(Witnesses());
                if (b.TryConvert("s", "belief_ash_witnesses", 5, Ctx(charisma: 0.0f, leaderActive: false), new SeededRng(seed)).Converted) withoutLeader++;
            }
            Assert.True(withLeader > withoutLeader, "charismatic leader must raise conversion odds");
            Assert.True(withoutLeader > 0, "no-leader contexts still occasionally convert (bounded, not zero)");
        }

        // ── fervor ─────────────────────────────────────────────────────

        [Fact]
        public void Fervor_RitualGains_DecayClamped()
        {
            var system = Create(Witnesses());
            system.RegisterLeader("survivor_a", "belief_ash_witnesses");
            var a = system.Believer("survivor_a")!;

            int fervorStart = a.fervor;
            system.RecordRitual("belief_ash_witnesses", 3);
            Assert.True(a.fervor > fervorStart);

            // Daily decay pulls it back down; never below zero.
            for (int day = 4; day <= 40; day++) system.TickDay(day);
            Assert.Equal(0, a.fervor);
        }

        // ── rituals & resource demands (§7.10, §16) ────────────────────

        [Fact]
        public void RitualDemand_BoundedCadence_FailureCostsFervor()
        {
            var system = Create(Witnesses());
            system.RegisterLeader("survivor_a", "belief_ash_witnesses");

            var demand = system.EmitRitualDemand("belief_ash_witnesses", 10);
            Assert.NotNull(demand);
            Assert.Contains("canned_food", demand!.ItemIds);

            // Missing resources → ritual fails, fervor drops, no free buff.
            var before = system.Believer("survivor_a")!.fervor;
            system.ResolveRitualDemand("belief_ash_witnesses", 10, resourcesAvailable: false);
            Assert.True(system.Believer("survivor_a")!.fervor < before);

            // Fulfilled demand resolves and re-gains through RecordRitual.
            system.EmitRitualDemand("belief_ash_witnesses", 20);
            var preFulfill = system.Believer("survivor_a")!.fervor;
            system.ResolveRitualDemand("belief_ash_witnesses", 20, resourcesAvailable: true);
            Assert.True(system.Believer("survivor_a")!.fervor > preFulfill);
        }

        // ── bounded buffs (§7.7-7.8: modify, never bypass) ─────────────

        [Fact]
        public void DespairResistance_Bounded_NeverBypassesMorale()
        {
            var system = Create(Witnesses());
            system.RegisterLeader("survivor_a", "belief_ash_witnesses");
            system.SetShrine("belief_ash_witnesses", true);
            var a = system.Believer("survivor_a")!;
            a.conviction = 100;
            a.fervor = 100;

            int resistance = system.GetDespairResistanceBp("survivor_a");
            Assert.InRange(resistance, 0, ZealotryCaps.MaxDespairResistanceBp);
            Assert.True(resistance > 0);

            // Crisis erases the modifier (morale damage routes through the host).
            system.TriggerCrisis("belief_ash_witnesses", 5);
            Assert.Equal(0, system.GetDespairResistanceBp("survivor_a"));
        }

        [Fact]
        public void CohesionBonus_ScalesWithMembership_ShrineBoosts()
        {
            var system = Create(Rebuilders());
            Assert.Equal(0, system.GetCohesionBonusBp("belief_rebuilders"));

            system.RegisterLeader("survivor_a", "belief_rebuilders");
            int withoutShrine = system.GetCohesionBonusBp("belief_rebuilders");
            system.SetShrine("belief_rebuilders", true);
            int withShrine = system.GetCohesionBonusBp("belief_rebuilders");
            Assert.True(withoutShrine > 0);
            Assert.True(withShrine > withoutShrine);
            Assert.InRange(withShrine, 0, ZealotryCaps.MaxCohesionBonusBp);
        }

        // ── dissent (§7.7 costs) ───────────────────────────────────────

        [Fact]
        public void Dissent_GrowsAboveFanaticismThreshold_LowToleranceFastest()
        {
            var system = Create(Witnesses());
            system.RegisterLeader("survivor_a", "belief_ash_witnesses");
            var a = system.Believer("survivor_a")!;
            a.fervor = 95; // above threshold (78), low tolerance → +3/day

            system.TickDay(5);
            Assert.True(a.dissent > 0);
        }

        // ── escalation ladder (§7.11) ──────────────────────────────────

        [Fact]
        public void Escalation_AdvancesToAssaultThreat_NeverInstantLethal()
        {
            var system = Create(Witnesses(), Rebuilders());
            system.RegisterLeader("a1", "belief_ash_witnesses");
            system.RegisterLeader("b1", "belief_rebuilders");
            var w = system.Believer("a1")!;
            var r = system.Believer("b1")!;
            w.fervor = 100;   // above witness threshold 78
            r.fervor = 100;   // above rebuilder threshold 85
            // belief_ash_witnesses ↔ belief_rebuilders is a friction conflict pair.

            var stages = new List<ZealotEscalationStage>();
            system.OnEscalationStageChanged += s => stages.Add(s);

            // Sustained tension: fervor is topped up daily (as repeated rituals
            // would in-game) — decay alone would exhaust it in ~6 days.
            for (int day = 1; day <= 60; day++)
            {
                w.fervor = 100;
                r.fervor = 100;
                system.TickDay(day);
            }

            Assert.Contains(ZealotEscalationStage.AssaultThreat, stages);
            Assert.Equal((int)ZealotEscalationStage.AssaultThreat, system.State.escalation_stage);
            // NO stage beyond the typed threat exists in state (no silent purge).
            Assert.True((ZealotEscalationStage)system.State.escalation_stage <= ZealotEscalationStage.AssaultThreat);
        }

        [Fact]
        public void Escalation_Deescalates_WhenTensionResolves()
        {
            var system = Create(Witnesses(), Rebuilders());
            system.RegisterLeader("a1", "belief_ash_witnesses");
            system.RegisterLeader("b1", "belief_rebuilders");
            var w = system.Believer("a1")!;
            var r = system.Believer("b1")!;
            w.fervor = 100;
            r.fervor = 100;
            for (int day = 1; day <= 20; day++)
            {
                w.fervor = 100;
                r.fervor = 100;
                system.TickDay(day);
            }
            Assert.True(system.State.escalation_stage > 0);

            // Fervor collapses → tension gone → ladder walks back down.
            w.fervor = 0;
            r.fervor = 0;
            for (int day = 21; day <= 40; day++) system.TickDay(day);
            Assert.Equal((int)ZealotEscalationStage.None, system.State.escalation_stage);
        }

        // ── multi-belief coexistence (§7.12) ───────────────────────────

        [Fact]
        public void MultiBelief_Coexistence_PeacefulWithoutFanaticism()
        {
            var system = Create(Witnesses(), Rebuilders());
            system.RegisterLeader("a1", "belief_ash_witnesses");
            system.RegisterLeader("b1", "belief_rebuilders");
            // Low fervor → coexistence stays calm at every stage.
            for (int day = 1; day <= 30; day++) system.TickDay(day);
            Assert.Equal((int)ZealotEscalationStage.None, system.State.escalation_stage);
        }

        // ── broadcast (§7.13) ──────────────────────────────────────────

        [Fact]
        public void Broadcast_RaisesFervorOfAdherents_NeverConvertsOutsiders()
        {
            var system = Create(Rebuilders());
            system.RegisterLeader("survivor_a", "belief_rebuilders");
            var before = system.Believer("survivor_a")!.fervor;

            system.ApplyBroadcast("belief_rebuilders", reach01: 1f);
            Assert.True(system.Believer("survivor_a")!.fervor > before);

            // No new believer appears from a broadcast alone.
            Assert.Null(system.Believer("survivor_untouched"));
        }

        // ── crisis reversal (§7.8) ─────────────────────────────────────

        [Fact]
        public void Crisis_Reversal_SlowPartialRecovery()
        {
            var system = Create(Witnesses());
            system.RegisterLeader("survivor_a", "belief_ash_witnesses");
            var a = system.Believer("survivor_a")!;
            a.conviction = 90;

            system.TriggerCrisis("belief_ash_witnesses", 5);
            Assert.True(a.in_crisis);
            Assert.True(a.conviction < 90);

            system.ResolveCrisis("belief_ash_witnesses");
            Assert.False(a.in_crisis);
            Assert.InRange(a.conviction, 0, ZealotryCaps.MaxConviction);
        }

        // ── persistence ────────────────────────────────────────────────

        [Fact]
        public void SaveLoad_PreservesExactBeliefState()
        {
            var system = Create(Witnesses(), Rebuilders());
            system.RegisterLeader("a1", "belief_ash_witnesses");
            system.TryConvert("b1", "belief_rebuilders", 4, Ctx(), new SeededRng(3));
            system.SetShrine("belief_ash_witnesses", true);
            for (int day = 1; day <= 10; day++) system.TickDay(day);

            var saved = system.CaptureState();
            var restored = Create(Witnesses(), Rebuilders());
            restored.RestoreState(saved);

            Assert.Equal(saved.believers.Count, restored.State.believers.Count);
            var a = saved.believers[0];
            var b = restored.State.believers[0];
            Assert.Equal(a.survivor_id, b.survivor_id);
            Assert.Equal(a.belief_id, b.belief_id);
            Assert.Equal(a.conviction, b.conviction);
            Assert.Equal(a.fervor, b.fervor);
            Assert.Equal(a.converted_day, b.converted_day);
            Assert.Equal(a.dissent, b.dissent);
            Assert.Equal(a.role, b.role);
            Assert.Equal(saved.escalation_stage, restored.State.escalation_stage);
            Assert.Equal(saved.shrine_belief_ids.Count, restored.State.shrine_belief_ids.Count);
        }

        [Fact]
        public void OldSaveBaseline_RestoreFromNullIsSafe()
        {
            var system = Create(Witnesses());
            system.RestoreState(null);
            Assert.Empty(system.State.believers);
            Assert.Equal(0, system.GetDespairResistanceBp("anyone"));
            Assert.Equal((int)ZealotEscalationStage.None, system.State.escalation_stage);
        }

        [Fact]
        public void SplitRun_MidReloadProducesSameFervorTrack()
        {
            var continuous = Create(Witnesses());
            continuous.RegisterLeader("a1", "belief_ash_witnesses");
            for (int day = 1; day <= 10; day++) continuous.TickDay(day);

            var split = Create(Witnesses());
            split.RegisterLeader("a1", "belief_ash_witnesses");
            for (int day = 1; day <= 5; day++) split.TickDay(day);
            var restored = Create(Witnesses());
            restored.RestoreState(split.CaptureState());
            for (int day = 6; day <= 10; day++) restored.TickDay(day);

            Assert.Equal(
                continuous.Believer("a1")!.fervor,
                restored.Believer("a1")!.fervor);
        }
    }
}
