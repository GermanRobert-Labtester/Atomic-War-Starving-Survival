using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Medical
{
    /// <summary>
    /// Disease Mutation (Spec #5 of Section VIII). If an infection is treated
    /// with antibiotics but the course isn't completed, the infection
    /// develops resistance. The system punishes resource hoarding.
    /// </summary>
    public class DiseaseMutationSystem
    {
        public enum Resistance { None, Resistant, MultiResistant }

        [Serializable]
        public class Infection
        {
            public string SurvivorId;
            public string Site;                  // "wound", "lung", etc.
            public Resistance Resistance;
            public float PillsRemaining;
            public float PillsRequired;          // 2, 4, or 99 (multi-resistant)
            public float DaysTreated;
            public float DaysRequired;
            public bool Aborted;
            public string InfectionId;           // unique per infection
        }

        [Serializable]
        public class State
        {
            public List<Infection> Infections = new List<Infection>();
        }

        private State _state = new State();
        public State Current => _state;

        public event Action<Infection> OnInfectionStarted;
        public event Action<Infection, Resistance> OnResistanceEvolved;
        public event Action<Infection> OnInfectionCured;
        public event Action<Infection> OnInfectionRequiresSurgery;

        // Host callbacks.
        public Action<string, float> RequestConsumeItem;          // itemId, count
        public Action<string> SpawnAffliction;                    // affliction id (e.g. "affliction_sepsis")
        public Func<string, string> GetItemId;                    // "antibiotics" -> "antibiotics"
        public System.Random Rng;

        public string StartInfection(string survivorId, string site, System.Random rng = null)
        {
            Rng = rng ?? Rng;
            var inf = new Infection
            {
                SurvivorId = survivorId,
                Site = site ?? "wound",
                Resistance = Resistance.None,
                PillsRequired = 2f,
                PillsRemaining = 2f,
                DaysTreated = 0f,
                DaysRequired = 2f,
                InfectionId = "inf_" + (Rng?.Next(100000, 999999) ?? new System.Random().Next(100000, 999999))
            };
            _state.Infections.Add(inf);
            OnInfectionStarted?.Invoke(inf);
            return inf.InfectionId;
        }

        /// <summary>
        /// Apply one pill of antibiotic. Returns true if accepted.
        /// </summary>
        public bool AdministerPill(Infection inf)
        {
            if (inf == null || inf.Aborted) return false;
            if (inf.PillsRemaining <= 0f) return false;
            if (RequestConsumeItem != null && !string.IsNullOrEmpty(GetItemId?.Invoke("antibiotics")))
                RequestConsumeItem.Invoke("antibiotics", 1f);
            inf.PillsRemaining -= 1f;
            inf.DaysTreated += 1f;
            if (inf.PillsRemaining <= 0f && inf.DaysTreated >= inf.DaysRequired)
            {
                _state.Infections.Remove(inf);
                OnInfectionCured?.Invoke(inf);
            }
            return true;
        }

        /// <summary>
        /// Player chose to stop treatment early to save pills.
        /// Infection mutates: None -> Resistant, Resistant -> Multi-Resistant.
        /// </summary>
        public bool AbortTreatment(Infection inf)
        {
            if (inf == null || inf.Resistance == Resistance.MultiResistant) return false;
            inf.Aborted = true;
            if (inf.Resistance == Resistance.None)
            {
                inf.Resistance = Resistance.Resistant;
                inf.PillsRequired = 4f;
                inf.PillsRemaining = 4f;
                inf.DaysRequired = 3f;
                OnResistanceEvolved?.Invoke(inf, Resistance.Resistant);
            }
            else if (inf.Resistance == Resistance.Resistant)
            {
                inf.Resistance = Resistance.MultiResistant;
                inf.PillsRequired = 99f;       // antibiotics will not work
                OnResistanceEvolved?.Invoke(inf, Resistance.MultiResistant);
                OnInfectionRequiresSurgery?.Invoke(inf);
                SpawnAffliction?.Invoke("affliction_sepsis");
            }
            return true;
        }

        public IEnumerable<Infection> ActiveInfections(string survivorId = null)
        {
            for (int i = 0; i < _state.Infections.Count; i++)
            {
                var inf = _state.Infections[i];
                if (survivorId != null && inf.SurvivorId != survivorId) continue;
                yield return inf;
            }
        }

        public State CaptureState() => _state;
        public void RestoreState(State s) { _state = s ?? new State(); }
    }
}
