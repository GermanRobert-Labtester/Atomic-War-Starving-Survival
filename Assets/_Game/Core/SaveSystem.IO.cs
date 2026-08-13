using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Ashfall.Core; // SaveChecksum (host-independent save integrity hash)
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Simulation; // CompostSystem, SterilizationSystem, etc. (audit C-3 split)
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Events;
using AtomicWar._Game.Utilities; // SaveSlotPaths (slot file naming shared with the main menu)

namespace AtomicWar._Game.Core
{
    public partial class SaveSystem
    {
        /// <summary>Write the current world state to the given slot.</summary>
        public bool Save(string slotId)
        {
            try
            {
                _preCaptureHook?.Invoke();
                var snapshot = CaptureSnapshot();
                if (_gameState != null && snapshot.GameState != null)
                {
                    snapshot.GameState.Phase = _gameState.Phase;
                    snapshot.GameState.Day = _gameState.Day;
                    snapshot.GameState.IsPaused = _gameState.IsPaused;
                }

                // C-2: the checksum covers the snapshot's STATE, not its serialized text, so a
                // save written by the Unity host still verifies under the Godot host and vice
                // versa. Hashing pretty-printed JSON coupled save validity to indent width and
                // null-string spelling, which differ between JsonUtility and System.Text.Json.
                // SaveChecksum skips the root Checksum field itself, so no placeholder dance and
                // no mutating the snapshot to blank it first.
                snapshot.Checksum = SaveChecksum.Compute(snapshot);
                string finalJson = JsonUtility.ToJson(snapshot, true);

                Directory.CreateDirectory(_savesDir);

                // A-1: Atomic save write. Write to a temp file, then rename over
                // the destination. If the process dies during the write, the temp
                // file is left partial but the previous save remains intact. We
                // also keep a .bak of the previous save.
                string finalPath = SlotPath(slotId);
                string tmpPath = finalPath + ".tmp";
                string bakPath = finalPath + ".bak";

                File.WriteAllText(tmpPath, finalJson);

                if (File.Exists(finalPath))
                {
                    // Single atomic replace that also rotates the previous save
                    // into .bak. The previous implementation deleted the .bak,
                    // copied final -> .bak, deleted final, and only then moved
                    // tmp into place -- so there was a window in which the slot
                    // did not exist at all, despite the comment claiming the
                    // sequence was atomic. A crash inside that window cost the
                    // main save and forced a .bak recovery on the next load.
                    //
                    // File.Replace is used rather than the three-argument
                    // File.Move: the overwrite overload is .NET Core 3.0+, and
                    // this project targets the .NET Standard 2.1 profile
                    // (apiCompatibilityLevel 6), where it does not exist.
                    File.Replace(tmpPath, finalPath, bakPath);
                }
                else
                {
                    // First save into this slot: nothing to replace or back up.
                    File.Move(tmpPath, finalPath);
                }

                // Unconditional: fires once per save, and the save/load audit trail is
                // the record used to diagnose corrupt-slot reports from players.
                Debug.Log($"[SaveSystem] Saved to slot '{slotId}' (atomic write + .bak backup).");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveSystem] Save to '{slotId}' failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// True after a successful <see cref="Load"/>; cleared on failed load.
        /// Hosts should refuse AutoSave while this is false after a failed Continue
        /// so a hybrid world cannot overwrite a good slot (SAVE-003 partial guard).
        /// </summary>
        public bool LastLoadSucceeded { get; private set; } = true;

        /// <summary>Replace the current world state from the given slot.</summary>
        public bool Load(string slotId)
        {
            string path = SlotPath(slotId);
            if (!File.Exists(path))
            {
                Debug.LogWarning($"[SaveSystem] Slot '{slotId}' not found.");
                LastLoadSucceeded = false;
                SuppressAutoSave = true;
                return false;
            }

            try
            {
                (bool ok, SaveData data, string json) = TryLoadFile(path);

                // A-1: If the main save failed (corrupt/unparseable/bad checksum),
                // try the .bak backup before giving up.
                if (!ok)
                {
                    string bakPath = BakPath(slotId);
                    if (File.Exists(bakPath))
                    {
                        Debug.LogWarning($"[SaveSystem] Slot '{slotId}' main save failed. Attempting recovery from backup...");
                        (ok, data, json) = TryLoadFile(bakPath);
                        if (ok)
                            Debug.LogWarning($"[SaveSystem] Backup recovered successfully for slot '{slotId}'.");
                        else
                        {
                            Debug.LogError($"[SaveSystem] Backup also corrupt. Load aborted.");
                            LastLoadSucceeded = false;
                            SuppressAutoSave = true;
                            return false;
                        }
                    }
                    else
                    {
                        Debug.LogError($"[SaveSystem] Slot '{slotId}' corrupt and no backup available. Load aborted.");
                        LastLoadSucceeded = false;
                        SuppressAutoSave = true;
                        return false;
                    }
                }

                // Forward compatibility: a save written by a newer build can
                // contain fields and semantics this build has no migration for.
                // Restoring it anyway silently produced a half-initialised world
                // that the next autosave would then write back over the player's
                // good save. Refuse instead -- a readable error beats silent
                // corruption, and the file is left untouched for a newer build.
                if (data.SaveVersion > CurrentSaveVersion)
                {
                    Debug.LogError(
                        $"[SaveSystem] Slot '{slotId}' was written by a newer build " +
                        $"(save version {data.SaveVersion}, this build supports up to " +
                        $"{CurrentSaveVersion}). Refusing to load rather than restoring " +
                        "partial state. Update the game to open this save.");
                    LastLoadSucceeded = false;
                    SuppressAutoSave = true;
                    return false;
                }

                if (data.SaveVersion < CurrentSaveVersion)
                {
                    Migrate(data);
                }

                RestoreFromSnapshot(data);

                // HARDEN: Post-restore sanity check. A corrupt or incomplete
                // save could restore with zero survivors or a negative day,
                // producing a zombie world that the next autosave would
                // write back over the player's good slot.
                if (!ValidateAfterRestore(data))
                {
                    Debug.LogError(
                        $"[SaveSystem] Slot '{slotId}' failed post-restore validation. " +
                        "Refusing to commit the restored state.");
                    LastLoadSucceeded = false;
                    SuppressAutoSave = true;
                    return false;
                }

                Debug.Log($"[SaveSystem] Loaded slot '{slotId}' (version {data.SaveVersion}).");
                LastLoadSucceeded = true;
                // Successful load clears suppress only if host is not holding Continue gate.
                // Host re-enables after Continue finally block.
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveSystem] Load from '{slotId}' failed: {ex.Message}");
                LastLoadSucceeded = false;
                SuppressAutoSave = true;
                return false;
            }
        }

