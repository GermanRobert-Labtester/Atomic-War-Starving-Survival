using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SeedLedgerState
    {
        public string itemId = "item_seed_ledger";
        public string displayName = "Svalbard Seed Ledger";
        public bool isDecrypted = false;
        public int decryptionDaysRequired = 5;
        public int daysSpentDecrypting = 0;
        public float tradeValue = 1000f;
        public bool cropUnlocked = false;
    }

    /// <summary>
    /// Prompt #610: Item: Svalbard Seed Ledger.
    /// Encrypted hard drive. If decrypted by AI Core, yields ultimate Hydroponics:
    /// pre-war Wheat, cures starvation, massive trade value.
    /// </summary>
    public class Item_SeedLedger
    {
        private SeedLedgerState _state = new SeedLedgerState();

        public event Action<SeedLedgerState> OnDecryptionStarted;
        public event Action<SeedLedgerState> OnDecryptionCompleted;
        public event Action<SeedLedgerState, string> OnCropUnlocked;

        public SeedLedgerState State => _state;

        public bool StartDecryption(bool hasAICore)
        {
            if (!hasAICore || _state.isDecrypted)
                return false;

            OnDecryptionStarted?.Invoke(_state);
            return true;
        }

        public void TickDay()
        {
            if (_state.isDecrypted)
                return;

            _state.daysSpentDecrypting++;

            if (_state.daysSpentDecrypting >= _state.decryptionDaysRequired)
            {
                _state.isDecrypted = true;
                _state.cropUnlocked = true;

                OnDecryptionCompleted?.Invoke(_state);
                OnCropUnlocked?.Invoke(_state, "wheat");
            }
        }

        public bool IsDecrypted()
        {
            return _state.isDecrypted;
        }

        public string GetUnlockedCrop()
        {
            return _state.cropUnlocked ? "wheat" : null;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public SeedLedgerState CaptureState() => _state;

        public void RestoreState(SeedLedgerState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
