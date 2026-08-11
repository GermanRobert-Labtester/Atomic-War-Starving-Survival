using System;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class ImpureIodineState
    {
        public string itemId = "item_impure_iodine";
        public float radReduction = 0.6f;
        public float toxicityHours = 12f;
        public bool isConsumed;
    }

    public class Item_ImpureIodine
    {
        public event Action<string, float> OnRadReduced;
        public event Action<string, float> OnToxicityApplied;

        private ImpureIodineState _state;

        public Item_ImpureIodine()
        {
            _state = new ImpureIodineState();
        }

        public Item_ImpureIodine(ImpureIodineState state)
        {
            _state = state ?? new ImpureIodineState();
        }

        public ImpureIodineState CaptureState() => _state;

        public void RestoreState(ImpureIodineState state)
        {
            _state = state ?? new ImpureIodineState();
        }

        public float Consume(string survivorId, float currentRad)
        {
            if (_state.isConsumed)
                return currentRad;

            float reduction = currentRad * _state.radReduction;
            float remainingRad = currentRad - reduction;

            _state.isConsumed = true;

            OnRadReduced?.Invoke(survivorId, reduction);
            OnToxicityApplied?.Invoke(survivorId, _state.toxicityHours);

            return remainingRad;
        }
    }
}
