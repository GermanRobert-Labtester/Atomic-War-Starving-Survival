// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Narrative;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plan 11 / 251 — host session for the cipher puzzle-chain engine.
    /// The engine is the single authority for chain progress and for which
    /// location a decoded chain reveals; this session only supplies the two
    /// live gameplay facts the engine needs (the wasteland map, so a decode can
    /// discover the authored location) and exposes player-facing entry points.
    /// </summary>
    public sealed class CipherQuestChainHostSession : HostSessionBase
    {
        private readonly CipherQuestChainEngine _engine;
        private WastelandMapSystem? _map;

        public CipherQuestChainEngine Engine => _engine;
        public WastelandMapSystem? WastelandMap => _map;
        public string LastEvent { get; private set; } = string.Empty;

        public CipherQuestChainHostSession(CipherQuestChainEngine engine)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            _engine.OnCipherDecoded += chainId =>
                LastEvent = $"Cipher chain {chainId} decoded.";
            _engine.OnLocationRevealedByCipher += (chainId, locationId) =>
                LastEvent = $"Cipher chain {chainId} revealed {locationId}.";
        }

        public static CipherQuestChainHostSession Create() =>
            new CipherQuestChainHostSession(new CipherQuestChainEngine());

        /// <summary>
        /// Binds the canonical wasteland map so decoded chains discover their
        /// authored target location through the one map authority.
        /// </summary>
        public void BindMap(WastelandMapSystem? map)
        {
            _map = map;
            if (map == null) return;
            // Restore path: already-revealed chains rediscover on bind.
            _engine.RestoreState(_engine.CaptureState(), map);
        }

        /// <summary>A broadcast or cipher station transmission was heard.</summary>
        public bool RecordBroadcastHeard(string broadcastOrStationId)
        {
            _engine.RecordBroadcastHeard(broadcastOrStationId, _map);
            RaiseStateChanged();
            return true;
        }

        /// <summary>The player acquired the key item a chain requires.</summary>
        public bool RecordKeyAcquired(string itemId)
        {
            _engine.RecordKeyAcquired(itemId, _map);
            RaiseStateChanged();
            return true;
        }

        public bool MarkResolved(string chainId)
        {
            _engine.MarkResolved(chainId);
            RaiseStateChanged();
            return true;
        }

        public CipherQuestState GetState(string chainId) => _engine.GetState(chainId);

        public IReadOnlyList<CipherQuestState> GetAllStates() => _engine.CaptureState();

        public List<CipherQuestState> CaptureSave() => _engine.CaptureState();

        public void RestoreSave(List<CipherQuestState>? save)
        {
            _engine.RestoreState(save, _map);
            RaiseStateChanged();
        }

        public bool TrySave() => CipherQuestChainSaveStore.TrySave(CaptureSave());

        public bool TryLoad()
        {
            var save = CipherQuestChainSaveStore.TryLoad();
            if (save == null) return false;
            RestoreSave(save);
            return true;
        }

        /// <summary>Read model: chains, how many are decoded, resolved and revealed.</summary>
        public (int Total, int Heard, int Decoded, int Revealed, int Resolved) GetCensus()
        {
            int total = 0, heard = 0, decoded = 0, revealed = 0, resolved = 0;
            foreach (var s in _engine.CaptureState())
            {
                if (s == null) continue;
                total++;
                if (s.isHeard) heard++;
                if (s.isDecoded) decoded++;
                if (s.isLocationRevealed) revealed++;
                if (s.isResolved) resolved++;
            }
            return (total, heard, decoded, revealed, resolved);
        }
    }
}
