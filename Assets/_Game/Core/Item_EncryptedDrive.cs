using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class EncryptedDriveState
    {
        public string itemId = "item_encrypted_drive";
        public string displayName = "Encrypted Military Drive";
        public bool isDecrypted = false;
        public bool revealsFogOfWarAndSecretBunkers = true;
    }

    /// <summary>
    /// Prompt #461: Artifact: Encrypted Military Drive.
    /// Requires a High-Tier Computer Terminal module (#200) to decrypt.
    /// Once decrypted, clears all fog of war on the world map and reveals hidden secret bunker nodes.
    /// </summary>
    public class Item_EncryptedDrive
    {
        private EncryptedDriveState _state = new EncryptedDriveState();

        public event Action<EncryptedDriveState> OnDriveDecryptedMapRevealed;

        public EncryptedDriveState State => _state;

        public bool DecryptDrive(bool hasComputerTerminal)
        {
            if (hasComputerTerminal && !_state.isDecrypted)
            {
                _state.isDecrypted = true;
                OnDriveDecryptedMapRevealed?.Invoke(_state);
                return true;
            }
            return false;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public EncryptedDriveState CaptureState() => _state;

        public void RestoreState(EncryptedDriveState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
