// SPDX-License-Identifier: MIT
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Host-integration tests for the patrol radio bridge. The authored
    /// encounter→broadcast map stays in Core; the travel encounter owner keeps
    /// raising choices; the radio owner keeps the intercept log. This test proves
    /// the one-shot queue between them.
    /// </summary>
    public sealed class PlanPatrolRadioHostIntegrationTests
    {
        private const string PatrolEncounter = "enc_patrol_warlord_raid";
        private const string PatrolBroadcast = "radio_patrol_warlord_raid";

        [Fact]
        public void TheAuthoredMap_ResolvesPatrolEncountersToBroadcasts()
        {
            Assert.True(PatrolRadioHooks.TryGetRadioSignalForEncounter(PatrolEncounter, out string id));
            Assert.Equal(PatrolBroadcast, id);
        }

        [Fact]
        public void UnmappedEncounters_ProduceNoSignal()
        {
            bool found = PatrolRadioHooks.TryGetRadioSignalForEncounter("enc_not_a_patrol", out string id);

            Assert.False(found);
            Assert.Null(id); // TryGetValue leaves an unmapped out param untouched
        }

        [Fact]
        public void TheFactionCapabilityTable_HonoursAuthoredRowsOnly()
        {
            // Authored radio-capable row vs an authored non-radio row vs an id that
            // appears in neither table.
            Assert.True(PatrolRadioHooks.RadioCapableFactions.Contains("iron_garrison"));
            Assert.False(PatrolRadioHooks.RadioCapableFactions.Contains("faction_scavengers"));
            Assert.True(PatrolRadioHooks.IsFactionRadioCapable("iron_garrison"));
            Assert.False(PatrolRadioHooks.IsFactionRadioCapable("faction_not_real"));
            Assert.False(PatrolRadioHooks.IsFactionRadioCapable(string.Empty));
        }

        [Fact]
        public void QueueSignal_SuppressesDuplicatesInPendingAndConsumed()
        {
            var hooks = new PatrolRadioHooks(NullLog.Instance);

            Assert.True(hooks.QueueSignal(PatrolBroadcast));
            Assert.False(hooks.QueueSignal(PatrolBroadcast), "the same signal must not queue twice");
            Assert.Equal(1, hooks.PendingCount);
        }

        [Fact]
        public void TickRadio_IsOneShot()
        {
            var hooks = new PatrolRadioHooks(NullLog.Instance);
            hooks.QueueSignal(PatrolBroadcast);

            var first = hooks.TickRadio();
            var second = hooks.TickRadio();

            Assert.Single(first);
            Assert.Equal(PatrolBroadcast, first[0]);
            Assert.Empty(second);
            Assert.Equal(0, hooks.PendingCount);
        }

        [Fact]
        public void CaptureAndRestore_RoundTripsTheQueue()
        {
            var hooks = new PatrolRadioHooks(NullLog.Instance);
            hooks.QueueSignal(PatrolBroadcast);

            var captured = hooks.CaptureState();
            var reloaded = new PatrolRadioHooks(NullLog.Instance);
            reloaded.RestoreState(captured);

            Assert.Equal(1, reloaded.PendingCount);
            Assert.Equal(PatrolBroadcast, reloaded.PendingSignals[0]);
        }

        [Fact]
        public void RestoreState_SkipsSignalsAlreadyConsumed()
        {
            var hooks = new PatrolRadioHooks(NullLog.Instance);
            hooks.QueueSignal(PatrolBroadcast);
            hooks.TickRadio();

            var captured = hooks.CaptureState();
            var reloaded = new PatrolRadioHooks(NullLog.Instance);
            reloaded.RestoreState(captured);

            Assert.Equal(0, reloaded.PendingCount);
            Assert.Contains(PatrolBroadcast, reloaded.ConsumedSignals);
        }

        [Fact]
        public void ResetForTest_ClearsBothQueues()
        {
            var hooks = new PatrolRadioHooks(NullLog.Instance);
            hooks.QueueSignal(PatrolBroadcast);

            hooks.ResetForTest();

            Assert.Equal(0, hooks.PendingCount);
            Assert.Empty(hooks.ConsumedSignals);
        }
    }
}
