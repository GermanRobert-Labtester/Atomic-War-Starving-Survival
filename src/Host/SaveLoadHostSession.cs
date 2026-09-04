using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp;

/// <summary>
/// Godot host session for save/load management. Owns the SaveSlotService,
/// exposes immutable slot cards for the UI, and coordinates save/load
/// through the selected slot root.
/// </summary>
public partial class SaveLoadHostSession : Node
{
    private SaveSlotService? _slotService;
    private string _basePath = string.Empty;
    private SaveProfileId _currentProfileId = new("default");
    private SaveSlotId? _activeSlotId;
    private AggregateSaveEnvelope? _activeEnvelope;
    private readonly HashSet<string> _restoredSections = new();
    private SaveSlotId? _selectionRollbackSlotId;
    private AggregateSaveEnvelope? _selectionRollbackEnvelope;
    private string? _selectionRollbackRoot;
    private HashSet<string>? _selectionRollbackSections;
    private SaveSlotId? _selectionRollbackTargetSlotId;
    private bool _hasSelectionRollback;

    /// <summary>Active loaded aggregate campaign envelope, or null if no slot loaded.</summary>
    public AggregateSaveEnvelope? ActiveEnvelope => _activeEnvelope;

    /// <summary>
    /// Try to get a specific section's JSON payload from the loaded in-memory envelope.
    /// </summary>
    public bool TryGetSectionPayload(string sectionKey, out string payload)
    {
        payload = string.Empty;
        if (_activeEnvelope?.sections == null) return false;
        foreach (var s in _activeEnvelope.sections)
        {
            if (string.Equals(s.sectionName, sectionKey, StringComparison.Ordinal))
            {
                payload = s.payloadJson;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Select or create the default slot if no slot is currently selected,
    /// guaranteeing an active slot root before sessions start.
    /// </summary>
    public SaveSlotId SelectOrCreateDefaultSlot(string slotName = "slot_1")
    {
        var slotId = new SaveSlotId(slotName);
        if (_slotService != null && !_slotService.SlotExists(_currentProfileId, slotId))
        {
            CreateSlot(slotId);
        }
        else
        {
            SelectSlot(slotId);
        }
        return slotId;
    }

    /// <summary>
    /// Clear the default slot as part of an explicit new-game transition.
    /// This is intentionally stronger than user-facing DeleteSlot: a new run
    /// must not inherit an old campaign envelope, derived projections, or
    /// terminal/corrupt metadata.
    /// </summary>
    public bool ResetSlotForNewGame(SaveSlotId slotId)
    {
        if (_slotService == null) return false;
        if (!_slotService.ResetSlotForNewGame(_currentProfileId, slotId))
            return false;

        if (_activeSlotId.HasValue && _activeSlotId.Value == slotId)
        {
            _activeSlotId = null;
            _activeEnvelope = null;
            _restoredSections.Clear();
            ClearSelectionRollback();
            ClearSlotRoot();
            ActiveSlotChanged?.Invoke(null);
        }

        SlotsChanged?.Invoke();
        GD.Print($"[SaveLoad] Reset slot for new game: {slotId}");
        return true;
    }

    /// <summary>Raised when slot data changes (create, delete, save, load).</summary>
    public event Action? SlotsChanged;

    /// <summary>Raised when the active slot changes.</summary>
    public event Action<SaveSlotId?>? ActiveSlotChanged;

    /// <summary>Raised when a load operation finishes with its detailed result and user-facing message.</summary>
    public event Action<SaveLoadResult>? OnLoadCompleted;

    /// <summary>Last result from a TryLoadSlot invocation.</summary>
    public SaveLoadResult? LastLoadResult { get; private set; }

    /// <summary>User-facing message from the last save/load operation.</summary>
    public string LastStatusMessage => LastLoadResult?.UserMessage ?? string.Empty;

    /// <summary>Sections that were successfully restored from the aggregate envelope during the last LoadAllDirect().</summary>
    public IReadOnlySet<string> RestoredSections => _restoredSections;

    /// <summary>
    /// Pack all individual save files into one aggregate envelope.
    /// </summary>
    public void Initialize(string basePath)
    {
        _basePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
        var files = new FileSystemIO();
        var json = new SystemTextJsonSerializer();
        var log = new GodotLog();
        _slotService = new SaveSlotService(files, json, log, _basePath);

        // Ensure the saves directory exists.
        string savesDir = Path.Combine(_basePath, SaveSlotService.SavesBaseDir);
        if (!Directory.Exists(savesDir))
            Directory.CreateDirectory(savesDir);

        GD.Print("[SaveLoad] Session initialized at: " + _basePath);
    }

    /// <summary>List all profiles on disk.</summary>
    public List<SaveProfileId> GetProfiles()
    {
        if (_slotService == null) return new List<SaveProfileId>();
        return _slotService.ListProfiles();
    }

    /// <summary>List all slots for the current profile.</summary>
    public List<SaveSlotId> GetSlots()
    {
        if (_slotService == null) return new List<SaveSlotId>();
        return _slotService.ListSlots(_currentProfileId);
    }

    /// <summary>Get the currently active profile ID.</summary>
    public SaveProfileId CurrentProfileId => _currentProfileId;

    /// <summary>Get the currently active slot ID, or null if none selected.</summary>
    public SaveSlotId? ActiveSlotId => _activeSlotId;

    /// <summary>
    /// Create a new empty slot. Returns true on success, false if the slot
    /// already exists.
    /// </summary>
    public bool CreateSlot(SaveSlotId slotId)
    {
        if (_slotService == null) return false;
        if (_slotService.SlotExists(_currentProfileId, slotId)) return false;

        var manifest = new SaveManifest
        {
            profileId = _currentProfileId,
            slotId = slotId,
            campaignName = $"Campaign {slotId.Value}",
            gameVersion = "0.1",
            buildId = "dev",
            currentDay = 1,
            seed = 0,
            lastSaveTick = 0,
            mode = CampaignMode.Normal,
            ironManTerminalState = IronManTerminalState.Active,
            lastSaveTimestamp = DateTime.UtcNow.ToString("o")
        };

        _slotService.SaveManifest(_currentProfileId, slotId, manifest);
        _activeSlotId = slotId;
        _activeEnvelope = null;
        _restoredSections.Clear();
        ClearSelectionRollback();
        ApplySlotRoot();
        SlotsChanged?.Invoke();
        ActiveSlotChanged?.Invoke(_activeSlotId);
        GD.Print($"[SaveLoad] Created slot: {slotId}");
        return true;
    }

    /// <summary>
    /// Select an existing slot as the active campaign. Returns false if the
    /// slot does not exist or is in an iron-man terminal state that blocks
    /// manual restore.
    /// </summary>
    public bool SelectSlot(SaveSlotId slotId, bool allowTerminalIronMan = false)
    {
        if (_slotService == null) return false;
        if (!_slotService.SlotExists(_currentProfileId, slotId)) return false;

        if (!allowTerminalIronMan && _slotService.IsIronManTerminal(_currentProfileId, slotId))
        {
            GD.PrintErr($"[SaveLoad] Slot '{slotId}' is iron-man terminal. Manual restore blocked.");
            return false;
        }

        CaptureSelectionRollback(slotId);
        _activeSlotId = slotId;
        _activeEnvelope = null;
        ApplySlotRoot();
        ActiveSlotChanged?.Invoke(_activeSlotId);
        GD.Print($"[SaveLoad] Selected slot: {slotId}");
        return true;
    }

    /// <summary>
    /// Delete a slot. Returns false if the slot does not exist or is the
    /// active iron-man slot.
    /// </summary>
    public bool DeleteSlot(SaveSlotId slotId)
    {
        if (_slotService == null) return false;
        if (!_slotService.SlotExists(_currentProfileId, slotId)) return false;

        // Iron-man terminal slots cannot be deleted from the UI.
        if (_slotService.IsIronManTerminal(_currentProfileId, slotId))
        {
            GD.PrintErr($"[SaveLoad] Cannot delete iron-man terminal slot '{slotId}'.");
            return false;
        }

        bool deleted = _slotService.DeleteSlot(_currentProfileId, slotId);
        if (deleted && _activeSlotId.HasValue && _activeSlotId.Value == slotId)
        {
            _activeSlotId = null;
            _activeEnvelope = null;
            _restoredSections.Clear();
            ClearSlotRoot();
            ActiveSlotChanged?.Invoke(null);
        }

        SlotsChanged?.Invoke();
        GD.Print($"[SaveLoad] Deleted slot: {slotId}");
        return deleted;
    }

    /// <summary>
    /// Get manifest data for a slot, or null if it does not exist.
    /// </summary>
    public SaveManifest? GetManifest(SaveSlotId slotId)
    {
        if (_slotService == null) return null;
        return _slotService.LoadManifest(_currentProfileId, slotId);
    }

    /// <summary>
    /// Plan VIII · Task 22.4 — per-section health of the slot's last aggregate
    /// save, read from the PERSISTED envelope (what the save said happened),
    /// never recomputed from live runtime state. Honest states: no envelope /
    /// aggregate checksum present / per-section checksum presence / legacy-
    /// migrated marker. Returns null when the slot service is unavailable.
    /// </summary>
    public SlotEnvelopeHealth? GetEnvelopeHealth(SaveSlotId slotId)
    {
        if (_slotService == null) return null;
        AggregateSaveEnvelope? envelope;
        try
        {
            envelope = _slotService.LoadAggregate(_currentProfileId, slotId);
        }
        catch (Exception)
        {
            return new SlotEnvelopeHealth { EnvelopePresent = false, LoadFailed = true };
        }
        if (envelope == null)
            return new SlotEnvelopeHealth { EnvelopePresent = false };

        var health = new SlotEnvelopeHealth
        {
            EnvelopePresent = true,
            ManifestVersion = envelope.manifestVersion,
            AggregateChecksumPresent = !string.IsNullOrEmpty(envelope.aggregateChecksum),
            MigratedFromLegacy = envelope.migratedFromLegacy,
            SectionCount = envelope.sections?.Count ?? 0
        };
        if (envelope.sections != null)
        {
            foreach (var s in envelope.sections)
            {
                if (s == null) continue;
                health.SectionLines.Add(
                    $"{s.sectionName} — {(string.IsNullOrEmpty(s.checksum) ? "no checksum" : "ok")}");
            }
        }
        return health;
    }

    /// <summary>
    /// Update manifest fields for the active slot. Call after a successful save.
    /// </summary>
    public void UpdateManifest(Action<SaveManifest> update)
    {
        if (update == null) throw new ArgumentNullException(nameof(update));
        if (_activeSlotId == null || _slotService == null) return;

        try
        {
            string aggregatePath = _slotService.GetAggregatePath(_currentProfileId, _activeSlotId.Value);
            if (File.Exists(aggregatePath))
            {
                // Once campaign.json exists, it is the sole current-generation
                // authority. Never update manifest.json independently of it.
                var loaded = _slotService.TryLoadAggregate(_currentProfileId, _activeSlotId.Value);
                if (!loaded.IsSuccess || loaded.Envelope?.manifest == null)
                {
                    GD.PrintErr($"[SaveLoad] Manifest update refused: authoritative campaign envelope could not be loaded for slot '{_activeSlotId}'.");
                    return;
                }

                var manifest = CloneManifest(loaded.Envelope.manifest);
                update(manifest);
                loaded.Envelope.manifest = manifest;
                loaded.Envelope.aggregateChecksum = SaveSlotService.ComputeAggregateChecksum(loaded.Envelope);
                if (!_slotService.WriteAggregateAtomically(_currentProfileId, _activeSlotId.Value, loaded.Envelope))
                {
                    GD.PrintErr($"[SaveLoad] Manifest update refused: campaign envelope commit failed for slot '{_activeSlotId}'.");
                    return;
                }

                _activeEnvelope = loaded.Envelope;
                TryWriteManifestProjection(manifest);
                return;
            }

            // Empty slots have no campaign envelope yet; retain the manifest-only
            // creation compatibility path until their first aggregate commit.
            var manifestOnly = _slotService.LoadManifest(_currentProfileId, _activeSlotId.Value);
            if (manifestOnly == null) return;
            update(manifestOnly);
            _slotService.SaveManifest(_currentProfileId, _activeSlotId.Value, manifestOnly);
        }
        catch (Exception ex)
        {
            GD.PrintErr($"[SaveLoad] Manifest update failed for slot '{_activeSlotId}': {ex.Message}");
        }
    }

    /// <summary>
    /// Seal the active slot as terminal (run finalized). Keeps the campaign
    /// envelope on disk as an inspectable memorial/archive but marks the
    /// manifest TerminalLoss so continuation and deletion are blocked.
    /// Returns false when no slot is active.
    /// </summary>
    public bool MarkActiveSlotTerminal(int finalDay)
    {
        if (_slotService == null || _activeSlotId == null) return false;
        bool sealed_ = _slotService.MarkTerminal(_currentProfileId, _activeSlotId.Value, finalDay);
        if (sealed_)
        {
            GD.Print($"[SaveLoad] Slot '{_activeSlotId.Value}' marked terminal (day {finalDay}). Final state preserved as memorial.");
            SlotsChanged?.Invoke();
        }
        return sealed_;
    }

    /// <summary>
    /// Build a slot card for UI display.
    /// </summary>
    public SlotCard BuildSlotCard(SaveSlotId slotId)
    {
        var manifest = GetManifest(slotId);
        bool exists = manifest != null;
        // A run-finalized slot is terminal regardless of campaign mode: the
        // envelope stays as an inspectable memorial, but the slot reads as
        // sealed so the UI hides its delete/continue affordances.
        bool isTerminal = manifest != null &&
                          manifest.ironManTerminalState == IronManTerminalState.TerminalLoss;

        return new SlotCard
        {
            SlotId = slotId,
            Exists = exists,
            CampaignName = manifest != null ? manifest.campaignName : "(empty)",
            CurrentDay = manifest != null ? manifest.currentDay : 0,
            Mode = manifest != null ? manifest.mode : CampaignMode.Normal,
            IsTerminalIronMan = isTerminal,
            LastSaveTimestamp = manifest != null ? manifest.lastSaveTimestamp : string.Empty,
            HasValidSave = exists && _slotService != null && File.Exists(
                Path.Combine(_slotService.GetAggregatePath(_currentProfileId, slotId)))
        };
    }

    /// <summary>
    /// Migrate pre-slot global section files (the legacy save layout under the
    /// global user:// directory) into a fresh envelope-backed slot. Payloads
    /// are the section file bytes verbatim — nothing is translated — and the
    /// original files are left untouched. Corrupt individual sections are
    /// skipped with a warning so one bad file cannot block the whole
    /// migration. Returns the new slot ID, or null when no legacy sections
    /// were found or the envelope write failed.
    /// </summary>
    public SaveSlotId? MigrateLegacyGlobalSaves(string globalUserDir)
    {
        if (_slotService == null || string.IsNullOrEmpty(globalUserDir)) return null;

        var payloads = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var pair in SaveSectionRegistry.SectionFileNames)
        {
            string path = Path.Combine(globalUserDir, pair.Value);
            if (!File.Exists(path)) continue;
            try
            {
                string raw = File.ReadAllText(path);
                if (!string.IsNullOrWhiteSpace(raw))
                    payloads[pair.Key] = raw;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[SaveLoad] Skipping corrupt legacy section '{pair.Value}' during migration: {ex.Message}");
            }
        }

        if (payloads.Count == 0) return null;

        var existingSlots = new HashSet<string>(_slotService.ListSlots(_currentProfileId).ConvertAll(s => s.Value));
        string newSlotId = "migrated_1";
        int counter = 1;
        while (existingSlots.Contains(newSlotId))
        {
            counter++;
            newSlotId = $"migrated_{counter}";
        }
        var slotId = new SaveSlotId(newSlotId);
        if (!CreateSlot(slotId))
            return null;

        if (!SaveEnvelopeFromPayloads(payloads))
        {
            GD.PrintErr("[SaveLoad] Legacy migration failed to write the campaign envelope; removing the incomplete slot.");
            if (_slotService.DeleteSlot(_currentProfileId, slotId))
            {
                if (_activeSlotId.HasValue && _activeSlotId.Value == slotId)
                {
                    _activeSlotId = null;
                    _activeEnvelope = null;
                    _restoredSections.Clear();
                    ClearSelectionRollback();
                    ClearSlotRoot();
                    ActiveSlotChanged?.Invoke(null);
                }
                SlotsChanged?.Invoke();
            }
            return null;
        }

        GD.Print($"[SaveLoad] Migrated {payloads.Count} legacy sections into slot '{slotId}' (originals left in place).");
        return slotId;
    }

    /// <summary>
    /// Import a legacy single-file save into a new slot. Returns the new slot
    /// ID on success, or null on failure.
    /// </summary>
    public SaveSlotId? ImportLegacySave(string legacyFilePath)    {
        if (_slotService == null) return null;

        // Find an unused slot ID.
        var existingSlots = new HashSet<string>(_slotService.ListSlots(_currentProfileId).ConvertAll(s => s.Value));
        string newSlotId = "imported_1";
        int counter = 1;
        while (existingSlots.Contains(newSlotId))
        {
            counter++;
            newSlotId = $"imported_{counter}";
        }

        var slotId = new SaveSlotId(newSlotId);
        string error;
        bool imported = _slotService.TryImportLegacySave(legacyFilePath, _currentProfileId, slotId, out error);
        if (!imported)
        {
            GD.PrintErr("[SaveLoad] Legacy import failed: " + error);
            return null;
        }

        _activeSlotId = slotId;
        _activeEnvelope = null;
        _restoredSections.Clear();
        ClearSelectionRollback();
        ApplySlotRoot();
        SlotsChanged?.Invoke();
        ActiveSlotChanged?.Invoke(_activeSlotId);
        GD.Print($"[SaveLoad] Imported legacy save to slot: {slotId}");
        return slotId;
    }

    /// <summary>
    /// Apply the active slot root so all stores write under the selected slot.
    /// </summary>
    public void ApplySlotRoot()
    {
        if (_activeSlotId == null || _slotService == null)
        {
            ClearSlotRoot();
            return;
        }

        string slotRoot = _slotService.GetSlotRoot(_currentProfileId, _activeSlotId.Value);
        SaveSlotRoot.CurrentRoot = slotRoot;
        GD.Print($"[SaveLoad] Slot root applied: {slotRoot}");
    }

    /// <summary>
    /// Clear the slot root so stores fall back to global user:// paths.
    /// </summary>
    public void ClearSlotRoot()
    {
        SaveSlotRoot.CurrentRoot = null;
    }

    /// <summary>
    /// Refresh the active slot's manifest after a save.
    /// </summary>
    public void RefreshActiveManifest()
    {
        if (_activeSlotId == null) return;
        UpdateManifest(m =>
        {
            m.lastSaveTick = DateTime.UtcNow.Ticks;
            m.lastSaveTimestamp = DateTime.UtcNow.ToString("o");
        });
    }

    /// <summary>
    /// Envelope-primary save: build the campaign envelope directly from
    /// in-memory section payloads (SaveStore&lt;T&gt;.CapturePersisted bytes,
    /// keyed by SaveSectionRegistry section key) and write it as the single
    /// authoritative save file. No individual section files are touched.
    /// Returns false when no slot is active or the atomic write fails; a
    /// builder rejection (unknown section key) propagates as a save failure.
    /// </summary>
    public bool SaveEnvelopeFromPayloads(IReadOnlyDictionary<string, string> payloads)
    {
        if (_activeSlotId == null || _slotService == null)
        {
            // This is a normal, expected early-out for contexts that
            // deliberately run without a save/load host (e.g. the
            // composition-root architecture selftest), so it must not read
            // as an error — but it must never be perfectly silent either;
            // a caller that expects saves to persist needs to be able to
            // see why nothing was written.
            GD.Print(_slotService == null
                ? "[SaveLoad] SaveEnvelopeFromPayloads skipped: save/load host has no slot service in this context."
                : "[SaveLoad] SaveEnvelopeFromPayloads skipped: no active slot selected.");
            return false;
        }
        if (payloads == null) return false;

        try
        {
            // An explicit empty required capture is a failed save, not an
            // absent/optional section. Missing dictionary keys remain the
            // existing never-created contract; optional empty sections may be
            // omitted by the builder.
            var captured = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var pair in payloads)
            {
                if (string.IsNullOrWhiteSpace(pair.Value))
                {
                    if (!SaveSectionRegistry.TryGetSection(pair.Key, out var metadata) || metadata == null)
                    {
                        GD.PrintErr($"[SaveLoad] Aggregate save refused: unknown section '{pair.Key}' captured empty.");
                        return false;
                    }
                    if (metadata.RequiresSetup)
                    {
                        GD.PrintErr($"[SaveLoad] Aggregate save refused: required section '{pair.Key}' captured empty.");
                        return false;
                    }
                    continue;
                }
                captured[pair.Key] = pair.Value;
            }

            if (captured.Count == 0)
            {
                GD.PrintErr("[SaveLoad] Aggregate save refused: no non-empty section captures were supplied.");
                return false;
            }

            string aggregatePath = _slotService.GetAggregatePath(_currentProfileId, _activeSlotId.Value);
            SaveManifest manifest;
            if (File.Exists(aggregatePath))
            {
                // Never rebuild a current save from the manifest projection;
                // campaign.json is the source for the next generation.
                var loaded = _slotService.TryLoadAggregate(_currentProfileId, _activeSlotId.Value);
                if (!loaded.IsSuccess || loaded.Envelope?.manifest == null)
                {
                    GD.PrintErr($"[SaveLoad] Aggregate save refused: current campaign envelope could not be loaded for slot '{_activeSlotId}'.");
                    return false;
                }
                manifest = CloneManifest(loaded.Envelope.manifest);
            }
            else
            {
                manifest = _slotService.LoadManifest(_currentProfileId, _activeSlotId.Value) ?? new SaveManifest
                {
                    profileId = _currentProfileId,
                    slotId = _activeSlotId.Value,
                    campaignName = $"Campaign {_activeSlotId.Value}",
                    currentDay = 1,
                    seed = 0,
                    mode = CampaignMode.Normal,
                    ironManTerminalState = IronManTerminalState.Active,
                };
            }

            manifest.profileId = _currentProfileId;
            manifest.slotId = _activeSlotId.Value;
            manifest.lastSaveTick = DateTime.UtcNow.Ticks;
            manifest.lastSaveTimestamp = DateTime.UtcNow.ToString("o");
            manifest.generationId = $"gen_{_activeSlotId.Value.Value}_{manifest.lastSaveTick}";

            // Strict mode rejects empty required captures and the registry
            // whitelist rejects any unknown key before campaign.json changes.
            var envelope = CampaignEnvelopeBuilder.Build(captured, manifest, rejectEmptyPayloads: true);
            if (envelope.sections == null || envelope.sections.Count == 0)
            {
                GD.PrintErr("[SaveLoad] Aggregate save refused: campaign envelope contains no captured sections.");
                return false;
            }

            if (!_slotService.WriteAggregateAtomically(_currentProfileId, _activeSlotId.Value, envelope))
            {
                GD.PrintErr("[SaveLoad] Aggregate save commit failed; previous campaign envelope preserved.");
                return false;
            }

            _activeEnvelope = envelope;
            ClearSelectionRollback();
            TryWriteManifestProjection(envelope.manifest);
            SlotsChanged?.Invoke();
            GD.Print($"[SaveLoad] Wrote campaign envelope with {envelope.sections.Count} sections (single atomic write).");
            return true;
        }
        catch (Exception ex)
        {
            GD.PrintErr($"[SaveLoad] Envelope save failed, previous envelope preserved: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Attempt to load and validate a specific slot. Returns true on success.
    /// On failure, provides a recoverable user-facing message, preserves live session,
    /// and fires OnLoadCompleted.
    /// </summary>
    public bool TryLoadSlot(SaveSlotId slotId, out SaveLoadResult result)
    {
        if (_slotService == null)
        {
            result = SaveLoadResult.Fail(SaveLoadStatus.MissingSlot, "Save slot service is not initialized.");
            LastLoadResult = result;
            OnLoadCompleted?.Invoke(result);
            return false;
        }

        var aggregateResult = _slotService.TryLoadAggregate(_currentProfileId, slotId);
        if (!aggregateResult.IsSuccess || aggregateResult.Envelope == null)
        {
            RestoreSelectionAfterFailedLoad(slotId);
            result = aggregateResult;
            LastLoadResult = result;
            OnLoadCompleted?.Invoke(result);
            GD.PrintErr($"[SaveLoad] Load failed for slot '{slotId}': {result.UserMessage}");
            return false;
        }

        // campaign.json has already passed aggregate validation. The section
        // files are only derived compatibility projections, so stage every
        // projection and commit/rollback them as one host operation before
        // exposing the new envelope or slot root to the rest of the game.
        if (!TryProjectEnvelope(aggregateResult.Envelope, slotId, out var restored, out var projectionErrors))
        {
            RestoreSelectionAfterFailedLoad(slotId);
            result = SaveLoadResult.Fail(
                SaveLoadStatus.CorruptData,
                $"Save slot '{slotId.Value}' could not restore its derived section projections. Live session preserved.",
                projectionErrors);
            LastLoadResult = result;
            OnLoadCompleted?.Invoke(result);
            GD.PrintErr($"[SaveLoad] Load failed while unpacking slot '{slotId}': {string.Join("; ", projectionErrors)}");
            return false;
        }

        _activeSlotId = slotId;
        _activeEnvelope = aggregateResult.Envelope;
        _restoredSections.Clear();
        foreach (string sectionName in restored)
            _restoredSections.Add(sectionName);
        ApplySlotRoot();

        result = aggregateResult;
        LastLoadResult = result;
        OnLoadCompleted?.Invoke(result);
        ActiveSlotChanged?.Invoke(_activeSlotId);
        ClearSelectionRollback();
        GD.Print($"[SaveLoad] Unpacked and loaded slot: {slotId} ({_restoredSections.Count} sections)");
        return true;
    }

    private bool TryProjectEnvelope(
        AggregateSaveEnvelope envelope,
        SaveSlotId slotId,
        out HashSet<string> restoredSections,
        out List<string> errors)
    {
        restoredSections = new HashSet<string>(StringComparer.Ordinal);
        errors = new List<string>();

        if (envelope == null || envelope.sections == null || envelope.sections.Count == 0)
        {
            errors.Add("campaign.json contains no section projections to restore.");
            return false;
        }

        var desired = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var section in envelope.sections)
        {
            if (section == null)
            {
                errors.Add("campaign.json contains a null section.");
                continue;
            }
            if (string.IsNullOrWhiteSpace(section.sectionName))
            {
                errors.Add("campaign.json contains a section with no name.");
                continue;
            }
            if (string.IsNullOrWhiteSpace(section.payloadJson))
            {
                errors.Add($"Section '{section.sectionName}' has an empty payload.");
                continue;
            }

            string? sectionFile = string.Equals(
                section.sectionName, SaveSlotService.LegacyImportSectionName, StringComparison.Ordinal)
                ? "legacy.json"
                : SaveSectionRegistry.FileNameFor(section.sectionName);
            if (string.IsNullOrEmpty(sectionFile))
            {
                errors.Add($"Section '{section.sectionName}' has no registered projection file.");
                continue;
            }
            if (desired.ContainsKey(sectionFile))
            {
                errors.Add($"Multiple aggregate sections map to projection file '{sectionFile}'.");
                continue;
            }

            desired[sectionFile] = section.payloadJson;
            restoredSections.Add(section.sectionName);
        }

        if (errors.Count > 0)
            return false;

        if (_slotService == null)
        {
            errors.Add("Save slot service is not initialized.");
            return false;
        }

        string slotRoot = _slotService.GetSlotRoot(_currentProfileId, slotId);
        string stageRoot = Path.Combine(slotRoot, ".campaign_projection_tmp");
        // A crash between individual per-file moves in the commit loop below
        // can leave a slot root with a mix of old and new derived/compat
        // files. campaign.json itself is unaffected (it already committed
        // before this method runs), and every Continue/TryLoadSlot call
        // recomputes `desired` from campaign.json and rewrites every file
        // unconditionally, so a subsequent successful load self-heals the
        // mix. This marker exists so an interrupted transaction is
        // detectable by anything that inspects the slot root directly
        // (diagnostics, selftests) rather than relying on that self-healing
        // happening to run first.
        string inProgressMarkerPath = Path.Combine(slotRoot, ".campaign_projection_inprogress");
        if (File.Exists(inProgressMarkerPath))
        {
            // Evidence of a crash during a previous commit loop. campaign.json
            // remains authoritative and this pass recomputes every derived
            // file from it unconditionally, so the transaction below still
            // repairs the slot; only log so the interruption is not silent.
            GD.PrintErr($"[SaveLoad] Slot '{slotId}' has a leftover projection-in-progress marker from an interrupted commit; re-projecting from campaign.json to repair.");
        }

        var staleFiles = new HashSet<string>(StringComparer.Ordinal);
        const string legacyProjectionFile = "legacy.json";
        string legacyTargetPath = Path.Combine(slotRoot, legacyProjectionFile);
        if (!desired.ContainsKey(legacyProjectionFile) && File.Exists(legacyTargetPath))
            staleFiles.Add(legacyProjectionFile);

        foreach (var metadata in SaveSectionRegistry.All)
        {
            string fileName = SaveSectionRegistry.FileNameFor(metadata.SectionKey)!;
            if (desired.ContainsKey(fileName)) continue;

            string targetPath = Path.Combine(slotRoot, fileName);
            if (!File.Exists(targetPath)) continue;
            if (metadata.RequiresSetup)
            {
                // A required derived file without a corresponding aggregate
                // section is evidence of an out-of-generation projection. Do
                // not delete it or let it restore stale state: fail closed.
                errors.Add($"Required section '{metadata.SectionKey}' is absent from campaign.json but has a derived file.");
            }
            else
            {
                staleFiles.Add(fileName);
            }
        }

        foreach (string previousSection in _restoredSections)
        {
            if (restoredSections.Contains(previousSection)) continue;
            string? previousFile = string.Equals(
                previousSection, SaveSlotService.LegacyImportSectionName, StringComparison.Ordinal)
                ? "legacy.json"
                : SaveSectionRegistry.FileNameFor(previousSection);
            if (!string.IsNullOrEmpty(previousFile))
                staleFiles.Add(previousFile);
        }

        if (errors.Count > 0)
            return false;

        var originals = new Dictionary<string, string?>(StringComparer.Ordinal);
        bool committed = false;
        bool markerWritten = false;
        try
        {
            Directory.CreateDirectory(slotRoot);
            if (Directory.Exists(stageRoot))
                Directory.Delete(stageRoot, recursive: true);
            Directory.CreateDirectory(stageRoot);

            var transactionFiles = new HashSet<string>(desired.Keys, StringComparer.Ordinal);
            transactionFiles.UnionWith(staleFiles);
            foreach (string fileName in transactionFiles)
            {
                string targetPath = Path.Combine(slotRoot, fileName);
                originals[fileName] = File.Exists(targetPath)
                    ? File.ReadAllText(targetPath)
                    : null;
            }

            foreach (var pair in desired.OrderBy(p => p.Key, StringComparer.Ordinal))
            {
                string stagedPath = Path.Combine(stageRoot, pair.Key);
                File.WriteAllText(stagedPath, pair.Value);
                if (!string.Equals(File.ReadAllText(stagedPath), pair.Value, StringComparison.Ordinal))
                    throw new IOException($"staged projection '{pair.Key}' did not round-trip");
            }

            // All files are staged and verified. Mark the commit phase as
            // in-progress before the first destructive per-file move so a
            // crash partway through the loop below leaves unambiguous
            // evidence on disk, then clear the marker only once every move
            // and every stale-file deletion has completed.
            File.WriteAllText(inProgressMarkerPath, DateTime.UtcNow.ToString("o"));
            markerWritten = true;

            // Commit every derived projection only after all staging and reads
            // succeeded. A later failure restores the exact previous files.
            foreach (var pair in desired.OrderBy(p => p.Key, StringComparer.Ordinal))
            {
                string stagedPath = Path.Combine(stageRoot, pair.Key);
                string targetPath = Path.Combine(slotRoot, pair.Key);
                File.Move(stagedPath, targetPath, overwrite: true);
                if (!string.Equals(File.ReadAllText(targetPath), pair.Value, StringComparison.Ordinal))
                    throw new IOException($"projection '{pair.Key}' did not round-trip after commit");
            }

            foreach (string fileName in staleFiles.OrderBy(f => f, StringComparer.Ordinal))
            {
                string targetPath = Path.Combine(slotRoot, fileName);
                if (File.Exists(targetPath))
                    File.Delete(targetPath);
            }

            committed = true;
            return true;
        }
        catch (Exception ex)
        {
            errors.Add($"Derived section projection transaction failed: {ex.Message}");
            foreach (var original in originals)
            {
                try
                {
                    string targetPath = Path.Combine(slotRoot, original.Key);
                    if (original.Value == null)
                    {
                        if (File.Exists(targetPath))
                            File.Delete(targetPath);
                    }
                    else
                    {
                        File.WriteAllText(targetPath, original.Value);
                    }
                }
                catch (Exception rollbackEx)
                {
                    errors.Add($"Rollback of '{original.Key}' failed: {rollbackEx.Message}");
                }
            }
            // The exception path above already restored every file this
            // process could reach, so the transaction is not left
            // in-progress from this process's point of view.
            if (markerWritten)
            {
                try { if (File.Exists(inProgressMarkerPath)) File.Delete(inProgressMarkerPath); }
                catch (Exception markerEx) { errors.Add($"In-progress marker cleanup failed: {markerEx.Message}"); }
            }
            return false;
        }
        finally
        {
            try
            {
                if (Directory.Exists(stageRoot))
                    Directory.Delete(stageRoot, recursive: true);
            }
            catch (Exception cleanupEx)
            {
                if (committed)
                    GD.PrintErr($"[SaveLoad] Projection staging cleanup failed for slot '{slotId}': {cleanupEx.Message}");
                else
                    errors.Add($"Projection staging cleanup failed: {cleanupEx.Message}");
            }

            if (committed && markerWritten)
            {
                try { if (File.Exists(inProgressMarkerPath)) File.Delete(inProgressMarkerPath); }
                catch (Exception markerEx) { GD.PrintErr($"[SaveLoad] In-progress marker cleanup failed for slot '{slotId}': {markerEx.Message}"); }
            }
        }
    }

    private void TryWriteManifestProjection(SaveManifest manifest)
    {
        if (_slotService == null || _activeSlotId == null || manifest == null) return;
        try
        {
            _slotService.SaveManifest(_currentProfileId, _activeSlotId.Value, manifest);
        }
        catch (Exception ex)
        {
            // campaign.json has already committed and remains authoritative;
            // report the compatibility projection failure without claiming that
            // the aggregate save itself failed.
            GD.PrintErr($"[SaveLoad] Manifest compatibility projection failed for slot '{_activeSlotId}': {ex.Message}");
        }
    }

    private static SaveManifest CloneManifest(SaveManifest source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        return new SaveManifest
        {
            manifestVersion = source.manifestVersion,
            gameVersion = source.gameVersion,
            buildId = source.buildId,
            currentDay = source.currentDay,
            seed = source.seed,
            lastSaveTick = source.lastSaveTick,
            mode = source.mode,
            slotId = source.slotId,
            profileId = source.profileId,
            campaignName = source.campaignName,
            ironManTerminalState = source.ironManTerminalState,
            lastSaveTimestamp = source.lastSaveTimestamp,
            generationId = source.generationId,
        };
    }

    private void CaptureSelectionRollback(SaveSlotId targetSlotId)
    {
        _selectionRollbackSlotId = _activeSlotId;
        _selectionRollbackEnvelope = _activeEnvelope;
        _selectionRollbackRoot = SaveSlotRoot.CurrentRoot;
        _selectionRollbackSections = new HashSet<string>(_restoredSections, StringComparer.Ordinal);
        _selectionRollbackTargetSlotId = targetSlotId;
        _hasSelectionRollback = true;
    }

    private void ClearSelectionRollback()
    {
        _selectionRollbackSlotId = null;
        _selectionRollbackEnvelope = null;
        _selectionRollbackRoot = null;
        _selectionRollbackSections = null;
        _selectionRollbackTargetSlotId = null;
        _hasSelectionRollback = false;
    }

    private void RestoreSelectionAfterFailedLoad(SaveSlotId targetSlotId)
    {
        if (!_hasSelectionRollback) return;
        if (!_selectionRollbackTargetSlotId.HasValue || _selectionRollbackTargetSlotId.Value != targetSlotId)
        {
            // A direct load of a different slot must not consume a pending
            // rollback snapshot created by SelectSlot for another target.
            ClearSelectionRollback();
            return;
        }

        _activeSlotId = _selectionRollbackSlotId;
        _activeEnvelope = _selectionRollbackEnvelope;
        _restoredSections.Clear();
        if (_selectionRollbackSections != null)
        {
            foreach (string sectionName in _selectionRollbackSections)
                _restoredSections.Add(sectionName);
        }

        if (_activeSlotId == null)
            ClearSlotRoot();
        else
            SaveSlotRoot.CurrentRoot = _selectionRollbackRoot;

        SaveSlotId? restoredSlot = _activeSlotId;
        ClearSelectionRollback();
        ActiveSlotChanged?.Invoke(restoredSlot);
    }

    /// <summary>
    /// Attempt to load and validate the currently active slot.
    /// </summary>
    public bool TryLoadActiveSlot(out SaveLoadResult result)
    {
        if (_activeSlotId == null)
        {
            result = SaveLoadResult.Fail(SaveLoadStatus.MissingSlot, "No active save slot selected.");
            LastLoadResult = result;
            OnLoadCompleted?.Invoke(result);
            return false;
        }

        return TryLoadSlot(_activeSlotId.Value, out result);
    }

    /// <summary>
    /// Unpack the aggregate envelope for the active slot back into individual
    /// JSON save files. Returns true if the envelope was found and unpacked.
    /// </summary>
    public bool UnpackAggregateEnvelope()
    {
        if (_activeSlotId == null) return false;
        return TryLoadSlot(_activeSlotId.Value, out _);
    }

    /// <summary>
    /// Aggregate-first save: build an envelope directly from subsystem payloads
    /// without touching individual files.
    /// </summary>
    public bool SaveAllDirect(IReadOnlyDictionary<string, string> sectionPayloads)
    {
        return SaveEnvelopeFromPayloads(sectionPayloads);
    }

    /// <summary>
    /// Load-from-envelope: restore all subsystems directly from the aggregate
    /// envelope without reading individual files.
    /// </summary>
    public bool LoadAllDirect()
    {
        return UnpackAggregateEnvelope();
    }
}

/// <summary>
/// Immutable slot card for UI display.
/// </summary>
public class SlotEnvelopeHealth
{
    public bool EnvelopePresent { get; set; }
    /// <summary>True when the envelope file exists but failed to load (corrupt).</summary>
    public bool LoadFailed { get; set; }
    public int ManifestVersion { get; set; }
    public bool AggregateChecksumPresent { get; set; }
    public bool MigratedFromLegacy { get; set; }
    public int SectionCount { get; set; }
    /// <summary>"sectionName — ok/no checksum" lines, manifest order.</summary>
    public List<string> SectionLines { get; } = new();
}

public class SlotCard
{
    public SaveSlotId SlotId { get; set; } = new("empty");
    public bool Exists { get; set; }
    public string CampaignName { get; set; } = string.Empty;
    public int CurrentDay { get; set; }
    public CampaignMode Mode { get; set; }
    public bool IsTerminalIronMan { get; set; }
    public string LastSaveTimestamp { get; set; } = string.Empty;
    public bool HasValidSave { get; set; }
}
