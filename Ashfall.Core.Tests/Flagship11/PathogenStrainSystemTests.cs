using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Xunit;
using PathogenStrainSystem = Ashfall.Core.Disease.PathogenStrainSystem;

namespace Ashfall.Core.Tests.Flagship11;

/// <summary>
/// Plan 155 behaviour matrix (amended architecture D2): strain merge into the
/// canonical engine, deterministic fictional mutation, abstract radiation
/// coupling, the fictional cure project, save round-trips, restore-is-non-
/// operative, and the 100-day deterministic replay.
/// </summary>
public class PathogenStrainSystemTests
{
    private const string ParentAir = "disease_dry_bunker_hiss";
    private const string ParentWater = "disease_cholera";

    private static PathogenStrainCatalogContainer SyntheticStrains()
    {
        var catalog = new PathogenStrainCatalogContainer();
        catalog.pathogen_strains.Add(new PathogenStrainDef
        {
            id = "pathogen_test_variant_a",
            display_name = "Test Variant A",
            strain_of = ParentAir,
            incubation_days = 1,
            illness_days = 5,
            lethality = 0.2f,
            infectivity = 0.6f,
            radiation_severity_gain = 0.5f,
            mutation_chance_per_day = 1f, // deterministic in tests: always mutates
            mutation_targets = { "pathogen_test_variant_b" }
        });
        catalog.pathogen_strains.Add(new PathogenStrainDef
        {
            id = "pathogen_test_variant_b",
            display_name = "Test Variant B",
            strain_of = ParentAir,
            incubation_days = 1,
            illness_days = 4,
            lethality = 0.35f,
            infectivity = 0.5f,
            radiation_severity_gain = 0.7f,
            mutation_chance_per_day = 0f,
            mutation_targets = { }
        });
        catalog.pathogen_strains.Add(new PathogenStrainDef
        {
            id = "pathogen_test_gut",
            display_name = "Test Gut Strain",
            strain_of = ParentWater,
            incubation_days = 2,
            illness_days = 5,
            lethality = 0.25f,
            infectivity = 0.5f,
            radiation_severity_gain = 0.2f,
            mutation_chance_per_day = 0f
        });
        return catalog;
    }

    private static (DiseaseSystem disease, PathogenStrainSystem strains) BuildWorld(
        int seed = 1013, PathogenStrainCatalogContainer? catalog = null)
    {
        var disease = new DiseaseSystem(rng: new SeededRng(seed));
        disease.BindCatalog(DiseaseCatalogLoader.Load(
            Flagship11TestBase.FindDataDirectory(), new FileSystemIO(), new SystemTextJsonSerializer()));
        var strains = new PathogenStrainSystem(catalog ?? SyntheticStrains(), disease);
        strains.AttachStrains();
        strains.BindEngineHooks();
        return (disease, strains);
    }

    // --------------------------------------------------------------- merge

    [Fact]
    public void AttachStrains_RegistersDerivedDefinitionsWithParentVector()
    {
        var (disease, strains) = BuildWorld();

        var variantA = disease.GetDefinition("pathogen_test_variant_a");
        Assert.NotNull(variantA);
        var parent = disease.GetDefinition(ParentAir);
        Assert.NotNull(parent);
        Assert.Equal(parent!.vector, variantA!.vector); // vector inherited from parent
        Assert.Equal(1, variantA.incubation_days);      // strain overrides apply
        Assert.Equal(0.2f, variantA.lethality, 4);
        Assert.Contains("A strain of", variantA.tell);  // provenance in clinical prose
        Assert.Equal(parent.spread_interval_days, variantA.spread_interval_days);
        Assert.Equal(parent.countermeasure_item_id, variantA.countermeasure_item_id);

        // The engine runs strains natively: infection by strain id is accepted.
        disease.Infect("survivor_t1", "pathogen_test_variant_a", 1);
        var entry = disease.GetDiseaseState("pathogen_test_variant_a");
        Assert.NotNull(entry);
        Assert.Single(entry!.infected);

        // Authored diseases remain untouched.
        Assert.NotNull(disease.GetDefinition(ParentWater));
        Assert.Equal(16, CountAuthoredDiseases(disease));
    }

