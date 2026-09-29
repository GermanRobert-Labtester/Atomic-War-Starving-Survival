// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : BioFermentationHostSession
// Core Source  : Ashfall.Core.Shelter.BioFermentationEngine
// Purpose      : Thin Godot presentation adapter — raises LastEvent feedback
//                and state-change notifications; owns NO gameplay logic.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public sealed class BioFermentationHostSession : HostSessionBase
    {
        public BioFermentationEngine System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        // W2-06 Decision Point 1 · Path B — authored fermentation assay log,
        // revived through the IFileIO port (never the raw-FS Core wrapper,
        // which is silently empty in an exported build). Read-only: the panel
        // renders these lines and owns no state of its own.
        private readonly List<AssayLogLine> _fieldLog = new List<AssayLogLine>();
        private readonly List<string> _fieldLogProblems = new List<string>();

        /// <summary>Authored assay-log lines, or empty when no file was read.</summary>
        public IReadOnlyList<AssayLogLine> FieldLog => _fieldLog;

        /// <summary>Why a file was skipped, for diagnostics. Never a silent empty.</summary>
        public IReadOnlyList<string> FieldLogProblems => _fieldLogProblems;

        /// <summary>Loads the authored assay log through the I/O port. Idempotent.</summary>
        public void LoadFieldLog(string? dataDir, IFileIO? io, Ashfall.Core.IJsonSerializer? serializer)
        {
            _fieldLog.Clear();
            _fieldLogProblems.Clear();
            if (string.IsNullOrWhiteSpace(dataDir) || io == null || serializer == null) return;
            _fieldLog.AddRange(NarrativeAssayLogCatalogLoader.Load(
                dataDir, io, serializer, NarrativeAssayLogCatalogs.Fermentation(), _fieldLogProblems));
        }

        public BioFermentationHostSession(BioFermentationEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));

            System.OnBatchStarted += processId =>
            {
                LastEvent = $"Fermentation batch started ({processId}).";
                RaiseStateChanged();
            };
            System.OnBatchCompleted += processId =>
            {
                LastEvent = $"Fermentation batch complete — output ready to harvest ({processId}).";
                RaiseStateChanged();
            };
            System.OnBatchAborted += reason =>
            {
                LastEvent = $"Fermentation batch aborted ({reason}).";
                RaiseStateChanged();
            };
            System.OnContaminationEvent += (before, after) =>
            {
                LastEvent = after == "spoiled"
                    ? "WARNING — the fermentation batch has spoiled. Service the reactor to clear it."
                    : after == "contaminated"
                        ? "Contamination detected — expected yield quality has dropped."
                        : "Trace contamination detected. Process health is degrading.";
                RaiseStateChanged();
            };
            System.OnFaultChange += fault =>
            {
                if (fault == "power_loss")
                    LastEvent = "Fermenter bay power lost — batch stalled.";
                else if (fault == "filter_clogged")
                    LastEvent = "Fermenter filter clogged — maintenance required.";
                RaiseStateChanged();
            };
            System.OnStateChanged += () => RaiseStateChanged();
        }

        public void SetLastEvent(string message) => LastEvent = message;
    }
}
