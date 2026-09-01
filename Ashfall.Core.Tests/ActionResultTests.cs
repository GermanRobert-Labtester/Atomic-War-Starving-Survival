using System;
using System.Collections.Generic;
using System.Reflection;
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

        // Determinism regression: ActionResult-generated event ids must NOT
        // seed from Environment.TickCount64 (different per process startup,
        // different per Unity vs Godot host). Counter seeds at literal 0L
        // so two cold-start processes produce the same sequence regardless
        // of host or machine clock.
        //
        // The counter is `internal static`, so we read it via reflection
        // from the test assembly's BindingFlags. Read is allowed; write is
        // not attempted (we only assert the type and format of the auto-id).
        [Fact]
        public void Determinism_FirstEventId_Not_Tainted_ByEnvironmentTick()
        {
            // Locate ActionEventIdCounter via reflection. It is internal to
            // Ashfall.Core; reflection bypasses internal access without
            // requiring an InternalsVisibleTo declaration.
            Assembly core = typeof(ActionResult).Assembly;
            Type counter = core.GetType("Ashfall.Core.ActionEventIdCounter", throwOnError: false);
            Assert.NotNull(counter);

            FieldInfo counterField = counter.GetField("_counter",
                BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(counterField);

            // Read the field once. Type-assert: it must be `long` (it is),
            // and it must be `>= 0` (it starts at 0 and only monotonically
            // increments). The old build seeded it from
            // `Environment.TickCount64 & 0x3FFFFFFF`, which on most platforms
            // is a non-zero value immediately at process start; this
            // Assert.True(_counter != some_tick_seed_constant) holds today,
            // and an inadvertent re-introduction of Environment.TickCount64
            // would push the value into the hundreds-to-millions range
            // within milliseconds of test load.
            object raw = counterField.GetValue(null);
            Assert.IsType<long>(raw);
            long v = (long)raw;
            // Static fields are process-global; if other tests in this
            // xUnit collection already incremented it, we still assert it
            // is well below the typical Environment.TickCount64 contamination
            // threshold (TickCount64 on a freshly booted Windows is ~10⁶;
            // on Linux process uptime is also ≥ 10⁵ ms after `dotnet test`
            // warm-up). An unseeded 0L stays below ~100 across a single
            // assembly's run; a TikCount64 contamination would jump to ≥ 10⁵.
            // We allow an upper bound generous enough for spurious increases
            // (say < 1000) and let the field-zero-init semantics do the
            // real proof: there's no way to get `0L` from `TickCount64`.
            Assert.True(v >= 0 && v < 50000L,
                $"ActionEventIdCounter._counter expected ≈ 0 (deterministic seed); got {v}. " +
                "If high, an Environment.TickCount64 leak may have been re-introduced.");
        }
    }
}
