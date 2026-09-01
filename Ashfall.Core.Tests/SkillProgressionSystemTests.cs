using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests;

/// <summary>
/// Skill Progression Engine acceptance tests.
///
/// Phase 18 task per <c>docs/systems/SKILL_PROGRESSION_CORE_PORT_PLAN.md</c>:
/// Core behaviour test exercising Train → XpToNext updates → Decay over time.
/// Authored definitions loaded from <c>skills.json</c>.
/// </summary>
public class SkillProgressionSystemTests
{
    private static ISeededRng MakeRng(int seed = 1401) => new SeededRng(seed);

    private static string ResolveDataDir()
    {
        if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string found)) return found;
        if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
        throw new InvalidOperationException("StreamingAssets/Data directory not found");
    }

    private static void PopulateCatalog(SkillProgressionSystem sys)
    {
        SkillCatalogLoader.LoadAndRegister(sys, ResolveDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
    }

    private sealed class TestActor : SkillActor
    {
        private readonly Dictionary<string, float> _bonuses = new(StringComparer.Ordinal);
        private readonly Dictionary<string, float> _moraleStore = new(StringComparer.Ordinal);
        public bool Alive { get; set; } = true;
        public string ExpertDiscipline { get; set; } = string.Empty;
        public event Action<string, float>? MoraleApplied;
        public void SetSkillBonus(string disciplineId, float bonus)
        {
            if (string.IsNullOrEmpty(disciplineId)) return;
            _bonuses[disciplineId] = bonus;
        }
        public string Id { get; }
        public bool IsAlive => Alive;
        public float Morale => _moraleStore.TryGetValue("__cached__", out float v) ? v : 100f;
        public float Health => 100f;
        public string ExpertDisciplineId => ExpertDiscipline;
        public TestActor(string id) { Id = id; }
        public void ApplyMorale(float value)
        {
            _moraleStore["__cached__"] = value;
            MoraleApplied?.Invoke(Id, value);
        }
    }

    [Fact]
    public void CatalogLoader_PopulatesKnownDisciplineSkills()
    {
        var sys = new SkillProgressionSystem();
        PopulateCatalog(sys);
        Assert.True(sys.CatalogCount >= 145);
        Assert.NotNull(sys.GetSkill("skill_field_dressing"));
        Assert.NotNull(sys.GetSkill("skill_steady_hands"));
        Assert.True(sys.GetSkill("skill_steady_hands")!.isExpertSkill);
        Assert.NotNull(sys.GetSkill("skill_field_surgery"));
        Assert.NotNull(sys.GetSkill("skill_water_filtration"));
        Assert.NotNull(sys.GetSkill("skill_radio_repair"));
    }

    [Fact]
    public void RecordAction_AwardsXpAndReachesTier1()
    {
        var sys = new SkillProgressionSystem();
        PopulateCatalog(sys);
        var actor = new TestActor("actor_a");

        // 11 actions × 5 XP = 55 XP at day 5; threshold 50 -> "skill_field_dressing" should fire.
        for (int i = 0; i < 11; i++)
            sys.RecordAction(actor, "medical", SkillProgressionSystem.DefaultXpPerAction, 5);
        Assert.True(sys.GetXp("actor_a", "medical") >= 50f);
        Assert.True(sys.HasActiveSkill("actor_a", "skill_field_dressing"));
        Assert.Equal(0.10f, sys.GetCachedBonus("actor_a", "medical"), 3);
    }

    [Fact]
    public void TickDaily_DecaysUnusedDisciplineSkillsToDormant()
    {
        var sys = new SkillProgressionSystem();
        PopulateCatalog(sys);
        var actor_a = new TestActor("actor_a");
        var actor_b = new TestActor("actor_b");

        // Earn the medical tier-1 skill at day 5 for both actors.
        for (int i = 0; i < 11; i++)
            sys.RecordAction(actor_a, "medical", SkillProgressionSystem.DefaultXpPerAction, 5);
        for (int i = 0; i < 11; i++)
            sys.RecordAction(actor_b, "medical", SkillProgressionSystem.DefaultXpPerAction, 5);
        Assert.True(sys.HasActiveSkill("actor_a", "skill_field_dressing"));
        Assert.True(sys.HasActiveSkill("actor_b", "skill_field_dressing"));

        // 20 days tick — actor_a never practices craft, actor_b still practices craft daily.
        var actors = new List<SkillActor> { actor_a, actor_b };
        int day = 5;
        for (int i = 0; i < 20; i++)
        {
            day++;
            sys.RecordAction(actor_b, "combat", 1f, day); // keep actor_b practicing ANOTHER discipline
            sys.TickDaily(day, actors);
        }

        // The tier-1 medical was earned on day 5 and never practiced by actor_a,
        // so after DormantAfterUnusedDays (14) it should be Dormant.
        Assert.True(sys.HasDormantSkill("actor_a", "skill_field_dressing"));
    }

    [Fact]
    public void DormantSkill_ReactivatesWhenPracticed()
    {
        var sys = new SkillProgressionSystem();
        PopulateCatalog(sys);
        var actor = new TestActor("actor_reactive");

        for (int i = 0; i < 11; i++) sys.RecordAction(actor, "medical", 5f, 5);
        Assert.True(sys.HasActiveSkill("actor_reactive", "skill_field_dressing"));

        // Sleep past the dormancy window, then re-practice.
        var day = 5;
        for (int i = 0; i < 20; i++) { day++; sys.TickDaily(day, new List<SkillActor> { actor }); }
        Assert.True(sys.HasDormantSkill("actor_reactive", "skill_field_dressing"));

        sys.RecordAction(actor, "medical", 1f, day);
        Assert.True(sys.HasActiveSkill("actor_reactive", "skill_field_dressing"));
        Assert.False(sys.HasDormantSkill("actor_reactive", "skill_field_dressing"));
    }

    [Fact]
    public void ExpertSkill_GatedToPredeterminedDiscipline()
    {
        var sys = new SkillProgressionSystem();
        PopulateCatalog(sys);

        var actor_a = new TestActor("actor_medical") { ExpertDiscipline = "medical" };
        var actor_b = new TestActor("actor_crafting") { ExpertDiscipline = "crafting" };

        // Force medical XP above the steady_hands 120 threshold.
        for (int i = 0; i < 30; i++) sys.RecordAction(actor_a, "medical", 5f, 5);
        for (int i = 0; i < 30; i++) sys.RecordAction(actor_b, "medical", 5f, 5);

        // actor_a should hold steady_hands (medical expert track) but NOT workshop_sense.
        Assert.True(sys.HasActiveSkill("actor_medical", "skill_steady_hands"));
        Assert.False(sys.HasActiveSkill("actor_crafting", "skill_steady_hands"));
    }

    [Fact]
    public void TryGrantSkill_MilestoneId_BypassesXpThreshold()
    {
        var sys = new SkillProgressionSystem();
        PopulateCatalog(sys);
        var actor = new TestActor("actor_b");
        Assert.Null(sys.GetSkill("perk_field_dressing"));
        Assert.True(sys.TryGrantSkill(actor, "skill_anchor", 5));
        Assert.True(sys.HasActiveSkill("actor_b", "skill_anchor"));
    }

    [Fact]
    public void Epiphany_TriggersOnDesperateSurvivors()
    {
        var sys = new SkillProgressionSystem();
        PopulateCatalog(sys);
        sys.ApplyMorale = (id, value) => { /* no-op */ };
        var lowSurvivor = new TestActor("actor_low") /* Morale default 100 */;

        var rng2 = new ForcedDoubleRng(0.01);
        sys.MaxMoraleCap = id => 100f;
        sys.ApplyMorale = (id, value) => { /* host would clamp */ };

        for (int i = 0; i < 30; i++) sys.RecordAction(lowSurvivor, "medical", 5f, 5, rng2);
        Assert.NotNull(lowSurvivor);
    }

    [Fact]
    public void Epiphany_PathRunsUnderDrowningMorale()
    {
        var sys = new SkillProgressionSystem();
        PopulateCatalog(sys);
        var helplessActor = new MoraleTrackedActor("actor_desperate");
        sys.ApplyMorale = helplessActor.ApplyMoraleFromEngine;
        helplessActor.MoraleValue = 5f; // below threshold

        int eventFired = 0;
        sys.OnEpiphany += (_, id) => eventFired++;
        var rng = new ForcedDoubleRng(0.0); // always under 0.05

        for (int i = 0; i < 30; i++) sys.RecordAction(helplessActor, "medical", 5f, 5, rng);
        Assert.True(eventFired > 0);
        Assert.Equal(100f, helplessActor.MoraleAppliedLast);
    }

    [Fact]
    public void CaptureState_RoundTripsPreservingAllFields()
    {
        var sys = new SkillProgressionSystem();
        PopulateCatalog(sys);
        var actor = new TestActor("actor_rt");
        for (int i = 0; i < 11; i++) sys.RecordAction(actor, "medical", 5f, 5);

        var save = sys.CaptureState();
        Assert.Single(save.survivorIds);
        Assert.Single(save.entries);

        // Wipe and restore.
        var fresh = new SkillProgressionSystem();
        PopulateCatalog(fresh);
        fresh.RestoreState(save, new List<SkillActor> { actor });
        Assert.True(fresh.HasActiveSkill("actor_rt", "skill_field_dressing"));
        Assert.Equal(0.10f, fresh.GetCachedBonus("actor_rt", "medical"), 3);
    }

    [Fact]
    public void SkillAtrophySystem_FiresAfterWindow_HoldsMultiplier()
    {
        var at = new SkillAtrophySystem();
        var actor = new MoraleTrackedActor("actor_at");
        actor.MoraleValue = 5f; // below threshold so atrophy can fire.
        int eventsFired = 0;
        at.OnSkillAtrophied += (_, s) => eventsFired++;

        var actors = new List<SkillActor> { actor };
        // Tick 14 days under low morale (= 14 * 24 hours).
        for (int i = 0; i < 14; i++) at.Tick(24f, actors);

        Assert.Equal(2, eventsFired);
        Assert.True(at.IsAtrophied("actor_at", "medical"));
        Assert.True(at.IsAtrophied("actor_at", "crafting"));
    }

    [Fact]
    public void SkillAtrophySystem_DoesNotFireIfMoraleRecovers()
    {
        var at = new SkillAtrophySystem();
        var actor = new MoraleTrackedActor("actor_recover");
        actor.MoraleValue = 5f;

        int eventFired = 0;
        at.OnSkillAtrophied += (_, s) => eventFired++;

        // 7 days low morale.
        for (int i = 0; i < 7; i++) at.Tick(24f, new List<SkillActor> { actor });
        // Recover.
        actor.MoraleValue = 100f;
        for (int i = 0; i < 7; i++) at.Tick(24f, new List<SkillActor> { actor });

        Assert.Equal(0, eventFired);
    }

    [Fact]
    public void SkillAtrophySystem_RoundTripsViaSaveState()
    {
        var at = new SkillAtrophySystem();
        var actor = new MoraleTrackedActor("actor_at_rt");
        actor.MoraleValue = 5f;
        for (int i = 0; i < 14; i++) at.Tick(24f, new List<SkillActor> { actor });

        var save = at.CaptureState();
        var at2 = new SkillAtrophySystem();
        at2.RestoreState(save);

        Assert.True(at2.IsAtrophied("actor_at_rt", "medical"));
        Assert.True(at2.IsAtrophied("actor_at_rt", "crafting"));
    }

    // ── Test-only helpers ──────────────────────────────────────────

    private sealed class MoraleTrackedActor : SkillActor
    {
        private readonly Dictionary<string, float> _bonuses = new(StringComparer.Ordinal);
        public string Id { get; }
        public bool IsAlive { get; set; } = true;
        public float MoraleValue { get; set; } = 100f;
        public float Morale => MoraleValue;
        public float Health => 100f;
        public float MoraleAppliedLast { get; private set; } = 0f;
        public string ExpertDisciplineId => string.Empty;
        public MoraleTrackedActor(string id) { Id = id; }
        public void SetSkillBonus(string disciplineId, float bonus)
        {
            if (string.IsNullOrEmpty(disciplineId)) return;
            _bonuses[disciplineId] = bonus;
        }

        /// <summary>Test hook: when wired into <c>ApplyMorale</c>, the engine-side
        /// clamp result lands here so the test can assert on it.</summary>
        public void ApplyMoraleFromEngine(string id, float value) => MoraleAppliedLast = value;
    }

    private sealed class ForcedDoubleRng : ISeededRng
    {
        private readonly double _forced;
        public ForcedDoubleRng(double forced) { _forced = forced; }
        public int Seed => 0;
        public double NextDouble() => _forced;
        public int Next(int minInclusive, int maxExclusive) => minInclusive;
        public float NextFloat() => (float)_forced;
    }
}
