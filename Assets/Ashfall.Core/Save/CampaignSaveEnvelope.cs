using System;
using System.Collections.Generic;
using Ashfall.Core.Save;

namespace Ashfall.Core.Save;

/// <summary>
/// Metadata for a save slot. Written into the manifest and validated before
/// any simulation state is mutated.
/// </summary>
public class SaveManifest
{
    /// <summary>Manifest schema version. Increment when fields are added/removed.</summary>
    public int manifestVersion = 1;

    /// <summary>Game version string that wrote this manifest.</summary>
    public string gameVersion = string.Empty;

    /// <summary>Build identifier for exact reproducibility.</summary>
    public string buildId = string.Empty;

    /// <summary>Current simulation day at time of save.</summary>
    public int currentDay;

    /// <summary>Deterministic seed used by the campaign.</summary>
    public int seed;

    /// <summary>Monotonic tick when the save was written (host-provided).</summary>
    public long lastSaveTick;

    /// <summary>Campaign mode policy.</summary>
    public CampaignMode mode = CampaignMode.Normal;

    /// <summary>Slot identity.</summary>
    public SaveSlotId slotId;

    /// <summary>Profile identity.</summary>
    public SaveProfileId profileId;

    /// <summary>Player-assigned campaign name (optional).</summary>
    public string campaignName = string.Empty;

    /// <summary>
    /// Iron-man terminal state. Set to TerminalLoss when a data-defined end
    /// condition is met. The save service enforces policy from this field.
    /// </summary>
    public IronManTerminalState ironManTerminalState = IronManTerminalState.Active;

    /// <summary>ISO-8601 timestamp of last successful save.</summary>
    public string lastSaveTimestamp = string.Empty;
}

/// <summary>
/// One section of an aggregate save envelope. Sections are the individual
/// subsystem saves (expedition, world, inventory, etc.) collected into one
/// slot save.
/// </summary>
public class SaveSectionEnvelope
{
    /// <summary>
    /// Canonical section name. Must match the section name registered by the
    /// subsystem's save store. Used for ordering and lookup.
    /// </summary>
    public string sectionName = string.Empty;

    /// <summary>Schema version of the payload.</summary>
    public int schemaVersion;

    /// <summary>SHA-256 checksum of the canonicalized payload.</summary>
    public string checksum = string.Empty;

    /// <summary>
    /// Raw JSON payload for this section. The payload is checksummed and
    /// validated before any state is restored.
    /// </summary>
    public string payloadJson = string.Empty;
}

/// <summary>
/// Aggregate save envelope for one slot. Contains the manifest, all section
/// payloads in canonical order, and an aggregate checksum covering the
/// manifest plus every section's canonicalized form.
/// </summary>
public class AggregateSaveEnvelope
{
    /// <summary>Manifest schema version.</summary>
    public int manifestVersion = 1;

    /// <summary>Campaign metadata.</summary>
    public SaveManifest manifest = new();

    /// <summary>
    /// Explicit ordered list of section envelopes. Lists are used instead of
    /// dictionaries so serialization order is deterministic and the aggregate
    /// checksum is stable.
    /// </summary>
    public List<SaveSectionEnvelope> sections = new();

    /// <summary>
    /// Aggregate SHA-256 checksum over the canonical manifest + all sections.
    /// </summary>
    public string aggregateChecksum = string.Empty;

    /// <summary>
    /// Optional chronicle summary derived from authoritative state at save time.
    /// Not included in the aggregate checksum; host-only display data.
    /// </summary>
    public string chronicleSummary = string.Empty;

    /// <summary>
    /// True when this envelope was created by migrating a legacy single-file save.
    /// </summary>
    public bool migratedFromLegacy;

    /// <summary>
    /// Source path of the legacy save, if migration was performed.
    /// </summary>
    public string legacySourcePath = string.Empty;
}

/// <summary>
/// Contract for a subsystem save section. Each registered section contributes
/// one payload to the aggregate envelope.
/// </summary>
public interface ICampaignSaveSection
{
    /// <summary>Canonical section name (e.g., "expedition", "world").</summary>
    string SectionName { get; }

    /// <summary>Current schema version for this section's payload.</summary>
    int CurrentSchemaVersion { get; }

    /// <summary>
    /// Capture the current subsystem state into a serializable DTO.
    /// Returns null if the subsystem has no state to save.
    /// </summary>
    object? CaptureState();

    /// <summary>
    /// Restore subsystem state from a previously captured DTO.
    /// </summary>
    void RestoreState(object state);

    /// <summary>
    /// Validate a captured state before restore. Returns true if the state
    /// is acceptable to restore.
    /// </summary>
    bool ValidateState(object state);
}

/// <summary>
/// Result of validating an aggregate save envelope.
/// </summary>
public class AggregateValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<string> SectionErrors { get; set; } = new();

    public static AggregateValidationResult Valid() => new() { IsValid = true };
    public static AggregateValidationResult Invalid(params string[] errors) =>
        new() { IsValid = false, Errors = new List<string>(errors) };
}
