using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class DogTagsState
    {
        public string itemId = "item_dog_tags";
        public string displayName = "Fallen Soldier Dog Tags";
        public float karmaGainOnReturn = 30f;
        public float factionRepGainOnReturn = 40f;
        public int scrapValueOnSell = 15;
    }

    /// <summary>
    /// Prompt #466: Artifact: Fallen Soldier Dog Tags.
    /// Can be returned to the Military Remnants faction for massive Karma and Faction Reputation,
    /// or sold to neutral scavengers for raw scrap metal.
    /// </summary>
    public class Item_DogTags
    {
        private DogTagsState _state = new DogTagsState();

        public event Action<DogTagsState, float, float> OnDogTagsReturnedToMilitary;
        public event Action<DogTagsState, int> OnDogTagsSoldForScrap;

        public DogTagsState State => _state;

        public void ReturnToMilitary(ref float globalKarma, ref float militaryFactionRep)
        {
            globalKarma += _state.karmaGainOnReturn;
            militaryFactionRep += _state.factionRepGainOnReturn;
            OnDogTagsReturnedToMilitary?.Invoke(_state, _state.karmaGainOnReturn, _state.factionRepGainOnReturn);
        }

        public int SellForScrap(ref int scrapStorage)
        {
            scrapStorage += _state.scrapValueOnSell;
            OnDogTagsSoldForScrap?.Invoke(_state, _state.scrapValueOnSell);
            return _state.scrapValueOnSell;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public DogTagsState CaptureState() => _state;

        public void RestoreState(DogTagsState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
