// SPDX-License-Identifier: MIT
// Task #132 — Canonical survivor identity.
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// The canonical identity of one campaign survivor.
    ///
    /// <para>
    /// Before this type existed, twenty-eight independent stores each accepted a
    /// bare <see cref="string"/> and decided locally whether a survivor existed.
    /// A <c>SurvivorId</c> cannot be constructed from a value the data authority
    /// would never author, so a malformed id fails at the boundary instead of
    /// silently becoming a twenty-ninth survivor nobody can find again.
    /// </para>
    ///
    /// <para><b>Grammar</b> — lowercase snake_case: <c>^[a-z0-9_]+$</c>, no leading
    /// or trailing underscore, no empty segment (<c>__</c>), at most
    /// <see cref="MaxLength"/> characters. Verified against every id value in
    /// <c>Assets/StreamingAssets/Data</c> (4749 values): zero uppercase, zero
    /// dashes, zero double underscores, longest 63 characters. Deliberately does
    /// <b>not</b> require a prefix — only 28 of the 129 authored survivors use
    /// <c>survivor_</c>; 69 use <c>the_</c> and 32 are bare given names.
    /// </para>
    ///
    /// <para><b>Normalization</b> — none. A non-canonical value is rejected, never
    /// rewritten. Lowercasing on the way in would let two distinct authored ids
    /// collapse into one survivor, which is precisely the failure this type
    /// exists to prevent. Because no authored id contains uppercase, rejection
    /// costs nothing: over the set of valid ids, ordinal and case-insensitive
    /// comparison are equivalent, so no existing comparer in the repository can
    /// disagree with <c>SurvivorId</c> equality.
    /// </para>
    ///
    /// <para><b>Determinism</b> — equality and ordering are ordinal and
    /// culture-independent (AGENTS.md Invariant 4). Never use
    /// <see cref="string.GetHashCode()"/> output as persisted identity; the
    /// serialized form is <see cref="Value"/> itself.
    /// </para>
    /// </summary>
    [JsonConverter(typeof(SurvivorIdJsonConverter))]
    public readonly struct SurvivorId : IEquatable<SurvivorId>, IComparable<SurvivorId>
    {
        /// <summary>Longest id the data authority may author. Longest observed is 63.</summary>
        public const int MaxLength = 64;

        private readonly string? _value;

        /// <summary>
        /// The canonical id text. Never null: <c>default(SurvivorId)</c> yields
        /// <see cref="string.Empty"/> so unset ids cannot throw on read.
        /// </summary>
        public string Value => _value ?? string.Empty;

        /// <summary>True for <c>default(SurvivorId)</c> — the absence of an identity.</summary>
        public bool IsEmpty => string.IsNullOrEmpty(_value);

        /// <summary>The absence of an identity. Never resolves to an aggregate.</summary>
        public static readonly SurvivorId None = default;

        /// <summary>
        /// Construct a canonical survivor id. Throws when <paramref name="value"/>
        /// violates the grammar — callers holding untrusted input should use
        /// <see cref="TryParse(string, out SurvivorId, out string)"/> instead.
        /// </summary>
        /// <exception cref="ArgumentException">The value is not a canonical survivor id.</exception>
        [JsonConstructor]
        public SurvivorId(string value)
        {
            if (!IsValid(value, out string error))
                throw new ArgumentException(error, nameof(value));
            _value = value;
        }

        private SurvivorId(string value, bool _) => _value = value;

        // ── Validation ─────────────────────────────────────────────────

        /// <summary>
        /// True when <paramref name="value"/> is a canonical survivor id.
        /// <paramref name="error"/> carries a specific, actionable reason on failure.
        /// </summary>
        public static bool IsValid(string? value, out string error)
        {
            if (value == null)
            {
                error = "SurvivorId cannot be null.";
                return false;
            }
            if (value.Length == 0)
            {
                error = "SurvivorId cannot be empty.";
                return false;
            }
            if (value.Length > MaxLength)
            {
                error = $"SurvivorId '{value}' is {value.Length} characters; the maximum is {MaxLength}.";
                return false;
            }
            if (value[0] == '_' || value[value.Length - 1] == '_')
            {
                error = $"SurvivorId '{value}' must not start or end with an underscore.";
                return false;
            }

            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                bool ok = (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == '_';
                if (!ok)
                {
                    error = (c >= 'A' && c <= 'Z')
                        ? $"SurvivorId '{value}' contains uppercase '{c}'. Survivor ids are lowercase snake_case; ids are never case-normalized because that would merge two distinct survivors."
                        : $"SurvivorId '{value}' contains invalid character '{c}'. Only lowercase letters, digits, and underscore are allowed.";
                    return false;
                }
                if (c == '_' && i > 0 && value[i - 1] == '_')
                {
                    error = $"SurvivorId '{value}' contains an empty segment ('__').";
                    return false;
                }
            }

            error = string.Empty;
            return true;
        }

        /// <summary>True when <paramref name="value"/> is a canonical survivor id.</summary>
        public static bool IsValid(string? value) => IsValid(value, out _);

        /// <summary>
        /// Parse without throwing. The compatibility boundary for code still
        /// carrying raw strings (Phase 61) — validate once at the edge, then pass
        /// the <see cref="SurvivorId"/> inward.
        /// </summary>
        public static bool TryParse(string? value, out SurvivorId id, out string error)
        {
            if (!IsValid(value, out error))
            {
                id = None;
                return false;
            }
            id = new SurvivorId(value!, true);
            return true;
        }

        /// <summary>Parse without throwing, discarding the reason on failure.</summary>
        public static bool TryParse(string? value, out SurvivorId id) => TryParse(value, out id, out _);

        /// <summary>Parse or throw. Equivalent to the constructor; reads better at call sites.</summary>
        public static SurvivorId Parse(string value) => new SurvivorId(value);

        // ── Value semantics ────────────────────────────────────────────

        public bool Equals(SurvivorId other) => string.Equals(Value, other.Value, StringComparison.Ordinal);

        public override bool Equals(object? obj) => obj is SurvivorId other && Equals(other);

        public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Value);

        /// <summary>Ordinal ordering — the canonical deterministic survivor order.</summary>
        public int CompareTo(SurvivorId other) => string.CompareOrdinal(Value, other.Value);

        public override string ToString() => Value;

        public static bool operator ==(SurvivorId left, SurvivorId right) => left.Equals(right);
        public static bool operator !=(SurvivorId left, SurvivorId right) => !left.Equals(right);
        public static bool operator <(SurvivorId left, SurvivorId right) => left.CompareTo(right) < 0;
        public static bool operator >(SurvivorId left, SurvivorId right) => left.CompareTo(right) > 0;
        public static bool operator <=(SurvivorId left, SurvivorId right) => left.CompareTo(right) <= 0;
        public static bool operator >=(SurvivorId left, SurvivorId right) => left.CompareTo(right) >= 0;

        // No custom IEqualityComparer / IComparer is provided, deliberately.
        // Because this struct implements IEquatable<SurvivorId> and
        // IComparable<SurvivorId>, EqualityComparer<SurvivorId>.Default and
        // List.Sort() already use the ordinal semantics above without boxing, so a
        // hand-written comparer would only duplicate them — and forwarding to
        // id.GetHashCode() inside one would trip Core's determinism gate, which
        // bans parameterless GetHashCode() calls because .NET randomizes string
        // hashing per process.
    }

    /// <summary>
    /// Serializes <see cref="SurvivorId"/> as a bare JSON string.
    ///
    /// <para>
    /// Deliberately unlike <c>SaveIdJsonConverter</c>, which wraps
    /// <c>SaveProfileId</c>/<c>SaveSlotId</c> in <c>{"Value":"..."}</c>. Every
    /// existing survivor id on disk — roster entries, needs/radiation slices,
    /// fate records, memorials, admissions, expeditions, duty rows — is a plain
    /// string, so the bare form keeps all of those save slices byte-compatible
    /// when their fields are migrated to <see cref="SurvivorId"/>.
    /// </para>
    /// </summary>
    public sealed class SurvivorIdJsonConverter : JsonConverter<SurvivorId>
    {
        public override SurvivorId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return SurvivorId.None;

            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException(
                    $"SurvivorId must be a JSON string; found {reader.TokenType}.");

            string? raw = reader.GetString();
            if (string.IsNullOrEmpty(raw))
                return SurvivorId.None;

            if (!SurvivorId.TryParse(raw, out var id, out string error))
                throw new JsonException(error);

            return id;
        }

        public override void Write(Utf8JsonWriter writer, SurvivorId value, JsonSerializerOptions options)
        {
            if (value.IsEmpty) writer.WriteNullValue();
            else writer.WriteStringValue(value.Value);
        }

        /// <summary>Survivor ids also appear as dictionary keys in component stores.</summary>
        public override void WriteAsPropertyName(Utf8JsonWriter writer, SurvivorId value, JsonSerializerOptions options)
            => writer.WritePropertyName(value.Value);

        public override SurvivorId ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? raw = reader.GetString();
            if (!SurvivorId.TryParse(raw, out var id, out string error))
                throw new JsonException(error);
            return id;
        }
    }
}
