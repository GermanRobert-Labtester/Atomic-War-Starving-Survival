using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    public enum MusicGenre
    {
        Jazz,
        Classical,
        Blues
    }

    [Serializable]
    public class VinylCollectionState
    {
        public string itemId = "item_vinyl_collection";
        public string displayName = "Rare Vinyl Collection";
        public MusicGenre currentTrack = MusicGenre.Jazz;
        public float jazzSleepBonus = 0.30f;      // +30% sleep quality
        public float classicalCraftingBonus = 0.20f; // +20% crafting speed
    }

    /// <summary>
    /// Prompt #462: Artifact: Rare Vinyl Collection.
    /// Used with the RecordPlayer (#440).
    /// Each vinyl genre unlocks unique passive mood buffs for bunker residents (Jazz = +Sleep, Classical = +CraftingSpeed).
    /// </summary>
    public class Item_VinylCollection
    {
        private VinylCollectionState _state = new VinylCollectionState();

        public event Action<VinylCollectionState, MusicGenre> OnTrackPlayedMoodBuffed;

        public VinylCollectionState State => _state;

        public void PlayGenreTrack(MusicGenre genre)
        {
            _state.currentTrack = genre;
            OnTrackPlayedMoodBuffed?.Invoke(_state, genre);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public VinylCollectionState CaptureState() => _state;

        public void RestoreState(VinylCollectionState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
