using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Core system managing radio operation: frequency tuning, signal strength calculation,
    /// intel extraction, and integration with weather/knowledge map. Handles pre/post Day 30
    /// broadcast availability. Save/load safe.
    /// </summary>
    public class RadioTunerSystem
    {
        /// <summary>Day when military frequencies go silent and automated loops begin.</summary>
        public const int MilitarySilenceDay = 30;

        /// <summary>Hours required to fully tune a frequency at 100% signal.</summary>
        public const float BaseTuningHours = 2f;

        /// <summary>Base tuning rate (progress per hour at 100% signal).</summary>
        public float TuningRate => 1f / BaseTuningHours;

        private readonly List<RadioFrequencySO> _frequencies = new List<RadioFrequencySO>();
        private readonly List<IntelNode> _extractedIntel = new List<IntelNode>();
        private readonly Random _rng;

        /// <summary>Runtime radio state (power, signal, EMP damage, tuning).</summary>
        public RadioState State { get; private set; }

        /// <summary>Fired when intel is successfully extracted.</summary>
        public event Action<IntelNode> OnIntelExtracted;

        /// <summary>Fired when the current frequency changes.</summary>
        public event Action<string> OnFrequencyChanged;

        public RadioTunerSystem(Random rng = null)
        {
            State = new RadioState();
            _rng = rng ?? new Random();
        }

        /// <summary>Load frequency definitions from a catalog.</summary>
        public void SetFrequencies(IEnumerable<RadioFrequencySO> frequencies)
        {
            _frequencies.Clear();
            if (frequencies != null)
            {
                foreach (var freq in frequencies)
                {
                    if (freq != null)
                    {
                        _frequencies.Add(freq);
                    }
                }
            }
        }

        /// <summary>Get all registered frequencies.</summary>
        public IReadOnlyList<RadioFrequencySO> Frequencies => _frequencies;

        /// <summary>Get a frequency by ID.</summary>
        public RadioFrequencySO GetFrequency(string frequencyId)
        {
            if (string.IsNullOrEmpty(frequencyId)) return null;
            foreach (var freq in _frequencies)
            {
                if (freq != null && freq.id == frequencyId) return freq;
            }
            return null;
        }

        /// <summary>
        /// Tune to a specific frequency. Resets tuning progress.
        /// </summary>
        public bool TuneToFrequency(string frequencyId)
        {
            var freq = GetFrequency(frequencyId);
            if (freq == null) return false;

            State.ResetTuning(frequencyId);
            OnFrequencyChanged?.Invoke(frequencyId);
            return true;
        }

        /// <summary>
        /// Get the currently tuned frequency (null if not tuned).
        /// </summary>
        public RadioFrequencySO GetCurrentFrequency()
        {
            if (string.IsNullOrEmpty(State.CurrentFrequencyId)) return null;
            return GetFrequency(State.CurrentFrequencyId);
        }

        /// <summary>
        /// Calculate weather-based signal modifier based on current weather.
        /// Clear = 1.0, Ashfall = 0.8, Blizzard = 0.6, FalloutStorm = 0.2 (static).
        /// </summary>
        public static float GetWeatherSignalModifier(WeatherKind weather)
        {
            switch (weather)
            {
                case WeatherKind.Clear: return 1.0f;
                case WeatherKind.Ashfall: return 0.8f;
                case WeatherKind.Blizzard: return 0.6f;
                case WeatherKind.FalloutStorm: return 0.2f; // Heavy static
                default: return 1.0f;
            }
        }

        /// <summary>
        /// Update signal strength based on current frequency, weather, and EMP damage.
        /// Called each tick to keep signal current.
        /// </summary>
        public void UpdateSignalStrength(WeatherKind currentWeather)
        {
            var freq = GetCurrentFrequency();
            if (freq == null)
            {
                State.SignalStrength = 0f;
                return;
            }

            float weatherModifier = GetWeatherSignalModifier(currentWeather);
            State.UpdateSignalStrength(freq.baseSignalStrength, weatherModifier, freq.interferenceSusceptibility);
        }

        /// <summary>
        /// Advance radio operation over elapsed hours: consume fuel, update signal, advance tuning.
        /// Returns true if tuning completes during this tick.
        /// </summary>
        public bool Tick(float gameHours, WeatherKind currentWeather, int currentDay)
        {
            if (gameHours <= 0f) return false;

            // Consume fuel
            State.ConsumeFuel(gameHours);

            // Update signal strength
            UpdateSignalStrength(currentWeather);

            // Advance tuning if operational
            if (State.IsOperational && !string.IsNullOrEmpty(State.CurrentFrequencyId))
            {
                bool tuningComplete = State.AdvanceTuning(gameHours, TuningRate);
                if (tuningComplete)
                {
                    // Extract intel when tuning completes
                    ExtractIntel(currentDay);
                }
                return tuningComplete;
            }

            return false;
        }

        /// <summary>
        /// Extract intel from the currently tuned frequency. Called automatically when
        /// tuning completes. Creates an IntelNode based on frequency type and day.
        /// </summary>
        private void ExtractIntel(int currentDay)
        {
            var freq = GetCurrentFrequency();
            if (freq == null) return;

            // Get a broadcast from the frequency's pool
            var broadcast = freq.GetRandomBroadcast(_rng);
            if (broadcast == null) return;

            // Determine intel type based on frequency and current day
            IntelNode intel = CreateIntelFromBroadcast(freq, broadcast, currentDay);

            if (intel != null)
            {
                _extractedIntel.Add(intel);
                OnIntelExtracted?.Invoke(intel);
            }
        }

        /// <summary>
        /// Create an IntelNode from a broadcast. Type depends on frequency type and day.
        /// Pre-Day 30: MortarWarning, TroopMovement, WeatherForecast
        /// Post-Day 30: PlumeReport, EmergencyLoop, NumbersStation
        /// </summary>
        private IntelNode CreateIntelFromBroadcast(RadioFrequencySO freq, RadioBroadcastSO broadcast, int currentDay)
        {
            int expirationDay = currentDay + 5; // Intel expires in 5 days

            // Pre-Day 30: Military/civilian frequencies provide tactical intel
            if (currentDay < MilitarySilenceDay)
            {
                if (freq.type == RadioFrequencyType.Military)
                {
                    // Military frequencies: MortarWarning or TroopMovement
                    return IntelNode.CreateMortarWarning(
                        broadcast.id,
                        0.7f, // confidence
                        currentDay,
                        expirationDay,
                        broadcast.message
                    );
                }
                else if (freq.type == RadioFrequencyType.Civilian)
                {
                    // Civilian frequencies: WeatherForecast or generic broadcast
                    return IntelNode.CreateWeatherForecast(
                        (int)WeatherKind.Clear, // Simplified: assume clear weather
                        0.6f,
                        currentDay,
                        expirationDay,
                        broadcast.message
                    );
                }
            }
            // Post-Day 30: Automated loops and numbers stations
            else
            {
                if (freq.type == RadioFrequencyType.Emergency || freq.type == RadioFrequencyType.NumbersStation)
                {
                    // Plume reports: radiation data for map update
                    return IntelNode.CreatePlumeReport(
                        broadcast.id, // Use broadcast ID as location ID (simplified)
                        50f, // Rumored rad level (placeholder)
                        0.5f, // Low confidence (post-war intel is unreliable)
                        currentDay,
                        expirationDay,
                        broadcast.message
                    );
                }
            }

            // Fallback: generic intel
            return new IntelNode
            {
                Id = Guid.NewGuid().ToString("N").Substring(0, 8),
                Type = IntelType.Unknown,
                SourceFrequencyId = freq.id,
                ExtractedDay = currentDay,
                ExpirationDay = expirationDay,
                Confidence = 0.3f,
                Text = broadcast.message
            };
        }

        /// <summary>
        /// Get all extracted intel nodes.
        /// </summary>
        public IReadOnlyList<IntelNode> ExtractedIntel => _extractedIntel;

        /// <summary>
        /// Get unexpired, unconsumed intel nodes.
        /// </summary>
        public List<IntelNode> GetActiveIntel(int currentDay)
        {
            var active = new List<IntelNode>();
            foreach (var intel in _extractedIntel)
            {
                if (!intel.IsConsumed && !intel.IsExpired(currentDay))
                {
                    active.Add(intel);
                }
            }
            return active;
        }

        /// <summary>
        /// Apply a plume report to the knowledge map. Returns true if applied.
        /// </summary>
        public bool ApplyPlumeReportToMap(IntelNode intel, RadiationKnowledgeMap map)
        {
            if (intel == null || map == null) return false;
            if (intel.Type != IntelType.PlumeReport) return false;
            if (intel.IsConsumed || intel.IsExpired(GetCurrentDay())) return false;

            // Apply the plume report to update the map's rumored rad
            map.SetRumor(intel.TargetLocationId, intel.NumericValue, 1f - intel.Confidence);
            intel.IsConsumed = true;
            return true;
        }

        /// <summary>
        /// Refuel the radio.
        /// </summary>
        public void Refuel(float amount)
        {
            State.Refuel(amount);
        }

        /// <summary>
        /// Repair EMP damage.
        /// </summary>
        public float Repair(float amount)
        {
            return State.Repair(amount);
        }

        /// <summary>
        /// Apply EMP damage to the radio.
        /// </summary>
        public bool ApplyEmpDamage(float damage)
        {
            return State.ApplyEmpDamage(damage);
        }

        /// <summary>
        /// Capture state for save/load.
        /// </summary>
        public RadioTunerSave CaptureState()
        {
            var save = new RadioTunerSave
            {
                RadioState = State.CaptureState(),
                ExtractedIntel = new List<IntelNode>(_extractedIntel)
            };
            return save;
        }

        /// <summary>
        /// Restore state from save/load.
        /// </summary>
        public void RestoreState(RadioTunerSave save)
        {
            if (save == null) return;
            State.RestoreState(save.RadioState);
            _extractedIntel.Clear();
            if (save.ExtractedIntel != null)
            {
                _extractedIntel.AddRange(save.ExtractedIntel);
            }
        }
    }

    /// <summary>
    /// Save/load snapshot of radio tuner system state.
    /// </summary>
    [Serializable]
    public class RadioTunerSave
    {
        public RadioStateSave RadioState;
        public List<IntelNode> ExtractedIntel = new List<IntelNode>();
    }
}
