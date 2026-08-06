using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class WasteChuteState
    {
        public string moduleId = "shelter_module_waste_chute";
        public bool isActive = false;
    }

    /// <summary>
    /// Prompt #808: Waste Chute.
    /// Built into walls. Dump waste from any floor and it routes instantly
    /// to the Incinerator (hazardous/medical waste) or CompostBin (organic waste).
    /// </summary>
    public class ShelterModule_WasteChute
    {
        public event Action<string, string> OnWasteDeposited;   // survivorId, destination

        private WasteChuteState _state;

        // Organic waste types route to compost; everything else to incinerator
        private static readonly string[] OrganicTypes = { "food", "organic", "compost", "corpse_animal", "plant", "shell" };

        public ShelterModule_WasteChute(WasteChuteState state = null)
        {
            _state = state ?? new WasteChuteState();
        }

        public string ModuleId => _state.moduleId;

        public void Activate()
        {
            _state.isActive = true;
        }

        public void Deactivate()
        {
            _state.isActive = false;
        }

        public bool IsActive() => _state.isActive;

        /// <summary>
        /// Deposit waste and route to appropriate destination.
        /// Returns the destination: "incinerator" or "compost_bin".
        /// </summary>
        public string DepositWaste(string survivorId, string wasteType)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(wasteType))
            {
                Debug.LogWarning("[ShelterModule_WasteChute] DepositWaste called with null/empty argument.");
                return "incinerator";
            }

            string destination = IsOrganic(wasteType) ? "compost_bin" : "incinerator";
            OnWasteDeposited?.Invoke(survivorId, destination);
            return destination;
        }

        private bool IsOrganic(string wasteType)
        {
            string lower = wasteType.ToLowerInvariant();
            for (int i = 0; i < OrganicTypes.Length; i++)
            {
                if (lower == OrganicTypes[i])
                    return true;
            }
            return false;
        }

        public WasteChuteState CaptureState()
        {
            return new WasteChuteState
            {
                moduleId = _state.moduleId,
                isActive = _state.isActive
            };
        }

        public void RestoreState(WasteChuteState state)
        {
            _state = state ?? new WasteChuteState();
        }
    }
}
