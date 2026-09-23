// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.Reputation;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private ShelterReputationHostSession _shelterReputation = null!;
        private ShelterReputationPanel _shelterReputationPanel = null!;
        private bool _shelterReputationDirty;

        public ShelterReputationHostSession ShelterReputation => EnsureShelterReputation();

        public ShelterReputationHostSession EnsureShelterReputation()
        {
            if (_shelterReputation != null) return _shelterReputation;

            var state = ShelterReputationSaveStore.TryLoad() ?? new ShelterReputationState();
            var system = new ShelterReputationSystem(state);

            // Plan 207 — bind the authored perception dimensions and public tags so
            // trade multipliers and titles resolve from canonical data.
            string reputationCatalogPath = CatalogPath.ResolveCatalog("reputation_dimensions.json");
            var reputationCatalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (reputationCatalogIo.FileExists(reputationCatalogPath))
            {
                var reputationCatalog = System.Text.Json.JsonSerializer.Deserialize<ReputationCatalogData>(
                    reputationCatalogIo.ReadAllText(reputationCatalogPath),
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (reputationCatalog != null)
                {
                    system.LoadCatalog(reputationCatalog);
                }
            }

            _shelterReputation = new ShelterReputationHostSession(system);
            _shelterReputation.StateChanged += OnShelterReputationStateChanged;
            return _shelterReputation;
        }

        private void OnShelterReputationStateChanged()
        {
            _shelterReputationDirty = true;
        }

        public void TickShelterReputation(int day)
        {
            EnsureShelterReputation();
            _shelterReputation.TickDay(day);
            _shelterReputationDirty = true;
        }

        private void SetupShelterReputation()
        {
            EnsureShelterReputation();
        }

        private void SaveShelterReputation()
        {
            var session = EnsureShelterReputation();
            if (session != null)
            {
                CaptureSection("shelter_reputation", ShelterReputationSaveStore.TryCapturePersisted(session.System.CaptureState()));
                _shelterReputationDirty = false;
            }
        }

        private void SetupShelterReputationPanel()
        {
            if (_shelterReputationPanel != null && _shelterReputationPanel.IsInsideTree())
                return;

            EnsureShelterReputation();
            _shelterReputationPanel = new ShelterReputationPanel();
            _shelterReputationPanel.Bind(_shelterReputation);
            _shelterReputationPanel.OnClose += () => _shelterReputationPanel.Visible = false;
            _shelterReputationPanel.Visible = false;
            AddChild(_shelterReputationPanel);
        }

        public void ShowShelterReputationPanel()
        {
            SetupShelterReputationPanel();
            _shelterReputationPanel.Visible = true;
            _shelterReputationPanel.RefreshView();
        }
    }
}
