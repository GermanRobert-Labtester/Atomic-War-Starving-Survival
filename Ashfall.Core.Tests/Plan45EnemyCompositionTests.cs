// SPDX-License-Identifier: MIT
// Plan 45 — enemyCombatantIds wired into expedition/patrol encounter setup.
//
// Pins the EnemyCompositionSelector binding matrix against the loaded combat
// catalog, and proves the full Core path: selector ids → BeginEncounter →
// CombatantFactory-spawned catalog rows (CatalogId set, catalog base_health
// honored, enemy_catalog_spawn events emitted, deterministic composition).

using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;

namespace Ashfall.Core.Tests;

public class Plan45EnemyCompositionTests : CatalogTestBase
{
    private static void ReloadCatalog()
    {
        CombatCatalog.Clear();
        Assert.True(CombatCatalogLoader.Load(DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer()));
    }

    // ── Selector contract ─────────────────────────────────────────────

    [Fact]
    public void Selector_ReturnsExactlyTheRequestedCount()
    {
        ReloadCatalog();
        foreach (var count in new[] { 1, 2, 3, 5 })
        {
            var ids = EnemyCompositionSelector.SelectAmbushComposition(4, count);
            Assert.Equal(count, ids.Count);
        }
        // Clamp: oversized groups cap at the ambush maximum.
        Assert.Equal(EnemyCompositionSelector.MaxAmbushCount,
            EnemyCompositionSelector.SelectAmbushComposition(4, 50).Count);
        // Degenerate input still yields a usable group.
        Assert.Single(EnemyCompositionSelector.SelectAmbushComposition(4, 0));
    }

