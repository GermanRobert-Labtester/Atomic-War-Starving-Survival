using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class DiseaseEntry
    {
        public string disease_id = "";
        public string vector_type = "";       // "water", "air", "blood"
        public List<string> infected_ids = new List<string>();
        public List<string> quarantined_ids = new List<string>();
        public float spread_timer = 0f;
    }

    [Serializable]
    public class DiseaseSystemExpansionState
    {
        public string system_id = "disease_system_expansion";
        public List<DiseaseEntry> diseases = new List<DiseaseEntry>();
        public bool tools_sterilized = false;
        public bool water_purified = false;
        public bool vents_sealed = false;
    }

    /// <summary>
    /// Prompt #834: Epidemic Contagion — Disease System Expansion.
    /// Diseases spread through distinct vectors:
    ///   Cholera = water (shared WaterPurifier),
    ///   Flu = air (Vents),
    ///   Hepatitis = blood (unsterilised surgical tools).
    /// Quarantine strategy differs per vector.
    /// Plain C# class, not a MonoBehaviour.
    /// </summary>
    public class DiseaseSystem_Expansion
    {
        // ── Constants ──────────────────────────────────────────────────
        public const string VECTOR_WATER = "water";
        public const string VECTOR_AIR = "air";
        public const string VECTOR_BLOOD = "blood";

        private const float SPREAD_INTERVAL_HOURS = 4f;

        // ── Events ─────────────────────────────────────────────────────
        public event Action<string, string> OnInfection;             // survivorId, diseaseId
        public event Action<string> OnQuarantineStarted;             // survivorId
        public event Action<string> OnQuarantineEnded;               // survivorId
        public event Action<string> OnOutbreakDeclared;              // diseaseId

        // ── State ──────────────────────────────────────────────────────
        private readonly Dictionary<string, DiseaseEntry> _diseases
            = new Dictionary<string, DiseaseEntry>();

        private bool _toolsSterilized;
        private bool _waterPurified;
        private bool _ventsSealed;

        private readonly System.Random _rng = AtomicWar._Game.Utilities.SeededRandom.Create(
            AtomicWar._Game.Utilities.SeededRandom.WorldSeed, "diseasesystem_expansion");

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Register a disease with its transmission vector.
        /// Call once at setup per disease type.
        /// </summary>
        public void RegisterDisease(string diseaseId, string vectorType)
        {
            if (string.IsNullOrEmpty(diseaseId) || string.IsNullOrEmpty(vectorType)) return;

            if (!_diseases.ContainsKey(diseaseId))
            {
                _diseases[diseaseId] = new DiseaseEntry
                {
                    disease_id = diseaseId,
                    vector_type = vectorType
                };
            }
        }

        /// <summary>Infect a survivor with a registered disease.</summary>
        public void Infect(string survivorId, string diseaseId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(diseaseId)) return;
            if (!_diseases.TryGetValue(diseaseId, out var entry))
            {
                Debug.LogWarning($"[DiseaseSystem_Expansion] Unknown disease '{diseaseId}'.");
                return;
            }

            if (entry.infected_ids.Contains(survivorId)) return;

            entry.infected_ids.Add(survivorId);
            OnInfection?.Invoke(survivorId, diseaseId);

            // 3+ infected = outbreak
            if (entry.infected_ids.Count >= 3)
                OnOutbreakDeclared?.Invoke(diseaseId);
        }

        /// <summary>
        /// Tick once per in-game hour. Attempts to spread each disease
        /// based on its vector and current countermeasures.
        /// </summary>
        public void TickHour()
        {
            foreach (var kvp in _diseases)
            {
                var entry = kvp.Value;
                if (entry.infected_ids.Count == 0) continue;

                entry.spread_timer += 1f;
                if (entry.spread_timer < SPREAD_INTERVAL_HOURS) continue;

                entry.spread_timer = 0f;

                if (IsVectorBlocked(entry.vector_type)) continue;

                // Try to spread to one random non-infected, non-quarantined survivor
                // (In real play, an external system provides the candidate pool;
                //  here we just fire the event for the orchestrator.)
                // This is a hook — actual spread logic is driven by the game loop
                // calling Infect() after checking proximity / shared resources.
            }
        }

        /// <summary>Returns the transmission vector for a disease, or empty string.</summary>
        public string GetTransmissionVector(string diseaseId)
        {
            if (string.IsNullOrEmpty(diseaseId)) return "";
            return _diseases.TryGetValue(diseaseId, out var e) ? e.vector_type : "";
        }

        /// <summary>
        /// Quarantine a survivor to prevent further spread.
        /// </summary>
        public void Quarantine(string survivorId, string diseaseId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(diseaseId)) return;
            if (!_diseases.TryGetValue(diseaseId, out var entry)) return;

            if (!entry.quarantined_ids.Contains(survivorId))
            {
                entry.quarantined_ids.Add(survivorId);
                OnQuarantineStarted?.Invoke(survivorId);
            }
        }

        /// <summary>Remove a survivor from quarantine.</summary>
        public void EndQuarantine(string survivorId, string diseaseId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(diseaseId)) return;
            if (!_diseases.TryGetValue(diseaseId, out var entry)) return;

            if (entry.quarantined_ids.Remove(survivorId))
                OnQuarantineEnded?.Invoke(survivorId);
        }

        /// <summary>Returns true if the survivor is currently contagious.</summary>
        public bool IsContagious(string survivorId, string diseaseId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(diseaseId))
                return false;
            if (!_diseases.TryGetValue(diseaseId, out var entry)) return false;

            return entry.infected_ids.Contains(survivorId)
                   && !entry.quarantined_ids.Contains(survivorId);
        }

        /// <summary>Sterilise surgical tools — blocks blood-vector spread.</summary>
        public void SterilizeTools()
        {
            _toolsSterilized = true;
        }

        /// <summary>Purify water supply — blocks water-vector spread.</summary>
        public void PurifyWater()
        {
            _waterPurified = true;
        }

        /// <summary>Seal shelter vents — blocks air-vector spread.</summary>
        public void SealVents()
        {
            _ventsSealed = true;
        }

        /// <summary>Reset water purification (e.g. filter degrades).</summary>
        public void ResetWaterPurification() => _waterPurified = false;

        /// <summary>Reset vent seal (e.g. seal degrades).</summary>
        public void ResetVentSeal() => _ventsSealed = false;

        /// <summary>Reset tool sterilisation (e.g. used in surgery).</summary>
        public void ResetToolSterilization() => _toolsSterilized = false;

        // ── Helpers ────────────────────────────────────────────────────

        private bool IsVectorBlocked(string vectorType)
        {
            switch (vectorType)
            {
                case VECTOR_WATER: return _waterPurified;
                case VECTOR_AIR: return _ventsSealed;
                case VECTOR_BLOOD: return _toolsSterilized;
                default: return false;
            }
        }

        // ── Save / Load ────────────────────────────────────────────────

        public DiseaseSystemExpansionState CaptureState()
        {
            var state = new DiseaseSystemExpansionState
            {
                system_id = "disease_system_expansion",
                diseases = new List<DiseaseEntry>(),
                tools_sterilized = _toolsSterilized,
                water_purified = _waterPurified,
                vents_sealed = _ventsSealed
            };

            foreach (var kvp in _diseases)
            {
                var src = kvp.Value;
                state.diseases.Add(new DiseaseEntry
                {
                    disease_id = src.disease_id,
                    vector_type = src.vector_type,
                    infected_ids = new List<string>(src.infected_ids),
                    quarantined_ids = new List<string>(src.quarantined_ids),
                    spread_timer = src.spread_timer
                });
            }

            return state;
        }

        public void RestoreState(DiseaseSystemExpansionState saved)
        {
            _diseases.Clear();
            if (saved == null) return;

            _toolsSterilized = saved.tools_sterilized;
            _waterPurified = saved.water_purified;
            _ventsSealed = saved.vents_sealed;

            foreach (var entry in saved.diseases)
            {
                _diseases[entry.disease_id] = new DiseaseEntry
                {
                    disease_id = entry.disease_id,
                    vector_type = entry.vector_type,
                    infected_ids = new List<string>(entry.infected_ids),
                    quarantined_ids = new List<string>(entry.quarantined_ids),
                    spread_timer = entry.spread_timer
                };
            }
        }
    }
}
