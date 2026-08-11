using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class IBeamState
    {
        public string itemId = "item_i_beam";
        public string displayName = "Steel I-Beam";
        public float weight = 100f;
        public bool requiresVehicle = true;
        public bool requiresPortableWinch = true;
        public int stackMax = 1;
    }

    /// <summary>
    /// Prompt #587: Item: Steel I-Beam.
    /// Required for all Mega-Projects. Weighs 100kg. Cannot be carried by a single human.
    /// Requires Vehicle or PortableWinch to transport.
    /// </summary>
    public class Item_IBeam
    {
        private IBeamState _state = new IBeamState();

        public event Action<IBeamState> OnIBeamPickedUp;
        public event Action<IBeamState, string> OnPickupFailed;

        public IBeamState State => _state;

        public bool CanPickup(bool hasVehicle, bool hasPortableWinch, float survivorStrength)
        {
            if (hasVehicle || hasPortableWinch)
                return true;

            OnPickupFailed?.Invoke(_state, "Requires Vehicle or Portable Winch");
            return false;
        }

        public float GetEffectiveWeight()
        {
            return _state.weight;
        }

        public bool Pickup(bool hasVehicle, bool hasPortableWinch, float survivorStrength)
        {
            if (!CanPickup(hasVehicle, hasPortableWinch, survivorStrength))
                return false;

            OnIBeamPickedUp?.Invoke(_state);
            return true;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public IBeamState CaptureState() => _state;

        public void RestoreState(IBeamState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
