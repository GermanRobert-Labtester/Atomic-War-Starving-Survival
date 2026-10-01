// SPDX-License-Identifier: MIT
// User task 6 — the "reasonable player" week-1 bot.
//
// A validation harness in the WorldPlaytest sense: it composes the REAL Core
// authorities (NeedsSystem, Inventory + the real item catalog, KitchenNutrition
//System, GreenhouseSystem, PerimeterDefenseSystem, WeatherSystem) from the
// REAL authored data (starting cohorts, starting supplies, difficulty
// presets, perimeter defenses) and drives them with a deterministic week-1
// policy. It is not a second gameplay authority: production rules stay in the
// Core systems and production ownership stays with Main's day owners.
//
// FIDELITY BOUNDARIES (documented, deliberate):
//   • Host day owners are not composed (auto-rationing, kitchen/greenhouse
//     owners). The bot performs the player actions itself through the same
//     Core seams the host sessions route to (Inventory.Consume's need
//     routing, KitchenNutritionSystem prep/serve, GreenhouseSystem
//     plant/water/harvest with the host's inventory billing, PerimeterDefense
//     construction).
//   • The radiation authority is not composed: irradiated-water contamination
//     is NOT applied when the bot drinks it (counted separately).
//   • The shelter is assumed heated (NeedsSystem isNearHeatSource = true):
//     week-1 death by cold is a heating question, not a food question.
using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Ashfall.Core;
using Ashfall.Core.Defense;
using Ashfall.Core.Difficulty;
using Ashfall.Core.Greenhouse;
using Ashfall.Core.Inventory;
using Ashfall.Core.Random;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        private static readonly int[] ReasonablePlayerSeeds = { 1986, 2026, 9001, 424242 };
        private static readonly string[] ReasonablePlayerPresets =
        {
            "difficulty_sparing", "difficulty_standard", "difficulty_austere", "difficulty_dirge"
        };

        /// <summary>
        /// Runs the reasonable-player week-1 policy across seeds and difficulty
        /// presets and reports whether doing the obvious things (ration, cook,
        /// plant, fortify) keeps the starting crew alive, against the no-action
        /// baseline the seven-day smoke test established.
        /// </summary>
        public static int RunReasonablePlayerSelfTest(string dataDirectory)
        {
            int passed = 0;
            int failed = 0;

            void Check(bool ok, string label, string evidence = "")
            {
                if (ok) passed++;
                else failed++;
                string line = $"[REASONABLE-PLAYER] {(ok ? "PASS" : "FAIL")} {label}" +
                    (string.IsNullOrEmpty(evidence) ? "" : $" ({evidence})");
                if (ok) GD.Print(line);
                else GD.PrintErr(line);
            }

            try
            {
                CatalogLocator.UseInvariantCulture();

                GD.Print("── REASONABLE PLAYER WEEK-1 BOT ──");
                GD.Print($"Seeds: {string.Join(", ", ReasonablePlayerSeeds)}  Presets: {string.Join(", ", ReasonablePlayerPresets)}");

                // ── Gate 1: the no-action baseline reproduces the smoke expectation ──
                // No policy, seed 1986, every preset: by day 7 the crew is dead
                // or at critical hunger and thirst (>= 90) — the same drift the
                // seven-day deterministic smoke test observes. Dead survivors
                // read needs as 0, so a dead baseline satisfies the gate.
                foreach (string preset in ReasonablePlayerPresets)
                {
                    var baseline = ReasonablePlayerBotRun.Create(dataDirectory, 1986, preset,
                        StartingSuppliesCatalog.StandardProfileId, applyPolicy: false);
                    baseline.AdvanceWeek();
                    var snap = baseline.Snapshot();
                    Check(snap.alive_count == 0 || (snap.min_hunger >= 90f && snap.min_thirst >= 90f),
                        $"baseline/{preset}: no-action crew is dead or critically starved by day 7",
                        $"hunger {snap.mean_hunger:0.#} thirst {snap.mean_thirst:0.#} health {snap.mean_health:0.#} alive {snap.alive_count}/{snap.roster_count}");
                }

                // ── The bot matrix: seeds × presets with the week-1 policy ──
                var results = new Dictionary<(int seed, string preset), ReasonablePlayerBotRun>();
                foreach (string preset in ReasonablePlayerPresets)
                {
                    foreach (int seed in ReasonablePlayerSeeds)
                    {
                        var run = ReasonablePlayerBotRun.Create(dataDirectory, seed, preset,
                            StartingSuppliesCatalog.StandardProfileId, applyPolicy: true);
                        run.AdvanceWeek();
                        results[(seed, preset)] = run;
                    }
                }

                GD.Print("\n── WEEK-1 SURVIVAL TABLE (policy: ration, cook, plant, fortify) ──");
                GD.Print("preset            seed    alive   hunger  thirst  health  morale  meals  rations  waters  crops  built");
                foreach (string preset in ReasonablePlayerPresets)
                {
                    var baseRun = ReasonablePlayerBotRun.Create(dataDirectory, 1986, preset,
                        StartingSuppliesCatalog.StandardProfileId, applyPolicy: false);
                    baseRun.AdvanceWeek();
                    var b = baseRun.Snapshot();
                    GD.Print($"{preset,-17} {1986,6}  {b.alive_count}/{b.roster_count}    {b.mean_hunger,6:0.#}  {b.mean_thirst,6:0.#}  {b.mean_health,6:0.#}  {b.mean_morale,6:0.#}   (no-action baseline)");

                    foreach (int seed in ReasonablePlayerSeeds)
                    {
                        var s = results[(seed, preset)].Snapshot();
                        var c = results[(seed, preset)].Counters;
                        GD.Print($"{preset,-17} {seed,6}  {s.alive_count}/{s.roster_count}    {s.mean_hunger,6:0.#}  {s.mean_thirst,6:0.#}  {s.mean_health,6:0.#}  {s.mean_morale,6:0.#}  {c.MealsServed,5}  {c.RationsEaten,7}  {c.WatersDrunk,6}  {c.CropsHarvested,5}  {c.EmplacementsBuilt,5}");
                    }
                }

                // ── Gate 2: the policy actually executes through the real seams ──
                var std = results[(1986, "difficulty_standard")];
                var stdCounters = std.Counters;
                Check(stdCounters.RationsEaten + stdCounters.MealsServed > 0,
                    "policy/standard: the bot actually fed the crew through the consumption seams",
                    $"rations eaten {stdCounters.RationsEaten}, meals served {stdCounters.MealsServed}");
                Check(stdCounters.WatersDrunk > 0,
                    "policy/standard: the bot actually hydrated the crew",
                    $"waters drunk {stdCounters.WatersDrunk}");

                // ── Gate 3: doing the obvious things beats the no-action baseline ──
                // The honest comparison is crew survival first: a dead baseline
                // reads needs as 0, which would make any needs comparison
                // meaningless. The policy wins if it keeps strictly more crew
                // alive, or — when baseline survivors remain — keeps their
                // needs strictly lower.
                foreach (string preset in ReasonablePlayerPresets)
                {
                    var bot = results[(1986, preset)].Snapshot();
                    var baseRun = ReasonablePlayerBotRun.Create(dataDirectory, 1986, preset,
                        StartingSuppliesCatalog.StandardProfileId, applyPolicy: false);
                    baseRun.AdvanceWeek();
                    var baseline = baseRun.Snapshot();
                    Check(bot.alive_count > baseline.alive_count ||
                            (baseline.alive_count > 0 &&
                             bot.mean_hunger < baseline.mean_hunger &&
                             bot.mean_thirst < baseline.mean_thirst),
                        $"policy/{preset}: week-1 policy keeps more crew alive than no action",
                        $"bot alive {bot.alive_count}/{bot.roster_count} vs baseline {baseline.alive_count}/{baseline.roster_count}; " +
                        $"bot hunger {bot.mean_hunger:0.#}, thirst {bot.mean_thirst:0.#} vs baseline {baseline.mean_hunger:0.#}/{baseline.mean_thirst:0.#}");
                    Check(bot.mean_health > baseline.mean_health,
                        $"policy/{preset}: week-1 policy preserves strictly more health than no action",
                        $"bot health {bot.mean_health:0.#} > {baseline.mean_health:0.#}");
                }

                // ── Gate 4: same seed + preset reproduces byte-identical state ──
                string firstHash = results[(1986, "difficulty_standard")].SnapshotHash();
                var repeat = ReasonablePlayerBotRun.Create(dataDirectory, 1986, "difficulty_standard",
                    StartingSuppliesCatalog.StandardProfileId, applyPolicy: true);
                repeat.AdvanceWeek();
                Check(repeat.SnapshotHash() == firstHash,
                    "determinism: the same seed and preset reproduce the identical week-1 state",
                    firstHash[..16]);

                // ── Gate 5: a different seed diverges ──
                Check(results[(2026, "difficulty_standard")].SnapshotHash() != firstHash,
                    "divergence: a different master seed produces a different week-1 state");

                // ── Gate 6: the plant leg works when the profile carries seeds ──
                // Pipeline probe, not a survival claim: with the remnant
                // profile's 8 clean_water the drinking-first reserve never
                // irrigates (plots drought from day 1 and nothing matures), so
                // this run prioritizes the crops (reserve 0) to prove the
                // plant → irrigate → harvest → cook pipeline through the real
                // seams. The survival matrix above keeps drinking-first.
                var grower = ReasonablePlayerBotRun.Create(dataDirectory, 1986, "difficulty_standard",
                    "origin_greenhouse_remnant", applyPolicy: true, waterReserveUnits: 0);
                grower.AdvanceWeek();
                var growerCounters = grower.Counters;
                Check(growerCounters.CropsPlanted > 0 && growerCounters.CropsHarvested > 0,
                    "plant leg: the greenhouse-remnant profile plants and harvests within the week",
                    $"planted {growerCounters.CropsPlanted}, harvested {growerCounters.CropsHarvested}, " +
                    $"cook jobs {growerCounters.CookJobsStarted}, waterings {growerCounters.Waterings}");

                // ── Reported evidence (not gates): fortify reachability ──
                var fortifyEvidence = results[(1986, "difficulty_standard")].Counters;
                GD.Print($"\n[REASONABLE-PLAYER] Fortify leg on standard supplies: {fortifyEvidence.EmplacementsBuilt} emplacement(s) built; last block reason: '{fortifyEvidence.LastFortifyBlockReason ?? "none"}'");

                string details = $"seeds {ReasonablePlayerSeeds.Length} x presets {ReasonablePlayerPresets.Length}; " +
                    $"standard day-7: alive {std.Snapshot().alive_count}/{std.Snapshot().roster_count}, " +
                    $"hunger {std.Snapshot().mean_hunger:0.#}, thirst {std.Snapshot().mean_thirst:0.#}";
                return EmitSummary("reasonable_player_selftest", failed == 0,
                    failed == 0 ? 0 : 1, passed, failed, details);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[REASONABLE-PLAYER] unhandled exception: {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}");
                return EmitSummary("reasonable_player_selftest", false, 1, passed, failed,
                    "unhandled exception");
            }
        }
    }

    /// <summary>
    /// One seeded week-1 run: real Core systems, real authored data, and (when
    /// applyPolicy is true) the deterministic reasonable-player policy.
    /// </summary>
    internal sealed class ReasonablePlayerBotRun
    {
        public const int WeekDays = 7;

        private readonly NeedsSystem _needs;
        private readonly Inventory _inventory;
        private readonly KitchenNutritionSystem _kitchen;
        private readonly GreenhouseSystem _greenhouse;
        private readonly PerimeterDefenseSystem _defense;
        private readonly WeatherSystem _weather;
        private readonly Dictionary<string, ItemDefinition> _defs;
        private readonly List<SurvivorNeedsState> _roster;
        private readonly bool _applyPolicy;
        private readonly string _presetId;
        private readonly int _waterReserveUnits;

        // The kitchen panel's authored recipe roster (KitchenNutritionPanel's
        // catalog, in panel order) — the UI truth a reasonable player sees.
        private static readonly (string RecipeId, Dictionary<string, int> Inputs)[] KitchenRecipes =
        {
            ("recipe_fungal_stew", new Dictionary<string, int> { ["crop_biolum_mushroom"] = 1, ["clean_water"] = 1 }),
            ("recipe_subterranean_mushroom_mash", new Dictionary<string, int> { ["harvested_mushrooms_subterranean"] = 2, ["clean_water"] = 1 }),
            ("recipe_canned_mash", new Dictionary<string, int> { ["military_rations"] = 1, ["fuel"] = 1 }),
            ("recipe_greenhouse_salad", new Dictionary<string, int> { ["crop_leafy_green"] = 2 }),
            ("recipe_cured_jerky_broth", new Dictionary<string, int> { ["cooked_meat"] = 1, ["clean_water"] = 2 }),
            ("recipe_ash_flour_bread", new Dictionary<string, int> { ["item_grain_flour"] = 1 }),
            ("recipe_confit_tuber", new Dictionary<string, int> { ["crop_tuber"] = 3, ["cooking_oil"] = 1, ["item_preservation_salt"] = 1 }),
        };

        private static readonly string[] PlantableSeeds =
        {
            "item_seed_mushroom", "item_seed_tuber", "item_seed_grain",
            "item_seed_hardy_tuber", "item_seed_ash_grain",
        };

        public ReasonablePlayerCounters Counters { get; } = new ReasonablePlayerCounters();

        private ReasonablePlayerBotRun(
            NeedsSystem needs,
            Inventory inventory,
            KitchenNutritionSystem kitchen,
            GreenhouseSystem greenhouse,
            PerimeterDefenseSystem defense,
            WeatherSystem weather,
            Dictionary<string, ItemDefinition> defs,
            List<SurvivorNeedsState> roster,
            string presetId,
            bool applyPolicy,
            int waterReserveUnits)
        {
            _needs = needs;
            _inventory = inventory;
            _kitchen = kitchen;
            _greenhouse = greenhouse;
            _defense = defense;
            _weather = weather;
            _defs = defs;
            _roster = roster;
            _presetId = presetId;
            _applyPolicy = applyPolicy;
            _waterReserveUnits = waterReserveUnits;
        }

        public static ReasonablePlayerBotRun Create(
            string dataDirectory,
            int masterSeed,
            string presetId,
            string suppliesProfileId,
            bool applyPolicy,
            int waterReserveUnits = 6)
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // Real item catalog — consumption uses the authored definitions.
            var allDefs = ItemCatalogLoader.Load(dataDirectory, fileIO, json);
            var defs = new Dictionary<string, ItemDefinition>(StringComparer.Ordinal);
            foreach (var def in allDefs)
                defs[def.id] = def;

            // Real starting roster (cohort_standard_holdfast).
            var cohort = StartingCohortCatalogLoader.Load(dataDirectory, fileIO, json);
            var profile = cohort.DefaultProfile;
            var roster = new List<SurvivorNeedsState>();
            var needs = new NeedsSystem(null, _ => true); // heated shelter (documented)
            foreach (var member in profile.members)
            {
                var state = new SurvivorNeedsState
                {
                    Id = member.id,
                    Health = member.health,
                    Hunger = member.hunger,
                    Thirst = member.thirst,
                    Warmth = member.warmth,
                    Morale = member.morale,
                    IsAlive = true
                };
                roster.Add(state);
                needs.Register(state);
            }

            // Real difficulty scalars bound exactly like Main.Difficulty.cs.
            var difficultyCatalog = DifficultyPresetCatalogLoader.Load(dataDirectory, fileIO);
            var director = new DifficultyDirector(difficultyCatalog);
            var scalars = director.ResolveProvider(presetId);
            needs.HungerRateMultiplier = () => scalars.HungerMult;
            needs.ThirstRateMultiplier = () => scalars.ThirstMult;

            // Real inventory seeded from the authored supplies profile.
            var inventory = new Inventory();
            var supplies = ItemCatalogLoader.LoadStartingSuppliesCatalog(dataDirectory, fileIO, json);
            var supplyProfile = supplies.ResolveOrDefault(suppliesProfileId);
            foreach (var (itemId, amount) in supplyProfile.supplies)
            {
                if (defs.TryGetValue(itemId, out var def))
                    inventory.Add(def, amount);
                else
                    inventory.AddById(itemId, amount);
            }

            // Seeded campaign RNG — the kitchen uses the production fork index.
            var rng = new CampaignRngManager(masterSeed);
            var kitchen = new KitchenNutritionSystem(
                rng.Fork(CampaignStreamIds.Shelter, 0, 12), inventory, needs);

            var greenhouse = new GreenhouseSystem(masterSeed);
            greenhouse.EnsurePlots(2);

            var defenseDefs = PerimeterDefenseCatalogLoader.Load(dataDirectory, fileIO);
            var defense = new PerimeterDefenseSystem(
                defenseDefs, inventory, rng.Fork(CampaignStreamIds.Shelter, 0, 20));

            var weather = new WeatherSystem(new WorldWeatherState());
            weather.BindProfile(BuildBotSeasonProfile(), masterSeed);

            return new ReasonablePlayerBotRun(
                needs, inventory, kitchen, greenhouse, defense, weather,
                defs, roster, presetId, applyPolicy, waterReserveUnits);
        }

        private static SeasonProfileDef BuildBotSeasonProfile()
        {
            // The same embedded profile the seven-day smoke test uses: the
            // harness stays independent of seasonal data churn.
            return new SeasonProfileDef
            {
                id = "reasonable_player_winter",
                displayName = "Reasonable Player Winter",
                weatherCheckIntervalHours = 6f,
                seasons = new List<SeasonWindowDef>
                {
                    new SeasonWindowDef
                    {
                        id = "rp_s1",
                        displayName = "The Long Winter",
                        startDay = 0,
                        clearWeight = 2f,
                        rainWeight = 1f,
                        overcastWeight = 2f,
                        ashfallWeight = 1f,
                        falloutStormWeight = 0.5f,
                        blizzardWeight = 0.5f,
                        blackRainWeight = 0.1f
                    }
                }
            };
        }

        /// <summary>Advances the seven-day week, applying the policy each day.</summary>
        public void AdvanceWeek()
        {
            for (int day = 1; day <= WeekDays; day++)
            {
                if (_applyPolicy)
                {
                    HarvestCrops(day);
                    Ration(day);
                    Cook(day);
                    PlantAndIrrigate(day);
                    Fortify(day);
                }

                // The daily ticks mirror the production day owners.
                _kitchen.TickDay(day);
                _greenhouse.TickDay(day, growLightHours: 6f, ashContaminationRate: 0.04f);
                for (int hour = 0; hour < 24; hour++)
                {
                    _weather.Tick(1f);
                    _needs.Tick(1f);
                }
            }
        }

        // ── Policy legs (deterministic; no RNG in the policy itself) ────────

        private const float RationThreshold = 60f;

        private void Ration(int day)
        {
            foreach (var survivor in _roster)
            {
                if (survivor == null || !survivor.IsAliveState) continue;

                // Thirst first — it drifts faster and kills faster.
                int drinks = 0;
                while (survivor.Thirst >= RationThreshold && drinks < 3)
                {
                    if (TryConsume(survivor, "clean_water"))
                    {
                        Counters.WatersDrunk++;
                    }
                    else if (TryConsume(survivor, "irradiated_water"))
                    {
                        // Contamination is not modeled in this harness
                        // // (radiation authority not composed); counted apart.
                        Counters.WatersDrunk++;
                        Counters.IrradiatedDrinks++;
                    }
                    else
                    {
                        Counters.ThirstUnmetDays++;
                        break;
                    }
                    drinks++;
                }

                int eats = 0;
                while (survivor.Hunger >= RationThreshold && eats < 3)
                {
                    string? pantryRecipe = FirstRecipeWithPortions();
                    if (pantryRecipe != null && _kitchen.ServeMeal(survivor.Id, pantryRecipe).IsSuccess)
                    {
                        Counters.MealsServed++;
                    }
                    else if (TryConsume(survivor, "canned_food"))
                    {
                        Counters.RationsEaten++;
                    }
                    else
                    {
                        Counters.HungerUnmetDays++;
                        break;
                    }
                    eats++;
                }
            }
        }

        private void Cook(int day)
        {
            int living = _roster.Count(s => s != null && s.IsAliveState);
            int pantryPortions = KitchenRecipes.Sum(r => _kitchen.GetAvailablePortions(r.RecipeId));
            if (pantryPortions >= living) return;

            string cookId = _roster.FirstOrDefault(s => s != null && s.IsAliveState)?.Id ?? "cook_shelter";
            foreach (var (recipeId, inputs) in KitchenRecipes)
            {
                var started = _kitchen.StartPrepJob(recipeId, cookId, inputs);
                if (started.IsSuccess)
                {
                    Counters.CookJobsStarted++;
                    return;
                }
                Counters.CookBlockedAttempts++;
            }
        }

        private void PlantAndIrrigate(int day)
        {
            // Week-1 water budget: one bed. A mushroom bed drinks 8 water
            // units a day, so two beds need ~14 clean_water across the week
            // while the crew also drinks — the remnant profile's 8 cannot
            // sustain both (measured: two beds dried out at growth 75 and
            // never matured). A reasonable player concentrates care on one.
            bool bedPlanted = false;
            foreach (string seedId in PlantableSeeds)
            {
                while (!bedPlanted && _inventory.CountById(seedId) > 0)
                {
                    var fallow = _greenhouse.Plots.FirstOrDefault(GreenhouseSystem.IsFallow);
                    if (fallow == null) break;
                    if (!_greenhouse.Plant(fallow.plotIndex, seedId, day, out string consumedSeedId))
                        break;
                    _inventory.Remove(consumedSeedId, 1);
                    Counters.CropsPlanted++;
                    bedPlanted = true;
                }
                if (bedPlanted) break;
            }

            // Irrigate thirsty plots, but never below the run's drinking
            // reserve — a reasonable player rations water to the crew first
            // (the plant-leg pipeline probe uses reserve 0).
            foreach (var plot in _greenhouse.Plots)
            {
                if (plot == null || GreenhouseSystem.IsFallow(plot) || plot.water >= 30f) continue;
                int requiredUnits = (int)Math.Ceiling(25f / 10f);
                if (_inventory.CountById("clean_water") <= _waterReserveUnits + requiredUnits) continue;
                if (!_inventory.HasSufficient("clean_water", requiredUnits)) continue;
                _inventory.Remove("clean_water", requiredUnits);
                _greenhouse.Water(plot.plotIndex, 25f, tainted: false);
                Counters.Waterings++;
            }
        }

        private void HarvestCrops(int day)
        {
            foreach (var plot in _greenhouse.Plots.ToList())
            {
                if (plot == null || plot.stage != (int)GreenhouseStage.Mature) continue;
                var harvest = _greenhouse.Harvest(plot.plotIndex);
                if (harvest.success && !string.IsNullOrEmpty(harvest.yieldItemId))
                {
                    if (_defs.TryGetValue(harvest.yieldItemId, out var def))
                        _inventory.Add(def, harvest.amount);
                    else
                        _inventory.AddById(harvest.yieldItemId, harvest.amount);
                    Counters.CropsHarvested++;
                }
            }
        }

        private void Fortify(int day)
        {
            var built = _defense.Emplacements
                .Select(e => e?.defense_id)
                .Where(id => !string.IsNullOrEmpty(id))
                .ToHashSet(StringComparer.Ordinal);
            foreach (var def in _defense.Definitions
                .OrderBy(d => d.build_costs.Values.Sum())
                .ThenBy(d => d.defense_id, StringComparer.Ordinal))
            {
                if (built.Contains(def.defense_id)) continue;
                var result = _defense.ConstructEmplacement(def.defense_id);
                if (result.IsSuccess)
                {
                    Counters.EmplacementsBuilt++;
                    return;
                }
                Counters.FortifyBlockedAttempts++;
                Counters.LastFortifyBlockReason = result.FailureCode;
            }
        }

        // ── Real consumption seam (mirrors InventoryHostSession routing) ─────

        private bool TryConsume(SurvivorNeedsState survivor, string itemId)
        {
            if (survivor == null || !survivor.IsAliveState) return false;
            if (!_defs.TryGetValue(itemId, out var def)) return false;
            if (!_inventory.HasSufficient(itemId, 1)) return false;

            return _inventory.Consume(
                def,
                (needType, delta) =>
                {
                    switch (needType)
                    {
                        case ItemType.Food:
                            _needs.Modify(survivor, NeedKind.Hunger, delta);
                            break;
                        case ItemType.Water:
                            _needs.Modify(survivor, NeedKind.Thirst, delta);
                            break;
                        case ItemType.Medical:
                            _needs.Modify(survivor, NeedKind.Health, delta);
                            break;
                        case ItemType.Comfort:
                            _needs.Modify(survivor, NeedKind.Morale, delta);
                            break;
                    }
                    return true;
                });
        }

        private string? FirstRecipeWithPortions()
        {
            foreach (var (recipeId, _) in KitchenRecipes)
            {
                if (_kitchen.GetAvailablePortions(recipeId) > 0)
                    return recipeId;
            }
            return null;
        }

        // ── Snapshot + hashing ───────────────────────────────────────────────

        public ReasonablePlayerSnapshot Snapshot()
        {
            var living = _roster.Where(s => s != null && s.IsAliveState).ToList();
            float Mean(Func<SurvivorNeedsState, float> pick) =>
                living.Count == 0 ? 0f : living.Average(pick);
            return new ReasonablePlayerSnapshot
            {
                preset_id = _presetId,
                roster_count = _roster.Count,
                alive_count = living.Count,
                mean_hunger = Mean(s => s.Hunger),
                min_hunger = living.Count == 0 ? 0f : living.Min(s => s.Hunger),
                mean_thirst = Mean(s => s.Thirst),
                min_thirst = living.Count == 0 ? 0f : living.Min(s => s.Thirst),
                mean_health = Mean(s => s.Health),
                mean_morale = Mean(s => s.Morale),
                canned_food = _inventory.CountById("canned_food"),
                clean_water = _inventory.CountById("clean_water"),
                final_weather = _weather.Current.ToString(),
            };
        }

        public string SnapshotHash()
        {
            string json = new SystemTextJsonSerializer().Serialize(Snapshot());
            using var sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(json));
            var sb = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
                sb.Append(b.ToString("x2", CultureInfo.InvariantCulture));
            return sb.ToString();
        }
    }

    public sealed class ReasonablePlayerCounters
    {
        public int RationsEaten;
        public int MealsServed;
        public int WatersDrunk;
        public int IrradiatedDrinks;
        public int HungerUnmetDays;
        public int ThirstUnmetDays;
        public int CookJobsStarted;
        public int CookBlockedAttempts;
        public int CropsPlanted;
        public int CropsHarvested;
        public int Waterings;
        public int EmplacementsBuilt;
        public int FortifyBlockedAttempts;
        public string? LastFortifyBlockReason;
    }

    public sealed class ReasonablePlayerSnapshot
    {
        public string preset_id = string.Empty;
        public int roster_count;
        public int alive_count;
        public float mean_hunger;
        public float min_hunger;
        public float mean_thirst;
        public float min_thirst;
        public float mean_health;
        public float mean_morale;
        public int canned_food;
        public int clean_water;
        public string final_weather = string.Empty;
    }
}