    [Fact]
    public void AttachStrains_IsIdempotent_AndRejectsUnknownParents()
    {
        var catalog = SyntheticStrains();
        catalog.pathogen_strains.Add(new PathogenStrainDef
        {
            id = "pathogen_test_orphan",
            display_name = "Orphan",
            strain_of = "disease_does_not_exist"
        });

        var (disease, strains) = BuildWorld(catalog: catalog);
        Assert.False(strains.AttachStrains()); // orphan reported, valid strains still attached
        Assert.NotNull(disease.GetDefinition("pathogen_test_variant_a"));
        Assert.Null(disease.GetDefinition("pathogen_test_orphan"));

        // Repeated attach is idempotent: valid strains stay present; the orphan
        // never resolves (its parent does not exist) so false is honest.
        Assert.False(strains.AttachStrains());
        Assert.NotNull(disease.GetDefinition("pathogen_test_variant_a"));
        Assert.NotNull(disease.GetDefinition("pathogen_test_variant_b"));
        Assert.NotNull(disease.GetDefinition("pathogen_test_gut"));
    }

    // ------------------------------------------------------------ mutation

    [Fact]
    public void MutationChance_ScalesWithRadiationAndClamps()
    {
        var strain = new PathogenStrainDef
        {
            mutation_chance_per_day = 0.04f,
            radiation_severity_gain = 0.5f
        };

        Assert.Equal(0.04f, PathogenStrainSystem.MutationChance(strain, 0f), 4);
        Assert.True(PathogenStrainSystem.MutationChance(strain, 50f) > 0.04f, "dose raises mutation pressure");
        Assert.True(PathogenStrainSystem.MutationChance(strain, 100f) > PathogenStrainSystem.MutationChance(strain, 50f));
        Assert.True(PathogenStrainSystem.MutationChance(strain, 100f) <= 1f, "clamped to 0..1");
        Assert.Equal(0f, PathogenStrainSystem.MutationChance(null!, 50f), 0);
    }

    [Fact]
    public void TickMutations_TransitionsDeterministically_AndPreservesHistory()
    {
        var (disease, strains) = BuildWorld();
        disease.Infect("survivor_m1", "pathogen_test_variant_a", 3);
        var before = disease.GetDiseaseState("pathogen_test_variant_a")!.infected[0];
        before.days_sick = 2;
        before.quarantined = true;

        strains.TickMutations(7);

        Assert.Empty(disease.GetDiseaseState("pathogen_test_variant_a")!.infected);
        var after = disease.GetDiseaseState("pathogen_test_variant_b")!.infected.Single();
        Assert.Equal("survivor_m1", after.survivor_id);
        Assert.Equal(3, after.infected_day);      // clinical history preserved
        Assert.Equal(2, after.days_sick);
        Assert.True(after.quarantined);           // quarantine rides the transition

        // Deterministic repeat from a fresh world.
        var (disease2, strains2) = BuildWorld(seed: 1013);
        disease2.Infect("survivor_m1", "pathogen_test_variant_a", 3);
        var b2 = disease2.GetDiseaseState("pathogen_test_variant_a")!.infected[0];
        b2.days_sick = 2;
        b2.quarantined = true;
        strains2.TickMutations(7);
        Assert.Single(disease2.GetDiseaseState("pathogen_test_variant_b")!.infected);
    }

    [Fact]
    public void TickMutations_ZeroChance_NeverMutates()
    {
        var (disease, strains) = BuildWorld();
        disease.Infect("survivor_m2", "pathogen_test_variant_b", 1); // chance 0
        disease.Infect("survivor_m3", "pathogen_test_gut", 1);       // chance 0

        for (int day = 1; day <= 5; day++) strains.TickMutations(day);

        Assert.Single(disease.GetDiseaseState("pathogen_test_variant_b")!.infected);
        Assert.Single(disease.GetDiseaseState("pathogen_test_gut")!.infected);
    }

    // ----------------------------------------------------------- radiation

