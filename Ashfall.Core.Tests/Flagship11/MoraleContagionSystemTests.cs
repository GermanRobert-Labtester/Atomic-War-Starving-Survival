using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Flagship11;

/// <summary>
/// In-memory world for contagion tests: wires MoraleContagionPorts to simple
/// dictionaries so each test controls bonds, rooms, shifts and morale directly.
/// </summary>
internal sealed class ContagionTestWorld
{
    public List<string> Alive = new() { "s_a", "s_b", "s_c" };
    public Dictionary<string, float> Morale = new();
    public Dictionary<string, string> Rooms = new();
    public Dictionary<string, string> DutyRole = new();
    public Dictionary<(string, string), float> Bonds = new();
    public bool BeaconActive;
    public List<(string survivorId, float stress)> Breakdowns = new();
    public List<(string survivorId, int day)> Unassigned = new();
    public List<string> ClearedRoles = new();
    public List<MoraleBreakdownEvent> BreakdownEvents = new();
    public List<MoraleSchismEvent> SchismEvents = new();

    public ContagionTestWorld()
    {
        foreach (var id in Alive) Morale[id] = 50f;
    }

    /// <summary>Deep-enough clone for identical-continuation determinism checks.</summary>
    public ContagionTestWorld Clone()
    {
        var copy = new ContagionTestWorld
        {
            Alive = new List<string>(Alive),
            Morale = new Dictionary<string, float>(Morale),
            Rooms = new Dictionary<string, string>(Rooms),
            DutyRole = new Dictionary<string, string>(DutyRole),
            Bonds = new Dictionary<(string, string), float>(Bonds),
            BeaconActive = BeaconActive
        };
        return copy;
    }

    public MoraleContagionPorts Ports() => new()
    {
        AliveSurvivors = () => Alive,
        GetMorale = id => Morale.TryGetValue(id, out var m) ? m : 50f,
        ApplyMoraleDelta = (id, delta) => Morale[id] = Math.Clamp(Morale[id] + delta, 0f, 100f),
        AreInSameRoom = (a, b) =>
            Rooms.TryGetValue(a, out var ra) && Rooms.TryGetValue(b, out var rb) && ra == rb && ra != null,
        GetDutyRole = id => DutyRole.TryGetValue(id, out var r) ? r : string.Empty,
        GetBondStrength = (a, b) => Bonds.TryGetValue((a, b), out var v) ? v : Bonds.TryGetValue((b, a), out var v2) ? v2 : 0f,
        IsHopeBeaconActive = () => BeaconActive,
        UnassignSurvivor = (id, day) => Unassigned.Add((id, day)),
        ClearDutyRole = id => ClearedRoles.Add(id),
        TriggerBreakdown = (id, stress) => Breakdowns.Add((id, stress))
    };

    public MoraleContagionSystem System(ContagionEventsCatalogContainer? catalog = null)
    {
        var system = new MoraleContagionSystem(catalog ?? Catalog(), Ports());
        system.OnMoraleBreakdown += e => BreakdownEvents.Add(e);
        system.OnMoraleSchismTriggered += e => SchismEvents.Add(e);
        return system;
    }

    /// <summary>A bonded same-room pair: influence math is large and predictable.</summary>
    public static ContagionTestWorld BondedRoommates()
    {
        var world = new ContagionTestWorld();
        world.Rooms["s_a"] = "r1";
        world.Rooms["s_b"] = "r1";
        world.Bonds[("s_a", "s_b")] = 0.8f;
        return world;
    }

    public static ContagionEventsCatalogContainer Catalog()
    {
        var catalog = new ContagionEventsCatalogContainer();
        catalog.contagion_events.Add(new ContagionEventDef
        {
            id = "contagion_test_grief", display_name = "Test Grief", emotion_type = "despair",
            base_intensity = 0.8f, duration_days = 40, bond_multiplier = 1.5f,
            proximity_multiplier = 1.2f, recovery_per_day = 0.02f
        });
        catalog.contagion_events.Add(new ContagionEventDef
        {
            id = "contagion_test_rescue", display_name = "Test Rescue", emotion_type = "hope",
            base_intensity = 0.8f, duration_days = 40, bond_multiplier = 1.5f,
            proximity_multiplier = 1.2f, recovery_per_day = 0.02f
        });
        catalog.contagion_events.Add(new ContagionEventDef
        {
            id = "contagion_test_scare", display_name = "Test Scare", emotion_type = "panic",
            base_intensity = 0.9f, duration_days = 40, bond_multiplier = 1f,
            proximity_multiplier = 1.5f, recovery_per_day = 0.02f
        });
        catalog.contagion_events.Add(new ContagionEventDef
        {
            id = "contagion_hope_beacon", display_name = "Beacon", emotion_type = "hope",
            base_intensity = 0.5f, duration_days = 5, bond_multiplier = 1.2f,
            proximity_multiplier = 1.3f, recovery_per_day = 0.3f
        });
        return catalog;
    }
}

