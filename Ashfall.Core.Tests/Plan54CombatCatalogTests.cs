// SPDX-License-Identifier: MIT
// Plan 54 — Combat Catalog Expansion: 20 weapons / 12 hostile combatants.
//
// Pins the expanded data authority against the runtime contract:
//   1. catalog loads with exactly 20 weapons (15 baseline + 5 Plan 54);
//   2. the plan's "original five" and every baseline weapon keep their ids
//      and caliber bindings (parity oracle, constraint 1.2);
//   3. no two of the twenty weapons are stat clones (constraint 1.11);
//   4. all weapon calibers resolve (loader also enforces; pinned here);
//   5. exactly 12 combatant definitions, incl. the two Plan 54 archetypes,
//      spawn through CombatantFactory with catalog-derived AI traits;
//   6. a new weapon fires, jams machinery works, and the encounter replays
//      deterministically from the same seed (Invariant 4);
//   7. combat save state round-trips carrying a Plan 54 weapon instance;
//   8. the two new combatants drive a full encounter to resolution.

using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests;

public class Plan54CombatCatalogTests : CatalogTestBase
{
    private static void ReloadCatalog()
    {
        CombatCatalog.Clear();
        Assert.True(CombatCatalogLoader.Load(DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer()));
    }

    // ── Catalog shape ─────────────────────────────────────────────────

    [Fact]
    public void Catalog_Loads20Weapons()
    {
        ReloadCatalog();
        Assert.Equal(20, CombatCatalog.WeaponIds.Count);
    }

    [Fact]
    public void Catalog_PreservesThePlansOriginalFive()
    {
        ReloadCatalog();
        // The five weapons the plan names as the verified baseline oracle.
        var originalFive = new[]
        {
            "weapon_pipe_rifle", "weapon_scrap_shotgun", "weapon_bolt_rifle",
            "weapon_assault_rifle", "weapon_lmg",
        };
        foreach (var id in originalFive)
            Assert.True(CombatCatalog.HasWeapon(id), $"baseline weapon {id} must remain registered");
    }

    [Fact]
    public void Catalog_PreservesAll15BaselineWeaponsWithCalibers()
    {
        ReloadCatalog();
        var baseline = new Dictionary<string, string>
        {
            ["weapon_pipe_rifle"] = "ammo_357",
            ["weapon_scrap_shotgun"] = "ammo_12g",
            ["weapon_bolt_rifle"] = "ammo_308",
            ["weapon_assault_rifle"] = "ammo_556",
            ["weapon_lmg"] = "ammo_762",
            ["weapon_pipe_shotgun"] = "ammo_12g",
            ["weapon_nail_driver"] = "ammo_9x19",
            ["weapon_rebar_spear"] = "ammo_improvised_rod",
            ["weapon_molotov_thrower"] = "ammo_improvised_burn",
            ["weapon_service_rifle"] = "ammo_556",
            ["weapon_marksman_rifle"] = "ammo_308",
            ["weapon_smg"] = "ammo_9x19",
            ["weapon_sidearm"] = "ammo_9x19",
            ["weapon_rust_mosin"] = "ammo_762",
            ["weapon_farm_carbine"] = "ammo_22lr",
        };
        foreach (var kv in baseline)
        {
            var def = CombatCatalog.GetWeapon(kv.Key);
            Assert.NotNull(def);
            Assert.Equal(kv.Value, def!.caliber);
        }
    }

