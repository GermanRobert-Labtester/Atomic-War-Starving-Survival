using System;
using System.Collections.Generic;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public class MedicalWardSystemTests
    {
        private static MedicalWardSystem MakeWard()
        {
            var beds = new List<MedicalBed>
            {
                new MedicalBed("bed_a", "Bed A", MedicalBedCategory.General),
                new MedicalBed("bed_b", "Bed B", MedicalBedCategory.Surgical),
                new MedicalBed("bed_c_iso", "Isolation C", MedicalBedCategory.Isolation, isolation: true)
            };
            var procs = new List<MedicalProcedureDef>
            {
                new MedicalProcedureDef("proc_bandage", "Bandage", "MedicalSystem",
                    new Dictionary<string, int> { ["bandage"] = 1 }),
                new MedicalProcedureDef("proc_chelation", "Chelation", "DoseLedgerSystem",
                    new Dictionary<string, int> { ["anti_rad"] = 1 }),
                new MedicalProcedureDef("proc_surgery", "Surgery", "MedicalSystem",
                    new Dictionary<string, int> { ["bandage"] = 3 })
            };
            return new MedicalWardSystem(new MedicalWardState(), beds, procs);
        }

        [Fact]
        public void Admit_AssignsPatientToBed()
        {
            var w = MakeWard();
            var r = w.Admit("elena_vasquez", "bed_a", 5);
            Assert.True(r.Succeeded);
            Assert.Equal("elena_vasquez", w.GetBedOccupant("bed_a"));
        }

        [Fact]
        public void Admit_BedOccupied_Fails()
        {
            var w = MakeWard();
            w.Admit("s1", "bed_a", 1);
            var r = w.Admit("s2", "bed_a", 2);
            Assert.False(r.Succeeded);
            Assert.Equal("bed_occupied", r.ReasonCode);
        }

        [Fact]
        public void Admit_UnknownBed_Fails()
        {
            var w = MakeWard();
            var r = w.Admit("s1", "bed_does_not_exist", 1);
            Assert.False(r.Succeeded);
            Assert.Equal("unknown_bed", r.ReasonCode);
        }

        [Fact]
        public void Discharge_RemovesOccupancy()
        {
            var w = MakeWard();
            w.Admit("elena_vasquez", "bed_a", 1);
            var r = w.Discharge("elena_vasquez", 3);
            Assert.True(r.Succeeded);
            Assert.Null(w.GetBedOccupant("bed_a"));
        }

        [Fact]
        public void Discharge_NotAdmitted_Fails()
        {
            var w = MakeWard();
            var r = w.Discharge("ghost", 3);
            Assert.False(r.Succeeded);
            Assert.Equal("not_admitted", r.ReasonCode);
        }

        [Fact]
        public void RunProcedure_RecordsAndDelegatesToSystem()
        {
            var w = MakeWard();
            w.Admit("elena_vasquez", "bed_b", 1);
            var r = w.RunProcedure("elena_vasquez", "proc_chelation", 2);
            Assert.True(r.Succeeded);
            Assert.Equal("proc_chelation", r.ProcedureId);
            Assert.Single(w.State.ProceduresRun);
        }

        [Fact]
        public void RunProcedure_NotAdmitted_Fails()
        {
            var w = MakeWard();
            var r = w.RunProcedure("ghost", "proc_bandage", 1);
            Assert.False(r.Succeeded);
            Assert.Equal("patient_not_admitted", r.ReasonCode);
        }

        [Fact]
        public void RunProcedure_UnknownProcedure_Fails()
        {
            var w = MakeWard();
            w.Admit("elena_vasquez", "bed_a", 1);
            var r = w.RunProcedure("elena_vasquez", "proc_unknown", 1);
            Assert.False(r.Succeeded);
            Assert.Equal("unknown_procedure", r.ReasonCode);
        }

        [Fact]
        public void Events_FireOnAdmitAndProcedure()
        {
            var w = MakeWard();
            var fired = new List<MedicalWardEvent>();
            w.OnWardChanged += e => fired.Add(e);
            w.Admit("s1", "bed_a", 1);
            w.RunProcedure("s1", "proc_bandage", 2);
            Assert.Equal(2, fired.Count);
            Assert.Equal(MedicalWardEventKind.Admitted, fired[0].Kind);
            Assert.Equal(MedicalWardEventKind.ProcedureRun, fired[1].Kind);
        }

        [Fact]
        public void CaptureRestore_RoundTrip()
        {
            var w = MakeWard();
            w.Admit("s1", "bed_a", 1);
            w.RunProcedure("s1", "proc_bandage", 2);
            var save = w.CaptureState();
            var fresh = MakeWard();
            fresh.RestoreState(save);
            Assert.Single(fresh.State.Admissions);
            Assert.Single(fresh.State.ProceduresRun);
        }

        [Fact]
        public void Save_RoundTrip_ChecksumStable()
        {
            var w = MakeWard();
            w.Admit("s1", "bed_a", 1);
            var save = new MedicalWardSave
            {
                simDay = 1,
                Beds = new List<MedicalBedSave>
                {
                    new MedicalBedSave { BedId = "bed_a", DisplayName = "Bed A", Category = 0, Isolation = false }
                },
                State = w.CaptureState()
            };
            var json = new SystemTextJsonSerializer();
            string text = MedicalWardSaveCodec.EncodeToString(save, json);
            var loaded = MedicalWardSaveCodec.Decode(text, json);
            Assert.Equal(save.Checksum, loaded.Checksum);
        }

        [Fact]
        public void Save_TamperedChecksumRejected()
        {
            var json = new SystemTextJsonSerializer();
            var save = new MedicalWardSave
            {
                simDay = 1,
                Beds = new List<MedicalBedSave>
                {
                    new MedicalBedSave { BedId = "x", DisplayName = "y", Category = 0 }
                },
                State = new MedicalWardState()
            };
            string text = MedicalWardSaveCodec.EncodeToString(save, json);
            int idx = text.IndexOf("simDay", StringComparison.Ordinal);
            char[] arr = text.ToCharArray();
            arr[idx + 8] = arr[idx + 8] == '1' ? '9' : '1';
            string tampered = new string(arr);
            Assert.Throws<InvalidOperationException>(() => MedicalWardSaveCodec.Decode(tampered, json));
        }

        [Fact]
        public void Save_EmptyChecksumRejected()
        {
            var json = new SystemTextJsonSerializer();
            var save = new MedicalWardSave { simDay = 1, Checksum = string.Empty };
            string text = json.Serialize(save);
            Assert.Throws<InvalidOperationException>(() => MedicalWardSaveCodec.Decode(text, json));
        }
    }
}
