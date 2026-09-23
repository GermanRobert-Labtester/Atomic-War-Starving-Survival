// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Ashfall.Core.Random;

namespace Ashfall.Core.Survivors
{
    [Serializable]
    public sealed class DreamTemplate
    {
        public string template_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string dream_type { get; set; } = "peaceful"; // peaceful, nightmare, memory, prophetic, surreal
        public float min_trauma { get; set; } = 0.0f;
        public float max_trauma { get; set; } = 100.0f;
        public float min_morale { get; set; } = 0.0f;
        public float max_morale { get; set; } = 100.0f;
        public float rest_bonus { get; set; } = 0.0f;
        public float trauma_delta { get; set; } = 0.0f;
        public float morale_delta { get; set; } = 0.0f;
        public string interpretation_insight { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class DreamCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<DreamTemplate> dream_templates { get; set; } = new List<DreamTemplate>();
    }

    [Serializable]
    public sealed class DreamRecord
    {
        public string record_id { get; set; } = string.Empty;
        public string survivor_id { get; set; } = string.Empty;
        public string template_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string dream_type { get; set; } = string.Empty;
        public int day { get; set; }
        public float rest_bonus { get; set; }
        public float trauma_delta { get; set; }
        public float morale_delta { get; set; }
        public bool is_interpreted { get; set; }
        public string interpretation_choice { get; set; } = string.Empty;
        public string insight { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class DreamSleepResult
    {
        public bool HadDream { get; set; }
        public DreamRecord? Record { get; set; }
        public float EffectiveRestBonus { get; set; }
        public float TraumaDelta { get; set; }
        public float MoraleDelta { get; set; }
        public string Summary { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class DreamSystemState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextRecordId { get; set; } = 1;
        public List<DreamTemplate> AuthoredTemplates { get; set; } = new List<DreamTemplate>();
        public List<DreamRecord> DreamRecords { get; set; } = new List<DreamRecord>();
        public Dictionary<string, int> ConsecutiveNightmares { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
    }

    /// <summary>
    /// Plan 177: Dream &amp; Sleep Cycle System.
    /// Governs survivor dream generation during sleep, trauma-driven nightmare triggers,
    /// rest quality bonuses/penalties, dream interpretation, and subconscious memory processing.
    /// Engine-neutral domain authority in Core.
    /// </summary>
    public sealed class DreamSystem
    {
        private DreamSystemState _state;

        public event Action<DreamSleepResult>? OnDreamExperienced;
        public event Action<DreamRecord>? OnDreamInterpreted;

        public Action<DreamSleepResult>? OnDreamExperiencedSeam { get; set; }
        public Action<DreamRecord>? OnDreamInterpretedSeam { get; set; }

        public IReadOnlyList<DreamTemplate> AuthoredTemplates => _state.AuthoredTemplates;
        public IReadOnlyList<DreamRecord> DreamRecords => _state.DreamRecords;

        public DreamSystem(DreamSystemState? state = null)
        {
            _state = state ?? new DreamSystemState();
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var catalog = JsonSerializer.Deserialize<DreamCatalog>(json, options);
                if (catalog?.dream_templates != null)
                {
                    foreach (var template in catalog.dream_templates)
                    {
                        RegisterTemplate(template);
                    }
                }
            }
            catch (Exception)
            {
                // Core silent resilience
            }
        }

        public void RegisterTemplate(DreamTemplate template)
        {
            if (template == null || string.IsNullOrWhiteSpace(template.template_id)) return;

            int idx = _state.AuthoredTemplates.FindIndex(t => string.Equals(t.template_id, template.template_id, StringComparison.Ordinal));
            if (idx >= 0)
            {
                _state.AuthoredTemplates[idx] = template;
            }
            else
            {
                _state.AuthoredTemplates.Add(template);
            }
        }

        public IReadOnlyList<DreamTemplate> GetAllTemplates() => _state.AuthoredTemplates;

        public DreamTemplate? GetTemplate(string templateId)
        {
            return _state.AuthoredTemplates.FirstOrDefault(t => string.Equals(t.template_id, templateId, StringComparison.Ordinal));
        }

        public DreamSleepResult ProcessSleepCycle(string survivorId, float trauma, float morale, int currentDay, ISeededRng rng, bool forceDream = false)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) throw new ArgumentNullException(nameof(survivorId));
            if (rng == null) throw new ArgumentNullException(nameof(rng));

            // Base dream probability: higher when traumatized or under severe stress
            float dreamProbability = trauma >= 60.0f ? 0.90f : 0.70f;
            if (!forceDream && (rng.NextDouble() > dreamProbability || _state.AuthoredTemplates.Count == 0))
            {
                return new DreamSleepResult
                {
                    HadDream = false,
                    EffectiveRestBonus = 0.0f,
                    TraumaDelta = 0.0f,
                    MoraleDelta = 0.0f,
                    Summary = "Dreamless sleep"
                };
            }

            // Find eligible templates based on survivor's trauma and morale
            var candidates = _state.AuthoredTemplates
                .Where(t => trauma >= t.min_trauma && trauma <= t.max_trauma && morale >= t.min_morale && morale <= t.max_morale)
                .OrderBy(t => t.template_id, StringComparer.Ordinal)
                .ToList();

            if (candidates.Count == 0)
            {
                // Fallback: match by trauma bounds alone
                candidates = _state.AuthoredTemplates
                    .Where(t => trauma >= t.min_trauma && trauma <= t.max_trauma)
                    .OrderBy(t => t.template_id, StringComparer.Ordinal)
                    .ToList();
            }

            if (candidates.Count == 0)
            {
                // Final fallback: all templates
                candidates = _state.AuthoredTemplates
                    .OrderBy(t => t.template_id, StringComparer.Ordinal)
                    .ToList();
            }

            int selectedIndex = rng.Next(0, candidates.Count);
            var chosen = candidates[selectedIndex];

            float effectiveRest = chosen.rest_bonus;
            float traumaDelta = chosen.trauma_delta;
            float moraleDelta = chosen.morale_delta;

            // Track consecutive nightmares
            if (!_state.ConsecutiveNightmares.ContainsKey(survivorId))
            {
                _state.ConsecutiveNightmares[survivorId] = 0;
            }

            if (string.Equals(chosen.dream_type, "nightmare", StringComparison.OrdinalIgnoreCase))
            {
                _state.ConsecutiveNightmares[survivorId]++;
                // Compounding insomnia penalty if 3 or more consecutive nightmares
                if (_state.ConsecutiveNightmares[survivorId] >= 3)
                {
                    effectiveRest -= 0.15f;
                    traumaDelta += 4.0f;
                }
            }
            else if (string.Equals(chosen.dream_type, "peaceful", StringComparison.OrdinalIgnoreCase))
            {
                _state.ConsecutiveNightmares[survivorId] = 0;
            }

            var record = new DreamRecord
            {
                record_id = $"dream_rec_{_state.NextRecordId++}",
                survivor_id = survivorId,
                template_id = chosen.template_id,
                display_name = chosen.display_name,
                dream_type = chosen.dream_type,
                day = currentDay,
                rest_bonus = effectiveRest,
                trauma_delta = traumaDelta,
                morale_delta = moraleDelta,
                is_interpreted = false,
                insight = chosen.interpretation_insight
            };

            _state.DreamRecords.Add(record);

            var result = new DreamSleepResult
            {
                HadDream = true,
                Record = record,
                EffectiveRestBonus = effectiveRest,
                TraumaDelta = traumaDelta,
                MoraleDelta = moraleDelta,
                Summary = $"Experienced {chosen.dream_type} dream: '{chosen.display_name}'"
            };

            OnDreamExperienced?.Invoke(result);
            OnDreamExperiencedSeam?.Invoke(result);

            return result;
        }

        public bool InterpretDream(string survivorId, string recordId, string interpretationChoice)
        {
            if (string.IsNullOrWhiteSpace(survivorId) || string.IsNullOrWhiteSpace(recordId)) return false;

            var record = _state.DreamRecords.FirstOrDefault(r =>
                string.Equals(r.record_id, recordId, StringComparison.Ordinal) &&
                string.Equals(r.survivor_id, survivorId, StringComparison.Ordinal));

            if (record == null || record.is_interpreted) return false;

            record.is_interpreted = true;
            record.interpretation_choice = interpretationChoice ?? "Reflected quietly";

            // If a nightmare was successfully interpreted and processed, grant psychological relief
            if (string.Equals(record.dream_type, "nightmare", StringComparison.OrdinalIgnoreCase))
            {
                record.trauma_delta -= 3.0f;
                record.morale_delta += 5.0f;
            }

            OnDreamInterpreted?.Invoke(record);
            OnDreamInterpretedSeam?.Invoke(record);

            return true;
        }

        public List<DreamRecord> GetDreamHistory(string survivorId)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) return new List<DreamRecord>();
            return _state.DreamRecords
                .Where(r => string.Equals(r.survivor_id, survivorId, StringComparison.Ordinal))
                .OrderBy(r => r.day)
                .ToList();
        }

        public int GetConsecutiveNightmares(string survivorId)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) return 0;
            return _state.ConsecutiveNightmares.TryGetValue(survivorId, out int count) ? count : 0;
        }

        public DreamSystemState CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(_state);
            return s.Deserialize<DreamSystemState>(json) ?? new DreamSystemState();
        }

        public void RestoreState(DreamSystemState state)
        {
            if (state == null) return;
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(state);
            _state = s.Deserialize<DreamSystemState>(json) ?? new DreamSystemState();
        }
    }
}
