// SPDX-License-Identifier: MIT
using Godot;
using System.IO;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private void RunDeconAirlockUiTestAndQuit()
        {
            BuildUserInterface();
            HandleDeconAirlockAction("OPEN");
            bool pass = true;
            if (_deconAirlockPanel == null) { GD.PrintErr("[FAIL] panel not constructed"); pass = false; }
            if (pass) GD.Print("DeconAirlockUiTest PASS");
            GetTree().Quit(pass ? 0 : 1);
        }

        private void RunGeodeticSurveyUiTestAndQuit()
        {
            BuildUserInterface();
            HandleGeodeticSurveyAction("OPEN");
            bool pass = true;
            if (_geodeticSurveyPanel == null) { GD.PrintErr("[FAIL] panel not constructed"); pass = false; }
            if (pass) GD.Print("GeodeticSurveyUiTest PASS");
            GetTree().Quit(pass ? 0 : 1);
        }

        private void RunKineticStorageUiTestAndQuit()
        {
            BuildUserInterface();
            HandleKineticStorageAction("OPEN");
            bool pass = true;
            if (_kineticStoragePanel == null) { GD.PrintErr("[FAIL] panel not constructed"); pass = false; }
            if (pass) GD.Print("KineticStorageUiTest PASS");
            GetTree().Quit(pass ? 0 : 1);
        }

        private void RunChemicalReconUiTestAndQuit()
        {
            BuildUserInterface();
            HandleChemicalReconAction("OPEN");
            bool pass = true;
            if (_chemicalReconPanel == null) { GD.PrintErr("[FAIL] panel not constructed"); pass = false; }
            if (pass) GD.Print("ChemicalReconUiTest PASS");
            GetTree().Quit(pass ? 0 : 1);
        }

        private void RunGeothermalAquiferUiTestAndQuit()
        {
            BuildUserInterface();
            if (_geothermalAquiferPanel != null) _geothermalAquiferPanel.Visible = true;
            bool pass = true;
            if (_geothermalAquiferPanel == null) { GD.PrintErr("[FAIL] panel not constructed"); pass = false; }
            if (pass) GD.Print("GeothermalAquiferUiTest PASS");
            GetTree().Quit(pass ? 0 : 1);
        }

        private void RunReconTelemetryUiTestAndQuit()
        {
            BuildUserInterface();
            HandleReconTelemetryAction("OPEN");
            bool pass = true;
            if (_reconTelemetryPanel == null) { GD.PrintErr("[FAIL] panel not constructed"); pass = false; }
            if (pass) GD.Print("ReconTelemetryUiTest PASS");
            GetTree().Quit(pass ? 0 : 1);
        }
    }
}
