// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private RelationshipDecayHostSession _relationshipDecay = null!;
        private RelationshipDecayPanel _relationshipDecayPanel = null!;
        private bool _relationshipDecayDirty;

        public RelationshipDecayHostSession RelationshipDecay => EnsureRelationshipDecay();

        public RelationshipDecayHostSession EnsureRelationshipDecay()
        {
            if (_relationshipDecay != null) return _relationshipDecay;

            var state = RelationshipDecaySaveStore.TryLoad() ?? new RelationshipDecayState();
            var system = new RelationshipDecaySystem(state);

            // Plan 182 — bind the authored bond decay profiles so drift/reconnection
            // tuning comes from the canonical catalog, not hardcoded defaults.
            string decayCatalogPath = CatalogPath.ResolveCatalog("relationship_decay_profiles.json");
            var decayCatalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (decayCatalogIo.FileExists(decayCatalogPath))
            {
                system.LoadCatalog(decayCatalogIo.ReadAllText(decayCatalogPath));
            }

            _relationshipDecay = new RelationshipDecayHostSession(system);
            _relationshipDecay.StateChanged += OnRelationshipDecayStateChanged;
            return _relationshipDecay;
        }

        private void OnRelationshipDecayStateChanged()
        {
            _relationshipDecayDirty = true;
        }

        /// <summary>
        /// Feed a real survivor-to-survivor interaction into the decay owner so
        /// pairs exist and neglect timers reset. Unknown or blank ids are ignored.
        /// </summary>
        private void RecordRelationshipInteraction(string survivorA, string survivorB, string interactionType, float affinityBonus)
        {
            if (string.IsNullOrWhiteSpace(survivorA) || string.IsNullOrWhiteSpace(survivorB)) return;
            if (string.Equals(survivorA, survivorB, StringComparison.Ordinal)) return;
            EnsureRelationshipDecay().RecordInteraction(survivorA, survivorB, interactionType, affinityBonus, _simDay);
            _relationshipDecayDirty = true;
        }

        private void FeedRelationshipDecayFromConflict(MediationEntry entry)
        {
            if (entry == null || _survivorRelationsCore == null) return;
            var conflict = _survivorRelationsCore.State.activeConflicts.Find(c =>
                c != null && string.Equals(c.conflictId, entry.conflictId, StringComparison.Ordinal));
            if (conflict == null) return;
            RecordRelationshipInteraction(conflict.dwellerA, conflict.dwellerB, "mediated_conflict", entry.affinityChange);
        }

        public void TickRelationshipDecay(int day)
        {
            EnsureRelationshipDecay();
            _relationshipDecay.TickDay(day);
            _relationshipDecayDirty = true;
        }

        private void SetupRelationshipDecay()
        {
            EnsureRelationshipDecay();
        }

        private void SaveRelationshipDecay()
        {
            var session = EnsureRelationshipDecay();
            if (session != null)
            {
                CaptureSection("relationship_decay", RelationshipDecaySaveStore.TryCapturePersisted(session.System.CaptureState()));
                _relationshipDecayDirty = false;
            }
        }

        private void FlushRelationshipDecayIfDirty()
        {
            if (_relationshipDecayDirty) SaveRelationshipDecay();
        }

        private void SetupRelationshipDecayPanel()
        {
            if (_relationshipDecayPanel != null && _relationshipDecayPanel.IsInsideTree())
                return;

            EnsureRelationshipDecay();
            _relationshipDecayPanel = new RelationshipDecayPanel();
            _relationshipDecayPanel.Bind(_relationshipDecay);
            _relationshipDecayPanel.OnClose += () => _relationshipDecayPanel.Visible = false;
            _relationshipDecayPanel.Visible = false;
            AddChild(_relationshipDecayPanel);
        }

        public void ShowRelationshipDecayPanel()
        {
            SetupRelationshipDecayPanel();
            ShowPanelLifecycle(_relationshipDecayPanel);
            _relationshipDecayPanel.RefreshView();
        }
    }
}
