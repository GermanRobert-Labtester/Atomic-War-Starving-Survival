// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Voice
{
    [Serializable]
    public sealed class VoiceLineDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("speaker")]
        public string Speaker { get; set; } = "any";

        [JsonPropertyName("trigger")]
        public string Trigger { get; set; } = string.Empty;

        [JsonPropertyName("register")]
        public string Register { get; set; } = "clipped";

        [JsonPropertyName("text_key")]
        public string TextKey { get; set; } = string.Empty;

        [JsonPropertyName("text_english")]
        public string TextEnglish { get; set; } = string.Empty;

        [JsonPropertyName("min_morale")]
        public float MinMorale { get; set; } = 0f;

        [JsonPropertyName("max_morale")]
        public float MaxMorale { get; set; } = 100f;

        [JsonPropertyName("min_fatigue")]
        public float MinFatigue { get; set; } = 0f;

        [JsonPropertyName("max_fatigue")]
        public float MaxFatigue { get; set; } = 100f;

        [JsonPropertyName("weight")]
        public float Weight { get; set; } = 1.0f;

        [JsonPropertyName("cooldown_days")]
        public int CooldownDays { get; set; } = 2;
    }

    public sealed class VoiceLineCatalogData
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("lines")]
        public List<VoiceLineDefinition> Lines { get; set; } = new List<VoiceLineDefinition>();
    }

    [Serializable]
    public sealed class VoiceLinePayload
    {
        public string LineId { get; set; } = string.Empty;
        public string SpeakerSurvivorId { get; set; } = string.Empty;
        public string Profession { get; set; } = string.Empty;
        public string Trigger { get; set; } = string.Empty;
        public string TextKey { get; set; } = string.Empty;
        public string TextEnglish { get; set; } = string.Empty;
        public string Register { get; set; } = string.Empty;
        public int UtteredDay { get; set; } = 1;
    }

    public sealed class SurvivorSpeechContext
    {
        public string SurvivorId { get; set; } = string.Empty;
        public string Profession { get; set; } = string.Empty;
        public float Morale { get; set; } = 50f;
        public float Fatigue { get; set; } = 0f;
        public string PreferredRegister { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class SurvivorVoiceState
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("line_last_uttered_day")]
        public Dictionary<string, int> LineLastUtteredDay { get; set; } = new Dictionary<string, int>();

        [JsonPropertyName("survivor_last_uttered_day")]
        public Dictionary<string, int> SurvivorLastUtteredDay { get; set; } = new Dictionary<string, int>();

        [JsonPropertyName("history")]
        public List<VoiceLinePayload> History { get; set; } = new List<VoiceLinePayload>();
    }

    /// <summary>
    /// Plan 42 / C2[18] — A Voice for Each of Them: Survivors Who Say Things.
    /// Pure Core engine-free deterministic voice line selection engine keyed to
    /// survivor identity (profession, register), state (morale, fatigue), and event triggers.
    /// </summary>
    public sealed class SurvivorVoiceSystem
    {
        private readonly SurvivorVoiceState _state;
        private readonly List<VoiceLineDefinition> _catalog = new List<VoiceLineDefinition>();

        public SurvivorVoiceState State => _state;
        public IReadOnlyList<VoiceLineDefinition> Catalog => _catalog;
        public IReadOnlyList<VoiceLinePayload> History => _state.History;

        public Action<VoiceLinePayload>? VoiceLineDeliveredSink { get; set; }
        public Action<string /*survivorId*/, string /*lineId*/>? VoiceBarkAudioSeam { get; set; }

        public SurvivorVoiceSystem(SurvivorVoiceState? state = null)
        {
            _state = state ?? new SurvivorVoiceState();
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            var data = JsonSerializer.Deserialize<VoiceLineCatalogData>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (data != null)
            {
                LoadCatalog(data);
            }
        }

        public void LoadCatalog(VoiceLineCatalogData data)
        {
            if (data?.Lines == null) return;
            _catalog.Clear();
            _catalog.AddRange(data.Lines);
        }

        public void RegisterLine(VoiceLineDefinition line)
        {
            if (line == null || string.IsNullOrWhiteSpace(line.Id)) return;
            _catalog.RemoveAll(l => string.Equals(l.Id, line.Id, StringComparison.OrdinalIgnoreCase));
            _catalog.Add(line);
        }

        public bool TrySelectVoiceLine(
            SurvivorSpeechContext context,
            string trigger,
            int currentDay,
            ISeededRng? rng,
            out VoiceLinePayload payload)
        {
            payload = new VoiceLinePayload();
            if (context == null || string.IsNullOrWhiteSpace(context.SurvivorId) || string.IsNullOrWhiteSpace(trigger))
            {
                return false;
            }

            // Check survivor-level speech cooldown (at most one line per survivor per day)
            if (_state.SurvivorLastUtteredDay.TryGetValue(context.SurvivorId, out int lastDaySpoken) &&
                currentDay <= lastDaySpoken)
            {
                return false;
            }

            var candidates = new List<(VoiceLineDefinition Line, float AdjustedWeight)>();

            foreach (var line in _catalog)
            {
                // Match Trigger
                if (!string.Equals(line.Trigger, trigger, StringComparison.OrdinalIgnoreCase))
                    continue;

                // Match Speaker (profession or wildcard 'any')
                if (!string.Equals(line.Speaker, "any", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(line.Speaker, context.Profession, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Match Conditions
                if (context.Morale < line.MinMorale || context.Morale > line.MaxMorale)
                    continue;
                if (context.Fatigue < line.MinFatigue || context.Fatigue > line.MaxFatigue)
                    continue;

                // Check Line Cooldown
                if (_state.LineLastUtteredDay.TryGetValue(line.Id, out int lastLineDay) &&
                    currentDay - lastLineDay < line.CooldownDays)
                {
                    continue;
                }

                float weight = Math.Max(0.1f, line.Weight);
                if (!string.IsNullOrWhiteSpace(context.PreferredRegister) &&
                    string.Equals(context.PreferredRegister, line.Register, StringComparison.OrdinalIgnoreCase))
                {
                    weight *= 2.0f;
                }

                candidates.Add((line, weight));
            }

            if (candidates.Count == 0)
            {
                return false;
            }

            // Seeded deterministic pick
            VoiceLineDefinition selected;
            if (rng != null && candidates.Count > 1)
            {
                float totalWeight = candidates.Sum(c => c.AdjustedWeight);
                float roll = (float)(rng.NextDouble() * totalWeight);
                float running = 0f;
                selected = candidates[0].Line;

                foreach (var (candLine, weight) in candidates)
                {
                    running += weight;
                    if (roll <= running)
                    {
                        selected = candLine;
                        break;
                    }
                }
            }
            else
            {
                selected = candidates[0].Line;
            }

            payload = new VoiceLinePayload
            {
                LineId = selected.Id,
                SpeakerSurvivorId = context.SurvivorId,
                Profession = context.Profession,
                Trigger = trigger,
                TextKey = selected.TextKey,
                TextEnglish = selected.TextEnglish,
                Register = selected.Register,
                UtteredDay = currentDay
            };

            // Update state
            _state.LineLastUtteredDay[selected.Id] = currentDay;
            _state.SurvivorLastUtteredDay[context.SurvivorId] = currentDay;
            _state.History.Add(payload);

            // Invoke delegate seams
            VoiceLineDeliveredSink?.Invoke(payload);
            VoiceBarkAudioSeam?.Invoke(context.SurvivorId, selected.Id);

            return true;
        }

        public SurvivorVoiceState CaptureState()
        {
            return new SurvivorVoiceState
            {
                SchemaVersion = _state.SchemaVersion,
                LineLastUtteredDay = new Dictionary<string, int>(_state.LineLastUtteredDay),
                SurvivorLastUtteredDay = new Dictionary<string, int>(_state.SurvivorLastUtteredDay),
                History = _state.History.Select(h => new VoiceLinePayload
                {
                    LineId = h.LineId,
                    SpeakerSurvivorId = h.SpeakerSurvivorId,
                    Profession = h.Profession,
                    Trigger = h.Trigger,
                    TextKey = h.TextKey,
                    TextEnglish = h.TextEnglish,
                    Register = h.Register,
                    UtteredDay = h.UtteredDay
                }).ToList()
            };
        }

        public void RestoreState(SurvivorVoiceState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.LineLastUtteredDay.Clear();
            _state.SurvivorLastUtteredDay.Clear();
            _state.History.Clear();

            if (state.LineLastUtteredDay != null)
            {
                foreach (var kvp in state.LineLastUtteredDay)
                    _state.LineLastUtteredDay[kvp.Key] = kvp.Value;
            }

            if (state.SurvivorLastUtteredDay != null)
            {
                foreach (var kvp in state.SurvivorLastUtteredDay)
                    _state.SurvivorLastUtteredDay[kvp.Key] = kvp.Value;
            }

            if (state.History != null)
            {
                _state.History.AddRange(state.History.Select(h => new VoiceLinePayload
                {
                    LineId = h.LineId,
                    SpeakerSurvivorId = h.SpeakerSurvivorId,
                    Profession = h.Profession,
                    Trigger = h.Trigger,
                    TextKey = h.TextKey,
                    TextEnglish = h.TextEnglish,
                    Register = h.Register,
                    UtteredDay = h.UtteredDay
                }));
            }
        }
    }
}
