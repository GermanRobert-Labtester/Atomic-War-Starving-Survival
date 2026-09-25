// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Shelter
{
    // ── Catalog DTOs ────────────────────────────────────────────────────────

    [Serializable]
    public sealed class NoiseSourceDef
    {
        public string source_def_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string type { get; set; } = "machinery";
        public float default_output { get; set; } = 50.0f;
        public string frequency { get; set; } = "medium";
        public float duration_hours { get; set; } = 24.0f;
        public bool can_be_soundproofed { get; set; } = true;
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class NoiseSourcesCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<NoiseSourceDef> sources { get; set; } = new List<NoiseSourceDef>();
    }

    // ── Enums & State DTOs ──────────────────────────────────────────────────

    public enum NoiseSourceType
    {
        Machinery = 0,
        HumanActivity = 1,
        IndustrialProcess = 2,
        Alarm = 3,
        Ventilation = 4,
        Generator = 5,
        Construction = 6,
        MusicRecreation = 7,
        Argument = 8
    }

    public enum NoiseFrequency
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    [Serializable]
    public sealed class NoiseSource
    {
        public string SourceId { get; set; } = string.Empty;
        public NoiseSourceType Type { get; set; } = NoiseSourceType.Machinery;
        public string RoomId { get; set; } = string.Empty;
        public float NoiseOutput { get; set; } = 50f;
        public NoiseFrequency Frequency { get; set; } = NoiseFrequency.Medium;
        public float DurationHours { get; set; } = 24f;
        public bool IsActive { get; set; } = true;
        public bool CanBeSoundproofed { get; set; } = true;
    }

    [Serializable]
    public sealed class RoomAcousticProfile
    {
        public string RoomId { get; set; } = string.Empty;
        public float BaseNoiseLevel { get; set; } = 0f;
        public float WallSoundproofing { get; set; } = 0f;
        public float DoorSoundproofing { get; set; } = 0f;
        public float EffectiveNoiseLevel { get; set; } = 0f;
    }

    [Serializable]
    public sealed class NoiseEvent
    {
        public string EventId { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public int Day { get; set; } = 1;
        public string RoomId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float NoiseLevel { get; set; } = 0f;
        public float DetectionRiskAdded { get; set; } = 0f;
    }

    [Serializable]
    public sealed class ShelterNoiseState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public float OverallNoiseLevel { get; set; } = 20f;
        public float DetectionRisk { get; set; } = 5f;
        public bool QuietHoursActive { get; set; } = false;
        public int QuietHoursStart { get; set; } = 22;
        public int QuietHoursEnd { get; set; } = 6;
        public List<RoomAcousticProfile> RoomProfiles { get; set; } = new List<RoomAcousticProfile>();
        public List<NoiseSource> Sources { get; set; } = new List<NoiseSource>();
        public List<NoiseEvent> Events { get; set; } = new List<NoiseEvent>();
    }

    /// <summary>
    /// Plan 205 — Shelter Noise Discipline & Acoustic Management System.
    /// Tracks acoustic outputs of machinery, ventilation, generators, and dweller activity;
    /// models soundproofing dampening, quiet hours enforcement, and external detection risk.
    /// </summary>
    public sealed class ShelterNoiseSystem
    {
        private readonly ShelterNoiseState _state;
        private readonly Dictionary<string, NoiseSource> _sourceLookup = new Dictionary<string, NoiseSource>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, RoomAcousticProfile> _roomLookup = new Dictionary<string, RoomAcousticProfile>(StringComparer.OrdinalIgnoreCase);

        public event Action<NoiseEvent>? OnNoiseSpike;
        public event Action<float>? OnThreatDetectionRiskIncreased;
        public event Action<bool>? OnQuietHoursChanged;

        public float OverallNoiseLevel => _state.OverallNoiseLevel;

        /// <summary>Alpha feature G5 — quiet-hours tradeoff floor: shelter noise
        /// above this during the quiet window books a violation (see TickDay).</summary>
        public const float QuietHoursNoiseFloorDb = 30f;

        /// <summary>Alpha feature G5 — detection risk booked per quiet-hours
        /// violation day (see TickDay).</summary>
        public const float QuietHoursViolationRisk = 5f;

        /// <summary>Alpha feature G5 — read-only projection: would the shelter's
        /// current noise breach the quiet-hours floor during the window?</summary>
        public bool WouldViolateQuietHours(int currentHour = 23)
            => _state.QuietHoursActive
               && IsHourWithinQuietHours(currentHour)
               && _state.OverallNoiseLevel > QuietHoursNoiseFloorDb;

        /// <summary>Alpha feature G5 — count of recorded quiet-hours violations.</summary>
        public int QuietHoursViolationCount
        {
            get
            {
                int count = 0;
                foreach (var ev in _state.Events)
                    if (ev != null && string.Equals(ev.EventType, "quiet_hours_violation", StringComparison.Ordinal))
                        count++;
                return count;
            }
        }

        /// <summary>Alpha feature G5 — most recent recorded quiet-hours violation, if any.</summary>
        public NoiseEvent? LatestQuietHoursViolation
        {
            get
            {
                for (int i = _state.Events.Count - 1; i >= 0; i--)
                {
                    var ev = _state.Events[i];
                    if (ev != null && string.Equals(ev.EventType, "quiet_hours_violation", StringComparison.Ordinal))
                        return ev;
                }
                return null;
            }
        }
        public float DetectionRisk => _state.DetectionRisk;
        public bool QuietHoursActive => _state.QuietHoursActive;
        public int ActiveSourceCount => _state.Sources.Count(s => s.IsActive);
        public int QuietHoursStart => _state.QuietHoursStart;
        public int QuietHoursEnd => _state.QuietHoursEnd;
        public IReadOnlyList<NoiseSource> Sources => _state.Sources;
        public IReadOnlyList<RoomAcousticProfile> RoomProfiles => _state.RoomProfiles;
        public IReadOnlyList<NoiseEvent> Events => _state.Events;
        public ShelterNoiseState State => _state;

        private readonly Dictionary<string, NoiseSourceDef> _sourceDefs =
            new Dictionary<string, NoiseSourceDef>(StringComparer.OrdinalIgnoreCase);

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Catalog JSON cannot be null or empty", nameof(json));

            var catalog = JsonSerializer.Deserialize<NoiseSourcesCatalog>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (catalog?.sources == null) return;

            _sourceDefs.Clear();
            foreach (var src in catalog.sources)
            {
                if (src == null || string.IsNullOrWhiteSpace(src.source_def_id)) continue;
                _sourceDefs[src.source_def_id.Trim()] = src;
            }
        }

        public IReadOnlyList<NoiseSourceDef> GetAllSourceDefs() => _sourceDefs.Values.ToList();

        public NoiseSourceDef? GetSourceDef(string id)
        {
            _sourceDefs.TryGetValue(id, out var def);
            return def;
        }

        public NoiseSource? AddNoiseSourceFromDef(string sourceDefId, string roomId)
        {
            if (!_sourceDefs.TryGetValue(sourceDefId, out var def)) return null;

            var type = ParseSourceType(def.type);
            var freq = ParseFrequency(def.frequency);

            var src = AddNoiseSource(type, roomId, def.default_output, freq);
            src.DurationHours = def.duration_hours;
            src.CanBeSoundproofed = def.can_be_soundproofed;
            return src;
        }

        private static NoiseSourceType ParseSourceType(string? type) => (type ?? string.Empty).ToLowerInvariant() switch
        {
            "human_activity" => NoiseSourceType.HumanActivity,
            "industrial_process" => NoiseSourceType.IndustrialProcess,
            "alarm" => NoiseSourceType.Alarm,
            "ventilation" => NoiseSourceType.Ventilation,
            "generator" => NoiseSourceType.Generator,
            "construction" => NoiseSourceType.Construction,
            "music_recreation" => NoiseSourceType.MusicRecreation,
            "argument" => NoiseSourceType.Argument,
            _ => NoiseSourceType.Machinery
        };

        private static NoiseFrequency ParseFrequency(string? freq) => (freq ?? string.Empty).ToLowerInvariant() switch
        {
            "low" => NoiseFrequency.Low,
            "high" => NoiseFrequency.High,
            _ => NoiseFrequency.Medium
        };

        public ShelterNoiseSystem(ShelterNoiseState? state = null)
        {
            _state = CloneState(state);
            RebuildLookups();
        }

        private void RebuildLookups()
        {
            _roomLookup.Clear();
            if (_state.RoomProfiles != null)
            {
                for (int i = 0; i < _state.RoomProfiles.Count; i++)
                {
                    var r = _state.RoomProfiles[i];
                    if (r != null && !string.IsNullOrEmpty(r.RoomId))
                    {
                        _roomLookup[r.RoomId] = r;
                    }
                }
            }

            _sourceLookup.Clear();
            if (_state.Sources != null)
            {
                for (int i = 0; i < _state.Sources.Count; i++)
                {
                    var s = _state.Sources[i];
                    if (s != null && !string.IsNullOrEmpty(s.SourceId))
                    {
                        _sourceLookup[s.SourceId] = s;
                    }
                }
            }
        }

        public RoomAcousticProfile RegisterRoom(string roomId, float wallSoundproofing = 0f, float doorSoundproofing = 0f)
        {
            if (string.IsNullOrWhiteSpace(roomId)) throw new ArgumentNullException(nameof(roomId));
            string trimmedId = roomId.Trim();

            if (_roomLookup.TryGetValue(trimmedId, out var existing))
            {
                return existing;
            }

            var profile = _state.RoomProfiles.FirstOrDefault(r => string.Equals(r.RoomId, trimmedId, StringComparison.OrdinalIgnoreCase));
            if (profile == null)
            {
                profile = new RoomAcousticProfile
                {
                    RoomId = trimmedId,
                    WallSoundproofing = SanitizeRange(wallSoundproofing, 0f, 100f),
                    DoorSoundproofing = SanitizeRange(doorSoundproofing, 0f, 100f),
                    BaseNoiseLevel = 0f,
                    EffectiveNoiseLevel = 0f
                };
                _state.RoomProfiles.Add(profile);
            }
            _roomLookup[trimmedId] = profile;
            return profile;
        }

        public NoiseSource AddNoiseSource(
            NoiseSourceType type,
            string roomId,
            float output = 50f,
            NoiseFrequency freq = NoiseFrequency.Medium,
            float durationHours = 24f,
            bool canSoundproof = true)
        {
            if (string.IsNullOrWhiteSpace(roomId))
                throw new ArgumentNullException(nameof(roomId));
            if (!Enum.IsDefined(typeof(NoiseSourceType), type))
                throw new ArgumentOutOfRangeException(nameof(type));
            if (!Enum.IsDefined(typeof(NoiseFrequency), freq))
                throw new ArgumentOutOfRangeException(nameof(freq));
            if (_state.NextSequence == int.MaxValue)
                throw new InvalidOperationException("Shelter noise source sequence exhausted.");

            string canonicalRoomId = roomId.Trim();
            RegisterRoom(canonicalRoomId);
            var source = new NoiseSource
            {
                SourceId = $"ns_{_state.NextSequence++}",
                Type = type,
                RoomId = canonicalRoomId,
                NoiseOutput = SanitizeRange(output, 0f, 100f),
                Frequency = freq,
                DurationHours = SanitizeRange(durationHours, 0.5f, 24f),
                IsActive = true,
                CanBeSoundproofed = canSoundproof
            };

            _state.Sources.Add(source);
            _sourceLookup[source.SourceId] = source;
            return source;
        }

        public NoiseSource? GetNoiseSource(string sourceId)
        {
            if (string.IsNullOrWhiteSpace(sourceId)) return null;
            if (_sourceLookup.TryGetValue(sourceId, out var s)) return s;
            var fallback = _state.Sources.FirstOrDefault(src => string.Equals(src.SourceId, sourceId, StringComparison.OrdinalIgnoreCase));
            if (fallback != null) _sourceLookup[sourceId] = fallback;
            return fallback;
        }

        public bool RemoveNoiseSource(string sourceId)
        {
            var source = GetNoiseSource(sourceId);
            if (source == null) return false;

            _state.Sources.Remove(source);
            _sourceLookup.Remove(source.SourceId);
            return true;
        }

        public bool SetSourceActive(string sourceId, bool isActive)
        {
            var source = GetNoiseSource(sourceId);
            if (source == null) return false;

            source.IsActive = isActive;
            return true;
        }

        public bool SoundproofRoom(string roomId, float wallAdd, float doorAdd)
        {
            var profile = RegisterRoom(roomId);
            profile.WallSoundproofing = SanitizeRange(profile.WallSoundproofing + wallAdd, 0f, 100f);
            profile.DoorSoundproofing = SanitizeRange(profile.DoorSoundproofing + doorAdd, 0f, 100f);
            return true;
        }

        public void SetQuietHours(bool enabled, int startHour = 22, int endHour = 6)
        {
            _state.QuietHoursActive = enabled;
            _state.QuietHoursStart = Math.Clamp(startHour, 0, 23);
            _state.QuietHoursEnd = Math.Clamp(endHour, 0, 23);
            OnQuietHoursChanged?.Invoke(enabled);
        }

        public bool IsHourWithinQuietHours(int hour)
        {
            if (!_state.QuietHoursActive || hour < 0 || hour > 23) return false;
            if (_state.QuietHoursStart > _state.QuietHoursEnd)
            {
                // Over midnight: e.g. 22 to 6
                return hour >= _state.QuietHoursStart || hour < _state.QuietHoursEnd;
            }
            return hour >= _state.QuietHoursStart && hour < _state.QuietHoursEnd;
        }

        public void TickDay(int currentDay, int currentHour = 12)
        {
            bool inQuietPeriod = IsHourWithinQuietHours(currentHour);
            int safeDay = Math.Max(0, currentDay);

            // 1. Calculate Room Noise Levels
            float maxRoomNoise = 0f;
            float totalRoomNoise = 0f;

            foreach (var profile in _state.RoomProfiles)
            {
                if (profile == null || string.IsNullOrWhiteSpace(profile.RoomId)) continue;
                float wall = SanitizeRange(profile.WallSoundproofing, 0f, 100f);
                float door = SanitizeRange(profile.DoorSoundproofing, 0f, 100f);
                profile.WallSoundproofing = wall;
                profile.DoorSoundproofing = door;

                var roomSources = _state.Sources
                    .Where(s => s != null
                        && s.IsActive
                        && string.Equals(s.RoomId, profile.RoomId, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                float rawNoise = 0f;
                foreach (var src in roomSources)
                {
                    float sourceContrib = SanitizeRange(src.NoiseOutput, 0f, 100f)
                        * (SanitizeRange(src.DurationHours, 0.5f, 24f) / 24f);

                    // Low frequency penetrates soundproofing more easily
                    float dampeningEffect = src.CanBeSoundproofed
                        ? (wall * 0.5f + door * 0.5f) / 100f
                        : 0f;

                    if (src.Frequency == NoiseFrequency.Low)
                    {
                        dampeningEffect *= 0.5f; // Bass/rumble penetrates
                    }
                    else if (src.Frequency == NoiseFrequency.High)
                    {
                        dampeningEffect *= 1.2f; // High frequency easily blocked
                    }

                    dampeningEffect = SanitizeRange(dampeningEffect, 0f, 0.85f);
                    rawNoise += sourceContrib * (1f - dampeningEffect);
                }

                profile.EffectiveNoiseLevel = SanitizeRange(rawNoise, 0f, 100f);
                maxRoomNoise = Math.Max(maxRoomNoise, profile.EffectiveNoiseLevel);
                totalRoomNoise += profile.EffectiveNoiseLevel;
            }

            // 2. Shelter overall noise: 60% max room peak + 40% average
            float avgNoise = _state.RoomProfiles.Count > 0 ? (totalRoomNoise / _state.RoomProfiles.Count) : 0f;
            _state.OverallNoiseLevel = SanitizeRange(
                (maxRoomNoise * 0.6f) + (avgNoise * 0.4f),
                0f,
                100f);

            // 3. Detection Risk
            float dailyRiskIncrease = 0f;
            if (_state.OverallNoiseLevel > 50f)
                dailyRiskIncrease = (_state.OverallNoiseLevel - 50f) * 0.3f;

            if (inQuietPeriod && _state.OverallNoiseLevel > 30f)
            {
                if (_state.NextSequence == int.MaxValue)
                    throw new InvalidOperationException("Shelter noise event sequence exhausted.");
                dailyRiskIncrease += 5f;
                var ev = new NoiseEvent
                {
                    EventId = $"nev_{_state.NextSequence++}",
                    EventType = "quiet_hours_violation",
                    Day = safeDay,
                    RoomId = "shelter",
                    Description = "Loud activity detected during designated quiet hours!",
                    NoiseLevel = _state.OverallNoiseLevel,
                    DetectionRiskAdded = 5f
                };
                _state.Events.Add(ev);
                OnNoiseSpike?.Invoke(ev);
            }

            _state.DetectionRisk = SanitizeRange(
                _state.DetectionRisk + dailyRiskIncrease - 1f,
                0f,
                100f);

            if (dailyRiskIncrease > 0f)
                OnThreatDetectionRiskIncreased?.Invoke(_state.DetectionRisk);
        }

        public float GetRoomNoise(string roomId)
        {
            if (string.IsNullOrWhiteSpace(roomId)) return 0f;
            var profile = _state.RoomProfiles.FirstOrDefault(r =>
                r != null && string.Equals(r.RoomId, roomId.Trim(), StringComparison.OrdinalIgnoreCase));
            return SanitizeRange(profile?.EffectiveNoiseLevel ?? 0f, 0f, 100f);
        }

        public void AttenuateDetectionRisk(float amount)
        {
            _state.DetectionRisk = SanitizeRange(_state.DetectionRisk - amount, 0f, 100f);
        }

        public ShelterNoiseState CaptureState() => CloneState(_state);

        public void RestoreState(ShelterNoiseState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            var restored = CloneState(state);
            _state.SchemaVersion = restored.SchemaVersion;
            _state.NextSequence = restored.NextSequence;
            _state.OverallNoiseLevel = restored.OverallNoiseLevel;
            _state.DetectionRisk = restored.DetectionRisk;
            _state.QuietHoursActive = restored.QuietHoursActive;
            _state.QuietHoursStart = restored.QuietHoursStart;
            _state.QuietHoursEnd = restored.QuietHoursEnd;
            _state.RoomProfiles = restored.RoomProfiles;
            _state.Sources = restored.Sources;
            _state.Events = restored.Events;
            RebuildLookups();
        }

        private static ShelterNoiseState CloneState(ShelterNoiseState? source)
        {
            var copy = new ShelterNoiseState
            {
                SchemaVersion = source?.SchemaVersion ?? 1,
                NextSequence = source?.NextSequence ?? 1,
                OverallNoiseLevel = SanitizeRange(source?.OverallNoiseLevel ?? 20f, 0f, 100f),
                DetectionRisk = SanitizeRange(source?.DetectionRisk ?? 5f, 0f, 100f),
                QuietHoursActive = source?.QuietHoursActive ?? false,
                QuietHoursStart = Math.Clamp(source?.QuietHoursStart ?? 22, 0, 23),
                QuietHoursEnd = Math.Clamp(source?.QuietHoursEnd ?? 6, 0, 23),
                RoomProfiles = new List<RoomAcousticProfile>(),
                Sources = new List<NoiseSource>(),
                Events = new List<NoiseEvent>()
            };
            if (source == null) return copy;

            int maxSequence = 0;
            var roomIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (source.RoomProfiles != null)
            {
                foreach (var profile in source.RoomProfiles)
                {
                    if (profile == null || string.IsNullOrWhiteSpace(profile.RoomId)
                        || !roomIds.Add(profile.RoomId.Trim())) continue;
                    copy.RoomProfiles.Add(new RoomAcousticProfile
                    {
                        RoomId = profile.RoomId.Trim(),
                        BaseNoiseLevel = SanitizeRange(profile.BaseNoiseLevel, 0f, 100f),
                        WallSoundproofing = SanitizeRange(profile.WallSoundproofing, 0f, 100f),
                        DoorSoundproofing = SanitizeRange(profile.DoorSoundproofing, 0f, 100f),
                        EffectiveNoiseLevel = SanitizeRange(profile.EffectiveNoiseLevel, 0f, 100f)
                    });
                }
            }

            var sourceIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (source.Sources != null)
            {
                foreach (var noiseSource in source.Sources)
                {
                    if (noiseSource == null || string.IsNullOrWhiteSpace(noiseSource.SourceId)
                        || string.IsNullOrWhiteSpace(noiseSource.RoomId)
                        || !sourceIds.Add(noiseSource.SourceId.Trim())) continue;
                    copy.Sources.Add(new NoiseSource
                    {
                        SourceId = noiseSource.SourceId.Trim(),
                        Type = Enum.IsDefined(typeof(NoiseSourceType), noiseSource.Type)
                            ? noiseSource.Type
                            : NoiseSourceType.Machinery,
                        RoomId = noiseSource.RoomId.Trim(),
                        NoiseOutput = SanitizeRange(noiseSource.NoiseOutput, 0f, 100f),
                        Frequency = Enum.IsDefined(typeof(NoiseFrequency), noiseSource.Frequency)
                            ? noiseSource.Frequency
                            : NoiseFrequency.Medium,
                        DurationHours = SanitizeRange(noiseSource.DurationHours, 0.5f, 24f),
                        IsActive = noiseSource.IsActive,
                        CanBeSoundproofed = noiseSource.CanBeSoundproofed
                    });
                    if (TryParseSequence(noiseSource.SourceId, "ns_", out int sequence))
                        maxSequence = Math.Max(maxSequence, sequence);
                }
            }

            var eventIds = new HashSet<string>(StringComparer.Ordinal);
            if (source.Events != null)
            {
                foreach (var noiseEvent in source.Events)
                {
                    if (noiseEvent == null || string.IsNullOrWhiteSpace(noiseEvent.EventId)
                        || !eventIds.Add(noiseEvent.EventId.Trim())) continue;
                    copy.Events.Add(new NoiseEvent
                    {
                        EventId = noiseEvent.EventId.Trim(),
                        EventType = noiseEvent.EventType ?? string.Empty,
                        Day = Math.Max(0, noiseEvent.Day),
                        RoomId = noiseEvent.RoomId ?? string.Empty,
                        Description = noiseEvent.Description ?? string.Empty,
                        NoiseLevel = SanitizeRange(noiseEvent.NoiseLevel, 0f, 100f),
                        DetectionRiskAdded = SanitizeFinite(noiseEvent.DetectionRiskAdded)
                    });
                    if (TryParseSequence(noiseEvent.EventId, "nev_", out int sequence))
                        maxSequence = Math.Max(maxSequence, sequence);
                }
            }

            if (copy.NextSequence < 1) copy.NextSequence = 1;
            long requiredSequence = (long)maxSequence + 1;
            if (requiredSequence > copy.NextSequence && requiredSequence <= int.MaxValue)
                copy.NextSequence = (int)requiredSequence;
            return copy;
        }

        private static bool TryParseSequence(string value, string prefix, out int sequence)
        {
            sequence = 0;
            return !string.IsNullOrEmpty(value)
                && value.StartsWith(prefix, StringComparison.Ordinal)
                && value.Length > prefix.Length
                && int.TryParse(value.Substring(prefix.Length), out sequence);
        }

        private static float SanitizeFinite(float value) =>
            float.IsNaN(value) || float.IsInfinity(value) ? 0f : value;

        private static float SanitizeRange(float value, float min, float max)
        {
            if (float.IsNaN(value) || float.IsInfinity(value)) return min;
            return Math.Clamp(value, min, max);
        }
    }
}
