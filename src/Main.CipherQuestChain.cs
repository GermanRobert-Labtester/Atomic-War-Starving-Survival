// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Radio;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plan 11 / 251 — cipher puzzle chains. Own session, own save section
    /// (<c>cipher_quest_chain</c>); the radio station keeps its own intercept
    /// authority and the wasteland map keeps its own discovery authority.
    /// Live gameplay feeds the engine from those two owners: radio intercepts
    /// whose id matches a chain's broadcast/cipher-station id are recorded as
    /// heard, and inventory acquisitions of a chain's required key item are
    /// recorded as the key.
    /// </summary>
    public sealed partial class Main
    {
        private CipherQuestChainHostSession? _cipherQuestChain;
        private bool _cipherQuestChainDirty;
        private bool _cipherRadioBound;

        public CipherQuestChainHostSession? CipherQuestChain => _cipherQuestChain;

        public void SetupCipherQuestChain()
        {
            if (_cipherQuestChain != null) return;

            _cipherQuestChain = CipherQuestChainHostSession.Create();
            _cipherQuestChain.StateChanged += () => _cipherQuestChainDirty = true;

            // Restore before binding the map so restored reveals replay onto it.
            if (_cipherQuestChain.TryLoad())
            {
                _cipherQuestChainDirty = false;
            }

            BindCipherWastelandMap();
            BindCipherToRadioStation();
        }

        /// <summary>Composition root: the map authority must exist before binds.</summary>
        private void BindCipherWastelandMap()
        {
            if (_cipherQuestChain == null) return;
            _cipherQuestChain.BindMap(_world?.WastelandMap);
        }

        /// <summary>
        /// Routes the live radio station's intercept detections into cipher
        /// chains. Subscribes at most once; presentation of the result stays in
        /// the panel/host feedback strip, all rules stay in Core.
        /// </summary>
        private void BindCipherToRadioStation()
        {
            if (_cipherRadioBound || _cipherQuestChain == null) return;
            if (_radioStationSystem == null) return;

            _cipherRadioBound = true;
            _radioStationSystem.OnInterceptDetected += interceptId =>
            {
                if (string.IsNullOrEmpty(interceptId)) return;
                _cipherQuestChain.RecordBroadcastHeard(interceptId);
                _cipherQuestChainDirty = true;
            };
        }

        public bool RecordCipherBroadcastHeard(string broadcastOrStationId)
        {
            SetupCipherQuestChain();
            if (_cipherQuestChain == null) return false;
            _cipherQuestChain.RecordBroadcastHeard(broadcastOrStationId);
            _cipherQuestChainDirty = true;
            return true;
        }

        public bool RecordCipherKeyAcquired(string itemId)
        {
            SetupCipherQuestChain();
            if (_cipherQuestChain == null) return false;
            _cipherQuestChain.RecordKeyAcquired(itemId);
            _cipherQuestChainDirty = true;
            return true;
        }

        public bool MarkCipherChainResolved(string chainId)
        {
            SetupCipherQuestChain();
            if (_cipherQuestChain == null) return false;
            _cipherQuestChain.MarkResolved(chainId);
            _cipherQuestChainDirty = true;
            return true;
        }

        public (int Total, int Heard, int Decoded, int Revealed, int Resolved)? GetCipherChainCensus()
        {
            SetupCipherQuestChain();
            if (_cipherQuestChain == null) return null;
            return _cipherQuestChain.GetCensus();
        }

        public void SaveCipherQuestChain()
        {
            if (_cipherQuestChain == null) return;
            if (CaptureSection(
                    "cipher_quest_chain",
                    CipherQuestChainSaveStore.TryCapturePersisted(_cipherQuestChain.CaptureSave())))
            {
                _cipherQuestChainDirty = false;
            }
        }

        private void ResetCipherQuestChain()
        {
            _cipherQuestChain = null;
            _cipherQuestChainDirty = false;
            _cipherRadioBound = false;
        }
    }
}
