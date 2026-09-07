// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot adapter for the Core plastic-pyrolysis authority (Plan 202).
    /// Presentation only — every mutation routes through the Core system's
    /// commands; feedback mirrors Core results. Never bypasses inventory.
    /// </summary>
    public sealed class PlasticPyrolysisHostSession : HostSessionBase
    {
        public PlasticPyrolysisSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public PlasticPyrolysisHostSession(PlasticPyrolysisSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnBatchCompleted += b =>
            {
                LastEvent = $"Batch {b.batch_id} completed — outputs staged for claim.";
                RaiseStateChanged();
            };
            System.OnIncident += (b, kind) =>
            {
                LastEvent = kind switch
                {
                    "quality_loss" => $"Batch {b.batch_id}: feed contamination fouled the yield.",
                    "machine_damage" => $"Batch {b.batch_id}: retort damage — service the bay.",
                    "fire" => $"FIRE in the retort bay during batch {b.batch_id}!",
                    "gas_release" => $"Batch {b.batch_id}: gas release — ventilation strained.",
                    _ => $"Batch {b.batch_id}: incident ({kind})."
                };
                RaiseStateChanged();
            };
            System.OnFireIncident += machineId =>
            {
                LastEvent = "Retort bay fire! The hazard authority has been notified.";
                RaiseStateChanged();
            };
            System.OnGasRelease += severity =>
            {
                // Ventilation burden is projected by the host wiring (Main), which owns
                // the ventilation session reference. Here we surface it as feedback.
                LastEvent = $"Retort off-gas release (severity {severity:P0}).";
                RaiseStateChanged();
            };
            System.OnOutputsClaimed += ob =>
            {
                LastEvent = $"Claimed outputs from {ob.batch_id}.";
                RaiseStateChanged();
            };
        }

        public ActionResult Construct()
        {
            var res = System.ConstructMachine();
            if (res.IsFailure) LastEvent = "Construction blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public ActionResult Maintain()
        {
            var res = System.PerformMaintenance();
            if (res.IsFailure) LastEvent = "Maintenance blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public ActionResult StartBatch(string profileId)
        {
            var res = System.StartBatch(profileId);
            if (res.IsFailure) LastEvent = "Batch blocked: " + res.FailureCode;
            RaiseStateChanged();
            return res;
        }

        public ClaimedPyrolysisOutputs? Claim()
        {
            var claimed = System.ClaimOutputs();
            LastEvent = claimed == null ? "Nothing staged to claim." : $"Claimed {claimed.batches_claimed} batch(es) of outputs.";
            RaiseStateChanged();
            return claimed;
        }
    }
}
