using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Ashfall.Core.Save;

namespace Ashfall.Core.Save;

/// <summary>
/// Core service for managing save profiles, slots, and aggregate save envelopes.
/// Owns the slot directory layout, atomic write strategy, validation, corruption
/// quarantine, legacy import, and iron-man policy enforcement.
/// </summary>
public class SaveSlotService
{
    /// <summary>Base directory name under user:// for all save data.</summary>
    public const string SavesBaseDir = "saves";

    /// <summary>File name for the slot manifest.</summary>
    public const string ManifestFileName = "manifest.json";

    /// <summary>File name for the aggregate envelope.</summary>
    public const string AggregateFileName = "campaign.json";

    /// <summary>Extension for quarantined corrupt saves.</summary>
    public const string QuarantineExtension = ".corrupt";

    private readonly IFileIO _files;
    private readonly IJsonSerializer _json;
    private readonly ILog _log;
    private readonly string _basePath;
    private readonly IWallClock _wallClock;

    /// <summary>
    /// Create a new save slot service rooted at the given base path.
    /// Typically the base path is the globalized user:// directory.
    /// </summary>
    public SaveSlotService(IFileIO files, IJsonSerializer json, ILog log, string basePath, IWallClock? wallClock = null)
    {
        _files = files ?? throw new ArgumentNullException(nameof(files));
        _json = json ?? throw new ArgumentNullException(nameof(json));
        _log = log ?? throw new ArgumentNullException(nameof(log));
        _basePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
        _wallClock = wallClock ?? SystemWallClock.Instance;
    }

    /// <summary>Resolve the root directory for a specific slot.</summary>
    public string GetSlotRoot(SaveProfileId profileId, SaveSlotId slotId)
    {
        return _files.Combine(_basePath, SavesBaseDir, $"profile-{profileId.Value}", $"slot-{slotId.Value}");
    }

    /// <summary>Resolve the manifest path for a specific slot.</summary>
    public string GetManifestPath(SaveProfileId profileId, SaveSlotId slotId)
    {
        return _files.Combine(GetSlotRoot(profileId, slotId), ManifestFileName);
    }

    /// <summary>Resolve the aggregate envelope path for a specific slot.</summary>
    public string GetAggregatePath(SaveProfileId profileId, SaveSlotId slotId)
    {
        return _files.Combine(GetSlotRoot(profileId, slotId), AggregateFileName);
    }

    /// <summary>
    /// Check whether a slot has a valid manifest on disk.
    /// </summary>
    public bool SlotExists(SaveProfileId profileId, SaveSlotId slotId)
    {
        string slotRoot = GetSlotRoot(profileId, slotId);
        if (_files.DirectoryExists(slotRoot))
            return true;
        return _files.FileExists(GetManifestPath(profileId, slotId));
    }

    /// <summary>
    /// Create a new empty slot with a default manifest. Returns false if the
    /// slot already exists.
    /// </summary>
    public bool CreateSlot(SaveProfileId profileId, SaveSlotId slotId)
    {
        if (SlotExists(profileId, slotId))
            return false;

        var manifest = new SaveManifest
        {
            profileId = profileId,
            slotId = slotId,
            campaignName = $"Campaign {slotId.Value}",
            gameVersion = "test",
            buildId = "test",
            currentDay = 1,
            seed = 0,
            lastSaveTick = 0,
            mode = CampaignMode.Normal,
            ironManTerminalState = IronManTerminalState.Active,
            lastSaveTimestamp = string.Empty
        };

        SaveManifest(profileId, slotId, manifest);
        return true;
    }

