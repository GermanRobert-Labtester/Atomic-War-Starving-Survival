// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Plan 199 — Communications Arrays & Distant Contact System
// Subsystem    : Strategic Communications Infrastructure & Orbital Telemetry
// ============================================================================
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.World
{
    /// <summary>Authoritative long-range communications target definition loaded from comms_targets.json.</summary>
    [Serializable]
    public sealed class CommsTargetDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("target_type")]
        public string TargetType { get; set; } = string.Empty;

        [JsonPropertyName("min_array_tier")]
        public int MinArrayTier { get; set; } = 1;

        [JsonPropertyName("frequency_khz")]
        public int FrequencyKhz { get; set; } = 14220;

        [JsonPropertyName("band")]
        public string Band { get; set; } = "HF";

        [JsonPropertyName("required_power_watts")]
        public int RequiredPowerWatts { get; set; } = 200;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("has_satellite_window")]
        public bool HasSatelliteWindow { get; set; }

        [JsonPropertyName("is_strategic")]
        public bool IsStrategic { get; set; }

        [JsonPropertyName("revealed_faction_id")]
        public string RevealedFactionId { get; set; } = string.Empty;
    }

    /// <summary>Signal lock progress and contact status for a specific communications target.</summary>
    [Serializable]
    public sealed class CommsTargetLockState
    {
        public string TargetId { get; set; } = string.Empty;
        public int LockPermille { get; set; }
        public bool IsContactEstablished { get; set; }
        public bool InSatelliteWindow { get; set; }
        public string InterceptedData { get; set; } = string.Empty;

        public CommsTargetLockState Clone() => new CommsTargetLockState
        {
            TargetId = TargetId,
            LockPermille = LockPermille,
            IsContactEstablished = IsContactEstablished,
            InSatelliteWindow = InSatelliteWindow,
            InterceptedData = InterceptedData
        };
    }

    /// <summary>Persistent campaign state for communications array infrastructure.</summary>
    [Serializable]
    public sealed class CommsArraySaveState
    {
        public string SystemId { get; set; } = CommsArraySystem.SystemId;
        public int ArrayTier { get; set; } = 1;
        public bool IsPowered { get; set; } = true;
        public float AvailablePowerWatts { get; set; } = 1000f;
        public int CurrentFrequencyKhz { get; set; } = 14220;
        public string CurrentBand { get; set; } = "HF";
        public List<CommsTargetLockState> Locks { get; set; } = new List<CommsTargetLockState>();
        public List<string> DecodedTargetIds { get; set; } = new List<string>();
        public List<string> StrategicAuthorizationCodes { get; set; } = new List<string>();
        public int TotalScansConducted { get; set; }
    }

    /// <summary>
    /// Engine-agnostic communications array and strategic contact system.
    /// Manages signal acquisition, deterministic orbital pass windows, power grid integration,
    /// off-map contact unlocks, and endgame strategic strike authorizations.
    /// </summary>
    public sealed class CommsArraySystem
    {
        public const string SystemId = "comms_array_system";
        public const int FrequencyToleranceKhz = 50;

        private readonly Dictionary<string, CommsTargetDefinition> _targetCatalog =
            new Dictionary<string, CommsTargetDefinition>(StringComparer.Ordinal);

        private CommsArraySaveState _state = new CommsArraySaveState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        // ── Events ─────────────────────────────────────────────────────────
        public event Action<CommsTargetDefinition, CommsTargetLockState>? OnContactEstablished;
        public event Action<string, string>? OnStrategicStrikeRequested; // targetId, code
        public event Action? OnStateChanged;

        public CommsArraySaveState State => _state;
        public IReadOnlyDictionary<string, CommsTargetDefinition> TargetCatalog => _targetCatalog;

        public CommsArraySystem(ISeededRng rng, ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;

            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("targets", out var targetsEl) && targetsEl.ValueKind == JsonValueKind.Array)
            {
                foreach (var el in targetsEl.EnumerateArray())
                {
                    var target = JsonSerializer.Deserialize<CommsTargetDefinition>(el.GetRawText());
                    if (target != null && !string.IsNullOrEmpty(target.Id))
                    {
                        _targetCatalog[target.Id] = target;
                    }
                }
            }
        }

        public void SetArrayTier(int tier)
        {
            _state.ArrayTier = Math.Clamp(tier, 1, 3);
            OnStateChanged?.Invoke();
        }

        public void SetPowerState(bool isPowered, float availableWatts)
        {
            _state.IsPowered = isPowered;
            _state.AvailablePowerWatts = Math.Max(0f, availableWatts);
            OnStateChanged?.Invoke();
        }

        public void TuneFrequency(int frequencyKhz, string band)
        {
            _state.CurrentFrequencyKhz = Math.Max(1000, frequencyKhz);
            _state.CurrentBand = band ?? "HF";
            OnStateChanged?.Invoke();
        }

        public bool IsInSatelliteWindow(CommsTargetDefinition target, int currentDay, int currentHour)
        {
            if (!target.HasSatelliteWindow) return true;

            // Deterministic orbital pass calculation based on target frequency hash and game time
            int totalHours = (currentDay * 24) + Math.Clamp(currentHour, 0, 23);
            int orbitPeriod = 8; // passes overhead every 8 hours
            int phase = (target.FrequencyKhz / 100) % orbitPeriod;
            int passHour = totalHours % orbitPeriod;

            // 2-hour observation window
            return passHour == phase || passHour == (phase + 1) % orbitPeriod;
        }

        public string? TickScan(int currentDay, int currentHour, float operatorSkill01)
        {
            if (!_state.IsPowered) return null;

            _state.TotalScansConducted++;
            operatorSkill01 = Math.Clamp(operatorSkill01, 0f, 1f);

            string? establishedContactId = null;

            foreach (var kvp in _targetCatalog)
            {
                var target = kvp.Value;
                if (target.MinArrayTier > _state.ArrayTier) continue;
                if (!string.Equals(target.Band, _state.CurrentBand, StringComparison.OrdinalIgnoreCase)) continue;

                // Check frequency tolerance (+/- 50 kHz)
                if (Math.Abs(target.FrequencyKhz - _state.CurrentFrequencyKhz) > FrequencyToleranceKhz) continue;

                // Check power requirement
                if (_state.AvailablePowerWatts < target.RequiredPowerWatts) continue;

                var lockState = GetOrCreateLock(target.Id);
                if (lockState.IsContactEstablished) continue;

                bool inWindow = IsInSatelliteWindow(target, currentDay, currentHour);
                lockState.InSatelliteWindow = inWindow;

                if (!inWindow)
                {
                    // Signal degrades outside orbital pass window
                    lockState.LockPermille = Math.Max(0, lockState.LockPermille - 20);
                    continue;
                }

                // Advance signal lock
                int lockDelta = (int)(150f * (1f + operatorSkill01 * 0.75f));
                lockState.LockPermille = Math.Min(1000, lockState.LockPermille + lockDelta);

                if (lockState.LockPermille >= 1000)
                {
                    lockState.IsContactEstablished = true;
                    if (!_state.DecodedTargetIds.Contains(target.Id))
                    {
                        _state.DecodedTargetIds.Add(target.Id);
                    }

                    if (target.IsStrategic)
                    {
                        string authCode = $"AUTH-ORBITAL-{target.Id.ToUpperInvariant()}-{(currentDay * 7919) % 99999:D5}";
                        lockState.InterceptedData = authCode;
                        if (!_state.StrategicAuthorizationCodes.Contains(authCode))
                        {
                            _state.StrategicAuthorizationCodes.Add(authCode);
                        }
                    }
                    else
                    {
                        lockState.InterceptedData = $"TELEMETRY-CARRIER-LOCKED-{target.Id}";
                    }

                    OnContactEstablished?.Invoke(target, lockState);
                    establishedContactId = target.Id;
                }
            }

            OnStateChanged?.Invoke();
            return establishedContactId;
        }

        public bool RequestStrategicStrike(string targetId, string authorizationCode, out string error)
        {
            error = string.Empty;
            if (_state.ArrayTier < 3)
            {
                error = "Array tier 3 required for strategic orbital uplinks.";
                return false;
            }

            if (!_state.IsPowered || _state.AvailablePowerWatts < 1200)
            {
                error = "Insufficient grid power (1200W required).";
                return false;
            }

            if (!_targetCatalog.TryGetValue(targetId, out var target) || !target.IsStrategic)
            {
                error = "Target is not an authorized strategic uplink.";
                return false;
            }

            if (!_state.StrategicAuthorizationCodes.Contains(authorizationCode))
            {
                error = "Invalid or unintercepted authorization code.";
                return false;
            }

            _state.StrategicAuthorizationCodes.Remove(authorizationCode);
            OnStrategicStrikeRequested?.Invoke(targetId, authorizationCode);
            OnStateChanged?.Invoke();
            return true;
        }

        public CommsTargetLockState GetOrCreateLock(string targetId)
        {
            var l = _state.Locks.Find(x => x.TargetId == targetId);
            if (l == null)
            {
                l = new CommsTargetLockState { TargetId = targetId };
                _state.Locks.Add(l);
            }
            return l;
        }

        // ── Save / Restore ──────────────────────────────────────────────────

        public CommsArraySaveState CaptureState()
        {
            var copy = new CommsArraySaveState
            {
                SystemId = _state.SystemId,
                ArrayTier = _state.ArrayTier,
                IsPowered = _state.IsPowered,
                AvailablePowerWatts = _state.AvailablePowerWatts,
                CurrentFrequencyKhz = _state.CurrentFrequencyKhz,
                CurrentBand = _state.CurrentBand,
                TotalScansConducted = _state.TotalScansConducted,
                DecodedTargetIds = new List<string>(_state.DecodedTargetIds),
                StrategicAuthorizationCodes = new List<string>(_state.StrategicAuthorizationCodes),
                Locks = new List<CommsTargetLockState>(_state.Locks.Count)
            };

            foreach (var l in _state.Locks)
            {
                copy.Locks.Add(l.Clone());
            }

            return copy;
        }

        public void RestoreState(CommsArraySaveState? state)
        {
            if (state == null)
            {
                _state = new CommsArraySaveState();
                return;
            }

            _state = new CommsArraySaveState
            {
                SystemId = state.SystemId ?? SystemId,
                ArrayTier = Math.Clamp(state.ArrayTier, 1, 3),
                IsPowered = state.IsPowered,
                AvailablePowerWatts = state.AvailablePowerWatts,
                CurrentFrequencyKhz = state.CurrentFrequencyKhz > 0 ? state.CurrentFrequencyKhz : 14220,
                CurrentBand = state.CurrentBand ?? "HF",
                TotalScansConducted = state.TotalScansConducted,
                DecodedTargetIds = state.DecodedTargetIds != null ? new List<string>(state.DecodedTargetIds) : new List<string>(),
                StrategicAuthorizationCodes = state.StrategicAuthorizationCodes != null ? new List<string>(state.StrategicAuthorizationCodes) : new List<string>(),
                Locks = new List<CommsTargetLockState>()
            };

            if (state.Locks != null)
            {
                foreach (var l in state.Locks)
                {
                    if (l != null)
                    {
                        _state.Locks.Add(l.Clone());
                    }
                }
            }

            OnStateChanged?.Invoke();
        }
    }
}