/// <summary>
/// Plan 154 behaviour matrix (§154.15 + edge cases): deterministic propagation,
/// buffered same-tick deltas, breakdown transitions, isolation, HopeBeacon,
/// schism rules, save round-trip and non-operative restore.
/// </summary>
public class MoraleContagionSystemTests
{
    private static ContagionEventsCatalogContainer LoadRealCatalog()
    {
        string dir = Flagship11TestBase.FindDataDirectory();
        return ContagionEventCatalogLoader.Load(dir, new FileSystemIO(), new SystemTextJsonSerializer());
    }

    // ---------------------------------------------------------- propagation

    [Fact]
    public void Influence_IsDeterministic_AcrossIndependentRuns()
    {
        var worldA = ContagionTestWorld.BondedRoommates();
        var worldB = ContagionTestWorld.BondedRoommates();
        var a = worldA.System();
        var b = worldB.System();

        a.StartContagionEvent("contagion_test_grief", "s_a", 1);
        b.StartContagionEvent("contagion_test_grief", "s_a", 1);
        for (int day = 1; day <= 6; day++)
        {
            a.EvaluateDailyContagion(day);
            b.EvaluateDailyContagion(day);
        }

        Assert.Equal(worldA.Morale, worldB.Morale);
        var stateA = a.CaptureState();
        var stateB = b.CaptureState();
        Assert.Equal(stateA.survivors.Count, stateB.survivors.Count);
        for (int i = 0; i < stateA.survivors.Count; i++)
        {
            Assert.Equal(stateA.survivors[i].survivorId, stateB.survivors[i].survivorId);
            Assert.Equal(stateA.survivors[i].despairPressure, stateB.survivors[i].despairPressure, 5);
        }
    }

    [Fact]
    public void Despair_Spreads_AndRoommatesReceiveMoreThanDistantSurvivors()
    {
        var world = new ContagionTestWorld();
        world.Rooms["s_a"] = "r1";
        world.Rooms["s_b"] = "r1";
        var system = world.System();
        system.StartContagionEvent("contagion_test_grief", "s_a", 1);

        system.EvaluateDailyContagion(1);

        // Higher morale = worse (polarity pinned in the smoke tests).
        Assert.True(world.Morale["s_b"] > 50f, "roommate receives despair");
        Assert.True(world.Morale["s_c"] > 50f, "settlement baseline carries some influence");
        Assert.True(world.Morale["s_b"] > world.Morale["s_c"], "same room must outweigh distance");
    }

    [Fact]
    public void Proximity_Grades_SameRoom_OverSameShift_OverDistant()
    {
        var catalog = ContagionTestWorld.Catalog();
        var worldNear = ContagionTestWorld.BondedRoommates();
        var worldShift = new ContagionTestWorld();
        worldShift.DutyRole["s_a"] = "night_watch";
        worldShift.DutyRole["s_b"] = "night_watch";
        var worldFar = new ContagionTestWorld();

        var sysNear = worldNear.System(catalog);
        var sysShift = worldShift.System(catalog);
        var sysFar = worldFar.System(catalog);

        sysNear.StartContagionEvent("contagion_test_grief", "s_a", 1);
        sysShift.StartContagionEvent("contagion_test_grief", "s_a", 1);
        sysFar.StartContagionEvent("contagion_test_grief", "s_a", 1);

        sysNear.EvaluateDailyContagion(1);
        sysShift.EvaluateDailyContagion(1);
        sysFar.EvaluateDailyContagion(1);

        float near = worldNear.Morale["s_b"] - 50f;
        float shift = worldShift.Morale["s_b"] - 50f;
        float far = worldFar.Morale["s_b"] - 50f;
        Assert.True(near > shift, $"same room ({near}) must outweigh same shift ({shift})");
        Assert.True(shift > far, $"same shift ({shift}) must outweigh baseline ({far})");
        Assert.True(far > 0f, "settlement baseline still carries some influence");
    }

