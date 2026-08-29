// SPDX-License-Identifier: MIT
// Task #132 — Canonical survivor identity contract.
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public class SurvivorIdTests : CatalogTestBase
    {
        // ── Construction and validation ────────────────────────────────

        [Theory]
        [InlineData("elena_vasquez")]
        [InlineData("survivor_dr_sarah_chen")]
        [InlineData("the_surgeon")]
        [InlineData("a")]
        [InlineData("survivor_2")]
        [InlineData("a_b_c_d_e")]
        public void Accepts_CanonicalSnakeCase(string raw)
        {
            var id = new SurvivorId(raw);
            Assert.Equal(raw, id.Value);
            Assert.False(id.IsEmpty);
        }

        [Theory]
        [InlineData(null, "null")]
        [InlineData("", "empty")]
        [InlineData(" ", "invalid character")]
        [InlineData("Elena_Vasquez", "uppercase")]
        [InlineData("ELENA", "uppercase")]
        [InlineData("elena-vasquez", "invalid character")]
        [InlineData("elena vasquez", "invalid character")]
        [InlineData("elena.vasquez", "invalid character")]
        [InlineData("elena/vasquez", "invalid character")]
        [InlineData("_elena", "underscore")]
        [InlineData("elena_", "underscore")]
        [InlineData("elena__vasquez", "empty segment")]
        [InlineData("élena", "invalid character")]
        public void Rejects_NonCanonical(string? raw, string expectedReasonFragment)
        {
            Assert.False(SurvivorId.IsValid(raw, out string error));
            Assert.Contains(expectedReasonFragment, error, StringComparison.OrdinalIgnoreCase);

            var ex = Assert.Throws<ArgumentException>(() => new SurvivorId(raw!));
            Assert.Contains(expectedReasonFragment, ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Rejects_OverMaxLength()
        {
            string tooLong = new string('a', SurvivorId.MaxLength + 1);
            Assert.False(SurvivorId.IsValid(tooLong, out string error));
            Assert.Contains("maximum", error, StringComparison.OrdinalIgnoreCase);

            string atLimit = new string('a', SurvivorId.MaxLength);
            Assert.True(SurvivorId.IsValid(atLimit));
        }

        /// <summary>
        /// The design decision that uppercase is rejected rather than lowercased.
        /// Normalizing would merge two distinct authored ids into one survivor,
        /// which is the exact failure the type exists to prevent.
        /// </summary>
        [Fact]
        public void Uppercase_IsRejected_NotNormalized()
        {
            Assert.False(SurvivorId.TryParse("Elena_Vasquez", out var id, out string error));
            Assert.True(id.IsEmpty);
            Assert.Contains("never case-normalized", error);
        }

        // ── Value semantics ────────────────────────────────────────────

        [Fact]
        public void Equality_IsOrdinal()
        {
            var a = new SurvivorId("elena_vasquez");
            var b = new SurvivorId("elena_vasquez");
            var c = new SurvivorId("marcus_olejnik");

            Assert.Equal(a, b);
            Assert.True(a == b);
            Assert.False(a != b);
            Assert.NotEqual(a, c);
            Assert.True(a != c);
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void Equality_AgainstOtherTypesAndBoxing()
        {
            var a = new SurvivorId("elena_vasquez");
            Assert.False(a.Equals("elena_vasquez"));
            Assert.False(a.Equals(null));
            Assert.True(a.Equals((object)new SurvivorId("elena_vasquez")));
        }

        [Fact]
        public void Default_IsEmptyAndNeverThrows()
        {
            SurvivorId none = default;
            Assert.True(none.IsEmpty);
            Assert.Equal(string.Empty, none.Value);
            Assert.Equal(string.Empty, none.ToString());
            Assert.Equal(SurvivorId.None, none);
            Assert.Equal(none.GetHashCode(), SurvivorId.None.GetHashCode());
        }

        [Fact]
        public void Ordering_IsOrdinalAndDeterministic()
        {
            var a = new SurvivorId("a_one");
            var b = new SurvivorId("b_two");

            Assert.True(a < b);
            Assert.True(b > a);
            Assert.True(a <= new SurvivorId("a_one"));
            Assert.True(a >= new SurvivorId("a_one"));
            Assert.True(a.CompareTo(b) < 0);
            Assert.Equal(0, a.CompareTo(new SurvivorId("a_one")));
        }

        /// <summary>
        /// Underscore is 0x5F and lowercase letters are 0x61..0x7A, so ordinal
        /// ordering places an underscore before any letter. Pinned because a
        /// culture-aware comparer would order these differently and silently change
        /// simulation order across machines.
        /// </summary>
        [Fact]
        public void Ordering_UnderscoreSortsBeforeLetters_Ordinal()
        {
            var ids = new List<SurvivorId>
            {
                new SurvivorId("the_vet"),
                new SurvivorId("the_veteran"),
                new SurvivorId("thea_one")
            };
            ids.Sort();

            Assert.Equal("the_vet", ids[0].Value);
            Assert.Equal("the_veteran", ids[1].Value);
            Assert.Equal("thea_one", ids[2].Value);
        }

        [Fact]
        public void Sorting_IsStableAcrossInsertionOrders()
        {
            var forward = new List<SurvivorId>
            {
                new SurvivorId("a_one"), new SurvivorId("m_two"), new SurvivorId("z_three")
            };
            var reverse = new List<SurvivorId>
            {
                new SurvivorId("z_three"), new SurvivorId("m_two"), new SurvivorId("a_one")
            };

            forward.Sort();
            reverse.Sort();
            Assert.Equal(forward, reverse);
        }

        [Fact]
        public void WorksAsDictionaryKey_WithDefaultComparer()
        {
            // No custom comparer is supplied anywhere: IEquatable<SurvivorId> gives
            // the default comparer ordinal semantics without boxing.
            var map = new Dictionary<SurvivorId, int>()
            {
                [new SurvivorId("elena_vasquez")] = 1
            };

            Assert.True(map.ContainsKey(new SurvivorId("elena_vasquez")));
            Assert.False(map.ContainsKey(new SurvivorId("marcus_olejnik")));
            Assert.Equal(1, map[new SurvivorId("elena_vasquez")]);
        }

        // ── Parsing boundary ───────────────────────────────────────────

        [Fact]
        public void TryParse_DoesNotThrow_AndReportsReason()
        {
            Assert.True(SurvivorId.TryParse("elena_vasquez", out var ok, out string noError));
            Assert.Equal("elena_vasquez", ok.Value);
            Assert.Equal(string.Empty, noError);

            Assert.False(SurvivorId.TryParse("Bad Id", out var bad, out string error));
            Assert.True(bad.IsEmpty);
            Assert.NotEqual(string.Empty, error);
        }

        [Fact]
        public void Parse_ThrowsOnInvalid()
        {
            Assert.Equal("the_hunter", SurvivorId.Parse("the_hunter").Value);
            Assert.Throws<ArgumentException>(() => SurvivorId.Parse("Not Canonical"));
        }

        // ── Serialization ──────────────────────────────────────────────

        private sealed class Holder
        {
            public SurvivorId Id { get; set; }
        }

        /// <summary>
        /// Must serialize as a bare string, not <c>{"Value":"..."}</c>. Every survivor
        /// id already on disk is a plain string, so the bare form is what keeps
        /// existing save slices loadable once their fields move to SurvivorId.
        /// </summary>
        [Fact]
        public void Json_SerializesAsBareString()
        {
            string json = JsonSerializer.Serialize(new Holder { Id = new SurvivorId("elena_vasquez") });
            Assert.Contains("\"elena_vasquez\"", json);
            Assert.DoesNotContain("\"Value\"", json);
        }

        [Fact]
        public void Json_RoundTripsExactly()
        {
            var original = new Holder { Id = new SurvivorId("survivor_gunner_mikhail") };
            string json = JsonSerializer.Serialize(original);
            var restored = JsonSerializer.Deserialize<Holder>(json);

            Assert.NotNull(restored);
            Assert.Equal(original.Id, restored!.Id);
            Assert.Equal(json, JsonSerializer.Serialize(restored));
        }

        [Fact]
        public void Json_NullAndEmptyBecomeNone()
        {
            Assert.True(JsonSerializer.Deserialize<Holder>("{\"Id\":null}")!.Id.IsEmpty);
            Assert.True(JsonSerializer.Deserialize<Holder>("{\"Id\":\"\"}")!.Id.IsEmpty);
        }

        [Fact]
        public void Json_NoneSerializesAsNull()
        {
            string json = JsonSerializer.Serialize(new Holder { Id = SurvivorId.None });
            Assert.Contains("null", json);
        }

        [Fact]
        public void Json_RejectsNonCanonicalString()
        {
            var ex = Assert.Throws<JsonException>(
                () => JsonSerializer.Deserialize<Holder>("{\"Id\":\"Elena_Vasquez\"}"));
            Assert.Contains("uppercase", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Json_RejectsNonStringToken()
        {
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Holder>("{\"Id\":42}"));
        }

        [Fact]
        public void Json_RoundTripsAsDictionaryKey()
        {
            var map = new Dictionary<SurvivorId, int>()
            {
                [new SurvivorId("the_surgeon")] = 7
            };

            string json = JsonSerializer.Serialize(map);
            Assert.Contains("\"the_surgeon\"", json);

            var restored = JsonSerializer.Deserialize<Dictionary<SurvivorId, int>>(json);
            Assert.NotNull(restored);
            Assert.Equal(7, restored![new SurvivorId("the_surgeon")]);
        }

        // ── Grammar vs the real data authority ─────────────────────────

        /// <summary>
        /// The grammar was derived from authored content, so it must accept all of
        /// it. If someone authors a survivor id this type rejects, this test fails
        /// before the campaign does.
        /// </summary>
        [Fact]
        public void EveryAuthoredSurvivorId_IsCanonical()
        {
            string path = Path.Combine(DataDirectory, "survivors.json");
            Assert.True(File.Exists(path), $"survivors.json not found at {path}");

            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            var rows = doc.RootElement.TryGetProperty("survivors", out var wrapped)
                ? wrapped
                : doc.RootElement;

            Assert.Equal(JsonValueKind.Array, rows.ValueKind);

            int checkedCount = 0;
            var rejected = new List<string>();

            foreach (var row in rows.EnumerateArray())
            {
                if (!row.TryGetProperty("id", out var idProp)) continue;
                string? raw = idProp.GetString();
                checkedCount++;
                if (!SurvivorId.IsValid(raw, out string error)) rejected.Add(error);
            }

            Assert.True(checkedCount > 100, $"expected the full survivor catalog, saw {checkedCount} ids");
            Assert.Empty(rejected);
        }

        [Fact]
        public void EveryStartingSurvivorId_IsCanonical()
        {
            string path = Path.Combine(DataDirectory, "starting_survivors.json");
            Assert.True(File.Exists(path), $"starting_survivors.json not found at {path}");

            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            // The starting roster wraps its rows under "starting_survivors".
            var rows = doc.RootElement.TryGetProperty("starting_survivors", out var wrapped)
                ? wrapped
                : doc.RootElement.TryGetProperty("survivors", out var alt)
                    ? alt
                    : doc.RootElement;

            Assert.Equal(JsonValueKind.Array, rows.ValueKind);

            int checkedCount = 0;
            foreach (var row in rows.EnumerateArray())
            {
                if (!row.TryGetProperty("id", out var idProp)) continue;
                checkedCount++;
                Assert.True(
                    SurvivorId.IsValid(idProp.GetString(), out string error),
                    error);
            }

            Assert.True(checkedCount > 0, "starting roster had no ids");
        }

        /// <summary>
        /// Prefix validation was deliberately left out: only 28 of 129 authored
        /// survivors use <c>survivor_</c>. Pinned so nobody "tightens" the grammar
        /// and locks 101 survivors out of the campaign.
        /// </summary>
        [Theory]
        [InlineData("the_surgeon")]
        [InlineData("elena_vasquez")]
        [InlineData("survivor_dr_sarah_chen")]
        public void Grammar_DoesNotRequireAPrefix(string raw)
        {
            Assert.True(SurvivorId.IsValid(raw));
        }
    }
}
