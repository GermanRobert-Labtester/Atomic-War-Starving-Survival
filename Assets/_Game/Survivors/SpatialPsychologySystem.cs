using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    public class SpatialPsychologySystem
    {
        private NeedsSystem _needsSystem;
        public void SetNeedsSystem(NeedsSystem ns) => _needsSystem = ns;

        public const string TraitClaustrophobic = "trait_claustrophobic";
        public const string TraitAgoraphobic = "trait_agoraphobic";

        public const float ClaustrophobiaMoraleDrainPerDay = 5f;
        public const float AgoraphobiaRadiationAnxietySpike = 0.20f;

        public event Action<Survivor, float> OnClaustrophobiaMoraleDrained;
        public event Action<Survivor, float> OnAgoraphobiaAnxietySpiked;

        public void Tick(float gameHours, IReadOnlyList<Survivor> survivors)
        {
            if (gameHours <= 0f || survivors == null) return;

            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;

                // Claustrophobia: morale drain inside bunker unless on expedition
                if (sv.HasTrait(TraitClaustrophobic) && !sv.IsOnExpedition)
                {
                    float drain = ClaustrophobiaMoraleDrainPerDay * (gameHours / 24f);
                    if (sv.Needs != null)
                    {
                        if (_needsSystem != null)
                            _needsSystem.Modify(sv, NeedKind.Morale, -drain);
                        else
                            sv.Needs.Morale = Mathf.Clamp(sv.Needs.Morale - drain, 0f, 100f);
                    }
                    OnClaustrophobiaMoraleDrained?.Invoke(sv, drain);
                }
            }
        }

        public void OnExpeditionStarted(Survivor sv)
        {
            if (sv == null || !sv.IsAlive) return;

            sv.IsOnExpedition = true;

            // Agoraphobia: immediate +20 (0.20) RadiationAnxiety upon stepping outside
            if (sv.HasTrait(TraitAgoraphobic))
            {
                sv.RadiationAnxiety = Mathf.Clamp(sv.RadiationAnxiety + AgoraphobiaRadiationAnxietySpike, 0f, 1f);
                if (sv.RadiationAnxiety >= 0.5f)
                {
                    sv.HasRadiationAnxietyStatus = true;
                }
                OnAgoraphobiaAnxietySpiked?.Invoke(sv, AgoraphobiaRadiationAnxietySpike);
            }
        }

        public void OnExpeditionEnded(Survivor sv)
        {
            if (sv == null) return;
            sv.IsOnExpedition = false;
        }
    }
}
