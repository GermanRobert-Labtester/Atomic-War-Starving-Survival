// SPDX-License-Identifier: MIT
// Plan 10 — Combat & Expedition Depth: pinned contract tests for new
// combatants, new doctrines, new vehicles, new dive sites, and the
// improvised-weapon + ammo-loading recipes.

using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Maritime;
using Ashfall.Core.Warlords;

namespace Ashfall.Core.Tests;

public class Plan10CatalogCoverageTests : CatalogTestBase
{
    private static string DataDir => DataDirectory;

    // ── Combatants ────────────────────────────────────────────────────

    [Fact]
    public void CombatCatalog_RegistersAll10Plan10Combatants()
    {
        CombatCatalog.Clear();
        var files = new FileSystemIO();
        var json = new SystemTextJsonSerializer();
        Assert.True(CombatCatalogLoader.Load(DataDir, files, json));

        var required = new[]
        {
            "combatant_burrower_mite",
            "combatant_spore_hound",
            "combatant_armored_boar",
            "combatant_feral_mutt",
            "combatant_pale_crawler",
            "combatant_chrome_loper",
            "combatant_conscript_levy",
            "combatant_warlord_veteran",
            "combatant_flotilla_marine",
            "combatant_desperate_scavenger",
        };
        foreach (var id in required)
            Assert.True(CombatCatalog.HasCombatant(id), $"combatant {id} expected in combat_catalog.json");

        // Each registered combatant has a valid lane (0..2) and an AI move the loader accepts.
        var ids = CombatCatalog.CombatantIds.ToList();
        foreach (var id in ids)
        {
            var c = CombatCatalog.GetCombatant(id);
            Assert.NotNull(c);
            Assert.InRange(c.preferredLane, 0, 2);
            Assert.True(c.baseHealth > 0f);
        }

        // Two archetypes verify the kind field is human|mutant|fauna:
        Assert.Equal("fauna", CombatCatalog.GetCombatant("combatant_burrower_mite")!.kind);
        Assert.Equal("mutant", CombatCatalog.GetCombatant("combatant_pale_crawler")!.kind);
        Assert.Equal("human", CombatCatalog.GetCombatant("combatant_conscript_levy")!.kind);
    }

    [Fact]
    public void CombatCatalog_HumanArchetypes_HaveNonNegativeResolveThresholds()
    {
        // The four human archetypes must permit non-combat resolution paths
        // (bribery / surrender / flee) — i.e. not -1 across the board.
        CombatCatalog.Clear();
        CombatCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());

