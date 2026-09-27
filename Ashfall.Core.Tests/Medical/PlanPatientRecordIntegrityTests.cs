// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    /// <summary>
    /// Core-contract tests for the clinical record integrity validator, pinned to
    /// its authored finding codes. The validator is a pure function over
    /// (pipeline state, reference context) and never mutates a record.
    /// </summary>
    public sealed class PlanPatientRecordIntegrityTests
    {
        private static readonly HashSet<string> Roster =
            new HashSet<string>(StringComparer.Ordinal) { "surv_a", "surv_b" };
        private static readonly HashSet<string> Items =
            new HashSet<string>(StringComparer.Ordinal) { "bandage", "antibiotics" };

        private static PatientRecordIntegrityValidator.Context Ctx(
            HashSet<string> roster = null, HashSet<string> items = null)
        {
            var r = roster ?? Roster;
            var i = items ?? Items;
            return new PatientRecordIntegrityValidator.Context
            {
                IsKnownSurvivor = id => id != null && r.Contains(id),
                IsTreatmentEligible = _ => true,
                IsKnownItem = id => id != null && i.Contains(id),
            };
        }

        private static MedicalPipelineSaveState Reservation(string survivorId, string itemId, int id = 1)
        {
            var state = new MedicalPipelineSaveState();
            state.reservations.reservations.Add(new MedicalReservation
            {
                reservationId = id,
                survivorId = survivorId,
                kind = "medicine",
                targetId = itemId,
                quantity = 1
            });
            return state;
        }

        [Fact]
        public void CleanPipelineProducesNoFindings()
        {
            var findings = PatientRecordIntegrityValidator.Validate(new MedicalPipelineSaveState(), Ctx());
            Assert.Empty(findings);
        }

        [Fact]
        public void RealReferencesProduceNoFindings()
        {
            var findings = PatientRecordIntegrityValidator.Validate(Reservation("surv_a", "bandage"), Ctx());
            Assert.Empty(findings);
        }

        [Fact]
        public void DanglingSurvivorReportsTheAuthoredCode()
        {
            var findings = PatientRecordIntegrityValidator.Validate(Reservation("ghost_survivor", "bandage"), Ctx());
            Assert.Contains(findings, f => f.code == "reservation_unknown_survivor" && f.fatal);
        }

        [Fact]
        public void DanglingItemReportsTheAuthoredCode()
        {
            var findings = PatientRecordIntegrityValidator.Validate(Reservation("surv_a", "ghost_item"), Ctx());
            Assert.Contains(findings, f => f.code == "reservation_unknown_item");
        }

        [Fact]
        public void DuplicateReservationIdIsFatal()
        {
            var state = Reservation("surv_a", "bandage", 7);
            state.reservations.reservations.Add(new MedicalReservation
            {
                reservationId = 7, survivorId = "surv_b", kind = "medicine", targetId = "bandage", quantity = 1
            });

            var findings = PatientRecordIntegrityValidator.Validate(state, Ctx());
            Assert.Contains(findings, f => f.code == "reservation_duplicate_id" && f.fatal);
        }

        [Fact]
        public void NonPositiveReservationIdIsRepairable()
        {
            var findings = PatientRecordIntegrityValidator.Validate(Reservation("surv_a", "bandage", 0), Ctx());
            Assert.Contains(findings, f => f.code == "reservation_invalid_id" && !f.fatal);
        }

        [Fact]
        public void WideningTheRosterClearsTheFinding()
        {
            var wider = new HashSet<string>(StringComparer.Ordinal) { "surv_a", "surv_b", "ghost_survivor" };
            var findings = PatientRecordIntegrityValidator.Validate(Reservation("ghost_survivor", "bandage"), Ctx(wider));
            Assert.DoesNotContain(findings, f => f.code == "reservation_unknown_survivor");
        }

        [Fact]
        public void ValidationNeverMutatesThePipelineState()
        {
            var state = Reservation("ghost_survivor", "ghost_item");
            int rows = state.reservations.reservations.Count;
            long version = state.stateVersion;

            PatientRecordIntegrityValidator.Validate(state, Ctx());
            PatientRecordIntegrityValidator.Validate(state, Ctx());

            Assert.Equal(rows, state.reservations.reservations.Count);
            Assert.Equal(version, state.stateVersion);
        }

        [Fact]
        public void RepeatValidationIsDeterministic()
        {
            var state = Reservation("ghost_survivor", "ghost_item");
            string first = Codes(PatientRecordIntegrityValidator.Validate(state, Ctx()));
            string second = Codes(PatientRecordIntegrityValidator.Validate(state, Ctx()));
            Assert.Equal(first, second);
        }

        [Fact]
        public void EveryFindingCarriesACodeAndDetail()
        {
            foreach (var f in PatientRecordIntegrityValidator.Validate(Reservation("ghost", "ghost"), Ctx()))
            {
                Assert.False(string.IsNullOrWhiteSpace(f.code));
                Assert.False(string.IsNullOrWhiteSpace(f.detail));
            }
        }

        [Fact]
        public void NullStateYieldsNoFindingsRatherThanACrash()
        {
            Assert.Empty(PatientRecordIntegrityValidator.Validate(null, Ctx()));
        }

        private static string Codes(List<MedicalPipelineIntegrityFinding> findings)
            => string.Join("|", findings.ConvertAll(f => f.code + ":" + f.detail));
    }
}
