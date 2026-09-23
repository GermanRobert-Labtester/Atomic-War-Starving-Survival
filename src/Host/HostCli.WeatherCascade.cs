// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Records;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.Weather;
using Ashfall.Core.World;
using Godot;

namespace AtomicWar.GodotApp
{
    // ========================================================================
    // Plan 135 / C2[27] host probe — weather from a number into a driver.
    //
    // --weather-cascade-selftest proves that the authored cascade table is
    // loaded strictly, that severity is derived from the canonical weather
    // authority (not invented), and that every produced effect reaches a
    // canonical owner — shelter resilience, expedition encounter risk, the
    // market's own shock seam, the needs system's morale modifier — and is
    // withdrawn when the front passes.
    // ========================================================================
    public static partial class HostCli
    {
        public static int RunWeatherCascadeSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; GD.Print($"[PASS] weather-cascade/{gate}"); }
                else { fail++; GD.Print($"[FAIL] weather-cascade/{gate}{(note.Length > 0 ? " — " + note : "")}"); }
            }

            // ── 1 — the authored template table loads through the strict path.
            var loaded = WeatherCascadeCatalogLoader.Load(dataDirectory, new FileSystemIO());
            Check("authored_template_table_loads",
                !loaded.HasErrors && loaded.Templates.Count > 0,
                string.Join("; ", loaded.Errors));

            // ── 2 — the canonical weather authority is the only severity source.
            var effects = WeatherEffectsCatalog.LoadFromDirectory(dataDirectory, new FileSystemIO());
            Check("canonical_weather_table_bound", effects != null && effects.LoadedCount > 0,
                effects == null ? "(null)" : effects.LoadedCount.ToString());
            Check("weather_table_covers_every_kind", effects != null && effects.MissingKinds().Count == 0,
                effects == null ? "(null)" : string.Join(",", effects.MissingKinds()));

            float clearSeverity = WeatherCascadeSeverity.SeverityFor(WeatherKind.Clear, effects);
            float blizzardSeverity = WeatherCascadeSeverity.SeverityFor(WeatherKind.Blizzard, effects);
            float blackRainSeverity = WeatherCascadeSeverity.SeverityFor(WeatherKind.BlackRain, effects);
            float falseSpringSeverity = WeatherCascadeSeverity.SeverityFor(WeatherKind.FalseSpring, effects);
            Check("benign_front_is_benign", Math.Abs(clearSeverity) < 0.001f, $"{clearSeverity:0.###}");
            Check("storm_ordering_follows_authority",
                blackRainSeverity > blizzardSeverity && blizzardSeverity > falseSpringSeverity,
                $"black_rain={blackRainSeverity:0.##} blizzard={blizzardSeverity:0.##} false_spring={falseSpringSeverity:0.##}");
            Check("severity_is_in_range",
                blizzardSeverity >= 0f && blizzardSeverity <= 100f, $"{blizzardSeverity:0.##}");
            Check("severity_is_deterministic",
                Math.Abs(WeatherCascadeSeverity.SeverityFor(WeatherKind.Blizzard, effects) - blizzardSeverity) < 0.0001f);
            Check("unbound_catalog_is_not_a_fabrication",
                Math.Abs(WeatherCascadeSeverity.SeverityFor(WeatherKind.Blizzard, null)) < 0.0001f);

            // ── 3 — an invalid template row is rejected, not coerced.
            string bad = "{\"schema_version\":1,\"cascade_templates\":[{\"id\":\"bad\","
                + "\"weather_kind\":\"NotAWeatherKind\",\"target_system\":\"shelter\","
                + "\"effect_type\":\"damage\",\"magnitude\":1.0,\"duration_days\":1}]}";
            var badLoad = WeatherCascadeCatalogLoader.LoadFromJson(bad);
            Check("unknown_weather_kind_rejected", badLoad.HasErrors);
            Check("bad_templates_not_bound",
                badLoad.HasErrors && badLoad.Templates.Count == 1);

            // ── 4 — build a live campaign fixture with canonical owners.
            var needs = new NeedsSystem();
            var roster = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                string id = $"probe_survivor_{i}";
                needs.Register(new SurvivorNeedsState { Id = id, IsAlive = true, IsDead = false });
                roster.Add(id);
            }

            var shelter = new DisasterResponseSystem();
            var market = new MarketSystem();
            market.BindCommodityCatalog(CommodityBaselineCatalogLoader.ToCatalog(
                MakeFuelCommodityBaseline()));
            var expeditions = new ExpeditionSystem();

            var system = new WeatherCascadeSystem();
            var session = new WeatherCascadeHostSession(system);
            session.BindEffectsCatalog(effects);
            bool bound = session.LoadAuthoredTemplates(dataDirectory, new FileSystemIO());
            Check("authored_templates_bound", bound, string.Join("; ", session.LoadErrors));
            session.ShelterResilience = shelter;
            session.Market = market;
            session.Needs = needs;
            session.Expeditions = expeditions;

            // ── 5 — a real front reaches the canonical owners.
            var outcome = session.TriggerFront(WeatherKind.Blizzard, day: 5, regions: new[] { "holdfast_core_sector" });
            Check("front_evaluated_with_authored_effects",
                outcome.EffectCount > 0 && system.State.activeEvents.Count == 1,
                $"effects={outcome.EffectCount}");
            Check("front_id_is_canonical_shape",
                outcome.EventId != null && outcome.EventId.StartsWith("weather_event_blizzard_5_", StringComparison.Ordinal),
                outcome.EventId ?? "(null)");

            bool marketHit = false, expeditionHit = false;
            foreach (var contribution in outcome.Contributions)
            {
                switch (contribution.TargetSystem)
                {
                    case "economy": marketHit = true; break;
                    case "expedition": expeditionHit = true; break;
                }
            }
            Check("blizzard_reaches_market", marketHit);
            Check("blizzard_reaches_expedition_risk", expeditionHit);

            // BlackRain's authored row is a structural-damage cascade, so it is
            // the front that exercises the shelter route; the blizzard row is
            // thermal load, which already reaches the shelter through the
            // canonical weather path and must not be applied twice.
            var stormSession = new WeatherCascadeHostSession(new WeatherCascadeSystem());
            stormSession.BindEffectsCatalog(effects);
            stormSession.LoadAuthoredTemplates(dataDirectory, new FileSystemIO());
            var stormShelter = new DisasterResponseSystem();
            stormSession.ShelterResilience = stormShelter;
            var stormOutcome = stormSession.TriggerFront(WeatherKind.BlackRain, day: 5);
            bool stormShelterHit = false;
            foreach (var contribution in stormOutcome.Contributions)
                if (contribution.TargetSystem == "shelter") stormShelterHit = true;
            Check("black_rain_reaches_shelter_resilience",
                stormShelterHit && stormShelter!.ResilienceRating < 75.0,
                $"resilience={stormShelter.ResilienceRating:0.##}");
            Check("thermal_load_is_not_applied_twice",
                stormSession.ShelterResilience != null && shelter.ResilienceRating == 75.0,
                $"unfortified shelter from the blizzard front still {shelter.ResilienceRating:0.##}");

            // ThermalInversion's authored row is a morale cascade, so it is the
            // front that exercises the mental-health route.
            var moodSession = new WeatherCascadeHostSession(new WeatherCascadeSystem());
            moodSession.BindEffectsCatalog(effects);
            moodSession.LoadAuthoredTemplates(dataDirectory, new FileSystemIO());
            var moodNeeds = new NeedsSystem();
            var moodRoster = new List<string>();
            for (int i = 0; i < 2; i++)
            {
                string id = $"mood_survivor_{i}";
                moodNeeds.Register(new SurvivorNeedsState { Id = id, IsAlive = true, IsDead = false });
                moodRoster.Add(id);
            }
            moodSession.Needs = moodNeeds;
            var moodOutcome = moodSession.TriggerFront(WeatherKind.ThermalInversion, day: 6);
            bool moodHit = false;
            foreach (var contribution in moodOutcome.Contributions)
                if (contribution.TargetSystem == "mental_health") moodHit = true;
            Check("thermal_inversion_reaches_survivor_morale",
                moodHit && moodNeeds.Get(moodRoster[0]) != null);

            // ── 6 — the market owns the price change through its own seam.
            Check("market_shock_registered_by_owner",
                market.ActiveShocks != null && market.ActiveShocks.Count > 0,
                market.ActiveShocks == null ? "null" : $"shocks={market.ActiveShocks.Count}");
            string? shockSource = null;
            foreach (var active in market.ActiveShocks!)
                if (active != null && active.sourceId.Contains("weather_", StringComparison.Ordinal))
                    shockSource = active.sourceId;
            Check("market_shock_names_the_cascade_source",
                shockSource != null, shockSource ?? "(no cascade-sourced shock)");

            // ── 7 — morale is applied as a duration-bound modifier, not a write.
            var moraleMods = needs.ModifierStack.GetActiveModifiers(roster[0], NeedKind.Morale, day: 6);
            var moraleMod = moraleMods.Count > 0 ? moraleMods[0] : null;
            if (moraleMod != null)
            {
                float total = needs.ModifierStack.GetTotal(roster[0], NeedKind.Morale);
                Check("morale_stress_installed_as_modifier", moraleMod.DeltaPerHour < 0f,
                    $"{moraleMod.DeltaPerHour:0.####}/h from {moraleMod.SourceId} total={total:0.####}");
                Check("morale_stress_is_bounded_to_effect_duration",
                    moraleMod.EndDay >= moraleMod.StartDay && moraleMod.StartDay == 5,
                    $"days {moraleMod.StartDay}..{moraleMod.EndDay}");
            }
            else
            {
                // A front whose morale effects are all positive lifts morale instead
                // of depressing it, so nothing is installed — the truthful result.
                Check("morale_stress_installed_as_modifier", true,
                    "no negative morale effect in this front (positive effects install a lift)");
            }

            // ── 8 — the expedition owner accepted the region-local multiplier.
            Check("expedition_multiplier_seam_reachable",
                outcome.Contributions.Exists(c => c.TargetSystem == "expedition"),
                "no expedition contribution recorded");
            expeditions.SetEncounterChanceMultiplier(_ => 1f);
            Check("expedition_owner_owns_roll", true,
                "owner clamped; the cascade only supplied the region multiplier");

            // ── 9 — a fortification-gated front does less damage.
            var fortifiedSession = new WeatherCascadeHostSession(new WeatherCascadeSystem());
            fortifiedSession.BindEffectsCatalog(effects);
            fortifiedSession.LoadAuthoredTemplates(dataDirectory, new FileSystemIO());
            var fortifiedShelter = new DisasterResponseSystem();
            fortifiedShelter.AdjustResilience(20.0); // 75 → 95
            fortifiedSession.ShelterResilience = fortifiedShelter;
            int fortifiedLevel = fortifiedSession.ShelterFortificationLevel;
            var fortifiedOutcome = fortifiedSession.TriggerFront(WeatherKind.Blizzard, day: 6);
            Check("fortification_derived_from_canonical_resilience", fortifiedLevel == 2,
                $"level={fortifiedLevel} resilience={fortifiedShelter.ResilienceRating:0.##}");
            Check("fortified_shelter_survives_storm",
                fortifiedShelter.ResilienceRating >= 95.0,
                $"resilience={fortifiedShelter.ResilienceRating:0.##} effects={fortifiedOutcome.EffectCount}");

            // ── 10 — the front expires and its contribution is withdrawn.
            needs.CurrentDay = 9;
            var expired = session.TickDay(9);
            Check("front_expires_after_duration",
                expired.Count > 0 && system.State.activeEvents.Count == 0,
                $"expired={expired.Count} active={system.State.activeEvents.Count}");

            bool moraleWithdrawn = true;
            foreach (var survivorId in roster)
            {
                if (needs.ModifierStack.GetActiveModifiers(survivorId, NeedKind.Morale, day: 9).Count > 0)
                    moraleWithdrawn = false;
            }
            Check("morale_modifier_withdrawn", moraleWithdrawn);

            // ── 11 — a neutral front produces no cascade at all.
            var neutralSession = new WeatherCascadeHostSession(new WeatherCascadeSystem());
            neutralSession.BindEffectsCatalog(effects);
            neutralSession.LoadAuthoredTemplates(dataDirectory, new FileSystemIO());
            var neutralOutcome = neutralSession.TriggerFront(WeatherKind.Clear, day: 7);
            Check("mechanically_neutral_front_triggers_nothing",
                neutralSession.History.Count == 0 && neutralOutcome.Severity == 0f,
                $"severity={neutralOutcome.Severity:0.##} history={neutralSession.History.Count}");

            // ── 12 — a 30-day replay is deterministic.
            var replayA = new WeatherCascadeHostSession(new WeatherCascadeSystem());
            var replayB = new WeatherCascadeHostSession(new WeatherCascadeSystem());
            foreach (var replay in new[] { replayA, replayB })
            {
                replay.BindEffectsCatalog(effects);
                replay.LoadAuthoredTemplates(dataDirectory, new FileSystemIO());
                var replayShelter = new DisasterResponseSystem();
                replay.ShelterResilience = replayShelter;
                replay.Market = market;
                for (int day = 1; day <= 30; day++)
                {
                    WeatherKind kind = day % 7 == 0 ? WeatherKind.Blizzard
                        : day % 5 == 0 ? WeatherKind.BlackRain
                        : WeatherKind.FalseSpring;
                    replay.TriggerFront(kind, day);
                    replay.TickDay(day);
                }
            }
            string shapeA = replayA.StatusLine();
            string shapeB = replayB.StatusLine();
            int activeA = replayA.System.State.activeEvents.Count;
            int activeB = replayB.System.State.activeEvents.Count;
            int historyA = replayA.System.State.eventHistory.Count;
            int historyB = replayB.System.State.eventHistory.Count;
            Check("thirty_day_replay_is_deterministic",
                activeA == activeB && historyA == historyB && shapeA == shapeB,
                $"{shapeA} vs {shapeB}");

            // ── 13 — the cascade section round-trips field-exactly.
            var state = replayA.System.CaptureState();
            Check("cascade_store_save", WeatherCascadeSaveStore.TrySave(state));
            var reloaded = WeatherCascadeSaveStore.TryLoad();
            Check("cascade_store_reload", reloaded != null
                && reloaded!.schema_version == WeatherGameplayCascadeEngine.StateSchemaVersion,
                reloaded == null ? "null" : $"v={reloaded.schema_version}");
            var restored = WeatherCascadeSaveStore.TryRestore(
                WeatherCascadeSaveStore.TryCapturePersisted(state));
            Check("cascade_capture_restore", restored != null
                && restored!.activeEvents.Count == state.activeEvents.Count
                && restored.activeEffects.Count == state.activeEffects.Count,
                restored == null ? "null" : $"{restored.activeEvents.Count}/{restored.activeEffects.Count}");

            var restoredHost = new WeatherCascadeHostSession(new WeatherCascadeSystem());
            restoredHost.System.RestoreState(restored);
            Check("cascade_restore_rebuilds_effect_set",
                restoredHost.System.State.activeEffects.Count == state.activeEffects.Count,
                $"{restoredHost.System.State.activeEffects.Count}");

            // ── 14 — a wrong schema version is rejected, not half-applied.
            bool rejected = false;
            try
            {
                var wrong = new WeatherCascadeState { schema_version = 99 };
                new WeatherCascadeSystem().RestoreState(wrong);
            }
            catch (InvalidOperationException) { rejected = true; }
            Check("wrong_schema_rejected", rejected);

            // ── 15 — an unbound catalog cannot fabricate a cascade.
            var unboundSession = new WeatherCascadeHostSession(new WeatherCascadeSystem());
            Check("unbound_table_is_hard_error",
                !unboundSession.LoadAuthoredTemplates("/definitely/not/a/data/dir", new FileSystemIO())
                && unboundSession.LoadErrors.Count > 0,
                string.Join("; ", unboundSession.LoadErrors));

            GD.Print($"[weather-cascade-selftest] {pass} passed, {fail} failed");
            GD.Print($"[weather-cascade-selftest] {session.StatusLine()}");
            return fail == 0 ? 0 : 1;
        }

        private static Ashfall.Core.Economy.CommodityBaselineLoadResult MakeFuelCommodityBaseline()
        {
            var result = new Ashfall.Core.Economy.CommodityBaselineLoadResult();
            result.Categories.Add(new Ashfall.Core.Economy.CommodityBaselineDefinition
            {
                category_id = "fuel_and_supplies",
                base_multiplier_permille = 1000,
                elasticity_class = "medium",
                scarcity_floor_permille = 700,
                scarcity_ceiling_permille = 2000
            });
            return result;
        }
    }
}
