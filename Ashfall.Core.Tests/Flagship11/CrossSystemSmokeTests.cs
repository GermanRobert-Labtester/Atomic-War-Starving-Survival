using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Flagship11;

/// <summary>
/// Flagship XI cross-system smoke (plan §Cross-System Smoke Scenario, compact
/// deterministic form): an outbreak scares the holdfast, quarantine breeds
/// resentment, contagion spreads both through bonds, and a save/restore
/// boundary replays identically. The links are the same typed source-event
/// calls the host raises — no direct cross-system state mutation.
/// </summary>
public class CrossSystemSmokeTests
{
    private const string Strain = "pathogen_test_variant_a";

    private sealed class World
    {
        public List<string> Alive = new() { "s_patient", "s_kin", "s_afar" };
        public Dictionary<string, string> Rooms = new()
        {
            ["s_patient"] = "ward", ["s_kin"] = "ward"
        };
        public Dictionary<(string, string), float> Bonds = new()
        {
            [("s_patient", "s_kin")] = 0.9f
        };

        public NeedsSystem Needs()
        {
            var needs = new NeedsSystem();
            foreach (var id in Alive)
                needs.Register(new SurvivorNeedsState { Id = id });
            return needs;
        }

        public MoraleContagionPorts Ports(NeedsSystem needs) => new()
        {
            AliveSurvivors = () => Alive,
            GetMorale = id => needs.Get(id)?.Morale ?? 50f,
            ApplyMoraleDelta = (id, delta) => needs.Modify(id, NeedKind.Morale, delta),
            AreInSameRoom = (a, b) =>
                Rooms.TryGetValue(a, out var ra) && Rooms.TryGetValue(b, out var rb) && ra == rb,
            GetDutyRole = _ => string.Empty,
            GetBondStrength = (a, b) => Bonds.TryGetValue((a, b), out var v) ? v : 0f,
            IsHopeBeaconActive = () => false
        };
    }

    private static PathogenStrainCatalogContainer Strains() => new()
    {
        pathogen_strains =
        {
            new PathogenStrainDef
            {
                id = Strain, display_name = "Test Strain", strain_of = "disease_dry_bunker_hiss",
                incubation_days = 1, illness_days = 4, lethality = 0.2f, infectivity = 0.6f
            }
        }
    };

    private static string DataDir => Flagship11TestBase.FindDataDirectory();

    private static DiseaseSystem NewDisease() =>
        new DiseaseSystem(rng: new SeededRng(31));

