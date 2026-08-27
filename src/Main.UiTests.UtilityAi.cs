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
        /// <summary>
        /// Headless smoke: utility AI panel builds, scores render, refresh +
        /// rebind are leak-free, evaluation selects an action.
        /// </summary>
        private void RunUtilityAiUiTestAndQuit()
        {
            BuildUserInterface();
            SetupUtilityAi();

            bool panel = _utilityAiPanel != null;
            bool catalog = _utilityAi.Actions.Count == 4;

            int before = _utilityAiPanel!.GetChild(0).GetChildCount();
            _utilityAiPanel.RefreshView();
            _utilityAiPanel.RefreshView();
            int after = _utilityAiPanel.GetChild(0).GetChildCount();
            bool noLeak = before == after;

            string result = _utilityAi.EvaluateDemo("sv_demo", 30f, 0.7f);
            bool selected = result.Contains("selects");

            bool pass = panel && catalog && noLeak && selected;
            GD.Print($"[UtilityAiUiTest] panel={panel} catalog={catalog} noLeak={noLeak} selected={selected}");
            HostCli.EmitSummary("utility_ai_uitest", pass, pass ? 0 : 1);
            QuitUiTestAfterFrame(pass ? 0 : 1);
        }

    }
}
