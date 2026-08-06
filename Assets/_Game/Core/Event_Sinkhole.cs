using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class FoundationSinkholeState
    {
        public string eventId;
        public int minLateralRooms = 8;
        public int minEarthWallsRequired = 3;
        public bool isTriggered;
        public string collapsedRoomId = string.Empty;
    }

    public class FoundationSinkholeSystem
    {
        private readonly FoundationSinkholeState _state;

        public FoundationSinkholeState State => _state;

        public event Action<string, string> OnCollapseTriggered;  // eventId, collapsedRoomId

        public FoundationSinkholeSystem(string eventId)
        {
            _state = new FoundationSinkholeState
            {
                eventId = eventId,
                minLateralRooms = 8,
                minEarthWallsRequired = 3,
                isTriggered = false,
                collapsedRoomId = string.Empty
            };
        }

        /// <summary>
        /// Check foundation stability. Returns true if sinkhole conditions met.
        /// </summary>
        public bool CheckFoundation(int lateralRooms, int earthWalls, Random rng)
        {
            if (lateralRooms < _state.minLateralRooms)
                return false;
            if (earthWalls >= _state.minEarthWallsRequired)
                return false;

            // Foundation is unstable — random chance to trigger
            if (rng.NextDouble() < 0.25)
                return true;

            return false;
        }

        /// <summary>
        /// Collapse a surface room into the level below, crushing everything.
        /// </summary>
        public void TriggerCollapse(string roomId, Action<string> crushRoom)
        {
            if (_state.isTriggered)
                return;

            _state.isTriggered = true;
            _state.collapsedRoomId = roomId;
            crushRoom?.Invoke(roomId);
            OnCollapseTriggered?.Invoke(_state.eventId, roomId);
        }
    }
}
