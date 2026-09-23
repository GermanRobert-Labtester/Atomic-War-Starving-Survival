// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.Communication;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private TimeCapsuleHostSession _timeCapsule = null!;
        private TimeCapsulePanel _timeCapsulePanel = null!;
        private bool _timeCapsuleDirty;

        public TimeCapsuleHostSession TimeCapsules => EnsureTimeCapsule();

        public TimeCapsuleHostSession EnsureTimeCapsule()
        {
            if (_timeCapsule != null) return _timeCapsule;

            var state = TimeCapsuleSaveStore.TryLoad() ?? new TimeCapsuleState();
            var system = new TimeCapsuleSystem(state);

            // Plan 212 — bind the authored pre-placed capsules so a fresh (or legacy)
            // shelter state contains the canonical sealed vaults, not an empty list.
            string capsuleCatalogPath = CatalogPath.ResolveCatalog("time_capsules.json");
            var capsuleCatalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (capsuleCatalogIo.FileExists(capsuleCatalogPath))
            {
                var capsuleCatalog = System.Text.Json.JsonSerializer.Deserialize<TimeCapsuleCatalogData>(
                    capsuleCatalogIo.ReadAllText(capsuleCatalogPath),
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (capsuleCatalog != null)
                {
                    system.LoadCatalog(capsuleCatalog);
                }
            }

            _timeCapsule = new TimeCapsuleHostSession(system);
            _timeCapsule.StateChanged += OnTimeCapsuleStateChanged;
            return _timeCapsule;
        }

        private void OnTimeCapsuleStateChanged()
        {
            _timeCapsuleDirty = true;
        }

        public void TickTimeCapsule(int day)
        {
            EnsureTimeCapsule();
            _timeCapsule.TickDay(day);
            _timeCapsuleDirty = true;
        }

        private void SetupTimeCapsules()
        {
            EnsureTimeCapsule();
        }

        private void SaveTimeCapsules()
        {
            var session = EnsureTimeCapsule();
            if (session != null)
            {
                CaptureSection("time_capsules", TimeCapsuleSaveStore.TryCapturePersisted(session.System.CaptureState()));
                _timeCapsuleDirty = false;
            }
        }

        private void FlushTimeCapsulesIfDirty()
        {
            if (_timeCapsuleDirty) SaveTimeCapsules();
        }

        private void SetupTimeCapsulePanel()
        {
            if (_timeCapsulePanel != null && _timeCapsulePanel.IsInsideTree())
                return;

            EnsureTimeCapsule();
            _timeCapsulePanel = new TimeCapsulePanel();
            _timeCapsulePanel.Bind(_timeCapsule);
            _timeCapsulePanel.OnClose += () => _timeCapsulePanel.Visible = false;
            _timeCapsulePanel.Visible = false;
            AddChild(_timeCapsulePanel);
        }

        public void ShowTimeCapsulePanel()
        {
            SetupTimeCapsulePanel();
            _timeCapsulePanel.Visible = true;
            _timeCapsulePanel.RefreshView();
        }

        /// <summary>
        /// Plan 212 — fires an event key at the sealed capsule ledger. EventBased
        /// capsules bound to the key open exactly once. Returns the number opened.
        /// </summary>
        public int NotifyTimeCapsuleEvent(string eventId, int day)
        {
            var session = EnsureTimeCapsule();
            int opened = session.System.TryOpenEventCapsules(eventId, day);
            if (opened > 0)
            {
                _timeCapsuleDirty = true;
            }
            return opened;
        }
    }
}
