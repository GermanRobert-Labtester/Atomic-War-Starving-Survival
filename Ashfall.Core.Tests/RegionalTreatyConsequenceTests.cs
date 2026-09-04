// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Economy;
using Ashfall.Core.YearOfAsh;
using Xunit;
using AshfallInventory = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan VIII · Task 21 — typed treaty consequences. Pins the effect
    /// contract (§21.2), lifecycle transitions incl. breach/expiry (§21.5–21.9),
    /// derived economy/raid consumers (§21.4–21.5), deterministic ordering
    /// (§21.11), save idempotency (§21.8) and the integration arc (§21.13).
    /// </summary>
    public class RegionalTreatyConsequenceTests
    {
        // ── fixtures ─────────────────────────────────────────────────────

        private static RegionalTreatySystem Create() => new RegionalTreatySystem();

        private static TreatyDefinition Def(
            string id, string faction, float termDays = 0f,
            params TreatyEffect[] effects)
        {
            var def = new TreatyDefinition
            {
                treaty_id = id,
                display_name = id,
                faction_id = faction,
                signatory_factions = new List<string> { faction },
                ratification_cost_scrap = 10f,
                compliance_check_interval_days = 30f,
                violation_penalty_affinity = -20f,
                term_days = termDays,
                effects = new List<TreatyEffect>(effects)
            };
            return def;
        }

        private static TreatyEffect Fx(string type, float value = 0f, string target = "") =>
            new TreatyEffect { effect_type = type, value = value, target_id = target };

        private static (RegionalTreatySystem sys, List<TreatyTransition> transitions) Capturing()
        {
            var sys = Create();
            var seen = new List<TreatyTransition>();
            sys.OnTreatyTransition += t => seen.Add(t);
            return (sys, seen);
        }

        private static void Ratify(RegionalTreatySystem sys, string id, int day = 1)
        {
            sys.TickDay(day);
            Assert.Equal(ActionResult.StatusKind.Success, sys.Propose(id).Status);
            Assert.Equal(ActionResult.StatusKind.Success, sys.Ratify(id, 100).Status);
        }

        // ── typed contract (§21.2) ──────────────────────────────────────

        [Fact]
        public void TryMapKind_KnownStrings_MapWithDefaults()
        {
            Assert.True(TreatyEffectTable.TryMapKind("economy_discount", out var trade, out var tradeFall));
            Assert.Equal(TreatyEffectKind.TradeDiscount, trade);
            Assert.Equal(TreatyEffectTable.DefaultTradeDiscount, tradeFall);

            Assert.True(TreatyEffectTable.TryMapKind("raid_pressure_relief", out var raid, out var raidFall));
            Assert.Equal(TreatyEffectKind.RaidPressureRelief, raid);
            Assert.Equal(TreatyEffectTable.DefaultRaidPressureRelief, raidFall);

            Assert.True(TreatyEffectTable.TryMapKind("water_quota", out var water, out _));
            Assert.Equal(TreatyEffectKind.WaterQuota, water);
            Assert.True(TreatyEffectTable.TryMapKind("power", out var power, out _));
            Assert.Equal(TreatyEffectKind.PowerQuota, power);

            Assert.False(TreatyEffectTable.TryMapKind("route_access", out _, out _));
        }

        [Fact]
        public void SourceId_StableIdentity_PerTreatyAndKind()
        {
            Assert.Equal("treaty:treaty_a:effect:trade_discount",
                TreatyActiveEffect.MakeSourceId("treaty_a", TreatyEffectKind.TradeDiscount));
        }

        // ── lifecycle transitions (§21.3–21.7, §21.9) ───────────────────

        [Fact]
        public void Propose_EmitsNoTransition()
        {
            var (sys, seen) = Capturing();
            sys.LoadCatalog(new List<TreatyDefinition> { Def("treaty_a", "fac_1", effects: Fx("economy_discount", 0.1f)) });
            sys.TickDay(1);
            Assert.Equal(ActionResult.StatusKind.Success, sys.Propose("treaty_a").Status);
            Assert.Empty(seen);
        }

        [Fact]
        public void Ratify_EmitsTypedTransition_WithStartedEffects()
        {
            var (sys, seen) = Capturing();
            sys.LoadCatalog(new List<TreatyDefinition> { Def("treaty_a", "fac_1", effects: Fx("economy_discount", 0.1f)) });
            Ratify(sys, "treaty_a");

            var t = Assert.Single(seen);
            Assert.Equal(TreatyStatus.Proposed, t.From);
            Assert.Equal(TreatyStatus.Ratified, t.To);
            Assert.Equal("fac_1", t.FactionId);
            Assert.False(t.IsBreach);
            var started = Assert.Single(t.StartedEffects);
            Assert.Equal(TreatyEffectKind.TradeDiscount, started.Kind);
            Assert.Equal(0.1f, started.Value, 4);
            Assert.Equal("treaty:treaty_a:effect:trade_discount", started.SourceId);
            Assert.Empty(t.EndedEffects);
        }

        [Fact]
        public void BreakTreaty_RemovesBenefit_EmitsBreachWithEndedEffects()
        {
            var (sys, seen) = Capturing();
            sys.LoadCatalog(new List<TreatyDefinition> { Def("treaty_a", "fac_1", effects: Fx("economy_discount", 0.1f)) });
            Ratify(sys, "treaty_a");
            Assert.Equal(0.1f, sys.GetTradeDiscount("fac_1"), 4);

            seen.Clear();
            Assert.Equal(ActionResult.StatusKind.Success, sys.BreakTreaty("treaty_a"));

            var t = Assert.Single(seen);
            Assert.Equal(TreatyViolationCause.Betrayal, t.Cause);
            Assert.True(t.IsBreach);
            Assert.Equal(TreatyStatus.Violated, t.To);
            Assert.Single(t.EndedEffects);
            Assert.Equal(0f, sys.GetTradeDiscount("fac_1"), 4);
        }

        [Fact]
        public void BreakTreaty_NotRatified_Blocks()
        {
            var sys = Create();
            sys.LoadCatalog(new List<TreatyDefinition> { Def("treaty_a", "fac_1") });
            Assert.Equal(ActionResult.StatusKind.Blocked, sys.BreakTreaty("treaty_a"));
        }

        [Fact]
        public void ComplianceFailure_EmitsBreachTransition_WithCause()
        {
            var (sys, seen) = Capturing();
            var def = Def("treaty_a", "fac_1", effects: Fx("economy_discount", 0.1f));
            def.compliance_check_interval_days = 1f; // decay every day for the test
            sys.LoadCatalog(new List<TreatyDefinition> { def });
            Ratify(sys, "treaty_a");
            seen.Clear();

            for (int day = 2; day <= 11; day++) sys.TickDay(day);

            var t = Assert.Single(seen);
            Assert.Equal(TreatyViolationCause.ComplianceFailure, t.Cause);
            Assert.Equal(TreatyStatus.Violated, t.To);
            Assert.Equal(0f, sys.GetTradeDiscount("fac_1"), 4);
        }

        [Fact]
        public void TermExpiry_TransitionsToExpired_RemovesEffects_NoBreach()
        {
            var (sys, seen) = Capturing();
            sys.LoadCatalog(new List<TreatyDefinition> { Def("treaty_a", "fac_1", termDays: 5, effects: Fx("raid_pressure_relief", 0.05f)) });
            Ratify(sys, "treaty_a", day: 1);
            Assert.Equal(-0.05f, sys.GetRaidPressureModifier(), 4);
            seen.Clear();

            sys.TickDay(5);   // not yet (5 - 1 >= 5 is false)
            Assert.Equal(TreatyStatus.Ratified, sys.State.treaties[0].status);
            Assert.Empty(seen);

            sys.TickDay(6);   // 6 - 1 >= 5 → expired

            var t = Assert.Single(seen);
            Assert.Equal(TreatyStatus.Expired, t.To);
            Assert.Equal(TreatyViolationCause.None, t.Cause);
            Assert.False(t.IsBreach);
            Assert.Single(t.EndedEffects);
            Assert.Equal(0f, sys.GetRaidPressureModifier(), 4); // no orphan modifier
            Assert.Equal(0f, sys.GetTradeDiscount("fac_1"), 4);
        }

        // ── save idempotency (§21.8) ─────────────────────────────────────

        [Fact]
        public void Restore_EmitsNoTransitions_AndDerivedStateIdentical()
        {
            var (sys, seen) = Capturing();
            sys.LoadCatalog(new List<TreatyDefinition> { Def("treaty_a", "fac_1", effects: Fx("economy_discount", 0.1f)) });
            Ratify(sys, "treaty_a");

            var saved = sys.CaptureState();

            var restored = Create();
            restored.LoadCatalog(new List<TreatyDefinition> { Def("treaty_a", "fac_1", effects: Fx("economy_discount", 0.1f)) });
            var restoredSeen = new List<TreatyTransition>();
            restored.OnTreatyTransition += t => restoredSeen.Add(t);
            restored.RestoreState(saved);

            Assert.Empty(restoredSeen); // restore must not re-fire transitions
            Assert.Equal(sys.GetTradeDiscount("fac_1"), restored.GetTradeDiscount("fac_1"));
            var d1 = sys.GetActiveEffectDescriptors();
            var d2 = restored.GetActiveEffectDescriptors();
            Assert.Single(d2);
            Assert.Equal(d1[0].SourceId, d2[0].SourceId);
        }

        [Fact]
        public void RestoreBrokenTreaty_DoesNotDuplicateBreachConsequence()
        {
            var war = new FactionWarSystem();
            var (sys, seen) = Capturing();
            sys.LoadCatalog(new List<TreatyDefinition> { Def("treaty_a", "fac_1", effects: Fx("economy_discount", 0.1f)) });
            Ratify(sys, "treaty_a");
            var savedActive = sys.CaptureState();

            // breach consumer mirrors the host wiring: exactly-once standing penalty
            sys.BreakTreaty("treaty_a");
            foreach (var t in seen)
                if (t.IsBreach) war.ModifyStanding(t.FactionId, -20);
            Assert.Equal(-20, war.GetStanding("fac_1"));

            // save AFTER breach, restore — transitions must not re-fire
            var savedBroken = sys.CaptureState();
            var restored = Create();
            restored.LoadCatalog(new List<TreatyDefinition> { Def("treaty_a", "fac_1", effects: Fx("economy_discount", 0.1f)) });
            int fired = 0;
            restored.OnTreatyTransition += _ => fired++;
            restored.RestoreState(savedBroken);

            Assert.Equal(0, fired);
            Assert.Equal(+TreatyEffectTable.BreachRaidPressure, restored.GetRaidPressureModifier(), 4);
            // the canonical escalation state is untouched by restore
            Assert.Equal(-20, war.GetStanding("fac_1"));
            Assert.True(restored.CountByStatus(TreatyStatus.Violated) == 1);
        }

        // ── derived raid pressure (§21.5) ────────────────────────────────

        [Fact]
        public void RaidModifier_RatifiedRelief_Negative_Violated_Positive_Clamped()
        {
            var sys = Create();
            sys.LoadCatalog(new List<TreatyDefinition>
            {
                Def("sec_pact", "fac_1", effects: Fx("raid_pressure_relief", 0.05f)),
                Def("treaty_b", "fac_2", effects: Fx("economy_discount", 0.1f)),
                Def("treaty_c", "fac_3"),
                Def("treaty_d", "fac_4")
            });

            Ratify(sys, "sec_pact");
            Assert.Equal(-0.05f, sys.GetRaidPressureModifier(), 4);

            Ratify(sys, "treaty_b", day: 2);
            sys.BreakTreaty("treaty_b"); // violated → +0.15 − 0.05 = +0.10
            Assert.Equal(0.10f, sys.GetRaidPressureModifier(), 4);

            Ratify(sys, "treaty_c", day: 3);
            sys.BreakTreaty("treaty_c"); // +0.25
            Ratify(sys, "treaty_d", day: 4);
            sys.BreakTreaty("treaty_d"); // +0.40
            Assert.Equal(0.40f, sys.GetRaidPressureModifier(), 4);
        }

        [Fact]
        public void RaidModifier_NeverExceedsClamp()
        {
            var sys = Create();
            var defs = new List<TreatyDefinition>();
            for (int i = 0; i < 6; i++)
                defs.Add(Def($"treaty_{i}", $"fac_{i}"));
            sys.LoadCatalog(defs);
            for (int i = 0; i < 6; i++)
            {
                Ratify(sys, $"treaty_{i}", day: i + 1);
                sys.BreakTreaty($"treaty_{i}");
            }
            Assert.Equal(TreatyEffectTable.RaidPressureModifierClamp, sys.GetRaidPressureModifier(), 4);
        }

        // ── economy consumers (§21.4) ────────────────────────────────────

        [Fact]
        public void TradeDiscount_MatchesAnySignatory_BestAcrossPacts()
        {
            var sys = Create();
            var multi = Def("treaty_multi", "fac_1", effects: Fx("economy_discount", 0.10f));
            multi.signatory_factions = new List<string> { "fac_1", "fac_2", "fac_9" };
            var solo = Def("treaty_solo", "fac_2", effects: Fx("economy_discount", 0.20f));
            sys.LoadCatalog(new List<TreatyDefinition> { multi, solo });

            Ratify(sys, "treaty_multi");
            Ratify(sys, "treaty_solo", day: 2);

            Assert.Equal(0.20f, sys.GetTradeDiscount("fac_2"), 4); // best of 0.10 / 0.20
            Assert.Equal(0.10f, sys.GetTradeDiscount("fac_1"), 4);
            Assert.Equal(0f, sys.GetTradeDiscount("fac_3"), 4);    // not a signatory
        }

        [Fact]
        public void CountByStatus_FeedsEscalationInput()
        {
            var sys = Create();
            sys.LoadCatalog(new List<TreatyDefinition>
            {
                Def("treaty_a", "fac_1"), Def("treaty_b", "fac_2")
            });
            Ratify(sys, "treaty_a");
            Ratify(sys, "treaty_b", day: 2);
            sys.BreakTreaty("treaty_b");

            Assert.Equal(1, sys.CountByStatus(TreatyStatus.Ratified));
            Assert.Equal(1, sys.CountByStatus(TreatyStatus.Violated));
        }

        // ── deterministic ordering (§21.11) ──────────────────────────────

        [Fact]
        public void Descriptors_OrderedDeterministically()
        {
            var sys = Create();
            sys.LoadCatalog(new List<TreatyDefinition>
            {
                Def("zz_last", "fac_2", effects: Fx("economy_discount", 0.1f), Fx("raid_pressure_relief", 0.05f)),
                Def("aa_first", "fac_1", effects: Fx("power"), Fx("water_quota", 120f))
            });
            Ratify(sys, "zz_last");
            Ratify(sys, "aa_first", day: 2);

            var d = sys.GetActiveEffectDescriptors();
            Assert.Equal(4, d.Count);
            Assert.Equal("aa_first", d[0].TreatyId);   // ordinal by treaty id
            Assert.Equal(TreatyEffectKind.WaterQuota, d[0].Kind);   // kind enum order within a treaty
            Assert.Equal(TreatyEffectKind.PowerQuota, d[1].Kind);
            Assert.Equal("zz_last", d[2].TreatyId);
            Assert.Equal(TreatyEffectKind.TradeDiscount, d[2].Kind);
            Assert.Equal(TreatyEffectKind.RaidPressureRelief, d[3].Kind);

            // same input → same order (stable, no dictionary enumeration)
            var again = sys.GetActiveEffectDescriptors();
            for (int i = 0; i < d.Count; i++)
                Assert.Equal(d[i].SourceId + d[i].TargetId, again[i].SourceId + again[i].TargetId);
        }

        // ── caravan price relief (§21.4) ─────────────────────────────────

        private static (CaravanTradeNetworkSystem system, CaravanManifestState manifest) Caravan()
        {
            var route = new CaravanRouteDefinition
            {
                route_id = "route_test",
                faction_id = "fac_1",
                export_surpluses = new List<string>()
            };
            var system = new CaravanTradeNetworkSystem(
                new[] { route }, new AshfallInventory(), new SeededRng(7));
            var manifest = new CaravanManifestState
            {
                manifest_id = "m1", route_id = "route_test", faction_id = "fac_1",
                status = CaravanStatus.Arrived
            };
            return (system, manifest);
        }

        [Fact]
        public void Caravan_TreatyRelief_ReducesBuyPrice_Visibly()
        {
            var (system, manifest) = Caravan();
            float baseline = system.CalculateItemBuyPrice(manifest, "clean_water");

            system.SetTreatyPriceReliefProvider(f => f == "fac_1" ? 0.10f : 0f);
            float relieved = system.CalculateItemBuyPrice(manifest, "clean_water");

            Assert.Equal(System.Math.Round(baseline * 0.9f, 2), relieved, 2);
            Assert.Equal(0.10f, system.GetTreatyPriceRelief("fac_1"), 4);
        }

        [Fact]
        public void Caravan_ReliefRemoved_PriceReturnsToBaseline()
        {
            var (system, manifest) = Caravan();
            float baseline = system.CalculateItemBuyPrice(manifest, "clean_water");

            system.SetTreatyPriceReliefProvider(_ => 0.10f);
            Assert.NotEqual(baseline, system.CalculateItemBuyPrice(manifest, "clean_water"));

            // treaty broken/expired → derived provider returns 0 → exact baseline
            system.SetTreatyPriceReliefProvider(_ => 0f);
            Assert.Equal(baseline, system.CalculateItemBuyPrice(manifest, "clean_water"));
        }

        [Fact]
        public void Caravan_Relief_ClampedAtHalf()
        {
            var (system, manifest) = Caravan();
            system.SetTreatyPriceReliefProvider(_ => 0.9f);
            Assert.Equal(0.5f, system.GetTreatyPriceRelief("fac_1"), 4);
        }

        // ── radio bulletins (§21.6) ──────────────────────────────────────

        [Fact]
        public void Bulletin_Ratified_StatesBenefitWhileAccordHolds()
        {
            var sys = Create();
            var def = Def("treaty_a", "fac_1", effects: Fx("economy_discount", 0.1f));
            sys.LoadCatalog(new List<TreatyDefinition> { def });
            Ratify(sys, "treaty_a");

            string line = TreatyBulletins.Compose(
                new TreatyTransition { TreatyId = "treaty_a", FactionId = "fac_1", From = TreatyStatus.Proposed, To = TreatyStatus.Ratified, StartedEffects = sys.GetActiveEffectDescriptors() },
                def);
            Assert.Contains("ratified", line);
            Assert.Contains("ease 10%", line);
            Assert.DoesNotContain("fac_1", line); // never render raw faction ids
        }

        [Fact]
        public void Bulletin_Breach_StatesLossAndMemory()
        {
            var sys = Create();
            var def = Def("treaty_a", "fac_1", effects: Fx("economy_discount", 0.1f));
            sys.LoadCatalog(new List<TreatyDefinition> { def });
            Ratify(sys, "treaty_a");
            sys.BreakTreaty("treaty_a");

            string line = TreatyBulletins.Compose(
                new TreatyTransition { TreatyId = "treaty_a", FactionId = "fac_1", From = TreatyStatus.Ratified, To = TreatyStatus.Violated, Cause = TreatyViolationCause.Betrayal, EndedEffects = sys.State.treaties.Count > 0 ? sys.GetActiveEffectDescriptors() : new List<TreatyActiveEffect>() },
                def);
            Assert.Contains("broken", line);
            Assert.Contains("back to full tariff", line);
        }

        // ── integration arc (§21.13) ─────────────────────────────────────

        [Fact]
        public void FullArc_RatifySaveRestoreBreach_ConsumersExactlyOnce()
        {
            var war = new FactionWarSystem();
            string? radioLine = null;
            int breachCount = 0;

            var sys = Create();
            var def = Def("treaty_a", "fac_1", effects: Fx("economy_discount", 0.1f));
            sys.LoadCatalog(new List<TreatyDefinition> { def });
            sys.OnTreatyTransition += t =>
            {
                radioLine = TreatyBulletins.Compose(t, def);
                if (t.IsBreach)
                {
                    breachCount++;
                    war.ModifyStanding(t.FactionId, (int)def.violation_penalty_affinity);
                }
            };

            // 1. ratify → cheaper prices, radio surfaces ratification
            Ratify(sys, "treaty_a");
            Assert.Equal(0.1f, sys.GetTradeDiscount("fac_1"), 4);
            Assert.Contains("ratified", radioLine);

            // 2. mid-treaty save → restore → benefit active once, nothing re-fired
            var saved = sys.CaptureState();
            var restored = Create();
            restored.LoadCatalog(new List<TreatyDefinition> { def });
            int restoredEvents = 0;
            restored.OnTreatyTransition += _ => restoredEvents++;
            restored.RestoreState(saved);
            Assert.Equal(0, restoredEvents);
            Assert.Equal(0.1f, restored.GetTradeDiscount("fac_1"), 4);

            // 3. betrayal → benefit removed, escalation input once, radio surfaces breach
            restored.OnTreatyTransition += t =>
            {
                radioLine = TreatyBulletins.Compose(t, def);
                if (t.IsBreach)
                {
                    breachCount++;
                    war.ModifyStanding(t.FactionId, (int)def.violation_penalty_affinity);
                }
            };
            Assert.Equal(ActionResult.StatusKind.Success, restored.BreakTreaty("treaty_a"));
            Assert.Equal(1, breachCount);
            Assert.Equal(-20, war.GetStanding("fac_1"));
            Assert.Equal(0f, restored.GetTradeDiscount("fac_1"), 4);
            Assert.Equal(+TreatyEffectTable.BreachRaidPressure, restored.GetRaidPressureModifier(), 4);
            Assert.Contains("broken", radioLine);

            // 4. save after breach → restore → no duplicate standing hit, no new radio
            radioLine = null;
            var savedBroken = restored.CaptureState();
            var restored2 = Create();
            restored2.LoadCatalog(new List<TreatyDefinition> { def });
            restored2.RestoreState(savedBroken);
            Assert.Null(radioLine);
            Assert.Equal(-20, war.GetStanding("fac_1")); // penalty applied exactly once
            Assert.Equal(+TreatyEffectTable.BreachRaidPressure, restored2.GetRaidPressureModifier(), 4);
        }

        // ── feed mapping over the authored corpus ─────────────────────────

        [Fact]
        public void Feed_TagDerivedEffects_AndTermDays()
        {
            var entry = new Ashfall.Core.Narrative.RegionalTreatyEntry
            {
                treaty_id = "treaty_test",
                treaty_title = "Test Security & Trade Pact",
                signatory_factions = new[] { "fac_1", "fac_2" },
                water_allocation_lpm = 120f,
                power_quota_kw = 15f,
                tags = new[] { "security", "trade" },
                term_days = 180f
            };
            var defs = RegionalTreatyFeed.Map(new[] { entry });
            var def = Assert.Single(defs);
            Assert.Equal(180f, def.term_days);
            Assert.Equal(2, def.signatory_factions.Count);

            var kinds = new List<TreatyEffectKind>();
            foreach (var e in def.effects)
                if (TreatyEffectTable.TryMapKind(e.effect_type, out var k, out _))
                    kinds.Add(k);
            Assert.Contains(TreatyEffectKind.WaterQuota, kinds);
            Assert.Contains(TreatyEffectKind.PowerQuota, kinds);
            Assert.Contains(TreatyEffectKind.RaidPressureRelief, kinds);
            Assert.Contains(TreatyEffectKind.TradeDiscount, kinds);
        }
    }
}
