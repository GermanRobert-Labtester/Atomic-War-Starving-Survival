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
        /// <summary>Headless smoke: THE MACHINE'S REGISTER panel builds, binds to the
        /// Verdict session, the TRANSMISSIONS section renders all 13 broadcasts once
        /// the Reckoning reaches Culpable with radio fired, and refresh is leak-free.</summary>
        private void RunVerdictUiTestAndQuit()
        {
            BuildUserInterface();
            SetupVerdict();

            bool panel = _verdictPanel != null;
            bool session = _verdict != null;

            // Drive a machine-log read to enroll a first piece of evidence.
            _verdict!.MachineLog.Post("loc_geophone_pit_1", 166, "operating", "a tap.", "evidence_geophone_hymn");
            _verdict.MachineLog.ReadEntry(0);
            _verdict.Evidence.Enroll("evidence_geophone_hymn", 166);

            // Advance Knowing → Culpable (evidence gate, day >= 210) then fire radio.
            int living = 14;
            _verdict.AdvanceDay(200, living, _verdict.MachineLog.ReadCount()); // → Knowing
            _verdict.AdvanceDay(211, living, _verdict.MachineLog.ReadCount()); // → Culpable
            _verdict.TickRadio(211); // pilot carrier (trigger 210) fires immediately in the window
            bool carrierOpenSoon = _verdict.Radio.HasFired("radio_verdict_carrier_on_window");

            _verdict.TickRadio(260); // fires the corpus whose dayTrigger <= 260
            bool someFired = _verdict.Radio.FiredCount > 0;

            // Refresh the panel and count rendered transmission rows (expect all 13).
            _verdictPanel!.RefreshView();
            int rows = _verdictPanel.RenderedRadioRowCount();
            bool transmissions = rows == 13;

            // Leak check: repeat refresh must not double the row count.
            _verdictPanel.RefreshView();
            int rows2 = _verdictPanel.RenderedRadioRowCount();
            bool noLeak = rows2 == 13;

            bool pass = panel && session && carrierOpenSoon && someFired && transmissions && noLeak;
            GD.Print($"[VerdictUiTest] panel={panel} session={session} " +
                     $"carrierOpenSoon={carrierOpenSoon} someFired={someFired} " +
                     $"transmissions={transmissions}({rows}) noLeak={noLeak}");
            HostCli.EmitSummary("verdict_uitest", pass, pass ? 0 : 1);
            QuitUiTestAfterFrame(pass ? 0 : 1);
        }

    }
}