    /// <summary>
    /// List all slot IDs that have a manifest for the given profile.
    /// Returns empty if the profile directory does not exist.
    /// </summary>
    public List<SaveSlotId> ListSlots(SaveProfileId profileId)
    {
        var result = new List<SaveSlotId>();
        string profileDir = _files.Combine(_basePath, SavesBaseDir, $"profile-{profileId.Value}");
        if (!_files.DirectoryExists(profileDir))
            return result;

        // Enumerate slot-* directories and parse their slot IDs.
        string[] entries;
        try
        {
            entries = Directory.GetDirectories(profileDir, "slot-*");
        }
        catch (Exception ex)
        {
            _log.Warn($"SaveSlotService: failed to enumerate profile directory '{profileDir}': {ex.Message}");
            return result;
        }

        foreach (string dir in entries)
        {
            string name = Path.GetFileName(dir);
            if (name.StartsWith("slot-", StringComparison.Ordinal))
            {
                string id = name.Substring(5);
                if (!string.IsNullOrWhiteSpace(id))
                {
                    try
                    {
                        result.Add(new SaveSlotId(id));
                    }
                    catch (ArgumentException)
                    {
                        _log.Warn($"SaveSlotService: skipping invalid slot directory '{name}'.");
                    }
                }
            }
        }

        result.Sort((a, b) => string.CompareOrdinal(a.Value, b.Value));
        return result;
    }

    /// <summary>
    /// List all profile IDs on disk.
    /// </summary>
    public List<SaveProfileId> ListProfiles()
    {
        var result = new List<SaveProfileId>();
        string savesDir = _files.Combine(_basePath, SavesBaseDir);
        if (!_files.DirectoryExists(savesDir))
            return result;

        string[] entries;
        try
        {
            entries = Directory.GetDirectories(savesDir, "profile-*");
        }
        catch (Exception ex)
        {
            _log.Warn($"SaveSlotService: failed to enumerate saves directory '{savesDir}': {ex.Message}");
            return result;
        }

        foreach (string dir in entries)
        {
            string name = Path.GetFileName(dir);
            if (name.StartsWith("profile-", StringComparison.Ordinal))
            {
                string id = name.Substring(8);
                if (!string.IsNullOrWhiteSpace(id))
                {
                    try
                    {
                        result.Add(new SaveProfileId(id));
                    }
                    catch (ArgumentException)
                    {
                        _log.Warn($"SaveSlotService: skipping invalid profile directory '{name}'.");
                    }
                }
            }
        }

        result.Sort((a, b) => string.CompareOrdinal(a.Value, b.Value));
        return result;
    }

