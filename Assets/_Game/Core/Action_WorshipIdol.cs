using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class WorshipIdolState
    {
        public string actionId = "action_worship_idol";
        public float moraleGenerated = 0.2f;
        public float productivityLoss = 1.0f;
        public string roomId;
        public string idolType;
        public List<string> worshippingSurvivorIds = new List<string>();
        public List<float> hoursSpentList = new List<float>();
    }

    public class Action_WorshipIdol
    {
        public event Action<string, string> OnIdolWorshipped;
        public event Action<string> OnProductivityHalted;

        private readonly WorshipIdolState _state;
        private readonly Dictionary<string, float> _worshippers; // survivorId → hours spent

        public Action_WorshipIdol()
        {
            _state = new WorshipIdolState();
            _worshippers = new Dictionary<string, float>();
        }

        public Action_WorshipIdol(WorshipIdolState state)
        {
            _state = state ?? new WorshipIdolState();
            _worshippers = new Dictionary<string, float>();

            // Rebuild dictionary from serialized lists
            if (state.worshippingSurvivorIds != null && state.hoursSpentList != null)
            {
                int count = Mathf.Min(state.worshippingSurvivorIds.Count, state.hoursSpentList.Count);
                for (int i = 0; i < count; i++)
                {
                    _worshippers[state.worshippingSurvivorIds[i]] = state.hoursSpentList[i];
                }
            }
        }

        /// <summary>
        /// Places a broken pre-war object as an idol in a room corner.
        /// </summary>
        public void PlaceIdol(string roomId, string idolType)
        {
            _state.roomId = roomId;
            _state.idolType = idolType;
        }

        /// <summary>
        /// A despairing survivor stares at the idol for a number of hours.
        /// They gain morale but all productivity halts during worship.
        /// </summary>
        public void Worship(string survivorId, float hours)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            if (hours <= 0f) return;

            if (_worshippers.ContainsKey(survivorId))
            {
                _worshippers[survivorId] += hours;
            }
            else
            {
                _worshippers[survivorId] = hours;
            }

            OnIdolWorshipped?.Invoke(survivorId, _state.idolType);
            OnProductivityHalted?.Invoke(survivorId);
        }

        /// <summary>
        /// Returns true if the survivor is currently worshipping.
        /// </summary>
        public bool IsWorshipping(string survivorId)
        {
            return _worshippers.ContainsKey(survivorId) && _worshippers[survivorId] > 0f;
        }

        /// <summary>
        /// Returns 0f (productivity halted) if worshipping, 1f otherwise.
        /// </summary>
        public float GetProductivityMultiplier(string survivorId)
        {
            return IsWorshipping(survivorId) ? 0f : 1f;
        }

        /// <summary>
        /// Stops a survivor from worshipping (e.g. another survivor intervenes or time expires).
        /// </summary>
        public void StopWorshipping(string survivorId)
        {
            _worshippers.Remove(survivorId);
        }

        public WorshipIdolState CaptureState()
        {
            _state.worshippingSurvivorIds.Clear();
            _state.hoursSpentList.Clear();

            foreach (var kvp in _worshippers)
            {
                _state.worshippingSurvivorIds.Add(kvp.Key);
                _state.hoursSpentList.Add(kvp.Value);
            }

            return _state;
        }

        public void RestoreState(WorshipIdolState state)
        {
            if (state == null) return;
            _state.moraleGenerated = state.moraleGenerated;
            _state.productivityLoss = state.productivityLoss;
            _state.roomId = state.roomId;
            _state.idolType = state.idolType;

            _worshippers.Clear();
            if (state.worshippingSurvivorIds != null && state.hoursSpentList != null)
            {
                int count = Mathf.Min(state.worshippingSurvivorIds.Count, state.hoursSpentList.Count);
                for (int i = 0; i < count; i++)
                {
                    _worshippers[state.worshippingSurvivorIds[i]] = state.hoursSpentList[i];
                }
            }
        }
    }
}
