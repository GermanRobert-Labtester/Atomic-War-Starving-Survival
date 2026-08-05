using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Shelter
{
    /// <summary>Prompt #125 — Hatch visibility/camouflage. Raids scale with visibility. Camouflage lowers it.</summary>
    public class HatchVisibilitySystem
    {
        public const float MaxVisibility = 1f;
        public const float GeneratorVisibilityPerHour = 0.02f;
        public const float WasteVisibilityPerDump = 0.08f;
        public const float SnowPathVisibilityPerExpedition = 0.05f;
        public const float CamouflageReducePerHour = 0.1f;
        public const float VisibilityDecayPerDay = 0.03f;

        private float _visibility;
        private PersonalQuestSystem _personalQuests;
        private Func<IReadOnlyList<Survivor>> _getSurvivors;

        /// <summary>Prompt #232 — Ghost forces hatch visibility to 0 when outside.</summary>
        public void BindPersonalQuests(
            PersonalQuestSystem personalQuests,
            Func<IReadOnlyList<Survivor>> getSurvivors = null)
        {
            _personalQuests = personalQuests;
            _getSurvivors = getSurvivors;
        }

        public float Visibility
        {
            get
            {
                if (_personalQuests != null
                    && _personalQuests.AnyGhostOutside(_getSurvivors?.Invoke()))
                    return 0f;
                return _visibility;
            }
        }
        public float RaidChanceMultiplier => 0.5f + Visibility * 1.5f; // 0.5x at 0 vis, 2.0x at max
        public event Action<float> OnVisibilityChanged;

        public void AddVisibility(float amount) { if (amount > 0f) SetVisibility(_visibility + amount); }
        public void Camouflage(float workHours) { if (workHours > 0f) SetVisibility(Mathf.Max(0f, _visibility - CamouflageReducePerHour * workHours)); }
        public void TickDaily() { SetVisibility(Mathf.Max(0f, _visibility - VisibilityDecayPerDay)); }

        private void SetVisibility(float v) { float old = _visibility; _visibility = Mathf.Clamp01(v); if (Mathf.Abs(_visibility - old) > 0.001f) OnVisibilityChanged?.Invoke(_visibility); }

        public HatchVisibilitySave CaptureState() => new HatchVisibilitySave { Visibility = _visibility };
        public void RestoreState(HatchVisibilitySave s) => _visibility = s?.Visibility ?? 0f;
    }
    [Serializable] public class HatchVisibilitySave { public float Visibility; }
}
