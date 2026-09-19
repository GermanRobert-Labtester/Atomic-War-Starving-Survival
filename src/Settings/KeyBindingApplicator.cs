// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.Settings;
using AtomicWar.GodotApp.Host;

namespace AtomicWar.GodotApp.Settings
{
    /// <summary>
    /// Applies user keybinding overrides onto Godot's live InputMap.
    /// Provides scope-aware conflict detection, per-action and global reset,
    /// and safe-mode boot support.
    /// </summary>
    public static class KeyBindingApplicator
    {
        /// <summary>
        /// Diffs user overrides onto the live InputMap. Iterates the sorted
        /// canonical action list (deterministic order). Never touches ui_*.
        /// Returns applied-override count for diagnostics.
        /// </summary>
        public static int Apply(UserSettingsData data)
        {
            if (data == null) return 0;

            // Safe-mode boot: holding Shift at startup skips applying custom bindings
            if (Input.IsKeyPressed(Key.Shift))
            {
                GD.Print("[KeyBindingApplicator] Shift held at startup; safe-mode active, skipping keybinding overrides.");
                return 0;
            }

            int appliedCount = 0;
            var sortedContracts = AshfallInputActions.Contract
                .OrderBy(c => c.Action, StringComparer.Ordinal)
                .ToList();

            foreach (var contract in sortedContracts)
            {
                if (!contract.Rebindable) continue;

                if (data.KeyBindings != null &&
                    data.KeyBindings.TryGetValue(contract.Action, out var customKeys) &&
                    customKeys != null &&
                    customKeys.Count > 0)
                {
                    if (InputMap.HasAction(contract.Action))
                    {
                        // Erase existing key events (preserving any joypad events)
                        var existingEvents = InputMap.ActionGetEvents(contract.Action);
                        foreach (var ev in existingEvents)
                        {
                            if (ev is InputEventKey keyEv)
                            {
                                InputMap.ActionEraseEvent(contract.Action, keyEv);
                            }
                        }

                        // Add new key events from user configuration
                        foreach (int code in customKeys)
                        {
                            var newKeyEv = new InputEventKey
                            {
                                Keycode = (Key)code,
                                Pressed = false,
                                Echo = false
                            };
                            InputMap.ActionAddEvent(contract.Action, newKeyEv);
                        }
                        appliedCount++;
                    }
                }
            }

            return appliedCount;
        }

        /// <summary>
        /// Scope-aware conflict detection for a proposed binding.
        /// Global actions conflict with other global/playing actions.
        /// Context-scoped actions (JournalBook, HoldfastTerminal) only conflict within their scope.
        /// Returns the conflicting action name, or null if clear.
        /// </summary>
        public static string? FindConflict(string action, Key proposed, UserSettingsData? data = null)
        {
            var targetContract = AshfallInputActions.Contract.FirstOrDefault(c => c.Action == action);
            if (targetContract == null) return null;

            bool isTargetGlobalOrPlaying = targetContract.Scope == AshfallInputActions.InputScope.Global ||
                                          targetContract.Scope == AshfallInputActions.InputScope.PlayingOnly;

            foreach (var other in AshfallInputActions.Contract)
            {
                if (other.Action == action) continue;

                bool isOtherGlobalOrPlaying = other.Scope == AshfallInputActions.InputScope.Global ||
                                             other.Scope == AshfallInputActions.InputScope.PlayingOnly;

                // Scope compatibility check:
                // Globals conflict with globals/playing. Scoped only conflict with identical scope.
                bool canCollide = (isTargetGlobalOrPlaying && isOtherGlobalOrPlaying) ||
                                  (!isTargetGlobalOrPlaying && other.Scope == targetContract.Scope);

                if (!canCollide) continue;

                Key otherKey = GetEffectiveKey(other.Action, data);
                if (otherKey == proposed)
                {
                    return other.Action;
                }
            }

            return null;
        }

        /// <summary>
        /// Retrieves the currently effective key for an action (custom override if set, else canonical default).
        /// </summary>
        public static Key GetEffectiveKey(string action, UserSettingsData? data = null)
        {
            var bindings = data?.KeyBindings ?? UserSettingsStore.Current.KeyBindings;
            if (bindings != null && bindings.TryGetValue(action, out var customKeys) && customKeys.Count > 0)
            {
                return (Key)customKeys[0];
            }

            if (AshfallInputActions.CanonicalDefaults.TryGetValue(action, out var defaultKey))
            {
                return defaultKey;
            }

            return Key.None;
        }

        /// <summary>
        /// Resets one action to its CanonicalDefault in user settings and live InputMap.
        /// </summary>
        public static void Reset(UserSettingsData data, string action)
        {
            if (data?.KeyBindings != null)
            {
                data.KeyBindings.Remove(action);
            }

            if (InputMap.HasAction(action) && AshfallInputActions.CanonicalDefaults.TryGetValue(action, out var defaultKey))
            {
                var existingEvents = InputMap.ActionGetEvents(action);
                foreach (var ev in existingEvents)
                {
                    if (ev is InputEventKey keyEv)
                    {
                        InputMap.ActionEraseEvent(action, keyEv);
                    }
                }
                InputMap.ActionAddEvent(action, new InputEventKey
                {
                    Keycode = defaultKey,
                    Pressed = false,
                    Echo = false
                });
            }
        }

        /// <summary>
        /// Clears all keybinding overrides in user settings and restores all actions to CanonicalDefaults.
        /// </summary>
        public static void ResetAll(UserSettingsData data)
        {
            if (data?.KeyBindings != null)
            {
                data.KeyBindings.Clear();
            }

            foreach (var contract in AshfallInputActions.Contract)
            {
                if (contract.Rebindable)
                {
                    Reset(data!, contract.Action);
                }
            }
        }
    }
}
