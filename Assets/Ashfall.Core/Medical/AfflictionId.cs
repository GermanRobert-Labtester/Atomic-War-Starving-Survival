// SPDX-License-Identifier: MIT
// Task #133 — Canonical affliction identity (definition + episode).
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// The canonical identity of one affliction <b>type</b> (definition), e.g.
    /// <c>affliction_respiratory_degeneration</c>. Mirrors the
    /// <see cref="Survivors.SurvivorId"/> discipline: lowercase snake_case,
    /// ordinal equality, no normalization, reject-don't-rewrite, stable JSON.
    ///
    /// <para>Deliberately separate from <see cref="AfflictionEpisodeId"/>: a
    /// definition is authored content; an episode is one active case of that
    /// condition in one survivor. Two wounds are two episodes of one definition.
    /// Conflating them makes "second broken arm" unrepresentable.</para>
    /// </summary>
    [JsonConverter(typeof(AfflictionIdJsonConverter))]
    public readonly struct AfflictionId : IEquatable<AfflictionId>, IComparable<AfflictionId>
    {
        public const int MaxLength = 64;

        private readonly string? _value;

        /// <summary>The canonical id text. Never null; default yields empty.</summary>
        public string Value => _value ?? string.Empty;

        /// <summary>True for <c>default(AfflictionId)</c>.</summary>
        public bool IsEmpty => string.IsNullOrEmpty(_value);

        /// <summary>The absence of an affliction identity.</summary>
        public static readonly AfflictionId None = default;

        /// <summary>Construct and validate. Throws on non-canonical input.</summary>
        [JsonConstructor]
        public AfflictionId(string value)
        {
            if (!IsValid(value, out string error))
                throw new ArgumentException(error, nameof(value));
            _value = value;
        }

        private AfflictionId(string value, bool _) => _value = value;

        public static bool IsValid(string? value, out string error)
        {
            if (value == null) { error = "AfflictionId cannot be null."; return false; }
            if (value.Length == 0) { error = "AfflictionId cannot be empty."; return false; }
            if (value.Length > MaxLength)
            {
                error = $"AfflictionId '{value}' is {value.Length} characters; the maximum is {MaxLength}.";
                return false;
            }
            if (value[0] == '_' || value[value.Length - 1] == '_')
            {
                error = $"AfflictionId '{value}' must not start or end with an underscore.";
                return false;
            }
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                bool ok = (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == '_';
                if (!ok)
                {
                    error = (c >= 'A' && c <= 'Z')
                        ? $"AfflictionId '{value}' contains uppercase '{c}'. Affliction ids are lowercase snake_case and never case-normalized."
                        : $"AfflictionId '{value}' contains invalid character '{c}'. Only lowercase letters, digits, and underscore are allowed.";
                    return false;
                }
                if (c == '_' && i > 0 && value[i - 1] == '_')
                {
                    error = $"AfflictionId '{value}' contains an empty segment ('__').";
                    return false;
                }
            }
            error = string.Empty;
            return true;
        }

        public static bool IsValid(string? value) => IsValid(value, out _);

        public static bool TryParse(string? value, out AfflictionId id, out string error)
        {
            if (!IsValid(value, out error)) { id = None; return false; }
            id = new AfflictionId(value!, true);
            return true;
        }

        public static bool TryParse(string? value, out AfflictionId id) => TryParse(value, out id, out _);

        public static AfflictionId Parse(string value) => new AfflictionId(value);

        public bool Equals(AfflictionId other) => string.Equals(Value, other.Value, StringComparison.Ordinal);
        public override bool Equals(object? obj) => obj is AfflictionId other && Equals(other);
        public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Value);
        public int CompareTo(AfflictionId other) => string.CompareOrdinal(Value, other.Value);
        public override string ToString() => Value;

        public static bool operator ==(AfflictionId left, AfflictionId right) => left.Equals(right);
        public static bool operator !=(AfflictionId left, AfflictionId right) => !left.Equals(right);
    }

    /// <summary>
    /// One active case ("episode") of an affliction definition in one survivor.
    ///
    /// <para>Deterministic composite identity — never a Guid, never a hashcode.
    /// Conditions the game models as once-per-survivor (respiratory degeneration,
    /// radiation sickness) use ordinal 0. Conditions that may recur or multiply
    /// (future wounds) increment the ordinal per new episode in deterministic
    /// order. The serialized form is the composite text itself, so a save written
    /// by any host restores the same identity.</para>
    /// </summary>
    [JsonConverter(typeof(AfflictionEpisodeIdJsonConverter))]
    public readonly struct AfflictionEpisodeId : IEquatable<AfflictionEpisodeId>, IComparable<AfflictionEpisodeId>
    {
        /// <summary>Separator between survivor, definition, and ordinal segments.</summary>
        public const char Separator = ':';

        private readonly string? _value;

        /// <summary>The composite id text: <c>{survivorId}:{definitionId}:{ordinal}</c>.</summary>
        public string Value => _value ?? string.Empty;

        /// <summary>True for <c>default(AfflictionEpisodeId)</c>.</summary>
        public bool IsEmpty => string.IsNullOrEmpty(_value);

        /// <summary>The absence of an episode identity.</summary>
        public static readonly AfflictionEpisodeId None = default;

        /// <summary>
        /// Construct from an already-validated composite. Prefer
        /// <see cref="Create"/> / <see cref="TryParse"/> at boundaries.
        /// </summary>
        [JsonConstructor]
        public AfflictionEpisodeId(string value)
        {
            if (!IsValid(value, out string error))
                throw new ArgumentException(error, nameof(value));
            _value = value;
        }

        private AfflictionEpisodeId(string value, bool _) => _value = value;

        /// <summary>Deterministically derive the episode id for a survivor's Nth case of a definition.</summary>
        public static AfflictionEpisodeId Create(Survivors.SurvivorId survivor, AfflictionId definition, int ordinal = 0)
        {
            if (survivor.IsEmpty)
                throw new ArgumentException("Episode requires a survivor.", nameof(survivor));
            if (definition.IsEmpty)
                throw new ArgumentException("Episode requires an affliction definition.", nameof(definition));
            if (ordinal < 0)
                throw new ArgumentOutOfRangeException(nameof(ordinal), "Ordinal cannot be negative.");
            return new AfflictionEpisodeId(
                survivor.Value + Separator + definition.Value + Separator + ordinal.ToString(System.Globalization.CultureInfo.InvariantCulture), true);
        }

        public static bool IsValid(string? value, out string error)
        {
            if (value == null) { error = "AfflictionEpisodeId cannot be null."; return false; }
            if (value.Length == 0) { error = "AfflictionEpisodeId cannot be empty."; return false; }

            // Must be exactly three non-empty segments; segment 1 and 2 are
            // canonical snake_case ids, segment 3 is a non-negative integer.
            int first = value.IndexOf(Separator);
            int last = value.LastIndexOf(Separator);
            if (first <= 0 || last == first || last == value.Length - 1)
            {
                error = $"AfflictionEpisodeId '{value}' must be '{{survivorId}}:{{definitionId}}:{{ordinal}}'.";
                return false;
            }
            string survivor = value.Substring(0, first);
            string definition = value.Substring(first + 1, last - first - 1);
            string ordinalText = value.Substring(last + 1);

            if (!Survivors.SurvivorId.IsValid(survivor, out error)) return false;
            if (!AfflictionId.IsValid(definition, out error)) return false;
            if (!int.TryParse(ordinalText, System.Globalization.NumberStyles.None,
                    System.Globalization.CultureInfo.InvariantCulture, out int ordinal) || ordinal < 0)
            {
                error = $"AfflictionEpisodeId '{value}' ordinal '{ordinalText}' is not a non-negative integer.";
                return false;
            }
            error = string.Empty;
            return true;
        }

        public static bool IsValid(string? value) => IsValid(value, out _);

        public static bool TryParse(string? value, out AfflictionEpisodeId id, out string error)
        {
            if (!IsValid(value, out error)) { id = None; return false; }
            id = new AfflictionEpisodeId(value!, true);
            return true;
        }

        public static bool TryParse(string? value, out AfflictionEpisodeId id) => TryParse(value, out id, out _);

        /// <summary>The survivor segment of the composite.</summary>
        public Survivors.SurvivorId Survivor
        {
            get
            {
                if (IsEmpty) return Survivors.SurvivorId.None;
                int first = Value.IndexOf(Separator);
                Survivors.SurvivorId.TryParse(Value.Substring(0, first), out var sv);
                return sv;
            }
        }

        /// <summary>The affliction-definition segment of the composite.</summary>
        public AfflictionId Definition
        {
            get
            {
                if (IsEmpty) return AfflictionId.None;
                int first = Value.IndexOf(Separator);
                int last = Value.LastIndexOf(Separator);
                AfflictionId.TryParse(Value.Substring(first + 1, last - first - 1), out var def);
                return def;
            }
        }

        /// <summary>The episode ordinal segment of the composite.</summary>
        public int Ordinal
        {
            get
            {
                if (IsEmpty) return 0;
                int last = Value.LastIndexOf(Separator);
                return int.TryParse(Value.Substring(last + 1), System.Globalization.NumberStyles.None,
                    System.Globalization.CultureInfo.InvariantCulture, out int o) ? o : 0;
            }
        }

        public bool Equals(AfflictionEpisodeId other) => string.Equals(Value, other.Value, StringComparison.Ordinal);
        public override bool Equals(object? obj) => obj is AfflictionEpisodeId other && Equals(other);
        public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Value);
        public int CompareTo(AfflictionEpisodeId other) => string.CompareOrdinal(Value, other.Value);
        public override string ToString() => Value;

        public static bool operator ==(AfflictionEpisodeId left, AfflictionEpisodeId right) => left.Equals(right);
        public static bool operator !=(AfflictionEpisodeId left, AfflictionEpisodeId right) => !left.Equals(right);
    }

    public sealed class AfflictionIdJsonConverter : JsonConverter<AfflictionId>
    {
        public override AfflictionId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? s = reader.GetString();
            if (!AfflictionId.TryParse(s, out var id))
                throw new JsonException($"Invalid AfflictionId '{s}'.");
            return id;
        }

        public override void Write(Utf8JsonWriter writer, AfflictionId value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.Value);
    }

    public sealed class AfflictionEpisodeIdJsonConverter : JsonConverter<AfflictionEpisodeId>
    {
        public override AfflictionEpisodeId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? s = reader.GetString();
            if (!AfflictionEpisodeId.TryParse(s, out var id))
                throw new JsonException($"Invalid AfflictionEpisodeId '{s}'.");
            return id;
        }

        public override void Write(Utf8JsonWriter writer, AfflictionEpisodeId value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.Value);
    }
}
