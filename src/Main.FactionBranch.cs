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

        private void SetupFactionBranch()
        {
            if (_factionBranch != null) return;

            string dataDir = ProjectSettings.GlobalizePath("res://Assets/StreamingAssets/Data");
            if (!System.IO.Directory.Exists(dataDir))
                dataDir = System.IO.Path.Combine(System.AppContext.BaseDirectory, "Assets/StreamingAssets/Data");

            _factionBranch = FactionBranchHostSession.CreateDefault(dataDir);
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

        private void FlushFactionBranch()
        {
            if (_factionBranchDirty)
                SaveFactionBranch();
        }
    }
}
