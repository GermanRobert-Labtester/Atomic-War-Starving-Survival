// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Gate for the deterministic hashing utility (Invariant 4). String.GetHashCode()
    /// is randomized per process in .NET Core; every simulation key / dedup id must
    /// use StableHash so same-seed simulations produce identical keys across runs
    /// and across hosts.
    /// </summary>
    public class StableHashTests
    {
        [Fact]
        public void Of_IsDeterministicAcrossCalls()
        {
            Assert.Equal(StableHash.Of("radio_broadcast_doctrine"), StableHash.Of("radio_broadcast_doctrine"));
            Assert.Equal(StableHash.Of(""), StableHash.Of(""));
            Assert.Equal(StableHash.Of(null), StableHash.Of(""));
        }

        [Fact]
        public void Of_DistinguishesDistinctInputs()
        {
            var a = StableHash.Of("intercept_salt_flat");
            var b = StableHash.Of("intercept_salt_plateau");
            Assert.NotEqual(a, b);
        }

        [Fact]
        public void Of_IsCaseSensitive()
        {
            Assert.NotEqual(StableHash.Of("flag_verdict_call_resolved"),
                            StableHash.Of("FLAG_VERDICT_CALL_RESOLVED"));
        }

        [Fact]
        public void Combine_HasPinnedCrossProcessValues()
        {
            // Independent constants pin the algorithm. A future edit to the
            // mixer must be deliberate because it can change seeded outcomes.
            Assert.Equal(-1935725075, StableHash.Combine(42, "node_alpha"));
            Assert.Equal(-2045881680, StableHash.Combine(42, "node_beta"));
            Assert.Equal(788022903, StableHash.Combine(42, int.MinValue));
            Assert.Equal(2080549329, StableHash.Combine(7, 123));
            Assert.Equal(912762830, StableHash.Combine(7, 1234567890123L));
        }

        [Theory]
        [InlineData(-1, 7, 6)]
        [InlineData(int.MinValue, 7, 5)]
        [InlineData(-8, 4, 0)]
        [InlineData(8, 4, 0)]
        public void NonNegativeRemainder_IsBoundedAndIntMinSafe(int value, int modulus, int expected)
        {
            Assert.Equal(expected, StableHash.NonNegativeRemainder(value, modulus));
        }

        [Fact]
        public void NonNegativeRemainder_RejectsNonPositiveModulus()
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => StableHash.NonNegativeRemainder(1, 0));
            Assert.Throws<System.ArgumentOutOfRangeException>(() => StableHash.NonNegativeRemainder(1, -1));
        }
    }
}
