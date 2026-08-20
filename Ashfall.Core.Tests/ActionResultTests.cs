using System.Collections.Generic;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ActionResultTests
    {
        [Fact]
        public void Success_Creates_SuccessStatus()
        {
            var r = ActionResult.Success("action.ok");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.Equal("action.ok", r.MessageKey);
            Assert.True(r.IsSuccessOrPartial);
        }

        [Fact]
        public void Success_WithDeltas()
        {
            var deltas = new Dictionary<string, double> { { "scrap", -5 }, { "research_progress", 0.25 } };
            var r = ActionResult.Success("research.complete", deltas, "evt-001");
            Assert.Equal("evt-001", r.EventId);
            Assert.Equal(-5, r.Deltas["scrap"]);
            Assert.Equal(0.25, r.Deltas["research_progress"]);
        }

        [Fact]
        public void Blocked_SetsFailureCode()
        {
            var r = ActionResult.Blocked("insufficient_scrap", "action.need_more_scrap");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
            Assert.Equal("insufficient_scrap", r.FailureCode);
            Assert.Equal("action.need_more_scrap", r.MessageKey);
            Assert.False(r.IsSuccessOrPartial);
        }

        [Fact]
        public void Failed_NoDeltas()
        {
            var r = ActionResult.Failed("unexpected_error", "action.error");
            Assert.Equal(ActionResult.StatusKind.Failed, r.Status);
            Assert.Equal("unexpected_error", r.FailureCode);
            Assert.NotNull(r.Deltas);
            Assert.Empty(r.Deltas);
        }

        [Fact]
        public void Cancelled_EmptyFailureCode()
        {
            var r = ActionResult.Cancelled("action.cancelled");
            Assert.Equal(ActionResult.StatusKind.Cancelled, r.Status);
            Assert.Empty(r.FailureCode);
            Assert.False(r.IsSuccessOrPartial);
        }

        [Fact]
        public void Partial_IsSuccessOrPartial()
        {
            var r = ActionResult.Partial("action.partial", new Dictionary<string, double> { { "scrap", 3 } });
            Assert.Equal(ActionResult.StatusKind.Partial, r.Status);
            Assert.True(r.IsSuccessOrPartial);
            Assert.Equal(3, r.Deltas["scrap"]);
        }

        [Fact]
        public void ToString_IncludesStatusAndMessage()
        {
            var r = ActionResult.Success("action.ok");
            var s = r.ToString();
            Assert.Contains("Success", s);
            Assert.Contains("action.ok", s);
        }

        [Fact]
        public void InnerEventId_TracksCompositeAction()
        {
            var inner = ActionResult.Success("inner.ok", eventId: "evt-inner");
            var outer = ActionResult.Success("outer.ok", eventId: "evt-outer", innerEventId: inner.EventId);
            Assert.Equal("evt-outer", outer.EventId);
            Assert.Equal("evt-inner", outer.InnerEventId);
        }
    }
}