    [Fact]
    public void Bond_Multiplier_Scales_Influence()
    {
        var catalog = ContagionTestWorld.Catalog();
        var worldStrangers = new ContagionTestWorld();
        worldStrangers.Rooms["s_a"] = "r1";
        worldStrangers.Rooms["s_b"] = "r1";
        var worldBonded = new ContagionTestWorld();
        worldBonded.Rooms["s_a"] = "r1";
        worldBonded.Rooms["s_b"] = "r1";
        worldBonded.Bonds[("s_a", "s_b")] = 0.9f;

        var strangers = worldStrangers.System(catalog);
        var bonded = worldBonded.System(catalog);
        strangers.StartContagionEvent("contagion_test_grief", "s_a", 1);
        bonded.StartContagionEvent("contagion_test_grief", "s_a", 1);
        strangers.EvaluateDailyContagion(1);
        bonded.EvaluateDailyContagion(1);

        Assert.True(worldBonded.Morale["s_b"] > worldStrangers.Morale["s_b"],
            "strong bond must carry more influence than no bond");
    }

    [Fact]
    public void Hope_And_Despair_Push_Morale_In_Opposite_Directions()
    {
        var catalog = ContagionTestWorld.Catalog();
        var worldDespair = ContagionTestWorld.BondedRoommates();
        var worldHope = ContagionTestWorld.BondedRoommates();

        var despair = worldDespair.System(catalog);
        var hope = worldHope.System(catalog);
        despair.StartContagionEvent("contagion_test_grief", "s_a", 1);
        hope.StartContagionEvent("contagion_test_rescue", "s_a", 1);
        despair.EvaluateDailyContagion(1);
        hope.EvaluateDailyContagion(1);

        Assert.True(worldDespair.Morale["s_b"] > 50f, "despair worsens morale (value rises)");
        Assert.True(worldHope.Morale["s_b"] < 50f, "hope improves morale (value falls)");
    }

    [Fact]
    public void Panic_IsModeledDistinctly_AndFeedsCrisisStress()
    {
        var catalog = ContagionTestWorld.Catalog();
        var world = ContagionTestWorld.BondedRoommates();
        var system = world.System(catalog);
        system.StartContagionEvent("contagion_test_scare", "s_a", 1);

        // Panic tips a survivor who is just shy of the band; the stress input
        // fed to the canonical crisis authority carries the panic component.
        world.Morale["s_b"] = 89.5f;
        system.EvaluateDailyContagion(1);

        var breakdown = world.BreakdownEvents.FirstOrDefault(e => e.SurvivorId == "s_b");
        Assert.NotNull(breakdown);
        Assert.Equal(MoraleEmotion.Panic, breakdown!.DominantEmotion);
        Assert.True(breakdown.StressInput > MoraleContagionSystem.DespairBreakdownMorale,
            "panic must contribute crisis stress beyond raw morale");
    }

    [Fact]
    public void SimultaneousSources_AreBuffered_NoSameTickFeedback()
    {
        var catalog = ContagionTestWorld.Catalog();
        var world = ContagionTestWorld.BondedRoommates();
        var system = world.System(catalog);

        // Both sources fire the same day; neither may amplify off the other's
        // commit within the same tick (deltas buffered, then committed).
        system.StartContagionEvent("contagion_test_grief", "s_a", 1);
        system.StartContagionEvent("contagion_test_scare", "s_b", 1);
        system.EvaluateDailyContagion(1);

        var summaryC = system.GetInfluenceSummary("s_c");
        Assert.Equal(2, summaryC.Influences.Count);

        var world2 = ContagionTestWorld.BondedRoommates();
        var system2 = world2.System(catalog);
        system2.StartContagionEvent("contagion_test_grief", "s_a", 1);
        system2.StartContagionEvent("contagion_test_scare", "s_b", 1);
        system2.EvaluateDailyContagion(1);
        Assert.Equal(world.Morale, world2.Morale);
    }

