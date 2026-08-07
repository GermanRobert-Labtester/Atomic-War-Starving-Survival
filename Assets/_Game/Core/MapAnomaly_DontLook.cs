using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class DontLookState
    {
        public string anomalyId = "map_anomaly_dont_look";
        public string warningText = "Averting eyes.";
    }

    /// <summary>
    /// Prompt #757: The "Don't Look" Anomaly.
    /// Indescribable horror. UI warns "Averting eyes." If player clicks to
    /// inspect LootTable, survivor suffers Catatonic Break.
    /// </summary>
    public class MapAnomaly_DontLook
    {
        private DontLookState _state = new DontLookState();

        public event Action OnWarningDisplayed;
        public event Action<string> OnCatatonicBreak;

        public DontLookState State => _state;

        public void DisplayWarning()
        {
            OnWarningDisplayed?.Invoke();
        }

        /// <summary>
        /// If player clicks to look at the node, the survivor suffers
        /// an instant catatonic break.
        /// </summary>
        public void InspectNode(string survivorId)
        {
            OnCatatonicBreak?.Invoke(survivorId);
        }

        /// <summary>
        /// Always true — the anomaly demands aversion.
        /// </summary>
        public bool ShouldAvert() => true;

        // ── Save / Load ────────────────────────────────────────────────

        public DontLookState CaptureState()
        {
            return new DontLookState
            {
                anomalyId = _state.anomalyId,
                warningText = _state.warningText,
            };
        }

        public void RestoreState(DontLookState saved)
        {
            if (saved == null)
            {
                _state = new DontLookState();
                return;
            }
            _state = new DontLookState
            {
                anomalyId = saved.anomalyId,
                warningText = saved.warningText,
            };
        }
    }
}
