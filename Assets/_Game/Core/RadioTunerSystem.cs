using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Core system managing radio operation: frequency tuning, signal strength calculation,
    /// intel extraction, and integration with weather/knowledge map. Handles pre/post Day 30
    /// broadcast availability. Save/load safe.
    /// </summary>
    public class RadioTunerSystem
    {
        private PersonalQuestSystem _personalQuests;
        private System.Func<System.Collections.Generic.IReadOnlyList<Survivor>> _getSurvivors;

        /// <summary>Prompt #239 — Voice of the Wastes: free power, instant intel, no traps.</summary>
        public void BindPersonalQuests(
            PersonalQuestSystem personalQuests,
            System.Func<System.Collections.Generic.IReadOnlyList<Survivor>> getSurvivors = null)
        {
            _personalQuests = personalQuests;
            _getSurvivors = getSurvivors;
        }

        public bool IsRadioPowerFree()
        {
            if (_personalQuests == null || _getSurvivors == null) return false;
            return _personalQuests.RadioPowerIsFree(_getSurvivors());
        }

        /// <summary>Day when military frequencies go silent and automated loops begin.</summary>
        public const int MilitarySilenceDay = 30;

        /// <summary>Hours required to fully tune a frequency at 100% signal.</summary>
        public const float BaseTuningHours = 2f;

        /// <summary>Base tuning rate (progress per hour at 100% signal).</summary>
        public float TuningRate => 1f / BaseTuningHours;

        private readonly List<RadioFrequencySO> _frequencies = new List<RadioFrequencySO>();
        private readonly List<IntelNode> _extractedIntel = new List<IntelNode>();
        private readonly System.Random _rng;

        /// <summary>Runtime radio state (power, signal, EMP damage, tuning).</summary>
        public RadioState State { get; private set; }

        /// <summary>Fired when intel is successfully extracted.</summary>
        public event Action<IntelNode> OnIntelExtracted;

        /// <summary>Fired when the current frequency changes.</summary>
        public event Action<string> OnFrequencyChanged;

        /// <summary>Injected delegate returning the current campaign day (from TimeSystem).</summary>
        private Func<int> _getDay;

        public RadioTunerSystem(System.Random rng = null, Func<int> getDay = null)
        {
            State = new RadioState();
            _rng = rng ?? AtomicWar._Game.Utilities.SeededRandom.CreateFixed("radiotunersystem");
            _getDay = getDay ?? (() => 0);
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

        /// <summary>
        /// Append a frequency if its id is not already registered (Prompt #19 ghosts).
        /// Returns true when the frequency was added.
        /// </summary>
        public bool AddFrequency(RadioFrequencySO frequency)
        {
            if (frequency == null || string.IsNullOrEmpty(frequency.id)) return false;
            for (int i = 0; i < _frequencies.Count; i++)
            {
                if (_frequencies[i] != null && _frequencies[i].id == frequency.id)
                    return false;
            }
            _frequencies.Add(frequency);
            return true;
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
        /// Clear the dial (no frequency). Intercept HUD shows ALL BANDS;
        /// intel extraction pauses until a frequency is selected.
        /// </summary>
        public void Detune()
        {
            if (string.IsNullOrEmpty(State.CurrentFrequencyId))
            {
                State.ResetTuning(null);
                return;
            }
            State.ResetTuning(null);
            State.SignalStrength = 0f;
            OnFrequencyChanged?.Invoke(null);
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
        /// Intercept channel tag for the currently tuned frequency, or empty
        /// when detuned / unmapped.
        /// </summary>
        public string GetCurrentInterceptChannelTag()
        {
            var freq = GetCurrentFrequency();
            return freq != null ? freq.ResolveInterceptChannelTag() : string.Empty;
        }

        /// <summary>
        /// Find a registered frequency that surfaces the given intercept channel tag.
        /// Prefers active-on-day when <paramref name="day"/> is provided.
        /// </summary>
        public RadioFrequencySO FindFrequencyForChannelTag(string channelTag, int day = -1)
        {
            if (string.IsNullOrEmpty(channelTag)) return null;
            RadioFrequencySO fallback = null;
            for (int i = 0; i < _frequencies.Count; i++)
            {
                var f = _frequencies[i];
                if (f == null) continue;
                string tag = f.ResolveInterceptChannelTag();
                if (!string.Equals(tag, channelTag, StringComparison.OrdinalIgnoreCase))
                    continue;
                if (day < 0 || f.IsActiveOnDay(day))
                    return f;
                if (fallback == null) fallback = f;
            }
            return fallback;
        }

        /// <summary>
        /// Build HUD tuner bands: index 0 = ALL (detuned), then each registered frequency.
        /// </summary>
        public List<RadioTunerBand> BuildTunerBands()
        {
            var bands = new List<RadioTunerBand>(_frequencies.Count + 1)
            {
                RadioTunerBand.AllBands
            };
            for (int i = 0; i < _frequencies.Count; i++)
            {
                var f = _frequencies[i];
                if (f == null || string.IsNullOrEmpty(f.id)) continue;
                bands.Add(RadioTunerBand.FromFrequency(f));
            }
            return bands;
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
                case WeatherKind.BlackRain: return 0.1f; // Oily storm static (Prompt #11)
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

            // #306 Theorist/Tinfoil Hat: paranoid sabotage detunes the radio.
            if (TryRollRadioSabotage())
            {
                Detune();
                return false;
            }

            // Consume fuel — Voice of the Wastes (#239) pays zero power/fuel.
            if (!IsRadioPowerFree())
                State.ConsumeFuel(gameHours);

            // Update signal strength
            UpdateSignalStrength(currentWeather);

            // Advance tuning if operational
            if (State.IsOperational && !string.IsNullOrEmpty(State.CurrentFrequencyId))
            {
                float rate = TuningRate;
                // Instant intel extraction for Voice of the Wastes.
                if (_personalQuests != null && _getSurvivors != null)
                {
                    var list = _getSurvivors();
                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i] != null && list[i].IsAlive
                                && _personalQuests.RadioIntelIsInstant(list[i]))
                            {
                                rate = 1000f;
                                break;
                            }
                        }
                    }
                }
                bool tuningComplete = State.AdvanceTuning(gameHours, rate);
                if (tuningComplete)
                {
                    // Extract intel when tuning completes
                    ExtractIntel(currentDay);
                }
                return tuningComplete;
            }

            return false;
        }

        /// <summary>Prompt #306 — Theorist/Tinfoil Hat randomly sabotages the tuned radio.</summary>
        private bool TryRollRadioSabotage()
        {
            if (_personalQuests == null || _getSurvivors == null) return false;
            if (string.IsNullOrEmpty(State.CurrentFrequencyId)) return false;
            var list = _getSurvivors();
            if (list == null) return false;
            for (int i = 0; i < list.Count; i++)
            {
                var sv = list[i];
                if (sv != null && sv.IsAlive && _personalQuests.ShouldSabotageRadio(sv, _rng))
                    return true;
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

            // #255 Pathological Liar: randomly plant false stash intel on the radio.
            if (TryInjectFalseLiarIntel(currentDay))
                return;

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
        /// #255 — if a living Deceptive Liar is in the bunker, ~30% chance the
        /// radio reports a fake stash node instead of real intel.
        /// </summary>
        private bool TryInjectFalseLiarIntel(int currentDay)
        {
            if (_personalQuests == null || _getSurvivors == null) return false;
            var list = _getSurvivors();
            if (list == null) return false;
            Survivor liar = null;
            for (int i = 0; i < list.Count; i++)
            {
                var s = list[i];
                if (s != null && s.IsAlive && _personalQuests.HasDeceptive(s))
                {
                    liar = s;
                    break;
                }
            }
            if (liar == null) return false;
            if (_rng.NextDouble() > 0.30) return false;

            string fakeId = _personalQuests.GenerateFalseIntelNode(liar, _rng);
            if (string.IsNullOrEmpty(fakeId)) return false;

            var fake = IntelNode.CreateLootLocation(
                locationId: fakeId,
                supplyValue: 0.9f,
                confidence: 0.25f,
                extractedDay: currentDay,
                expirationDay: currentDay + 3,
                text: "Stash report — coordinates look wrong. " + fakeId);
            fake.Id = fakeId;
            fake.SourceFrequencyId = State.CurrentFrequencyId;
            _extractedIntel.Add(fake);
            OnIntelExtracted?.Invoke(fake);
            return true;
        }

        /// <summary>
        /// Create an IntelNode from a broadcast. Type depends on frequency type and day.
        /// Pre-Day 30: MortarWarning, TroopMovement, WeatherForecast
        /// Post-Day 30: PlumeReport, EmergencyLoop, NumbersStation
        /// </summary>
        private IntelNode CreateIntelFromBroadcast(RadioFrequencySO freq, RadioBroadcastSO broadcast, int currentDay)
        {
            int expirationDay = currentDay + 5; // Intel expires in 5 days
            IntelNode intel = null;

            // Prompt #19 — ghost stations never yield plume/military/extraction intel.
            if (freq.type == RadioFrequencyType.GhostStation)
            {
                intel = IntelNode.CreateGhostLoop(
                    freq.id,
                    currentDay,
                    broadcast != null ? broadcast.message : string.Empty,
                    confidence: 0.15f);
                intel.SourceFrequencyId = freq.id;
                return intel;
            }

            // Pre-Day 30: Military/civilian frequencies provide tactical intel
            if (currentDay < MilitarySilenceDay)
            {
                if (freq.type == RadioFrequencyType.Military)
                {
                    // Military frequencies: MortarWarning or TroopMovement
                    intel = IntelNode.CreateMortarWarning(
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
                    intel = IntelNode.CreateWeatherForecast(
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
                    intel = IntelNode.CreatePlumeReport(
                        broadcast.id, // Use broadcast ID as location ID (simplified)
                        50f, // Rumored rad level (placeholder)
                        0.5f, // Low confidence (post-war intel is unreliable)
                        currentDay,
                        expirationDay,
                        broadcast.message
                    );
                }
            }

            if (intel == null)
            {
                // Fallback: generic intel
                intel = new IntelNode
                {
                    Id = Guid.NewGuid().ToString("N").Substring(0, 8),
                    Type = IntelType.Unknown,
                    ExtractedDay = currentDay,
                    ExpirationDay = expirationDay,
                    Confidence = 0.3f,
                    Text = broadcast.message
                };
            }

            // Always stamp source so VictoryProject can count military decrypts.
            intel.SourceFrequencyId = freq.id;
            return intel;
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
        /// Optionally also updates the proc-gen wasteland graph (rumoredRad + reveal).
        /// </summary>
        public bool ApplyPlumeReportToMap(
            IntelNode intel,
            RadiationKnowledgeMap map,
            GeneratedMap generatedMap = null)
        {
            if (intel == null || map == null) return false;
            if (intel.Type != IntelType.PlumeReport) return false;
            if (intel.IsConsumed || intel.IsExpired(GetCurrentDay())) return false;

            // Apply the plume report to update the map's rumored rad
            map.SetRumor(intel.TargetLocationId, intel.NumericValue, 1f - intel.Confidence);

            // Radio intel (#22): reveal silhouette + push rumored rad onto the node graph
            if (generatedMap != null && !string.IsNullOrEmpty(intel.TargetLocationId))
            {
                generatedMap.SetRumoredRad(intel.TargetLocationId, intel.NumericValue);
                // High-confidence reports fully reveal the site; low confidence only rumors.
                if (intel.Confidence >= 0.55f)
                    generatedMap.RevealNode(intel.TargetLocationId);
            }

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
        /// Current campaign day, resolved from the injected TimeSystem delegate.
        /// Returns 0 when not yet wired (safe default: no intel expiration).
        /// </summary>
        private int GetCurrentDay()
        {
            return _getDay != null ? _getDay() : 0;
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

    /// <summary>
    /// One dial position shared by RadioTunerSystem (intel) and the intercept HUD.
    /// Index 0 is always ALL BANDS (detuned).
    /// </summary>
    [Serializable]
    public struct RadioTunerBand
    {
        public string FrequencyId;
        public string Label;
        public string ChannelTag;
        public float FrequencyMHz;
        public RadioFrequencyType Type;
        public int ActiveFromDay;
        public int ActiveUntilDay;

        public bool IsAllBands => string.IsNullOrEmpty(FrequencyId);

        public static RadioTunerBand AllBands => new RadioTunerBand
        {
            FrequencyId = string.Empty,
            Label = "ALL BANDS",
            ChannelTag = string.Empty,
            FrequencyMHz = 0f,
            Type = RadioFrequencyType.Unknown,
            ActiveFromDay = 0,
            ActiveUntilDay = -1
        };

        public static RadioTunerBand FromFrequency(RadioFrequencySO freq)
        {
            if (freq == null) return AllBands;
            string label = !string.IsNullOrEmpty(freq.displayName)
                ? freq.displayName
                : freq.id;
            string tag = freq.ResolveInterceptChannelTag();
            if (!string.IsNullOrEmpty(tag))
                label = $"{label} · {tag}";
            return new RadioTunerBand
            {
                FrequencyId = freq.id ?? string.Empty,
                Label = label,
                ChannelTag = tag ?? string.Empty,
                FrequencyMHz = freq.frequencyMHz,
                Type = freq.type,
                ActiveFromDay = freq.activeFromDay,
                ActiveUntilDay = freq.activeUntilDay
            };
        }
    }
}
