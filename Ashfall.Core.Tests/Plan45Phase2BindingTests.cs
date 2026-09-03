// SPDX-License-Identifier: MIT
// Plan 45 phase 2 — raid + wildlife/excavation bindings through the
// EnemyCompositionSelector path. Pins:
//   1. raid composition (§46 human strata, danger-weighted anchor);
//   2. wildlife packs (§47 species-tag → fauna/mutant mapping, honest empty);
//   3. site defense (§38/§48 excavation seam);
//   4. the hostile-encounter router (Creature→wildlife, Human→raid/ambush,
//      non-combat categories → nothing);
//   5. the data binding (travel_encounters.json combatant_tag round-trip);
//   6. the TravelEncounterCombatBinder (hostile choice → catalog spawn).

using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;

namespace Ashfall.Core.Tests;

public class Plan45Phase2BindingTests : CatalogTestBase
{
    private static void ReloadCatalog()
    {
        CombatCatalog.Clear();
        Assert.True(CombatCatalogLoader.Load(DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer()));
    }

    private static readonly string[] RaidPool =
    {
        "combatant_warlord_veteran", "combatant_hydro_pump_warden",
        "combatant_salvage_veteran", "combatant_desperate_scavenger",
    };

    // ── Raid composition (§46) ─────────────────────────────────────────

    [Fact]
    public void RaidComposition_HumanOnlyPool_CountAndAnchor()
    {
        ReloadCatalog();
        // High danger: organized raider (warlord veteran) anchors the crew.
        var high = EnemyCompositionSelector.SelectRaidComposition(7, 4);
        Assert.Equal(4, high.Count);
        Assert.Equal("combatant_warlord_veteran", high[0]);
        Assert.All(high, id => Assert.Contains(id, RaidPool));
        Assert.All(high, id => Assert.True(CombatCatalog.HasCombatant(id)));

        // Low danger: scavenger attackers lead, no warlord enforcer anchor.
        var low = EnemyCompositionSelector.SelectRaidComposition(2, 4);
        Assert.Equal("combatant_salvage_veteran", low[0]);
        Assert.Contains("combatant_desperate_scavenger", low);
        Assert.DoesNotContain("combatant_feral_mutt", low); // fauna never raids
    }

    [Fact]
    public void RaidComposition_IsDeterministic()
    {
        ReloadCatalog();
        var a = EnemyCompositionSelector.SelectRaidComposition(6, 3);
        var b = EnemyCompositionSelector.SelectRaidComposition(6, 3);
        Assert.Equal(a, b);
        var c = EnemyCompositionSelector.SelectRaidComposition(6, 3, new SeededRng(88));
        var d = EnemyCompositionSelector.SelectRaidComposition(6, 3, new SeededRng(88));
        Assert.Equal(c, d);
    }

    // ── Wildlife packs (§47) ───────────────────────────────────────────

    [Theory]
    [InlineData("pack_canine", "combatant_feral_mutt")]
    [InlineData("swarm", "combatant_burrower_mite")]
    [InlineData("lurker", "combatant_pale_crawler")]
    [InlineData("spore_predator", "combatant_spore_hound")]
    [InlineData("charger", "combatant_armored_boar")]
    [InlineData("apex", "combatant_armored_boar")]
    public void WildlifeComposition_TagMapsToSingleSpeciesPack(string tag, string expectedId)
    {
        ReloadCatalog();
        var pack = EnemyCompositionSelector.SelectWildlifeComposition(tag, 3);
        Assert.Equal(3, pack.Count);
        Assert.All(pack, id => Assert.Equal(expectedId, id));
        Assert.True(CombatCatalog.HasCombatant(expectedId), $"{expectedId} must be registered");
    }

    [Fact]
    public void WildlifeComposition_UnknownOrEmptyTag_IsHonestlyEmpty()
    {
        // An unbound creature encounter must not become humans in hides.
        Assert.Empty(EnemyCompositionSelector.SelectWildlifeComposition("", 3));
        Assert.Empty(EnemyCompositionSelector.SelectWildlifeComposition("robot_dragon", 3));
    }

