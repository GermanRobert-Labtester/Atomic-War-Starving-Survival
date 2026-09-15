// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    /// <summary>
    /// Plan 14A (C1 checkpoint C1.2) — TradeEmbargoSystem: rule evaluation,
    /// route blocking/slowing, price multipliers, decay-to-exactly-neutral,
    /// deterministic reactivation, save/load round trip, malformed-rule
    /// rejection, and trade_embargoes.json characterization. Caravan and
    /// market wiring belong to checkpoint C1.3.
    /// </summary>
    public sealed class TradeEmbargoSystemTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        private static TradeEmbargoSystem LoadRealSystem()
        {
            var load = TradeEmbargoCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(load.HasErrors, string.Join("; ", load.Errors));
            return new TradeEmbargoSystem(TradeEmbargoCatalogLoader.ToCatalog(load));
        }

        private static EmbargoRule MakeRule(
            string id = "embargo_test_rule",
            WeatherKind weather = WeatherKind.FalloutStorm,
            string[]? regions = null,
            string[]? categories = null,
            string[]? items = null,
            bool all = false,
            int permille = 1500,
            bool blocked = true,
            int slow = 1000,
            int decay = 2)
        {
            return new EmbargoRule(id, weather,
                regions ?? new[] { "*" },
                categories ?? (all ? Array.Empty<string>() : new[] { "medical" }),
                items ?? Array.Empty<string>(),
                all, permille, blocked, slow, decay);
        }

        // ── Pure rule evaluation ──────────────────────────────────────────

        [Fact]
        public void NeutralWeather_NoEmbargoActive_AllQueriesNeutral()
        {
            var system = LoadRealSystem();
            Assert.False(system.IsEmbargoActive(WeatherKind.Clear));
            Assert.False(system.IsRouteBlocked("foundry", WeatherKind.Clear));
            Assert.Equal(1f, system.GetRouteProgressMultiplier("foundry", WeatherKind.Clear));
            Assert.Equal(1000, system.GetWeatherPriceMultiplierPermille("foundry", WeatherKind.Clear, "bandages", "medical"));
            Assert.Equal(1000, system.GetCurrentPriceMultiplierPermille("foundry", "bandages", "medical"));
        }

        [Fact]
        public void FalloutStorm_AllRegions_Blocked_AndMedicalSurcharged()
        {
            var system = LoadRealSystem();
            Assert.True(system.IsEmbargoActive(WeatherKind.FalloutStorm));
            foreach (var region in new[] { "flotilla", "foundry", "greenhouse", "traplines", "settlement" })
                Assert.True(system.IsRouteBlocked(region, WeatherKind.FalloutStorm), $"{region} must be blocked");
            Assert.Equal(1500, system.GetWeatherPriceMultiplierPermille("foundry", WeatherKind.FalloutStorm, "bandages", "medical"));
            Assert.Equal(1500, system.GetWeatherPriceMultiplierPermille("flotilla", WeatherKind.FalloutStorm, "anti_rad", "medical"));
        }

        [Fact]
        public void RegionSpecificBlock_AppliesOnlyToCoveredRegion()
        {
            var system = LoadRealSystem(); // BlackRain blocks flotilla only.
            Assert.True(system.IsRouteBlocked("flotilla", WeatherKind.BlackRain));
            Assert.False(system.IsRouteBlocked("foundry", WeatherKind.BlackRain));
            Assert.False(system.IsRouteBlocked("traplines", WeatherKind.BlackRain));
        }

        [Fact]
        public void UnaffectedGood_RemainsNeutral_UnderActiveEmbargo()
        {
            var system = LoadRealSystem(); // FalloutStorm targets medical only.
            Assert.Equal(1000, system.GetWeatherPriceMultiplierPermille("foundry", WeatherKind.FalloutStorm, "clean_water", "water"));
            Assert.Equal(1000, system.GetWeatherPriceMultiplierPermille("foundry", WeatherKind.FalloutStorm, "cooked_meat", "food"));
        }

        [Fact]
        public void AllGoodsRule_SurchargesEveryCategory()
        {
            var system = LoadRealSystem(); // RadHail: affects_all_goods 1250.
            foreach (var (item, category) in new[] { ("clean_water", "water"), ("bandages", "medical"), ("scrap_metal", "materials") })
                Assert.Equal(1250, system.GetWeatherPriceMultiplierPermille("foundry", WeatherKind.RadHail, item, category));
        }

        [Fact]
        public void NullOrUnknownRegion_NeverCrashes_NeverMatchesWildcard()
        {
            var system = LoadRealSystem();
            Assert.False(system.IsRouteBlocked(null, WeatherKind.FalloutStorm));
            Assert.False(system.IsRouteBlocked("", WeatherKind.FalloutStorm));
            Assert.False(system.IsRouteBlocked("not_a_region", WeatherKind.FalloutStorm));
            Assert.Equal(1000, system.GetCurrentPriceMultiplierPermille(null, "bandages", "medical"));
        }

        [Fact]
        public void SlowRouteWeather_ChangesProgress_WithoutBlocking()
        {
            var system = LoadRealSystem(); // AcidSnow: slow 500, not blocked, tools +30%.
            Assert.False(system.IsRouteBlocked("foundry", WeatherKind.AcidSnow));
            Assert.Equal(0.5f, system.GetRouteProgressMultiplier("foundry", WeatherKind.AcidSnow));
            Assert.Equal(1300, system.GetWeatherPriceMultiplierPermille("foundry", WeatherKind.AcidSnow, "crowbar", "tools"));
            Assert.Equal(1000, system.GetWeatherPriceMultiplierPermille("foundry", WeatherKind.AcidSnow, "clean_water", "water"));
            Assert.Equal(0.7f, system.GetRouteProgressMultiplier("settlement", WeatherKind.IceStorm));
        }

        [Fact]
        public void CombinedRules_ProductAppliesEachRuleOnce_AndClampsToShockBand()
        {
            var system = new TradeEmbargoSystem();
            Assert.True(system.RegisterRule(new EmbargoRule("embargo_combo_item", WeatherKind.FalloutStorm,
                new[] { "*" }, Array.Empty<string>(), new[] { "bandages" }, false, 1500, true, 1000, 2)));
            Assert.True(system.RegisterRule(new EmbargoRule("embargo_combo_category", WeatherKind.FalloutStorm,
                new[] { "*" }, new[] { "medical" }, Array.Empty<string>(), false, 1300, true, 1000, 2)));
            // 1.5 × 1.3 = 1.95 — each rule applied exactly once, inside the [0.4, 2.5] band.
            Assert.Equal(1950, system.GetWeatherPriceMultiplierPermille("foundry", WeatherKind.FalloutStorm, "bandages", "medical"));
            // An unaffected good stays neutral under both rules.
            Assert.Equal(1000, system.GetWeatherPriceMultiplierPermille("foundry", WeatherKind.FalloutStorm, "clean_water", "water"));

            var capped = new TradeEmbargoSystem();
            Assert.True(capped.RegisterRule(new EmbargoRule("embargo_cap_a", WeatherKind.GlassStorm,
                new[] { "*" }, Array.Empty<string>(), new[] { "scrap_metal" }, false, 2500, true, 1000, 2)));
            Assert.True(capped.RegisterRule(new EmbargoRule("embargo_cap_b", WeatherKind.GlassStorm,
                new[] { "*" }, new[] { "materials" }, Array.Empty<string>(), false, 2500, true, 1000, 2)));
            // 2.5 × 2.5 = 6.25 → clamped to the market shock-product ceiling 2.5.
            Assert.Equal(2500, capped.GetWeatherPriceMultiplierPermille("foundry", WeatherKind.GlassStorm, "scrap_metal", "materials"));
        }

        // ── Decay state machine ───────────────────────────────────────────

        [Fact]
        public void WeatherClears_RouteResumesImmediately_ShockEntersDecay()
        {
            var system = LoadRealSystem();
            system.NotifyWeather(10, WeatherKind.FalloutStorm);
            Assert.Equal(1500, system.GetCurrentPriceMultiplierPermille("foundry", "bandages", "medical"));

            system.NotifyWeather(11, WeatherKind.Clear); // routes recover instantly
            Assert.False(system.IsRouteBlocked("foundry", WeatherKind.Clear));
            // decay_days = 2 for the fallout rule → first decay step.
            var shock = Assert.Single(system.ActiveShocks);
            Assert.InRange(shock.currentPermille, 1000, shock.peakPermille);
            Assert.Equal(1, shock.remainingDecayDays);
            Assert.True(shock.currentPermille < 1500, "first decay step must move below peak");
            Assert.Equal(shock.currentPermille, system.GetCurrentPriceMultiplierPermille("foundry", "bandages", "medical"));
        }

        [Fact]
        public void DecayReachesExactlyNeutral_ShockRemoved()
        {
            var system = LoadRealSystem();
            system.NotifyWeather(10, WeatherKind.FalloutStorm);
            system.NotifyWeather(11, WeatherKind.Clear);
            system.NotifyWeather(12, WeatherKind.Clear);
            Assert.Empty(system.ActiveShocks);
            Assert.Equal(1000, system.GetCurrentPriceMultiplierPermille("foundry", "bandages", "medical"));
        }

        [Fact]
        public void ReactivationDuringDecay_IsDeterministic_RestoresPeak()
        {
            var first = LoadRealSystem();
            first.NotifyWeather(10, WeatherKind.FalloutStorm);
            first.NotifyWeather(11, WeatherKind.Clear); // decaying
            first.NotifyWeather(12, WeatherKind.FalloutStorm); // reactivated

            var second = LoadRealSystem();
            second.NotifyWeather(12, WeatherKind.FalloutStorm);

            var a = first.CaptureState();
            var b = second.CaptureState();
            Assert.Equal(b.shocks.Count, a.shocks.Count);
            for (int i = 0; i < b.shocks.Count; i++)
            {
                Assert.Equal(b.shocks[i].ruleId, a.shocks[i].ruleId);
                Assert.Equal(b.shocks[i].currentPermille, a.shocks[i].currentPermille);
                Assert.Equal(b.shocks[i].remainingDecayDays, a.shocks[i].remainingDecayDays);
            }
            Assert.Equal(1500, first.GetCurrentPriceMultiplierPermille("foundry", "bandages", "medical"));
        }

        [Fact]
        public void SameDayReNotify_DoesNotDoubleDecay()
        {
            var system = LoadRealSystem();
            system.NotifyWeather(10, WeatherKind.FalloutStorm);
            system.NotifyWeather(11, WeatherKind.Clear);
            var afterFirst = system.CaptureState();
            system.NotifyWeather(11, WeatherKind.Clear); // same day again
            var afterRepeat = system.CaptureState();
            Assert.Equal(afterFirst.shocks[0].remainingDecayDays, afterRepeat.shocks[0].remainingDecayDays);
            Assert.Equal(afterFirst.shocks[0].currentPermille, afterRepeat.shocks[0].currentPermille);
        }

        [Fact]
        public void DeterministicResults_IdenticalSequencesProduceIdenticalState()
        {
            int[] days = { 10, 11, 12, 13, 14, 15 };
            WeatherKind[] weather = { WeatherKind.FalloutStorm, WeatherKind.Clear, WeatherKind.Clear, WeatherKind.FalloutStorm, WeatherKind.Clear, WeatherKind.Clear };
            var a = LoadRealSystem();
            var b = LoadRealSystem();
            for (int i = 0; i < days.Length; i++)
            {
                a.NotifyWeather(days[i], weather[i]);
                b.NotifyWeather(days[i], weather[i]);
            }
            Assert.Equal(b.CaptureState().shocks.Count, a.CaptureState().shocks.Count);
            Assert.Equal(
                b.GetCurrentPriceMultiplierPermille("foundry", "bandages", "medical"),
                a.GetCurrentPriceMultiplierPermille("foundry", "bandages", "medical"));
        }

        // ── Save / load ───────────────────────────────────────────────────

        [Fact]
        public void SaveLoadRoundTrip_PreservesMultiplierAndDecay_Exactly()
        {
            var system = LoadRealSystem();
            system.NotifyWeather(10, WeatherKind.FalloutStorm);
            system.NotifyWeather(11, WeatherKind.Clear); // mid-decay snapshot
            var saved = system.CaptureState();

            string json = new SystemTextJsonSerializer().Serialize(saved);
            var restored = new SystemTextJsonSerializer().Deserialize<TradeEmbargoState>(json);

            var target = new TradeEmbargoSystem(TradeEmbargoCatalogLoader.ToCatalog(
                TradeEmbargoCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer())));
            target.RestoreState(restored);

            Assert.Equal(
                system.GetCurrentPriceMultiplierPermille("foundry", "bandages", "medical"),
                target.GetCurrentPriceMultiplierPermille("foundry", "bandages", "medical"));
            var origin = saved.shocks.Single();
            var copy = target.ActiveShocks.Single();
            Assert.Equal(origin.ruleId, copy.ruleId);
            Assert.Equal(origin.currentPermille, copy.currentPermille);
            Assert.Equal(origin.remainingDecayDays, copy.remainingDecayDays);
        }

        [Fact]
        public void RestoreState_NullOrLegacy_RestoresNeutral_NewerVersionThrows()
        {
            var system = LoadRealSystem();
            system.RestoreState(null);
            Assert.Empty(system.ActiveShocks);

            var future = new TradeEmbargoState { version = TradeEmbargoState.Version + 1 };
            Assert.Throws<InvalidOperationException>(() => system.RestoreState(future));

            system.RestoreState(new TradeEmbargoState { version = 1 });
            Assert.Empty(system.ActiveShocks);
            Assert.Equal(1000, system.GetCurrentPriceMultiplierPermille("foundry", "bandages", "medical"));
        }

        // ── Validation ────────────────────────────────────────────────────

        [Fact]
        public void MalformedRules_AreRejected_WithNamedErrors()
        {
            var cases = new[]
            {
                MakeRule(id: "Not_Snake_Case"),
                MakeRule(permille: 100),                      // below multiplier floor
                MakeRule(slow: 0),                            // slowdown of 0 — use blocked instead
                MakeRule(decay: -1),                          // negative decay
                MakeRule(regions: Array.Empty<string>()),     // empty region list
                MakeRule(regions: new[] { "not_a_region" }),  // unknown region
                MakeRule(categories: new[] { "not_a_category" }), // unknown category
                MakeRule(categories: Array.Empty<string>()),  // no targets at all
                MakeRule(all: true, categories: new[] { "medical" }), // mixed all + category
                MakeRule(blocked: true, slow: 500)            // blocked + slowdown
            };
            foreach (var rule in cases)
            {
                Assert.True(!TradeEmbargoSystem.IsRuleValid(rule, out var errors), $"rule '{rule.RuleId}' variant should fail");
                Assert.NotEmpty(errors);
                Assert.All(errors, e => Assert.Contains(rule.RuleId, e));
            }
        }

        [Fact]
        public void DuplicateRules_Rejected_Explicitly()
        {
            var system = new TradeEmbargoSystem();
            Assert.True(system.RegisterRule(MakeRule()));
            Assert.False(system.RegisterRule(MakeRule()));                 // same id
            Assert.False(system.RegisterRule(MakeRule(id: "embargo_test_rule_copy"))); // same signature
            Assert.Equal(1, system.RuleCount);
            // A different weather/region signature registers fine.
            Assert.True(system.RegisterRule(MakeRule(id: "embargo_test_rule_two", weather: WeatherKind.Blizzard, regions: new[] { "traplines" })));
            Assert.Equal(2, system.RuleCount);
        }

        // ── Read-model seed ───────────────────────────────────────────────

        [Fact]
        public void EmbargoSummary_ReflectsActiveState_AndNeutralState()
        {
            var system = LoadRealSystem();
            var neutral = system.GetEmbargoSummary(WeatherKind.Clear);
            Assert.False(neutral.embargoActive);
            Assert.Empty(neutral.affectedRegions);

            var storm = system.GetEmbargoSummary(WeatherKind.FalloutStorm);
            Assert.True(storm.embargoActive);
            Assert.Equal(1, storm.activeRuleCount);
            Assert.Contains("medical", storm.affectedCategories);
            Assert.True(storm.anyRouteBlocked);
            Assert.Equal(new[] { "*" }, storm.affectedRegions);
        }

        // ── Data characterization ─────────────────────────────────────────

        [Fact]
        public void RealCatalog_Loads_TenRules_IncludingAllSourceRequiredCases()
        {
            var load = TradeEmbargoCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(load.HasErrors, string.Join("; ", load.Errors));
            Assert.Equal(10, load.Rules.Count);

            var byWeather = load.Rules.Select(r => (r.Weather, r)).ToDictionary(t => t.Item1, t => t.Item2);
            Assert.True(byWeather.ContainsKey(WeatherKind.FalloutStorm));   // all blocked, medical +50%
            Assert.True(byWeather.ContainsKey(WeatherKind.BlackRain));      // flotilla blocked, water +80%
            Assert.True(byWeather.ContainsKey(WeatherKind.EMPStorm));       // foundry blocked, electronics +100%
            Assert.True(byWeather.ContainsKey(WeatherKind.Blizzard));       // traplines blocked, food +40%
            Assert.True(byWeather.ContainsKey(WeatherKind.AcidSnow));       // slowed, not blocked, tools +30%
            Assert.True(byWeather.ContainsKey(WeatherKind.BioFog));         // greenhouse blocked, seeds +60%
            Assert.True(byWeather.ContainsKey(WeatherKind.GlassStorm));     // settlement blocked, materials +40%
            Assert.True(byWeather.ContainsKey(WeatherKind.RadHail));        // all blocked one day, all +25%

            var acid = byWeather[WeatherKind.AcidSnow];
            Assert.False(acid.CaravanBlocked);
            Assert.Equal(500, acid.RouteSlowPermille);
            var rad = byWeather[WeatherKind.RadHail];
            Assert.True(rad.AffectsAllGoods);
            Assert.Equal(1250, rad.PriceMultiplierPermille);
        }

        [Fact]
        public void RealCatalog_LoadIsDeterministic_AndSameWeatherSameMultiplier()
        {
            var a = LoadRealSystem();
            var b = LoadRealSystem();
            foreach (var kind in Enum.GetValues(typeof(WeatherKind)).Cast<WeatherKind>())
            {
                Assert.Equal(
                    a.GetWeatherPriceMultiplierPermille("foundry", kind, "bandages", "medical"),
                    b.GetWeatherPriceMultiplierPermille("foundry", kind, "bandages", "medical"));
            }
        }
    }
}
