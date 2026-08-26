using System;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Save;

/// <summary>
/// Validated, filesystem-safe profile identifier. Profiles are the top-level
/// grouping for save slots (e.g., different players or playstyles on one install).
/// </summary>
[JsonConverter(typeof(SaveIdJsonConverter<SaveProfileId>))]
public readonly struct SaveProfileId : IEquatable<SaveProfileId>
{
    public string Value { get; }

    [JsonConstructor]
    public SaveProfileId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("SaveProfileId cannot be null or whitespace.", nameof(value));

        // Filesystem-safe: alphanumeric, dash, underscore only. No path separators.
        foreach (char c in value)
        {
            if (!char.IsLetterOrDigit(c) && c != '_' && c != '-')
                throw new ArgumentException(
                    $"SaveProfileId contains invalid character '{c}'. Only alphanumeric, dash, and underscore are allowed.",
                    nameof(value));
        }

        Value = value;
    }

    public bool Equals(SaveProfileId other) => string.Equals(Value, other.Value, StringComparison.Ordinal);
    public override bool Equals(object? obj) => obj is SaveProfileId other && Equals(other);
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Value);
    public override string ToString() => Value;
    public static bool operator ==(SaveProfileId left, SaveProfileId right) => left.Equals(right);
    public static bool operator !=(SaveProfileId left, SaveProfileId right) => !left.Equals(right);
}

/// <summary>
/// Validated, filesystem-safe slot identifier. Slots hold one campaign each
/// within a profile.
/// </summary>
[JsonConverter(typeof(SaveIdJsonConverter<SaveSlotId>))]
public readonly struct SaveSlotId : IEquatable<SaveSlotId>
{
    public string Value { get; }

    [JsonConstructor]
    public SaveSlotId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("SaveSlotId cannot be null or whitespace.", nameof(value));

        foreach (char c in value)
        {
            if (!char.IsLetterOrDigit(c) && c != '_' && c != '-')
                throw new ArgumentException(
                    $"SaveSlotId contains invalid character '{c}'. Only alphanumeric, dash, and underscore are allowed.",
                    nameof(value));
        }

        Value = value;
    }

    public bool Equals(SaveSlotId other) => string.Equals(Value, other.Value, StringComparison.Ordinal);
    public override bool Equals(object? obj) => obj is SaveSlotId other && Equals(other);
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Value);
    public override string ToString() => Value;
    public static bool operator ==(SaveSlotId left, SaveSlotId right) => left.Equals(right);
    public static bool operator !=(SaveSlotId left, SaveSlotId right) => !left.Equals(right);
}

/// <summary>
/// Campaign mode policy. Iron-man prevents manual restore after a terminal loss.
/// </summary>
public enum CampaignMode
{
    /// <summary>Standard campaign with normal save/load rules.</summary>
    Normal = 0,

    /// <summary>
    /// Iron-man campaign: once a terminal loss occurs, the slot cannot be
    /// manually restored from the UI. Chronicle export remains available.
    /// </summary>
    IronMan = 1
}

/// <summary>
/// Terminal state for an iron-man campaign. Once set, the slot is sealed
/// against manual restore until explicitly reset by the save service.
/// </summary>
public enum IronManTerminalState
{
    /// <summary>Campaign is still active.</summary>
    Active = 0,

    /// <summary>
    /// Terminal loss recorded (death, evacuation, or other data-defined end).
    /// No further manual restores permitted.
    /// </summary>
    TerminalLoss = 1,

    /// <summary>
    /// Terminal state was overridden by an explicit reset (e.g., legacy import
    /// or service-level reset). Restores are again permitted.
    /// </summary>
    Reset = 2
}

/// <summary>
/// System.Text.Json converter for SaveProfileId and SaveSlotId readonly structs.
/// Serializes as {"Value":"..."} and deserializes via the public constructor.
/// </summary>
public class SaveIdJsonConverter<T> : System.Text.Json.Serialization.JsonConverter<T> where T : struct
{
    public override T Read(ref System.Text.Json.Utf8JsonReader reader, System.Type typeToConvert, System.Text.Json.JsonSerializerOptions options)
    {
        if (reader.TokenType != System.Text.Json.JsonTokenType.StartObject)
            throw new System.Text.Json.JsonException();

        string value = string.Empty;
        while (reader.Read())
        {
            if (reader.TokenType == System.Text.Json.JsonTokenType.EndObject)
                break;
            if (reader.TokenType == System.Text.Json.JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString();
                reader.Read();
                if (string.Equals(propertyName, "Value", System.StringComparison.OrdinalIgnoreCase))
                {
                    value = reader.GetString() ?? string.Empty;
                }
                else
                {
                    reader.Skip();
                }
            }
        }

        if (typeToConvert == typeof(SaveSlotId))
            return (T)(object)new SaveSlotId(value);
        return (T)(object)new SaveProfileId(value);
    }

    public override void Write(System.Text.Json.Utf8JsonWriter writer, T value, System.Text.Json.JsonSerializerOptions options)
    {
        string val = value switch
        {
            SaveProfileId p => p.Value,
            SaveSlotId s => s.Value,
            _ => string.Empty
        };
        writer.WriteStartObject();
        writer.WriteString("Value", val);
        writer.WriteEndObject();
    }
}
