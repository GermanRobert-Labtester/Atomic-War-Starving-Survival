using System;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>Prompt #165 — Clothing degradation: sweat/humidity rots clothes, ragged = warmth & morale penalty, SewingKit+Cloth repair.</summary>
    public class ClothingDegradationSystem
    {
        private PersonalQuestSystem _personalQuests;
        private System.Func<System.Collections.Generic.IReadOnlyList<Survivor>> _getSurvivors;
        private NeedsSystem _needsSystem;
        public void SetNeedsSystem(NeedsSystem ns) => _needsSystem = ns;

        /// <summary>Prompt #243 — Armorer: clothing degrades 75% slower bunker-wide.</summary>
        public void BindPersonalQuests(
            PersonalQuestSystem personalQuests,
            System.Func<System.Collections.Generic.IReadOnlyList<Survivor>> getSurvivors = null)
        {
            _personalQuests = personalQuests;
            _getSurvivors = getSurvivors;
        }

        public const float DegradePerHour = 0.15f;
        public const float HighHumidityMultiplier = 2.5f;
        public const float RaggedThreshold = 0f;
        public const float RepairPerCloth = 30f;
        public const float MaxDurability = 100f;
        public const float RaggedWarmthLossMultiplier = 1.5f;
        public const float RaggedMoraleDrainPerHour = 1f;

        public event Action<Survivor> OnRagged;
        public event Action<Survivor> OnRepaired;

        public void Tick(Survivor sv, float gameHours, float roomHumidity)
        {
            if (sv == null || !sv.IsAlive) return;
            // #256 Dragon's Hoard: personal inventory (clothing included) never degrades.
            if (_personalQuests != null && _personalQuests.PersonalInventoryNeverDegrades(sv))
                return;
            float rate = DegradePerHour * (roomHumidity > 0.6f ? HighHumidityMultiplier : 1f);
            if (_personalQuests != null && _getSurvivors != null)
                rate *= _personalQuests.GetClothingDegradeMultiplier(_getSurvivors());
            sv.ClothingDurability = Mathf.Max(0f, sv.ClothingDurability - rate * gameHours);
            if (sv.ClothingDurability <= 0f && !sv.IsRagged) { sv.IsRagged = true; OnRagged?.Invoke(sv); }
            if (sv.IsRagged)
            {
                if (_needsSystem != null)
                    _needsSystem.Modify(sv, NeedKind.Morale, -(RaggedMoraleDrainPerHour * gameHours));
                else
                    sv.Needs.Morale = Mathf.Clamp(sv.Needs.Morale - RaggedMoraleDrainPerHour * gameHours, 0f, 100f);
            }
        }

        public float GetWarmthLossMultiplier(Survivor sv) => sv != null && sv.IsRagged ? RaggedWarmthLossMultiplier : 1f;

        public void Repair(Survivor sv)
        {
            if (sv == null || !sv.IsRagged) return;
            sv.ClothingDurability = MaxDurability; sv.IsRagged = false; OnRepaired?.Invoke(sv);
        }
    }
}
