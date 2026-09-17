// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class Plan24SurvivorJourneyTests
    {
        [Fact]
        public void ActivePatientCannotOccupyTwoWardBeds()
        {
            var ward = MakeWard();
            Assert.True(ward.Admit("survivor_a", "bed_a", 1).Succeeded);

            var second = ward.Admit("survivor_a", "bed_b", 2);

            Assert.False(second.Succeeded);
            Assert.Equal("already_admitted", second.ReasonCode);
            Assert.Equal("bed_a", ward.GetActiveAdmission("survivor_a")!.BedId);
        }

        [Fact]
        public void AdmissionEmitsLaborVacancySignalOnce()
        {
            var ward = MakeWard();
            var admitted = new List<string>();
            ward.OnPatientAdmitted += id => admitted.Add(id);

            ward.Admit("survivor_a", "bed_a", 1);
            ward.Admit("survivor_a", "bed_b", 2);

            Assert.Equal(new[] { "survivor_a" }, admitted);
        }

        [Fact]
        public void AdmissionAndDischargeEmitOneCanonicalWardTransitionEach()
        {
            var ward = MakeWard();
            var events = new List<MedicalWardEvent>();
            ward.OnWardChanged += evt => events.Add(evt);

            Assert.True(ward.Admit("survivor_a", "bed_a", 3).Succeeded);
            Assert.True(ward.Discharge("survivor_a", 5).Succeeded);
            Assert.False(ward.Discharge("survivor_a", 6).Succeeded);

            Assert.Collection(events,
                evt =>
                {
                    Assert.Equal(MedicalWardEventKind.Admitted, evt.Kind);
                    Assert.Equal(3, evt.Day);
                },
                evt =>
                {
                    Assert.Equal(MedicalWardEventKind.Discharged, evt.Kind);
                    Assert.Equal(5, evt.Day);
                });
        }

        private static MedicalWardSystem MakeWard()
        {
            return new MedicalWardSystem(
                new MedicalWardState(),
                new[]
                {
                    new MedicalBed("bed_a", "Bed A", MedicalBedCategory.General),
                    new MedicalBed("bed_b", "Bed B", MedicalBedCategory.Surgical)
                },
                new[]
                {
                    new MedicalProcedureDef("proc_bandage", "Bandage", "MedicalSystem")
                });
        }
    }
}
