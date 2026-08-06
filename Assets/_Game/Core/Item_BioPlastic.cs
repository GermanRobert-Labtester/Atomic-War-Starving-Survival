using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BioPlasticState
    {
        public string itemId = "item_bio_plastic";
        public float aestheticAuraPenalty = -0.3f;
        public bool replacesPlasticScrap = true;
    }

    public class Item_BioPlastic
    {
        public event Action<string, float> OnAestheticPenalty;
        public event Action<string> OnBioPlasticCrafted;

        private BioPlasticState _state;

        public Item_BioPlastic()
        {
            _state = new BioPlasticState();
        }

        public Item_BioPlastic(BioPlasticState state)
        {
            _state = state ?? new BioPlasticState();
        }

        public BioPlasticState CaptureState() => _state;

        public void RestoreState(BioPlasticState state)
        {
            _state = state ?? new BioPlasticState();
        }

        public void Craft(string survivorId)
        {
            OnBioPlasticCrafted?.Invoke(survivorId);
        }

        public void StoreInRoom(string roomId)
        {
            OnAestheticPenalty?.Invoke(roomId, _state.aestheticAuraPenalty);
        }

        public bool CanReplace(string itemId)
        {
            return itemId == "item_plastic_scrap";
        }
    }
}
