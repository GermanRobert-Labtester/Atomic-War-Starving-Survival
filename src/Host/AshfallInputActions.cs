// SPDX-License-Identifier: MIT
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
        /// <summary>C2 / Plan 17B Phase D — onboarding guidance (F2 default).
        /// The plan's requested F1 binding collides with <see cref="Help"/>
        /// (tutorial panel, pre-existing binding); F2 is the nearest free
        /// function key. Rebindable via the standard input conventions.</summary>
        public const string Guidance = "ashfall_guidance";
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
            Guidance,
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

        /// <summary>Scope in which an action fires. Drives conflict policy and the gate.</summary>
        public enum InputScope
        {
            Global,
            PlayingOnly,
            JournalBook,
            HoldfastTerminal
        }

        /// <summary>
        /// Static contract row consumed by the gate script and the rebinding UI.
        /// Single source for: action -> scope -> route (nullable) -> rebindable flag.
        /// </summary>
        public sealed record InputActionContract(
            string Action,
            InputScope Scope,
            string? RouteId,
            bool Rebindable);

        public static readonly IReadOnlyList<InputActionContract> Contract = new[]
        {
            new InputActionContract(Close, InputScope.Global, null, true),
            new InputActionContract(Confirm, InputScope.Global, null, true),
            new InputActionContract(NextTab, InputScope.Global, null, true),
            new InputActionContract(NavUp, InputScope.Global, null, true),
            new InputActionContract(NavDown, InputScope.Global, null, true),
            new InputActionContract(NavLeft, InputScope.Global, null, true),
            new InputActionContract(NavRight, InputScope.Global, null, true),
            new InputActionContract(Journal, InputScope.PlayingOnly, "journal", true),
            new InputActionContract(Help, InputScope.PlayingOnly, "help", true),
            new InputActionContract(Guidance, InputScope.PlayingOnly, "guidance", true),
            new InputActionContract(Forecast, InputScope.PlayingOnly, "weather_forecast", true),
            new InputActionContract(WeatherHistory, InputScope.PlayingOnly, "weather_history", true),
            new InputActionContract(Events, InputScope.PlayingOnly, "events_log", true),
            new InputActionContract(Expeditions, InputScope.PlayingOnly, "expeditions", true),
            new InputActionContract(Holdfast, InputScope.PlayingOnly, "holdfast", true),
            new InputActionContract(JournalTab1, InputScope.JournalBook, null, false),
            new InputActionContract(JournalTab2, InputScope.JournalBook, null, false),
            new InputActionContract(JournalTab3, InputScope.JournalBook, null, false),
            new InputActionContract(JournalTab4, InputScope.JournalBook, null, false),
            new InputActionContract(JournalTab5, InputScope.JournalBook, null, false),
            new InputActionContract(HoldfastBuild, InputScope.HoldfastTerminal, null, false),
            new InputActionContract(HoldfastStatus, InputScope.HoldfastTerminal, null, false)
        };

        public static readonly IReadOnlyDictionary<string, Key> CanonicalDefaults = new Dictionary<string, Key>
        {
            { Close, Key.Escape },
            { Confirm, Key.Enter },
            { NextTab, Key.Tab },
            { NavUp, Key.Up },
            { NavDown, Key.Down },
            { NavLeft, Key.Left },
            { NavRight, Key.Right },
            { Journal, Key.J },
            { Help, Key.F1 },
            { Guidance, Key.F2 },
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

        public static bool IsCloseOrCancel(InputEvent @event)
        {
            return @event.IsActionPressed(Close) || @event.IsActionPressed(UiCancel);
        }

        public static bool IsConfirm(InputEvent @event)
        {
            return @event.IsActionPressed(Confirm) || @event.IsActionPressed(UiAccept);
        }

        public static bool IsNextTab(InputEvent @event)
        {
            return @event.IsActionPressed(NextTab);
        }

        public static bool IsNavUp(InputEvent @event) => @event.IsActionPressed(NavUp);
        public static bool IsNavDown(InputEvent @event) => @event.IsActionPressed(NavDown);
        public static bool IsNavLeft(InputEvent @event) => @event.IsActionPressed(NavLeft);
        public static bool IsNavRight(InputEvent @event) => @event.IsActionPressed(NavRight);

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

        public static bool IsGuidance(InputEvent @event)
        {
            return @event.IsActionPressed(Guidance);
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
