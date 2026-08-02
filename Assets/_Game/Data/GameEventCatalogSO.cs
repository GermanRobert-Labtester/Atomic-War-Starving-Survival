using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Events;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// ScriptableObject catalog of scripted game events; imported from
    /// StreamingAssets/Data/events.json.
    /// </summary>
    [CreateAssetMenu(fileName = "NewGameEventCatalog", menuName = "ASHFALL/Data/Game Event Catalog")]
    public class GameEventCatalogSO : ScriptableObject
    {
        public List<GameEvent> events = new List<GameEvent>();

        /// <summary>Look up a game event by its snake_case id.</summary>
        public GameEvent GetById(string id) => throw new System.NotImplementedException();
    }
}
