// SPDX-License-Identifier: MIT
using System;
using Godot;
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
            _relationshipDecayPanel.Visible = true;
            _relationshipDecayPanel.RefreshView();
        }
    }
}
