using System;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class TrashHazmatState
    {
        public string itemId = "item_trash_hazmat";
        public float radProtection = 0.4f;
        public float tearChance = 0.25f;
        public bool isTorn;
    }

    public class Item_TrashHazmat
    {
        public event Action<string> OnHazmatTorn;
        public event Action<string, float> OnRadProtectionApplied;

        private TrashHazmatState _state;

        public Item_TrashHazmat()
        {
            _state = new TrashHazmatState();
        }

        public Item_TrashHazmat(TrashHazmatState state)
        {
            _state = state ?? new TrashHazmatState();
        }

        public TrashHazmatState CaptureState() => _state;

        public void RestoreState(TrashHazmatState state)
        {
            _state = state ?? new TrashHazmatState();
        }

        public bool PerformPhysicalAction(string survivorId, string actionType, Random rng)
        {
            if (_state.isTorn)
                return true;

            bool isPhysical = actionType == "vaulting"
                           || actionType == "melee"
                           || actionType == "climbing"
                           || actionType == "sprinting";

            if (isPhysical)
            {
                double roll = rng.NextDouble();
                if (roll < _state.tearChance)
                {
                    _state.isTorn = true;
                    OnHazmatTorn?.Invoke(survivorId);
                    return true;
                }
            }

            return false;
        }

        public float GetProtection(string survivorId)
        {
            if (_state.isTorn)
                return 0f;

            OnRadProtectionApplied?.Invoke(survivorId, _state.radProtection);
            return _state.radProtection;
        }

        public void Repair(string survivorId)
        {
            _state.isTorn = false;
        }
    }
}
