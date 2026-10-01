// SPDX-License-Identifier: MIT
// ASHFALL save-envelope checksum invariant suite.
//
// The retry probe and the hotfix rehearsal both assume a save's integrity hash
// is a pure function of its state, independent of serializer formatting,
// collection null-vs-empty, and the machine culture. This pins those invariants
// directly on SaveChecksum so a future codec/serializer change cannot silently
// make a save written by one host unreadable by the other.
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests.Save
{
    public sealed class SaveEnvelopeChecksumInvariantTests
    {
        private sealed class SampleEnvelope
        {
            public string? Name;
            public int Count;
            public List<string>? Tags;
            public float Ratio;
        }

        [Fact]
        public void Checksum_IsStableAcrossSerializerRoundTrip()
        {
            var original = new SampleEnvelope
            {
                Name = "holdfast",
                Count = 7,
                Tags = new List<string> { "a", "b", "c" },
                Ratio = 1.25f,
            };
            var serializer = new SystemTextJsonSerializer();
            var roundTripped = serializer.Deserialize<SampleEnvelope>(serializer.Serialize(original));

            Assert.NotNull(roundTripped);
            Assert.Equal(SaveChecksum.Compute(original), SaveChecksum.Compute(roundTripped!));
        }

        [Fact]
        public void Checksum_NormalizesNullStringToEmptyString()
        {
            var withNull = new SampleEnvelope { Name = null, Count = 1 };
            var withEmpty = new SampleEnvelope { Name = string.Empty, Count = 1 };
            Assert.Equal(SaveChecksum.Compute(withNull), SaveChecksum.Compute(withEmpty));
        }

        [Fact]
        public void Checksum_NormalizesNullCollectionToEmptyCollection()
        {
            var withNull = new SampleEnvelope { Tags = null, Count = 1 };
            var withEmpty = new SampleEnvelope { Tags = new List<string>(), Count = 1 };
            Assert.Equal(SaveChecksum.Compute(withNull), SaveChecksum.Compute(withEmpty));
        }

        [Fact]
        public void Checksum_ChangesWhenStateIsTampered()
        {
            var baseline = new SampleEnvelope { Name = "a", Count = 10, Ratio = 1f };
            var tampered = new SampleEnvelope { Name = "a", Count = 11, Ratio = 1f };
            Assert.NotEqual(SaveChecksum.Compute(baseline), SaveChecksum.Compute(tampered));
        }

        [Fact]
        public void Canonicalize_IsCultureInvariantForFloats()
        {
            var previous = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
                var dto = new SampleEnvelope { Name = "x", Count = 1, Ratio = 1.5f };
                string canonical = SaveChecksum.Canonicalize(dto);
                Assert.DoesNotContain("1,5", canonical);
                Assert.Contains("1.5", canonical);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = previous;
            }
        }
    }
}
