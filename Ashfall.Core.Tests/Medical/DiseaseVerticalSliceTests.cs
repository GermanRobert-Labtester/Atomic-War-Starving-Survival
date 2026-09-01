// SPDX-License-Identifier: MIT
// Task #133 P1 — Disease vertical slice: quarantine parity, protocol economics,
// hidden identity until identified, single progression owner.
using System;
using System.Linq;
using Ashfall.Core.Disease;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public class DiseaseVerticalSliceTests
    {
        private const string SvId = "survivor_disease_patient";
        private const string Cholera = "disease_cholera";
        private const string Flu = "disease_zoonotic_flu";

        private static DiseaseCatalog MakeCatalog()
        {
            var catalog = new DiseaseCatalog();
            catalog.Add(new DiseaseDefinition
            {
                id = Cholera,
                display_name = "Cholera",
                vector = DiseaseVectorNames.Water,
                lethality = 0.3f,
                incubation_days = 2,
                illness_days = 4,
                infectivity = 0.4f
            });
            catalog.Add(new DiseaseDefinition
            {
                id = Flu,
                display_name = "Zoonotic Flu",
                vector = DiseaseVectorNames.Air,
                lethality = 0.18f,
                incubation_days = 1,
                illness_days = 5,
                infectivity = 0.55f
            });
            return catalog;
        }

        private sealed class Fixture
        {
            public Ashfall.Core.Inventory.Inventory Inventory { get; }
                = new Ashfall.Core.Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            public DiagnosisKnowledgeStore Diagnosis { get; } = new DiagnosisKnowledgeStore();
            public MedicalReservationLedger Reservations { get; } = new MedicalReservationLedger();
            public MedicalProcedureSchedule Schedule { get; } = new MedicalProcedureSchedule();
            public DiseaseSystem Disease { get; }
            public DiseaseCatalog Catalog { get; }
            public MedicalPipelineCoordinator Pipeline { get; }
            public PatientRecordProjector Projector { get; }
            public int Day = 1;

            public Fixture(int seed = 0)
            {
                Catalog = MakeCatalog();
                Disease = new DiseaseSystem(new DiseaseSystemState());
                Disease.BindCatalog(Catalog);
                Pipeline = new MedicalPipelineCoordinator(
                    Inventory, Diagnosis, Reservations, Schedule,
                    _ => PatientAvailability.Ok(), () => Day);
                DiseaseAfflictionHandler.RegisterAll(Pipeline, Disease, Catalog);
                DiseaseProtocolHandler.RegisterAll(Pipeline, Disease);
                Projector = new PatientRecordProjector(Pipeline);
                Inventory.TryProduce(MedicalTreatmentCatalog.ItemCleanWater, 5);
                Inventory.TryProduce(MedicalTreatmentCatalog.ItemGasMask, 5);
                Inventory.TryProduce(MedicalTreatmentCatalog.ItemAntibiotics, 5);
                Inventory.TryProduce(MedicalTreatmentCatalog.ItemHazmatSuit, 5);
            }

            public Ashfall.Core.Survivors.SurvivorId Sv => Ashfall.Core.Survivors.SurvivorId.Parse(SvId);
            public Ashfall.Core.Survivors.SurvivorId Other => Ashfall.Core.Survivors.SurvivorId.Parse("survivor_disease_control");

            public void InfectBoth(string diseaseId, int day)
            {
                Disease.Infect(SvId, diseaseId, day);
                Disease.Infect(Other.Value, diseaseId, day);
            }
        }

        private static string DiseaseChecksum(DiseaseSystem system)
            => Ashfall.Core.SaveChecksum.Compute(system.CaptureState());

        // ── Quarantine/release parity (pipeline == direct domain call) ──

        [Fact]
        public void PipelineQuarantine_MatchesDirectDomainCall_Exactly()
        {
            var direct = new Fixture();
            direct.InfectBoth(Cholera, 1);
            direct.Disease.Quarantine(SvId, Cholera);

            var piped = new Fixture();
            piped.InfectBoth(Cholera, 1);
            piped.Pipeline.ExecuteDiagnose(piped.Sv, new AfflictionId(Cholera));
            var result = piped.Pipeline.ExecuteTreatment(
                piped.Sv, MedicalTreatmentCatalog.TreatmentQuarantine, target: new AfflictionId(Cholera));

            Assert.True(result.Success, result.ReasonCode);
            Assert.True(piped.Disease.IsQuarantined(SvId, Cholera));
            Assert.Equal(DiseaseChecksum(direct.Disease), DiseaseChecksum(piped.Disease));
        }

        [Fact]
        public void PipelineRelease_MatchesDirectDomainCall_Exactly()
        {
            var direct = new Fixture();
            direct.InfectBoth(Cholera, 1);
            direct.Disease.Quarantine(SvId, Cholera);
            direct.Disease.EndQuarantine(SvId, Cholera);

            var piped = new Fixture();
            piped.InfectBoth(Cholera, 1);
            piped.Pipeline.ExecuteDiagnose(piped.Sv, new AfflictionId(Cholera));
            piped.Pipeline.ExecuteTreatment(piped.Sv, MedicalTreatmentCatalog.TreatmentQuarantine, target: new AfflictionId(Cholera));
            var release = piped.Pipeline.ExecuteTreatment(piped.Sv, MedicalTreatmentCatalog.TreatmentRelease, target: new AfflictionId(Cholera));

            Assert.True(release.Success, release.ReasonCode);
            Assert.False(piped.Disease.IsQuarantined(SvId, Cholera));
            Assert.Equal(DiseaseChecksum(direct.Disease), DiseaseChecksum(piped.Disease));
        }

        [Fact]
        public void Quarantine_RequiresConfirmedDiagnosis_AndDomainState()
        {
            var fx = new Fixture();
            fx.InfectBoth(Cholera, 1);
            fx.Pipeline.SuspectFromEvidence(fx.Sv, new AfflictionId(Cholera), 1, "infection_event");

            // Suspected is not enough: the identity is still hidden.
            var preview = fx.Pipeline.PreviewTreatment(
                fx.Sv, MedicalTreatmentCatalog.TreatmentQuarantine, target: new AfflictionId(Cholera));
            Assert.False(preview.IsAvailable);
            Assert.Equal("diagnosis_unconfirmed", preview.FailureCode);

            // The control patient is infected but unidentified: the diagnosis
            // gate fires before any domain mutation could be reached. (A never-
            // infected survivor is rejected earlier by the handler's own
            // contraindication check — see the healthy probe below.)
            var unidentified = fx.Pipeline.PreviewTreatment(
                fx.Other, MedicalTreatmentCatalog.TreatmentQuarantine, target: new AfflictionId(Cholera));
            Assert.False(unidentified.IsAvailable);
            Assert.Equal("diagnosis_unconfirmed", unidentified.FailureCode);

            // A survivor who never had the disease is rejected by the domain
            // handler's contraindication before the diagnosis gate.
            var healthyId = "survivor_disease_healthy";
            fx.Disease.Infect(healthyId, Flu, 1); // presence in the pool, but never cholera
            var healthy = fx.Pipeline.PreviewTreatment(
                Ashfall.Core.Survivors.SurvivorId.Parse(healthyId),
                MedicalTreatmentCatalog.TreatmentQuarantine, target: new AfflictionId(Cholera));
            Assert.False(healthy.IsAvailable);
            Assert.Equal("not_infected", healthy.FailureCode);

            fx.Pipeline.ExecuteIdentify(fx.Sv);
            var after = fx.Pipeline.PreviewTreatment(
                fx.Sv, MedicalTreatmentCatalog.TreatmentQuarantine, target: new AfflictionId(Cholera));
            Assert.True(after.IsAvailable, after.FailureCode);
        }

        // ── Camp-wide protocol economics ──────────────────────────────

        [Fact]
        public void Protocol_ConsumesCountermeasureOnce_SecondApplyRejected()
        {
            var fx = new Fixture();
            int before = fx.Inventory.CountById(MedicalTreatmentCatalog.ItemCleanWater);

            var result = fx.Pipeline.ExecuteProtocol(MedicalTreatmentCatalog.ProtocolPurifyWater);
            Assert.True(result.Success, result.ReasonCode);
            Assert.True(fx.Disease.State.water_purified);
            Assert.Equal(before - 1, fx.Inventory.CountById(MedicalTreatmentCatalog.ItemCleanWater));

            var again = fx.Pipeline.ExecuteProtocol(MedicalTreatmentCatalog.ProtocolPurifyWater);
            Assert.False(again.Success);
            Assert.Equal("already_applied", again.ReasonCode);
            Assert.Equal(before - 1, fx.Inventory.CountById(MedicalTreatmentCatalog.ItemCleanWater));
        }

        [Fact]
        public void Protocol_MissingItem_DoesNotSetFlag()
        {
            var fx = new Fixture();
            fx.Inventory.RemoveById(MedicalTreatmentCatalog.ItemCleanWater, 5);

            var preview = fx.Pipeline.PreviewProtocol(MedicalTreatmentCatalog.ProtocolPurifyWater);
            Assert.False(preview.IsAvailable);
            Assert.Equal("missing_medicine", preview.FailureCode);

            var result = fx.Pipeline.ExecuteProtocol(MedicalTreatmentCatalog.ProtocolPurifyWater);
            Assert.False(result.Success);
            Assert.False(fx.Disease.State.water_purified);
        }

        [Fact]
        public void Protocol_PreviewDoesNotMutate_StateOrInventory()
        {
            var fx = new Fixture();
            int stock = fx.Inventory.CountById(MedicalTreatmentCatalog.ItemGasMask);

            var first = fx.Pipeline.PreviewProtocol(MedicalTreatmentCatalog.ProtocolSealVents);
            var second = fx.Pipeline.PreviewProtocol(MedicalTreatmentCatalog.ProtocolSealVents);

            Assert.True(first.IsAvailable && second.IsAvailable);
            Assert.False(fx.Disease.State.vents_sealed);
            Assert.Equal(stock, fx.Inventory.CountById(MedicalTreatmentCatalog.ItemGasMask));
        }

        [Fact]
        public void UnknownProtocol_IsRejected()
        {
            var fx = new Fixture();
            var result = fx.Pipeline.ExecuteProtocol("protocol_not_real");
            Assert.False(result.Success);
            Assert.Equal("unknown_protocol", result.ReasonCode);
        }

        // ── Hidden identity until identified ──────────────────────────

        [Fact]
        public void UnknownInfection_ShowsNoAfflictionRow_ButShowsSymptoms()
        {
            var fx = new Fixture();
            fx.InfectBoth(Cholera, 1);

            var record = fx.Projector.Project(fx.Sv);

            Assert.DoesNotContain(record.Afflictions, a => a.AfflictionId == Cholera);
            Assert.DoesNotContain(record.Afflictions, a => a.AfflictionId == MedicalTreatmentCatalog.UnidentifiedIllnessId);
            Assert.Contains(record.Symptoms, s => s.SymptomId == DiseaseAfflictionHandler.SymptomGastrointestinalDistress);
            Assert.DoesNotContain(record.Symptoms, s => s.Presentation.Contains("Cholera", StringComparison.Ordinal));
        }

        [Fact]
        public void SuspectedInfection_IsMasked_NameAndEpisodeIdHidden()
        {
            var fx = new Fixture();
            fx.InfectBoth(Cholera, 1);
            fx.Pipeline.SuspectFromEvidence(fx.Sv, new AfflictionId(Cholera), 1, "infection_event");

            var record = fx.Projector.Project(fx.Sv);

            var row = record.Afflictions.Single(a => a.DiagnosisStatus == "suspected");
            Assert.Equal(MedicalTreatmentCatalog.UnidentifiedIllnessId, row.AfflictionId);
            Assert.Equal(string.Empty, row.EpisodeId);
            Assert.Equal("Unidentified illness", row.StageLabel);
            Assert.False(row.SeverityDisclosed);
            Assert.Equal(0f, row.SeverityValue);
        }

        [Fact]
        public void Identify_RevealsNameAndDaysSick_ForAllSuspectedEpisodes()
        {
            var fx = new Fixture();
            fx.InfectBoth(Cholera, 1);
            fx.InfectBoth(Flu, 1);
            fx.Pipeline.SuspectFromEvidence(fx.Sv, new AfflictionId(Cholera), 1, "infection_event");
            fx.Pipeline.SuspectFromEvidence(fx.Sv, new AfflictionId(Flu), 1, "infection_event");

            var result = fx.Pipeline.ExecuteIdentify(fx.Sv);

            Assert.True(result.Success, result.ReasonCode);
            var record = fx.Projector.Project(fx.Sv);
            var rows = record.Afflictions.Where(a => a.DiagnosisStatus == "confirmed").ToList();
            Assert.Equal(2, rows.Count);
            Assert.Contains(rows, a => a.AfflictionId == Cholera);
            Assert.Contains(rows, a => a.AfflictionId == Flu);
            Assert.All(rows, a => Assert.True(a.SeverityDisclosed));

            // The control patient was suspected (cholera) but never identified:
            // exactly one masked row, and the unnamed flu stays invisible.
            fx.Pipeline.SuspectFromEvidence(fx.Other, new AfflictionId(Cholera), 1, "infection_event");
            var otherRecord = fx.Projector.Project(fx.Other);
            var masked = otherRecord.Afflictions.Where(a => a.AfflictionId == MedicalTreatmentCatalog.UnidentifiedIllnessId).ToList();
            Assert.Equal(1, masked.Count);
            Assert.DoesNotContain(otherRecord.Afflictions, a => a.AfflictionId == Cholera);
            Assert.DoesNotContain(otherRecord.Afflictions, a => a.AfflictionId == Flu);
        }

        [Fact]
        public void Identify_WithNothingSuspected_IsRejected()
        {
            var fx = new Fixture();
            var result = fx.Pipeline.ExecuteIdentify(fx.Sv);
            Assert.False(result.Success);
            Assert.Equal("no_suspected_condition", result.ReasonCode);
        }

        [Fact]
        public void RestoredInfection_SuspectedStaysBelowConfirmed()
        {
            // Restore path: a fresh pipeline + restored disease state replays the
            // host's suspect pass (SuspectFromEvidence) — never a confirm.
            var fx = new Fixture();
            fx.InfectBoth(Cholera, 1);

            var restored = new DiseaseSystem(fx.Disease.CaptureState());
            restored.BindCatalog(fx.Catalog);
            var pipeline2 = new MedicalPipelineCoordinator(
                fx.Inventory, new DiagnosisKnowledgeStore(), new MedicalReservationLedger(),
                new MedicalProcedureSchedule(), _ => PatientAvailability.Ok(), () => 1);
            DiseaseAfflictionHandler.RegisterAll(pipeline2, restored, fx.Catalog);

            Assert.True(restored.TryGetInfection(SvId, Cholera, out int _, out bool _));
            pipeline2.SuspectFromEvidence(fx.Sv, new AfflictionId(Cholera), 1, "restored_infection");

            var record = new PatientRecordProjector(pipeline2).Project(fx.Sv);
            Assert.Contains(record.Afflictions, a =>
                a.AfflictionId == MedicalTreatmentCatalog.UnidentifiedIllnessId
                && a.DiagnosisStatus == "suspected");
            Assert.DoesNotContain(record.Afflictions, a => a.AfflictionId == Cholera);
        }

        // ── Single progression owner ──────────────────────────────────

        [Fact]
        public void Handler_NeverAdvancesDaysSick()
        {
            var fx = new Fixture();
            fx.InfectBoth(Cholera, 1);
            fx.Disease.TryGetInfection(SvId, Cholera, out int before, out _);

            var handler = fx.Pipeline.GetHandler(new AfflictionId(Cholera))!;
            for (int i = 0; i < 10; i++)
            {
                handler.GetEpisode(fx.Sv);
                handler.ProjectSymptoms(fx.Sv);
                handler.ValidateTreatment(fx.Sv, MedicalTreatmentCatalog.TreatmentQuarantine);
                handler.HasResolved(fx.Sv);
            }

            fx.Disease.TryGetInfection(SvId, Cholera, out int after, out _);
            Assert.Equal(before, after);
        }

        [Fact]
        public void PipelinePresence_DoesNotPerturbTickDaily()
        {
            // Two identical worlds: one with the pipeline attached and one
            // identify-only interaction. Identical day advance proves the
            // pipeline owns no disease clock.
            var plain = new Fixture();
            plain.InfectBoth(Cholera, 1);
            plain.InfectBoth(Flu, 1);

            var piped = new Fixture();
            piped.InfectBoth(Cholera, 1);
            piped.InfectBoth(Flu, 1);
            piped.Pipeline.SuspectFromEvidence(piped.Sv, new AfflictionId(Cholera), 1, "infection_event");
            piped.Pipeline.ExecuteIdentify(piped.Sv);

            var candidates = new[] { SvId, "survivor_disease_control" };
            plain.Disease.TickDaily(2, candidates);
            piped.Disease.TickDaily(2, candidates);

            Assert.Equal(DiseaseChecksum(plain.Disease), DiseaseChecksum(piped.Disease));
        }
    }
}
