// SPDX-License-Identifier: MIT
// Plan 214 — Visitor Integration & Temporary Housing host seam.
// AirlockSecurity owns admission; this host opens one temporary stay per
// admitted source identity, projects it to the UI, draws rations through the
// canonical inventory owner, and hands integrated visitors to the permanent
// survivor roster. Room topology, security findings, and item stacks remain
// with their owners.
using System;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Visitors;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private VisitorIntegrationHostSession _visitorIntegration = null!;
        private VisitorIntegrationPanel _visitorIntegrationPanel = null!;
        private bool _visitorIntegrationDirty;

        public VisitorIntegrationHostSession VisitorIntegration => EnsureVisitorIntegration();

        public VisitorIntegrationHostSession EnsureVisitorIntegration()
        {
            if (_visitorIntegration != null) return _visitorIntegration;

            var state = VisitorIntegrationSaveStore.TryLoad() ?? new VisitorIntegrationSaveState();
            var system = new VisitorIntegrationSystem(state);

            string catalogPath = CatalogPath.ResolveCatalog("visitor_templates.json");
            var catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (catalogIo.FileExists(catalogPath))
                system.LoadCatalog(catalogIo.ReadAllText(catalogPath));

            _visitorIntegration = new VisitorIntegrationHostSession(system);
            _visitorIntegration.StateChanged += OnVisitorIntegrationStateChanged;
            _visitorIntegration.ConsumeDailyRations = ConsumeVisitorRations;
            _visitorIntegration.RecruitToSurvivor = RecruitVisitorAsResident;
            _visitorIntegration.JournalEvent = PublishVisitorNotice;
            return _visitorIntegration;
        }

        private void OnVisitorIntegrationStateChanged()
        {
            _visitorIntegrationDirty = true;
        }

        /// <summary>
        /// Draws visitor rations from the canonical shared inventory only when
        /// the full day's total is available; a shortfall consumes nothing and
        /// never ejects the visitor.
        /// </summary>
        private bool ConsumeVisitorRations(float food, float water)
        {
            SetupInventory();
            var inventory = _inventory?.Inventory;
            if (inventory == null) return false;

            int foodUnits = (int)Math.Ceiling(Math.Max(0f, food));
            int waterUnits = (int)Math.Ceiling(Math.Max(0f, water));
            bool canCoverFood = foodUnits <= 0 || inventory.HasSufficient("canned_food", foodUnits);
            bool canCoverWater = waterUnits <= 0 || inventory.HasSufficient("clean_water", waterUnits);
            if (!canCoverFood || !canCoverWater) return false;

            if (foodUnits > 0 && !inventory.TryConsume("canned_food", foodUnits)) return false;
            if (waterUnits > 0 && !inventory.TryConsume("clean_water", waterUnits)) return false;
            return true;
        }

        /// <summary>
        /// Converts a fully integrated visitor into a permanent resident
        /// through the survivor roster owner. The visitor stay closes only
        /// after the roster registers the resident.
        /// </summary>
        private bool RecruitVisitorAsResident(VisitorRecord visitor)
        {
            if (visitor == null) return false;
            SetupSurvivors();
            if (_survivors == null || _survivors.Find(visitor.VisitorId) != null) return false;
            return _survivors.AddSurvivor(
                id: visitor.VisitorId,
                displayName: visitor.Name,
                health: 100f,
                hunger: 10f,
                thirst: 15f,
                warmth: 85f,
                morale: 55f);
        }

        private void PublishVisitorNotice(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            SetupJournal();
            _journal?.TryAddRawEntry($"visitor_{_simDay}", message, null!, _simDay);
        }

        private void SetupVisitorIntegration()
        {
            EnsureVisitorIntegration();

            // Airlock admission handoff: a committed Admit decision opens
            // exactly one temporary stay. The incident ledger stays owned by
            // AirlockSecurity; the host session de-duplicates by source id.
            SetupAirlockSecurity();
            if (_airlockSecurity != null)
            {
                _airlockSecurity.System.OnIncidentResolved -= OnAirlockIncidentResolved;
                _airlockSecurity.System.OnIncidentResolved += OnAirlockIncidentResolved;
            }
        }

        private void OnAirlockIncidentResolved(AirlockIncidentLog log)
        {
            if (log == null || log.decision != VisitorDecision.Admit) return;
            var session = EnsureVisitorIntegration();
            session.HandleAirlockAdmission(
                sourceVisitorId: log.visitorId,
                visitorType: _airlockSecurity?.System.State.visitorType ?? string.Empty,
                admittedBy: _airlockSecurity?.System.State.sentryId ?? "airlock_operator",
                currentDay: log.day > 0 ? log.day : _simDay);
            _visitorIntegrationPanel?.RefreshView();
        }

        private void TickVisitorIntegration(int day)
        {
            EnsureVisitorIntegration();
            _visitorIntegration.TickDay(day);
            _visitorIntegrationDirty = true;
        }

        private void SaveVisitorIntegration()
        {
            var session = EnsureVisitorIntegration();
            if (session == null) return;
            if (CaptureSection("visitor_integration", VisitorIntegrationSaveStore.TryCapturePersisted(session.CaptureState())))
                _visitorIntegrationDirty = false;
        }

        private void SetupVisitorIntegrationPanel()
        {
            if (_visitorIntegrationPanel != null && _visitorIntegrationPanel.IsInsideTree())
                return;

            EnsureVisitorIntegration();
            _visitorIntegrationPanel = new VisitorIntegrationPanel();
            _visitorIntegrationPanel.Bind(_visitorIntegration);
            _visitorIntegrationPanel.OnClose += () => _visitorIntegrationPanel.Visible = false;
            _visitorIntegrationPanel.Visible = false;
            AddChild(_visitorIntegrationPanel);
        }

        public void ShowVisitorIntegrationPanel()
        {
            SetupVisitorIntegrationPanel();
            _visitorIntegrationPanel.Visible = true;
            _visitorIntegrationPanel.RefreshView();
        }

        /// <summary>Read-only projection for tests and debug surfaces.</summary>
        public System.Collections.Generic.IReadOnlyList<VisitorRecord> GetActiveVisitors()
        {
            SetupVisitorIntegration();
            return _visitorIntegration.GetActiveVisitors();
        }
    }
}