    [Fact]
    public void RadiationCouplesThroughReadOnlyQuery()
    {
        var (disease, strains) = BuildWorld();
        float dose = 0f;
        strains.RadiationDoseQuery = _ => dose;

        Assert.Equal(0f, strains.RadiationSeverityPressure("s", "pathogen_test_variant_b"), 4);
        dose = 80f;
        var pressure = strains.RadiationSeverityPressure("s", "pathogen_test_variant_b");
        Assert.True(pressure > 0f && pressure <= PathogenStrainSystem.MaxRadiationLethalityPressure + 0.0001f,
            "dose maps into a bounded abstract severity pressure");
        Assert.Equal(0f, strains.RadiationSeverityPressure("s", ParentWater), 4); // authored diseases untouched

        // The engine hook is bound and reads the same numbers.
        Assert.NotNull(disease.EffectiveLethalityModifier);
        Assert.Equal(pressure, disease.EffectiveLethalityModifier!("s", "pathogen_test_variant_b"), 4);
    }

    // ---------------------------------------------------------------- cure

    [Fact]
    public void CureProject_ValidatesAdvancesAndUnlocks()
    {
        var (_, strains) = BuildWorld();

        Assert.False(strains.StartCureProject("pathogen_unknown", 1));
        Assert.True(strains.StartCureProject("pathogen_test_variant_a", 1));
        Assert.False(strains.StartCureProject("pathogen_test_variant_a", 2)); // in progress
        Assert.False(strains.IsCureUnlocked("pathogen_test_variant_a"));

        for (int day = 1; day < PathogenStrainSystem.DefaultCureDays; day++)
            Assert.False(strains.AdvanceCureProjects(day));
        Assert.True(strains.AdvanceCureProjects(99)); // completion day
        Assert.True(strains.IsCureUnlocked("pathogen_test_variant_a"));
        Assert.False(strains.StartCureProject("pathogen_test_variant_a", 100)); // unlocked
    }

    [Fact]
    public void CureProject_SaveRoundTripAndRestoreIsNonOperative()
    {
        var (_, strains) = BuildWorld();
        strains.StartCureProject("pathogen_test_variant_a", 1);
        strains.AdvanceCureProjects(2);
        strains.AdvanceCureProjects(3);

        var json = PathogenStrainSaveCodec.Encode(
            PathogenStrainSaveCodec.ToSaveState(strains.CaptureState()), new SystemTextJsonSerializer());
        Assert.True(PathogenStrainSaveCodec.TryDecode(json, new SystemTextJsonSerializer(), out var decoded));

        var restored = new PathogenStrainSystem(SyntheticStrains(),
            new DiseaseSystem(rng: new SeededRng(7)));
        int events = 0;
        restored.OnStrainMutation += (_, _, _) => events++;
        restored.RestoreState(PathogenStrainSaveCodec.FromSaveState(decoded));

        // Restore reconstructs state but advances nothing (non-operative).
        Assert.Equal(0, events);
        var project = restored.CureProjects.Single();
        Assert.Equal("pathogen_test_variant_a", project.strainId);
        Assert.Equal(2, project.daysInvested);
        Assert.False(project.complete);
        Assert.False(restored.IsCureUnlocked("pathogen_test_variant_a"));

        // Tamper rejection.
        var stripped = System.Text.RegularExpressions.Regex.Replace(json, "\"Checksum\":\"[^\"]*\"", "\"Checksum\":\"\"");
        Assert.False(PathogenStrainSaveCodec.TryDecode(stripped, new SystemTextJsonSerializer(), out _));
    }

    // ------------------------------------------------- deterministic replay

    [Fact]
    public void HundredDayReplay_IsDeterministic_AndSurvivesSaveBoundary()
    {
        var traceA = RunReplay(saveAtDay: null);
        var traceB = RunReplay(saveAtDay: null);
        Assert.Equal(traceA, traceB); // same seed ⇒ identical 100-day trace

        var traceSaved = RunReplay(saveAtDay: 40);
        Assert.Equal(traceA, traceSaved); // save/restore at day 40 lands on the same trace
    }

