using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    [CreateAssetMenu(fileName = "WorkbenchModule", menuName = "ASHFALL/Shelter/Workbench Module")]
    public class WorkbenchModuleSO : ShelterModule
    {
        [Header("Workbench Parameters")]
        public float CraftingSpeedMultiplier = 1f;
    }
}
