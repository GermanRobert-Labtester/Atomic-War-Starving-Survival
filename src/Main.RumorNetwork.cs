// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.InformationFlow;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private RumorNetworkHostSession _rumorNetwork = null!;
        private RumorBoardPanel _rumorBoardPanel = null!;
        private bool _rumorNetworkDirty;

        public RumorNetworkHostSession RumorNetwork => EnsureRumorNetwork();

        public RumorNetworkHostSession EnsureRumorNetwork()
        {
            if (_rumorNetwork != null) return _rumorNetwork;

            var state = RumorNetworkSaveStore.TryLoad() ?? new RumorNetworkState();
            var system = new RumorSystem(state);

            // Plan 203 — bind the canonical information hubs before the host session
            // is created, so the hardcoded fallback hubs are never the runtime truth.
            string hubCatalogPath = CatalogPath.ResolveCatalog("rumor_hubs.json");
            var hubCatalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (hubCatalogIo.FileExists(hubCatalogPath))
            {
                system.LoadCatalog(hubCatalogIo.ReadAllText(hubCatalogPath));
            }

            _rumorNetwork = new RumorNetworkHostSession(system);
            _rumorNetwork.StateChanged += OnRumorNetworkStateChanged;
            return _rumorNetwork;
        }

        private void OnRumorNetworkStateChanged()
        {
            _rumorNetworkDirty = true;
        }

        public void TickRumorNetwork(int day)
        {
            EnsureRumorNetwork();
            _rumorNetwork.TickDay(day);
            _rumorNetworkDirty = true;
        }

        private void SetupRumorNetwork()
        {
            EnsureRumorNetwork();
        }

        private void SaveRumorNetwork()
        {
            var session = EnsureRumorNetwork();
            if (session != null)
            {
                CaptureSection("wasteland_rumors", RumorNetworkSaveStore.TryCapturePersisted(session.System.CaptureState()));
                _rumorNetworkDirty = false;
            }
        }

        private void SetupRumorBoardPanel()
        {
            if (_rumorBoardPanel != null && _rumorBoardPanel.IsInsideTree())
                return;

            EnsureRumorNetwork();
            _rumorBoardPanel = new RumorBoardPanel();
            _rumorBoardPanel.Bind(_rumorNetwork);
            _rumorBoardPanel.OnClose += () => _rumorBoardPanel.Visible = false;
            _rumorBoardPanel.Visible = false;
            AddChild(_rumorBoardPanel);
        }

        public void ShowRumorNetworkPanel()
        {
            SetupRumorBoardPanel();
            _rumorBoardPanel.Visible = true;
            _rumorBoardPanel.RefreshView();
        }
    }
}
