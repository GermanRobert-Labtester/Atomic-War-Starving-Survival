using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class C4State
    {
        public string itemId = "item_c4";
        public bool deafensSurvivor = true;
        public bool triggersReinforcements = true;
    }

    public class Item_C4
    {
        // Events
        public event Action<string, string> OnBreachSuccessful;      // survivorId, targetId
        public event Action<string> OnSurvivorDeafened;              // survivorId
        public event Action<string> OnReinforcementsTriggered;       // nodeId

        // Internal state
        private readonly C4State _state;
        private readonly HashSet<string> _deafenedSurvivors = new HashSet<string>();
        private readonly List<string> _destroyedTargets = new List<string>();

        public Item_C4()
        {
            _state = new C4State();
        }

        /// <summary>
        /// Detonate C4 charge. Instantly destroys any locked door/safe/barricade.
        /// Deafens the survivor (disables audio scouting).
        /// Triggers highest-tier enemy reinforcements at the node.
        /// </summary>
        public void Detonate(string survivorId, string targetId, string nodeId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(targetId))
            {
                Debug.LogWarning("[Item_C4] Invalid survivor or target id.");
                return;
            }

            // Destroy the target (door/safe/barricade)
            _destroyedTargets.Add(targetId);
            OnBreachSuccessful?.Invoke(survivorId, targetId);

            // Deafen the survivor
            if (_state.deafensSurvivor)
            {
                _deafenedSurvivors.Add(survivorId);
                OnSurvivorDeafened?.Invoke(survivorId);
            }

            // Trigger reinforcements
            if (_state.triggersReinforcements && !string.IsNullOrEmpty(nodeId))
            {
                OnReinforcementsTriggered?.Invoke(nodeId);
            }
        }

        /// <summary>
        /// Returns true if the survivor is currently deafened.
        /// </summary>
        public bool IsDeafened(string survivorId)
        {
            return !string.IsNullOrEmpty(survivorId) && _deafenedSurvivors.Contains(survivorId);
        }

        /// <summary>
        /// Clear deafened status for a survivor (e.g., after recovery).
        /// </summary>
        public void ClearDeafened(string survivorId)
        {
            if (!string.IsNullOrEmpty(survivorId))
            {
                _deafenedSurvivors.Remove(survivorId);
            }
        }

        public C4State CaptureState()
        {
            return new C4State
            {
                itemId = _state.itemId,
                deafensSurvivor = _state.deafensSurvivor,
                triggersReinforcements = _state.triggersReinforcements
            };
        }

        public void RestoreState(C4State saved)
        {
            if (saved == null) return;
            _state.itemId = saved.itemId;
            _state.deafensSurvivor = saved.deafensSurvivor;
            _state.triggersReinforcements = saved.triggersReinforcements;
        }
    }
}
