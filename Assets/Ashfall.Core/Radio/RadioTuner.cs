// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// ASHFALL Radio Tuning model (item 7).
    ///
    /// Continuous tuning + signal-lock thresholds + VU strength. The Core
    /// owns the deterministic mapping from (tuned_frequency, broadcast,
    /// rng, day) to a SignalLockResult. The host drives the rotary/slider
    /// tuner and renders the live signal display.
    /// </summary>
    public sealed class RadioTuner
    {
        private readonly RadioTunerState _state;

        public event Action<RadioSignalEvent>? OnSignalChanged;

        public RadioTuner(RadioTunerState state)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
        }

        public RadioTunerState State => _state;

        public float TunedFrequencyKHz => _state.TunedFrequencyKHz;

        public bool IsTunedTo(float frequencyKHz)
        {
            return Math.Abs(_state.TunedFrequencyKHz - frequencyKHz) < 0.5f;
        }

        public void TuneTo(float frequencyKHz)
        {
            if (frequencyKHz < 0) frequencyKHz = 0;
            _state.TunedFrequencyKHz = frequencyKHz;
        }

        public void TuneBy(float deltaKHz)
        {
            TuneTo(_state.TunedFrequencyKHz + deltaKHz);
        }

        /// <summary>
        /// Evaluate signal lock against a list of broadcasts. Returns the
        /// strongest lockable broadcast, or empty when none qualifies.
        /// </summary>
        public SignalLockResult Evaluate(IEnumerable<RadioBroadcast> broadcasts,
            float staticNoiseFloor, ISeededRng rng)
        {
            if (broadcasts == null) return SignalLockResult.NoSignal;
            const float toleranceKHz = 5.0f;
            RadioBroadcast? best = null;
            float bestStrength = 0f;
            foreach (var b in broadcasts)
            {
                if (b == null) continue;
                float offset = Math.Abs(_state.TunedFrequencyKHz - b.FrequencyKHz);
                if (offset > toleranceKHz) continue;
                float strength = b.SignalStrength * Math.Max(0f, 1f - offset / toleranceKHz);
                if (strength > bestStrength)
                {
                    bestStrength = strength;
                    best = b;
                }
            }
            if (best == null) return SignalLockResult.NoSignal;
            var locked = best;
            float vu = bestStrength * (1f - staticNoiseFloor);
            OnSignalChanged?.Invoke(new RadioSignalEvent
            {
                TunedFrequencyKHz = _state.TunedFrequencyKHz,
                IsLocked = false,
                VuStrength = vu
            });

            if (vu < 0.05f) return SignalLockResult.NoSignal;
            if (vu < locked.LockThreshold)
            {
                return new SignalLockResult
                {
                    IsLocked = false,
                    Broadcast = locked,
                    VuStrength = vu,
                    Noise = staticNoiseFloor,
                    DecodedContent = string.Empty
                };
            }
            // Locked: pick which transcript line to surface based on rng.
            var lines = locked.TranscriptLines ?? new List<string>();
            if (lines.Count == 0)
            {
                OnSignalChanged?.Invoke(new RadioSignalEvent
                {
                    TunedFrequencyKHz = _state.TunedFrequencyKHz,
                    IsLocked = true,
                    VuStrength = vu
                });
                return new SignalLockResult { IsLocked = true, Broadcast = locked, VuStrength = vu,
                    Noise = staticNoiseFloor, DecodedContent = locked.Headline ?? string.Empty };
            }
            int idx = (int)(rng.NextDouble() * lines.Count);
            if (idx >= lines.Count) idx = lines.Count - 1;
            if (idx < 0) idx = 0;
            OnSignalChanged?.Invoke(new RadioSignalEvent
            {
                TunedFrequencyKHz = _state.TunedFrequencyKHz,
                IsLocked = true,
                VuStrength = vu
            });
            return new SignalLockResult
            {
                IsLocked = true,
                Broadcast = locked,
                VuStrength = vu,
                Noise = staticNoiseFloor,
                DecodedContent = lines[idx]
            };
        }

        /// <summary>
        /// Evaluate continuous distress signal lock against the authoritative distress system.
        /// </summary>
        public DistressFrequencyTuneResult EvaluateFrequency(
            float frequencyMhz,
            RadioDistressSystem distress,
            float staticNoiseFloor,
            ISeededRng rng,
            int day)
        {
            if (distress == null)
            {
                return new DistressFrequencyTuneResult
                {
                    IsLocked = false,
                    Noise = staticNoiseFloor
                };
            }

            TuneTo(frequencyMhz);

            var signal = distress.FindSignalAtFrequency(frequencyMhz, 0.5f);
            if (signal == null)
            {
                return new DistressFrequencyTuneResult
                {
                    IsLocked = false,
                    Noise = staticNoiseFloor
                };
            }

            float offset = Math.Abs(frequencyMhz - signal.FrequencyMhz);
            float strength = Math.Max(0f, 1f - (offset / 0.5f));
            float vu = strength * (1f - staticNoiseFloor);

            string decoded = signal.SourceName;
            float clarity = 0.35f;

            if (signal.MessageFragments != null && signal.MessageFragments.Count > 0)
            {
                // Tasks 9–12 Wave 1: single shared stage resolver (was a
                // duplicated inline loop; contract documented on the resolver).
                // The ?? fallback is unreachable under the Count guard and only
                // satisfies nullable analysis; it mirrors the legacy fallback.
                var stage = DistressStageResolver.Resolve(signal, day)
                            ?? new DistressStageView(0, signal.MessageFragments[0]);
                decoded = stage.Fragment.Text;
                clarity = stage.Fragment.Clarity;
            }

            bool isLocked = vu >= 0.25f;

            return new DistressFrequencyTuneResult
            {
                IsLocked = isLocked,
                Signal = signal,
                IsGenuineRescue = signal.IsGenuineRescue,
                IsDeceptive = signal.IsTrapOrDeception,
                VuStrength = vu,
                Noise = staticNoiseFloor,
                DecodedContent = decoded,
                Clarity = clarity
            };
        }
    }

    [Serializable]
    public sealed class DistressFrequencyTuneResult
    {
        public bool IsLocked;
        public DistressSignalDefinition? Signal;
        public bool IsGenuineRescue;
        public bool IsDeceptive;
        public float VuStrength;
        public float Noise;
        public string DecodedContent = string.Empty;
        public float Clarity;
    }

    [Serializable]
    public sealed class RadioBroadcast
    {
        public string BroadcastId;
        public float FrequencyKHz;
        public float SignalStrength;
        public float LockThreshold;
        public string Headline;
        public List<string> TranscriptLines;
    }

    [Serializable]
    public sealed class RadioTunerState
    {
        public float TunedFrequencyKHz;
    }

    [Serializable]
    public sealed class SignalLockResult
    {
        public bool IsLocked;
        public RadioBroadcast Broadcast;
        public float VuStrength;
        public float Noise;
        public string DecodedContent;

        public static SignalLockResult NoSignal => new SignalLockResult
        {
            IsLocked = false,
            Broadcast = null!,
            VuStrength = 0f,
            Noise = 1f,
            DecodedContent = string.Empty
        };
    }

    [Serializable]
    public sealed class RadioSignalEvent
    {
        public float TunedFrequencyKHz;
        public bool IsLocked;
        public float VuStrength;
    }

    internal static class DoubleClampExtensions
    {
        public static double ClampTo(this double v, double min, double max)
        {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }
    }
}
