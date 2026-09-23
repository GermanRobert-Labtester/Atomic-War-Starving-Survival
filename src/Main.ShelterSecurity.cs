// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.Shelter;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private ShelterSecurityHostSession _shelterSecurity = null!;
        private ShelterSecurityPanel _shelterSecurityPanel = null!;
        private bool _shelterSecurityDirty;

        public ShelterSecurityHostSession ShelterSecurity => EnsureShelterSecurity();

        public ShelterSecurityHostSession EnsureShelterSecurity()
        {
            if (_shelterSecurity != null) return _shelterSecurity;

            var state = ShelterSecuritySaveStore.TryLoad() ?? new ShelterSecurityState();
            var system = new ShelterSecuritySystem(state);

            // Plan 138 — bind the authored security zones so access requests resolve
            // against canonical zone definitions instead of an empty state ledger.
            string securityCatalogPath = CatalogPath.ResolveCatalog("shelter_security_zones.json");
            var securityCatalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (securityCatalogIo.FileExists(securityCatalogPath))
            {
                system.LoadCatalog(securityCatalogIo.ReadAllText(securityCatalogPath));
            }

            _shelterSecurity = new ShelterSecurityHostSession(system);
            _shelterSecurity.StateChanged += OnShelterSecurityStateChanged;
            return _shelterSecurity;
        }

        private void OnShelterSecurityStateChanged()
        {
            _shelterSecurityDirty = true;
        }

        public void TickShelterSecurity(int day)
        {
            EnsureShelterSecurity();
            _shelterSecurityDirty = true;
        }

        private void SetupShelterSecurity()
        {
            EnsureShelterSecurity();
        }

        private void SaveShelterSecurity()
        {
            var session = EnsureShelterSecurity();
            if (session != null)
            {
                CaptureSection("shelter_security", ShelterSecuritySaveStore.TryCapturePersisted(session.System.CaptureState()));
                _shelterSecurityDirty = false;
            }
        }

        private void SetupShelterSecurityPanel()
        {
            if (_shelterSecurityPanel != null && _shelterSecurityPanel.IsInsideTree())
                return;

            EnsureShelterSecurity();
            _shelterSecurityPanel = new ShelterSecurityPanel();
            _shelterSecurityPanel.Bind(_shelterSecurity);
            _shelterSecurityPanel.OnClose += () => _shelterSecurityPanel.Visible = false;
            _shelterSecurityPanel.Visible = false;
            AddChild(_shelterSecurityPanel);
        }

        public void ShowShelterSecurityPanel()
        {
            SetupShelterSecurityPanel();
            _shelterSecurityPanel.Visible = true;
            _shelterSecurityPanel.RefreshView();
        }
    }
}
