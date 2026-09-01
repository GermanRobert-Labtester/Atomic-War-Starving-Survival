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
        /// <summary>Headless smoke: muster roster widget + approach modal render, escalate, select.</summary>
        private void RunMusterUiTestAndQuit()
        {
            BuildUserInterface();
            SetupMuster();

            bool roster = _currentsRoster != null && _muster.Roster.Count >= 15;
            bool camp = _campWidget != null;
            bool witnesses = _witnessPanel != null && _muster.Witnesses.Count >= 3;
            bool epilogues = _muster.Epilogues.Count >= 8;
            bool modal = _approachModal != null;
            bool escalate = _muster.Escalate(300).Contains("Muster is open");
            bool campFormed = _muster.Camp.Formed && _muster.Camp.MembersRallied == CoalitionCampSystem.BaseMembers;
            bool strategy = _muster.SetStrategy(QuestApproach.B).Contains("Strategy B");
            bool resolved = _muster.SelectApproach("quest_the_rate_card_war", QuestApproach.A)
                .Contains("selected");
            bool ending = _muster.Engine.EndingKeyFor("quest_the_rate_card_war") == "the_rate_card_revised";
            bool matrix = _muster.Engine.EndingKeyForAny("the_rate_card_revised")
                && _muster.EndingProseFor("the_rate_card_revised").Contains("rate card is finally a published price");
            _muster.CycleAuthorBias();
            bool biasCycle = _muster.AuthorBias != RiskBiasTrait.Realist;

            bool pass = roster && camp && witnesses && epilogues && modal && escalate &&
                        campFormed && strategy && resolved && ending && matrix && biasCycle;
            GD.Print($"[MusterUiTest] roster={roster} camp={camp} witnesses={witnesses} " +
                     $"epilogues={epilogues} modal={modal} escalate={escalate} campFormed={campFormed} " +
                     $"strategy={strategy} select={resolved} ending={ending} matrix={matrix}");
            HostCli.EmitSummary("muster_uitest", pass, pass ? 0 : 1);
            if (System.IO.File.Exists(MusterSaveStore.SavePath))
                System.IO.File.Delete(MusterSaveStore.SavePath);
            QuitUiTestAfterFrame(pass ? 0 : 1);
        }

    }
}
