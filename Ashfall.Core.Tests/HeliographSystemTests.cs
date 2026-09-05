using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class HeliographSystemTests
    {
        [Fact]
        public void BlockedLineOfSightDoesNotDeliverOrDiscover()
        {
            bool discovered = false;
            var system = new HeliographSystem(
                hasLineOfSight: (_, _) => false,
                visibility01: () => 1f,
                discoverMapNode: _ => discovered = true);
            Assert.True(system.RegisterStation("heli_a", "node_a"));
            Assert.True(system.RegisterStation("heli_b", "node_b"));

            var result = system.Transmit("msg_1", "heli_a", "heli_b", "status_check", 4, "node_hidden");

            Assert.False(result.IsSuccess);
            Assert.False(discovered);
            Assert.Equal((int)HeliographMessageStatus.Blocked, system.State.messages[0].status);
            Assert.Equal(0, system.State.delivered_count);
        }

        [Fact]
        public void WeatherVisibilityBlocksTransmissionWithoutChangingStationState()
        {
            var system = new HeliographSystem(
                hasLineOfSight: (_, _) => true,
                visibility01: () => 0.2f);
            Assert.True(system.RegisterStation("heli_a", "node_a", 90f));
            Assert.True(system.RegisterStation("heli_b", "node_b", 90f));

            Assert.False(system.Transmit("msg_1", "heli_a", "heli_b", "status_check", 4).IsSuccess);
            Assert.Equal(90f, system.GetStation("heli_a")!.condition);
            Assert.Equal("weather_visibility_blocked", system.State.messages[0].block_reason);
        }

        [Fact]
        public void SuccessfulTransmissionDiscoversMapNodeAndHandsOffDistress()
        {
            bool discovered = false;
            string? dispatched = null;
            int delivered = 0;
            var system = new HeliographSystem(
                hasLineOfSight: (_, _) => true,
                visibility01: () => 1f,
                isMapNodeKnown: _ => false,
                discoverMapNode: id => discovered = id == "node_hidden",
                dispatchDistress: id =>
                {
                    dispatched = id;
                    return true;
                });
            system.OnMessageDelivered += _ => delivered++;
            Assert.True(system.RegisterStation("heli_a", "node_a"));
            Assert.True(system.RegisterStation("heli_b", "node_b"));

            Assert.True(system.Transmit(
                "msg_1", "heli_a", "heli_b", "distress_ack", 7, "node_hidden", "distress_7").IsSuccess);

            Assert.True(discovered);
            Assert.Equal("distress_7", dispatched);
            Assert.Equal(1, delivered);
            Assert.Equal(1, system.State.delivered_count);
        }

        [Fact]
        public void DuplicateMessageIsIdempotentlyRejected()
        {
            var system = new HeliographSystem(
                hasLineOfSight: (_, _) => true,
                visibility01: () => 1f);
            Assert.True(system.RegisterStation("heli_a", "node_a"));
            Assert.True(system.RegisterStation("heli_b", "node_b"));
            Assert.True(system.Transmit("msg_1", "heli_a", "heli_b", "status_check", 1).IsSuccess);

            Assert.False(system.Transmit("msg_1", "heli_a", "heli_b", "status_check", 2).IsSuccess);
            Assert.Single(system.State.messages);
            Assert.Equal(1, system.State.delivered_count);
        }

        [Fact]
        public void SaveRoundTripPreservesDeliveredMessage()
        {
            var system = new HeliographSystem(
                hasLineOfSight: (_, _) => true,
                visibility01: () => 1f);
            Assert.True(system.RegisterStation("heli_a", "node_a"));
            Assert.True(system.RegisterStation("heli_b", "node_b"));
            Assert.True(system.Transmit("msg_1", "heli_a", "heli_b", "status_check", 1).IsSuccess);

            var restored = new HeliographSystem(
                hasLineOfSight: (_, _) => true,
                visibility01: () => 1f);
            restored.RestoreState(system.CaptureState());

            Assert.Equal(1, restored.State.delivered_count);
            Assert.Equal((int)HeliographMessageStatus.Delivered, restored.State.messages[0].status);
        }
    }
}
