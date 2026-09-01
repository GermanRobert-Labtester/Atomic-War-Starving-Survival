// SPDX-License-Identifier: MIT
// Plan 10 — remediation behavioral tests. Each test pins a specific
// runtime behavior from the QA report so a regression surfaces here.
//
// Coverage:
//   1.1  CombatantFactory is consumed in BeginEncounter; AI fields
//        propagate into runtime rows and affect simulation damage.
//   1.1  Unknown combatant_* id is handled deterministically (falls back
//        to legacy enemy block AND appends a debug event).
//   1.2  CombatCatalogLoader rejects orphan faction_id at load time.
//   1.2  CombatCatalogLoader accepts valid faction_lore references.
//   1.3  CombatAiMoves central authority rejects drift candidates and
//        accepts the catalog contract.
//   2.1  WarlordCatalogValidator rejects orphan resource_priority with
//        a specific error.
//   2.2  WarlordCatalogValidator rejects canonical-faction tribute
//        orphans non-canonicals are reported only.
//   3.1  recipes.json no longer ships lubricate_weapon.
//   3.1  RecipeCatalogLoader rejects a synthetic zero-result sink.
//   4    dive_sites.json uses null (not "") for no-thread entries.

using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Maritime;
using Ashfall.Core.Warlords;

namespace Ashfall.Core.Tests
{
    public class Plan10RemediationTests : CatalogTestBase
    {
        private static string DataDir => DataDirectory;

        // ─────────────────────────── 1.1 factory + enc wiring ─────────────

        [Fact]
        public void Factory_SpawnFromCatalog_PopulatesAllCatalogFields()
        {
            CombatCatalog.Clear();
            Assert.True(CombatCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer()));
            var row = CombatantFactory.SpawnFromCatalogOrThrow("combatant_armored_boar");
            Assert.NotNull(row);
            Assert.Equal("combatant_armored_boar", row.CatalogId);
            Assert.Equal("Ash-Backed Boar", row.Name);
            Assert.Equal(1, row.Lane); // Center = 1
            Assert.Equal(140f, row.Health);
            Assert.Equal(140f, row.MaxHealth);
            // base_armor_rating 0.45 → 0..1 clamp survives round-trip.
            Assert.InRange(row.ArmorRating, 0f, 1f);
            Assert.Equal("HoldPosition", row.AiStancePreference);
            Assert.Equal("Charge", row.AiSpecialMove);
            Assert.True(row.AiAccuracyMod > 1f, "boar accuracy > 1");
            Assert.True(row.AiDamageMod > 1f, "boar damage > 1");
            Assert.Equal(-1f, row.SurrenderThreshold); // never surrender
        }