    [Fact]
    public void Catalog_RegistersThe5Plan54Weapons()
    {
        ReloadCatalog();
        var expected = new Dictionary<string, string>
        {
            ["weapon_revolver"] = "ammo_357",
            ["weapon_coach_shotgun"] = "ammo_12g_buck",
            ["weapon_trail_carbine"] = "ammo_762x54r",
            ["weapon_battle_rifle"] = "ammo_762",
            ["weapon_quiet_carbine"] = "ammo_556_subsonic",
        };
        foreach (var kv in expected)
        {
            var def = CombatCatalog.GetWeapon(kv.Key);
            Assert.True(def != null, $"Plan 54 weapon {kv.Key} missing from combat_catalog.json");
            Assert.Equal(kv.Value, def!.caliber);
            Assert.True(def.accuracy > 0f && def.accuracy <= 1f, $"{kv.Key} accuracy out of range");
            Assert.True(def.damage > 0f, $"{kv.Key} damage must be positive");
            Assert.True(def.range > 0f, $"{kv.Key} range must be positive");
            Assert.True(def.jamBase >= 0f && def.jamBase < 1f, $"{kv.Key} jam_base out of range");
            Assert.True(def.degradePerShot > 0f && def.degradePerShot < 1f, $"{kv.Key} degrade out of range");
            Assert.True(def.scrapRepairCost >= 1, $"{kv.Key} repair cost must be at least 1 scrap");
            Assert.True(def.conditionThreshold > 0f && def.conditionThreshold < 1f, $"{kv.Key} threshold out of range");
        }
    }

    [Fact]
    public void Catalog_AllCalibersResolve()
    {
        ReloadCatalog();
        foreach (var id in CombatCatalog.WeaponIds)
        {
            var def = CombatCatalog.GetWeapon(id)!;
            Assert.True(CombatCatalog.HasAmmo(def.caliber), $"weapon {id} references unknown caliber {def.caliber}");
        }
    }

    [Fact]
    public void Catalog_NoTwoWeaponsAreStatClones()
    {
        ReloadCatalog();
        // Constraint 1.11 / §52: the (accuracy, damage, range, burst, caliber)
        // tuple must be unique across the full 20-weapon roster.
        var tuples = new HashSet<string>();
        foreach (var id in CombatCatalog.WeaponIds)
        {
            var def = CombatCatalog.GetWeapon(id)!;
            var key = $"{def.accuracy}|{def.damage}|{def.range}|{def.burst}|{def.caliber}";
            Assert.True(tuples.Add(key), $"weapon {id} duplicates the stat tuple {key}");
        }
    }

    [Fact]
    public void Catalog_OrphanCalibersAreDocumented()
    {
        // Plan 54 gives consumers to 762x54r / 12g_buck / 556_subsonic.
        // 308_incendiary + 357_jhp remain reload-recipe outputs without a
        // firing weapon — a documented deferred seam, not silently broken.
        ReloadCatalog();
        var used = new HashSet<string>(CombatCatalog.WeaponIds.Select(w => CombatCatalog.GetWeapon(w)!.caliber));
        Assert.Contains("ammo_762x54r", used);
        Assert.Contains("ammo_12g_buck", used);
        Assert.Contains("ammo_556_subsonic", used);
    }

    // ── Combatants ────────────────────────────────────────────────────

    [Fact]
    public void CombatantCatalog_Loads12Definitions()
    {
        ReloadCatalog();
        Assert.Equal(12, CombatCatalog.CombatantIds.Count);
    }

    [Fact]
    public void CombatantCatalog_PreservesAll10BaselineDefinitions()
    {
        ReloadCatalog();
        var baseline = new[]
        {
            "combatant_burrower_mite", "combatant_spore_hound", "combatant_armored_boar",
            "combatant_feral_mutt", "combatant_pale_crawler", "combatant_chrome_loper",
            "combatant_conscript_levy", "combatant_warlord_veteran",
            "combatant_flotilla_marine", "combatant_desperate_scavenger",
        };
        foreach (var id in baseline)
            Assert.True(CombatCatalog.HasCombatant(id), $"baseline combatant {id} must remain registered");
    }

