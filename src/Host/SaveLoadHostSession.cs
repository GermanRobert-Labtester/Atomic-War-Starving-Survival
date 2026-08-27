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
    private readonly HashSet<string> _restoredSections = new();

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

        _activeSlotId = slotId;
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
    /// Update manifest fields for the active slot. Call after a successful save.
    /// </summary>
    public void UpdateManifest(Action<SaveManifest> update)
    {
        if (_activeSlotId == null || _slotService == null) return;
        var manifest = _slotService.LoadManifest(_currentProfileId, _activeSlotId.Value);
        if (manifest == null) return;

        update(manifest);
        _slotService.SaveManifest(_currentProfileId, _activeSlotId.Value, manifest);
    }

    /// <summary>
    /// Build a slot card for UI display.
    /// </summary>
    public SlotCard BuildSlotCard(SaveSlotId slotId)
    {
        var manifest = GetManifest(slotId);
        bool exists = manifest != null;
        bool isTerminal = manifest != null && manifest.mode == CampaignMode.IronMan &&
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
            GD.PrintErr("[SaveLoad] Legacy migration failed to write the campaign envelope.");
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
        ApplySlotRoot();
        SlotsChanged?.Invoke();
        ActiveSlotChanged?.Invoke(_activeSlotId);
        GD.Print($"[SaveLoad] Imported legacy save to slot: {slotId}");
        return slotId;
    }

    /// <summary>
    /// Delete the active slot's campaign envelope (and its backup) so a
    /// finished run cannot be continued. The manifest is kept — slot history
    /// and iron-man policy survive; only the save payload is removed.
    /// </summary>
    public void ClearActiveSlotEnvelope()
    {
        if (_activeSlotId == null || _slotService == null) return;
        string aggregatePath = _slotService.GetAggregatePath(_currentProfileId, _activeSlotId.Value);
        try
        {
            if (File.Exists(aggregatePath)) File.Delete(aggregatePath);
            if (File.Exists(aggregatePath + ".bak")) File.Delete(aggregatePath + ".bak");
            SlotsChanged?.Invoke();
        }
        catch (Exception ex)
        {
            GD.PrintErr($"[SaveLoad] Failed to clear envelope for slot '{_activeSlotId}': {ex.Message}");
        }
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
        if (_activeSlotId == null || _slotService == null) return false;

        try
        {
            RefreshActiveManifest();
            var manifest = _slotService.LoadManifest(_currentProfileId, _activeSlotId.Value) ?? new SaveManifest
            {
                profileId = _currentProfileId,
                slotId = _activeSlotId.Value,
                campaignName = $"Campaign {_activeSlotId.Value}",
                currentDay = 1,
                seed = 0,
                mode = CampaignMode.Normal,
                ironManTerminalState = IronManTerminalState.Active,
                lastSaveTimestamp = DateTime.UtcNow.ToString("o"),
            };

            var envelope = CampaignEnvelopeBuilder.Build(payloads, manifest);
            bool written = _slotService.WriteAggregateAtomically(_currentProfileId, _activeSlotId.Value, envelope);
            if (written)
            {
                SlotsChanged?.Invoke();
                GD.Print($"[SaveLoad] Wrote campaign envelope with {envelope.sections.Count} sections (single atomic write).");
            }
            return written;
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

        result = _slotService.TryLoadAggregate(_currentProfileId, slotId);
        LastLoadResult = result;
        OnLoadCompleted?.Invoke(result);

        if (!result.IsSuccess)
        {
            GD.PrintErr($"[SaveLoad] Load failed for slot '{slotId}': {result.UserMessage}");
            return false;
        }

        _activeSlotId = slotId;
        ApplySlotRoot();

        // Unpack aggregate envelope sections into individual subsystem files on
        // disk. Sections are registry keys; each payload lands at the section's
        // registered file name so the unchanged SetupXxx store loads find it.
        // Unreserved/unknown sections fall back to "<sectionName>.json".
        if (result.Envelope?.sections != null)
        {
            string slotRoot = _slotService.GetSlotRoot(_currentProfileId, slotId);
            if (!System.IO.Directory.Exists(slotRoot))
                System.IO.Directory.CreateDirectory(slotRoot);

            _restoredSections.Clear();
            foreach (var section in result.Envelope.sections)
            {
                if (string.IsNullOrEmpty(section.sectionName) || string.IsNullOrEmpty(section.payloadJson))
                    continue;

                string sectionFile = SaveSectionRegistry.FileNameFor(section.sectionName) ?? section.sectionName + ".json";
                string filePath = System.IO.Path.Combine(slotRoot, sectionFile);
                try
                {
                    System.IO.File.WriteAllText(filePath, section.payloadJson);
                    _restoredSections.Add(section.sectionName);
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"[SaveLoad] Failed to unpack section '{section.sectionName}': {ex.Message}");
                }
            }
        }

        ActiveSlotChanged?.Invoke(_activeSlotId);
        GD.Print($"[SaveLoad] Unpacked and loaded slot: {slotId} ({_restoredSections.Count} sections)");
        return true;
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
