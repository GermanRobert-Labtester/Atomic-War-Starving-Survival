// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 211 — Internal shelter communication host composition.
// The Core InternalCommunicationSystem remains the sole message/board/intercom
// authority. This partial only composes its catalog, save section, canonical
// campaign day, identity providers, and the existing Shelter Social surface.
// ============================================================================

using System;
using System.Linq;
using Godot;
using Ashfall.Core.Communication;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private InternalCommunicationHostSession? _internalCommunication;
        private bool _internalCommunicationDirty;
        private bool _internalCommunicationSaveBlocked;

        public InternalCommunicationHostSession? InternalCommunication => _internalCommunication;

        public InternalCommunicationHostSession EnsureInternalCommunication()
        {
            SetupInternalCommunication();
            return _internalCommunication!;
        }

        public void SetupInternalCommunication()
        {
            if (_internalCommunication != null) return;

            // Leadership is the canonical author authority. Compose it before
            // binding the identity callback so a fresh campaign can actually
            // enable the leader-only public command; the coordinator is
            // idempotent and remains the existing social owner.
            if (_survivorSocial == null)
                SetupSurvivorSocial();

            InternalCommunicationState? saved = InternalCommunicationSaveStore.TryLoad();
            bool saveLoadBlocked = saved == null && InternalCommunicationSaveStore.Exists;
            if (saveLoadBlocked)
            {
                GD.PrintErr("[InternalCommunication] Saved section could not be restored; preserving it and blocking overwrite until repaired.");
            }

            var system = new InternalCommunicationSystem();
            if (saved != null)
            {
                try
                {
                    system.RestoreState(saved);
                }
                catch (Exception ex)
                {
                    saveLoadBlocked = true;
                    GD.PrintErr("[InternalCommunication] Refusing unsupported or malformed saved state; preserving it and blocking overwrite: " + ex.Message);
                    system = new InternalCommunicationSystem();
                }
            }

            var session = new InternalCommunicationHostSession(
                system,
                survivorExists: id => _survivors?.RosterState?.Any(survivor =>
                    string.Equals(survivor.Id, id, StringComparison.OrdinalIgnoreCase)) == true,
                canAuthor: id => _survivorSocial?.Leadership != null &&
                    (string.Equals(_survivorSocial.Leadership.CurrentLeaderId, id, StringComparison.OrdinalIgnoreCase) ||
                     _survivorSocial.Leadership.IsDesignatedLeader(id)));

            if (!session.TryLoadCatalogFile(CatalogPath.ResolveDataDir(), out string catalogReason))
            {
                GD.PrintErr($"[InternalCommunication] Catalog unavailable ({catalogReason}): {session.CatalogLoadError}");
            }

            _internalCommunication = session;
            _internalCommunicationSaveBlocked = saveLoadBlocked;
            session.System.OnMessagePosted += OnInternalCommunicationMessagePosted;
            GD.Print("[InternalCommunication] Host ready; catalog=" + (session.CatalogReady ? "ready" : "unavailable") + ".");
            _internalCommunication.StateChanged += () => _internalCommunicationDirty = true;
            _internalCommunicationDirty = false;
            BindInternalCommunicationPanel();
        }

        public InternalCommunicationCommandResult PostWaterAdvisory(
            string authorId,
            string? details = null)
        {
            InternalCommunicationHostSession session = EnsureInternalCommunication();
            return session.PostWaterAdvisory(authorId, _simDay, details);
        }

        public void TickInternalCommunication(int day)
        {
            // This is called from the existing Plans 46-49 daily composition
            // seam, after campaign day/roster state is available. Repeated calls
            // at the same day are harmless because Core expiry is set-based.
            if (_internalCommunication == null) return;
            _internalCommunication.TickDay(day);
        }

        public void SaveInternalCommunication()
        {
            if (_internalCommunication == null) return;
            if (_internalCommunicationSaveBlocked)
            {
                GD.PrintErr("[InternalCommunication] Save refused because the existing section could not be restored safely.");
                return;
            }

            InternalCommunicationState state = _internalCommunication.CaptureState();
            bool savedToFile = InternalCommunicationSaveStore.TrySave(state);
            string payload = InternalCommunicationSaveStore.TryCapturePersisted(state);
            bool captured = CaptureSection(InternalCommunicationSaveStore.SectionName, payload);
            if (savedToFile && captured)
            {
                _internalCommunicationDirty = false;
                GD.Print("[InternalCommunication] Section captured.");
            }
        }

        public void FlushInternalCommunicationIfDirty()
        {
            if (_internalCommunicationDirty)
                SaveInternalCommunication();
        }

        public void ResetInternalCommunication()
        {
            if (_internalCommunication != null)
                _internalCommunication.System.OnMessagePosted -= OnInternalCommunicationMessagePosted;
            _internalCommunication?.Dispose();
            _internalCommunication = null;
            _internalCommunicationDirty = false;
            _internalCommunicationSaveBlocked = false;
            BindInternalCommunicationPanel();
        }

        public string GetInternalCommunicationActorId()
        {
            return _survivorSocial?.Leadership?.CurrentLeaderId
                ?? _survivors?.RosterState?.FirstOrDefault()?.Id
                ?? string.Empty;
        }

        public bool CanCurrentActorAuthorInternalCommunication()
        {
            return _internalCommunication?.CanAuthor(GetInternalCommunicationActorId()) == true;
        }

        private void OnInternalCommunicationMessagePosted(CommunicationMessage message)
        {
            // Public notices are journal facts; private mail content never
            // enters the shared journal through this bridge.
            if (!string.IsNullOrEmpty(message.RecipientId)) return;
            _journal?.TryAddRawEntry(
                "internal_communication_notice_posted",
                $"Internal shelter notice posted: {message.Subject}",
                null!,
                _simDay);
        }

        private void BindInternalCommunicationPanel()
        {
            if (_shelterSocialPanel == null) return;
            _shelterSocialPanel.BindCommunications(
                _internalCommunication,
                GetInternalCommunicationActorId(),
                () => _simDay);
        }
    }
}
