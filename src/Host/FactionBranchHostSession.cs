// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.Flags;
using Godot;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for FactionBranchCoordinator ("The Weight of Choices").
    /// Thin Godot host layer exposing coordinator actions to Factions and Quests panels.
    /// </summary>
    public sealed class FactionBranchHostSession : HostSessionBase
    {
        public FactionBranchCoordinator Coordinator { get; }

        public FactionBranchHostSession(FactionBranchCoordinator coordinator)
        {
            Coordinator = coordinator ?? throw new ArgumentNullException(nameof(coordinator));
            Coordinator.OnStateChanged += () => RaiseStateChanged();
        }

        public static FactionBranchHostSession CreateDefault(string dataDir, IFlagLedger? flags = null)
        {
            var coordinator = FactionBranchCoordinator.LoadFromData(
                dataDir,
                new FileSystemIO(),
                new SystemTextJsonSerializer(),
                flags ?? new CampaignConsequenceLedger(),
                new GodotLog());
            return new FactionBranchHostSession(coordinator);
        }

        public bool TrySave()
        {
            return WeightOfChoicesSaveStore.TrySave(Coordinator.CaptureState());
        }

        public bool TryLoad()
        {
            var loaded = WeightOfChoicesSaveStore.TryLoad();
            if (loaded == null) return false;
            Coordinator.RestoreState(loaded);
            return true;
        }

        public override void Save()
        {
            if (!IsDirty) return;
            TrySave();
            base.Save();
        }
    }
}
