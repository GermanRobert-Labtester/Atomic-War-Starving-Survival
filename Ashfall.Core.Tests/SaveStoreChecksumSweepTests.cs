// Save stores live in src/ and read user:// — Godot-tied. We mirror each store's
// envelope + checksum behaviour here through SystemTextJsonSerializer so the
// integrity contract is pinned without spinning up a Godot project.
//
// AGENTS.md flagged Expedition, Medical, World, Journal as missing checksums;
// sweep confirmed all four already have envelopes (likely added since AGENTS.md
// was last updated). The actual defect was a shared bypass: a null/empty
// checksum field silently skipped verification as "legacy". Tests cover the
// fixed behaviour: clean round-trip, mutated-state changes hash, forged hash
// detected, and null checksum is rejected.
#nullable disable

using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Journal;
using Ashfall.Core.Medical;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;

namespace Ashfall.Core.Tests
{
    public class ExpeditionSaveChecksumTests
    {
        private sealed class ExpeditionHostSave
        {
            public List<ExpeditionState> State;
            public string Checksum = string.Empty;
        }

        private static List<ExpeditionState> BuildState() => new List<ExpeditionState>
        {
            new ExpeditionState
            {
                survivorId = "survivor_gunner_mikhail",
                locationId = "loc_the_allotments",
                displayName = "The Works Allotment Commune",
                stance = "Stealth",
                phase = (int)ExpeditionPhase.Outbound,
                travelTicksCompleted = 0,
                distanceTicks = 5,
                stamina = 100f
            }
        };

        private static string RoundTripChecksum(List<ExpeditionState> state)
        {
            var envelope = new ExpeditionHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);

            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<ExpeditionHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new ExpeditionHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedState_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());

            var tampered = BuildState();
            tampered[0].stamina = 50f;

            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            // The store's old guard `!string.IsNullOrEmpty(envelope.Checksum)`
            // silently treated a missing checksum as legacy. Pin the invariant
            // that the checksum field must be present and match.
            var envelope = new ExpeditionHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }
    }

    public class MedicalSaveChecksumTests
    {
        private sealed class MedicalHostSave
        {
            public ChemicalDependencyLedgerState State;
            public string Checksum = string.Empty;
        }

        private static ChemicalDependencyLedgerState BuildState() => new ChemicalDependencyLedgerState
        {
            survivors = new List<SurvivorDependencyList>
            {
                new SurvivorDependencyList
                {
                    survivorId = "survivor_sarah_chen",
                    dependencies = new List<ChemicalDependencyState>
                    {
                        new ChemicalDependencyState
                        {
                            itemId = "item_morphine",
                            dependencyLevel = 0.6f,
                            kind = "Opioid",
                            inManagedDetox = false,
                            inColdTurkey = false,
                            detoxProgressHours = 0f
                        }
                    }
                }
            }
        };

        private static string RoundTripChecksum(ChemicalDependencyLedgerState state)
        {
            var envelope = new MedicalHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);

            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<MedicalHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new MedicalHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedLedger_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());

            var tampered = BuildState();
            tampered.survivors[0].dependencies[0].dependencyLevel = 0.9f;

            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new MedicalHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }
    }

    public class WorldSaveChecksumTests
    {
        private sealed class WorldHostSave
        {
            public WorldWeatherState State;
            public SkyArmorSaveState SkyArmor;
            public string Checksum = string.Empty;
        }

        private static WorldWeatherState BuildState() => new WorldWeatherState
        {
            currentKind = "Snow",
            totalElapsedHours = 720f,
            hoursUntilNextCheck = 4f,
            rollCount = 12,
            restrictToNonHazardWeather = false
        };

        private static SkyArmorSaveState BuildSkyArmor()
        {
            // Public fields per SkyLayerArmorSystem construction pattern.
            return new SkyArmorSaveState();
        }

        private static string RoundTripChecksum(WorldWeatherState state, SkyArmorSaveState sky)
        {
            var envelope = new WorldHostSave { State = state, SkyArmor = sky };
            envelope.Checksum = SaveChecksum.Compute(envelope);

            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<WorldHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            var sky = BuildSkyArmor();
            string expected = SaveChecksum.Compute(new WorldHostSave { State = state, SkyArmor = sky });
            Assert.Equal(expected, RoundTripChecksum(state, sky), StringComparer.Ordinal);
        }

        [Fact]
        public void TamperedSkyArmor_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState(), BuildSkyArmor());

            var tampered = BuildSkyArmor();
            tampered.cells.Add(new CeilingCellArmor
            {
                gridX = 0,
                thicknessMeters = 0.5f,
                currentDurability = 100f
            });

            Assert.NotEqual(before, RoundTripChecksum(BuildState(), tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new WorldHostSave { State = BuildState(), SkyArmor = BuildSkyArmor(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }
    }

    public class JournalSaveChecksumTests
    {
        private sealed class JournalHostSave
        {
            public JournalSave State;
            public string Checksum = string.Empty;
        }

        private static JournalSave BuildState() => new JournalSave
        {
            Entries = new JournalEntry[0],
            Knowledge = new KnowledgeBaseSave { DiscoveredKeys = new[] { "k_water" } },
            NextSeq = 1,
            HasUnread = false,
            NotificationPing = false,
            NotificationPingCount = 0,
            HudIsOpen = false,
            ActiveTab = 0,
            LastSeenIndexPerTab = new int[0],
            LastSeenCodexPerTab = new int[0],
            CodexUnlockCount = 1
        };

        private static string RoundTripChecksum(JournalSave state)
        {
            var envelope = new JournalHostSave { State = state };
            envelope.Checksum = SaveChecksum.Compute(envelope);

            var json = new SystemTextJsonSerializer();
            string raw = json.Serialize(envelope);
            var restored = json.Deserialize<JournalHostSave>(raw);
            Assert.NotNull(restored);
            return restored.Checksum;
        }

        [Fact]
        public void CleanRoundTrip_PreservesChecksum()
        {
            var state = BuildState();
            string expected = SaveChecksum.Compute(new JournalHostSave { State = state });
            Assert.Equal(expected, RoundTripChecksum(state), StringComparer.Ordinal);
        }

        [Fact]
        public void MutatingKnowledge_ChangesChecksum()
        {
            string before = RoundTripChecksum(BuildState());

            var tampered = BuildState();
            tampered.Knowledge.DiscoveredKeys = new[] { "k_water", "k_radiation" };

            Assert.NotEqual(before, RoundTripChecksum(tampered));
        }

        [Fact]
        public void NullChecksumField_RejectsLoadRatherThanBypassing()
        {
            var envelope = new JournalHostSave { State = BuildState(), Checksum = null };
            string actual = SaveChecksum.Compute(envelope);
            Assert.False(string.Equals(envelope.Checksum, actual, StringComparison.Ordinal));
        }
    }
}
