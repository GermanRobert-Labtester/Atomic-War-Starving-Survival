using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class GuitarState
    {
        public string itemId = "item_guitar";
        public string displayName = "Acoustic Guitar";
        public float powerRequired = 0f;
        public float noviceSkillThreshold = 20f;
        public float masterSkillThreshold = 80f;
        public float noviceNoiseGenerated = 15f;
        public float masterMoraleBonus = 30f;
        public bool curesDepressionAtMaster = true;
    }

    /// <summary>
    /// Prompt #611: Item: Acoustic Guitar.
    /// Upgrade over RecordPlayer. Requires no power. Morale buff depends on SkillXP:
    /// Novice = annoying noise, Master = cures Depression.
    /// </summary>
    public class Item_Guitar
    {
        private GuitarState _state = new GuitarState();

        public event Action<GuitarState, float, float> OnGuitarPlayed;
        public event Action<GuitarState, string> OnDepressionCured;
        public event Action<GuitarState, float> OnNoiseComplaint;

        public GuitarState State => _state;

        public (float moraleChange, float noiseGenerated, bool curesDepression) Play(float skillXP)
        {
            float moraleChange = 0f;
            float noiseGenerated = 0f;
            bool curesDepression = false;

            if (skillXP < _state.noviceSkillThreshold)
            {
                moraleChange = -10f;
                noiseGenerated = _state.noviceNoiseGenerated;
                OnNoiseComplaint?.Invoke(_state, noiseGenerated);
            }
            else if (skillXP >= _state.masterSkillThreshold)
            {
                moraleChange = _state.masterMoraleBonus;
                noiseGenerated = 0f;

                if (_state.curesDepressionAtMaster)
                {
                    curesDepression = true;
                    OnDepressionCured?.Invoke(_state, "depression_cured");
                }
            }
            else
            {
                float t = (skillXP - _state.noviceSkillThreshold) / (_state.masterSkillThreshold - _state.noviceSkillThreshold);
                moraleChange = Mathf.Lerp(5f, _state.masterMoraleBonus, t);
                noiseGenerated = Mathf.Lerp(_state.noviceNoiseGenerated, 0f, t);
            }

            OnGuitarPlayed?.Invoke(_state, moraleChange, noiseGenerated);
            return (moraleChange, noiseGenerated, curesDepression);
        }
    }
}
