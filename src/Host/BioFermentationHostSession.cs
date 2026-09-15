// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : BioFermentationHostSession
// Core Source  : Ashfall.Core.Shelter.BioFermentationEngine
// Purpose      : Thin Godot presentation adapter — raises LastEvent feedback
//                and state-change notifications; owns NO gameplay logic.
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public sealed class BioFermentationHostSession : HostSessionBase
    {
        public BioFermentationEngine System { get; }
        public string LastEvent { get; private set; } = string.Empty;

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