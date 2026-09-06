using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Subterranean;
using Xunit;
using UndergroundInventory = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Flagship11;

/// <summary>
/// Plan 156 behaviour matrix: deterministic generation, stable ids, seeded
/// cave-ins, atomic shoring/ventilation, oxygen depletion with ventilation and
/// forced retreat, weather-driven flood, blockage clearing, claustrophobia
/// morale hook, save round-trip and restore-never-regenerates.
/// </summary>
public class SubterraneanSystemTests
{
    private static SubterraneanZoneCatalogContainer LoadRealZones() =>
        SubterraneanZoneCatalogLoader.Load(
            Flagship11TestBase.FindDataDirectory(), new FileSystemIO(), new SystemTextJsonSerializer());

    private static UndergroundInventory StockedInventory()
    {
        var inventory = new UndergroundInventory();
        Assert.True(inventory.AddById("scrap_wood", 99));
        Assert.True(inventory.AddById("steel_rebar", 99));
        Assert.True(inventory.AddById("scrap_metal", 99));
        Assert.True(inventory.AddById("battery", 99));
        return inventory;
    }

    private static SubterraneanSystem BuildSystem(int seed = 20260905, UndergroundInventory? inventory = null)
    {
        var system = new SubterraneanSystem(LoadRealZones(), inventory ?? StockedInventory());
        system.EnsureNetwork(seed);
        return system;
    }

    // ---------------------------------------------------------- generation

    [Fact]
    public void Generation_IsDeterministic_WithStableIds()
    {
        var a = BuildSystem(seed: 42);
        var b = BuildSystem(seed: 42);
        var c = BuildSystem(seed: 43);

        Assert.True(a.State.nodes.Count >= 10);
        Assert.True(a.State.generated);
        for (int i = 0; i < a.State.nodes.Count; i++)
        {
            Assert.Equal(a.State.nodes[i].nodeId, b.State.nodes[i].nodeId);
            Assert.Equal(a.State.nodes[i].structuralIntegrity, b.State.nodes[i].structuralIntegrity, 5);
            Assert.Equal(a.State.nodes[i].oxygenLevel, b.State.nodes[i].oxygenLevel, 5);
        }
        // Different seed ⇒ generated variance differs somewhere (integrity).
        Assert.NotEqual(
            a.State.nodes.Select(n => n.structuralIntegrity).ToList(),
            c.State.nodes.Select(n => n.structuralIntegrity).ToList());

        // Node ids come from the catalog verbatim.
        var zoneIds = a.Zones.Keys.ToHashSet(StringComparer.Ordinal);
        Assert.All(a.State.nodes, n => Assert.Contains(n.nodeId, zoneIds));
        Assert.Contains("subnode_metro_relay_annex", zoneIds);
    }

    [Fact]
    public void EnsureNetwork_IsIdempotent_AfterGeneration()
    {
        var system = BuildSystem(seed: 42);
        var node = system.Find("subnode_metro_relay_annex")!;
        node.structuralIntegrity = 55f;
        system.EnsureNetwork(43); // must NOT regenerate over live state
        Assert.Equal(55f, system.Find("subnode_metro_relay_annex")!.structuralIntegrity);
    }

    // ---------------------------------------------------------- discovery

    [Fact]
    public void Discovery_FlowsFromSurfaceAnchorDownThroughConnections()
    {
        var system = BuildSystem();
        Assert.False(system.IsDiscovered("subnode_metro_relay_annex"));

        var revealed = system.DiscoverFromAnchor("loc_hidden_relay_bunker");
        Assert.Contains("subnode_metro_relay_annex", revealed);
        Assert.True(system.IsDiscovered("subnode_metro_relay_annex"));
        Assert.False(system.IsDiscovered("subnode_transit_undercrossing")); // other anchor, deeper tier

        var deeper = system.DiscoverConnectedFrom("subnode_metro_relay_annex");
        Assert.Contains("subnode_bunker_annex_vault", deeper);
    }

    // ------------------------------------------------------------- hazards

    [Fact]
    public void Oxygen_DepletesWithOccupants_Ventilates_RequestsHealth_ForcesRetreat()
    {
        var system = BuildSystem();
        var node = system.Find("subnode_metro_relay_annex")!;
        node.oxygenLevel = 30f;

        var healthCalls = new List<(string survivorId, float delta)>();
        system.ApplyHealthDelta = (id, delta) => healthCalls.Add((id, delta));
        var retreats = new List<string>();
        system.OnForcedRetreat += (survivorId, _, _) => retreats.Add(survivorId);

        // Two occupants, one day, no ventilation: 30 - 5 = 25 → still >= threshold.
        system.ApplyUndergroundDay(10, new[] { ("s_a", node.nodeId), ("s_b", node.nodeId) });
        Assert.Equal(25f, node.oxygenLevel, 3);
        Assert.Empty(healthCalls);

        // Next day without ventilation drops below the low band: health requested.
        system.ApplyUndergroundDay(11, new[] { ("s_a", node.nodeId) });
        Assert.True(node.oxygenLevel < SubterraneanSystem.LowOxygenThreshold);
        Assert.Contains(("s_a", -SubterraneanSystem.LowOxygenHealthCostPerDay), healthCalls);

        // Below the retreat band: forced retreat fires (once per survivor/day).
        node.ventilationInstalled = false;
        node.oxygenLevel = 9f;
        system.ApplyUndergroundDay(12, new[] { ("s_a", node.nodeId), ("s_b", node.nodeId) });
        Assert.Equal(2, retreats.Count);

        // Ventilation recovers the reserve faster than one occupant drains it.
        node.oxygenLevel = 10f;
        system.TryInstallVentilation(node.nodeId);
        system.ApplyUndergroundDay(13, new[] { ("s_a", node.nodeId) });
        Assert.Equal(22.5f, node.oxygenLevel, 3); // 10 - 2.5 + 15
    }

