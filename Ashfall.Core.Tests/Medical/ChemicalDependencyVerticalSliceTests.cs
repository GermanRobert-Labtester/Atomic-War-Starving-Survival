// SPDX-License-Identifier: MIT
// Task #133 P1b — Chemical-dependency vertical slice: parity, single-clock, sub-case targeting.
using System;
using System.Linq;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    /// <summary>
    /// Detox starts flow through the unified pipeline while the dependency
    /// domain keeps every clock and rule. Proves: behavioral parity with the
    /// legacy direct Begin* calls, sub-case (substance) targeting, no second
    /// detox clock on the procedure schedule, and an untouched domain clock.
    /// </summary>
    public class ChemicalDependencyVerticalSliceTests
    {
        private const string SvId = "survivor_dep_patient";
        private const string ItemPainkillers = "painkillers";
        private const string ItemAlcohol = "alcohol";

        private sealed class Fixture
        {
            public Ashfall.Core.Inventory.Inventory Inventory { get; }
                = new Ashfall.Core.Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            public DiagnosisKnowledgeStore Diagnosis { get; } = new DiagnosisKnowledgeStore();
            public MedicalReservationLedger Reservations { get; } = new MedicalReservationLedger();
            public MedicalProcedureSchedule Schedule { get; } = new MedicalProcedureSchedule();
            public ChemicalDependencySystem Dependency { get; } = new ChemicalDependencySystem();
            public MedicalPipelineCoordinator Pipeline { get; }
            public PatientRecordProjector Projector { get; }
            public int Day = 1;

            public Fixture()
            {
                Pipeline = new MedicalPipelineCoordinator(
                    Inventory, Diagnosis, Reservations, Schedule,
                    _ => PatientAvailability.Ok(), () => Day);
                Pipeline.RegisterHandler(new ChemicalDependencyAfflictionHandler(Dependency));
                Projector = new PatientRecordProjector(Pipeline);
            }

            public Ashfall.Core.Survivors.SurvivorId Sv => Ashfall.Core.Survivors.SurvivorId.Parse(SvId);

            public void AddDoses(string itemId, int doses)
            {
                for (int i = 0; i < doses; i++)
                    Dependency.OnSubstanceConsumed(SvId, itemId, ChemicalDependencyKind.Opioid);
            }
        }

        private static string Checksum(ChemicalDependencySystem system)
            => Ashfall.Core.SaveChecksum.Compute(system.CaptureState());

        // ── Parity: pipeline detox start == legacy direct Begin* ─────

        [Fact]
        public void ManagedDetox_MatchesLegacyDirectCall_Exactly()
        {
            var legacy = new ChemicalDependencySystem();
            legacy.OnSubstanceConsumed(SvId, ItemPainkillers, ChemicalDependencyKind.Opioid);
            legacy.OnSubstanceConsumed(SvId, ItemPainkillers, ChemicalDependencyKind.Opioid);
            Assert.True(legacy.BeginManagedDetox(SvId, ItemPainkillers));

            var fx = new Fixture();
            fx.AddDoses(ItemPainkillers, 2);
            var result = fx.Pipeline.ExecuteTreatment(
                fx.Sv, MedicalTreatmentCatalog.TreatmentManagedDetox, targetItem: ItemPainkillers);

            Assert.True(result.Success, result.ReasonCode);
            Assert.Equal(Checksum(legacy), Checksum(fx.Dependency));
        }

        [Fact]
        public void ColdTurkey_MatchesLegacyDirectCall_Exactly()
        {
            var legacy = new ChemicalDependencySystem();
            legacy.OnSubstanceConsumed(SvId, ItemPainkillers, ChemicalDependencyKind.Opioid);
            legacy.OnSubstanceConsumed(SvId, ItemPainkillers, ChemicalDependencyKind.Opioid);
            Assert.True(legacy.BeginColdTurkey(SvId, ItemPainkillers));

            var fx = new Fixture();
            fx.AddDoses(ItemPainkillers, 2);
            var result = fx.Pipeline.ExecuteTreatment(
                fx.Sv, MedicalTreatmentCatalog.TreatmentColdTurkey, targetItem: ItemPainkillers);

            Assert.True(result.Success, result.ReasonCode);
            Assert.Equal(Checksum(legacy), Checksum(fx.Dependency));
        }

        // ── Sub-case (targetItem) validation ─────────────────────────

        [Fact]
        public void MissingTargetItem_IsRejected()
        {
            var fx = new Fixture();
            fx.AddDoses(ItemPainkillers, 2);

            var result = fx.Pipeline.ExecuteTreatment(
                fx.Sv, MedicalTreatmentCatalog.TreatmentManagedDetox);

            Assert.False(result.Success);
            Assert.Equal("target_item_required", result.ReasonCode);
            Assert.True(fx.Dependency.DependenciesFor(SvId)![0].dependencyLevel >= 0f);
            Assert.False(fx.Dependency.DependenciesFor(SvId)![0].inManagedDetox);
        }

        [Fact]
        public void UnknownSubstance_MissingDependency()
        {
            var fx = new Fixture();
            fx.AddDoses(ItemPainkillers, 2);

            var result = fx.Pipeline.ExecuteTreatment(
                fx.Sv, MedicalTreatmentCatalog.TreatmentManagedDetox, targetItem: "alcohol");

            Assert.False(result.Success);
            Assert.Equal("missing_dependency", result.ReasonCode);
        }

        [Fact]
        public void BelowThreshold_Blocked()
        {
            var fx = new Fixture();
            fx.AddDoses(ItemPainkillers, 1); // 0.15 < 0.3 threshold

            var result = fx.Pipeline.ExecuteTreatment(
                fx.Sv, MedicalTreatmentCatalog.TreatmentManagedDetox, targetItem: ItemPainkillers);

            Assert.False(result.Success);
            Assert.Equal("below_threshold", result.ReasonCode);
        }

        [Fact]
        public void AlreadyInTreatment_Blocked()
        {
            var fx = new Fixture();
            fx.AddDoses(ItemPainkillers, 2);
            var first = fx.Pipeline.ExecuteTreatment(
                fx.Sv, MedicalTreatmentCatalog.TreatmentManagedDetox, targetItem: ItemPainkillers);
            Assert.True(first.Success, first.ReasonCode);

            var second = fx.Pipeline.ExecuteTreatment(
                fx.Sv, MedicalTreatmentCatalog.TreatmentManagedDetox, targetItem: ItemPainkillers);

            Assert.False(second.Success);
            Assert.Equal("already_in_treatment", second.ReasonCode);
        }

        [Fact]
        public void SwitchFromManagedToColdTurkey_MatchesDomainSemantics()
        {
            // The domain allows switching programs (BeginColdTurkey leaves
            // managed mode); the pipeline must not add a stricter rule.
            var legacy = new ChemicalDependencySystem();
            legacy.OnSubstanceConsumed(SvId, ItemPainkillers, ChemicalDependencyKind.Opioid);
            legacy.OnSubstanceConsumed(SvId, ItemPainkillers, ChemicalDependencyKind.Opioid);
            legacy.BeginManagedDetox(SvId, ItemPainkillers);
            Assert.True(legacy.BeginColdTurkey(SvId, ItemPainkillers));

            var fx = new Fixture();
            fx.AddDoses(ItemPainkillers, 2);
            fx.Pipeline.ExecuteTreatment(
                fx.Sv, MedicalTreatmentCatalog.TreatmentManagedDetox, targetItem: ItemPainkillers);
            var switchResult = fx.Pipeline.ExecuteTreatment(
                fx.Sv, MedicalTreatmentCatalog.TreatmentColdTurkey, targetItem: ItemPainkillers);

            Assert.True(switchResult.Success, switchResult.ReasonCode);
            Assert.Equal(Checksum(legacy), Checksum(fx.Dependency));
        }

        [Fact]
        public void ForeignTreatmentViaTarget_Rejected()
        {
            // Routing a foreign treatment id at the chem-dep handler through
            // the target seam must hit the handler's own guard.
            var fx = new Fixture();
            fx.AddDoses(ItemPainkillers, 2);

            var result = fx.Pipeline.ExecuteTreatment(
                fx.Sv, MedicalTreatmentCatalog.TreatmentBandage,
                target: new AfflictionId(MedicalTreatmentCatalog.ChemicalDependencyId));

            Assert.False(result.Success);
            Assert.Equal("treatment_not_for_affliction", result.ReasonCode);
        }

        // ── Independence and single-clock guarantees ─────────────────

        [Fact]
        public void TwoSubstances_StartingOneLeavesOtherUntouched()
        {
            var fx = new Fixture();
            fx.AddDoses(ItemPainkillers, 2);
            fx.AddDoses(ItemAlcohol, 2);

            var result = fx.Pipeline.ExecuteTreatment(
                fx.Sv, MedicalTreatmentCatalog.TreatmentManagedDetox, targetItem: ItemPainkillers);

            Assert.True(result.Success, result.ReasonCode);
            var painkillers = fx.Dependency.DependenciesFor(SvId)!.First(d => d.itemId == ItemPainkillers);
            var alcohol = fx.Dependency.DependenciesFor(SvId)!.First(d => d.itemId == ItemAlcohol);
            Assert.True(painkillers.inManagedDetox);
            Assert.False(alcohol.inManagedDetox);
            Assert.False(alcohol.inColdTurkey);
            Assert.Equal(0f, alcohol.detoxProgressHours);
        }

        [Fact]
        public void NoSecondClock_ScheduleStaysEmpty()
        {
            var fx = new Fixture();
            fx.AddDoses(ItemPainkillers, 2);

            fx.Pipeline.ExecuteTreatment(
                fx.Sv, MedicalTreatmentCatalog.TreatmentManagedDetox, targetItem: ItemPainkillers);
            fx.Pipeline.ExecuteTreatment(
                fx.Sv, MedicalTreatmentCatalog.TreatmentColdTurkey, targetItem: ItemAlcohol);

            Assert.Empty(fx.Pipeline.Schedule.Active);
            Assert.Empty(fx.Pipeline.Schedule.History);
        }

        [Fact]
        public void DomainClock_AloneAdvancesDetox()
        {
            var fx = new Fixture();
            fx.AddDoses(ItemPainkillers, 2);
            fx.Pipeline.ExecuteTreatment(
                fx.Sv, MedicalTreatmentCatalog.TreatmentManagedDetox, targetItem: ItemPainkillers);

            fx.Dependency.TickHours(SvId, 10f);
            fx.Dependency.TickHours(SvId, 5f);

            var dep = fx.Dependency.DependenciesFor(SvId)!.First(d => d.itemId == ItemPainkillers);
            Assert.Equal(15f, dep.detoxProgressHours, 5);
            Assert.True(dep.inManagedDetox);
        }

        // ── Preview purity ───────────────────────────────────────────

        [Fact]
        public void Preview_DoesNotMutateLedger()
        {
            var fx = new Fixture();
            fx.AddDoses(ItemPainkillers, 2);
            string before = Checksum(fx.Dependency);

            var preview = fx.Pipeline.PreviewTreatment(
                fx.Sv, MedicalTreatmentCatalog.TreatmentManagedDetox, targetItem: ItemPainkillers);

            Assert.True(preview.IsAvailable);
            Assert.Equal(before, Checksum(fx.Dependency));
        }

        // ── Projection ───────────────────────────────────────────────

        [Fact]
        public void Episode_SurfacesHighestDependency_OnlyWhenFormed()
        {
            var fx = new Fixture();
            var sv = fx.Sv;

            // One dose below the threshold: habituation, not an affliction.
            fx.AddDoses(ItemAlcohol, 1);
            Assert.Null(fx.Pipeline.GetHandler(new AfflictionId(MedicalTreatmentCatalog.ChemicalDependencyId))!
                .GetEpisode(sv));

            // Two substances; the higher one defines the episode.
            fx.AddDoses(ItemPainkillers, 3); // 0.45
            fx.AddDoses(ItemAlcohol, 1);     // 0.30 (threshold)
            var handler = fx.Pipeline.GetHandler(new AfflictionId(MedicalTreatmentCatalog.ChemicalDependencyId))!;
            var episode = handler.GetEpisode(sv);

            Assert.NotNull(episode);
            Assert.Equal(45f, episode!.SeverityValue, 5);
            Assert.Equal("ADDICTED", episode.StageLabel);
        }

        [Fact]
        public void Episode_ReflectsActiveProgram()
        {
            var fx = new Fixture();
            fx.AddDoses(ItemPainkillers, 2);
            fx.Pipeline.ExecuteTreatment(
                fx.Sv, MedicalTreatmentCatalog.TreatmentColdTurkey, targetItem: ItemPainkillers);

            var handler = fx.Pipeline.GetHandler(new AfflictionId(MedicalTreatmentCatalog.ChemicalDependencyId))!;
            var episode = handler.GetEpisode(fx.Sv);

            Assert.NotNull(episode);
            Assert.Equal("COLD TURKEY", episode!.StageLabel);
            var symptoms = handler.ProjectSymptoms(fx.Sv);
            Assert.Contains(symptoms, s => s.SymptomId == ChemicalDependencyAfflictionHandler.SymptomWithdrawalTremor);
        }

        [Fact]
        public void DiagnosisStore_PlaysNoRole()
        {
            // The ledger is player-facing by design: no suspect/confirm traffic.
            var fx = new Fixture();
            fx.AddDoses(ItemPainkillers, 2);
            fx.Pipeline.ExecuteTreatment(
                fx.Sv, MedicalTreatmentCatalog.TreatmentManagedDetox, targetItem: ItemPainkillers);

            var episode = AfflictionEpisodeId.Create(
                fx.Sv, new AfflictionId(MedicalTreatmentCatalog.ChemicalDependencyId));
            Assert.Equal(DiagnosisStatus.Unknown, fx.Pipeline.Diagnosis.GetStatus(episode));
        }
    }
}
