// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 38 / C1[11] — Generalized commitments, deadlines & consequence routing.
//
// Host composition for Ashfall.Core.Commitments.CommitmentSystem:
//   Setup   — load the authored catalog, restore the registered section.
//   Tick    — CommitmentDayOwner (phase 4) evaluates warnings / misses and
//             drains the system's semantic day events into the briefing.
//   Command — host session exposes RecordProgress / Settle for delivery and
//             action/flag settlement; consequences route through the canonical
//             faction-standing, consequence-ledger and journal owners.
//   Save    — "commitment" section via SaveSectionRegistry + SaveStore.
// ============================================================================
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Commitments;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private CommitmentHostSession? _commitments;
        private bool _commitmentsDirty;

        public CommitmentHostSession? Commitments => _commitments;

        /// <summary>
        /// Constructs the commitment authority on first use. Guarded so both the
        /// fresh-game and restore paths share one construction path.
        /// </summary>
        private CommitmentHostSession EnsureCommitments()
        {
            if (_commitments != null) return _commitments;

            var system = new CommitmentSystem();

            // Catalog authority: commitments.json is the sole source of authored
            // obligations; a missing/corrupt catalog leaves the system empty
            // rather than inventing obligations.
            var loaderIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            var loader = CommitmentCatalogLoader.Load(CatalogPath.ResolveDataDir(), loaderIo, new SystemTextJsonSerializer());
            if (loader.HasErrors)
            {
                for (int i = 0; i < loader.Errors.Count; i++) GD.PrintErr($"[Commitments] {loader.Errors[i]}");
            }
            for (int i = 0; i < loader.Commitments.Count; i++) system.RegisterCommitment(loader.Commitments[i]);

            // Restore the registered section (envelope projection file, or a
            // pre-envelope standalone capture during migration).
            var saved = CommitmentSaveStore.TryLoad();
            if (saved != null) system.RestoreState(saved);

            var session = new CommitmentHostSession(system);
            session.StateChanged += () => _commitmentsDirty = true;

            ConfigureCommitmentObservability(system, session);
            _commitments = session;
            return _commitments;
        }

        private void ConfigureCommitmentObservability(CommitmentSystem system, CommitmentHostSession session)
        {
            system.OnWarningIssued += model =>
            {
                _journal?.TryAddRawEntry(
                    "obligation_warning",
                    $"Deadline approaching: {model.Title} — {model.DaysRemaining} day(s) remain for {model.Counterparty}.",
                    null!, _simDay);
                _commitmentsDirty = true;
            };

            system.OnCommitmentMet += model =>
            {
                _journal?.TryAddRawEntry(
                    "obligation_met",
                    $"Obligation honored: {model.Title} settled with {model.Counterparty}.",
                    null!, _simDay);
                _commitmentsDirty = true;
            };

            system.OnCommitmentMissed += model =>
            {
                _journal?.TryAddRawEntry(
                    "obligation_missed",
                    $"Obligation missed: {model.Title} was not delivered to {model.Counterparty} by day {model.DueDay}.",
                    null!, _simDay);
                _commitmentsDirty = true;
            };

            // Consequence routing: the Core system names a class/target/magnitude;
            // the host applies it to the canonical owner for that concern and
            // records the provenance in the campaign consequence ledger. Never a
            // second standing/flag store.
            system.OnConsequenceRouted += RouteCommitmentConsequence;
        }

        private void RouteCommitmentConsequence(string consequenceClass, string target, int magnitude)
        {
            switch (consequenceClass)
            {
                case "standing_penalty":
                    // Canonical faction standing owner clamps and raises its event.
                    _yearOfAsh?.FactionWar.ModifyStanding(target, magnitude);
                    _consequenceLedger.Increment(
                        "commitment_standing_penalty", magnitude, "commitments",
                        "obligation_missed", _simDay, target);
                    break;

                case "economic_shock":
                    _consequenceLedger.Increment(
                        "commitment_economic_shock", magnitude, "commitments",
                        "obligation_missed", _simDay, target);
                    break;

                case "trigger_crisis":
                    _consequenceLedger.Set(
                        "commitment_crisis:" + target, "commitments",
                        "obligation_missed", _simDay, target);
                    break;

                default:
                    _consequenceLedger.Increment(
                        "commitment_" + consequenceClass, magnitude, "commitments",
                        "obligation_missed", _simDay, target);
                    break;
            }
            _commitmentsDirty = true;
        }

        private void SetupCommitments()
        {
            EnsureCommitments();
        }

        private void SaveCommitments()
        {
            if (_commitments == null) return;
            CaptureSection(
                CommitmentSaveStore.SectionName,
                CommitmentSaveStore.TryCapturePersisted(_commitments.System.CaptureState()));
            _commitmentsDirty = false;
        }

        private void FlushCommitmentsIfDirty()
        {
            if (_commitmentsDirty) SaveCommitments();
        }

        private void ResetCommitments()
        {
            _commitments?.Dispose();
            _commitments = null;
            _commitmentsDirty = false;
        }
    }
}