    private static string RunReplay(int? saveAtDay)
    {
        var trace = new List<string>();
        DiseaseSystem disease;
        PathogenStrainSystem strains;
        if (saveAtDay == null)
        {
            (disease, strains) = BuildWorld(seed: 20260);
            disease.Infect("survivor_r1", "pathogen_test_variant_a", 1);
            disease.Infect("survivor_r2", "pathogen_test_gut", 3);
            strains.OnStrainMutation += (survivorId, fromId, toId) =>
                trace.Add($"mut:{survivorId}:{fromId}->{toId}");
        }
        else
        {
            // Checkpoint run: record the pre-boundary trace, then restore and continue.
            var (a, b) = BuildWorld(seed: 20260);
            a.Infect("survivor_r1", "pathogen_test_variant_a", 1);
            a.Infect("survivor_r2", "pathogen_test_gut", 3);
            b.OnStrainMutation += (survivorId, fromId, toId) =>
                trace.Add($"mut:{survivorId}:{fromId}->{toId}");
            for (int day = 1; day < saveAtDay.Value; day++)
            {
                a.TickDaily(day, Candidates());
                b.TickMutations(day);
                b.AdvanceCureProjects(day);
            }
            var engineJson = new SystemTextJsonSerializer()
                .Serialize(a.CaptureState());
            var strainJson = PathogenStrainSaveCodec.Encode(
                PathogenStrainSaveCodec.ToSaveState(b.CaptureState()), new SystemTextJsonSerializer());

            var restoredEngine = new DiseaseSystem();
            restoredEngine.RestoreState(new SystemTextJsonSerializer()
                .Deserialize<DiseaseSystemState>(engineJson)!);
            restoredEngine.BindCatalog(DiseaseCatalogLoader.Load(
                Flagship11TestBase.FindDataDirectory(), new FileSystemIO(), new SystemTextJsonSerializer()));
            strains = new PathogenStrainSystem(SyntheticStrains(), restoredEngine);
            strains.AttachStrains();
            strains.BindEngineHooks();
            strains.RestoreState(PathogenStrainSaveCodec.FromSaveState(
                PathogenStrainSaveCodec.TryDecode(strainJson, new SystemTextJsonSerializer(), out var s) ? s! : new PathogenStrainSaveState()));
            strains.OnStrainMutation += (survivorId, fromId, toId) =>
                trace.Add($"mut:{survivorId}:{fromId}->{toId}");
            disease = restoredEngine;
        }

        for (int day = saveAtDay ?? 1; day <= 100; day++)
        {
            disease.TickDaily(day, Candidates());
            strains.TickMutations(day);
            strains.AdvanceCureProjects(day);
        }

        // Final state fingerprint: per-disease survivor lists + outcome counters.
        foreach (var id in new[]
                 {
                     "pathogen_test_variant_a", "pathogen_test_variant_b", "pathogen_test_gut",
                     "pathogen_ash_fever", "pathogen_red_lung", "pathogen_frost_rot", "pathogen_glass_cough"
                 })
        {
            var entry = disease.GetDiseaseState(id);
            if (entry == null) { trace.Add(id + ":-"); continue; }
            trace.Add(id + ":" + string.Join(",", entry.infected
                .OrderBy(i => i.survivor_id, StringComparer.Ordinal)
                .Select(i => i.survivor_id + "@" + i.infected_day + "+" + i.days_sick)) +
                "|d" + entry.deaths_total + "|r" + entry.recovered_total);
        }
        return string.Join("\n", trace);
    }

    private static IReadOnlyList<string> Candidates() =>
        new[] { "survivor_r1", "survivor_r2", "survivor_r3", "survivor_r4", "survivor_r5", "survivor_r6" };

    private static int CountAuthoredDiseases(DiseaseSystem disease)
    {
        int count = 0;
        foreach (var id in new[]
                 {
                     "disease_cholera", "disease_zoonotic_flu", "disease_blood_fever", "disease_spore_blight",
                     "disease_acute_radiation_syndrome", "disease_fungal_respiratory", "disease_typhoid_waterborne",
                     "disease_wellspring_cramps", "disease_silt_jaundice", "disease_condemned_air_cough",
                     "disease_dry_bunker_hiss", "disease_septic_rust_wound_fever", "disease_reused_needle_fever",
                     "disease_deep_excavation_mold_lung", "disease_silo_lung", "disease_prion_tremor"
                 })
            if (disease.GetDefinition(id) != null) count++;
        return count;
    }
}
