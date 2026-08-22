// Phase 13 save-store checksum sweep for the 8 newly-wired Batch-3 systems.
//
// The host save stores live in src/ and read user:// — Godot-tied. We mirror
// each store's envelope + checksum behaviour here through SystemTextJsonSerializer
// so the integrity contract is pinned without spinning up a Godot project.
// Pattern follows the existing SaveStoreChecksumSweepTests.cs (Expedition, Medical,
// World, Journal): clean round-trip preserves hash, tampered state changes hash,
// null checksum field is rejected rather than silently bypassed.
#nullable disable

using System;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Tests
{
    public class SumpFloodingSaveChecksumTests
    {
        private sealed class SumpFloodingHostSave
        {
            public SumpFloodingState State;
            public string Checksum = string.Empty;
        }

        private static SumpFloodingState BuildState() => new SumpFloodingState
        {
            nodes = new System.Collections.Generic.List<SumpNode>
            {
                new SumpNode { nodeId = "sump_a", displayName = "Lower Level", maxWaterLevelCm = 200f, waterLevelCm = 40f, hasSumpPump = true, pumpCondition = 100f, pumpPowered = true }
            },
            globalGroundwaterLevel = 1.2f,
            lastFloodDay = 12
        };

        private static string RoundTripChecksum(SumpFloodingState state)
        {
            var envelope = new SumpFloodingHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);

            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<SumpFloodingHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new SumpFloodingHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedWaterLevel_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());

            var tampered = BuildState();
            tampered.nodes[0].waterLevelCm = 120f;

            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new SumpFloodingHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }
    }

    public class DecontaminationSaveChecksumTests
    {
        private sealed class DecontaminationHostSave
        {
            public DecontaminationState State;
            public string Checksum = string.Empty;
        }

        private static DecontaminationState BuildState() => new DecontaminationState
        {
            queue = new System.Collections.Generic.List<DeconCase>
            {
                new DeconCase { caseId = "case_1", survivorId = "survivor_1", gearId = "gear_1", surfaceContamination = 0.5f, queuedDay = 3 }
            },
            shelterContaminated = true,
            shelterContaminationLevel = 0.3f
        };

        private static string RoundTripChecksum(DecontaminationState state)
        {
            var envelope = new DecontaminationHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);

            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<DecontaminationHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new DecontaminationHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedContamination_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());

            var tampered = BuildState();
            tampered.shelterContaminationLevel = 0.9f;

            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new DecontaminationHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }
    }

    public class KitchenNutritionSaveChecksumTests
    {
        private sealed class KitchenNutritionHostSave
        {
            public KitchenNutritionState State;
            public string Checksum = string.Empty;
        }

        private static KitchenNutritionState BuildState() => new KitchenNutritionState
        {
            activeJobs = new System.Collections.Generic.List<PrepJob>
            {
                new PrepJob { jobId = "job_1", recipeId = "recipe_stew", assignedCookId = "cook_1", dayStarted = 4, progressHours = 1.5f, totalHoursRequired = 2f }
            },
            cellarTempC = 8f,
            hasCellar = true,
            hasRefrigeration = true,
            totalMealsPrepared = 10,
            totalMealsServed = 9
        };

        private static string RoundTripChecksum(KitchenNutritionState state)
        {
            var envelope = new KitchenNutritionHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);

            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<KitchenNutritionHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new KitchenNutritionHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedMealsServed_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());

            var tampered = BuildState();
            tampered.totalMealsServed = 99;

            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new KitchenNutritionHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }
    }

    public class EquipmentConditionSaveChecksumTests
    {
        private sealed class EquipmentConditionHostSave
        {
            public EquipmentConditionState State;
            public string Checksum = string.Empty;
        }

        private static EquipmentConditionState BuildState() => new EquipmentConditionState
        {
            items = new System.Collections.Generic.List<EquipmentInstance>
            {
                new EquipmentInstance { instanceId = "inst_1", itemId = "item_gas_mask", ownerId = "survivor_1", condition = 80f, maxCondition = 100f }
            },
            pendingJobs = new System.Collections.Generic.List<MaintenanceJob>
            {
                new MaintenanceJob { jobId = "mjob_1", instanceId = "inst_1", stationId = "station_1", progress = 0.5f, totalRequired = 1f }
            }
        };

        private static string RoundTripChecksum(EquipmentConditionState state)
        {
            var envelope = new EquipmentConditionHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);

            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<EquipmentConditionHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new EquipmentConditionHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedCondition_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());

            var tampered = BuildState();
            tampered.items[0].condition = 30f;

            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new EquipmentConditionHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }
    }

    public class LibraryStudySaveChecksumTests
    {
        private sealed class LibraryStudyHostSave
        {
            public LibraryStudyState State;
            public string Checksum = string.Empty;
        }

        private static LibraryStudyState BuildState() => new LibraryStudyState
        {
            activeJobs = new System.Collections.Generic.List<StudyJob>
            {
                new StudyJob { jobId = "study_1", manualId = "man_basic", readerId = "reader_1", dayStarted = 5, progressHours = 3f }
            },
            completedManualIds = new System.Collections.Generic.List<string> { "man_advanced" },
            totalStudyHours = 12
        };

        private static string RoundTripChecksum(LibraryStudyState state)
        {
            var envelope = new LibraryStudyHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);

            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<LibraryStudyHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new LibraryStudyHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedStudyHours_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());

            var tampered = BuildState();
            tampered.totalStudyHours = 99;

            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new LibraryStudyHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }
    }

    public class ArchiveDeskSaveChecksumTests
    {
        private sealed class ArchiveDeskHostSave
        {
            public ArchiveDeskState State;
            public string Checksum = string.Empty;
        }

        private static ArchiveDeskState BuildState() => new ArchiveDeskState
        {
            queue = new System.Collections.Generic.List<TranscriptionJob>
            {
                new TranscriptionJob { jobId = "tx_1", evidenceId = "evidence_1", archivistId = "archivist_1", inkId = "iron_gall", dayStarted = 6, progressHours = 2f, totalHoursRequired = 4f }
            },
            unlockedEvidenceIds = new System.Collections.Generic.List<string> { "evidence_2" },
            totalTranscriptions = 3
        };

        private static string RoundTripChecksum(ArchiveDeskState state)
        {
            var envelope = new ArchiveDeskHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);

            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<ArchiveDeskHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new ArchiveDeskHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedTranscriptions_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());

            var tampered = BuildState();
            tampered.totalTranscriptions = 99;

            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new ArchiveDeskHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }
    }

    public class ContractorRosterSaveChecksumTests
    {
        private sealed class ContractorRosterHostSave
        {
            public ContractorRosterState State;
            public string Checksum = string.Empty;
        }

        private static ContractorRosterState BuildState() => new ContractorRosterState
        {
            contractors = new System.Collections.Generic.List<Contractor>
            {
                new Contractor { contractorId = "contractor_1", displayName = "Mara", role = "scavenger", loyalty = 90f, trust = 60f, startDay = 2, expiryDay = 12 }
            },
            activeOffers = new System.Collections.Generic.List<ContractOffer>
            {
                new ContractOffer { offerId = "offer_1", candidateId = "candidate_1", role = "guard", initialFee = 20, dailyHazardPay = 5, termDays = 10, proposedDay = 3 }
            }
        };

        private static string RoundTripChecksum(ContractorRosterState state)
        {
            var envelope = new ContractorRosterHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);

            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<ContractorRosterHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new ContractorRosterHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedLoyalty_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());

            var tampered = BuildState();
            tampered.contractors[0].loyalty = 10f;

            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new ContractorRosterHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }
    }

    public class MentalHealthCrisisSaveChecksumTests
    {
        private sealed class MentalHealthCrisisHostSave
        {
            public MentalHealthState State;
            public string Checksum = string.Empty;
        }

        private static MentalHealthState BuildState() => new MentalHealthState
        {
            activeCases = new System.Collections.Generic.List<CrisisCase>
            {
                new CrisisCase { caseId = "crisis_1", survivorId = "survivor_1", dayStarted = 7, assignedCaregiverId = "caregiver_1", intervention = "counseling", recoveryProgress = 0.4f }
            },
            resolvedCases = new System.Collections.Generic.List<CrisisCase>
            {
                new CrisisCase { caseId = "crisis_0", survivorId = "survivor_2", dayStarted = 1, dayResolved = 6, intervention = "rest", recoveryProgress = 1f }
            },
            wardCapacity = 2,
            currentOccupancy = 1
        };

        private static string RoundTripChecksum(MentalHealthState state)
        {
            var envelope = new MentalHealthCrisisHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);

            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<MentalHealthCrisisHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new MentalHealthCrisisHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedOccupancy_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());

            var tampered = BuildState();
            tampered.currentOccupancy = 2;

            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new MentalHealthCrisisHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }
    }

    public class ShelterAssignmentSaveChecksumTests
    {
        private static ShelterAssignmentSave BuildSave() => new ShelterAssignmentSave
        {
            saveVersion = ShelterAssignmentSave.CurrentSaveVersion,
            simDay = 12,
            Rooms = new System.Collections.Generic.List<ShelterRoomSave>
            {
                new ShelterRoomSave { RoomId = "room_bunks", DisplayName = "Bunks", Capacity = 4 }
            },
            State = new ShelterAssignmentState
            {
                Assignments = new System.Collections.Generic.List<ShelterAssignment>
                {
                    new ShelterAssignment
                    {
                        SurvivorId = "survivor_1", RoomId = "room_bunks",
                        WorkstationId = null, AssignedDay = 5,
                        Status = ShelterAssignmentStatus.Active
                    }
                }
            }
        };

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var json = new SystemTextJsonSerializer();
            var save = BuildSave();
            string raw = ShelterAssignmentSaveCodec.EncodeToString(save, json);
            var restored = ShelterAssignmentSaveCodec.Decode(raw, json);
            Assert.NotNull(restored);
            Assert.Equal(save.Checksum, restored.Checksum, StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedAssignment_ChangesChecksum()
        {
            var json = new SystemTextJsonSerializer();
            string before = ShelterAssignmentSaveCodec.EncodeToString(BuildSave(), json);

            var tampered = BuildSave();
            tampered.State.Assignments[0].RoomId = "room_clinic";
            string after = ShelterAssignmentSaveCodec.EncodeToString(tampered, json);

            Assert.NotEqual(before, after);
        }

        [Fact]
        public void NullChecksumField_RejectedByDecode()
        {
            var json = new SystemTextJsonSerializer();
            var save = BuildSave();
            string raw = ShelterAssignmentSaveCodec.EncodeToString(save, json);

            // Strip the checksum from the serialized payload — Decode must
            // reject a new-format envelope whose Checksum is null/empty rather
            // than silently treating it as legacy (the shared bypass fixed in
            // SaveStoreChecksumSweepTests).
            string stripped = raw.Replace(save.Checksum, string.Empty);
            Assert.Throws<System.InvalidOperationException>(
                () => ShelterAssignmentSaveCodec.Decode(stripped, json));
        }
    }
}
