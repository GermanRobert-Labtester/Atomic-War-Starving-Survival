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
    }
}
