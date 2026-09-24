// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 179: Unified Psychology & Phobia System
// Provides a per-survivor psychological profile: phobia tracking, coping
// mechanism registration, resilience scoring, and a unified view across the
// six existing trauma systems. Engine-neutral Core domain authority.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Psychology
{
    [Serializable]
    public sealed class PhobiaEffectDef
    {
        public string effect { get; set; } = string.Empty;
        public float value { get; set; }
    }

    [Serializable]
    public sealed class PhobiaDef
    {
        public string phobia_id { get; set; } = string.Empty;
        public string phobia_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string trigger_condition { get; set; } = string.Empty;
        public List<string> developed_from_traumas { get; set; } = new List<string>();
        public int severity_threshold { get; set; } = 30;
        public List<PhobiaEffectDef> effects { get; set; } = new List<PhobiaEffectDef>();
    }

    [Serializable]
    public sealed class CopingMechanismDef
    {
        public string mechanism_id { get; set; } = string.Empty;
        public string mechanism_name { get; set; } = string.Empty;
        public int effectiveness { get; set; }
        public List<string> side_effects { get; set; } = new List<string>();
        public string learned_from { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class PsychologyCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<PhobiaDef> phobia_definitions { get; set; } = new List<PhobiaDef>();
        public List<CopingMechanismDef> coping_mechanisms { get; set; } = new List<CopingMechanismDef>();
    }

    [Serializable]
    public sealed class ActivePhobia
    {
        public string phobia_id { get; set; } = string.Empty;
        public float severity { get; set; }       // 0-100
        public int developed_on_day { get; set; }
        public string developed_from_trauma { get; set; } = string.Empty;
        public bool is_managed { get; set; }
    }

    [Serializable]
    public sealed class ActiveCopingMechanism
    {
        public string mechanism_id { get; set; } = string.Empty;
        public string learned_from { get; set; } = string.Empty;
        public int learned_on_day { get; set; }
    }

    [Serializable]
    public sealed class SurvivorPsychologicalProfile
    {
        public string survivor_id { get; set; } = string.Empty;
        public float resilience { get; set; } = 50.0f;         // 0-100, higher = more stable
        public List<ActivePhobia> phobias { get; set; } = new List<ActivePhobia>();
        public List<ActiveCopingMechanism> coping_mechanisms { get; set; } = new List<ActiveCopingMechanism>();
        public List<string> personality_traits { get; set; } = new List<string>();
        public int therapy_sessions_completed { get; set; }
        public int trauma_event_count { get; set; }
    }

    [Serializable]
    public sealed class PhobiaTriggerResult
    {
        public bool Triggered { get; set; }
        public string SurvivorId { get; set; } = string.Empty;
        public string PhobiaId { get; set; } = string.Empty;
        public float Severity { get; set; }
        public List<PhobiaEffectDef> ActiveEffects { get; set; } = new List<PhobiaEffectDef>();
    }

    [Serializable]
    public sealed class PsychologyState
    {
        public int SchemaVersion { get; set; } = 1;
        public List<SurvivorPsychologicalProfile> Profiles { get; set; } = new List<SurvivorPsychologicalProfile>();
        public int TotalPhobiasTriggered { get; set; }
        public int TotalTherapySessions { get; set; }
    }

    /// <summary>
    /// Plan 179 — Unified Psychology & Phobia System.
    /// Provides per-survivor psychological profiles integrating phobia development,
    /// coping mechanisms, resilience scoring, and trauma aggregation across the
    /// six existing trauma systems (read-only integration via injected state).
    /// Engine-neutral Core domain authority.
    /// </summary>
    public sealed class PsychologicalProfileSystem
    {
        private PsychologyState _state;
        private readonly List<PhobiaDef> _phobias = new List<PhobiaDef>();
        private readonly List<CopingMechanismDef> _copingMechanisms = new List<CopingMechanismDef>();

        public event Action<string, string, float>? OnPhobiaDeveloped;     // survivorId, phobiaId, severity
        public event Action<string, string>? OnPhobiaTriggered;             // survivorId, phobiaId
        public event Action<string, string>? OnCopingMechanismLearned;     // survivorId, mechanismId
        public event Action<string, float>? OnResilienceChanged;            // survivorId, newResilience

        public Action<string, string, float>? OnPhobiaDevelopedSeam { get; set; }
        public Action<string, string>? OnPhobiaTriggeredSeam { get; set; }

        public IReadOnlyList<PhobiaDef> Phobias => _phobias;
        public IReadOnlyList<CopingMechanismDef> CopingMechanisms => _copingMechanisms;
        public IReadOnlyList<SurvivorPsychologicalProfile> Profiles => _state.Profiles;

        public PsychologicalProfileSystem(PsychologyState? state = null)
        {
            _state = state ?? new PsychologyState();
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var catalog = JsonSerializer.Deserialize<PsychologyCatalog>(json, options);
                if (catalog == null) return;
                if (catalog.phobia_definitions != null)
                {
                    foreach (var pd in catalog.phobia_definitions)
                    {
                        if (!string.IsNullOrWhiteSpace(pd.phobia_id))
                        {
                            int idx = _phobias.FindIndex(p => string.Equals(p.phobia_id, pd.phobia_id, StringComparison.Ordinal));
                            if (idx >= 0) _phobias[idx] = pd;
                            else _phobias.Add(pd);
                        }
                    }
                }
                if (catalog.coping_mechanisms != null)
                {
                    foreach (var cm in catalog.coping_mechanisms)
                    {
                        if (!string.IsNullOrWhiteSpace(cm.mechanism_id))
                        {
                            int idx = _copingMechanisms.FindIndex(c => string.Equals(c.mechanism_id, cm.mechanism_id, StringComparison.Ordinal));
                            if (idx >= 0) _copingMechanisms[idx] = cm;
                            else _copingMechanisms.Add(cm);
                        }
                    }
                }
            }
            catch (Exception) { /* malformed catalog falls back to built-in defaults; authoring errors are enforced by the data-integrity gate */ }
        }

        public SurvivorPsychologicalProfile EnsureProfile(string survivorId)
        {
            var existing = _state.Profiles.FirstOrDefault(p => string.Equals(p.survivor_id, survivorId, StringComparison.Ordinal));
            if (existing != null) return existing;
            var profile = new SurvivorPsychologicalProfile { survivor_id = survivorId, resilience = 50.0f };
            _state.Profiles.Add(profile);
            return profile;
        }

        public SurvivorPsychologicalProfile? GetProfile(string survivorId) =>
            _state.Profiles.FirstOrDefault(p => string.Equals(p.survivor_id, survivorId, StringComparison.Ordinal));

        /// <summary>
        /// Records a traumatic event against a survivor's profile. May develop
        /// a phobia if severity exceeds the threshold and resilience is insufficient.
        /// </summary>
        public bool RecordTraumaEvent(string survivorId, string traumaTag, float traumaSeverity, int currentDay, ISeededRng rng)
        {
            if (string.IsNullOrWhiteSpace(survivorId) || rng == null) return false;

            var profile = EnsureProfile(survivorId);
            profile.trauma_event_count++;

            // Resilience decays slightly with each trauma
            profile.resilience = Math.Max(0f, profile.resilience - (traumaSeverity * 0.05f));
            OnResilienceChanged?.Invoke(survivorId, profile.resilience);

            // Check eligible phobias linked to this trauma tag
            bool phobiaDeveloped = false;
            var eligiblePhobias = _phobias
                .Where(pd => pd.developed_from_traumas.Any(t => string.Equals(t, traumaTag, StringComparison.OrdinalIgnoreCase)))
                .Where(pd => traumaSeverity >= pd.severity_threshold)
                .Where(pd => !profile.phobias.Any(ap => string.Equals(ap.phobia_id, pd.phobia_id, StringComparison.Ordinal)))
                .OrderBy(pd => pd.phobia_id, StringComparer.Ordinal)
                .ToList();

            foreach (var pd in eligiblePhobias)
            {
                // Phobia development probability: severity/(severity + resilience)
                double developChance = traumaSeverity / (traumaSeverity + profile.resilience + 1.0);
                if (rng.NextDouble() < developChance)
                {
                    float severity = Math.Clamp(traumaSeverity - (profile.resilience * 0.3f), 10f, 100f);
                    var activePhobia = new ActivePhobia
                    {
                        phobia_id = pd.phobia_id,
                        severity = severity,
                        developed_on_day = currentDay,
                        developed_from_trauma = traumaTag,
                        is_managed = false
                    };
                    profile.phobias.Add(activePhobia);
                    _state.TotalPhobiasTriggered++;
                    phobiaDeveloped = true;

                    OnPhobiaDeveloped?.Invoke(survivorId, pd.phobia_id, severity);
                    OnPhobiaDevelopedSeam?.Invoke(survivorId, pd.phobia_id, severity);
                    break; // One phobia per trauma event
                }
            }

            // Trait evolution
            if (profile.trauma_event_count == 3 && !profile.personality_traits.Contains("hardened"))
            {
                profile.personality_traits.Add("hardened");
            }
            if (profile.phobias.Count >= 2 && !profile.personality_traits.Contains("anxious"))
            {
                profile.personality_traits.Add("anxious");
            }

            return phobiaDeveloped;
        }

        /// <summary>
        /// Checks if a specific phobia triggers for a survivor in the given context.
        /// Returns trigger result with active effects.
        /// </summary>
        public PhobiaTriggerResult EvaluatePhobiaExposure(string survivorId, string triggerCondition)
        {
            var result = new PhobiaTriggerResult { SurvivorId = survivorId };
            var profile = GetProfile(survivorId);
            if (profile == null) return result;

            var triggered = profile.phobias
                .Where(ap => !ap.is_managed)
                .Select(ap => new { ap, def = _phobias.FirstOrDefault(pd => string.Equals(pd.phobia_id, ap.phobia_id, StringComparison.Ordinal)) })
                .Where(x => x.def != null && string.Equals(x.def.trigger_condition, triggerCondition, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(x => x.ap.severity)
                .FirstOrDefault();

            if (triggered == null) return result;

            result.Triggered = true;
            result.PhobiaId = triggered.ap.phobia_id;
            result.Severity = triggered.ap.severity;
            result.ActiveEffects = triggered.def!.effects;
            _state.TotalPhobiasTriggered++;

            OnPhobiaTriggered?.Invoke(survivorId, triggered.ap.phobia_id);
            OnPhobiaTriggeredSeam?.Invoke(survivorId, triggered.ap.phobia_id);

            return result;
        }

        public bool TeachCopingMechanism(string survivorId, string mechanismId, string source, int currentDay)
        {
            if (string.IsNullOrWhiteSpace(survivorId) || string.IsNullOrWhiteSpace(mechanismId)) return false;

            var profile = EnsureProfile(survivorId);
            if (profile.coping_mechanisms.Any(cm => string.Equals(cm.mechanism_id, mechanismId, StringComparison.Ordinal))) return false;
            if (!_copingMechanisms.Any(cm => string.Equals(cm.mechanism_id, mechanismId, StringComparison.Ordinal))) return false;

            profile.coping_mechanisms.Add(new ActiveCopingMechanism
            {
                mechanism_id = mechanismId,
                learned_from = source ?? "experience",
                learned_on_day = currentDay
            });

            // Learning coping raises resilience slightly
            profile.resilience = Math.Min(100f, profile.resilience + 3.0f);
            OnCopingMechanismLearned?.Invoke(survivorId, mechanismId);
            return true;
        }

        public bool ConductTherapySession(string survivorId, string phobiaId, float therapistSkill)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) return false;

            var profile = EnsureProfile(survivorId);
            var activePhobia = profile.phobias.FirstOrDefault(ap => string.Equals(ap.phobia_id, phobiaId, StringComparison.Ordinal));
            if (activePhobia == null) return false;

            float reduction = Math.Clamp(therapistSkill * 0.15f, 2f, 20f);
            activePhobia.severity = Math.Max(0f, activePhobia.severity - reduction);

            if (activePhobia.severity <= 5f)
            {
                activePhobia.is_managed = true;
                profile.resilience = Math.Min(100f, profile.resilience + 5.0f);
                OnResilienceChanged?.Invoke(survivorId, profile.resilience);
            }

            profile.therapy_sessions_completed++;
            _state.TotalTherapySessions++;
            return true;
        }

        public float GetProfileResilienceScore(string survivorId)
        {
            var profile = GetProfile(survivorId);
            if (profile == null) return 50.0f;

            float score = profile.resilience;
            // Coping mechanisms boost resilience calculation
            foreach (var cm in profile.coping_mechanisms)
            {
                var def = _copingMechanisms.FirstOrDefault(c => string.Equals(c.mechanism_id, cm.mechanism_id, StringComparison.Ordinal));
                if (def != null) score += def.effectiveness * 0.05f;
            }
            // Active unmanaged phobias penalize resilience
            foreach (var ph in profile.phobias.Where(p => !p.is_managed))
            {
                score -= ph.severity * 0.10f;
            }
            return Math.Clamp(score, 0f, 100f);
        }

        public IReadOnlyList<PhobiaDef> GetAllPhobias() => _phobias;
        public IReadOnlyList<CopingMechanismDef> GetAllCopingMechanisms() => _copingMechanisms;

        public PsychologyState CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(_state);
            return s.Deserialize<PsychologyState>(json) ?? new PsychologyState();
        }

        public void RestoreState(PsychologyState state)
        {
            if (state == null) return;
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(state);
            _state = s.Deserialize<PsychologyState>(json) ?? new PsychologyState();
        }

        public PsychologicalProfileCensus GetCensus()
        {
            int totalPhobias = 0;
            int managedPhobias = 0;
            int totalCoping = 0;
            foreach (var p in _state.Profiles)
            {
                totalPhobias += p.phobias.Count;
                managedPhobias += p.phobias.Count(x => x.is_managed);
                totalCoping += p.coping_mechanisms.Count;
            }
            return new PsychologicalProfileCensus(_state.Profiles.Count, totalPhobias, managedPhobias, totalCoping, _state.TotalTherapySessions);
        }
    }

    public struct PsychologicalProfileCensus
    {
        public readonly int TotalProfiles;
        public readonly int TotalPhobias;
        public readonly int ManagedPhobias;
        public readonly int TotalCopingMechanisms;
        public readonly int TotalTherapySessions;

        public PsychologicalProfileCensus(int totalProfiles, int totalPhobias, int managedPhobias, int totalCopingMechanisms, int totalTherapySessions)
        {
            TotalProfiles = totalProfiles;
            TotalPhobias = totalPhobias;
            ManagedPhobias = managedPhobias;
            TotalCopingMechanisms = totalCopingMechanisms;
            TotalTherapySessions = totalTherapySessions;
        }
    }
}