    // ── Site defense (§38/§48) ─────────────────────────────────────────

    [Fact]
    public void SiteDefense_WardenAnchor()
    {
        ReloadCatalog();
        var defense = EnemyCompositionSelector.SelectSiteDefense(2);
        Assert.Equal(2, defense.Count);
        Assert.All(defense, id => Assert.Equal("combatant_hydro_pump_warden", id));
    }

    // ── Hostile-encounter router ───────────────────────────────────────

    [Fact]
    public void Router_CreatureRoutesToWildlifeByTag()
    {
        ReloadCatalog();
        var ids = EnemyCompositionSelector.SelectForHostileEncounter("Creature", "pack_canine", 2, 3);
        Assert.Equal(3, ids.Count);
        Assert.All(ids, id => Assert.Equal("combatant_feral_mutt", id));
    }

    [Fact]
    public void Router_HumanRoutesRaidAtHighDanger_AmbushBelow()
    {
        ReloadCatalog();
        // High danger → raid crew (warlord anchor).
        var raid = EnemyCompositionSelector.SelectForHostileEncounter("Human", "", 7, 3);
        Assert.Equal("combatant_warlord_veteran", raid[0]);
        // Low danger → ambush patrol (desperate scavenger anchor).
        var ambush = EnemyCompositionSelector.SelectForHostileEncounter("Human", "", 2, 3);
        Assert.Equal("combatant_desperate_scavenger", ambush[0]);
    }

    [Theory]
    [InlineData("Environmental")]
    [InlineData("Chained")]
    [InlineData("Discovery")]
    [InlineData("Social")]
    [InlineData("Trade")]
    public void Router_NonCombatCategories_YieldNothing(string category)
    {
        EnemyCompositionSelector.SelectForHostileEncounter(category, "pack_canine", 7, 3);
        var ids = EnemyCompositionSelector.SelectForHostileEncounter(category, "", 7, 3);
        Assert.Empty(ids);
    }

    // ── Data binding: combatant_tag round-trip ─────────────────────────

    [Fact]
    public void TravelCatalog_CreatureEncountersCarryCombatantTags()
    {
        var cwd = System.IO.Directory.GetCurrentDirectory();
        CatalogLocator.TryFindDataDirectory(cwd, out var dataDir);
        var dir = string.IsNullOrEmpty(dataDir) ? cwd : dataDir;

        var catalog = TravelEncounterCatalog.LoadFromDirectory(dir, new FileSystemIO());
        Assert.NotNull(catalog);

        var creatures = new List<TravelEncounterDefinition>();
        foreach (var def in catalog.Encounters)
            if (def != null && def.Category == "Creature")
                creatures.Add(def);

        Assert.Equal(8, creatures.Count);
        Assert.All(creatures, c => Assert.False(string.IsNullOrEmpty(c.CombatantTag),
            $"creature encounter {c.Id} missing combatant_tag"));
        // Every tag binds to a registered combatant pack.
        foreach (var c in creatures)
        {
            var pack = EnemyCompositionSelector.SelectWildlifeComposition(c.CombatantTag, 1);
            Assert.Single(pack);
            Assert.True(CombatCatalog.HasCombatant(pack[0]),
                $"{c.Id}: tag '{c.CombatantTag}' binds unregistered id '{pack[0]}'");
        }
        // Non-creature rows stay untagged (ambush/raid routing instead).
        Assert.All(catalog.Encounters, e =>
        {
            if (e != null && e.Category != "Creature")
                Assert.True(string.IsNullOrEmpty(e.CombatantTag),
                    $"{e.Id}: non-creature row should not carry a wildlife tag");
        });
    }

    // ── Binder: hostile choice → composition ───────────────────────────

    private static TravelEncounterChoice Choice(bool nonviolent, bool avoidance)
        => new TravelEncounterChoice { ChoiceId = "c1", IsNonviolent = nonviolent, IsAvoidance = avoidance };

