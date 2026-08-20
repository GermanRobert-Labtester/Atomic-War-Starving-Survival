using System;
using System.Collections.Generic;
using Ashfall.Core.Warlords;
using Xunit;

namespace Ashfall.Core.Tests.Warlords
{
    public class WarlordResponseActionsTests
    {
        private static WarlordResponseActions Make()
        {
            return new WarlordResponseActions(
                new WarlordResponseState(),
                new WarlordDoctrineSystem(seedSalt: 11));
        }

        [Fact]
        public void Pay_RecordsResponse()
        {
            var a = Make();
            var r = a.Pay("tribute_a", 50, 5);
            Assert.True(r.Succeeded);
            Assert.True(r.Record.SettledFully);
            Assert.Equal(50, r.Record.AmountPaid);
        }

        [Fact]
        public void Pay_IsIdempotent()
        {
            var a = Make();
            a.Pay("tribute_a", 50, 5);
            var second = a.Pay("tribute_a", 50, 5);
            Assert.False(second.Succeeded);
            Assert.Equal("already_responded", second.ReasonCode);
        }

        [Fact]
        public void Contest_RecordsResponse()
        {
            var a = Make();
            var r = a.Contest("tribute_b", 6);
            Assert.True(r.Succeeded);
            Assert.Equal(WarlordResponseKind.Contest, r.Record.Kind);
        }

        [Fact]
        public void Submit_RecordsResponse()
        {
            var a = Make();
            var r = a.Submit("tribute_c", 7);
            Assert.True(r.Succeeded);
            Assert.Equal(WarlordResponseKind.Submit, r.Record.Kind);
            Assert.True(r.Record.SettledFully);
        }

        [Fact]
        public void DifferentTributes_BothRespond()
        {
            var a = Make();
            a.Pay("tribute_a", 10, 1);
            a.Contest("tribute_b", 1);
            Assert.True(a.IsResponded("tribute_a"));
            Assert.True(a.IsResponded("tribute_b"));
        }

        [Fact]
        public void ValidatesInputs()
        {
            var a = Make();
            Assert.False(a.Pay("", 1, 1).Succeeded);
            Assert.False(a.Pay("tribute_a", -1, 1).Succeeded);
            Assert.False(a.Contest("", 1).Succeeded);
            Assert.False(a.Submit("", 1).Succeeded);
        }

        [Fact]
        public void CaptureRestore_RoundTrip()
        {
            var a = Make();
            a.Pay("tribute_a", 100, 5);
            var save = a.CaptureState();
            var fresh = new WarlordResponseActions(new WarlordResponseState(),
                new WarlordDoctrineSystem(seedSalt: 11));
            fresh.RestoreState(save);
            Assert.True(fresh.IsResponded("tribute_a"));
        }

        [Fact]
        public void Events_FireOnResponse()
        {
            var a = Make();
            WarlordResponseRecord? captured = null;
            a.OnResponded += r => captured = r;
            a.Pay("tribute_x", 10, 3);
            Assert.NotNull(captured);
            Assert.Equal("tribute_x", captured.TributeId);
        }
    }
}
