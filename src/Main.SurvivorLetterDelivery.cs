// SPDX-License-Identifier: MIT
// ============================================================================
// ORPHAN-SEAL follow-up — dead-letter delivery composition
//
// The 25 authored survivor letters previously had a catalog and a tested
// delivery lifecycle but no host: nothing loaded them, nothing persisted the
// resolved states, and the morale consequence had no owner. This file is the
// composition root only. Authority stays where it already lives:
//   - death-letter content ........ SurvivorLetterCatalog (authored JSON)
//   - delivery state machine ...... Ashfall.Core.Narrative.SurvivorLetterDeliverySystem
//   - dwellers .................... SurvivorsHostSession roster
//   - morale ...................... NeedsSystem (NeedKind.Morale)
// No panel, cache or parallel ledger holds letter or morale state.
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Godot;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private SurvivorLetterDeliveryHostSession? _survivorLetterDelivery;
        private bool _survivorLetterDeliveryDirty;

        /// <summary>Live dead-letter delivery host. Null until setup.</summary>
        public SurvivorLetterDeliveryHostSession? SurvivorLetterDelivery => _survivorLetterDelivery;

        public SurvivorLetterDeliveryHostSession EnsureSurvivorLetterDelivery()
        {
            if (_survivorLetterDelivery != null) return _survivorLetterDelivery;
            SetupSurvivorLetterDelivery();
            return _survivorLetterDelivery!;
        }

        private void SetupSurvivorLetterDelivery()
        {
            if (_survivorLetterDelivery != null) return;

            var files = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var session = SurvivorLetterDeliveryHostSession.Create(files, new SystemTextJsonSerializer());
            var saved = SurvivorLetterDeliverySaveStore.TryLoad();
            if (saved != null) session.System.RestoreState(saved);

            session.LetterDelivered += (_, _, _) =>
            {
                _survivorLetterDeliveryDirty = true;
                _journal?.TryAddRawEntry(
                    "survivor_letter_delivered",
                    "A letter found in the holdfast dead-letter pigeonholes finally reached its kin.",
                    null!,
                    _simDay);
            };
            session.StateChangedHook = () => _survivorLetterDeliveryDirty = true;

            _survivorLetterDelivery = session;

            var (catalog, found, addressed, delivered, withheld, unanswered) = session.GetCensus();
            if (catalog > 0)
                GD.Print($"[Letters] {catalog} authored letters · {found} found · {addressed} addressed · " +
                         $"{delivered} delivered · {withheld} withheld · {unanswered} unanswered.");
        }

        private void SaveSurvivorLetterDelivery()
        {
            var session = _survivorLetterDelivery;
            if (session == null) return;
            try
            {
                CaptureSection(
                    SurvivorLetterDeliverySaveStore.SectionName,
                    SurvivorLetterDeliverySaveStore.TryCapturePersisted(session.System.CaptureState()));
                _survivorLetterDeliveryDirty = false;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Letters] Failed to save letter delivery section: {ex.Message}");
            }
        }

        private void ResetSurvivorLetterDelivery()
        {
            _survivorLetterDelivery = null;
            _survivorLetterDeliveryDirty = false;
        }

        // ── Player-facing commands ────────────────────────────────────────

        /// <summary>Marks an authored letter as found in the holdfast pigeonholes.</summary>
        public bool FindSurvivorLetter(string letterId)
        {
            var session = EnsureSurvivorLetterDelivery();
            return session.MarkFound(letterId, _simDay);
        }

        /// <summary>Attempts to address a found letter to a living dweller.</summary>
        public bool AddressSurvivorLetter(string letterId) =>
            EnsureSurvivorLetterDelivery().TryAddressToSurvivor(letterId, _survivors);

        /// <summary>Assigns the recipient explicitly; the dweller still owns identity.</summary>
        public bool DeliverSurvivorLetterTo(string letterId, string survivorId)
        {
            var session = EnsureSurvivorLetterDelivery();
            if (!session.AssignRecipientExplicit(letterId, survivorId)) return false;
            return session.Deliver(letterId, _simDay, _survivors);
        }

        /// <summary>Delivers an addressed letter, applying the morale consequence.</summary>
        public bool DeliverSurvivorLetter(string letterId) =>
            EnsureSurvivorLetterDelivery().Deliver(letterId, _simDay, _survivors);

        /// <summary>Withholds an addressed letter, applying the morale consequence.</summary>
        public bool WithholdSurvivorLetter(string letterId) =>
            EnsureSurvivorLetterDelivery().Withhold(letterId, _simDay, _survivors);

        /// <summary>Truthful delivery census for panels and probes.</summary>
        public (int Catalog, int Found, int Addressed, int Delivered, int Withheld, int Unanswered)
            GetSurvivorLetterCensus() => EnsureSurvivorLetterDelivery().GetCensus();
    }
}