    [Fact]
    public void Binder_HostileCreatureChoiceBindsWildlifePack()
    {
        ReloadCatalog();
        var def = new TravelEncounterDefinition
        {
            Id = "enc_probe_wolves", Category = "Creature", CombatantTag = "pack_canine",
        };
        Assert.True(TravelEncounterCombatBinder.TryBind(
            def, Choice(nonviolent: false, avoidance: false), 3, 3, out var ids));
        Assert.Equal(3, ids.Count);
        Assert.All(ids, id => Assert.Equal("combatant_feral_mutt", id));
    }

    [Fact]
    public void Binder_NonviolentOrAvoidanceChoicesBindNothing()
    {
        ReloadCatalog();
        var def = new TravelEncounterDefinition
        {
            Id = "enc_probe_wolves", Category = "Creature", CombatantTag = "pack_canine",
        };
        Assert.False(TravelEncounterCombatBinder.TryBind(
            def, Choice(nonviolent: true, avoidance: false), 3, 3, out var a));
        Assert.Empty(a);
        Assert.False(TravelEncounterCombatBinder.TryBind(
            def, Choice(nonviolent: false, avoidance: true), 3, 3, out var b));
        Assert.Empty(b);
        // The real data choice the selftest exercises (throw flare) stays non-combat.
        Assert.False(TravelEncounterCombatBinder.TryBind(
            def, Choice(nonviolent: true, avoidance: true), 3, 3, out var c));
        Assert.Empty(c);
    }

    [Fact]
    public void Binder_EnvironmentalChoicesNeverBind_EvenWhenHostile()
    {
        ReloadCatalog();
        var def = new TravelEncounterDefinition
        {
            Id = "enc_probe_storm", Category = "Environmental", CombatantTag = "",
        };
        Assert.False(TravelEncounterCombatBinder.TryBind(
            def, Choice(nonviolent: false, avoidance: false), 7, 3, out var ids));
        Assert.Empty(ids);
    }

    // ── End-to-end: binder ids spawn catalog rows ──────────────────────

    [Fact]
    public void Encounter_WildlifePackFromBinderSpawnsFaunaRows()
    {
        ReloadCatalog();
        var def = new TravelEncounterDefinition
        {
            Id = "enc_probe_hyenas", Category = "Creature", CombatantTag = "pack_canine",
        };
        Assert.True(TravelEncounterCombatBinder.TryBind(
            def, Choice(nonviolent: false, avoidance: false), 4, 3, out var ids));

        var sys = new TacticalCombatSystem(null, new CombatHostPorts(null, null, null, consumeAmmo: (id, n) => 5000));
        var players = new List<CombatantState>
        {
            new CombatantState { Id = "p0", Name = "Yuki", SurvivorId = "sv0", IsPlayer = true, Health = 100, MaxHealth = 100 },
        };
        var weapons = new List<WeaponInstanceState>
        {
            new WeaponInstanceState { InstanceId = "w0", WeaponId = "weapon_bolt_rifle", OwnerSurvivorId = "sv0", ConditionPct = 0.9f, AmmoId = "ammo_308", AmmoRemaining = 150 },
        };
        Assert.True(sys.BeginEncounter("enc_p45b", "exp", "loc", "Hyena Den", 9, 4242, players, weapons, ids.Count, 0f, ids));

        var enemies = sys.State.Combatants.Where(c => !c.IsPlayer).ToList();
        Assert.Equal(3, enemies.Count);
        // A hyena den is a mutt pack — fauna rows, no humans in hides.
        Assert.All(enemies, e => Assert.Equal("combatant_feral_mutt", e.CatalogId));
        Assert.Contains(sys.State.Events, e => e.Kind == "enemy_catalog_spawn");

        // Pack fight resolves within budget (squad beats the pack).
        sys.ResolveToEnd(new SeededRng(909), maxTurns: 120);
        Assert.True(sys.State.Resolved, "pack fight must resolve within budget");
    }
}
