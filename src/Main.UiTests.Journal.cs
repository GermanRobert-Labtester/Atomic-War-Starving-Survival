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
        private void RunSelfTestAndQuit()
        {
            var catalogs = CatalogJsonLoader.Load(new FileSystemIO(), _dataDir);
            int code = JournalSelfTest.Run(catalogs);
            GetTree().Quit(code);
        }

        /// <summary>Headless smoke test: build the book, open it, cycle every tab.</summary>
        private void RunJournalUiTestAndQuit()
        {
            BuildUserInterface();
            SetupJournal();

            _journalBook.Open();
            bool opened = _journalBook.IsOpen && _journalBook.Visible;
            int logLen = _journalBook.ActiveTabContent.Length;
            int summaryLen = _journalBook.DetailSummary.Length;

            int tabsWithContent = 0;
            for (int t = 0; t < JournalSystem.TabCount; t++)
            {
                _journal.SwitchTab(t);
                if (_journalBook.ActiveTabContent.Length > 0) tabsWithContent++;
                GD.Print($"[JournalUiTest] tab {t} ({_journalBook.ActiveTab}) content={_journalBook.ActiveTabContent.Length} chars · status=\"{_journalBook.StatusLine}\"");
            }
            _journalBook.Close();
            bool closed = !_journalBook.IsOpen && !_journalBook.Visible;

            bool pass = opened && closed && logLen > 0 && summaryLen > 0 && tabsWithContent == JournalSystem.TabCount;
            HostCli.EmitSummary("journal_uitest", pass, pass ? 0 : 1);
            QuitUiTestAfterFrame(pass ? 0 : 1);
        }

    }
}