    [Fact]
    public void Flood_FollowsCanonicalWeather_AndBlocksWhenSaturated()
    {
        var system = BuildSystem();
        var zone = system.Zones["subnode_flooded_maintenance_reach"]; // susceptibility 0.9
        var node = system.Find(zone.id)!;

        system.CurrentWeather = () => WeatherKind.FalloutStorm; // pressure 20
        system.ApplyUndergroundDay(1, Array.Empty<(string, string)>());
        Assert.True(node.waterLevel > 15f, $"storm should raise water, was {node.waterLevel}");

        system.CurrentWeather = () => WeatherKind.Clear;
        for (int day = 2; day <= 10; day++)
            system.ApplyUndergroundDay(day, Array.Empty<(string, string)>());
        Assert.True(node.waterLevel < 10f, "dry weather must let water seep out");

        // Saturation blocks the node and damages integrity.
        node.waterLevel = 79.5f;
        system.CurrentWeather = () => WeatherKind.FalloutStorm;
        float integrityBefore = node.structuralIntegrity;
        system.ApplyUndergroundDay(40, Array.Empty<(string, string)>());
        Assert.True(node.waterLevel >= SubterraneanSystem.FloodedThreshold, $"water after storm day was {node.waterLevel}, weather-delegate null? {system.CurrentWeather == null}");
        Assert.True(node.blocked);
        Assert.True(node.structuralIntegrity < integrityBefore);
    }

    [Fact]
    public void CaveIn_IsSeededDeterministic_AndShoringDampsRisk()
    {
        // Find a (day) where the seeded roll fires for a damaged, unshored node.
        var (day, damaged) = FindCaveInDay();
        Assert.True(day > 0, "expected some day to trigger a cave-in for integrity 25");

        var a = BuildSystem();
        var b = BuildSystem();
        foreach (var system in new[] { a, b })
        {
            var node = system.Find("subnode_metro_relay_annex")!;
            node.structuralIntegrity = damaged;
            system.ApplyUndergroundDay(day, Array.Empty<(string, string)>());
        }
        float afterA = a.Find("subnode_metro_relay_annex")!.structuralIntegrity;
        float afterB = b.Find("subnode_metro_relay_annex")!.structuralIntegrity;
        Assert.Equal(afterA, afterB, 5); // same seed ⇒ same outcome
        Assert.True(afterA < damaged, "cave-in must cost integrity");

        // Shoring damps the roll: an identically damaged, max-shored node survives.
        var shored = BuildSystem();
        var sNode = shored.Find("subnode_metro_relay_annex")!;
        sNode.structuralIntegrity = damaged;
        sNode.shoringLevel = SubterraneanSystem.MaxShoringLevel;
        shored.ApplyUndergroundDay(day, Array.Empty<(string, string)>());
        Assert.True(shored.Find("subnode_metro_relay_annex")!.lastHazardDay != day,
            "fully shored node should ride out the same roll");
    }

    private static (int day, float integrity) FindCaveInDay()
    {
        // integrity 0, no shoring, dry weather: risk = base + 0.5 + 0 → roll < risk*0.1.
        // integrity 25, no shoring: risk = 0.2 + 75/200 = 0.575 → unshored
        // threshold 0.0575, fully-shored threshold 0.575*0.25*0.1 = 0.014375.
        // Pick a roll in [0.014375, 0.0575): fires unshored, rides out shored.
        for (int day = 1; day <= 4000; day++)
        {
            var rng = new Ashfall.Core.SeededRng(SubterraneanSystem.NodeSeed(day, "subnode_metro_relay_annex"));
            double roll = rng.NextDouble();
            if (roll >= 0.014375 && roll < 0.0575)
                return (day, 25f);
        }
        return (0, 0f);
    }

    // ------------------------------------------------- shoring / materials