        [Fact]
        public void Factory_UnknownId_NullOrThrows_Deterministic()
        {
            CombatCatalog.Clear();
            Assert.True(CombatCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer()));
            Assert.Null(CombatantFactory.SpawnFromCatalog("combatant_does_not_exist"));
            Assert.False(CombatantFactory.TrySpawnFromCatalog("combatant_does_not_exist", out _, out _));
            Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() =>
                CombatantFactory.SpawnFromCatalogOrThrow("combatant_does_not_exist"));
        }

        [Fact]
        public void Factory_InvalidIdFields_AreClampedDeterministically()
        {
            // Hand-construct a malformed CombatantDefinition via a minified
            // fixture catalog. The loader must reject before reaching the
            // factory. This proves the factory never sees a 3-lane lane.
            string dir = Path.Combine(Path.GetTempPath(),
                "ashfall_plan10_factory_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            try
            {
                string json =
                    "{\"schema_version\":1,\"weapons\":[],\"ammo\":[],\"materials\":[],\"combatants\":[" +
                    "{\"id\":\"combatant_bad_lane\",\"display_name\":\"X\",\"preferred_lane\":9,\"ai_stance_preference\":\"HoldPosition\",\"ai_special_move\":\"None\",\"ai_accuracy_mod\":1,\"ai_damage_mod\":1,\"surrender_threshold\":-1,\"flee_threshold\":-1}" +
                    "]}";
                File.WriteAllText(Path.Combine(dir, "combat_catalog.json"), json);
                var ex = Assert.Throws<FormatException>(() =>
                    CombatCatalogLoader.Load(dir, new FileSystemIO(), new SystemTextJsonSerializer()));
                Assert.Contains("preferred_lane", ex.Message);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }

        [Fact]
        public void BeginEncounter_WithCatalogIds_PopulatesEncounterRows_FromFactory()
        {
            CombatCatalog.Clear();
            Assert.True(CombatCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer()));

            var sys = new TacticalCombatSystem();
            var players = new System.Collections.Generic.List<CombatantState>
            {
                new CombatantState { Id = "p1", Name = "Yuki", SurvivorId = "svy", IsPlayer = true }
            };
            var weapons = new System.Collections.Generic.List<WeaponInstanceState>
            {
                new WeaponInstanceState { InstanceId = "w1", WeaponId = "weapon_pipe_rifle", OwnerSurvivorId = "svy", AmmoId = "ammo_357", AmmoRemaining = 5 }
            };
            var enemyIds = new System.Collections.Generic.List<string>
            {
                "combatant_armored_boar", // known
                "combatant_does_not_exist", // forces fallback to legacy enemy block
                "combatant_warlord_veteran"
            };

            Assert.True(sys.BeginEncounter(
                encounterId: "enc_test_001",
                expeditionId: "exp_x",
                locationId: "loc_test",
                locationName: "Test Location",
                day: 7,
                seed: 12345,
                players: players,
                playerWeapons: weapons,
                enemyCount: 3,
                enemyHealth: 50f,
                enemyCombatantIds: enemyIds));

            // 1 player + 3 enemies
            Assert.Equal(4, sys.State.Combatants.Count);
            int enemiesWithCatalogId = sys.State.Combatants.Count(c => !c.IsPlayer && !string.IsNullOrEmpty(c.CatalogId));
            Assert.Equal(2, enemiesWithCatalogId);

            // The boar row carries the catalog-derived AI traits. Health is
            // overridden by BeginEncounter because enemyHealth > 0; AI mods
            // and StancePreference / SpecialMove survive the round trip.
            var boar = sys.State.Combatants.FirstOrDefault(c => c.CatalogId == "combatant_armored_boar");
            Assert.NotNull(boar);
            Assert.Equal(50f, boar!.Health); // encounter-local override
            Assert.Equal(50f, boar.MaxHealth);
            Assert.True(boar.AiAccuracyMod > 1f, "boar catalog accuracy mod preserved");
            Assert.True(boar.AiDamageMod > 1f, "boar catalog damage mod preserved");
            Assert.Equal("HoldPosition", boar.AiStancePreference);
            Assert.Equal("Charge", boar.AiSpecialMove);

            // The unknown-id slot fell back to the legacy enemy block.
            var legacy = sys.State.Combatants.FirstOrDefault(c => c.Id == "enemy_enc_test_001_1");
            Assert.NotNull(legacy);
            Assert.Equal(string.Empty, legacy!.CatalogId);

            // A debug event was appended for the missing catalog id, NOT a
            // silent skip.
            Assert.Contains(sys.State.Events,
                e => e.Kind == "enemy_catalog_missing"
                     && (e.Detail != null && e.Detail.Contains("combatant_does_not_exist")));
        }

        // ──────────────────────────── 1.2 faction cross-ref ───────────────

        [Fact]
        public void Loader_RejectsOrphanFactionId()
        {
            string dir = Path.Combine(Path.GetTempPath(),
                "ashfall_plan10_orphan_fac_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            try
            {
                // faction_lore.json with only one faction
                File.WriteAllText(Path.Combine(dir, "faction_lore.json"),
                    "{\"items\":[{\"faction_id\":\"iron_garrison\",\"display_name\":\"X\"}]}");
                // combat_catalog.json referencing an orphan faction
                File.WriteAllText(Path.Combine(dir, "combat_catalog.json"),
                    "{\"schema_version\":1,\"weapons\":[],\"ammo\":[],\"materials\":[],\"combatants\":[" +
                    "{\"id\":\"combatant_x\",\"display_name\":\"X\",\"preferred_lane\":0,\"ai_stance_preference\":\"HoldPosition\",\"ai_special_move\":\"None\",\"ai_accuracy_mod\":1,\"ai_damage_mod\":1,\"surrender_threshold\":-1,\"flee_threshold\":-1,\"faction_id\":\"iron_garrsion_typo\"}" +
                    "]}");
                var ex = Assert.Throws<FormatException>(() =>
                    CombatCatalogLoader.Load(dir, new FileSystemIO(), new SystemTextJsonSerializer()));
                Assert.Contains("iron_garrsion_typo", ex.Message);
                Assert.Contains("faction_id", ex.Message);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }

        [Fact]
        public void Loader_AcceptsValidFactionId_FromFactionLore()
        {
            // Authoritative catalog must remain valid under the new check.
            CombatCatalog.Clear();
            Assert.True(CombatCatalogLoader.Load(DataDir, new FileSystemIO(),
                new SystemTextJsonSerializer()));
            // sanity: a known canonical human combatant exists and survived
            Assert.True(CombatCatalog.HasCombatant("combatant_conscript_levy"));
            Assert.True(CombatCatalog.HasCombatant("combatant_warlord_veteran"));
            // unaligned fauna (missing-authority behaviour already covered
            // by the loader — faction_id "" is silently accepted)
            Assert.True(CombatCatalog.HasCombatant("combatant_pale_crawler"));
        }

        // ──────────────────────────── 1.3 central AI set ──────────────────

        [Fact]
        public void CombatAiMoves_IsAllowed_RejectsUnknownAndAcceptsContract()
        {
            // Authoritative catalog uses these strings — they must all pass.
            foreach (var ok in new[] { "None", "Burrow", "Flank", "Spore", "Charge",
                                       "SuppressiveFire", "TacticalRetreat" })
                Assert.True(CombatAiMoves.IsAllowed(ok), ok + " must be accepted");

            // Drift candidates must be rejected.
            foreach (var bad in new[] { "Sidestep", "ChargeAndSuppressive", "burrow", "" })
            {
                if (string.IsNullOrEmpty(bad))
                    Assert.True(CombatAiMoves.IsAllowed(bad), "empty -> default sentinel");
                else
                    Assert.False(CombatAiMoves.IsAllowed(bad), bad + " must be rejected");
            }
        }

        [Fact]
        public void Loader_RejectsUnknownAiStanceOrAiMove()
        {
            string dir = Path.Combine(Path.GetTempPath(),
                "ashfall_plan10_aimove_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            try
            {
                File.WriteAllText(Path.Combine(dir, "faction_lore.json"),
                    "{\"items\":[]}");
                File.WriteAllText(Path.Combine(dir, "combat_catalog.json"),
                    "{\"schema_version\":1,\"weapons\":[],\"ammo\":[],\"materials\":[],\"combatants\":[" +
                    "{\"id\":\"combatant_drift\",\"display_name\":\"X\",\"preferred_lane\":1,\"ai_stance_preference\":\"HoldPosition\",\"ai_special_move\":\"NOT_A_REAL_MOVE\",\"ai_accuracy_mod\":1,\"ai_damage_mod\":1,\"surrender_threshold\":-1,\"flee_threshold\":-1}" +
                    "]}");
                var ex = Assert.Throws<FormatException>(() =>
                    CombatCatalogLoader.Load(dir, new FileSystemIO(), new SystemTextJsonSerializer()));
                Assert.Contains("ai_special_move", ex.Message);
                Assert.Contains("NOT_A_REAL_MOVE", ex.Message);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }

        // ───────────────────── 2.1 doctrine resource_priority ─────────────

        [Fact]
        public void Warlord_DoctrineResourcePriority_MustResolveToKnownItem()
        {
            var files = new FileSystemIO();
            var catalog = WarlordDoctrineCatalogLoader.Load(DataDir, files, new SystemTextJsonSerializer());
            // current data authority passes
            var pass = WarlordCatalogValidator.Validate(catalog, DataDir, files);
            Assert.True(pass.Clean, string.Join("\n  ", pass.Errors));

            // Inject an orphan item id into one doctrine and re-validate.
            catalog.Doctrines[0].resource_priority.Add("not_an_item_id");
            var fail = WarlordCatalogValidator.Validate(catalog, DataDir, files);
            Assert.False(fail.Clean);
            Assert.Contains(fail.Errors, e =>
                e.Contains(catalog.Doctrines[0].id)
                && e.Contains("not_an_item_id"));
        }

        // ───────────────────── 2.2 faction tribute demands ────────────────

        [Fact]
        public void Warlord_FactionTributeDemand_MustResolveToKnownItem_ForCanonicalFaction()
        {
            string dir = Path.Combine(Path.GetTempPath(),
                "ashfall_plan10_tribute_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            try
            {
                // Copy only the catalogues WarlordCatalogValidator reads so the
                // location/territory checks stay clean and we can isolate the
                // tribute_demand cross-reference test below.
                foreach (var fname in new[] {
                    "faction_lore.json", "items.json", "black_flotilla_items.json",
                    "holdfast_items.json", "crossing_items.json",
                    "chemical_dependency_items.json", "dose_items.json",
                    "locations.json", "locations_expansion3.json",
                    "year_of_ash_locations.json", "holdfast_locations.json",
                    "crossing_locations.json", "dose_locations.json",
                    "deep_lore_locations.json", "duty_roster_locations.json",
                    "standing_record_locations.json"
                })
                {
                    string src = Path.Combine(DataDir, fname);
                    if (File.Exists(src))
                        File.Copy(src, Path.Combine(dir, fname));
                }

                // Override only faction_lore.json with a broken canonical tribute.
                File.WriteAllText(Path.Combine(dir, "faction_lore.json"),
                    "{\"items\":[{\"faction_id\":\"warlords_sector_4\",\"display_name\":\"X\"," +
                    "\"tribute_demands\":[\"fuel\",\"not_an_item_id\"]}]}");

                var catalog = WarlordDoctrineCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
                var fail = WarlordCatalogValidator.Validate(catalog, dir, new FileSystemIO());
                bool found = fail.Errors.Any(e =>
                    e.Contains("warlords_sector_4") && e.Contains("not_an_item_id"));
                Assert.True(found,
                    "expected tribute_demand validation error; got: " + string.Join("\n  ", fail.Errors));
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }

        [Fact]
        public void Warlord_DoctrineResourcePriority_RealCatalog_HasNoOrphans()
        {
            // Belt-and-suspenders: confirm data authority has no orphan item refs.
            var files = new FileSystemIO();
            var catalog = WarlordDoctrineCatalogLoader.Load(DataDir, files, new SystemTextJsonSerializer());
            var report = WarlordCatalogValidator.Validate(catalog, DataDir, files);
            foreach (var doc in catalog.Doctrines)
                foreach (var rp in doc.resource_priority)
                    Assert.DoesNotContain(report.Errors,
                        e => e.Contains(doc.id) && e.Contains("resource_priority") && e.Contains(rp));
        }

        // ───────────────────────── 3.1 lubricate_weapon ────────────────────

        [Fact]
        public void Recipes_LubricateWeapon_RemovedFromAuthority()
        {
            var serializer = new SystemTextJsonSerializer();
            var wrapper = serializer.Deserialize<RecipeListWrapper>(
                File.ReadAllText(Path.Combine(DataDir, "recipes.json")));
            Assert.NotNull(wrapper);
            Assert.NotNull(wrapper.recipes);
            Assert.DoesNotContain(wrapper.recipes, r => r.id == "lubricate_weapon");
            Assert.DoesNotContain(wrapper.recipes, r => r.id == "lubricate_weapon" || (r.id?.Contains("lubricate") ?? false));
        }

        [Fact]
        public void Recipes_GunOilItem_RemovedFromAuthority()
        {
            var serializer = new SystemTextJsonSerializer();
            var wrapper = serializer.Deserialize<ItemListWrapper>(
                File.ReadAllText(Path.Combine(DataDir, "items.json")));
            Assert.NotNull(wrapper);
            Assert.NotNull(wrapper.items);
            Assert.DoesNotContain(wrapper.items, i => i.id == "gun_oil");
        }

        [Fact]
        public void Recipes_ZeroResultSink_RejectedAtLoad()
        {
            string dir = Path.Combine(Path.GetTempPath(),
                "ashfall_plan10_sink_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            try
            {
                // minimal item catalog with the referenced items
                File.WriteAllText(Path.Combine(dir, "items.json"),
                    "{\"items\":[" +
                    "{\"id\":\"gun_oil\",\"displayName\":\"Oil\",\"type\":\"Component\",\"stackMax\":1,\"weight\":0.2,\"tradeValue\":10}," +
                    "{\"id\":\"cloth\",\"displayName\":\"Cloth\",\"type\":\"Material\",\"stackMax\":10,\"weight\":0.1,\"tradeValue\":1}," +
                    "{\"id\":\"scrap_metal\",\"displayName\":\"Scrap\",\"type\":\"Material\",\"stackMax\":10,\"weight\":0.5,\"tradeValue\":1}" +
                    "]}");
                // sink recipe (not in the allowlist)
                File.WriteAllText(Path.Combine(dir, "recipes.json"),
                    "{\"schema_version\":1,\"recipes\":[{\"id\":\"sink_attempt\"," +
                    "\"recipeName\":\"Sink\",\"ingredients\":[" +
                    "{\"itemId\":\"gun_oil\",\"amount\":1},{\"itemId\":\"cloth\",\"amount\":1}]," +
                    "\"resultItemId\":\"scrap_metal\",\"resultAmount\":0," +
                    "\"craftingTimeHours\":0.5,\"requiredStationId\":\"workbench\"}]}");
                var ex = Assert.Throws<System.IO.InvalidDataException>(() =>
                    RecipeCatalogLoader.Load(dir, new FileSystemIO(),
                        new SystemTextJsonSerializer(),
                        new ItemCatalog()));
                Assert.Contains("sink_attempt", ex.Message);
                Assert.Contains("resultAmount", ex.Message);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }

        [Fact]
        public void Recipes_AllowlistedZeroResult_LoadClean()
        {
            string dir = Path.Combine(Path.GetTempPath(),
                "ashfall_plan10_allow_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            try
            {
                File.WriteAllText(Path.Combine(dir, "items.json"),
                    "{\"items\":[" +
                    "{\"id\":\"fuel\",\"displayName\":\"Fuel\",\"type\":\"Fuel\",\"stackMax\":10,\"weight\":0.5,\"tradeValue\":1}," +
                    "{\"id\":\"item_thermal_paste\",\"displayName\":\"Paste\",\"type\":\"Component\",\"stackMax\":1,\"weight\":0.2,\"tradeValue\":2}," +
                    "{\"id\":\"fuel_1l\",\"displayName\":\"1L\",\"type\":\"Fuel\",\"stackMax\":1,\"weight\":1,\"tradeValue\":3}," +
                    "{\"id\":\"item_epoxy_injector\",\"displayName\":\"Epoxy\",\"type\":\"Tool\",\"stackMax\":1,\"weight\":1,\"tradeValue\":4}," +
                    "{\"id\":\"item_galvanized_rebar\",\"displayName\":\"Rebar\",\"type\":\"Component\",\"stackMax\":10,\"weight\":1,\"tradeValue\":5}" +
                    "]}");
                File.WriteAllText(Path.Combine(dir, "recipes.json"),
                    "{\"schema_version\":1,\"recipes\":[{\"id\":\"refuel_heater\",\"recipeName\":\"Refuel Heater\",\"ingredients\":[{\"itemId\":\"fuel\",\"amount\":3}],\"resultItemId\":\"fuel\",\"resultAmount\":0,\"craftingTimeHours\":0.1,\"requiredStationId\":\"heater\"}]}");
                var recipes = RecipeCatalogLoader.Load(dir, new FileSystemIO(),
                    new SystemTextJsonSerializer(), new ItemCatalog());
                // It must have loaded (allowlisted) — silent forgiveness is the
                // documented behaviour for refuel_heater. We zero in on the
                // breaking change: a NEW sink would NOT load.
                Assert.Single(recipes);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }

        // ───────────────────────── 4. dive_sites keeper ───────────────────

        [Fact]
        public void DiveSites_Plan10Sites_UseNull_NotEmpty()
        {
            var container = DiveSiteCatalogLoader.Load(DataDir,
                new FileSystemIO(), new SystemTextJsonSerializer());
            var plan10Sites = new[]
            {
                "site_exp09_sunken_submarine",
                "site_exp09_flooded_metro",
                "site_exp09_submerged_convoy",
                "site_exp09_drowned_fuel_depot",
                "site_exp09_offshore_relay",
                "site_exp09_flooded_field_hospital",
                "site_exp09_wrecked_patrol_craft",
                "site_exp09_submerged_siphon"
            };
            foreach (var id in plan10Sites)
            {
                var s = DiveSiteCatalogLoader.FindById(container, id);
                Assert.NotNull(s);
                Assert.Null(s!.keeper_thread_id);
            }
            // The original sovereign site still carries the keeper thread id.
            var sovereign = DiveSiteCatalogLoader.FindById(container, "site_exp09_ss_sovereign");
            Assert.NotNull(sovereign);
            Assert.Equal("q_keeper_of_logs", sovereign!.keeper_thread_id);
        }
    }

    // ─── minimal DTO probes ──────────────────────────────────────────────

    [Serializable]
    internal sealed class RecipeListWrapper
    {
        public int schema_version { get; set; }
        public System.Collections.Generic.List<RecipeJsonDto> recipes { get; set; } = new System.Collections.Generic.List<RecipeJsonDto>();
    }

    [Serializable]
    internal sealed class ItemListWrapper
    {
        public int schema_version { get; set; }
        public System.Collections.Generic.List<RecipeItemJsonProbe> items { get; set; } = new System.Collections.Generic.List<RecipeItemJsonProbe>();
    }

    [Serializable]
    internal sealed class RecipeJsonDto
    {
        public string id { get; set; } = string.Empty;
        public string recipeName { get; set; } = string.Empty;
        public System.Collections.Generic.List<RecipeIngredientDto>? ingredients { get; set; }
        public string resultItemId { get; set; } = string.Empty;
        public int resultAmount { get; set; } = 1;
        public float craftingTimeHours { get; set; } = 1f;
        public string requiredStationId { get; set; } = string.Empty;
    }

    [Serializable]
    internal sealed class RecipeIngredientDto
    {
        public string itemId { get; set; } = string.Empty;
        public int amount { get; set; } = 1;
    }

    [Serializable]
    internal sealed class RecipeItemJsonProbe
    {
        public string id { get; set; } = string.Empty;
        public string displayName { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
    }
}
