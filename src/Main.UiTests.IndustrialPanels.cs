// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Medical;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using AtomicWar.GodotApp.UI;
using Godot;
using System;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {

#if ASHFALL_SELFTEST

        // ─── UI Tests ───

        private void RunEbPvdCoatingUiTestAndQuit()
        {
            BuildPlans146To149Panels();
            HandleEbPvdCoatingAction("OPEN");
            bool pass = _ebPvdCoatingPanel != null && _ebPvdCoatingPanel.IsBound;
            if (pass) GD.Print("EbPvdCoatingUiTest PASS");
            else GD.PrintErr("[FAIL] EbPvdCoatingPanel not ready");
            GetTree().Quit(pass ? 0 : 1);
        }

#endif

#if ASHFALL_SELFTEST

        private void RunMicrofluidicDiagnosticUiTestAndQuit()
        {
            BuildPlans146To149Panels();
            HandleMicrofluidicDiagnosticAction("OPEN");
            bool pass = _microfluidicDiagnosticPanel != null && _microfluidicDiagnosticPanel.IsBound;
            if (pass) GD.Print("MicrofluidicDiagnosticUiTest PASS");
            else GD.PrintErr("[FAIL] MicrofluidicDiagnosticPanel not ready");
            GetTree().Quit(pass ? 0 : 1);
        }

#endif

#if ASHFALL_SELFTEST

        private void RunMineFlailUiTestAndQuit()
        {
            BuildPlans146To149Panels();
            HandleMineFlailAction("OPEN");
            bool pass = _mineFlailPanel != null && _mineFlailPanel.IsBound;
            if (pass) GD.Print("MineFlailUiTest PASS");
            else GD.PrintErr("[FAIL] MineFlailPanel not ready");
            GetTree().Quit(pass ? 0 : 1);
        }

#endif

#if ASHFALL_SELFTEST

        private void RunRailGrindingUiTestAndQuit()
        {
            BuildPlans146To149Panels();
            HandleRailGrindingAction("OPEN");
            bool pass = _railGrindingPanel != null && _railGrindingPanel.IsBound;
            if (pass) GD.Print("RailGrindingUiTest PASS");
            else GD.PrintErr("[FAIL] RailGrindingPanel not ready");
            GetTree().Quit(pass ? 0 : 1);
        }

#endif

    }
}