    private static void BindDiseaseCatalog(DiseaseSystem disease) =>
        disease.BindCatalog(DiseaseCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer()));

    private static MoraleContagionSystem NewContagion(World world, NeedsSystem needs) =>
        new MoraleContagionSystem(
            ContagionEventCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer()),
            world.Ports(needs));

    private static string Run(int? saveAtDay)
    {
        World world = new World();
        var needs = world.Needs();
        var disease = NewDisease();
        BindDiseaseCatalog(disease);
        var strains = new PathogenStrainSystem(Strains(), disease);
        strains.AttachStrains();
        var contagion = NewContagion(world, needs);

        // The host's cross-system links, expressed as the typed source calls.
        disease.OnOutbreakDeclared += _ =>
            contagion.StartContagionEvent("contagion_outbreak_fear", string.Empty, 0);
        disease.OnQuarantineStarted += (survivorId, _) =>
            contagion.StartContagionEvent("contagion_quarantine_resentment", survivorId, 0);
        disease.Infect("s_patient", Strain, 1);

        if (saveAtDay != null)
        {
            // Checkpoint run to the boundary, then restore all three sections
            // into fresh instances and continue from there.
            for (int d = 1; d < saveAtDay.Value; d++)
            {
                disease.TickDaily(d, world.Alive);
                strains.TickMutations(d);
                contagion.EvaluateDailyContagion(d);
            }
            var diseaseJson = new SystemTextJsonSerializer().Serialize(disease.CaptureState());
            var contagionJson = MoraleContagionSaveCodec.Encode(
                MoraleContagionSaveCodec.ToSaveState(contagion.CaptureState()), new SystemTextJsonSerializer());
            // The host's survivors save section owns morale; mirror it here.
            var moraleSnapshot = world.Alive.ToDictionary(
                id => id, id => needs.Get(id)!.Morale);

            world = new World();
            needs = world.Needs();
            foreach (var pair in moraleSnapshot)
                needs.Modify(pair.Key, NeedKind.Morale, pair.Value - 50f);
            disease = new DiseaseSystem();
            disease.RestoreState(new SystemTextJsonSerializer()
                .Deserialize<DiseaseSystemState>(diseaseJson)!);
            BindDiseaseCatalog(disease);
            strains = new PathogenStrainSystem(Strains(), disease);
            strains.AttachStrains();
            contagion = NewContagion(world, needs);
            contagion.RestoreState(MoraleContagionSaveCodec.FromSaveState(
                MoraleContagionSaveCodec.TryDecode(contagionJson, new SystemTextJsonSerializer(), out var cs)
                    ? cs!
                    : new MoraleContagionSaveState()));
            // re-wire the cross-system links on the fresh instances
            disease.OnOutbreakDeclared += _ =>
                contagion.StartContagionEvent("contagion_outbreak_fear", string.Empty, 0);
            disease.OnQuarantineStarted += (survivorId, _) =>
                contagion.StartContagionEvent("contagion_quarantine_resentment", survivorId, 0);
        }

        var startedDay = saveAtDay ?? 1;
        for (int day = startedDay; day <= 12; day++)
        {
            disease.TickDaily(day, world.Alive);
            strains.TickMutations(day);
            contagion.EvaluateDailyContagion(day);
        }

        var traceLines = new List<string>();
        foreach (var id in world.Alive.OrderBy(x => x, StringComparer.Ordinal))
        {
            var s = contagion.GetInfluenceSummary(id);
            traceLines.Add($"{id}:m{needs.Get(id)!.Morale:F3}:d{s.DespairPressure:F3}:p{s.PanicPressure:F3}");
        }
        var entry = disease.GetDiseaseState(Strain);
        traceLines.Add("strain:" + (entry == null
            ? "-"
            : string.Join(",", entry.infected
                .OrderBy(i => i.survivor_id, StringComparer.Ordinal)
                .Select(i => i.survivor_id + "@" + i.infected_day + "+" + i.days_sick + (i.quarantined ? "Q" : "")))));
        return string.Join("\n", traceLines);
    }

    [Fact]
    public void Outbreak_SpreadsFearAndResentment_ThroughTheCrossSystemLinks()
    {
        World world = new World();
        var needs = world.Needs();
        var disease = NewDisease();
        BindDiseaseCatalog(disease);
        var strains = new PathogenStrainSystem(Strains(), disease);
        strains.AttachStrains();
        var contagion = NewContagion(world, needs);

        int outbreakDays = 0;
        int today = 1; // the host passes the current sim day; tests track it here
        disease.OnOutbreakDeclared += _ =>
        {
            outbreakDays++;
            contagion.StartContagionEvent("contagion_outbreak_fear", string.Empty, today);
        };
        disease.OnQuarantineStarted += (survivorId, _) =>
            contagion.StartContagionEvent("contagion_quarantine_resentment", survivorId, today);

        // Outbreak threshold is 3 active infections; seed three.
        disease.Infect("s_patient", Strain, 1);
        disease.Infect("s_afar", Strain, 1);
        disease.Infect("s_kin", Strain, 1);
        disease.TickDaily(1, world.Alive);
        Assert.Equal(1, outbreakDays);
        Assert.NotEmpty(contagion.GetInfluenceSummary("s_afar").Influences);

        // Days pass; the fear source reaches everyone at settlement baseline.
        for (int day = 2; day <= 3; day++)
        {
            disease.TickDaily(day, world.Alive);
            strains.TickMutations(day);
            contagion.EvaluateDailyContagion(day);
        }
        Assert.True(contagion.GetInfluenceSummary("s_afar").PanicPressure > 0f,
            "outbreak fear must reach the whole holdfast");

        // Quarantine closes the ward: resentment reaches those shut outside.
        today = 4;
        disease.Quarantine("s_patient", Strain);
        contagion.EvaluateDailyContagion(4);
        Assert.True(contagion.GetInfluenceSummary("s_kin").DespairPressure > 0f,
            "quarantine resentment must accrue on the bonded ward-mate");
    }

    [Fact]
    public void OutbreakCrossSystem_SaveBoundary_ReplaysIdentically()
    {
        var fresh = Run(saveAtDay: null);
        Assert.Equal(fresh, Run(saveAtDay: null));      // same seed, identical run
        Assert.Equal(fresh, Run(saveAtDay: 5));         // boundary at day 5 lands identically
        Assert.Equal(fresh, Run(saveAtDay: 9));         // and at day 9
    }
}
