// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    public class RadioDistressSystemTests
    {
        [Fact]
        public void DistressLifecycle_Intercept_Triangulate_Dispatch_Resolve_WorksCleanly()
        {
            var system = new RadioDistressSystem();
            string signalId = "freq_distress_134_5"; // Relay 44 Elena Vasquez

            var def = system.GetDefinition(signalId);
            Assert.NotNull(def);
            Assert.Equal("survivor_elena_vasquez", def!.RecruitSurvivorId);

            // 1. Initial state
            var state = system.GetActiveState(signalId);
            Assert.NotNull(state);
            Assert.Equal(DistressSignalStatus.Inactive, state!.Status);

            // 2. Intercept
            bool intercepted = system.Intercept(signalId, day: 5);
            Assert.True(intercepted);
            Assert.Equal(DistressSignalStatus.Intercepted, state.Status);
            Assert.Equal(5, state.InterceptedDay);
            Assert.Equal(def.DaysToTrace, state.DaysRemaining);

            // 3. Triangulate
            bool triangulated = system.MarkTriangulated(signalId);
            Assert.True(triangulated);
            Assert.Equal(DistressSignalStatus.Triangulated, state.Status);
            Assert.True(state.IsTriangulated);

            // 4. Dispatch Expedition
            bool dispatched = system.DispatchExpedition(signalId);
            Assert.True(dispatched);
            Assert.Equal(DistressSignalStatus.Dispatched, state.Status);
            Assert.True(state.IsDispatched);

            // 5. Resolve with Survivor Recruit
            bool resolved = system.Resolve(signalId, DistressSignalStatus.ResolvedRescued, "Elena Vasquez safely escorted to shelter.");
            Assert.True(resolved);
            Assert.Equal(DistressSignalStatus.ResolvedRescued, state.Status);
            Assert.True(state.IsResolved);
            Assert.Contains("Elena Vasquez", state.ResolutionSummary);
        }

        [Fact]
        public void Distress_TickingDaily_DecrementsCountdown_AndExpiresOverdueSignal()
        {
            var system = new RadioDistressSystem();
            string signalId = "freq_distress_148_2"; // Bunker 4-East (3 days to trace)

            system.Intercept(signalId, day: 10);
            var state = system.GetActiveState(signalId);
            Assert.NotNull(state);
            Assert.Equal(3, state!.DaysRemaining);

            // Tick 1 day
            system.TickDaily(11);
            Assert.Equal(2, state.DaysRemaining);
            Assert.Equal(DistressSignalStatus.Intercepted, state.Status);

            // Tick 2 days
            system.TickDaily(12);
            Assert.Equal(1, state.DaysRemaining);

            // Tick 3 days -> Expires
            system.TickDaily(13);
            Assert.Equal(0, state.DaysRemaining);
            Assert.Equal(DistressSignalStatus.Expired, state.Status);
            Assert.Contains("expired", state.ResolutionSummary, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Distress_CaptureAndRestoreState_PreservesActiveAndResolvedSignals()
        {
            var system = new RadioDistressSystem();
            system.Intercept("freq_distress_217_4", day: 20);
            system.Resolve("freq_distress_217_4", DistressSignalStatus.ResolvedGrimTooLate, "Found Corporal Maren's final log.");

            system.Intercept("freq_distress_77_3", day: 21); // Pavel in cold store

            var captured = system.CaptureState();
            Assert.NotNull(captured);
            Assert.NotEmpty(captured);

            var newSystem = new RadioDistressSystem();
            newSystem.RestoreState(captured);

            var s217 = newSystem.GetActiveState("freq_distress_217_4");
            Assert.NotNull(s217);
            Assert.Equal(DistressSignalStatus.ResolvedGrimTooLate, s217!.Status);
            Assert.True(s217.IsResolved);

            var s77 = newSystem.GetActiveState("freq_distress_77_3");
            Assert.NotNull(s77);
            Assert.Equal(DistressSignalStatus.Intercepted, s77!.Status);
            Assert.Equal(21, s77.InterceptedDay);
        }
    }
}
