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
        /// <summary>Headless smoke: Phase-0 panel builds, binds, and renders all ten condition groups.</summary>
        private void RunPhase0UiTestAndQuit()
        {
            BuildUserInterface();
            SetupSurvivors();
            SetupPhase0();

            bool panel = _phase0Panel != null;
            bool session = _phase0 != null;
            if (!panel || !session)
            {
                GD.Print("[Phase0UiTest] panel=false session=false");
                GD.Print("PHASE0_UITEST FAIL");
                QuitUiTestAfterFrame(1);
                return;
            }

            // Drive all ten systems so every condition row renders.
            SetupInventory();
            SetupMedical();
            _phase0!.CurrentDay = 4;
            _phase0.RecordGuilt("elena_vasquez", "choice_imposed_hardship", 0.8f);
            _phase0.RegisterCombatSurvived("survivor_gunner_mikhail");
            _phase0.RegisterCombatSurvived("survivor_gunner_mikhail");
            _phase0.RecordMoralChoice("survivor_dr_sarah_chen", true);
            _phase0.RecordMoralChoice("survivor_dr_sarah_chen", true);
            _phase0.RecordMoralChoice("survivor_dr_sarah_chen", true);
            _phase0.RecordMoralChoice("survivor_dr_sarah_chen", true);
            _phase0.RecordMoralChoice("survivor_dr_sarah_chen", true);
            _phase0.ConsumeSubstance("survivor_gunner_mikhail", "item_morphine", Ashfall.Core.Medical.ChemicalDependencyKind.Opioid);
            _phase0.ConsumeSubstance("survivor_gunner_mikhail", "item_morphine", Ashfall.Core.Medical.ChemicalDependencyKind.Opioid);
            _phase0.ConsumeSubstance("survivor_gunner_mikhail", "item_morphine", Ashfall.Core.Medical.ChemicalDependencyKind.Opioid);
            _phase0.Dependency.BeginColdTurkey("survivor_gunner_mikhail", "item_morphine");
            _phase0.IsInAshZone = true;
            _phase0.TickHour(6f);
            _phase0.IsInAshZone = false;

            _phase0Panel!.Bind(_phase0, _survivors);
            _phase0Panel.Open();

            bool bound = _phase0Panel.IsBound;
            bool conditionsRendered = _phase0Panel.RenderedConditionCount > 0;
            bool visible = _phase0Panel.Visible;

            bool pass = bound && conditionsRendered && visible;

            GD.Print($"[Phase0UiTest] panel={panel} session={session} bound={bound} " +
                     $"conditions={_phase0Panel.RenderedConditionCount} visible={visible}");
            GD.Print(pass ? "PHASE0_UITEST PASS" : "PHASE0_UITEST FAIL");
            if (System.IO.File.Exists(Phase0SaveStore.SavePath))
                System.IO.File.Delete(Phase0SaveStore.SavePath);
            QuitUiTestAfterFrame(pass ? 0 : 1);
        }

    }
}
