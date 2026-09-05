using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Canonical InputMap action names and runtime registration for ASHFALL.
    /// Maps all player actions to rebindable actions with controller and keyboard defaults.
    /// </summary>
    public static class AshfallInputActions
    {
        // Global navigation and window management
        public const string Close = "ashfall_close";
        public const string UiCancel = "ui_cancel";
        public const string Confirm = "ashfall_confirm";
        public const string UiAccept = "ui_accept";
        public const string NextTab = "ashfall_next_tab";

        // Navigation
        public const string NavUp = "ashfall_nav_up";
        public const string NavDown = "ashfall_nav_down";
        public const string NavLeft = "ashfall_nav_left";
        public const string NavRight = "ashfall_nav_right";

        // Global shortcuts
        public const string Journal = "ashfall_journal";
        public const string Help = "ashfall_help";
        public const string Forecast = "ashfall_forecast";
        public const string WeatherHistory = "ashfall_weather_history";
        public const string Events = "ashfall_events";
        public const string Expeditions = "ashfall_expeditions";
        public const string Holdfast = "ashfall_holdfast";

        // Journal Tabs
        public const string JournalTab1 = "ashfall_journal_tab_1";
        public const string JournalTab2 = "ashfall_journal_tab_2";
        public const string JournalTab3 = "ashfall_journal_tab_3";
        public const string JournalTab4 = "ashfall_journal_tab_4";
        public const string JournalTab5 = "ashfall_journal_tab_5";

        // Holdfast Terminal Controls
        public const string HoldfastBuild = "ashfall_holdfast_build";
        public const string HoldfastStatus = "ashfall_holdfast_status";

        public static readonly IReadOnlyList<string> AllActions = new[]
        {
            Close,
            Confirm,
            NextTab,
            NavUp,
            NavDown,
            NavLeft,
            NavRight,
            Journal,
            Help,
            Forecast,
            WeatherHistory,
            Events,
            Expeditions,
            Holdfast,
            JournalTab1,
            JournalTab2,
            JournalTab3,
            JournalTab4,
            JournalTab5,
            HoldfastBuild,
            HoldfastStatus
        };

        public static readonly IReadOnlyDictionary<string, Key> CanonicalDefaults = new Dictionary<string, Key>
        {
            { Close, Key.Escape },
            { Confirm, Key.Enter },
            { NextTab, Key.Tab },
            { NavUp, Key.W },
            { NavDown, Key.S },
            { NavLeft, Key.A },
            { NavRight, Key.D },
            { Journal, Key.J },
            { Help, Key.F1 },
            { Forecast, Key.F },
            { WeatherHistory, Key.H },
            { Events, Key.E },
            { Expeditions, Key.X },
            { Holdfast, Key.T },
            { JournalTab1, Key.Key1 },
            { JournalTab2, Key.Key2 },
            { JournalTab3, Key.Key3 },
            { JournalTab4, Key.Key4 },
            { JournalTab5, Key.Key5 },
            { HoldfastBuild, Key.B },
            { HoldfastStatus, Key.S }
        };

        /// <summary>
        /// Ensures all canonical ASHFALL actions are registered in the runtime InputMap
        /// if not already loaded from project.godot, and reconciles pairwise collisions.
        /// </summary>
        public static void EnsureActionsRegistered()
        {
            RegisterAction(Close, Key.Escape, JoyButton.B);
            RegisterAction(Confirm, Key.Enter, JoyButton.A, Key.Space);
            RegisterAction(NextTab, Key.Tab, JoyButton.RightShoulder);
            RegisterAction(NavUp, Key.W, JoyButton.DpadUp, Key.Up);
            RegisterAction(NavDown, Key.S, JoyButton.DpadDown, Key.Down);
            RegisterAction(NavLeft, Key.A, JoyButton.DpadLeft, Key.Left);
            RegisterAction(NavRight, Key.D, JoyButton.DpadRight, Key.Right);
            RegisterAction(Journal, Key.J, JoyButton.Y);
            RegisterAction(Help, Key.F1, JoyButton.Back);
            RegisterAction(Forecast, Key.F);
            RegisterAction(WeatherHistory, Key.H);
            RegisterAction(Events, Key.E);
            RegisterAction(Expeditions, Key.X);
            RegisterAction(Holdfast, Key.T);
            RegisterAction(JournalTab1, Key.Key1);
            RegisterAction(JournalTab2, Key.Key2);
            RegisterAction(JournalTab3, Key.Key3);
            RegisterAction(JournalTab4, Key.Key4);
            RegisterAction(JournalTab5, Key.Key5);
            RegisterAction(HoldfastBuild, Key.B);
            RegisterAction(HoldfastStatus, Key.S);

            ReconcileCollisions();
        }

        public static int ReconcileCollisions()
        {
            int repaired = 0;
            var keyOwners = new Dictionary<Key, string>();

            foreach (var action in AllActions)
            {
                if (!InputMap.HasAction(action)) continue;
                Key primaryKey = Key.None;
                InputEventKey? keyEvent = null;

                foreach (var ev in InputMap.ActionGetEvents(action))
                {
                    if (ev is InputEventKey k)
                    {
                        primaryKey = k.PhysicalKeycode != Key.None ? k.PhysicalKeycode : k.Keycode;
                        keyEvent = k;
                        break;
                    }
                }

                if (primaryKey != Key.None)
                {
                    if (keyOwners.TryGetValue(primaryKey, out var existingAction))
                    {
                        // Collision detected! Repair this action to its canonical default.
                        if (CanonicalDefaults.TryGetValue(action, out var canonicalKey))
                        {
                            if (keyEvent != null)
                            {
                                InputMap.ActionEraseEvent(action, keyEvent);
                            }
                            var repairedEvent = new InputEventKey { Keycode = canonicalKey, PhysicalKeycode = canonicalKey };
                            InputMap.ActionAddEvent(action, repairedEvent);
                            repaired++;
                            keyOwners[canonicalKey] = action;
                        }
                    }
                    else
                    {
                        keyOwners[primaryKey] = action;
                    }
                }
            }
            return repaired;
        }

        private static void RegisterAction(string action, Key primaryKey, JoyButton? joyButton = null, Key? secondaryKey = null)
        {
            if (!InputMap.HasAction(action))
            {
                InputMap.AddAction(action);
            }

            bool hasKey = false;
            foreach (var ev in InputMap.ActionGetEvents(action))
            {
                if (ev is InputEventKey)
                {
                    hasKey = true;
                    break;
                }
            }

            if (!hasKey)
            {
                var k = new InputEventKey { Keycode = primaryKey, PhysicalKeycode = primaryKey };
                InputMap.ActionAddEvent(action, k);

                if (secondaryKey.HasValue)
                {
                    var k2 = new InputEventKey { Keycode = secondaryKey.Value, PhysicalKeycode = secondaryKey.Value };
                    InputMap.ActionAddEvent(action, k2);
                }

                if (joyButton.HasValue)
                {
                    var jb = new InputEventJoypadButton { ButtonIndex = joyButton.Value };
                    InputMap.ActionAddEvent(action, jb);
                }
            }
        }

        public static bool IsCloseOrCancel(InputEvent @event)
        {
            return @event.IsActionPressed(Close) || @event.IsActionPressed(UiCancel);
        }

        public static bool IsConfirm(InputEvent @event)
        {
            return @event.IsActionPressed(Confirm) || @event.IsActionPressed(UiAccept);
        }

        public static bool IsConfirmOrAccept(InputEvent @event)
        {
            return IsConfirm(@event);
        }

        public static bool IsNextTab(InputEvent @event)
        {
            return @event.IsActionPressed(NextTab);
        }

        public static bool IsForecast(InputEvent @event)
        {
            return @event.IsActionPressed(Forecast);
        }

        public static bool IsWeatherHistory(InputEvent @event)
        {
            return @event.IsActionPressed(WeatherHistory);
        }

        public static bool IsJournal(InputEvent @event)
        {
            return @event.IsActionPressed(Journal);
        }

        public static bool IsHelp(InputEvent @event)
        {
            return @event.IsActionPressed(Help);
        }

        public static bool IsEvents(InputEvent @event)
        {
            return @event.IsActionPressed(Events);
        }

        public static bool IsExpeditions(InputEvent @event)
        {
            return @event.IsActionPressed(Expeditions);
        }

        public static bool IsHoldfast(InputEvent @event)
        {
            return @event.IsActionPressed(Holdfast);
        }

        public static bool IsHoldfastBuild(InputEvent @event)
        {
            return @event.IsActionPressed(HoldfastBuild);
        }

        public static bool IsHoldfastStatus(InputEvent @event)
        {
            return @event.IsActionPressed(HoldfastStatus);
        }

        public static bool GetJournalTabNumber(InputEvent @event, out int tab)
        {
            if (@event.IsActionPressed(JournalTab1)) { tab = 1; return true; }
            if (@event.IsActionPressed(JournalTab2)) { tab = 2; return true; }
            if (@event.IsActionPressed(JournalTab3)) { tab = 3; return true; }
            if (@event.IsActionPressed(JournalTab4)) { tab = 4; return true; }
            if (@event.IsActionPressed(JournalTab5)) { tab = 5; return true; }
            tab = 0;
            return false;
        }

        /// <summary>
        /// Returns the human-readable prompt string for an action (e.g. "[J]" or "[F1]")
        /// derived dynamically from the runtime InputMap rather than hardcoding keys in tutorial copy.
        /// </summary>
        public static string GetActionPrompt(string action)
        {
            if (InputMap.HasAction(action))
            {
                var events = InputMap.ActionGetEvents(action);
                foreach (var ev in events)
                {
                    if (ev is InputEventKey key)
                    {
                        string keyStr = OS.GetKeycodeString(key.PhysicalKeycode != Key.None ? key.PhysicalKeycode : key.Keycode);
                        if (!string.IsNullOrEmpty(keyStr))
                            return $"[{keyStr}]";
                    }
                }
            }

            // Fallback default prompts if headless / uninitialized
            return action switch
            {
                Close => "[Esc]",
                Confirm => "[Enter]",
                NextTab => "[Tab]",
                Journal => "[J]",
                Help => "[F1]",
                Forecast => "[F]",
                WeatherHistory => "[H]",
                Events => "[E]",
                Expeditions => "[X]",
                Holdfast => "[T]",
                _ => $"[{action}]"
            };
        }
    }
}
