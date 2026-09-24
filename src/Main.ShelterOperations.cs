// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Random;
using Ashfall.Core.Settlements;
using Ashfall.Core.Survivors;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private ShelterOperationsHostSession? _shelterOperations;
        private ShelterOperationsPanel? _shelterOperationsPanel;

        private ShelterOperationsHostSession? EnsureShelterOperationsSession()
        {
            if (_shelterOperations != null) return _shelterOperations;
            SetupSurvivors();
            SetupInventory();
            SetupShelterAssignment();
            SetupOrphanSealWave1();
            var outposts = EnsureOutpostSettlement();
            if (_shelterExpansion == null || _seasonalCelebration == null || outposts == null)
                return null;

            _shelterOperations = new ShelterOperationsHostSession(
                _shelterExpansion,
                outposts,
                _seasonalCelebration,
                _inventory?.Inventory,
                _survivors,
                () => Math.Max(1, _simDay),
                survivorId => EvaluateSurvivorFitness(survivorId).Level != FitnessLevel.Incapacitated,
                day => _campaignDay?.Rng?.Fork(CampaignStreamIds.Social, day, 62)
                    ?? new SeededRng(6200 + day),
                ResolveCanonicalRationItemId,
                ResolveCanonicalFuelItemId);
            return _shelterOperations;
        }

        private void SetupShelterOperationsPanel()
        {
            var session = EnsureShelterOperationsSession();
            if (session == null) return;
            if (_shelterOperationsPanel != null && _shelterOperationsPanel.IsInsideTree())
            {
                _shelterOperationsPanel.Bind(session);
                return;
            }

            _shelterOperationsPanel = new ShelterOperationsPanel();
            _shelterOperationsPanel.Bind(session);
            _shelterOperationsPanel.OnClose += () =>
            {
                if (_shelterOperationsPanel != null)
                    _shelterOperationsPanel.Visible = false;
            };
            _shelterOperationsPanel.Visible = false;
            AddChild(_shelterOperationsPanel);
        }

        private void ShowShelterOperationsPanel()
        {
            SetupShelterOperationsPanel();
            if (_shelterOperationsPanel == null)
            {
                if (_statusLabel != null)
                    _statusLabel.Text = "Shelter Operations is unavailable. Check construction and outpost catalogs.";
                return;
            }
            _shelterOperationsPanel.Visible = true;
            _shelterOperationsPanel.RefreshView();
        }

        private void ResetShelterOperations()
        {
            _shelterOperations?.Dispose();
            _shelterOperations = null;
            if (_shelterOperationsPanel != null)
            {
                var panel = _shelterOperationsPanel;
                panel.Unbind();
                if (panel.IsInsideTree())
                    RemoveChild(panel);
                panel.QueueFree();
                _shelterOperationsPanel = null;
            }
        }
    }
}