    // ------------------------------------------------------------ breakdown

    [Fact]
    public void Breakdown_FiresOnce_PerCrossing_AndRearmsAfterLeaving()
    {
        var catalog = ContagionTestWorld.Catalog();
        var world = ContagionTestWorld.BondedRoommates();
        var system = world.System(catalog);
        system.StartContagionEvent("contagion_test_grief", "s_a", 1);

        // Day 1: contagion tips s_b across the band (89.5 + ~1.5 pressure delta).
        world.Morale["s_b"] = 89.5f;
        system.EvaluateDailyContagion(1);
        Assert.Equal(1, world.BreakdownEvents.Count(e => e.SurvivorId == "s_b"));

        // Days 2-3: still in band — no re-fire.
        system.EvaluateDailyContagion(2);
        system.EvaluateDailyContagion(3);
        Assert.Equal(1, world.BreakdownEvents.Count(e => e.SurvivorId == "s_b"));

        // Day 4: the world heals s_b out of the band — transition re-arms.
        world.Morale["s_b"] = 40f;
        system.EvaluateDailyContagion(4);
        // Days 5-7: cooldown (last breakdown day 1, cooldown 7) still holds.
        world.Morale["s_b"] = 95f;
        system.EvaluateDailyContagion(5);
        Assert.Equal(1, world.BreakdownEvents.Count(e => e.SurvivorId == "s_b"));
        world.Morale["s_b"] = 40f;
        system.EvaluateDailyContagion(6);
        system.EvaluateDailyContagion(7);
        system.EvaluateDailyContagion(8);

        // Day 9: re-entry after cooldown — fires again.
        world.Morale["s_b"] = 95f;
        system.EvaluateDailyContagion(9);
        Assert.Equal(2, world.BreakdownEvents.Count(e => e.SurvivorId == "s_b"));
    }

    [Fact]
    public void Breakdown_DoesNotFire_InsideCooldown()
    {
        var catalog = ContagionTestWorld.Catalog();
        var world = ContagionTestWorld.BondedRoommates();
        var system = world.System(catalog);
        system.StartContagionEvent("contagion_test_grief", "s_a", 1);

        world.Morale["s_b"] = 89.5f;
        system.EvaluateDailyContagion(1);
        Assert.Equal(1, world.BreakdownEvents.Count(e => e.SurvivorId == "s_b"));

        // Heal out, re-enter next day (inside cooldown): suppressed.
        world.Morale["s_b"] = 30f;
        system.EvaluateDailyContagion(2);
        world.Morale["s_b"] = 95f;
        system.EvaluateDailyContagion(3);
        Assert.Equal(1, world.BreakdownEvents.Count(e => e.SurvivorId == "s_b"));
    }

    [Fact]
    public void Breakdown_RoutesThroughCanonicalCrisisPort()
    {
        var catalog = ContagionTestWorld.Catalog();
        var world = ContagionTestWorld.BondedRoommates();
        var system = world.System(catalog);
        system.StartContagionEvent("contagion_test_grief", "s_a", 1);
        world.Morale["s_b"] = 89.5f;
        system.EvaluateDailyContagion(1);

        Assert.Single(world.Breakdowns);
        Assert.Equal("s_b", world.Breakdowns[0].survivorId);
        Assert.True(world.Breakdowns[0].stress >= MoraleContagionSystem.DespairBreakdownMorale);
    }

    // ------------------------------------------------------------ isolation

