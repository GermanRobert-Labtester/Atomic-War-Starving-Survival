// SPDX-License-Identifier: MIT
// Task #133 P1c — Psychology observe-only projection + Phase0 inhaler pipeline path.
using System.Linq;
using Ashfall.Core.Medical;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    /// <summary>
    /// Task #133 P1c: the three Phase-0 psychology conditions (combat trauma,
    /// somatic flashbacks, guilt insomnia) project into the medical pipeline
    /// as read-only patient rows. The handlers never treat, never tick, and
    /// never touch the Phase-0 day owner's clocks; the Phase-0 inhaler action
    /// flows through the same pipeline ExecuteTreatment path MedicalPanel
    /// uses (consume + parity with the raw domain ApplyInhaler).
    /// </summary>
    public class PsychologyProjectionTests
    {
        private const string SvId = "survivor_psych_patient";

        private sealed class Fixture
        {
            public Ashfall.Core.Inventory.Inventory Inventory { get; }
                = new Ashfall.Core.Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            public DiagnosisKnowledgeStore Diagnosis { get; } = new DiagnosisKnowledgeStore();
            public MedicalReservationLedger Reservations { get; } = new MedicalReservationLedger();
            public MedicalProcedureSchedule Schedule { get; } = new MedicalProcedureSchedule();
            public CombatTraumaSystem Trauma { get; } = new CombatTraumaSystem();
            public SomaticFlashbackSystem Flashbacks { get; } = new SomaticFlashbackSystem();
            public GuiltInsomniaSystem Guilt { get; } = new GuiltInsomniaSystem();
            public RespiratoryDegenerationSystem Respiratory { get; } = new RespiratoryDegenerationSystem();
            public MedicalPipelineCoordinator Pipeline { get; }
            public PatientRecordProjector Projector { get; }
            public CombatTraumaAfflictionHandler TraumaHandler { get; }
            public SomaticFlashbackAfflictionHandler FlashbackHandler { get; }
            public GuiltInsomniaAfflictionHandler GuiltHandler { get; }

            public Fixture()
            {
                TraumaHandler = new CombatTraumaAfflictionHandler(Trauma);
                FlashbackHandler = new SomaticFlashbackAfflictionHandler(Flashbacks);
                GuiltHandler = new GuiltInsomniaAfflictionHandler(Guilt);
                Pipeline = new MedicalPipelineCoordinator(
                    Inventory, Diagnosis, Reservations, Schedule,
                    _ => PatientAvailability.Ok(), () => 1);
                Pipeline.RegisterHandler(new RespiratoryAfflictionHandler(Respiratory));
                Pipeline.RegisterHandler(TraumaHandler);
                Pipeline.RegisterHandler(FlashbackHandler);
                Pipeline.RegisterHandler(GuiltHandler);
                Projector = new PatientRecordProjector(Pipeline);
                Inventory.TryProduce("inhaler", 3);
                Inventory.TryProduce("bandage", 3);
            }

            public Ashfall.Core.Survivors.SurvivorId Sv => Ashfall.Core.Survivors.SurvivorId.Parse(SvId);

            public void ActivateAllThree()
            {
                Trauma.OnCombatSurvived(SvId);
                Flashbacks.IncreaseSusceptibility(SvId, 0.3f);
                Guilt.RecordGuilt(SvId, "choice_test", 0.5f, 2);
            }
        }

        private static string PsychologyChecksum(Fixture fx)
            => Ashfall.Core.SaveChecksum.Compute(fx.Trauma.CaptureState())
             + "|" + Ashfall.Core.SaveChecksum.Compute(fx.Flashbacks.CaptureState())
             + "|" + Ashfall.Core.SaveChecksum.Compute(fx.Guilt.CaptureState());

        // ── Combat trauma projection ─────────────────────────────────

        [Fact]
        public void Trauma_EpisodeAppears_WhenHypervigilancePositive()
        {
            var fx = new Fixture();
            fx.Trauma.OnCombatSurvived(SvId);

            var episode = fx.TraumaHandler.GetEpisode(fx.Sv);

            Assert.NotNull(episode);
            Assert.Equal(CombatTraumaAfflictionHandler.StageHypervigilant, episode!.StageLabel);
            Assert.Equal(fx.Trauma.GetHypervigilanceLevel(SvId) * 100f, episode.SeverityValue, 3);
            Assert.True(episode.IsActive);
        }

        [Fact]
        public void Trauma_NoEpisode_WhenHypervigilanceZero()
        {
            var fx = new Fixture();

            Assert.Null(fx.TraumaHandler.GetEpisode(fx.Sv));
            Assert.Empty(fx.TraumaHandler.ProjectSymptoms(fx.Sv));
        }

        [Fact]
        public void Trauma_ProjectsHypervigilanceSymptom()
        {
            var fx = new Fixture();
            fx.Trauma.OnCombatSurvived(SvId);

            var symptoms = fx.TraumaHandler.ProjectSymptoms(fx.Sv);

            var symptom = Assert.Single(symptoms);
            Assert.Equal(CombatTraumaAfflictionHandler.SymptomHypervigilance, symptom.SymptomId);
        }

        // ── Flashback projection ─────────────────────────────────────

        [Fact]
        public void Flashback_SusceptibilityOnly_ShowsSusceptibleStage()
        {
            var fx = new Fixture();
            fx.Flashbacks.IncreaseSusceptibility(SvId, 0.3f);

            var episode = fx.FlashbackHandler.GetEpisode(fx.Sv);

            Assert.NotNull(episode);
            Assert.Equal(SomaticFlashbackAfflictionHandler.StageSusceptible, episode!.StageLabel);
            Assert.Equal(30f, episode.SeverityValue, 3);
            // No active flashback yet: the flashback symptom stays hidden.
            Assert.Empty(fx.FlashbackHandler.ProjectSymptoms(fx.Sv));
        }

        [Fact]
        public void Flashback_ActiveHours_ShowFlashbackStage_AndSymptom()
        {
            var fx = new Fixture();
            var state = new FlashbackSurvivorState
            {
                survivorId = SvId,
                susceptibility = 0.3f,
                activeRemainingHours = 4f
            };
            var save = new SomaticFlashbackSaveState();
            save.survivors.Add(state);
            fx.Flashbacks.RestoreState(save);

            var episode = fx.FlashbackHandler.GetEpisode(fx.Sv);

            Assert.NotNull(episode);
            Assert.Equal(SomaticFlashbackAfflictionHandler.StageFlashback, episode!.StageLabel);
            Assert.Equal(4f, episode.SeverityValue, 3);
            var symptom = Assert.Single(fx.FlashbackHandler.ProjectSymptoms(fx.Sv));
            Assert.Equal(SomaticFlashbackAfflictionHandler.SymptomFlashback, symptom.SymptomId);
        }

        [Fact]
        public void Flashback_NoState_NoEpisode()
        {
            var fx = new Fixture();

            Assert.Null(fx.FlashbackHandler.GetEpisode(fx.Sv));
            Assert.Empty(fx.FlashbackHandler.ProjectSymptoms(fx.Sv));
        }

        // ── Guilt insomnia projection ────────────────────────────────

        [Fact]
        public void Guilt_BelowThreshold_ShowsInsomniaStage()
        {
            var fx = new Fixture();
            fx.Guilt.RecordGuilt(SvId, "choice_test", 0.5f, currentDay: 2);

            var episode = fx.GuiltHandler.GetEpisode(fx.Sv);

            Assert.NotNull(episode);
            Assert.Equal(GuiltInsomniaAfflictionHandler.StageInsomnia, episode!.StageLabel);
            Assert.Equal(50f, episode.SeverityValue, 3);
            var symptom = Assert.Single(fx.GuiltHandler.ProjectSymptoms(fx.Sv));
            Assert.Equal(GuiltInsomniaAfflictionHandler.SymptomInsomnia, symptom.SymptomId);
        }

        [Fact]
        public void Guilt_AtHighSeverityThreshold_ShowsCriticalInsomnia()
        {
            var fx = new Fixture();
            fx.Guilt.RecordGuilt(SvId, "choice_test", 0.8f, currentDay: 2);

            var episode = fx.GuiltHandler.GetEpisode(fx.Sv);

            Assert.NotNull(episode);
            Assert.Equal(GuiltInsomniaAfflictionHandler.StageCriticalInsomnia, episode!.StageLabel);
        }

        // ── Observe-only contract ────────────────────────────────────

        [Fact]
        public void PsychologyTreatments_AreAlwaysRefused_WithoutConsuming()
        {
            var fx = new Fixture();
            fx.ActivateAllThree();
            int inhalersBefore = fx.Inventory.CountById("inhaler");
            int bandagesBefore = fx.Inventory.CountById("bandage");

            var targets = new[]
            {
                new AfflictionId(MedicalTreatmentCatalog.CombatTraumaId),
                new AfflictionId(MedicalTreatmentCatalog.SomaticFlashbackId),
                new AfflictionId(MedicalTreatmentCatalog.GuiltInsomniaId)
            };
            foreach (var target in targets)
            {
                foreach (string treatmentId in new[]
                {
                    MedicalTreatmentCatalog.TreatmentBandage,
                    MedicalTreatmentCatalog.TreatmentInhaler
                })
                {
                    var result = fx.Pipeline.ExecuteTreatment(fx.Sv, treatmentId, target: target);
                    Assert.False(result.Success);
                    Assert.Equal("treatment_not_for_affliction", result.ReasonCode);
                }
            }

            Assert.Equal(inhalersBefore, fx.Inventory.CountById("inhaler"));
            Assert.Equal(bandagesBefore, fx.Inventory.CountById("bandage"));
            Assert.Equal(0, fx.Reservations.ReservedQuantity("inhaler"));
        }

        [Fact]
        public void PsychologyHandlers_NeverMutateTheirDomains()
        {
            var fx = new Fixture();
            fx.ActivateAllThree();
            string before = PsychologyChecksum(fx);

            _ = fx.TraumaHandler.GetEpisode(fx.Sv);
            _ = fx.TraumaHandler.ProjectSymptoms(fx.Sv);
            _ = fx.TraumaHandler.ValidateTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentBandage);
            _ = fx.FlashbackHandler.GetEpisode(fx.Sv);
            _ = fx.FlashbackHandler.ProjectSymptoms(fx.Sv);
            _ = fx.GuiltHandler.GetEpisode(fx.Sv);
            _ = fx.GuiltHandler.ProjectSymptoms(fx.Sv);

            Assert.Equal(before, PsychologyChecksum(fx));
        }

        [Fact]
        public void PsychologyHandlers_ProjectThroughPatientRecord()
        {
            var fx = new Fixture();
            fx.ActivateAllThree();

            var record = fx.Projector.Project(fx.Sv);

            var ids = record.Afflictions.Select(a => a.AfflictionId).ToList();
            Assert.Contains(MedicalTreatmentCatalog.CombatTraumaId, ids);
            Assert.Contains(MedicalTreatmentCatalog.SomaticFlashbackId, ids);
            Assert.Contains(MedicalTreatmentCatalog.GuiltInsomniaId, ids);
            // Player-facing by design: rows render without any diagnosis traffic.
            Assert.Contains(record.Afflictions, a =>
                a.AfflictionId == MedicalTreatmentCatalog.CombatTraumaId
                && a.DiagnosisStatus == "unknown"
                && a.StageLabel == CombatTraumaAfflictionHandler.StageHypervigilant);
            Assert.Contains(record.Symptoms, s => s.SymptomId == CombatTraumaAfflictionHandler.SymptomHypervigilance);
            Assert.Contains(record.Symptoms, s => s.SymptomId == GuiltInsomniaAfflictionHandler.SymptomInsomnia);
        }

        [Fact]
        public void PsychologyHandlers_AreObserveOnly_BySource()
        {
            // Mirrors the architecture gate at the unit level: the handler
            // source must not call the Phase-0 mutation or clock APIs.
            string source = System.IO.File.ReadAllText(FindRepoFile(
                "Assets", "Ashfall.Core", "Medical", "PsychologyAfflictionHandlers.cs"));
            Assert.DoesNotContain(".Tick(", source);
            Assert.DoesNotContain(".ApplySedative(", source);
            Assert.DoesNotContain(".OnCombatSurvived(", source);
            Assert.DoesNotContain(".IncreaseSusceptibility(", source);
        }

        // ── Phase-0 keeps the clocks (handlers never steal them) ─────

        [Fact]
        public void DomainTicks_StillAdvance_AndProjectionsFollow()
        {
            var fx = new Fixture();
            fx.ActivateAllThree();
            Assert.NotNull(fx.TraumaHandler.GetEpisode(fx.Sv));

            // The Phase-0 day owner drives the domains directly; the handlers
            // never intercept. 96h past the combat-decay threshold erases
            // hypervigilance (0.05 − 0.02 × 4 days).
            fx.Trauma.Tick(SvId, 96f, isNightTime: false);
            Assert.Equal(0f, fx.Trauma.GetHypervigilanceLevel(SvId));
            Assert.Null(fx.TraumaHandler.GetEpisode(fx.Sv));

            // Flashback susceptibility decays 0.03/day; 4 days erodes 0.3 → 0.18.
            fx.Flashbacks.Tick(SvId, 96f);
            Assert.Equal(0.18f, fx.Flashbacks.GetSusceptibility(SvId), 2);
            Assert.Equal(18f, fx.FlashbackHandler.GetEpisode(fx.Sv)!.SeverityValue, 1);

            // Guilt sources expire after 30 days; the insomnia episode closes.
            fx.Guilt.Tick(SvId, 1f, currentDay: 40);
            Assert.Equal(0f, fx.Guilt.GetInsomniaSeverity(SvId));
            Assert.Null(fx.GuiltHandler.GetEpisode(fx.Sv));
        }

        // ── Phase-0 inhaler through the pipeline (Task #133 P1c) ─────

        [Fact]
        public void PipelineInhaler_PreviewUnavailable_WithoutDamageOrSupply()
        {
            var fx = new Fixture();

            // No lung damage: blocked regardless of stock.
            var noDamage = fx.Pipeline.PreviewTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);
            Assert.False(noDamage.IsAvailable);
            Assert.Equal("no_respiratory_damage", noDamage.FailureCode);

            // Damage but no inhaler: blocked as missing medicine.
            fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 20f;
            fx.Inventory.TryConsume("inhaler", 3);
            var noMedicine = fx.Pipeline.PreviewTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);
            Assert.False(noMedicine.IsAvailable);
            Assert.Equal("missing_medicine", noMedicine.FailureCode);
        }

        [Fact]
        public void PipelineInhaler_ExecutesThroughPipeline_ConsumesOne()
        {
            var fx = new Fixture();
            fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 20f;

            var result = fx.Pipeline.ExecuteTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);

            Assert.True(result.Success, result.ReasonCode);
            Assert.Equal(2, fx.Inventory.CountById("inhaler"));
            Assert.Equal(10f, fx.Respiratory.RespiratoryDegradation(SvId), 3); // 20 − 10
            Assert.Equal(0, fx.Reservations.ReservedQuantity("inhaler"));
        }

        [Fact]
        public void PipelineInhaler_MatchesDirectApplyInhaler_Checksum()
        {
            // Path A: the pipeline path the Phase-0 panel now drives.
            var fx = new Fixture();
            fx.Respiratory.GetOrCreate(SvId).respiratoryDegradation = 55f;
            var viaPipeline = fx.Pipeline.ExecuteTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentInhaler);

            // Path B: the raw domain call the host CLI/tests keep using
            // (Phase0HostSession.ApplyInhaler forwards here 1:1).
            var legacy = new RespiratoryDegenerationSystem();
            legacy.GetOrCreate(SvId).respiratoryDegradation = 55f;
            legacy.ApplyInhaler(SvId);

            Assert.True(viaPipeline.Success, viaPipeline.ReasonCode);
            Assert.Equal(
                Ashfall.Core.SaveChecksum.Compute(legacy.CaptureState()),
                Ashfall.Core.SaveChecksum.Compute(fx.Respiratory.CaptureState()));
        }

        private static string FindRepoFile(params string[] segments)
        {
            var dir = new System.IO.DirectoryInfo(System.AppContext.BaseDirectory);
            while (dir != null)
            {
                string probe = System.IO.Path.Combine(dir.FullName, "Ashfall.csproj");
                if (System.IO.File.Exists(probe))
                    return System.IO.Path.Combine(new[] { dir.FullName }.Concat(segments).ToArray());
                dir = dir.Parent;
            }
            throw new System.IO.DirectoryNotFoundException("Repository root not found");
        }
    }
}