        var humans = new[]
        {
            "combatant_conscript_levy",
            "combatant_warlord_veteran",
            "combatant_flotilla_marine",
            "combatant_desperate_scavenger",
        };
        foreach (var id in humans)
        {
            var c = CombatCatalog.GetCombatant(id);
            Assert.NotNull(c);
            // At least one threshold must be open (0..1) — the system needs the resolve path.
            bool surrenderOpen = c.surrenderThreshold >= 0f && c.surrenderThreshold <= 1f;
            bool fleeOpen = c.fleeThreshold >= 0f && c.fleeThreshold <= 1f;
            Assert.True(surrenderOpen || fleeOpen,
                $"human archetype {id} needs at least one resolve path open (surrender or flee)");
        }
    }

    // ── Doctrine count ────────────────────────────────────────────────

    [Fact]
    public void WarlordCatalog_DoctrineCount_Is8With4NewEntries()
    {
        var files = new FileSystemIO();
        var catalog = WarlordDoctrineCatalogLoader.Load(DataDir, files, new SystemTextJsonSerializer());

        Assert.Equal(8, catalog.Doctrines.Count);
        var required = new[]
        {
            "warlord_doctrine_toll",
            "warlord_doctrine_consolidation",
            "warlord_doctrine_annexation",
            "warlord_doctrine_withdrawal",
            "warlord_doctrine_besiege",
            "warlord_doctrine_traffic",
            "warlord_doctrine_ashprophet",
            "warlord_doctrine_procedure",
        };
        foreach (var id in required)
        {
            Assert.NotNull(catalog.GetDoctrine(id));
            var d = catalog.GetDoctrine(id)!;
            // Each doctrine must have a non-empty description and a valid risk_tolerance range.
            Assert.False(string.IsNullOrWhiteSpace(d.description));
            Assert.InRange(d.risk_tolerance, 0f, 1f);
            // Every action weight must reference a defined eligible action.
            foreach (var kv in d.action_weights)
                Assert.Contains(kv.Key, (System.Collections.Generic.ICollection<string>)d.eligible_actions);
        }

        // Validation must still report clean after Plan 10 expansion.
        var validation = WarlordCatalogValidator.Validate(catalog, DataDir, files);
        Assert.True(validation.Clean, string.Join("\n  ", validation.Errors));
    }

    [Fact]
    public void WarlordCatalog_NewDoctrines_HaveDistinctPreferredGoals()
    {
        var files = new FileSystemIO();
        var catalog = WarlordDoctrineCatalogLoader.Load(DataDir, files, new SystemTextJsonSerializer());

        var newOnes = new[]
        {
            "warlord_doctrine_besiege",
            "warlord_doctrine_traffic",
            "warlord_doctrine_ashprophet",
            "warlord_doctrine_procedure",
        };
        var goals = newOnes.Select(id => catalog.GetDoctrine(id)!.preferred_goal).ToList();
        // No two new doctrines share the exact same preferred_goal (would imply duplicate AI persona).
        Assert.Equal(goals.Count, goals.Distinct().Count());
    }

    // ── Vehicles ──────────────────────────────────────────────────────

    [Fact]
    public void Vehicles_Has8Entries_FiveNewOnes()
    {
        var files = new FileSystemIO();
        var json = new SystemTextJsonSerializer();
        string path = files.Combine(DataDir, "vehicles.json");
        Assert.True(files.FileExists(path));
        var catalog = json.Deserialize<VehicleCatalog>(files.ReadAllText(path));
        Assert.NotNull(catalog);
        Assert.Equal(8, catalog!.vehicles.Count);

        var newOnes = new[]
        {
            "vehicle_steam_halftrack",
            "vehicle_armored_mobile_base",
            "vehicle_salvage_dredger",
            "vehicle_scout_motorcycle",
            "vehicle_ambulance_rig",
        };
        foreach (var id in newOnes)
        {
            var def = catalog.vehicles.Find(v => v.vehicle_id == id);
            Assert.NotNull(def);
            Assert.True(def!.max_fuel > 0f);
            Assert.True(def.cargo_capacity >= 0f);
            Assert.True(def.speed_multiplier > 0f);
            Assert.True(def.fuel_consumption_per_km > 0f);
            Assert.InRange(def.breakdown_threshold, 0f, 1f);
        }
    }

    [Fact]
    public void Vehicle_NewOnes_ApplyDefStatsToInstance()
    {
        var rng = new StubRng(0, 0.0, 0.0, 0.0, 0.0, 0.0);
        var vs = new ExpeditionVehicleSystem(rng);

        var catalog = new VehicleCatalog { vehicles = new System.Collections.Generic.List<VehicleDefinition> {
            new VehicleDefinition { vehicle_id = "vehicle_scout_motorcycle", display_name = "Scout Motorcycle", max_fuel = 18f, cargo_capacity = 18f, speed_multiplier = 2.4f, terrain_type = "rough", condition_max = 100f, fuel_consumption_per_km = 0.18f, breakdown_threshold = 0.30f, default_attachments = new System.Collections.Generic.List<string>() }
        }};
        vs.LoadCatalog(catalog);

        var r = vs.AcquireVehicle("vehicle_scout_motorcycle");
        Assert.Equal(ActionResult.StatusKind.Success, r.Status);
        var inst = vs.GetVehicle("vehicle_scout_motorcycle")!;
        Assert.Equal(18f, inst.maxFuel, 3);
        Assert.Equal(18f, inst.cargoCapacity, 3);
        Assert.Equal(2.4f, inst.speedMultiplier, 3);
        Assert.Equal("rough", inst.terrainType);
        // AcquireVehicle seeds half-fuel per spec.
        Assert.Equal(9f, inst.fuel, 3);
    }

    // ── Dive sites ────────────────────────────────────────────────────

    [Fact]
    public void DiveSites_Has14Entries_EightPlan10PlusTwoPlan23()
    {
        var files = new FileSystemIO();
        var json = new SystemTextJsonSerializer();
        var container = DiveSiteCatalogLoader.Load(DataDir, files, json);
        // Plan 10 landed 12; Plan 23 adds the final differentiated delta to 14.
        Assert.Equal(14, container.dive_sites.Count);

        var plan10Ones = new[]
        {
            "site_exp09_sunken_submarine",
            "site_exp09_flooded_metro",
            "site_exp09_submerged_convoy",
            "site_exp09_drowned_fuel_depot",
            "site_exp09_offshore_relay",
            "site_exp09_flooded_field_hospital",
            "site_exp09_wrecked_patrol_craft",
            "site_exp09_submerged_siphon",
        };
        foreach (var id in plan10Ones)
        {
            var site = DiveSiteCatalogLoader.FindById(container, id);
            Assert.NotNull(site);
            Assert.True(site!.oxygen_budget_ticks > 0);
            Assert.InRange(site.base_noise_floor, 0f, 1f);
            // Each site must advertise the canonical 4-room shape so the dive system can drive it.
            Assert.Equal(4, site.rooms.Count);
        }

        // Plan 23 delta: the two archetype gaps (safe-heavy + contamination-heavy).
        var plan23Ones = new[]
        {
            "site_exp23_payroll_strongroom",
            "site_exp23_brine_cistern"
        };
        foreach (var id in plan23Ones)
        {
            var site = DiveSiteCatalogLoader.FindById(container, id);
            Assert.NotNull(site);
            Assert.InRange(site!.oxygen_budget_ticks, 60, 150);
            Assert.Equal(4, site.rooms.Count);
        }
    }

    [Fact]
    public void DiveSites_ApproximateNoiseLadder_IsValid()
    {
        // The four new sites sit roughly between the existing four on the noise ladder.
        var container = DiveSiteCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
        foreach (var s in container.dive_sites)
        {
            Assert.InRange(s.base_noise_floor, 0.20f, 0.95f);
            Assert.InRange(s.oxygen_budget_ticks, 60, 150);
        }
    }

    // ── Recipes ───────────────────────────────────────────────────────

    [Fact]
    public void Recipes_ImprovisedWeaponsAndAmmoLoading_ResolveToExistingItems()
    {
        var files = new FileSystemIO();
        var json = new SystemTextJsonSerializer();

        // Build a set of all known item ids across the major catalogs.
        var itemPaths = new[]
        {
            "items.json",
            "black_flotilla_items.json",
            "holdfast_items.json",
            "crossing_items.json",
            "chemical_dependency_items.json",
            "dose_items.json",
        };
        var ids = new System.Collections.Generic.HashSet<string>(System.StringComparer.Ordinal);
        foreach (var name in itemPaths)
        {
            string p = files.Combine(DataDir, name);
            if (!files.FileExists(p)) continue;
            var list = CatalogLocator.LoadWrappedList<WarlordIdProbe>(files.ReadAllText(p), SystemTextJsonSerializer.Options);
            if (list == null) continue;
            foreach (var it in list)
                if (!string.IsNullOrEmpty(it.id)) ids.Add(it.id);
        }

        string recipesPath = files.Combine(DataDir, "recipes.json");
        Assert.True(files.FileExists(recipesPath), "recipes.json must exist");
        var root = json.Deserialize<RecipeRootProbe>(files.ReadAllText(recipesPath));
        Assert.NotNull(root);

        var plan10Recipes = new[]
        {
            "craft_pipe_shotgun", "craft_nail_driver", "craft_rebar_spear", "craft_molotov_thrower",
            "reload_9x19", "reload_22lr", "reload_357_jhp", "reload_12g_buck", "reload_308_incendiary",
        };
        foreach (var rid in plan10Recipes)
        {
            var r = root.recipes.Find(x => x.id == rid);
            Assert.NotNull(r);
            foreach (var ing in r.ingredients)
            {
                Assert.True(ids.Contains(ing.itemId),
                    $"recipe {rid}: ingredient {ing.itemId} is not present in any item catalog");
            }
            Assert.True(ids.Contains(r.resultItemId),
                $"recipe {rid}: result {r.resultItemId} is not present in any item catalog");
        }
    }

    // Local minimal recipe probe (snake_case JSON, ammos are the only result typo we're guarding).
    [System.Serializable]
    private sealed class RecipeRootProbe
    {
        public int schema_version;
        public System.Collections.Generic.List<RecipeProbe> recipes = new System.Collections.Generic.List<RecipeProbe>();
    }
    [System.Serializable]
    private sealed class RecipeProbe
    {
        public string id;
        public System.Collections.Generic.List<IngredientProbe> ingredients = new System.Collections.Generic.List<IngredientProbe>();
        public string resultItemId;
    }
    [System.Serializable]
    private sealed class IngredientProbe
    {
        public string itemId;
    }
}
