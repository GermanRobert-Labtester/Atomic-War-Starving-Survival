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

    /// <summary>A single radio broadcast: id, day window, message text, and
    /// optional <see cref="triggerEventId"/> linking the broadcast to a
    /// GameEvent in the EventRunner pool (Prompt #46). When a broadcast with
    /// a <c>triggerEventId</c> plays while a survivor is at the radio, the
    /// bridge in GameBootstrap raises the named event so the player can
    /// interact with it (send an expedition, analyze the audio, ignore).</summary>
    [CreateAssetMenu(fileName = "NewRadioBroadcast", menuName = "ASHFALL/Data/Radio Broadcast")]
    public class RadioBroadcastSO : ScriptableObject
    {
        public string id;
        public int minDay;
        public int maxDay = -1;
        [TextArea(3, 6)] public string message;

        /// <summary>Snake_case id of a <see cref="AtomicWar._Game.Events.GameEvent"/>
        /// in the EventRunner pool. When set and a survivor is at the radio
        /// (EventContext.IsOnRadio), the broadcast raises the named event
        /// through the standard EventRunner.Run path. Empty = no event
        /// (ambient flavor broadcast).</summary>
        public string triggerEventId;
    }
}
