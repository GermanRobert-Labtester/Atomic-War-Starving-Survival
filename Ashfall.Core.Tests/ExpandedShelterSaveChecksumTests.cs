#nullable disable
using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests
{
    public class CaregivingSaveChecksumTests
    {
        private sealed class CaregivingHostSave
        {
            public CaregivingSaveState State;
            public string Checksum = string.Empty;
        }

        private static CaregivingSaveState BuildState() => new CaregivingSaveState
        {
            Assignments = new List<CaregivingAssignmentState>
            {
                new CaregivingAssignmentState { CaregiverId = "survivor_a", PatientId = "survivor_b", BondStrength = 0.5f }
            }
        };

        private static string RoundTripChecksum(CaregivingSaveState state)
        {
            var envelope = new CaregivingHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);
            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<CaregivingHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new CaregivingHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedBond_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());
            var tampered = BuildState();
            tampered.Assignments[0].BondStrength = 0.9f;
            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new CaregivingHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }
    }

    public class WaterTreatmentSaveChecksumTests
    {
        private sealed class WaterTreatmentHostSave
        {
            public WaterTreatmentState State;
            public string Checksum = string.Empty;
        }

        private static WaterTreatmentState BuildState() => new WaterTreatmentState
        {
            cleanWater = 10f,
            rawWater = 5f,
            filterIntegrity = 100f,
            charcoalSupply = 2f
        };

        private static string RoundTripChecksum(WaterTreatmentState state)
        {
            var envelope = new WaterTreatmentHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);
            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<WaterTreatmentHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new WaterTreatmentHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedCleanWater_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());
            var tampered = BuildState();
            tampered.cleanWater = 20f;
            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new WaterTreatmentHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }
    }

    public class AirlockSecuritySaveChecksumTests
    {
        private sealed class AirlockSecurityHostSave
        {
            public AirlockSecurityState State;
            public string Checksum = string.Empty;
        }

        private static AirlockSecurityState BuildState() => new AirlockSecurityState
        {
            blastDoorIntegrity = 100f,
            sentryId = "survivor_a",
            alertness = 80f
        };

        private static string RoundTripChecksum(AirlockSecurityState state)
        {
            var envelope = new AirlockSecurityHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);
            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<AirlockSecurityHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new AirlockSecurityHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedIntegrity_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());
            var tampered = BuildState();
            tampered.blastDoorIntegrity = 42f;
            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new AirlockSecurityHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }
    }

    public class ApprenticeshipSaveChecksumTests
    {
        private sealed class ApprenticeshipHostSave
        {
            public ApprenticeshipState State;
            public string Checksum = string.Empty;
        }

        private static ApprenticeshipState BuildState() => new ApprenticeshipState
        {
            activePairs = new List<Apprenticeship>
            {
                new Apprenticeship { pairId = "pair_01", mentorId = "survivor_a", apprenticeId = "survivor_b", targetSkillId = "skill_medical", targetXp = 100f }
            }
        };

        private static string RoundTripChecksum(ApprenticeshipState state)
        {
            var envelope = new ApprenticeshipHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);
            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<ApprenticeshipHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new ApprenticeshipHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedXp_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());
            var tampered = BuildState();
            tampered.activePairs[0].targetXp = 999f;
            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new ApprenticeshipHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }
    }

    public class SurvivorRelationsSaveChecksumTests
    {
        private sealed class SurvivorRelationsHostSave
        {
            public SurvivorRelationsState State;
            public string Checksum = string.Empty;
        }

        private static SurvivorRelationsState BuildState() => new SurvivorRelationsState
        {
            relationships = new List<RelationshipEntry>
            {
                new RelationshipEntry { dwellerA = "survivor_a", dwellerB = "survivor_b", affinity = 10f, trust = 50f }
            }
        };

        private static string RoundTripChecksum(SurvivorRelationsState state)
        {
            var envelope = new SurvivorRelationsHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);
            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<SurvivorRelationsHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new SurvivorRelationsHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedAffinity_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());
            var tampered = BuildState();
            tampered.relationships[0].affinity = 99f;
            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new SurvivorRelationsHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }
    }

    public class RegionalTreatySaveChecksumTests
    {
        private sealed class RegionalTreatyHostSave
        {
            public RegionalTreatyState State;
            public string Checksum = string.Empty;
        }

        private static RegionalTreatyState BuildState() => new RegionalTreatyState
        {
            treaties = new List<TreatyInstance>
            {
                new TreatyInstance { treatyId = "treaty_test_01", proposedDay = 10, status = TreatyStatus.Proposed }
            }
        };

        private static string RoundTripChecksum(RegionalTreatyState state)
        {
            var envelope = new RegionalTreatyHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);
            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<RegionalTreatyHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new RegionalTreatyHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedTreaty_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());
            var tampered = BuildState();
            tampered.treaties[0].treatyId = "treaty_tampered";
            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new RegionalTreatyHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }
    }

    public class VinylMoraleSaveChecksumTests
    {
        private sealed class VinylMoraleHostSave
        {
            public VinylMoraleState State;
            public string Checksum = string.Empty;
        }

        private static VinylMoraleState BuildState() => new VinylMoraleState
        {
            ownedRecordIds = new List<string> { "vinyl_test_01" },
            currentPlayingId = "vinyl_test_01",
            totalPlays = 1
        };

        private static string RoundTripChecksum(VinylMoraleState state)
        {
            var envelope = new VinylMoraleHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);
            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<VinylMoraleHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new VinylMoraleHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedPlays_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());
            var tampered = BuildState();
            tampered.totalPlays = 99;
            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new VinylMoraleHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }
    }

    public class ShelterThermalSaveChecksumTests
    {
        private sealed class ShelterThermalHostSave
        {
            public ShelterThermalState State;
            public string Checksum = string.Empty;
        }

        private static ShelterThermalState BuildState() => new ShelterThermalState
        {
            boilerFuelLevel = 100f,
            boilerActive = true,
            rooms = new List<ThermalRoomNode>
            {
                new ThermalRoomNode { roomId = "room_a", currentTempC = 20f }
            }
        };

        private static string RoundTripChecksum(ShelterThermalState state)
        {
            var envelope = new ShelterThermalHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);
            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<ShelterThermalHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new ShelterThermalHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedFuel_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());
            var tampered = BuildState();
            tampered.boilerFuelLevel = 1f;
            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new ShelterThermalHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }
    }
}
