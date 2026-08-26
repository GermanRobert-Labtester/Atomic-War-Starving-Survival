// Main.cs — thin composition root (187 lines). Lifecycle orchestration extracted to Main.Application.cs,
// domain logic split across 15+ partials (Survivors, World, Campaign, Quests, Economy, etc.).
// See docs/architecture.md for partial ownership.

using Godot;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AtomicWar.Journal;
using AtomicWar.GodotApp.Host;
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
        private Label _titleLabel = null!;
        private Label _statusLabel = null!;
        private Label _diagnosticsLabel = null!;
        private Label _iceRoadLabel = null!;
        private Label _catalogLabel = null!;
        private Label _briefingPreviewLabel = null!;
        private VBoxContainer _menuContainer = null!;
        private TextEdit _codexViewer = null!;
        private SaveLoadHostSession _saveLoadHost = null!;

        // Questline master registry (loaded early for expansion quest ID validation)
        private QuestlineMasterCatalog _questlineMaster = null!;

        // Journal (docs/ui/JOURNAL_UI_PLAN.md)
        private Ashfall.Core.Events.SimpleEventBus _eventBus = new Ashfall.Core.Events.SimpleEventBus();
        private AtomicWar.GodotApp.Host.HostEventAdapter _hostEventAdapter = null!;
        private string _dataDir = string.Empty;
        private int _simDay = 4;

        // Diagnostics strip throttling. Engine.GetVersionInfo() allocates a Godot
        // Dictionary, so the version string is resolved once and cached for the process.
        private const double DiagnosticsRefreshSeconds = 0.25;
        private static readonly string s_engineVersion =
            Engine.GetVersionInfo()["string"].AsString();
        private double _diagnosticsAccum;
        private double _diagnosticsLogAccum;

        // Journal save coalescing. Saving on every entry rewrote the whole file once
        // per seeded entry; entries are marked dirty and flushed on the diagnostics tick,
        // on close, and on quit instead.
        private bool _journalDirty;


        // Sleep / Advance confirmation fields
        private const double AdvanceCountdownDefaultSeconds = 3.0;
        private double _advanceTimerRemaining;
        private bool _advanceConfirmed;
        private bool _advanceCancelled;

        private enum GameState { Menu, Playing, GameOver }
        private GameState _state = GameState.Menu;
    }
}
