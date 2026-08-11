using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class PhotoAlbumState
    {
        public string itemId = "item_photo_album";
        public string displayName = "Family Photo Album";
        public int tradeValue = 0; // Zero monetary value
        public float stressDecayReductionRatio = 0.25f; // 25% slower stress decay
        public bool isPlacedOnDesk = false;
    }

    /// <summary>
    /// Prompt #459: Artifact: Family Photo Album.
    /// Has zero monetary trade value. Placing it on a survivor's desk acts as a sentimental Morale anchor,
    /// reducing the rate of stress accumulation by 25%.
    /// </summary>
    public class Item_PhotoAlbum
    {
        private PhotoAlbumState _state = new PhotoAlbumState();

        public event Action<PhotoAlbumState, string> OnPhotoAlbumPlacedOnDesk;

        public PhotoAlbumState State => _state;

        public float PlaceOnDesk(string survivorId)
        {
            _state.isPlacedOnDesk = true;
            OnPhotoAlbumPlacedOnDesk?.Invoke(_state, survivorId);
            return _state.stressDecayReductionRatio;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public PhotoAlbumState CaptureState() => _state;

        public void RestoreState(PhotoAlbumState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
