// SPDX-License-Identifier: MIT
// ASHFALL survivor narrative questline host session (Plan 104 runtime wiring).

using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Quests;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host-side session for the authored survivor arcs in
    /// <c>narrative_questlines.json</c>. Thin projection only: it loads the
    /// catalog, forwards commands to <see cref="NarrativeQuestlineSystem"/>, and
    /// owns persistence. Gameplay rules stay in Core.
    /// <para>
    /// Consequence application is deliberately left to the caller: a resolved
    /// branch reports its <see cref="NarrativeQuestlineBranchDef"/> so Main can
    /// apply morale through the existing narrative morale authority and surface
    /// the granted trait through the arc record, matching how
    /// <c>LatentExpertAwakeningSystem</c> and <c>DesperationSystem</c> record
    /// granted traits rather than mutating a central survivor trait store.
    /// </para>
    /// </summary>
    public sealed class NarrativeQuestlineHostSession : HostSessionBase
    {
        private readonly NarrativeQuestlineSystem _system;

        public NarrativeQuestlineSystem System => _system;

        public NarrativeQuestlineHostSession(
            IJsonSerializer jsonSerializer, IFileIO fileIO, string dataDir, ILog? log = null)
        {
            var defs = NarrativeQuestlineCatalogLoader.LoadEntries(dataDir, fileIO, jsonSerializer);
            _system = new NarrativeQuestlineSystem(defs, log);

            _system.OnArcStarted += _ => RaiseStateChanged();
            _system.OnStageAdvanced += (_, _) => RaiseStateChanged();
            _system.OnBranchChosen += (_, _) => RaiseStateChanged();
            _system.OnArcResolved += _ => RaiseStateChanged();

            var saved = NarrativeQuestlineSaveStore.TryLoad();
            if (saved != null)
                _system.RestoreState(saved);
        }

        public static NarrativeQuestlineHostSession Create(string dataDir, ILog? log = null)
        {
            return new NarrativeQuestlineHostSession(
                new SystemTextJsonSerializer(), new FileSystemIO(), dataDir, log);
        }

        public int DefinitionCount => _system.DefinitionCount;

        public IReadOnlyList<NarrativeQuestlineDef> Definitions => _system.Definitions;

        public NarrativeQuestlineDef? GetDefinitionForSurvivor(string survivorId)
            => _system.GetDefinitionForSurvivor(survivorId);

        public NarrativeQuestlineArcState? GetArc(string survivorId) => _system.GetArc(survivorId);

        public IReadOnlyList<NarrativeQuestlineArcState> Arcs => _system.Arcs;

        public List<string> GetOutstandingObjectives(string survivorId)
            => _system.GetOutstandingObjectives(survivorId);

        public bool IsAwaitingBranch(string survivorId) => _system.IsAwaitingBranch(survivorId);

        public bool TryBegin(string survivorId, int day) => _system.TryBegin(survivorId, day);

        public bool TryDeliverItem(string survivorId, string itemId, int day)
            => _system.TryDeliverItem(survivorId, itemId, day);

        public bool TryChooseBranch(
            string survivorId, string branchId, int day,
            out NarrativeQuestlineBranchDef? chosenBranch)
            => _system.TryChooseBranch(survivorId, branchId, day, out chosenBranch);

        public NarrativeQuestlineSaveState CaptureState() => _system.CaptureState();

        public void RestoreState(NarrativeQuestlineSaveState state)
        {
            _system.RestoreState(state);
            RaiseStateChanged();
        }

        public bool TrySave() => NarrativeQuestlineSaveStore.TrySave(_system.CaptureState());

        public bool TryLoad()
        {
            var loaded = NarrativeQuestlineSaveStore.TryLoad();
            if (loaded == null) return false;
            _system.RestoreState(loaded);
            RaiseStateChanged();
            return true;
        }

        public string TryCapturePersisted() => NarrativeQuestlineSaveStore.TryCapturePersisted(_system.CaptureState());
    }
}
