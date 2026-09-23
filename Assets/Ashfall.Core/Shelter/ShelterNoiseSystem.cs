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
                if (!string.IsNullOrEmpty(src.source_def_id))
                {
                    _sourceDefs[src.source_def_id] = src;
                }
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

        private static NoiseSourceType ParseSourceType(string type) => type.ToLowerInvariant() switch
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

        private static NoiseFrequency ParseFrequency(string freq) => freq.ToLowerInvariant() switch
        {
            "low" => NoiseFrequency.Low,
            "high" => NoiseFrequency.High,
            _ => NoiseFrequency.Medium
        };

        public ShelterNoiseSystem(ShelterNoiseState? state = null)
        {
            _state = state ?? new ShelterNoiseState();
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
                    WallSoundproofing = Math.Clamp(wallSoundproofing, 0f, 100f),
                    DoorSoundproofing = Math.Clamp(doorSoundproofing, 0f, 100f),
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
            var source = new NoiseSource
            {
                SourceId = $"ns_{_state.NextSequence++}",
                Type = type,
                RoomId = roomId ?? string.Empty,
                NoiseOutput = Math.Clamp(output, 0f, 100f),
                Frequency = freq,
                DurationHours = Math.Clamp(durationHours, 0.5f, 24f),
                IsActive = true,
                CanBeSoundproofed = canSoundproof
            };

            _state.Sources.Add(source);
            _sourceLookup[source.SourceId] = source;
            RegisterRoom(roomId);
            return source;
        }

        public NoiseSource? GetNoiseSource(string sourceId)
        {
            if (string.IsNullOrEmpty(sourceId)) return null;
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
            profile.WallSoundproofing = Math.Clamp(profile.WallSoundproofing + wallAdd, 0f, 100f);
            profile.DoorSoundproofing = Math.Clamp(profile.DoorSoundproofing + doorAdd, 0f, 100f);
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
            if (!_state.QuietHoursActive) return false;
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

            // 1. Calculate Room Noise Levels
            float maxRoomNoise = 0f;
            float totalRoomNoise = 0f;

            foreach (var profile in _state.RoomProfiles)
            {
                var roomSources = _state.Sources
                    .Where(s => s.IsActive && string.Equals(s.RoomId, profile.RoomId, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                float rawNoise = 0f;
                foreach (var src in roomSources)
                {
                    float sourceContrib = src.NoiseOutput * (src.DurationHours / 24f);

                    // Low frequency penetrates soundproofing more easily
                    float dampeningEffect = src.CanBeSoundproofed
                        ? (profile.WallSoundproofing * 0.5f + profile.DoorSoundproofing * 0.5f) / 100f
                        : 0f;

                    if (src.Frequency == NoiseFrequency.Low)
                    {
                        dampeningEffect *= 0.5f; // Bass/rumble penetrates
                    }
                    else if (src.Frequency == NoiseFrequency.High)
                    {
                        dampeningEffect *= 1.2f; // High frequency easily blocked
                    }

                    dampeningEffect = Math.Clamp(dampeningEffect, 0f, 0.85f);
                    rawNoise += (sourceContrib * (1.0f - dampeningEffect));
                }

                profile.EffectiveNoiseLevel = Math.Clamp(rawNoise, 0f, 100f);
                if (profile.EffectiveNoiseLevel > maxRoomNoise)
                {
                    maxRoomNoise = profile.EffectiveNoiseLevel;
                }
                totalRoomNoise += profile.EffectiveNoiseLevel;
            }

            // 2. Shelter overall noise: 60% max room peak + 40% average
            float avgNoise = _state.RoomProfiles.Count > 0 ? (totalRoomNoise / _state.RoomProfiles.Count) : 0f;
            _state.OverallNoiseLevel = Math.Clamp((maxRoomNoise * 0.6f) + (avgNoise * 0.4f), 0f, 100f);

            // 3. Detection Risk
            float dailyRiskIncrease = 0f;
            if (_state.OverallNoiseLevel > 50f)
            {
                dailyRiskIncrease = (_state.OverallNoiseLevel - 50f) * 0.3f;
            }

            if (inQuietPeriod && _state.OverallNoiseLevel > 30f)
            {
                // Violation during quiet hours
                dailyRiskIncrease += 5.0f;
                var ev = new NoiseEvent
                {
                    EventId = $"nev_{_state.NextSequence++}",
                    EventType = "quiet_hours_violation",
                    Day = currentDay,
                    RoomId = "shelter",
                    Description = "Loud activity detected during designated quiet hours!",
                    NoiseLevel = _state.OverallNoiseLevel,
                    DetectionRiskAdded = 5.0f
                };
                _state.Events.Add(ev);
                OnNoiseSpike?.Invoke(ev);
            }

            _state.DetectionRisk = Math.Clamp(_state.DetectionRisk + dailyRiskIncrease - 1.0f, 0f, 100f);

            if (dailyRiskIncrease > 0f)
            {
                OnThreatDetectionRiskIncreased?.Invoke(_state.DetectionRisk);
            }
        }

        public float GetRoomNoise(string roomId)
        {
            var profile = _state.RoomProfiles.FirstOrDefault(r => string.Equals(r.RoomId, roomId, StringComparison.OrdinalIgnoreCase));
            return profile?.EffectiveNoiseLevel ?? 0f;
        }

        public void AttenuateDetectionRisk(float amount)
        {
            _state.DetectionRisk = Math.Clamp(_state.DetectionRisk - amount, 0f, 100f);
        }

        public ShelterNoiseState CaptureState()
        {
            var state = new ShelterNoiseState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                OverallNoiseLevel = _state.OverallNoiseLevel,
                DetectionRisk = _state.DetectionRisk,
                QuietHoursActive = _state.QuietHoursActive,
                QuietHoursStart = _state.QuietHoursStart,
                QuietHoursEnd = _state.QuietHoursEnd,
                RoomProfiles = new List<RoomAcousticProfile>(_state.RoomProfiles.Count),
                Sources = new List<NoiseSource>(_state.Sources.Count),
                Events = new List<NoiseEvent>(_state.Events.Count)
            };

            foreach (var r in _state.RoomProfiles)
            {
                state.RoomProfiles.Add(new RoomAcousticProfile
                {
                    RoomId = r.RoomId,
                    BaseNoiseLevel = r.BaseNoiseLevel,
                    WallSoundproofing = r.WallSoundproofing,
                    DoorSoundproofing = r.DoorSoundproofing,
                    EffectiveNoiseLevel = r.EffectiveNoiseLevel
                });
            }

            foreach (var s in _state.Sources)
            {
                state.Sources.Add(new NoiseSource
                {
                    SourceId = s.SourceId,
                    Type = s.Type,
                    RoomId = s.RoomId,
                    NoiseOutput = s.NoiseOutput,
                    Frequency = s.Frequency,
                    DurationHours = s.DurationHours,
                    IsActive = s.IsActive,
                    CanBeSoundproofed = s.CanBeSoundproofed
                });
            }

            foreach (var e in _state.Events)
            {
                state.Events.Add(new NoiseEvent
                {
                    EventId = e.EventId,
                    EventType = e.EventType,
                    Day = e.Day,
                    RoomId = e.RoomId,
                    Description = e.Description,
                    NoiseLevel = e.NoiseLevel,
                    DetectionRiskAdded = e.DetectionRiskAdded
                });
            }

            return state;
        }

        public void RestoreState(ShelterNoiseState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.OverallNoiseLevel = state.OverallNoiseLevel;
            _state.DetectionRisk = state.DetectionRisk;
            _state.QuietHoursActive = state.QuietHoursActive;
            _state.QuietHoursStart = state.QuietHoursStart;
            _state.QuietHoursEnd = state.QuietHoursEnd;
            _state.RoomProfiles.Clear();
            _state.Sources.Clear();
            _state.Events.Clear();

            if (state.RoomProfiles != null)
            {
                foreach (var r in state.RoomProfiles)
                {
                    _state.RoomProfiles.Add(new RoomAcousticProfile
                    {
                        RoomId = r.RoomId,
                        BaseNoiseLevel = r.BaseNoiseLevel,
                        WallSoundproofing = r.WallSoundproofing,
                        DoorSoundproofing = r.DoorSoundproofing,
                        EffectiveNoiseLevel = r.EffectiveNoiseLevel
                    });
                }
            }

            if (state.Sources != null)
            {
                foreach (var s in state.Sources)
                {
                    _state.Sources.Add(new NoiseSource
                    {
                        SourceId = s.SourceId,
                        Type = s.Type,
                        RoomId = s.RoomId,
                        NoiseOutput = s.NoiseOutput,
                        Frequency = s.Frequency,
                        DurationHours = s.DurationHours,
                        IsActive = s.IsActive,
                        CanBeSoundproofed = s.CanBeSoundproofed
                    });
                }
            }

            if (state.Events != null)
            {
                foreach (var e in state.Events)
                {
                    _state.Events.Add(new NoiseEvent
                    {
                        EventId = e.EventId,
                        EventType = e.EventType,
                        Day = e.Day,
                        RoomId = e.RoomId,
                        Description = e.Description,
                        NoiseLevel = e.NoiseLevel,
                        DetectionRiskAdded = e.DetectionRiskAdded
                    });
                }
            }

            RebuildLookups();
        }
    }
}
