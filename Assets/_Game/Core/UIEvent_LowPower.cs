using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class LowPowerUIState
    {
        public string eventId = "ui_event_low_power";
        public float batteryThreshold = 0.1f;
        public bool isGlitching = false;
    }

    /// <summary>
    /// Prompt #749: UI Glitching — Low Power.
    /// Below 10% battery, HUD flickers. Text on Needs bars scrambles
    /// (e.g., "H@ng#r"). Player can't perfectly optimize tasks.
    /// </summary>
    public class UIEvent_LowPower
    {
        private LowPowerUIState _state = new LowPowerUIState();
        private static readonly char[] ScrambleChars = { '@', '#', '%' };

        public event Action OnGlitchStarted;
        public event Action OnGlitchStopped;
        public event Action<string, string> OnTextScrambled;

        public LowPowerUIState State => _state;

        public void CheckPower(float batteryPercent)
        {
            bool shouldGlitch = batteryPercent < _state.batteryThreshold;

            if (shouldGlitch && !_state.isGlitching)
            {
                _state.isGlitching = true;
                OnGlitchStarted?.Invoke();
            }
            else if (!shouldGlitch && _state.isGlitching)
            {
                _state.isGlitching = false;
                OnGlitchStopped?.Invoke();
            }
        }

        public string ScrambleText(string text, System.Random rng)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            char[] chars = text.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (rng.NextDouble() < 0.3 && !char.IsWhiteSpace(chars[i]))
                {
                    chars[i] = ScrambleChars[rng.Next(ScrambleChars.Length)];
                }
            }

            string scrambled = new string(chars);
            OnTextScrambled?.Invoke(text, scrambled);
            return scrambled;
        }

        public bool IsGlitching() => _state.isGlitching;
    }
}
