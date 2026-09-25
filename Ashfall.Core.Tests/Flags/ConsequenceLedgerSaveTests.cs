// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Ashfall.Core.Flags;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Flags
{
    /// <summary>
    /// Tests for the consequence ledger save section registration and
    /// lifecycle reset behavior added in WHOLEGAME-P1A-CORE-LOOP-FEEDBACK.
    /// Does NOT duplicate CampaignConsequenceLedgerTests (round-trip, clear,
    /// normalization) — focuses on the save/lifecycle integration seam.
    /// </summary>
    public class ConsequenceLedgerSaveTests
    {
        [Fact]
        public void SaveSectionRegistry_RegistersConsequenceLedgerSection()
        {
            var entry = SaveSectionRegistry.All.FirstOrDefault(s => s.SectionKey == "consequence_ledger");
            Assert.NotNull(entry);
            Assert.Equal("SaveConsequenceLedger", entry!.SaveMethod);
            Assert.Equal("SetupConsequenceLedger", entry.SetupMethod);
            Assert.Equal("campaign", entry.Owner);
        }

        [Fact]
        public void SaveSectionRegistry_HasConsequenceLedgerFileName()
        {
            Assert.True(SaveSectionRegistry.SectionFileNames.ContainsKey("consequence_ledger"));
            Assert.Equal("consequence_ledger_save.json", SaveSectionRegistry.SectionFileNames["consequence_ledger"]);
        }

        [Fact]
        public void LifecycleReset_ClearsAllFlagsCountersAndHistory()
        {
            var ledger = new CampaignConsequenceLedger();
            ledger.Set("flag_a", "test", "set", 1);
            ledger.Set("flag_b", "test", "set", 1);
            ledger.SetCounter("counter_x", 5, "test", "inc", 1);

            var state = ledger.CaptureState();
            Assert.Equal(2, state.flags.Count);
            Assert.Single(state.counters);
            Assert.Equal(3, state.history.Count);

            // Simulate lifecycle ResetAll -> onReset -> ClearAll
            ledger.ClearAll();

            var after = ledger.CaptureState();
            Assert.Empty(after.flags);
            Assert.Empty(after.counters);
            Assert.Empty(after.history);
        }

        [Fact]
        public void CaptureRestore_PreservesFlagsAcrossSaveLoad()
        {
            var original = new CampaignConsequenceLedger();
            original.Set("flag_quest_prereq", "quest", "complete", 3);
            original.SetCounter("counter_moral_choices", 7, "moral", "choose", 3);

            var state = original.CaptureState();

            // Simulate disk round-trip via JSON serialization
            var json = System.Text.Json.JsonSerializer.Serialize(state);
            var deserialized = System.Text.Json.JsonSerializer.Deserialize<CampaignConsequenceSaveState>(json);
            Assert.NotNull(deserialized);

            var restored = new CampaignConsequenceLedger();
            restored.RestoreState(deserialized);

            Assert.True(restored.IsSet("flag_quest_prereq"));
            Assert.Equal(7, restored.GetCounter("counter_moral_choices"));
        }
    }
}
