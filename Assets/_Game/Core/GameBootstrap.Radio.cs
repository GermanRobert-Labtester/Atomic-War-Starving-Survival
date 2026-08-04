using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        private void InitializeRadioFrequencies()
        {
            // Create default frequencies. interceptChannelTag links each band to
            // faction intercept filtering (intel extraction uses the same dial).
            var civilian = ScriptableObject.CreateInstance<RadioFrequencySO>();
            civilian.id = RadioFrequencySO.Ids.Civilian;
            civilian.displayName = "88.5 FM Civilian";
            civilian.frequencyMHz = 88.5f;
            civilian.type = RadioFrequencyType.Civilian;
            civilian.activeFromDay = 0;
            civilian.activeUntilDay = 30;
            civilian.baseSignalStrength = 0.7f;
            civilian.interferenceSusceptibility = 0.3f;
            civilian.interceptChannelTag = RadioFrequencySO.DefaultChannelTagForType(RadioFrequencyType.Civilian);

            var military = ScriptableObject.CreateInstance<RadioFrequencySO>();
            military.id = RadioFrequencySO.Ids.Military;
            military.displayName = "102.1 Military";
            military.frequencyMHz = 102.1f;
            military.type = RadioFrequencyType.Military;
            military.activeFromDay = 0;
            military.activeUntilDay = 30;
            military.baseSignalStrength = 0.6f;
            military.interferenceSusceptibility = 0.2f;
            military.interceptChannelTag = RadioFrequencySO.DefaultChannelTagForType(RadioFrequencyType.Military);

            var numbers = ScriptableObject.CreateInstance<RadioFrequencySO>();
            numbers.id = RadioFrequencySO.Ids.Numbers;
            numbers.displayName = "99.0 Numbers Station";
            numbers.frequencyMHz = 99.0f;
            numbers.type = RadioFrequencyType.NumbersStation;
            numbers.activeFromDay = 31;
            numbers.activeUntilDay = -1;
            numbers.baseSignalStrength = 0.4f;
            numbers.interferenceSusceptibility = 0.5f;
            numbers.interceptChannelTag = RadioFrequencySO.DefaultChannelTagForType(RadioFrequencyType.NumbersStation);

            var emergency = ScriptableObject.CreateInstance<RadioFrequencySO>();
            emergency.id = RadioFrequencySO.Ids.Emergency;
            emergency.displayName = "107.0 Emergency";
            emergency.frequencyMHz = 107.0f;
            emergency.type = RadioFrequencyType.Emergency;
            emergency.activeFromDay = 31;
            emergency.activeUntilDay = -1;
            emergency.baseSignalStrength = 0.5f;
            emergency.interferenceSusceptibility = 0.4f;
            emergency.interceptChannelTag = RadioFrequencySO.DefaultChannelTagForType(RadioFrequencyType.Emergency);

            RadioTunerSystem.SetFrequencies(new[] { civilian, military, numbers, emergency });
        }

        // ─────────────────────────────────────────────────────────────────
        // Prompt #46 — Radio → EventRunner bridge + Safe Haven ambush wiring.
        // The radio is a narrative tool, not just an intel sink: broadcasts
        // with a triggerEventId surface as player choices (send the team,
        // analyze the audio, warn other wastelanders) — and a careless
        // expedition on a Trap broadcast is a casualty-producing decision.
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Radio-broadcast listener: when a broadcast with a triggerEventId
        /// plays AND a survivor is at the radio, raise the named event
        /// through EventRunner.Run. Mirrors the standard hourly event tick
        /// but uses a context tagged with <c>IsOnRadio=true</c> so the
        /// event's RequiredFlagId gate resolves and the modal fires.
        /// </summary>
        private void HandleRadioBroadcastTrigger(RadioBroadcastSO broadcast)
        {
            if (broadcast == null || string.IsNullOrEmpty(broadcast.triggerEventId)) return;
            if (EventRunner == null) return;

            // The player must be at the radio for the broadcast to surface
            // as an interactive choice. Without IsOnRadio, the event stays
            // in the pool — the loop is just audio flavor.
            bool anyoneAtRadio = false;
            if (Survivors != null)
            {
                for (int i = 0; i < Survivors.Count; i++)
                {
                    var s = Survivors[i];
                    if (s == null || !s.IsAlive) continue;
                    // The listen-to-radio AI action sets CurrentRoomId to the
                    // radio station; in test scenes we accept the flag as well.
                    if (s.CurrentRoomId == "radio" || s.CurrentRoomId == "radio_station")
                    {
                        anyoneAtRadio = true;
                        break;
                    }
                }
            }
            if (!anyoneAtRadio) return;

            // Build a context tagged with IsOnRadio and the broadcast's id
            // so the named event can also gate on a per-broadcast flag.
            var ctx = BuildEventContext(TimeSystem != null ? TimeSystem.CurrentDay : 1);
            ctx.IsOnRadio = true;
            ctx.SetEventFlag("is_on_radio", true);
            ctx.SetEventFlag("broadcast_" + broadcast.id, true);

            // Prompt #47 — the medical convoy broadcast also opens the
            // Blood for Water gate. We do this here (rather than in the
            // event's CanTrigger) so the gate stays decoupled from the
            // radio bridge: the convoy can also be triggered by a
            // hatch-visit faction event in the future without code
            // changes to the radio path.
            if (broadcast.id == "medical_convoy_announcement")
            {
                ctx.SetEventFlag("is_blood_for_water_offered", true);
            }

            // Default reliability is Unverified; the player must verify
            // (or get ambushed) to flip it.
            ctx.ActiveIntelReliability = IntelReliability.Unverified;

            // Find the event by id; if it's already in the pool just Run it.
            var ev = EventRunner.FindInPool(broadcast.triggerEventId);
            if (ev == null)
            {
                Debug.LogWarning($"[GameBootstrap] Radio broadcast '{broadcast.id}' wants event " +
                                 $"'{broadcast.triggerEventId}' but it is not in the EventRunner pool.");
                return;
            }
            EventRunner.Run(ev, ctx);
        }

        /// <summary>
        /// Push RadioTunerSystem.State (signal, tuned label, lock progress) onto
        /// the intercept strip so StatusLine / TunerLine stay live each frame.
        /// </summary>
        public void PushRadioLiveStateToHud()
        {
            if (_hud == null || RadioTunerSystem == null) return;
            var strip = _hud.EnsureRadioInterceptHud();
            if (strip == null) return;

            var state = RadioTunerSystem.State;
            if (state == null)
            {
                strip.ClearLiveRadioState();
                return;
            }

            // Keep signal current even between radio ticks (weather / EMP can change).
            if (WeatherSystem != null)
                RadioTunerSystem.UpdateSignalStrength(WeatherSystem.Current);

            var freq = RadioTunerSystem.GetCurrentFrequency();
            string label = string.Empty;
            float mhz = 0f;
            if (freq != null)
            {
                mhz = freq.frequencyMHz;
                if (!string.IsNullOrEmpty(freq.displayName))
                    label = freq.displayName;
                else if (mhz > 0f)
                    label = $"{mhz:0.#} MHz";
                else
                    label = freq.id ?? string.Empty;

                // Append intercept channel tag when present (matches dial labels).
                string tag = freq.ResolveInterceptChannelTag();
                if (!string.IsNullOrEmpty(tag) && !label.Contains(tag))
                    label = $"{label} · {tag}";
            }

            strip.SetLiveRadioState(
                signalStrength: state.SignalStrength,
                tunedFrequencyLabel: label,
                frequencyMHz: mhz,
                tuningProgress: state.TuningProgress,
                isOperational: state.IsOperational);
        }

        /// <summary>
        /// Rebuild the radio strip from the intercept log (after WireHUD / save load).
        /// </summary>
        public void SyncRadioInterceptHudFromLog()
        {
            if (_hud == null || FactionRadioIntercepts == null) return;
            var strip = _hud.EnsureRadioInterceptHud();
            if (strip == null) return;

            // Ensure bands are bound before applying a saved tuner index.
            if (RadioTunerSystem != null && strip.BandCount <= 1)
                WireRadioInterceptTuner();

            var log = FactionRadioIntercepts.Log;
            var lines = new System.Collections.Generic.List<RadioInterceptHUD.Line>(log.Count);
            for (int i = 0; i < log.Count; i++)
            {
                var e = log[i];
                if (e == null || string.IsNullOrEmpty(e.Message)) continue;
                lines.Add(new RadioInterceptHUD.Line
                {
                    Message = e.Message,
                    Kind = e.Kind ?? string.Empty,
                    FactionId = e.FactionId ?? string.Empty,
                    Day = e.Day,
                    ChannelTag = DynamicEconomySystem.GetParleyChannelTag(e.FactionId)
                });
            }
            strip.SetLines(lines);
            // Restore presentation (open / unread / tuner). notifyTuner=true so
            // RadioTunerSystem re-tunes to the saved dial for intel extraction.
            strip.ApplyUiState(
                FactionRadioIntercepts.HudIsOpen,
                FactionRadioIntercepts.HudHasUnread,
                FactionRadioIntercepts.HudTunerIndex,
                notifyTuner: true);
        }

    }
}
