#pragma warning disable CS0067 // Public API event surface; subscribers arrive with feature wiring
using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Bunker Manifesto System (#71) — as the campaign progresses, the
    /// player drafts a formal code of laws (rationing rules, medical
    /// priorities, defense protocols) that shapes survivor behavior
    /// and crisis responses.
    ///
    /// Owns: Survivor.ManifestoLawCodeId, Survivor.ManifestoAdherence.
    /// </summary>
    public class BunkerManifestoSystem
    {
        public const float AdherenceBonusMoralePerDay = 2f;
        public const float NonAdherencePenaltyMoralePerDay = -1f;
        public const float LawEnactmentMoraleBoost = 5f;

        public event Action<string, int> OnLawEnacted;
        // lawId, day
        public event Action<Survivor, float> OnAdherenceChanged;
        public event Action<string> OnManifestoCompleted;
        // all 4 categories filled

        private readonly Dictionary<string, ManifestoLaw> _laws =
            new Dictionary<string, ManifestoLaw>();
        private readonly HashSet<string> _categoriesFilled = new HashSet<string>();

        public IReadOnlyDictionary<string, ManifestoLaw> Laws => _laws;
        public bool IsComplete => _categoriesFilled.Count >= 4;

        public bool EnactLaw(ManifestoLaw law)
        {
            if (_laws.ContainsKey(law.LawId)) return false;

            _laws[law.LawId] = law;
            _categoriesFilled.Add(law.Category);
            OnLawEnacted?.Invoke(law.LawId, law.DayEnacted);

            if (IsComplete)
                OnManifestoCompleted?.Invoke("manifesto_complete");

            return true;
        }

        public void TickSurvivor(Survivor sv, float gameHours)
        {
            if (sv == null || !sv.IsAlive) return;
            if (string.IsNullOrEmpty(sv.ManifestoLawCodeId)) return;

            // Adherents get morale boost
            sv.ManifestoAdherence = Math.Min(1f,
                sv.ManifestoAdherence + 0.01f * (gameHours / 24f));
        }

        public float GetAdherenceMoraleModifier(Survivor sv)
        {
            if (sv == null || string.IsNullOrEmpty(sv.ManifestoLawCodeId))
                return 0f;
            return sv.ManifestoAdherence > 0.7f
                ? AdherenceBonusMoralePerDay / 24f
                : NonAdherencePenaltyMoralePerDay / 24f;
        }

        public string[] GetLawCategories() => new[]
            { "rationing", "medical", "defense", "justice" };
    }
}
