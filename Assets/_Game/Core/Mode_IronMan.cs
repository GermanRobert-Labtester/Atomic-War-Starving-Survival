// Mode_IronMan.cs — Iron Man Mode (Prompt #862)
// SaveSystem deletes save when last survivor dies. No savescumming. Permanent history.
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Serializable state for Iron Man mode (Prompt #862).
    /// Once enabled, cannot be disabled mid-game.
    /// </summary>
    [Serializable]
    public class IronManState
    {
        public string mode_id = "mode_iron_man";
        public bool is_active;
        public string save_path = string.Empty;
        public bool last_survivor_died;
        public bool save_deleted;
        public string death_log = string.Empty;
    }

    /// <summary>
    /// Iron Man mode (Prompt #862).
    /// When the last survivor dies, the save file is immediately deleted.
    /// Death is logged to a separate file for memorial.
    /// No loading previous saves. Cannot be disabled mid-game.
    /// </summary>
    public class Mode_IronMan
    {
        // ── Events ─────────────────────────────────────────────────────
        public event Action OnIronManEnabled;
        public event Action<string, int> OnSurvivorDied;
        public event Action<string> OnLastSurvivorDied;
        public event Action<string> OnSaveDeleted;
        public event Action<string> OnDeathLogged;

        // ── State ──────────────────────────────────────────────────────
        private IronManState _state = new IronManState();

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Enable Iron Man mode for a given save path.
        /// Cannot be disabled once activated.
        /// </summary>
        public void EnableIronMan(string savePath)
        {
            _state.is_active = true;
            _state.save_path = savePath;
            _state.last_survivor_died = false;
            _state.save_deleted = false;
            OnIronManEnabled?.Invoke();
        }

        /// <summary>
        /// Called when a survivor dies. If this was the last survivor
        /// (remainingCount == 0), triggers save deletion flow.
        /// </summary>
        public void OnSurvivorDeath(string survivorId, int remainingCount)
        {
            if (!_state.is_active)
                return;

            string entry = $"[{DateTime.UtcNow:O}] Survivor '{survivorId}' died. Remaining: {remainingCount}";
            _state.death_log += entry + "\n";
            OnDeathLogged?.Invoke(entry);
            OnSurvivorDied?.Invoke(survivorId, remainingCount);

            if (remainingCount <= 0)
            {
                _state.last_survivor_died = true;
                OnLastSurvivorDied?.Invoke(survivorId);
            }
        }

        /// <summary>
        /// Returns true when the save should be deleted (last survivor died).
        /// </summary>
        public bool ShouldDeleteSave()
        {
            return _state.is_active && _state.last_survivor_died && !_state.save_deleted;
        }

        /// <summary>
        /// Delete the save file. The death log is preserved in a separate
        /// memorial file.
        /// </summary>
        public void DeleteSave()
        {
            if (_state.save_deleted)
                return;

            string path = _state.save_path;

            // Write memorial death log alongside before deleting save
            if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(_state.death_log))
            {
                string memorialPath = Path.ChangeExtension(path, ".memorial.txt");
                try
                {
                    File.WriteAllText(memorialPath, _state.death_log);
                }
                catch (Exception ex)
                {
                    // Best-effort memorial write; don't block save deletion
                    Debug.LogWarning($"[Mode_IronMan] Failed to write memorial at '{memorialPath}': {ex}");
                }
            }

            // Delete the main save file, backup file (.bak), and temp file (.tmp)
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    if (File.Exists(path)) File.Delete(path);
                    string bak = path + ".bak";
                    if (File.Exists(bak)) File.Delete(bak);
                    string tmp = path + ".tmp";
                    if (File.Exists(tmp)) File.Delete(tmp);
                }
                catch (Exception ex)
                {
                    // Deletion failed (locked file, permissions, etc.) — do not
                    // report success, or the UI will believe the save is gone.
                    _state.save_deleted = false;
                    Debug.LogError($"[Mode_IronMan] Failed to delete save at '{path}': {ex}");
                    return;
                }
            }

            _state.save_deleted = true;
            OnSaveDeleted?.Invoke(path);
        }

        /// <summary>
        /// Returns true when Iron Man mode is currently active.
        /// </summary>
        public bool IsIronManActive()
        {
            return _state.is_active;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public IronManState CaptureState()
        {
            return new IronManState
            {
                mode_id = string.IsNullOrEmpty(_state.mode_id) ? "mode_iron_man" : _state.mode_id,
                is_active = _state.is_active,
                save_path = _state.save_path ?? string.Empty,
                last_survivor_died = _state.last_survivor_died,
                save_deleted = _state.save_deleted,
                death_log = _state.death_log ?? string.Empty
            };
        }

        public void RestoreState(IronManState state)
        {
            if (state == null)
            {
                _state = new IronManState();
                return;
            }
            _state = new IronManState
            {
                mode_id = string.IsNullOrEmpty(state.mode_id) ? "mode_iron_man" : state.mode_id,
                is_active = state.is_active,
                save_path = state.save_path ?? string.Empty,
                last_survivor_died = state.last_survivor_died,
                save_deleted = state.save_deleted,
                death_log = state.death_log ?? string.Empty
            };
        }
    }
}
