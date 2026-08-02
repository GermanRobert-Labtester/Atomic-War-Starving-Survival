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

    /// <summary>A single radio broadcast: id, day window, and message text.</summary>
    [CreateAssetMenu(fileName = "NewRadioBroadcast", menuName = "ASHFALL/Data/Radio Broadcast")]
    public class RadioBroadcastSO : ScriptableObject
    {
        public string id;
        public int minDay;
        public int maxDay = -1;
        [TextArea(3, 6)] public string message;
    }
}
