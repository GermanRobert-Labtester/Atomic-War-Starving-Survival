using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Narrative
{
    [Serializable]
    public class GhostRadioState
    {
        public string eventId = "ui_event_ghost_radio";
    }

    /// <summary>
    /// Prompt #753: Ghost Radio Signals.
    /// RadioSystem prints terrifying 4th-wall text logs.
    /// If tuned to find source, it vanishes.
    /// </summary>
    public class UIEvent_GhostRadio
    {
        private GhostRadioState _state = new GhostRadioState();
        private bool _isTunedIn = false;

        private static readonly string[] GhostMessages = new string[]
        {
            "Are you watching them starve?",
            "They know you're there.",
            "This is not a game.",
            "Can you hear us through the screen?",
            "The bunker was never empty.",
            "You chose who lived. Remember?",
            "We see the cursor moving.",
            "Save won't protect them.",
            "The radiation is for you, not them.",
            "Do you feel warm right now?"
        };

        public event Action<string> OnGhostMessageReceived;
        public event Action OnSourceVanished;

        public GhostRadioState State => _state;

        public void TickRadio(System.Random rng)
        {
            if (_isTunedIn)
                return;

            // ~5% chance per tick to receive a ghost message
            if (rng.NextDouble() < 0.05)
            {
                string msg = GhostMessages[rng.Next(GhostMessages.Length)];
                OnGhostMessageReceived?.Invoke(msg);
            }
        }

        public void TryTuneIn()
        {
            _isTunedIn = true;
            OnSourceVanished?.Invoke();
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public GhostRadioState CaptureState() => _state;

        public void RestoreState(GhostRadioState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