    [Fact]
    public void SocialIsolation_UsesCanonicalAuthorities_CutsInfluence_ImposesCost()
    {
        var catalog = ContagionTestWorld.Catalog();
        var world = ContagionTestWorld.BondedRoommates();
        world.DutyRole["s_b"] = "mess";
        var system = world.System(catalog);
        system.StartContagionEvent("contagion_test_grief", "s_a", 1);

        Assert.True(system.TryApplySocialIsolation("s_b", 1, durationDays: 3));
        Assert.Single(world.Unassigned);       // room authority called
        Assert.Equal("s_b", world.Unassigned[0].survivorId);
        Assert.Contains("s_b", world.ClearedRoles); // roster authority called

        float before = world.Morale["s_b"];
        system.EvaluateDailyContagion(1);

        // Isolated survivor receives no despair — only the isolation cost.
        Assert.Equal(before + MoraleContagionSystem.IsolationCostMoralePerDay, world.Morale["s_b"], 4);
        var summary = system.GetInfluenceSummary("s_b");
        Assert.Empty(summary.Influences);
        Assert.True(summary.IsIsolated);

        // And an isolated SOURCE spreads nothing either (the curtain works both ways).
        var world2 = ContagionTestWorld.BondedRoommates();
        var system2 = world2.System(catalog);
        system2.TryApplySocialIsolation("s_a", 1, 3);
        system2.StartContagionEvent("contagion_test_grief", "s_a", 1);
        system2.EvaluateDailyContagion(1);
        Assert.Equal(50f, world2.Morale["s_b"]);
    }

    [Fact]
    public void Isolation_Expires_AndRefusesDoubleIsolation()
    {
        var catalog = ContagionTestWorld.Catalog();
        var world = new ContagionTestWorld();
        var system = world.System(catalog);

        Assert.True(system.TryApplySocialIsolation("s_b", 1, 2));
        Assert.False(system.TryApplySocialIsolation("s_b", 2, 2)); // already isolated
        Assert.True(system.EndSocialIsolation("s_b", 5));          // day 5 > ends day 3
        Assert.False(system.GetInfluenceSummary("s_b").IsIsolated);
        Assert.False(system.TryApplySocialIsolation("s_nobody", 5, 2)); // unknown survivor
        Assert.False(system.TryApplySocialIsolation("s_c", 5, 0));      // invalid duration
    }

    // ----------------------------------------------------------- HopeBeacon

    [Fact]
    public void HopeBeacon_Counters_Despair()
    {
        var catalog = ContagionTestWorld.Catalog();
        var worldWith = ContagionTestWorld.BondedRoommates();
        worldWith.BeaconActive = true;
        var worldWithout = ContagionTestWorld.BondedRoommates();

        var withBeacon = worldWith.System(catalog);
        var withoutBeacon = worldWithout.System(catalog);
        withBeacon.StartContagionEvent("contagion_test_grief", "s_a", 1);
        withoutBeacon.StartContagionEvent("contagion_test_grief", "s_a", 1);

        for (int day = 1; day <= 4; day++)
        {
            withBeacon.EvaluateDailyContagion(day);
            withoutBeacon.EvaluateDailyContagion(day);
        }

        Assert.True(worldWith.Morale["s_b"] < worldWithout.Morale["s_b"],
            "beacon hope must measurably counter despair");

        // Beacon down: the standing source decays out instead of persisting forever.
        worldWith.BeaconActive = false;
        var pressuresBefore = withBeacon.CaptureState().survivors.Sum(s => s.hopePressure);
        for (int day = 5; day <= 9; day++) withBeacon.EvaluateDailyContagion(day);
        var pressuresAfter = withBeacon.CaptureState().survivors.Sum(s => s.hopePressure);
        Assert.True(pressuresAfter < pressuresBefore, "hope pressure decays once the beacon stops");
    }

    // --------------------------------------------------------------- schism

    [Fact]
    public void Schism_RequiresSustainedMajorityDespair_AndFiresOncePerQualifyingTransition()
    {
        var catalog = ContagionTestWorld.Catalog();
        var world = new ContagionTestWorld();
        world.DutyRole["s_a"] = "mess";
        world.DutyRole["s_b"] = "night_watch";
        world.DutyRole["s_c"] = "night_watch";
        var system = world.System(catalog);

        // Both night-watch members carry despair pressure above the member
        // threshold; 1.0 survives three days of 0.8 decay (0.8/0.64/0.512).
        system.RestoreState(new MoraleContagionState
        {
            survivors =
            {
                new SurvivorContagionPressureState { survivorId = "s_b", despairPressure = 1.0f },
                new SurvivorContagionPressureState { survivorId = "s_c", despairPressure = 1.0f }
            }
        });

        // Days 1-2 build the sustained-pressure ledger; day 3 crosses the threshold.
        system.EvaluateDailyContagion(1);
        system.EvaluateDailyContagion(2);
        Assert.Empty(world.SchismEvents);
        system.EvaluateDailyContagion(3);

        Assert.Single(world.SchismEvents);
        var schism = world.SchismEvents[0];
        Assert.Equal("night_watch", schism.SubgroupId);
        Assert.Equal(2, schism.MemberCount);
        Assert.Equal(2, schism.AffectedCount);
        Assert.Equal(3, schism.TriggerDay);

        // Cooldown blocks an immediate second schism even under fresh pressure.
        system.EvaluateDailyContagion(4);
        system.EvaluateDailyContagion(5);
        system.EvaluateDailyContagion(6);
        Assert.Single(world.SchismEvents);
    }

