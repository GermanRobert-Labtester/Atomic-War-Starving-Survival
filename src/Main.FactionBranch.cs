// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Godot;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private FactionBranchHostSession _factionBranch = null!;
        private bool _factionBranchDirty;
        private CounterIntelligenceHostSession _counterIntelligence = null!;
        private bool _counterIntelligenceDirty;
        private InformantNetworkHostSession? _informantNetwork;
        private bool _informantNetworkDirty;

        private void SetupFactionBranch()
        {
            if (_factionBranch != null) return;

            // Plan 26A — resolve through the one data-path authority.
            string dataDir = CatalogPath.ResolveDataDir();

            _factionBranch = FactionBranchHostSession.CreateDefault(dataDir, flags: _consequenceLedger);
            _factionBranch.StateChanged += () => _factionBranchDirty = true;

            if (_factionBranch.TryLoad())
            {
                _factionBranchDirty = false;
            }
        }

        private void SaveFactionBranch()
        {
            if (_factionBranch != null)
            {
                if (CaptureSection("weight_of_choices", WeightOfChoicesSaveStore.TryCapturePersisted(_factionBranch.Coordinator.CaptureState())))
                    _factionBranchDirty = false;
            }
        }

        private void SetupCounterIntelligence()
        {
            if (_counterIntelligence != null) return;
            var state = CounterIntelligenceSaveStore.TryLoad() ?? new CounterIntelligenceState();
            var system = new CounterIntelligenceSystem(state, new SeededRng(2001), new GodotLog());
            _counterIntelligence = new CounterIntelligenceHostSession(system);
            _counterIntelligence.LoadCatalog(_dataDir);
            _counterIntelligence.StateChanged += () => _counterIntelligenceDirty = true;
        }

        private void SaveCounterIntelligence()
        {
            if (_counterIntelligence != null)
            {
                if (CaptureSection("counter_intelligence", CounterIntelligenceSaveStore.TryCapturePersisted(_counterIntelligence.System.CaptureState())))
                    _counterIntelligenceDirty = false;
            }
        }

        /// <summary>
        /// Plan 146 batch-4 / A.83: informant field-ops ledger. Own session,
        /// own save section; the counter-intelligence system keeps its
        /// shelter-side vetting concern untouched.
        /// </summary>
        private void SetupInformantNetwork()
        {
            if (_informantNetwork != null) return;
            _informantNetwork = InformantNetworkHostSession.Create();
            _informantNetwork.StateChanged += () => _informantNetworkDirty = true;
        }

        private void SaveInformantNetwork()
        {
            if (_informantNetwork == null) return;
            if (CaptureSection("informant_network", InformantNetworkSaveStore.TryCapturePersisted(_informantNetwork.System.CaptureState())))
                _informantNetworkDirty = false;
        }

        private void FlushInformantNetwork()
        {
            if (_informantNetworkDirty) SaveInformantNetwork();
        }

        private void FlushFactionBranch()
        {
            if (_factionBranchDirty)
                SaveFactionBranch();
        }

        public bool CommitFactionBranch(string branchId)
        {
            SetupFactionBranch();
            SetupMoralChoice();
            if (_factionBranch == null || _moralChoice == null) return false;
            var result = _factionBranch.Coordinator.CommitBranch(branchId, _moralChoice);
            if (result.IsSuccess)
            {
                _factionBranchDirty = true;
                _moralChoiceDirty = true;
                AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayCue(AtomicWar.GodotApp.Audio.AudioCueCatalog.UiConfirm);
                return true;
            }
            return false;
        }

        private void TickFactionBranchDay(int day)
        {
            SetupFactionBranch();
            _factionBranch?.Coordinator.TickDay(day);
            _factionBranchDirty = true;
        }
    }
}