    /// <summary>
    /// Load the manifest for a slot. Returns null if the slot does not exist
    /// or the manifest cannot be parsed.
    /// </summary>
    public SaveManifest? LoadManifest(SaveProfileId profileId, SaveSlotId slotId)
    {
        string path = GetManifestPath(profileId, slotId);
        if (!_files.FileExists(path))
            return null;

        try
        {
            string raw = _files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return null;
            var manifest = _json.Deserialize<SaveManifest>(raw);
            return manifest;
        }
        catch (Exception ex)
        {
            _log.Warn($"SaveSlotService: failed to load manifest for slot '{slotId}': {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Save the manifest for a slot. Creates directories as needed.
    /// </summary>
    public void SaveManifest(SaveProfileId profileId, SaveSlotId slotId, SaveManifest manifest)
    {
        if (manifest == null) throw new ArgumentNullException(nameof(manifest));
        string path = GetManifestPath(profileId, slotId);
        string? dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir) && !_files.DirectoryExists(dir))
            Directory.CreateDirectory(dir);
        _files.WriteAllText(path, _json.Serialize(manifest));
    }

    /// <summary>
    /// Validate an aggregate envelope before restoring state. Checks manifest
    /// version, section ordering, per-section checksums, and aggregate checksum.
    /// </summary>
    public AggregateValidationResult ValidateAggregate(AggregateSaveEnvelope envelope)
    {
        if (envelope == null)
            return AggregateValidationResult.Invalid("Aggregate envelope is null.");

            var errors = new List<string>();
            var sectionErrors = new List<string>();

            // Version ladder: V1 (filename-keyed sections) and the current
            // version are both loadable; V1 migrates in memory via
            // MigrateToCurrent. Anything newer than what this build knows is
            // rejected — a future-format save must not be silently truncated.
            if (envelope.manifestVersion != 1 &&
                envelope.manifestVersion != CampaignEnvelopeBuilder.CurrentEnvelopeVersion)
                errors.Add($"Unsupported manifest version: {envelope.manifestVersion}");

        if (envelope.sections == null || envelope.sections.Count == 0)
            errors.Add("Aggregate envelope contains no sections.");

        // Validate each section independently.
        if (envelope.sections != null)
        {
            for (int i = 0; i < envelope.sections.Count; i++)
            {
                SaveSectionEnvelope section = envelope.sections[i];
                if (string.IsNullOrWhiteSpace(section.sectionName))
                    sectionErrors.Add($"Section {i}: sectionName is empty.");

            if (string.IsNullOrEmpty(section.payloadJson))
                sectionErrors.Add($"Section {i} ({section.sectionName}): payloadJson is empty.");

            if (!string.IsNullOrEmpty(section.payloadJson) && !string.IsNullOrEmpty(section.checksum))
            {
                string expected = ComputeSectionChecksum(section);
                if (!string.Equals(section.checksum, expected, StringComparison.Ordinal))
                    sectionErrors.Add($"Section {i} ({section.sectionName}): checksum mismatch.");
            }
            else if (string.IsNullOrEmpty(section.checksum) && !string.IsNullOrEmpty(section.payloadJson))
            {
                sectionErrors.Add($"Section {i} ({section.sectionName}): checksum is empty for a non-empty payload.");
            }

            if (!string.IsNullOrEmpty(envelope.manifest.generationId) &&
                !string.IsNullOrEmpty(section.generationId) &&
                !string.Equals(envelope.manifest.generationId, section.generationId, StringComparison.Ordinal))
            {
                sectionErrors.Add($"Section {i} ({section.sectionName}): generation mismatch (manifest '{envelope.manifest.generationId}', section '{section.generationId}').");
            }
        }
        }

        // Validate aggregate checksum.
        if (errors.Count == 0 && sectionErrors.Count == 0)
        {
            string expected = ComputeAggregateChecksum(envelope);
            if (!string.Equals(envelope.aggregateChecksum, expected, StringComparison.Ordinal))
                errors.Add("Aggregate checksum mismatch.");
        }

        if (errors.Count > 0 || sectionErrors.Count > 0)
        {
            var allErrors = new List<string>(errors);
            allErrors.AddRange(sectionErrors);
            return new AggregateValidationResult
            {
                IsValid = false,
                Errors = allErrors,
                SectionErrors = new List<string>(sectionErrors)
            };
        }

        return AggregateValidationResult.Valid();
    }

    /// <summary>
    /// Write an aggregate envelope atomically: write to a temp file, validate,
    /// then replace the active envelope. Returns true on success.
    /// </summary>
    public bool WriteAggregateAtomically(SaveProfileId profileId, SaveSlotId slotId, AggregateSaveEnvelope envelope)
    {
        if (envelope == null) throw new ArgumentNullException(nameof(envelope));

        string targetPath = GetAggregatePath(profileId, slotId);
        string? dir = Path.GetDirectoryName(targetPath);
        if (!string.IsNullOrEmpty(dir) && !_files.DirectoryExists(dir))
            _files.CreateDirectory(dir);

        string tempPath = targetPath + ".tmp";
        string backupPath = targetPath + ".bak";

        try
        {
            // Ensure section checksums are populated before computing the aggregate checksum.
            for (int i = 0; i < envelope.sections.Count; i++)
            {
                var section = envelope.sections[i];
                if (string.IsNullOrEmpty(section.checksum) && !string.IsNullOrEmpty(section.payloadJson))
                {
                    section.checksum = ComputeSectionChecksum(section);
                    envelope.sections[i] = section;
                }
            }

            // Serialize envelope.
            envelope.aggregateChecksum = ComputeAggregateChecksum(envelope);
            string raw = _json.Serialize(envelope);

            // Write temp file.
            _files.WriteAllText(tempPath, raw);

            // Read back and validate.
            string readBack = _files.ReadAllText(tempPath);
            if (string.IsNullOrWhiteSpace(readBack))
            {
                _log.Error("SaveSlotService: atomic write produced empty temp file.");
                return false;
            }

            var parsed = _json.Deserialize<AggregateSaveEnvelope>(readBack);
            if (parsed == null)
            {
                _log.Error("SaveSlotService: atomic write temp file failed to parse.");
                return false;
            }

            var validation = ValidateAggregate(parsed);
            if (!validation.IsValid)
            {
                throw new InvalidOperationException($"SaveSlotService: atomic write validation failed: {string.Join("; ", validation.Errors)}");
            }

            // Move previous active to backup, then temp to active.
            if (_files.FileExists(targetPath))
            {
                try
                {
                    string existing = _files.ReadAllText(targetPath);
                    _files.WriteAllText(backupPath, existing);
                }
                catch (Exception ex)
                {
                    _log.Warn($"SaveSlotService: could not create backup '{backupPath}': {ex.Message}");
                }
            }

            _files.WriteAllText(targetPath, readBack);
            if (_files.FileExists(tempPath))
                _files.DeleteFile(tempPath);

            return true;
        }
        catch (Exception ex)
        {
            _log.Error($"SaveSlotService: failed to write aggregate envelope for slot '{slotId}': {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Load and validate an aggregate envelope, returning a detailed SaveLoadResult
    /// with a user-facing recoverable status message on failure.
    /// Quarantines corrupt or checksum-invalid saves.
    /// </summary>
    public SaveLoadResult TryLoadAggregate(SaveProfileId profileId, SaveSlotId slotId)
    {
        if (IsIronManTerminal(profileId, slotId))
        {
            return SaveLoadResult.Fail(
                SaveLoadStatus.IronManBlocked,
                $"Save slot '{slotId.Value}' is sealed (Iron Man terminal defeat). Manual restore blocked.");
        }

        string path = GetAggregatePath(profileId, slotId);
        if (!_files.FileExists(path))
        {
            return SaveLoadResult.Fail(
                SaveLoadStatus.MissingFile,
                $"Save file for slot '{slotId.Value}' was not found.");
        }

        string raw;
        try
        {
            raw = _files.ReadAllText(path);
        }
        catch (Exception ex)
        {
            _log.Warn($"SaveSlotService: failed to read aggregate for slot '{slotId}': {ex.Message}");
            return SaveLoadResult.Fail(
                SaveLoadStatus.CorruptData,
                $"Save file for slot '{slotId.Value}' could not be read: {ex.Message}");
        }

        if (string.IsNullOrWhiteSpace(raw))
        {
            QuarantineCorruptSave(profileId, slotId, path, "Empty or whitespace save file");
            return SaveLoadResult.Fail(
                SaveLoadStatus.CorruptData,
                $"Save data for slot '{slotId.Value}' is empty or corrupt. Live session preserved.");
        }

        AggregateSaveEnvelope? envelope;
        try
        {
            envelope = _json.Deserialize<AggregateSaveEnvelope>(raw);
        }
        catch (Exception ex)
        {
            _log.Warn($"SaveSlotService: JSON deserialize failed for slot '{slotId}': {ex.Message}");
            QuarantineCorruptSave(profileId, slotId, path, $"JSON parse error: {ex.Message}");
            return SaveLoadResult.Fail(
                SaveLoadStatus.CorruptData,
                $"Save data for slot '{slotId.Value}' is corrupted (malformed JSON). Live session preserved.",
                new[] { ex.Message });
        }

        if (envelope == null)
        {
            _log.Warn($"SaveSlotService: aggregate envelope for slot '{slotId}' deserialized as null.");
            QuarantineCorruptSave(profileId, slotId, path, "Deserialized as null");
            return SaveLoadResult.Fail(
                SaveLoadStatus.CorruptData,
                $"Save data for slot '{slotId.Value}' could not be parsed. Live session preserved.");
        }

            var validation = ValidateAggregate(envelope);
            if (!validation.IsValid)
            {
                QuarantineCorruptSave(profileId, slotId, path, string.Join("; ", validation.Errors));
                bool isChecksumFailure = validation.Errors.Any(e => e.IndexOf("checksum", StringComparison.OrdinalIgnoreCase) >= 0);
                var status = isChecksumFailure ? SaveLoadStatus.ChecksumMismatch : SaveLoadStatus.CorruptData;
                string msg = isChecksumFailure
                    ? $"Save data for slot '{slotId.Value}' failed checksum validation (corrupted or modified). Live session preserved."
                    : $"Save data for slot '{slotId.Value}' failed validation ({string.Join(", ", validation.Errors)}). Live session preserved.";
                return SaveLoadResult.Fail(status, msg, validation.Errors);
            }

            // Older envelopes migrate to the current format in memory; the
            // on-disk file is rewritten as current on the next save.
            if (envelope.manifestVersion != CampaignEnvelopeBuilder.CurrentEnvelopeVersion)
                envelope = MigrateToCurrent(envelope);

            return SaveLoadResult.Ok(envelope, $"Save slot '{slotId.Value}' loaded successfully.");
    }

        /// <summary>
        /// Load and validate an aggregate envelope. Returns null if the slot does
        /// not exist or the envelope fails validation. V1 envelopes are migrated
        /// to the current format in memory (the on-disk file is only rewritten
        /// as V2 on the next save).
        /// </summary>
        public AggregateSaveEnvelope? LoadAggregate(SaveProfileId profileId, SaveSlotId slotId)
        {
            var result = TryLoadAggregate(profileId, slotId);
            return result.Envelope;
        }

        /// <summary>
        /// Reserved section name used by <see cref="TryImportLegacySave"/> for
        /// its single-blob import payload. It is not a registry section, but
        /// V1 migration preserves it verbatim so imported slots keep loading
        /// exactly as they did before versioning.
        /// </summary>
        public const string LegacyImportSectionName = "legacy";

        /// <summary>
        /// Migrate an older aggregate envelope to the current format in
        /// memory. V1 named sections after their files (e.g.
        /// "inventory_save"); the current format keys them by
        /// SaveSectionRegistry SectionKey and stamps real schema versions.
        /// Unknown/stray sections are dropped with a warning — the registry is
        /// the whitelist — except the reserved single-file import section,
        /// which is preserved verbatim. Payload bytes are never touched, so
        /// per-section content survives migration verbatim. Envelopes already
        /// at the current version are returned unchanged.
        /// </summary>
        public static AggregateSaveEnvelope MigrateToCurrent(AggregateSaveEnvelope envelope, ILog? log = null)
        {
            if (envelope == null) throw new ArgumentNullException(nameof(envelope));
            if (envelope.manifestVersion == CampaignEnvelopeBuilder.CurrentEnvelopeVersion)
                return envelope;
            if (envelope.manifestVersion != 1)
                throw new InvalidOperationException(
                    $"No migration path from envelope manifestVersion {envelope.manifestVersion} to {CampaignEnvelopeBuilder.CurrentEnvelopeVersion}.");

            var sections = new List<SaveSectionEnvelope>();
            var dropped = new List<string>();
            foreach (var section in envelope.sections ?? new List<SaveSectionEnvelope>())
            {
                bool reserved = string.Equals(section.sectionName, LegacyImportSectionName, StringComparison.Ordinal);
                if (reserved)
                {
                    var kept = new SaveSectionEnvelope
                    {
                        sectionName = section.sectionName,
                        schemaVersion = section.schemaVersion,
                        payloadJson = section.payloadJson,
                        checksum = section.checksum,
                    };
                    sections.Add(kept);
                    continue;
                }

                if (string.IsNullOrEmpty(section.sectionName) ||
                    !SaveSectionRegistry.TryGetKeyForSectionName(section.sectionName, out var key))
                {
                    dropped.Add(section.sectionName ?? "(null)");
                    continue;
                }

                var migrated = new SaveSectionEnvelope
                {
                    sectionName = key!,
                    schemaVersion = SaveSectionRegistry.SchemaVersionFor(key!),
                    payloadJson = section.payloadJson,
                };
                migrated.checksum = ComputeSectionChecksum(migrated);
                sections.Add(migrated);
            }

            if (dropped.Count > 0)
                log?.Warn($"SaveSlotService: dropped {dropped.Count} unknown legacy section(s) during V1 migration: {string.Join(", ", dropped)}");

            var result = new AggregateSaveEnvelope
            {
                manifestVersion = CampaignEnvelopeBuilder.CurrentEnvelopeVersion,
                manifest = envelope.manifest,
                sections = sections,
                migratedFromLegacy = envelope.migratedFromLegacy,
                legacySourcePath = envelope.legacySourcePath,
            };
            result.aggregateChecksum = ComputeAggregateChecksum(result);
            return result;
        }

    /// <summary>
    /// Delete a slot and all its files. Returns true if the slot was deleted
    /// or did not exist.
    /// </summary>
    public bool DeleteSlot(SaveProfileId profileId, SaveSlotId slotId)
    {
        // Iron-man terminal slots cannot be deleted from the UI.
        if (IsIronManTerminal(profileId, slotId))
        {
            _log.Warn($"SaveSlotService: cannot delete iron-man terminal slot '{slotId}'.");
            return false;
        }

        string slotRoot = GetSlotRoot(profileId, slotId);
        try
        {
            if (_files.DirectoryExists(slotRoot))
            {
                Directory.Delete(slotRoot, recursive: true);
            }
            return true;
        }
        catch (Exception ex)
        {
            _log.Error($"SaveSlotService: failed to delete slot '{slotId}': {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Check whether a slot is in an iron-man terminal state and should reject
    /// manual restore.
    /// </summary>
    public bool IsIronManTerminal(SaveProfileId profileId, SaveSlotId slotId)
    {
        var manifest = LoadManifest(profileId, slotId);
        if (manifest == null)
            return false;

        return manifest.mode == CampaignMode.IronMan &&
               manifest.ironManTerminalState == IronManTerminalState.TerminalLoss;
    }

    /// <summary>
    /// Import a legacy single-file save into a new slot. The legacy file is
    /// expected to be a raw subsystem state JSON (pre-envelope format).
    /// Import is idempotent: running twice on the same legacy file does not
    /// merge or overwrite an existing slot.
    /// </summary>
    public bool TryImportLegacySave(
        string legacyFilePath,
        SaveProfileId targetProfileId,
        SaveSlotId targetSlotId,
        out string error)
    {
        error = string.Empty;

        if (string.IsNullOrWhiteSpace(legacyFilePath))
        {
            error = "Legacy file path is empty.";
            return false;
        }

        if (!File.Exists(legacyFilePath))
        {
            error = $"Legacy file not found: {legacyFilePath}";
            return false;
        }

        // Idempotent: do not overwrite an existing slot.
        if (SlotExists(targetProfileId, targetSlotId))
        {
            error = $"Slot '{targetSlotId}' already exists. Import is idempotent and will not overwrite.";
            return false;
        }

        try
        {
            string raw = File.ReadAllText(legacyFilePath);
            if (string.IsNullOrWhiteSpace(raw))
            {
                error = "Legacy file is empty.";
                return false;
            }

            // Create a minimal aggregate envelope from the legacy payload.
            var envelope = new AggregateSaveEnvelope
            {
                manifestVersion = 1,
                manifest = new SaveManifest
                {
                    profileId = targetProfileId,
                    slotId = targetSlotId,
                    campaignName = $"Imported ({Path.GetFileName(legacyFilePath)})",
                    gameVersion = "imported",
                    buildId = "legacy",
                    currentDay = 1,
                    seed = 0,
                    lastSaveTick = 0,
                    mode = CampaignMode.Normal,
                    ironManTerminalState = IronManTerminalState.Active,
                    lastSaveTimestamp = string.Empty
                },
                migratedFromLegacy = true,
                legacySourcePath = legacyFilePath,
                sections = new List<SaveSectionEnvelope>
                {
                    new SaveSectionEnvelope
                    {
                        sectionName = "legacy",
                        schemaVersion = 1,
                        payloadJson = raw,
                        checksum = ComputeSectionChecksum(new SaveSectionEnvelope
                        {
                            sectionName = "legacy",
                            schemaVersion = 1,
                            payloadJson = raw
                        })
                    }
                }
            };

            envelope.aggregateChecksum = ComputeAggregateChecksum(envelope);

            if (!WriteAggregateAtomically(targetProfileId, targetSlotId, envelope))
            {
                error = "Failed to write imported aggregate envelope.";
                return false;
            }

            _log.Info($"SaveSlotService: imported legacy save '{legacyFilePath}' into slot '{targetSlotId}'.");
            return true;
        }
        catch (Exception ex)
        {
            error = $"Legacy import failed: {ex.Message}";
            _log.Error($"SaveSlotService: legacy import failed: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Move a corrupt save file to a quarantine path so the last known valid
    /// save is preserved.
    /// </summary>
    private void QuarantineCorruptSave(SaveProfileId profileId, SaveSlotId slotId, string path, string reason)
    {
        try
        {
            string quarantinePath = path + "." + slotId.Value + QuarantineExtension;
            if (_files.FileExists(path))
            {
                string content = _files.ReadAllText(path);
                _files.WriteAllText(quarantinePath, content);
                _files.DeleteFile(path);
            }
            _log.Warn($"SaveSlotService: quarantined corrupt save for slot '{slotId}' to '{quarantinePath}'. Reason: {reason}");
        }
        catch (Exception ex)
        {
            _log.Error($"SaveSlotService: failed to quarantine corrupt save for slot '{slotId}': {ex.Message}");
        }
    }

    /// <summary>
    /// Compute the checksum for a single section envelope.
    /// </summary>
    public static string ComputeSectionChecksum(SaveSectionEnvelope section)
    {
        if (section == null)
            throw new ArgumentNullException(nameof(section));

        // Canonical form: sectionName + schemaVersion + payloadJson.
        string canonical = $"{section.sectionName}\n{section.schemaVersion}\n{section.payloadJson ?? string.Empty}";
        byte[] bytes = Encoding.UTF8.GetBytes(canonical);
        using var sha = SHA256.Create();
        byte[] hash = sha.ComputeHash(bytes);
        var sb = new StringBuilder(hash.Length * 2);
        for (int i = 0; i < hash.Length; i++)
            sb.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));
        return sb.ToString();
    }

    /// <summary>
    /// Compute the aggregate checksum over the manifest and all sections in
    /// canonical order.
    /// </summary>
    public static string ComputeAggregateChecksum(AggregateSaveEnvelope envelope)
    {
        if (envelope == null)
            throw new ArgumentNullException(nameof(envelope));

        var sb = new StringBuilder();

        // Manifest fields in fixed order.
        sb.Append("manifestVersion=").Append(envelope.manifestVersion).Append('\n');
        sb.Append("gameVersion=").Append(envelope.manifest?.gameVersion ?? string.Empty).Append('\n');
        sb.Append("buildId=").Append(envelope.manifest?.buildId ?? string.Empty).Append('\n');
        sb.Append("currentDay=").Append(envelope.manifest?.currentDay ?? 0).Append('\n');
        sb.Append("seed=").Append(envelope.manifest?.seed ?? 0).Append('\n');
        sb.Append("lastSaveTick=").Append(envelope.manifest?.lastSaveTick ?? 0).Append('\n');
        sb.Append("mode=").Append((int)(envelope.manifest?.mode ?? CampaignMode.Normal)).Append('\n');
        sb.Append("slotId=").Append(envelope.manifest?.slotId.Value ?? string.Empty).Append('\n');
        sb.Append("profileId=").Append(envelope.manifest?.profileId.Value ?? string.Empty).Append('\n');
        sb.Append("campaignName=").Append(envelope.manifest?.campaignName ?? string.Empty).Append('\n');
        sb.Append("ironManTerminalState=").Append((int)(envelope.manifest?.ironManTerminalState ?? IronManTerminalState.Active)).Append('\n');
        sb.Append("lastSaveTimestamp=").Append(envelope.manifest?.lastSaveTimestamp ?? string.Empty).Append('\n');

        // Sections in order.
        if (envelope.sections != null)
        {
            for (int i = 0; i < envelope.sections.Count; i++)
            {
                SaveSectionEnvelope s = envelope.sections[i];
                sb.Append("section[").Append(i).Append("].name=").Append(s.sectionName ?? string.Empty).Append('\n');
                sb.Append("section[").Append(i).Append("].version=").Append(s.schemaVersion).Append('\n');
                sb.Append("section[").Append(i).Append("].checksum=").Append(s.checksum ?? string.Empty).Append('\n');
                sb.Append("section[").Append(i).Append("].payload=").Append(s.payloadJson ?? string.Empty).Append('\n');
            }
        }

        byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
        using var sha = SHA256.Create();
        byte[] hash = sha.ComputeHash(bytes);
        var result = new StringBuilder(hash.Length * 2);
        for (int i = 0; i < hash.Length; i++)
            result.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));
        return result.ToString();
    }
}

