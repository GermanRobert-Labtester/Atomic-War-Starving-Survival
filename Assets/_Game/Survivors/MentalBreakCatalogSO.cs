using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Designer-facing catalog of <see cref="MentalBreakSO"/> assets. The
    /// GameBootstrap registers every break in this catalog on init. Empty
    /// catalog = survivors never roll for a break (safe default).
    /// </summary>
    [CreateAssetMenu(fileName = "MentalBreakCatalog", menuName = "ASHFALL/Survivor/Mental Break Catalog")]
    public class MentalBreakCatalogSO : ScriptableObject
    {
        public List<MentalBreakSO> breaks = new List<MentalBreakSO>();
    }
}
