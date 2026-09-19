// SPDX-License-Identifier: MIT
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class FactionWarClockTests
    {
        [Fact]
        public void ToAuthoredDay_MapsPlayableYearOfAshOntoWarChainEpoch()
        {
            Assert.Equal(480, FactionWarChainRunner.ToAuthoredDay(180));
            Assert.Equal(607, FactionWarChainRunner.ToAuthoredDay(307));
            Assert.Equal(660, FactionWarChainRunner.ToAuthoredDay(360));
        }
    }
}
