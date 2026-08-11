using System;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class WastelandSoapState
    {
        public string itemId = "item_wasteland_soap";
        public float hygieneRestored = 0.5f;
        public float chemicalBurnChance = 0.15f;
    }

    public class Item_WastelandSoap
    {
        public event Action<string, float> OnHygieneRestored;
        public event Action<string> OnChemicalBurn;

        private WastelandSoapState _state;

        public Item_WastelandSoap()
        {
            _state = new WastelandSoapState();
        }

        public Item_WastelandSoap(WastelandSoapState state)
        {
            _state = state ?? new WastelandSoapState();
        }

        public WastelandSoapState CaptureState() => _state;

        public void RestoreState(WastelandSoapState state)
        {
            _state = state ?? new WastelandSoapState();
        }

        public bool Use(string survivorId, float craftQuality, Random rng)
        {
            OnHygieneRestored?.Invoke(survivorId, _state.hygieneRestored);

            if (craftQuality < 0.5f)
            {
                double roll = rng.NextDouble();
                if (roll < _state.chemicalBurnChance)
                {
                    OnChemicalBurn?.Invoke(survivorId);
                    return false;
                }
            }

            return true;
        }
    }
}
