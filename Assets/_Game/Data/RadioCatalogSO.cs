using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// ScriptableObject catalog of radio broadcasts / signal logs; imported from
    /// StreamingAssets/Data/radio.json. Ambient narrative and hints; no gameplay
    /// effect by default.
    /// </summary>
    [CreateAssetMenu(fileName = "NewRadioCatalog", menuName = "ASHFALL/Data/Radio Catalog")]
    public class RadioCatalogSO : ScriptableObject
    {
        public List<RadioBroadcastSO> broadcasts = new List<RadioBroadcastSO>();
    }
}