    [Fact]
    public void Selector_OnlyReturnsRegisteredCatalogIds()
    {
        ReloadCatalog();
        foreach (var danger in new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })
        {
            var ids = EnemyCompositionSelector.SelectAmbushComposition(danger, EnemyCompositionSelector.MaxAmbushCount);
            Assert.All(ids, id =>
                Assert.True(CombatCatalog.HasCombatant(id),
                    $"danger {danger}: selector produced unregistered id '{id}'"));
        }
    }

    [Fact]
    public void Selector_ThreatBands_MatchTheBindingMatrix()
    {
        ReloadCatalog();
        // Low band (danger ≤ 2): desperate stragglers + feral dogs only.
        foreach (var danger in new[] { 0, 1, 2 })
        {
            var ids = EnemyCompositionSelector.SelectAmbushComposition(danger, 6);
            Assert.All(ids, id => Assert.Contains(id, new[]
            {
                "combatant_desperate_scavenger", "combatant_feral_mutt",
            }));
        }

        // Medium band (3..5): veteran scavengers, conscripts, flotilla, spore hounds.
        foreach (var danger in new[] { 3, 4, 5 })
        {
            var ids = EnemyCompositionSelector.SelectAmbushComposition(danger, 6);
            Assert.All(ids, id => Assert.Contains(id, new[]
            {
                "combatant_salvage_veteran", "combatant_conscript_levy",
                "combatant_flotilla_marine", "combatant_spore_hound",
            }));
        }

        // High band (≥ 6): warlord veterans, wardens, deep-wood mutants.
        foreach (var danger in new[] { 6, 7, 8, 9, 10 })
        {
            var ids = EnemyCompositionSelector.SelectAmbushComposition(danger, 6);
            Assert.All(ids, id => Assert.Contains(id, new[]
            {
                "combatant_warlord_veteran", "combatant_hydro_pump_warden",
                "combatant_pale_crawler", "combatant_chrome_loper",
                "combatant_armored_boar",
            }));
        }
    }

    [Fact]
    public void Selector_AnchorArchetypeLeadsEveryGroup()
    {
        ReloadCatalog();
        // The band's primary threat always takes the first slot.
        Assert.Equal("combatant_desperate_scavenger",
            EnemyCompositionSelector.SelectAmbushComposition(1, 3)[0]);
        Assert.Equal("combatant_salvage_veteran",
            EnemyCompositionSelector.SelectAmbushComposition(4, 3)[0]);
        Assert.Equal("combatant_warlord_veteran",
            EnemyCompositionSelector.SelectAmbushComposition(7, 3)[0]);
    }

    [Fact]
    public void Selector_IsDeterministicWithoutRng_AndWithSharedStream()
    {
        ReloadCatalog();
        // Pure calls: same context → identical composition.
        var a = EnemyCompositionSelector.SelectAmbushComposition(7, 3);
        var b = EnemyCompositionSelector.SelectAmbushComposition(7, 3);
        Assert.Equal(a, b);

        // Shared-stream calls: same seed → identical composition (the
        // ExpeditionEncounterBridge ordering decision — one stream per tick).
        var c = EnemyCompositionSelector.SelectAmbushComposition(7, 3, new SeededRng(2024));
        var d = EnemyCompositionSelector.SelectAmbushComposition(7, 3, new SeededRng(2024));
        Assert.Equal(c, d);
    }

    // ── BeginEncounter integration ────────────────────────────────────

    private static TacticalCombatSystem BeginWithSelector(int dangerLevel, int count, int seed, int playerCount = 1)
    {
        var sys = new TacticalCombatSystem(null, new CombatHostPorts(null, null, null, consumeAmmo: (id, n) => 5000));
        var players = new List<CombatantState>();
        for (int i = 0; i < playerCount; i++)
        {
            players.Add(new CombatantState
            {
                Id = "p" + i, Name = "Survivor " + i, SurvivorId = "sv" + i,
                IsPlayer = true, Health = 100, MaxHealth = 100,
            });
        }
        var weapons = new List<WeaponInstanceState>();
        for (int i = 0; i < playerCount; i++)
        {
            weapons.Add(new WeaponInstanceState
            {
                InstanceId = "w" + i, WeaponId = "weapon_bolt_rifle", OwnerSurvivorId = "sv" + i,
                ConditionPct = 0.9f, AmmoId = "ammo_308", AmmoRemaining = 150,
            });
        }
        var ids = EnemyCompositionSelector.SelectAmbushComposition(dangerLevel, count);
        Assert.True(sys.BeginEncounter("enc_p45", "exp_p45", "loc_p45", "Plan 45 Crossing",
            12, seed, players, weapons, ids.Count, 0f, ids));
        return sys;
    }

    [Fact]
    public void Encounter_SelectorIdsSpawnCatalogRowsWithCatalogHealth()
    {
        ReloadCatalog();
        // enemyHealth = 0 → catalog base_health honored (host passes 0 when
        // ids are supplied unless it forces an override).
        var sys = BeginWithSelector(dangerLevel: 7, count: 3, seed: 4711);

        var enemies = sys.State.Combatants.Where(c => !c.IsPlayer).ToList();
        Assert.Equal(3, enemies.Count);
        Assert.All(enemies, e =>
        {
            Assert.False(string.IsNullOrEmpty(e.CatalogId), "enemy must carry a catalog id");
            Assert.Contains(e.CatalogId, (IEnumerable<string>)EnemyCompositionSelector.BandPool(7));
        });
        // Catalog health honored (warlord 110 / warden 95 / mutants 80–110).
        Assert.All(enemies, e => Assert.Equal(e.MaxHealth, e.Health));
        Assert.All(enemies, e => Assert.True(e.MaxHealth >= 80f, $"catalog health expected, saw {e.MaxHealth}"));
        // Spawn events are honest about the catalog path.
        Assert.Contains(sys.State.Events, e => e.Kind == "enemy_catalog_spawn");
        Assert.DoesNotContain(sys.State.Events, e => e.Kind == "enemy_catalog_missing");
    }

    [Fact]
    public void Encounter_CompositionIsDeterministicPerDangerAndSeed()
    {
        ReloadCatalog();
        var a = BeginWithSelector(dangerLevel: 4, count: 3, seed: 99);
        var b = BeginWithSelector(dangerLevel: 4, count: 3, seed: 99);
        var aIds = a.State.Combatants.Where(c => !c.IsPlayer).Select(c => c.CatalogId).ToList();
        var bIds = b.State.Combatants.Where(c => !c.IsPlayer).Select(c => c.CatalogId).ToList();
        Assert.Equal(aIds, bIds);
    }

    [Fact]
    public void Encounter_HighDangerAmbushIsFightableAndResolvable()
    {
        ReloadCatalog();
        // A patrol-strength squad (3 armed survivors) vs the high-band anchor
        // sentry (warlord veteran, 110 HP behind 0.45 armor / 0.55 cover).
        // The fight must be dangerous but winnable within the turn budget
        // (DoD: high-tier enemies dangerous yet beatable by a competent
        // squad). Two-enemy high-band pairs (e.g. veteran + boar) are boss
        // territory — deliberately outside this guarantee.
        //
        // NOTE: ResolveToEnd cannot be used here — it never issues
        // PlayerClearJam, so a mid-fight jam stalls the headless loop forever
        // (the real UI clears jams through PlayerClearJam). Drive the
        // encounter the way a player would: clear jams, fire, end turn.
        var sys = BeginWithSelector(dangerLevel: 8, count: 1, seed: 777, playerCount: 3);
        var rng = new SeededRng(31337);
        int guard = 0;
        while (!sys.State.Resolved && guard++ < 120)
        {
            var jammedWeapon = sys.State.Weapons.FirstOrDefault(w => w.IsJammed && !string.IsNullOrEmpty(w.OwnerCombatantId));
            if (jammedWeapon != null)
            {
                var owner = sys.State.Combatants.First(c => c.IsPlayer && !c.IsDowned && c.WeaponInstanceId == jammedWeapon.InstanceId);
                sys.PlayerClearJam(owner.Id, rng);
            }
            else
            {
                // Downed enemies are valid targets (finishing blows) —
                // LivingEnemies only excludes HasFled, and the encounter
                // resolves only when every hostile is down-and-out.
                var target = sys.State.Combatants.FirstOrDefault(c => !c.IsPlayer && !c.HasFled);
                if (target == null) break;
                sys.PlayerFire(target.Id, rng);
            }
            if (!sys.State.Resolved)
                sys.EndTurn(rng);
        }
        Assert.True(sys.State.Resolved, "high-band ambush must resolve within budget");
        Assert.Equal((int)CombatPhase.Won, sys.State.Phase);
        // Victory loot still flows through the runtime grant.
        Assert.NotEmpty(sys.State.Loot);
    }
}