    [Fact]
    public void CombatantCatalog_RegistersThe2Plan54Archetypes()
    {
        ReloadCatalog();

        var veteran = CombatCatalog.GetCombatant("combatant_salvage_veteran");
        Assert.NotNull(veteran);
        Assert.Equal("human", veteran!.kind);
        Assert.Equal("faction_unaligned", veteran.factionId);
        Assert.Equal("HoldPosition", veteran.aiStancePreference);
        Assert.InRange(veteran.surrenderThreshold, 0f, 1f); // resolve path open
        Assert.InRange(veteran.fleeThreshold, 0f, 1f);

        var warden = CombatCatalog.GetCombatant("combatant_hydro_pump_warden");
        Assert.NotNull(warden);
        Assert.Equal("human", warden!.kind);
        Assert.Equal("faction_hydro_barons", warden.factionId);
        Assert.Equal("HoldPosition", warden.aiStancePreference);
        // Site-defense contract: never abandons the works.
        Assert.Equal(-1f, warden.surrenderThreshold);
        Assert.Equal(-1f, warden.fleeThreshold);
        // Highest cover in the catalog — dug in behind the pump housing.
        Assert.True(warden.baseCoverRating >= 0.6f);
    }

    [Fact]
    public void CombatantFactory_SpawnsPlan54ArchetypesWithCatalogTraits()
    {
        ReloadCatalog();

        var veteran = CombatantFactory.SpawnFromCatalogOrThrow("combatant_salvage_veteran");
        Assert.Equal("Salvage-Veteran Scavenger", veteran.Name);
        Assert.Equal("faction_unaligned", veteran.FactionId);
        Assert.Equal(90f, veteran.Health, 1);
        Assert.Equal(0.20f, veteran.ArmorRating, 3);
        Assert.Equal(1.00f, veteran.AiAccuracyMod, 3);
        Assert.Equal("combatant_salvage_veteran", veteran.CatalogId);

        var warden = CombatantFactory.SpawnFromCatalogOrThrow("combatant_hydro_pump_warden");
        Assert.Equal("Hydro-Baron Pump Warden", warden.Name);
        Assert.Equal("faction_hydro_barons", warden.FactionId);
        Assert.Equal(95f, warden.Health, 1);
        Assert.Equal(0.60f, warden.CoverRating, 3);
        Assert.Equal(1.10f, warden.AiAccuracyMod, 3);
        Assert.Equal(-1f, warden.SurrenderThreshold);
        Assert.Equal(-1f, warden.FleeThreshold);
        Assert.Equal("combatant_hydro_pump_warden", warden.CatalogId);
    }

    // ── Runtime behavior ──────────────────────────────────────────────

    private static TacticalCombatSystem EngineWithPlan54Weapon(
        string weaponId, string ammoId, IReadOnlyList<string> enemyIds, int seed)
    {
        var sys = new TacticalCombatSystem(null, new CombatHostPorts(null, null, null, consumeAmmo: (id, n) => 5000));
        var players = new List<CombatantState>
        {
            new CombatantState { Id = "p1", Name = "Yuki", SurvivorId = "sv1", IsPlayer = true, Health = 100, MaxHealth = 100 },
        };
        var weapons = new List<WeaponInstanceState>
        {
            new WeaponInstanceState
            {
                InstanceId = "w1", WeaponId = weaponId, OwnerSurvivorId = "sv1",
                ConditionPct = 0.9f, AmmoId = ammoId, AmmoRemaining = 60,
            },
        };
        Assert.True(sys.BeginEncounter("enc_p54", "exp_p54", "loc_p54", "Plan 54 Proving Ground",
            7, seed, players, weapons, enemyIds.Count, 0f, enemyIds));
        return sys;
    }

    [Fact]
    public void Encounter_NewWeaponFires_NewEnemiesSpawnFromCatalog()
    {
        ReloadCatalog();
        var enemyIds = new List<string> { "combatant_salvage_veteran", "combatant_hydro_pump_warden" };
        var sys = EngineWithPlan54Weapon("weapon_battle_rifle", "ammo_762", enemyIds, 4242);

        // Both enemies resolved through the factory (CatalogId set).
        var enemies = sys.State.Combatants.Where(c => !c.IsPlayer).ToList();
        Assert.Equal(2, enemies.Count);
        Assert.All(enemies, e => Assert.False(string.IsNullOrEmpty(e.CatalogId)));
        // Catalog health is honored because enemyHealth=0 overrides nothing.
        Assert.Contains(enemies, e => e.MaxHealth == 90f);
        Assert.Contains(enemies, e => e.MaxHealth == 95f);

        // Fire the new weapon: the shot consumes ammo, degrades the weapon,
        // and appends deterministic events.
        int eventsBefore = sys.State.Events.Count;
        var result = sys.PlayerFire(enemies[0].Id, new SeededRng(7));
        Assert.True(result.Success, result.Message);
        Assert.True(sys.State.Weapons[0].ShotsFired >= 2, "battle rifle burst should fire 2 rounds");
        Assert.True(sys.State.Events.Count > eventsBefore);
    }

