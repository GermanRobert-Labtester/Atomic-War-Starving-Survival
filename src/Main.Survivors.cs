using Godot;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AtomicWar.Journal;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Radio;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.Economy;
using AtomicWar.GodotApp.YearOfAsh;
using AtomicWar.GodotApp.Muster;
using AtomicWar.GodotApp.Dose;
using AtomicWar.GodotApp.UtilityAI;
using AtomicWar.GodotApp.Radio;
using AtomicWar.GodotApp.Audio;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        // ── Survivor / UtilityAI fields (GAP-ARCH-01 Phase 1) ──
        private SurvivorsHostSession _survivors = null!;
        private UtilityAiHostSession _utilityAi = null!;

        private static string FormatSurvivorName(string id)
        {
            if (string.IsNullOrEmpty(id)) return "Unknown";
            return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(id.Replace('_', ' '));
        }

        private void SetupSurvivors()
        {
            if (_survivors != null) return;
            _survivors = new SurvivorsHostSession();
            _survivors.LoadCatalog(_dataDir);
            _survivors.SeedDemoRoster();
            _survivors.StateChanged += () =>
            {
                SaveSurvivors();
                _survivorsOverlay?.RefreshView();
                _medicalPanel?.RefreshView();
                _shelterPanel?.RefreshView();
                if (_state == GameState.Playing) UpdateHud();
            };

            var save = SurvivorsSaveStore.TryLoad();
            if (save != null && save.survivors.Count > 0)
                _survivors.RestoreSave(save);
        }

        private void SetupUtilityAi()
        {
            if (_utilityAi != null) return;
            _utilityAi = UtilityAiHostSession.Create(_dataDir);

            if (_utilityAiPanel == null && _rightColumn != null)
            {
                _utilityAiPanel = new UtilityAiPanel();
                _rightColumn.AddChild(_utilityAiPanel);
            }
            if (_utilityAiPanel != null)
            {
                _utilityAiPanel.BindSession(_utilityAi);
                _utilityAiPanel.RefreshView();
            }
        }

        private void OnUtilityAiEvaluateClicked()
        {
            SetupUtilityAi();
            _statusLabel.Text = _utilityAi.EvaluateDemo("survivor_gunner_mikhail", 30f, 0.7f);
            _utilityAiPanel.RefreshView();
        }

        private void OnSurvivorsOpenClicked()
        {
            SetupSurvivors();
            _statusLabel.Text = "Survivors panel open. Needs and radiation are simulated.";
            _codexViewer.Text = _survivors.StatusLine();
        }

        private void OnSurvivorsTickClicked()
        {
            SetupSurvivors();
            _survivors.TickHour(6f);
            SetupPhase0();
            _phase0.TickHour(6f);
            _statusLabel.Text = _survivors.LastEvent + "\n" + _phase0.LastEvent;
            _codexViewer.Text = _survivors.StatusLine();
        }

        private void OnSurvivorsExposeClicked(string id, float rads)
        {
            SetupSurvivors();
            _statusLabel.Text = _survivors.ExposeToZone(id, rads);
            _codexViewer.Text = _survivors.StatusLine();
        }

        private void OnSurvivorsIodineClicked(string id)
        {
            SetupSurvivors();
            _statusLabel.Text = _survivors.AdministerIodine(id);
            _codexViewer.Text = _survivors.StatusLine();
        }

        private void OnSurvivorsAntiRadClicked(string id, float rads)
        {
            SetupSurvivors();
            _statusLabel.Text = _survivors.AdministerAntiRad(id, rads);
            _codexViewer.Text = _survivors.StatusLine();
        }

        private void SaveSurvivors()
        {
            if (_survivors == null) return;
            if (SurvivorsSaveStore.TrySave(_survivors.CaptureSave()))
                GD.Print("[Ashfall Godot] Survivors save written.");
        }

        private void CloseSurvivorsOverlay()
        {
            _survivorsOverlay.Visible = false;
        }

        private void CloseSurvivorDetailPanel()
        {
            _survivorDetailPanel.Visible = false;
        }

    }
}