    [Fact]
    public void Schism_IgnoresSubgroupsBelowMinimumSize_AndResetsWhenPressureDrops()
    {
        var catalog = ContagionTestWorld.Catalog();
        var world = new ContagionTestWorld();
        world.DutyRole["s_b"] = "solo_crew";
        var system = world.System(catalog);

        // Subgroup of one: never eligible regardless of pressure.
        system.RestoreState(new MoraleContagionState
        {
            survivors =
            {
                new SurvivorContagionPressureState { survivorId = "s_b", despairPressure = 0.9f }
            }
        });
        system.EvaluateDailyContagion(1);
        system.EvaluateDailyContagion(2);
        system.EvaluateDailyContagion(3);
        Assert.Empty(world.SchismEvents);

        // A real trio sustains for two days, then one member recovers:
        // the majority fraction drops and the ledger resets — no schism.
        world.DutyRole["s_a"] = "trio_crew";
        world.DutyRole["s_b"] = "trio_crew";
        world.DutyRole["s_c"] = "trio_crew";
        system.RestoreState(new MoraleContagionState
        {
            survivors =
            {
                new SurvivorContagionPressureState { survivorId = "s_a", despairPressure = 0.9f },
                new SurvivorContagionPressureState { survivorId = "s_b", despairPressure = 0.9f },
                new SurvivorContagionPressureState { survivorId = "s_c", despairPressure = 0.9f }
            }
        });
        system.EvaluateDailyContagion(1);
        system.EvaluateDailyContagion(2);
        Assert.Empty(world.SchismEvents);

        system.RestoreState(new MoraleContagionState
        {
            survivors =
            {
                new SurvivorContagionPressureState { survivorId = "s_a", despairPressure = 0.9f },
                new SurvivorContagionPressureState { survivorId = "s_b", despairPressure = 0.9f },
                new SurvivorContagionPressureState { survivorId = "s_c", despairPressure = 0.1f }
            }
        });
        system.EvaluateDailyContagion(3);
        system.EvaluateDailyContagion(4);
        system.EvaluateDailyContagion(5);
        Assert.Empty(world.SchismEvents);
    }

    // -------------------------------------------------------- event + save

    [Fact]
    public void StartContagionEvent_ValidatesIds_Survivors_AndDoesNotStack()
    {
        var catalog = ContagionTestWorld.Catalog();
        var world = new ContagionTestWorld();
        var system = world.System(catalog);

        Assert.False(system.StartContagionEvent("contagion_unknown", "s_a", 1));
        Assert.False(system.StartContagionEvent("contagion_test_grief", "s_ghost", 1));
        Assert.True(system.StartContagionEvent("contagion_test_grief", "s_a", 1));
        Assert.True(system.StartContagionEvent("contagion_test_grief", "s_a", 3)); // idempotent refresh
        Assert.Single(system.CaptureState().activeSources);

        // Ambient sources (no named origin) are legal.
        Assert.True(system.StartContagionEvent("contagion_test_scare", "", 1));
        Assert.Equal(2, system.CaptureState().activeSources.Count);
    }

