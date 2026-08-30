// SPDX-License-Identifier: MIT
// Task #133 P1b — Ward procedures with a pipeline treatment route through it.
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    /// <summary>
    /// The ward-pipeline bridge: bandage/chelation execute the real pipeline
    /// treatment (validated, consuming supplies) and are logged only on
    /// success; surgery stays log-only; an unbound pipeline preserves the
    /// legacy log-only behaviour; the ward's admission gate fires first.
    /// </summary>
    public class MedicalWardPipelineBridgeTests
    {
        private const string Patient = "elena_vasquez";

        private sealed class Fixture
        {
            public Dictionary<string, float> Health { get; } = new Dictionary<string, float>();
            public Dictionary<string, float> Dose { get; } = new Dictionary<string, float>();
            public Ashfall.Core.Inventory.Inventory Inventory { get; }
                = new Ashfall.Core.Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            public DiagnosisKnowledgeStore Diagnosis { get; } = new DiagnosisKnowledgeStore();
            public MedicalReservationLedger Reservations { get; } = new MedicalReservationLedger();
            public MedicalProcedureSchedule Schedule { get; } = new MedicalProcedureSchedule();
            public MedicalPipelineCoordinator Pipeline { get; }
            public MedicalWardSystem Ward { get; }
            public int Day = 5;

            public Fixture()
            {
                Pipeline = new MedicalPipelineCoordinator(
                    Inventory, Diagnosis, Reservations, Schedule,
                    _ => PatientAvailability.Ok(), () => Day);

                float Max = 100f;
                Pipeline.RegisterHandler(new HealthDeficitAfflictionHandler(
                    getHealth: id => Health.TryGetValue(id, out float h) ? h : Max,
                    getMaxHealth: _ => Max,
                    applyHeal: (id, amount) =>
                    {
                        Health[id] = Math.Min(Max, (Health.TryGetValue(id, out float h) ? h : Max) + amount);
                        return true;
                    }));
                Pipeline.RegisterHandler(new RadiationSicknessAfflictionHandler(
                    getDose: id => Dose.TryGetValue(id, out float d) ? d : 0f,
                    getPhaseName: _ => "Healthy",
                    hasAcuteSickness: _ => false,
                    applyIodine: _ => true,
                    applyAntiRad: (id, rads) =>
                    {
                        Dose[id] = Math.Max(0f, (Dose.TryGetValue(id, out float d) ? d : 0f) - rads);
                        return true;
                    }));

                var beds = new List<MedicalBed>
                {
                    new MedicalBed("bed_a", "Bed A", MedicalBedCategory.General),
                    new MedicalBed("bed_surgery", "Surgical", MedicalBedCategory.Surgical)
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
                Ward = new MedicalWardSystem(new MedicalWardState(), beds, procs);
            }

            public void Admit(string patient = Patient, string bedId = "bed_a")
                => Assert.True(Ward.Admit(patient, bedId, Day).Succeeded);
        }

        [Fact]
        public void Bandage_HealsThroughPipeline_AndLogs()
        {
            var fx = new Fixture();
            fx.Health[Patient] = 60f;
            fx.Inventory.TryProduce("bandage", 1);
            fx.Admit();

            var result = MedicalWardPipelineBridge.RunProcedure(
                fx.Ward, fx.Pipeline, Patient, "proc_bandage", fx.Day);

            Assert.True(result.Succeeded, result.ReasonCode);
            Assert.Equal(85f, fx.Health[Patient], 5);
            Assert.Equal(0, fx.Inventory.CountById("bandage"));
            Assert.Single(fx.Ward.State.ProceduresRun);
        }

        [Fact]
        public void Bandage_MissingMedicine_Fails_NoLog()
        {
            var fx = new Fixture();
            fx.Health[Patient] = 60f;
            fx.Admit();

            var result = MedicalWardPipelineBridge.RunProcedure(
                fx.Ward, fx.Pipeline, Patient, "proc_bandage", fx.Day);

            Assert.False(result.Succeeded);
            Assert.Equal("missing_medicine", result.ReasonCode);
            Assert.Equal(60f, fx.Health[Patient], 5);
            Assert.Empty(fx.Ward.State.ProceduresRun);
        }

        [Fact]
        public void Bandage_HealthFull_Fails_NoLog()
        {
            var fx = new Fixture();
            fx.Health[Patient] = 100f;
            fx.Inventory.TryProduce("bandage", 1);
            fx.Admit();

            var result = MedicalWardPipelineBridge.RunProcedure(
                fx.Ward, fx.Pipeline, Patient, "proc_bandage", fx.Day);

            Assert.False(result.Succeeded);
            Assert.Equal("health_full", result.ReasonCode);
            // Nothing consumed on a failed treatment.
            Assert.Equal(1, fx.Inventory.CountById("bandage"));
            Assert.Empty(fx.Ward.State.ProceduresRun);
        }

        [Fact]
        public void Chelation_AppliesAntiRad_AndLogs()
        {
            var fx = new Fixture();
            fx.Dose[Patient] = 80f;
            fx.Inventory.TryProduce("rad_away", 1);
            fx.Admit();

            var result = MedicalWardPipelineBridge.RunProcedure(
                fx.Ward, fx.Pipeline, Patient, "proc_chelation", fx.Day);

            Assert.True(result.Succeeded, result.ReasonCode);
            Assert.Equal(40f, fx.Dose[Patient], 5);
            Assert.Equal(0, fx.Inventory.CountById("rad_away"));
            Assert.Single(fx.Ward.State.ProceduresRun);
        }

        [Fact]
        public void Chelation_NoDose_Fails_NoLog_NoConsumption()
        {
            var fx = new Fixture();
            fx.Inventory.TryProduce("rad_away", 1);
            fx.Admit();

            var result = MedicalWardPipelineBridge.RunProcedure(
                fx.Ward, fx.Pipeline, Patient, "proc_chelation", fx.Day);

            Assert.False(result.Succeeded);
            Assert.Equal("no_radiation_dose", result.ReasonCode);
            Assert.Equal(1, fx.Inventory.CountById("rad_away"));
            Assert.Empty(fx.Ward.State.ProceduresRun);
        }

        [Fact]
        public void Surgery_StaysLogOnly_NoPipelineCost()
        {
            var fx = new Fixture();
            fx.Health[Patient] = 60f;
            fx.Inventory.TryProduce("bandage", 1);
            fx.Admit(bedId: "bed_surgery");

            var result = MedicalWardPipelineBridge.RunProcedure(
                fx.Ward, fx.Pipeline, Patient, "proc_surgery", fx.Day);

            Assert.True(result.Succeeded, result.ReasonCode);
            // No wound system exists: the pipeline is not involved, supplies untouched.
            Assert.Equal(60f, fx.Health[Patient], 5);
            Assert.Equal(1, fx.Inventory.CountById("bandage"));
            Assert.Single(fx.Ward.State.ProceduresRun);
            Assert.Equal("proc_surgery", fx.Ward.State.ProceduresRun[0].ProcedureId);
        }

        [Fact]
        public void NotAdmitted_Fails_BeforePipeline()
        {
            var fx = new Fixture();
            fx.Health[Patient] = 60f;
            fx.Inventory.TryProduce("bandage", 1);

            var result = MedicalWardPipelineBridge.RunProcedure(
                fx.Ward, fx.Pipeline, Patient, "proc_bandage", fx.Day);

            Assert.False(result.Succeeded);
            Assert.Equal("patient_not_admitted", result.ReasonCode);
            Assert.Equal(1, fx.Inventory.CountById("bandage"));
            Assert.Equal(60f, fx.Health[Patient], 5);
        }

        [Fact]
        public void UnboundPipeline_PreservesLegacyLogOnlyPath()
        {
            var fx = new Fixture();
            fx.Health[Patient] = 60f;
            fx.Admit();

            var result = MedicalWardPipelineBridge.RunProcedure(
                fx.Ward, null, Patient, "proc_bandage", fx.Day);

            // Headless/unbound sessions keep the pre-bridge behaviour: the
            // procedure is recorded without any pipeline transaction.
            Assert.True(result.Succeeded, result.ReasonCode);
            Assert.Equal(60f, fx.Health[Patient], 5);
            Assert.Single(fx.Ward.State.ProceduresRun);
        }

        [Fact]
        public void UnknownProcedure_StillRejectedByWard()
        {
            var fx = new Fixture();
            fx.Admit();

            var result = MedicalWardPipelineBridge.RunProcedure(
                fx.Ward, fx.Pipeline, Patient, "proc_unknown", fx.Day);

            Assert.False(result.Succeeded);
            Assert.Equal("unknown_procedure", result.ReasonCode);
        }
    }
}