        /// <summary>
        /// A-1: Helper — try to load and validate a single save file.
        /// Returns (true, data, json) if the file is valid, or (false, null, null)
        /// if the file is missing, unparseable, or fails checksum.
        /// AUDIT-005: parse / null / checksum failures are logged (no silent catch).
        /// </summary>
        private (bool ok, SaveData data, string json) TryLoadFile(string path)
        {
            if (!File.Exists(path)) return (false, null, null);
            string json;
            SaveData data;
            try
            {
                json = File.ReadAllText(path);
                data = JsonUtility.FromJson<SaveData>(json);
            }
            catch (Exception ex)
            {
                // AUDIT-005: corrupt / truncated JSON must be observable in logs.
                Debug.LogWarning(
                    $"[SaveSystem] Failed to parse save file '{path}': {ex.GetType().Name}: {ex.Message}");
                return (false, null, null);
            }
            if (data == null)
            {
                Debug.LogWarning(
                    $"[SaveSystem] Corrupt save parse: '{path}' produced null SaveData.");
                return (false, null, null);
            }
            if (!VerifyChecksum(data, json))
            {
                Debug.LogWarning(
                    $"[SaveSystem] Checksum mismatch for save file '{path}'.");
                return (false, null, null);
            }
            return (true, data, json);
        }

        /// <summary>Whether a save exists for the given slot.</summary>
        public bool SlotExists(string slotId) => File.Exists(SlotPath(slotId));

        /// <summary>All slot ids that have save files.</summary>
        public string[] ListSlots()
        {
            if (!Directory.Exists(_savesDir)) return Array.Empty<string>();
            return Directory.GetFiles(_savesDir, SaveSlotPaths.SlotFileName("*"))
                .Select(f => SaveSlotPaths.SlotIdFromFileName(Path.GetFileName(f)))
                .Where(slotId => slotId != null)
                .ToArray();
        }

        /// <summary>Auto-save to the "autosave" slot.</summary>
        public void AutoSave() => Save("autosave");

        /// <summary>
        /// Release all subscriptions and event-bus references held by this
        /// SaveSystem. Call this when the SaveSystem is replaced (e.g. a
        /// "new game" flow, or a test fixture that re-creates the
        /// bootstrap). Idempotent: calling Dispose twice is a no-op.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            // Unsubscribe the OnPhaseChanged handler that the constructor
            // attached to GameState. The event is a static-style class event
            // so the handler is held by GameState's static delegate field;
            // leaving it attached keeps the old SaveSystem alive forever.
            if (_gameState != null)
            {
                _gameState.OnPhaseChanged -= OnPhaseChanged;
            }
            // Note: SaveSystem does not subscribe to EventBus directly. The
            // Companion systems (SaveData-bound) do their own subscribe via
            // EventRunner.SetPool. Disposing the SaveSystem is therefore
            // sufficient to break the static event reference.
        }

