using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Economy
{
    [Serializable]
    public class CaravanInventoryItem : Ashfall.Core.CaravanInventoryItem { }

    [Serializable]
    public class CaravanEntry : Ashfall.Core.CaravanEntry { }

    [Serializable]
    public class TravelingCaravanState : Ashfall.Core.TravelingCaravanState { }

    /// <summary>
    /// Legacy Unity wrapper forwarding to Ashfall.Core.TravelingCaravanSystem.
    /// </summary>
    public class TravelingCaravanSystem : Ashfall.Core.TravelingCaravanSystem
    {
        public TravelingCaravanSystem(Ashfall.Core.TravelingCaravanState state = null) : base(state) { }
    }
}