    [Fact]
    public void Shoring_IsAtomic_Permanent_AndReducesIntegrityDamage()
    {
        var inventory = StockedInventory();
        var system = BuildSystem(inventory: inventory);
        var node = system.Find("subnode_metro_relay_annex")!;
        node.structuralIntegrity = 60f;

        Assert.Null(system.TryShoreNode(node.nodeId)); // success
        Assert.Equal(1, node.shoringLevel);
        Assert.Equal(85f, node.structuralIntegrity, 3);
        Assert.Equal(95, inventory.CountById("scrap_wood"));

        // Second shoring demands more (steel rebar + scrap metal).
        Assert.Null(system.TryShoreNode(node.nodeId));
        Assert.Equal(2, node.shoringLevel);

        // Empty pocket: the transaction cancels and state is untouched.
        var poor = new UndergroundInventory();
        poor.AddById("scrap_wood", 1);
        var poorSystem = new SubterraneanSystem(LoadRealZones(), poor);
        poorSystem.EnsureNetwork(1);
        var poorNode = poorSystem.Find("subnode_metro_relay_annex")!;
        Assert.Equal("missing_materials", poorSystem.TryShoreNode(poorNode.nodeId));
        Assert.Equal(0, poorNode.shoringLevel);

        node.shoringLevel = SubterraneanSystem.MaxShoringLevel;
        Assert.Equal("shoring_maxed", system.TryShoreNode(node.nodeId));
    }

    [Fact]
    public void Blockage_ClearsOnlyWhenStructureIsSound()
    {
        var system = BuildSystem();
        var node = system.Find("subnode_metro_relay_annex")!;

        Assert.Equal("not_blocked", system.TryClearBlockage(node.nodeId));
        node.blocked = true;
        node.structuralIntegrity = 15f; // unsound
        Assert.Equal("structure_unsound", system.TryClearBlockage(node.nodeId));
        node.structuralIntegrity = 60f;
        Assert.Null(system.TryClearBlockage(node.nodeId));
        Assert.False(node.blocked);
    }

    // ---------------------------------------------------------- morale hook

    [Fact]
    public void Claustrophobia_Modifier_AppliesOnlyToBearers_UndergroundOnly()
    {
        var system = BuildSystem();
        Assert.Equal(SubterraneanSystem.ClaustrophobiaMoralePenalty,
            system.UndergroundMoraleDelta("subnode_metro_relay_annex", hasClaustrophobia: true));
        Assert.Equal(0f, system.UndergroundMoraleDelta("subnode_metro_relay_annex", hasClaustrophobia: false));
        Assert.Equal(0f, system.UndergroundMoraleDelta("not_a_node", hasClaustrophobia: true));
        Assert.Equal("trait_claustrophobe", SubterraneanSystem.ClaustrophobiaTraitId);
    }

    // ---------------------------------------------------------- persistence

    [Fact]
    public void SaveRoundTrip_PreservesTopology_AndRestoreNeverRegenerates()
    {
        var system = BuildSystem(seed: 77);
        system.DiscoverFromAnchor("loc_hidden_relay_bunker");
        var node = system.Find("subnode_metro_relay_annex")!;
        node.structuralIntegrity = 44f;
        node.oxygenLevel = 33f;
        node.waterLevel = 22f;
        node.shoringLevel = 1;

        var json = SubterraneanSaveCodec.Encode(
            SubterraneanSaveCodec.ToSaveState(system.CaptureState()), new SystemTextJsonSerializer());
        Assert.True(SubterraneanSaveCodec.TryDecode(json, new SystemTextJsonSerializer(), out var decoded));

        var restored = new SubterraneanSystem(LoadRealZones(), StockedInventory());
        restored.RestoreState(SubterraneanSaveCodec.FromSaveState(decoded));
        Assert.True(restored.State.generated);
        Assert.Equal(77, restored.State.networkSeed);
        Assert.Equal(44f, restored.Find("subnode_metro_relay_annex")!.structuralIntegrity, 4);

        // EnsureNetwork after restore must be a no-op: the persisted topology is
        // authoritative — restore never regenerates.
        restored.EnsureNetwork(999);
        Assert.Equal(77, restored.State.networkSeed);
        Assert.Equal(44f, restored.Find("subnode_metro_relay_annex")!.structuralIntegrity, 4);

        // Tamper rejection.
        var stripped = System.Text.RegularExpressions.Regex.Replace(json, "\"Checksum\":\"[^\"]*\"", "\"Checksum\":\"\"");
        Assert.False(SubterraneanSaveCodec.TryDecode(stripped, new SystemTextJsonSerializer(), out _));
    }

    [Fact]
    public void OldCampaigns_LoadAsNoNetwork_UntilFirstUnlock()
    {
        // A pre-Flagship campaign has no section: TryLoad yields null and the
        // host generates lazily from the campaign seed on first underground use.
        var system = new SubterraneanSystem(LoadRealZones(), StockedInventory());
        Assert.False(system.State.generated);
        Assert.Empty(system.State.nodes);
        system.EnsureNetwork(5);
        Assert.True(system.State.generated);
        Assert.NotEmpty(system.State.nodes);
    }
}
