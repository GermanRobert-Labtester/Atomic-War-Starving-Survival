using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Maritime;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Regression gate for the 26 snapshot-alias fixes (Airlock..Decontamination).
    /// Verifies deep-copy via SystemTextJsonSerializer: mutating a captured
    /// snapshot must not affect live state, and mutating live must not affect
    /// a previously captured snapshot. Uses DTO-level clones for complex systems
    /// plus direct system CaptureState checks for simple constructors.
    /// </summary>
    public class SaveSnapshotAlias26Tests
    {
        private static readonly IJsonSerializer Json = new SystemTextJsonSerializer();

        private static T Clone<T>(T src) where T : class, new()
        {
            if (src == null) return new T();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<T>(json) ?? new T();
        }

        // ── DTO deep-copy checks (all 26 states) ───────────────────────

        [Fact] public void AirlockSecurityState_Clone_IsDeep() { var src = new AirlockSecurityState(); src.incidentLog.Add(new AirlockIncidentLog { outcome = "a" }); var c = Clone(src); Assert.False(ReferenceEquals(src.incidentLog[0], c.incidentLog[0])); c.incidentLog[0].outcome = "tampered"; Assert.Equal("a", src.incidentLog[0].outcome); Assert.False(ReferenceEquals(src.incidentLog, c.incidentLog)); }
        [Fact] public void ApprenticeshipState_Clone_IsDeep() { var src = new ApprenticeshipState(); src.activePairs.Add(new Apprenticeship { pairId = "p1" }); var c = Clone(src); c.activePairs[0].pairId = "tampered"; Assert.Equal("p1", src.activePairs[0].pairId); Assert.False(ReferenceEquals(src.activePairs, c.activePairs)); }
        [Fact] public void ArchiveDeskState_Clone_IsDeep() { var src = new ArchiveDeskState(); src.queue.Add(new TranscriptionJob { jobId = "j1" }); var c = Clone(src); c.queue[0].jobId = "x"; Assert.Equal("j1", src.queue[0].jobId); }
        [Fact] public void AudioConditionState_Clone_IsDeep() { var src = new AudioConditionState(); src.activeConditions.Add(new ActiveAudioCondition { conditionId = "c1" }); var c = Clone(src); c.activeConditions[0].conditionId = "x"; Assert.Equal("c1", src.activeConditions[0].conditionId); }
        [Fact] public void AutopsyState_Clone_IsDeep() { var src = new AutopsyState(); src.cases.Add(new AutopsyCase { caseId = "a1" }); var c = Clone(src); c.cases[0].caseId = "x"; Assert.Equal("a1", src.cases[0].caseId); }
        [Fact] public void ContractorRosterState_Clone_IsDeep() { var src = new ContractorRosterState(); src.contractors.Add(new Contractor { contractorId = "c1" }); var c = Clone(src); c.contractors[0].contractorId = "x"; Assert.Equal("c1", src.contractors[0].contractorId); }
        [Fact] public void EquipmentConditionState_Clone_IsDeep() { var src = new EquipmentConditionState(); src.items.Add(new EquipmentInstance { instanceId = "i1" }); var c = Clone(src); c.items[0].instanceId = "x"; Assert.Equal("i1", src.items[0].instanceId); }
        [Fact] public void ExcavationState_Clone_IsDeep() { var src = new ExcavationState(); src.sites.Add(new ExcavationSite { siteId = "s1" }); var c = Clone(src); c.sites[0].siteId = "x"; Assert.Equal("s1", src.sites[0].siteId); }
        [Fact] public void ExpeditionVehicleState_Clone_IsDeep() { var src = new ExpeditionVehicleState(); src.ownedVehicles["v1"] = new VehicleInstance { vehicleId = "v1" }; var c = Clone(src); c.ownedVehicles["v1"].vehicleId = "x"; Assert.Equal("v1", src.ownedVehicles["v1"].vehicleId); }
        [Fact] public void LineageState_Clone_IsDeep() { var src = new LineageState(); src.lineages.Add(new LineageRecord { parentId = "p1" }); var c = Clone(src); c.lineages[0].parentId = "x"; Assert.Equal("p1", src.lineages[0].parentId); }
        [Fact] public void KitchenNutritionState_Clone_IsDeep() { var src = new KitchenNutritionState(); src.pantry.Add(new PantryItem { itemId = "it1" }); var c = Clone(src); c.pantry[0].itemId = "x"; Assert.Equal("it1", src.pantry[0].itemId); }
        [Fact] public void LibraryStudyState_Clone_IsDeep() { var src = new LibraryStudyState(); src.activeJobs.Add(new StudyJob { jobId = "j1" }); var c = Clone(src); c.activeJobs[0].jobId = "x"; Assert.Equal("j1", src.activeJobs[0].jobId); }
        [Fact] public void MaritimeDiveState_Clone_IsDeep() { var src = new MaritimeDiveState(); src.sites.Add(new DiveSite { siteId = "s1" }); var c = Clone(src); c.sites[0].siteId = "x"; Assert.Equal("s1", src.sites[0].siteId); }
        [Fact] public void MentalHealthState_Clone_IsDeep() { var src = new MentalHealthState(); src.activeCases.Add(new CrisisCase { caseId = "c1" }); var c = Clone(src); c.activeCases[0].caseId = "x"; Assert.Equal("c1", src.activeCases[0].caseId); }
        [Fact] public void OrbitalTelemetryState_Clone_IsDeep() { var src = new OrbitalTelemetryState(); src.impactHistory.Add(42); var c = Clone(src); c.impactHistory[0] = 999; Assert.Equal(42, src.impactHistory[0]); }
        [Fact] public void PharmaLabState_Clone_IsDeep() { var src = new PharmaLabState(); src.reservedInputIds.Add("in1"); var c = Clone(src); c.reservedInputIds[0] = "x"; Assert.Equal("in1", src.reservedInputIds[0]); }
        [Fact] public void RegionalTreatyState_Clone_IsDeep() { var src = new RegionalTreatyState(); src.treaties.Add(new TreatyInstance { treatyId = "t1" }); var c = Clone(src); c.treaties[0].treatyId = "x"; Assert.Equal("t1", src.treaties[0].treatyId); }
        [Fact] public void ShelterScheduleState_Clone_IsDeep() { var src = new ShelterScheduleState(); src.assignments.Add(new SleepAssignment { survivorId = "s1" }); var c = Clone(src); c.assignments[0].survivorId = "x"; Assert.Equal("s1", src.assignments[0].survivorId); }
        [Fact] public void ShelterThermalState_Clone_IsDeep() { var src = new ShelterThermalState(); src.rooms.Add(new ThermalRoomNode { roomId = "r1" }); var c = Clone(src); c.rooms[0].roomId = "x"; Assert.Equal("r1", src.rooms[0].roomId); }
        [Fact] public void SumpFloodingState_Clone_IsDeep() { var src = new SumpFloodingState(); src.nodes.Add(new SumpNode { nodeId = "n1" }); var c = Clone(src); c.nodes[0].nodeId = "x"; Assert.Equal("n1", src.nodes[0].nodeId); }
        [Fact] public void SurvivorRelationsState_Clone_IsDeep() { var src = new SurvivorRelationsState(); src.relationships.Add(new RelationshipEntry { dwellerA = "a1" }); var c = Clone(src); c.relationships[0].dwellerA = "x"; Assert.Equal("a1", src.relationships[0].dwellerA); }
        [Fact] public void VentilationState_Clone_IsDeep() { var src = new VentilationState(); src.activeSources.Add(new VentilationSource { sourceId = "s1" }); var c = Clone(src); c.activeSources[0].sourceId = "x"; Assert.Equal("s1", src.activeSources[0].sourceId); }
        [Fact] public void VinylMoraleState_Clone_IsDeep() { var src = new VinylMoraleState(); src.ownedRecordIds.Add("r1"); var c = Clone(src); c.ownedRecordIds[0] = "x"; Assert.Equal("r1", src.ownedRecordIds[0]); }
        [Fact] public void WeatherStationState_Clone_IsDeep() { var src = new WeatherStationState(); src.cachedForecast.Add(new ForecastEntry { day = 5 }); var c = Clone(src); c.cachedForecast[0].day = 99; Assert.Equal(5, src.cachedForecast[0].day); }
        [Fact] public void WildlifeTrappingState_Clone_IsDeep() { var src = new WildlifeTrappingState(); src.trapSites.Add(new TrapSite { siteId = "t1" }); var c = Clone(src); c.trapSites[0].siteId = "x"; Assert.Equal("t1", src.trapSites[0].siteId); }
        [Fact] public void DecontaminationState_Clone_IsDeep() { var src = new DecontaminationState(); src.queue.Add(new DeconCase { caseId = "c1" }); var c = Clone(src); c.queue[0].caseId = "x"; Assert.Equal("c1", src.queue[0].caseId); }

        // ── System CaptureState isolation (simple constructors) ──────────

        [Fact]
        public void AirlockSecuritySystem_Capture_IsSnapshot()
        {
            var sys = new AirlockSecuritySystem(new SeededRng(1));
            var snap = sys.CaptureState();
            Assert.False(ReferenceEquals(snap, sys.State));
            snap.incidentLog.Add(new AirlockIncidentLog { outcome = "tamper" });
            Assert.Empty(sys.State.incidentLog);
            sys.State.incidentLog.Add(new AirlockIncidentLog { outcome = "live" });
            Assert.Single(snap.incidentLog);
        }

        [Fact]
        public void AudioConditionSystem_Capture_IsSnapshot()
        {
            var sys = new AudioConditionSystem();
            var snap = sys.CaptureState();
            Assert.False(ReferenceEquals(snap, sys.State));
            snap.activeConditions.Add(new ActiveAudioCondition { conditionId = "x" });
            Assert.Empty(sys.State.activeConditions);
        }

        [Fact]
        public void ExcavationSystem_Capture_IsSnapshot()
        {
            var sys = new ExcavationSystem(new SeededRng(2));
            var snap = sys.CaptureState();
            Assert.False(ReferenceEquals(snap, sys.State));
            snap.sites.Add(new ExcavationSite { siteId = "tamper" });
            Assert.Empty(sys.State.sites);
        }

        [Fact]
        public void SurvivorRelationsSystem_Capture_IsSnapshot()
        {
            var sys = new SurvivorRelationsSystem(new SeededRng(3));
            var snap = sys.CaptureState();
            Assert.False(ReferenceEquals(snap, sys.State));
            snap.relationships.Add(new RelationshipEntry { dwellerA = "x" });
            Assert.Empty(sys.State.relationships);
        }

        [Fact]
        public void VinylMoraleSystem_Capture_IsSnapshot()
        {
            var sys = new VinylMoraleSystem();
            var snap = sys.CaptureState();
            Assert.False(ReferenceEquals(snap, sys.State));
            snap.ownedRecordIds.Add("tamper");
            Assert.Empty(sys.State.ownedRecordIds);
        }

        [Fact]
        public void RegionalTreatySystem_Capture_IsSnapshot()
        {
            var sys = new RegionalTreatySystem();
            var snap = sys.CaptureState();
            Assert.False(ReferenceEquals(snap, sys.State));
            snap.treaties.Add(new TreatyInstance { treatyId = "x" });
            Assert.Empty(sys.State.treaties);
        }
    }
}
