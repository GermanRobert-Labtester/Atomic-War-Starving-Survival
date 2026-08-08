using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Simulation; // CompostSystem, SterilizationSystem, etc. (audit C-3 split)
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Events;

namespace AtomicWar._Game.Core
{
    public partial class SaveSystem
    {
        /// <summary>
        /// Iterate all registered ISaveable systems, call CaptureState() on each,
        /// serialize to JSON, and store in the paired SubsystemSaveIds/SubsystemSaveJsons
        /// lists on SaveData. This is the generic save path — new systems only need
        /// to implement ISaveable and call Register() to be save-safe.
        /// </summary>
        private void CaptureSubsystemStates(SaveData data)
        {
            data.SubsystemSaveIds = new List<string>(_saveables.Count);
            data.SubsystemSaveJsons = new List<string>(_saveables.Count);
            for (int i = 0; i < _saveables.Count; i++)
            {
                var s = _saveables[i];
                if (s == null) continue;
                try
                {
                    object state = s.CaptureState();
                    string json = state != null ? JsonUtility.ToJson(state) : "null";
                    data.SubsystemSaveIds.Add(s.SaveId);
                    data.SubsystemSaveJsons.Add(json);
                }
                catch (Exception ex)
                {
                    // MISC-001: do not write a "successful" save that silently dropped
                    // a subsystem. Re-throw so Save() returns false and the previous
                    // slot file is left intact.
                    Debug.LogError($"[SaveSystem] ISaveable.CaptureState failed for '{s.SaveId}': {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Iterate the paired SubsystemSaveIds/SubsystemSaveJsons lists, find the
        /// matching registered ISaveable by SaveId, deserialize, and call RestoreState().
        /// Systems in the save but not registered are silently skipped (forward-compat).
        /// AUDIT-004: when <see cref="FailFastRestore"/> is true, uses two-phase
        /// restore (deserialize all, then apply all) and rethrows on first failure
        /// so Load aborts instead of leaving a hybrid world.
        /// </summary>
        private void RestoreSubsystemStates(SaveData data)
        {
            if (data.SubsystemSaveIds == null || data.SubsystemSaveJsons == null) return;
            int count = Mathf.Min(data.SubsystemSaveIds.Count, data.SubsystemSaveJsons.Count);
            if (FailFastRestore)
            {
                RestoreSubsystemStatesFailFast(data, count);
                return;
            }

            for (int i = 0; i < count; i++)
            {
                string id = data.SubsystemSaveIds[i];
                string json = data.SubsystemSaveJsons[i];
                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(json)) continue;
                ISaveable target = FindSaveable(id);
                if (target == null) continue; // forward-compat: system not registered
                try
                {
                    // Infer the type from a fresh CaptureState() call and deserialize.
                    // Small allocation during load (acceptable).
                    object template = target.CaptureState();
                    if (template == null) continue;
                    object state = JsonUtility.FromJson(json, template.GetType());
                    target.RestoreState(state);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[SaveSystem] ISaveable.RestoreState failed for '{id}': {ex.Message}");
                }
            }
        }

        /// <summary>
        /// AUDIT-004 fail-fast path: deserialize every registered subsystem first;
        /// only then apply RestoreState. Any deserialize failure aborts before any
        /// ISaveable is mutated. Any RestoreState failure rethrows after log.
        /// </summary>
        private void RestoreSubsystemStatesFailFast(SaveData data, int count)
        {
            var prepared = new List<(ISaveable target, object state, string id)>(count);
            for (int i = 0; i < count; i++)
            {
                string id = data.SubsystemSaveIds[i];
                string json = data.SubsystemSaveJsons[i];
                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(json)) continue;
                ISaveable target = FindSaveable(id);
                if (target == null) continue;
                try
                {
                    object template = target.CaptureState();
                    if (template == null) continue;
                    object state = JsonUtility.FromJson(json, template.GetType());
                    prepared.Add((target, state, id));
                }
                catch (Exception ex)
                {
                    Debug.LogError(
                        $"[SaveSystem] ISaveable deserialize failed for '{id}' (FailFastRestore): {ex.Message}");
                    throw;
                }
            }

            for (int i = 0; i < prepared.Count; i++)
            {
                var entry = prepared[i];
                try
                {
                    entry.target.RestoreState(entry.state);
                }
                catch (Exception ex)
                {
                    Debug.LogError(
                        $"[SaveSystem] ISaveable.RestoreState failed for '{entry.id}' (FailFastRestore): {ex.Message}");
                    throw;
                }
            }
        }

        private ISaveable FindSaveable(string id)
        {
            for (int j = 0; j < _saveables.Count; j++)
            {
                if (_saveables[j]?.SaveId == id)
                    return _saveables[j];
            }
            return null;
        }

    }
}
