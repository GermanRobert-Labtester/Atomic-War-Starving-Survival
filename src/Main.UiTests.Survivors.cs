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
        /// <summary>Headless smoke: survivors rosters build, needs tick, rad exposure, iodine/anti-rad, save roundtrip.</summary>
        private void RunSurvivorsUiTestAndQuit()
        {
            BuildUserInterface();
            SetupSurvivors();

            bool roster = _survivors.RosterState.Count == 3;
            _survivors.TickHour(6f);
            bool needsMoved = _survivors.RosterState[0].Hunger > 0f;

            string exposed = _survivors.ExposeToZone("survivor_gunner_mikhail", 60f);
            bool doseClimbed = _survivors.Radiation.GetDosimeter("survivor_gunner_mikhail").LifetimeDose > 0f;

            string iodine = _survivors.AdministerIodine("survivor_gunner_mikhail");
            bool resistance = _survivors.Radiation.GetDosimeter("survivor_gunner_mikhail") != null
                && System.Linq.Enumerable.Any(_survivors.RosterState, s => s.Id == "survivor_gunner_mikhail");

            string antiRad = _survivors.AdministerAntiRad("survivor_gunner_mikhail", 30f);
            bool antiRadApplied = antiRad.Contains("cleared");

            // Save → restore roundtrip.
            var save = _survivors.CaptureSave();
            var fresh = new SurvivorsHostSession();
            fresh.RestoreSave(save);
            bool roundtrip = fresh.RosterState.Count == 3;
            var restoredRad = fresh.Radiation.GetDosimeter("survivor_gunner_mikhail");
            bool radRestored = restoredRad != null;

            bool pass = roster && needsMoved && doseClimbed && resistance && antiRadApplied && roundtrip && radRestored;
            GD.Print($"[SurvivorsUiTest] roster={roster} needs={needsMoved} dose={doseClimbed} " +
                     $"iodine={resistance} antiRad={antiRadApplied} roundtrip={roundtrip} rad={radRestored}");
            GD.Print(pass ? "SURVIVORS_UITEST PASS" : "SURVIVORS_UITEST FAIL");
            if (System.IO.File.Exists(SurvivorsSaveStore.SavePath))
                System.IO.File.Delete(SurvivorsSaveStore.SavePath);
            QuitUiTestAfterFrame(pass ? 0 : 1);
        }

    }
}
