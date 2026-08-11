using System;
using System.Collections.Generic;

using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Survivors
{
    [Serializable]
    public class AnthropophobiaState
    {
        public string survivorId;
        public bool isPhobic;
        public float actionSpeedBuffAlone = 1.5f;
        public float stealthBuffAlone = 1.5f;
        public bool isPanicking;
        public string triggerCause;
    }

    /// <summary>
    /// Prompt #597: Phobia — Anthropophobia (Fear of Humans).
    /// Developed after surviving capture/torture by Raiders. The survivor gains
    /// massive ActionSpeed and Stealth buffs, BUT only if entirely alone.
    /// If another human (even an ally) enters their room, they suffer a PanicAttack.
    /// </summary>
    
    [Serializable]
    public class Trait_AnthropophobiaSave
    {
        public string systemId = "trait_anthropophobia";

        public List<AnthropophobiaState> states = new List<AnthropophobiaState>();
    }
public class Trait_Anthropophobia
    {
        private readonly System.Collections.Generic.Dictionary<string, AnthropophobiaState> _states =
            new System.Collections.Generic.Dictionary<string, AnthropophobiaState>();

        public event Action<string> OnAnthropophobiaDeveloped;
        public event Action<string> OnPanicAttackTriggered;
        public event Action<string, float, float> OnIsolationBuffApplied;

        public void DevelopPhobia(string survivorId, string cause)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            if (_states.ContainsKey(survivorId)) return;

            _states[survivorId] = new AnthropophobiaState
            {
                survivorId = survivorId,
                isPhobic = true,
                triggerCause = cause ?? "raider_capture"
            };

            OnAnthropophobiaDeveloped?.Invoke(survivorId);
        }

        /// <summary>
        /// Called when another human enters the phobic survivor's room.
        /// </summary>
        public void NotifyHumanEnteredRoom(string survivorId)
        {
            if (!_states.TryGetValue(survivorId, out var state)) return;
            if (!state.isPhobic) return;

            state.isPanicking = true;
            OnPanicAttackTriggered?.Invoke(survivorId);
        }

        public void NotifyLeftAlone(string survivorId)
        {
            if (!_states.TryGetValue(survivorId, out var state)) return;
            state.isPanicking = false;
        }

        public bool IsPhobic(string survivorId)
        {
            return _states.TryGetValue(survivorId, out var state) && state.isPhobic;
        }

        public bool IsPanicking(string survivorId)
        {
            return _states.TryGetValue(survivorId, out var state) && state.isPanicking;
        }

        /// <summary>
        /// Returns (actionSpeedMult, stealthMult) when alone; (1,1) when not alone.
        /// </summary>
        public (float actionSpeedMult, float stealthMult) GetBuffs(string survivorId, bool isAlone)
        {
            if (!_states.TryGetValue(survivorId, out var state) || !state.isPhobic)
                return (1f, 1f);

            if (!isAlone) return (1f, 1f);

            OnIsolationBuffApplied?.Invoke(survivorId, state.actionSpeedBuffAlone, state.stealthBuffAlone);
            return (state.actionSpeedBuffAlone, state.stealthBuffAlone);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public Trait_AnthropophobiaSave CaptureState() => new Trait_AnthropophobiaSave
        {
            states = SaveMap.Capture(_states),
        };

        public void RestoreState(Trait_AnthropophobiaSave saved) =>
            SaveMap.Restore(_states, saved?.states, e => e.survivorId);

}
}
