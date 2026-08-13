// The fixtures below deliberately model save DTOs, which are plain nullable public fields
// (Ashfall.Core builds with <Nullable>disable</Nullable>). Null is the value under test here.
#nullable disable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Pins audit finding C-2: "a save written by one host MUST load in the other".
    ///
    /// The old scheme hashed pretty-printed JSON text, so Unity's JsonUtility (4-space indent,
    /// null string written as "") and System.Text.Json (2-space indent, null written as null)
    /// produced different hashes for identical state, and each host hard-rejected the other's
    /// saves as corrupt. These tests hold the replacement to the property that actually matters:
    /// equal state hashes equally, no matter which serializer materialized the object.
    /// </summary>
    public class SaveChecksumTests
    {
        private sealed class Nested
        {
            public string Label;
            public int Count;
        }

        private enum Phase { Before = 0, During = 1, After = 2 }

        private sealed class Snapshot
        {
            public string Checksum;
            public string Name;
            public int Day;
            public float Temperature;
            public double Precise;
            public bool Paused;
            public Phase Stage;
            public List<string> Tags;
            public List<Nested> Children;
        }

        private sealed class Swapped
        {
            public string Alpha;
            public string Beta;
        }

        private sealed class Cyclic
        {
            public Cyclic Next;
        }

        private static Snapshot Sample() => new Snapshot
        {
            Checksum = "",
            Name = "holdfast",
            Day = 42,
            Temperature = -17.5f,
            Precise = 0.1 + 0.2,
            Paused = true,
            Stage = Phase.During,
            Tags = new List<string> { "ice", "road" },
            Children = new List<Nested> { new Nested { Label = "clerk", Count = 3 } }
        };

        [Fact]
        public void IdenticalStateHashesIdentically()
        {
            Assert.Equal(SaveChecksum.Compute(Sample()), SaveChecksum.Compute(Sample()));
        }

        [Fact]
        public void NullStringHashesTheSameAsEmptyString()
        {
            // System.Text.Json yields null here; Unity's JsonUtility yields "". Same file,
            // different in-memory object, and the hash must not be able to tell.
            var fromSystemTextJson = Sample();
            fromSystemTextJson.Name = null;
            var fromJsonUtility = Sample();
            fromJsonUtility.Name = "";

            Assert.Equal(SaveChecksum.Compute(fromJsonUtility), SaveChecksum.Compute(fromSystemTextJson));
        }

        [Fact]
        public void NullCollectionHashesTheSameAsEmptyCollection()
        {
            var fromSystemTextJson = Sample();
            fromSystemTextJson.Tags = null;
            var fromJsonUtility = Sample();
            fromJsonUtility.Tags = new List<string>();

            Assert.Equal(SaveChecksum.Compute(fromJsonUtility), SaveChecksum.Compute(fromSystemTextJson));
        }

        [Fact]
        public void CrossHostRoundTripAgrees()
        {
            // The whole point, stated end to end: the Godot host parses null/null where the Unity
            // host parses ""/[], and both must accept the same stored checksum.
            var godotSideParse = Sample();
            godotSideParse.Name = null;
            godotSideParse.Tags = null;

            var unitySideParse = Sample();
            unitySideParse.Name = "";
            unitySideParse.Tags = new List<string>();

            string written = SaveChecksum.Compute(unitySideParse);
            Assert.Equal(written, SaveChecksum.Compute(godotSideParse));
        }

        [Fact]
        public void RootChecksumFieldIsExcluded()
        {
            // The hash is written into this field, so including it would be unsatisfiable.
            var a = Sample();
            var b = Sample();
            b.Checksum = "deadbeef";

            Assert.Equal(SaveChecksum.Compute(a), SaveChecksum.Compute(b));
        }

        [Theory]
        [InlineData("Name")]
        [InlineData("Day")]
        [InlineData("Temperature")]
        [InlineData("Paused")]
        [InlineData("Stage")]
        [InlineData("Tags")]
        [InlineData("Children")]
        public void MutatingAnyFieldChangesTheHash(string field)
        {
            string baseline = SaveChecksum.Compute(Sample());
            var mutated = Sample();

            switch (field)
            {
                case "Name": mutated.Name = "waystation"; break;
                case "Day": mutated.Day = 43; break;
                case "Temperature": mutated.Temperature = -17.4f; break;
                case "Paused": mutated.Paused = false; break;
                case "Stage": mutated.Stage = Phase.After; break;
                case "Tags": mutated.Tags.Add("extra"); break;
                case "Children": mutated.Children[0].Count = 4; break;
            }

            Assert.NotEqual(baseline, SaveChecksum.Compute(mutated));
        }

        [Fact]
        public void SwappingTwoFieldValuesChangesTheHash()
        {
            // Would collide if field names were omitted and only values concatenated.
            string first = SaveChecksum.Compute(new Swapped { Alpha = "x", Beta = "y" });
            string second = SaveChecksum.Compute(new Swapped { Alpha = "y", Beta = "x" });

            Assert.NotEqual(first, second);
        }

        [Fact]
        public void StringContentCannotForgeDelimiters()
        {
            // Length-prefixed strings: no payload can imitate the field/element separators.
            string first = SaveChecksum.Compute(new Swapped { Alpha = "a,Beta=b", Beta = "" });
            string second = SaveChecksum.Compute(new Swapped { Alpha = "a", Beta = "b" });

            Assert.NotEqual(first, second);
        }

        [Fact]
        public void HashIsCultureIndependent()
        {
            // A comma decimal separator must not change the hash of a float field.
            CultureInfo original = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                string invariantSide = SaveChecksum.Compute(Sample());
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
                string commaSide = SaveChecksum.Compute(Sample());

                Assert.Equal(invariantSide, commaSide);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = original;
            }
        }

        [Fact]
        public void ListOrderIsSignificant()
        {
            var forward = Sample();
            var reversed = Sample();
            reversed.Tags = new List<string> { "road", "ice" };

            Assert.NotEqual(SaveChecksum.Compute(forward), SaveChecksum.Compute(reversed));
        }

        [Fact]
        public void ReferenceCycleFailsLoudlyRatherThanOverflowing()
        {
            var node = new Cyclic();
            node.Next = node;

            Assert.Throws<InvalidOperationException>(() => SaveChecksum.Compute(node));
        }

        [Fact]
        public void ChecksumIsLowercaseHexSha256()
        {
            string checksum = SaveChecksum.Compute(Sample());

            Assert.Equal(64, checksum.Length);
            Assert.All(checksum, c => Assert.True("0123456789abcdef".IndexOf(c) >= 0, $"unexpected char '{c}'"));
        }
    }
}
