using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    [Serializable]
    public class SorterState
    {
        public string moduleId = "shelter_module_sorter";
        public bool isActive = false;
    }

    /// <summary>
    /// Prompt #800: Automated Sorting Bins.
    /// Replaces Quartermaster. Pulls items from Airlock, routes to categorized storage.
    /// Eliminates InternalHauling fatigue.
    /// </summary>
    public class ShelterModule_Sorter
    {
        public event Action<string, string> OnItemRouted;           // itemId, destinationCategory
        public event Action OnHaulingFatigueEliminated;

        private SorterState _state;

        // Maps itemType -> destination category
        private static readonly Dictionary<string, string> TypeRouteMap = new Dictionary<string, string>
        {
            { "medicine",  "meds" },
            { "meds",      "meds" },
            { "bandage",   "meds" },
            { "iodine",    "meds" },
            { "ammo",      "ammo" },
            { "bullet",    "ammo" },
            { "weapon",    "ammo" },
            { "food",      "food" },
            { "ration",    "food" },
            { "water",     "food" },
            { "canned",    "food" },
        };

        public ShelterModule_Sorter(SorterState state = null)
        {
            _state = state ?? new SorterState();
        }

        public string ModuleId => _state.moduleId;

        public void Activate()
        {
            _state.isActive = true;
            // Automation eliminates manual hauling
            OnHaulingFatigueEliminated?.Invoke();
        }

        public void Deactivate()
        {
            _state.isActive = false;
        }

        public bool IsActive() => _state.isActive;

        /// <summary>
        /// Sort a single item by its type. Returns the destination bin category.
        /// </summary>
        public string SortItem(string itemId, string itemType)
        {
            if (string.IsNullOrEmpty(itemId) || string.IsNullOrEmpty(itemType))
            {
                Debug.LogWarning("[ShelterModule_Sorter] SortItem called with null/empty itemId or itemType.");
                return "misc";
            }

            string destination;
            if (!TypeRouteMap.TryGetValue(itemType.ToLowerInvariant(), out destination))
            {
                destination = "misc";
            }

            OnItemRouted?.Invoke(itemId, destination);
            return destination;
        }

        /// <summary>
        /// Process all items dumped from the airlock in bulk.
        /// </summary>
        public void ProcessAirlockDump(List<string> itemIds, List<string> itemTypes)
        {
            if (itemIds == null || itemTypes == null)
            {
                Debug.LogWarning("[ShelterModule_Sorter] ProcessAirlockDump called with null list.");
                return;
            }

            int count = Mathf.Min(itemIds.Count, itemTypes.Count);
            for (int i = 0; i < count; i++)
            {
                SortItem(itemIds[i], itemTypes[i]);
            }

            // Bulk automation eliminates hauling fatigue for all involved
            if (count > 0)
            {
                OnHaulingFatigueEliminated?.Invoke();
            }
        }

        public SorterState CaptureState()
        {
            return new SorterState
            {
                moduleId = _state.moduleId,
                isActive = _state.isActive
            };
        }

        public void RestoreState(SorterState state)
        {
            _state = state ?? new SorterState();
        }
    }
}