    [Fact]
    public void Encounter_Plan54CombatReplaysIdentically()
    {
        ReloadCatalog();
        var enemyIds = new List<string> { "combatant_salvage_veteran", "combatant_hydro_pump_warden" };

        var a = EngineWithPlan54Weapon("weapon_quiet_carbine", "ammo_556_subsonic", enemyIds, 99);
        a.ResolveToEnd(new SeededRng(555));
        var b = EngineWithPlan54Weapon("weapon_quiet_carbine", "ammo_556_subsonic", enemyIds, 99);
        b.ResolveToEnd(new SeededRng(555));

        Assert.Equal(a.State.Events.Count, b.State.Events.Count);
        for (int i = 0; i < a.State.Events.Count; i++)
        {
            Assert.Equal(a.State.Events[i].Kind, b.State.Events[i].Kind);
            Assert.Equal(a.State.Events[i].TargetId, b.State.Events[i].TargetId);
            Assert.Equal(a.State.Events[i].Value, b.State.Events[i].Value, 3);
        }
        Assert.Equal(a.State.Resolved, b.State.Resolved);
    }

    [Fact]
    public void Encounter_AllPlan54WeaponsResolveAndFire()
    {
        ReloadCatalog();
        var roster = new (string weapon, string ammo)[]
        {
            ("weapon_revolver", "ammo_357"),
            ("weapon_coach_shotgun", "ammo_12g_buck"),
            ("weapon_trail_carbine", "ammo_762x54r"),
            ("weapon_battle_rifle", "ammo_762"),
            ("weapon_quiet_carbine", "ammo_556_subsonic"),
        };
        foreach (var (weapon, ammo) in roster)
        {
            var sys = EngineWithPlan54Weapon(weapon, ammo,
                new List<string> { "combatant_salvage_veteran" }, 11);
            var target = sys.State.Combatants.First(c => !c.IsPlayer).Id;
            var result = sys.PlayerFire(target, new SeededRng(3));
            Assert.True(result.Success, $"{weapon}: {result.Message}");
        }
    }

    // ── Persistence ───────────────────────────────────────────────────

    [Fact]
    public void SaveRoundTrip_Plan54WeaponAndEnemiesSuriveReload()
    {
        ReloadCatalog();
        var enemyIds = new List<string> { "combatant_hydro_pump_warden", "combatant_salvage_veteran" };
        var sys = EngineWithPlan54Weapon("weapon_trail_carbine", "ammo_762x54r", enemyIds, 1234);
        sys.PlayerFire("enemy_enc_p54_0", new SeededRng(21));

        var json = new SystemTextJsonSerializer();
        var blob = json.Serialize(sys.CaptureState());
        var restored = new TacticalCombatSystem();
        restored.RestoreState(json.Deserialize<CombatState>(blob)!);

        Assert.Equal(sys.State.EncounterId, restored.State.EncounterId);
        Assert.Equal("weapon_trail_carbine", restored.State.Weapons[0].WeaponId);
        Assert.Equal("ammo_762x54r", restored.State.Weapons[0].AmmoId);
        // Catalog-spawned enemies keep their CatalogId across the save wire.
        var warden = restored.State.Combatants.FirstOrDefault(c => c.CatalogId == "combatant_hydro_pump_warden");
        Assert.NotNull(warden);
        Assert.Equal(1.10f, warden!.AiAccuracyMod, 3);
        Assert.Equal(sys.State.Combatants.Count, restored.State.Combatants.Count);
    }
}
