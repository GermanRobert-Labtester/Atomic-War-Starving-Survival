using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>A single radio broadcast: id, day window, message text, and
    /// optional <see cref="triggerEventId"/> linking the broadcast to a
    /// GameEvent in the EventRunner pool (Prompt #46). When a broadcast with
    /// a <c>triggerEventId</c> plays while a survivor is at the radio, the
    /// bridge in GameBootstrap raises the named event so the player can
    /// interact with it (send an expedition, analyze the audio, ignore).
    ///
    /// Lives in its own file because Unity only links a MonoScript to an asset
    /// when the type's file name matches the class name. Declared inside
    /// RadioCatalogSO.cs it serialized with m_Script: {fileID: 0}, which made
    /// every generated broadcast unresolvable by type.</summary>
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