    [Fact]
    public void SaveRoundTrip_PreservesActiveContagion_AndContinuesIdentically()
    {
        var catalog = ContagionTestWorld.Catalog();
        var world = ContagionTestWorld.BondedRoommates();
        var system = world.System(catalog);
        system.StartContagionEvent("contagion_test_grief", "s_a", 1);
        system.TryApplySocialIsolation("s_c", 1, 4);
        system.EvaluateDailyContagion(1);
        system.EvaluateDailyContagion(2);

        var json = MoraleContagionSaveCodec.Encode(
            MoraleContagionSaveCodec.ToSaveState(system.CaptureState()), new SystemTextJsonSerializer());
        Assert.False(string.IsNullOrWhiteSpace(json));
        Assert.True(MoraleContagionSaveCodec.TryDecode(json, new SystemTextJsonSerializer(), out var decoded));

        // Identical world snapshot for the restored instance.
        var world2 = world.Clone();
        var restored = new MoraleContagionSystem(catalog, world2.Ports());
        restored.RestoreState(MoraleContagionSaveCodec.FromSaveState(decoded));

        system.EvaluateDailyContagion(3);
        restored.EvaluateDailyContagion(3);

        // Identical continuation traces (save-boundary determinism).
        Assert.Equal(world.Morale, world2.Morale);
        var a = system.CaptureState();
        var b = restored.CaptureState();
        Assert.Equal(a.activeSources.Count, b.activeSources.Count);
        Assert.Equal(a.survivors.Count, b.survivors.Count);
        for (int i = 0; i < a.survivors.Count; i++)
        {
            Assert.Equal(a.survivors[i].survivorId, b.survivors[i].survivorId);
            Assert.Equal(a.survivors[i].despairPressure, b.survivors[i].despairPressure, 5);
            Assert.Equal(a.survivors[i].isolationEndsDay, b.survivors[i].isolationEndsDay);
        }
    }

    [Fact]
    public void Restore_IsNonOperative_NoPropagation_NoEvents()
    {
        var catalog = ContagionTestWorld.Catalog();
        var world = ContagionTestWorld.BondedRoommates();
        var donor = world.System(catalog);
        donor.StartContagionEvent("contagion_test_grief", "s_a", 1);
        donor.EvaluateDailyContagion(1);
        var snapshot = donor.CaptureState();

        int moraleChanges = 0;
        var ports = world.Ports();
        var originalApply = ports.ApplyMoraleDelta;
        ports.ApplyMoraleDelta = (id, delta) => { moraleChanges++; originalApply(id, delta); };

        var fresh = new MoraleContagionSystem(catalog, ports);
        int schisms = 0, breakdowns = 0;
        fresh.OnMoraleSchismTriggered += _ => schisms++;
        fresh.OnMoraleBreakdown += _ => breakdowns++;

        float before = world.Morale["s_b"];
        fresh.RestoreState(snapshot);
        Assert.Equal(0, moraleChanges);
        Assert.Equal(0, schisms);
        Assert.Equal(0, breakdowns);
        Assert.Equal(before, world.Morale["s_b"]); // untouched

        // Restored influence state is intact but passive until the next evaluation.
        Assert.Single(fresh.GetInfluenceSummary("s_b").Influences);
    }

    [Fact]
    public void SaveCodec_RejectsTamperedAndChecksumlessPayloads()
    {
        var world = new ContagionTestWorld();
        var system = world.System();
        system.StartContagionEvent("contagion_test_grief", "s_a", 1);
        var json = MoraleContagionSaveCodec.Encode(
            MoraleContagionSaveCodec.ToSaveState(system.CaptureState()), new SystemTextJsonSerializer());

        // Tampered content must fail the checksum.
        var tampered = json.Replace("\"lastSchismDay\":-1", "\"lastSchismDay\":99");
        if (tampered != json)
            Assert.False(MoraleContagionSaveCodec.TryDecode(tampered, new SystemTextJsonSerializer(), out _));

        // A new-format payload without a checksum is malformed, not legacy.
        var stripped = System.Text.RegularExpressions.Regex.Replace(json, "\"Checksum\":\"[^\"]*\"", "\"Checksum\":\"\"");
        Assert.False(MoraleContagionSaveCodec.TryDecode(stripped, new SystemTextJsonSerializer(), out _));
    }

    [Fact]
    public void RealCatalog_SourcesResolveThroughTheSystem()
    {
        var catalog = LoadRealCatalog();
        Assert.True(catalog.contagion_events.Count >= 6);
        var world = new ContagionTestWorld();
        var system = world.System(catalog);

        foreach (var def in catalog.contagion_events)
            Assert.True(system.StartContagionEvent(def.id, "", 1),
                $"authored event '{def.id}' must instantiate");
        Assert.Equal(catalog.contagion_events.Count, system.CaptureState().activeSources.Count);
    }
}