        private static string ComputeChecksum(string json)
        {
            using var sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(json));
            var sb = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        /// <summary>
        /// C-2: state-based verification, with a fallback to the legacy text-based scheme so saves
        /// written before this change still load. A legacy save re-verifies only on the host that
        /// wrote it — that is inherent to the old scheme and is what this change exists to end —
        /// but nothing that used to load stops loading, and the next Save rewrites the slot with a
        /// portable checksum.
        /// </summary>
        private static bool VerifyChecksum(SaveData data, string rawJson)
        {
            string saved = data.Checksum;
            if (string.IsNullOrEmpty(saved)) return false;

            if (string.Equals(SaveChecksum.Compute(data), saved, StringComparison.Ordinal))
                return true;

            return VerifyLegacyTextChecksum(data, saved);
        }

        /// <summary>
        /// Pre-C-2 scheme: SHA256 over the pretty-printed JSON with the Checksum field blanked.
        /// Retained for backward compatibility only; do not use for new saves.
        /// </summary>
        private static bool VerifyLegacyTextChecksum(SaveData data, string saved)
        {
            string original = data.Checksum;
            try
            {
                data.Checksum = "";
                string computed = ComputeChecksum(JsonUtility.ToJson(data, true));
                return string.Equals(computed, saved, StringComparison.Ordinal);
            }
            finally
            {
                // Restore even if serialization throws: the caller's SaveData must not be left
                // with its checksum blanked.
                data.Checksum = original;
            }
        }

        /// <summary>
        /// HARDEN: Post-restore sanity check. Verifies that the restored world
        /// state is coherent enough to continue. Catches corrupt saves where
        /// individual systems restored successfully but the aggregate world
        /// doesn't make sense (zero survivors on a non-empty save, negative day,
        /// etc.). Returns false to signal Load should abort rather than commit
        /// a broken state that the next autosave would write over the player's
        /// good slot.
        /// </summary>
        private bool ValidateAfterRestore(SaveData data)
        {
            if (data == null) return false;

            // Day must not be negative (day < 0 means corrupted state).
            if (data.GameState != null && data.GameState.Day < 0)
            {
                Debug.LogWarning("[SaveSystem] Post-restore validation failed: GameState.Day < 0.");
                return false;
            }

            // A save that claims to have survivors must actually have them.
            if (data.Survivors != null && data.Survivors.Count > 0)
            {
                bool anyAlive = false;
                for (int i = 0; i < data.Survivors.Count; i++)
                {
                    if (data.Survivors[i] != null && data.Survivors[i].State != SurvivorState.Dead)
                    {
                        anyAlive = true;
                        break;
                    }
                }
                if (!anyAlive)
                {
                    // All survivors dead is valid (Iron Man trigger) — don't block.
                    // Only block if the save says there are survivors but ALL are null.
                    bool allNull = true;
                    for (int i = 0; i < data.Survivors.Count; i++)
                    {
                        if (data.Survivors[i] != null) { allNull = false; break; }
                    }
                    if (allNull)
                    {
                        Debug.LogWarning("[SaveSystem] Post-restore validation failed: all survivor entries are null.");
                        return false;
                    }
                }
            }

            // SaveVersion must be within supported range.
            if (data.SaveVersion < 1 || data.SaveVersion > CurrentSaveVersion)
            {
                Debug.LogWarning(
                    $"[SaveSystem] Post-restore validation failed: unsupported SaveVersion {data.SaveVersion}.");
                return false;
            }

            return true;
        }

        private static void Migrate(SaveData data)
        {
            if (data.SaveVersion < 2) MigrateV1toV2(data);
            if (data.SaveVersion < 3) MigrateV2toV3(data);
        }

        /// <summary>V1 -> V2 migration: V1 saves lack the FlashpointChoreographer
        /// snapshot. Default values leave the choreographer in a fresh state
        /// (no buildup days processed, choreography not started). The
        /// WorldPhaseSystem.HasTriggeredExchange flag in the same save
        /// determines whether the choreography restarts on next load.</summary>
        private static void MigrateV1toV2(SaveData data)
        {
            data.FlashpointChoreographer = null;
            data.SaveVersion = 2;
        }

        /// <summary>V2 -> V3 migration (H-4 ISaveable refactor): add paired
        /// SubsystemSaveIds/SubsystemSaveJsons lists. Positional fields are
        /// preserved for backward compat — existing state stays in positional
        /// fields; the paired lists start empty. Migration is a no-op for data
        /// integrity since the ISaveable path is additive.</summary>
        private static void MigrateV2toV3(SaveData data)
        {
            data.SubsystemSaveIds = new List<string>();
            data.SubsystemSaveJsons = new List<string>();
            data.SaveVersion = 3;
        }

        // Naming lives in SaveSlotPaths so the main menu's "Continue" probe
        // resolves the same files without duplicating the convention.
        private string SlotPath(string slotId) => SaveSlotPaths.SlotPath(_savesDir, slotId);

        /// <summary>A-1: Path to the backup save file (previous version).</summary>
        private string BakPath(string slotId) => SaveSlotPaths.BakPath(_savesDir, slotId);
    }
}
