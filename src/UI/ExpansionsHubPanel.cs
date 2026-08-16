using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Crossing;
using Ashfall.Core.Medical;
using Ashfall.Core.Muster;
using Ashfall.Core.UI;
using Ashfall.Core.Verdict;
using Ashfall.Core.World;
using CoreTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Master 12-Expansion Operational Hub Panel for ASHFALL.
    /// Presents real-time status telemetry and navigation entry points
    /// for all 12 operational expansion modules.
    ///
    /// Presentation only — queries host sessions for simulation state.
    /// </summary>
    public partial class ExpansionsHubPanel : Control
    {
        public event Action? OnClose;
        public event Action<string>? OnOpenExpansionRequested;

        private ExpansionHostSession? _expansions;
        private GreenhouseHostSession? _greenhouse;
        private DutyRosterHostSession? _dutyRoster;
        private MusterHostSession? _muster;
        private MaritimeHostSession? _maritime;
        private DeepCoastHostSession? _deepCoast;
        private WorldHostSession? _world;
        private MedicalHostSession? _medical;
        private VerdictHostSession? _verdict;
        private int _currentDay = 1;

        private VBoxContainer _modulesContainer = null!;
        private Label _headerSubtitle = null!;
        private Label _statusLabel = null!;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildLayout();
            Visible = false;
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && !key.Echo && key.Keycode == Key.Escape)
            {
                Close();
                GetViewport().SetInputAsHandled();
            }
        }

        public void Bind(
            ExpansionHostSession? expansions,
            GreenhouseHostSession? greenhouse,
            DutyRosterHostSession? dutyRoster,
            MusterHostSession? muster,
            MaritimeHostSession? maritime,
            DeepCoastHostSession? deepCoast,
            WorldHostSession? world,
            MedicalHostSession? medical,
            VerdictHostSession? verdict,
            int currentDay)
        {
            _expansions = expansions;
            _greenhouse = greenhouse;
            _dutyRoster = dutyRoster;
            _muster = muster;
            _maritime = maritime;
            _deepCoast = deepCoast;
            _world = world;
            _medical = medical;
            _verdict = verdict;
            _currentDay = currentDay;

            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public void Close()
        {
            Visible = false;
            OnClose?.Invoke();
        }

        private void BuildLayout()
        {
            var backdrop = new ColorRect
            {
                Color = new Color(0.02f, 0.03f, 0.04f, 0.95f)
            };
            backdrop.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(backdrop);

            var margin = new MarginContainer();
            margin.SetAnchorsPreset(LayoutPreset.FullRect);
            margin.AddThemeConstantOverride("margin_left", (int)CoreTheme.SpacingLg);
            margin.AddThemeConstantOverride("margin_right", (int)CoreTheme.SpacingLg);
            margin.AddThemeConstantOverride("margin_top", (int)CoreTheme.SpacingLg);
            margin.AddThemeConstantOverride("margin_bottom", (int)CoreTheme.SpacingLg);
            AddChild(margin);

            var mainVBox = new VBoxContainer();
            mainVBox.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
            margin.AddChild(mainVBox);

            // ── Header Bar ──
            var headerCard = AshfallUiHelpers.MakeCardFrame(
                "ASHFALL // 12-EXPANSION STRATEGIC OPERATIONAL HUB",
                "Full unified tactical matrix across all post-nuclear expansion modules."
            );
            mainVBox.AddChild(headerCard);

            // ── Scrollable Body ──
            var scroll = new ScrollContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill,
                HorizontalScrollMode = ScrollContainer.ScrollMode.Disabled
            };
            mainVBox.AddChild(scroll);

            _modulesContainer = new VBoxContainer();
            _modulesContainer.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
            _modulesContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(_modulesContainer);

            // ── Bottom Action Bar ──
            var bottomBar = new HBoxContainer();
            bottomBar.AddThemeConstantOverride("separation", (int)CoreTheme.SpacingMd);
            mainVBox.AddChild(bottomBar);

            var btnClose = AshfallUiHelpers.MakeButton("RETURN TO DASHBOARD [ESC]", Close);
            btnClose.CustomMinimumSize = new Vector2(240, 44);
            bottomBar.AddChild(btnClose);

            _statusLabel = AshfallUiHelpers.MakeMono("Operational matrix synchronized. All 12 systems linked.");
            _statusLabel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            bottomBar.AddChild(_statusLabel);
        }

        public void RefreshView()
        {
            if (_modulesContainer == null) return;
            ClearContainer(_modulesContainer);

            // ── 1. The Holdfast (Exp 01) ──
            AddModuleCard(
                "EXP 01 — THE HOLDFAST",
                "Waystation economy, ledger barter, merchant caravan dispatch, and dynamic pricing across 3 trade stances.",
                "OPERATIONAL",
                CoreTheme.Warm,
                new (string, string, (float r, float g, float b, float a))[]
                {
                    ("Trade Network", "3 Regional Factions Linked", CoreTheme.Pale),
                    ("Ledger State", "Live dynamic commodity pricing active", CoreTheme.Pale),
                    ("Waystation", "Water, food, and rad-filter exchange", CoreTheme.Warm)
                },
                "OPEN HOLDFAST TRADE TERMINAL",
                () => OnOpenExpansionRequested?.Invoke("holdfast")
            );

            // ── 2. Duty Roster (Exp 02) ──
            string rosterStatus = _dutyRoster != null ? $"{_dutyRoster.Roster.State.rows.Count} Tracked" : "Standby";
            AddModuleCard(
                "EXP 02 — THE DUTY ROSTER",
                "Survivor labor shifts, specialized workstations, fatigue accumulation, trauma bonding, and work strikes.",
                "ROSTER ACTIVE",
                CoreTheme.Warm,
                new (string, string, (float r, float g, float b, float a))[]
                {
                    ("Work Shifts", "Day & Night cycle rotations", CoreTheme.Pale),
                    ("Stations", "Air Filtration, Hydroponics, Infirmary, Radio", CoreTheme.Pale),
                    ("Active Roster", rosterStatus, CoreTheme.Warm)
                },
                "OPEN DUTY ROSTER",
                () => OnOpenExpansionRequested?.Invoke("duty_roster")
            );

            // ── 3. Standing Record (Exp 03) ──
            int roomCount = _expansions?.Layouts?.Layouts?.Count ?? 14;
            AddModuleCard(
                "EXP 03 — THE STANDING RECORD",
                "Deep ground layouts, 14 hierarchical room structures, search strata, memory scars, and structural exploration.",
                "STRATA READY",
                CoreTheme.Hot,
                new (string, string, (float r, float g, float b, float a))[]
                {
                    ("Ground Layouts", $"{roomCount} Locations Inscribed", CoreTheme.Warm),
                    ("Memory Strata", "38 Environmental Memory Scars", CoreTheme.Pale),
                    ("Search Depth", "Multi-tiered room scavenge tree", CoreTheme.Pale)
                },
                "EXPLORE GROUND LAYOUTS & STRATA",
                () => OnOpenExpansionRequested?.Invoke("standing_record")
            );

            // ── 4. Nobody's Charter / The Crossing (Exp 04) ──
            bool hasVouch = _expansions?.Vouch?.HasAccess ?? false;
            string vouchStr = hasVouch ? "VOUCH ACCESS GRANTED" : "GATE RESTRICTED";
            AddModuleCard(
                "EXP 04 — NOBODY'S CHARTER / THE CROSSING",
                "Quarantine gate passage, backer vouch arbitration, contraband inspection, and 7 branching multi-stage quests.",
                vouchStr,
                hasVouch ? CoreTheme.Warm : CoreTheme.Critical,
                new (string, string, (float r, float g, float b, float a))[]
                {
                    ("Gate Clearance", vouchStr, hasVouch ? CoreTheme.Warm : CoreTheme.Critical),
                    ("Arbitration Rulings", "5 Backer Figures Available", CoreTheme.Pale),
                    ("Crossing Quests", "7 Multi-stage charter narratives", CoreTheme.Pale)
                },
                "OPEN CROSSING QUEST LOG",
                () => OnOpenExpansionRequested?.Invoke("crossing_quests")
            );

            // ── 5. The Glass Orchard / Greenhouse (Exp 05) ──
            string ghStatus = _greenhouse != null ? $"{_greenhouse.System.PlotCount} Plots Cultivated" : "Standby";
            AddModuleCard(
                "EXP 05 — THE GLASS ORCHARD",
                "Hydroponic crop cultivation, mutation drift, nutrient management, lighting cycles, and clean food yield.",
                "HYDROPONICS ONLINE",
                CoreTheme.Warm,
                new (string, string, (float r, float g, float b, float a))[]
                {
                    ("Hydroponic Array", ghStatus, CoreTheme.Warm),
                    ("Flora Varieties", "Ash-Wheat, Rad-Kale, Gloom-Bean, Rust-Beet", CoreTheme.Pale),
                    ("Harvest Security", "Self-sustaining organic food production", CoreTheme.Pale)
                },
                "OPEN GREENHOUSE CONSOLE",
                () => OnOpenExpansionRequested?.Invoke("greenhouse")
            );

            // ── 6. The Silent Foundry (Exp 10) ──
            string foundryStatus = _expansions?.SilentFoundry != null
                ? (_expansions.SilentFoundry.IsUnlocked ? "FURNACE OPEN" : "SEALED — BLUEPRINT CATALOGUED")
                : "Standby";
            AddModuleCard(
                "EXP 10 — THE SILENT FOUNDRY",
                "Smelter-bay production and repair: green-sand casting, heavy-alloy fabrication, the 4-day maintenance cycle, treaty quotas, and the labor accord.",
                foundryStatus,
                CoreTheme.Entropy,
                new (string, string, (float r, float g, float b, float a))[]
                {
                    ("Heat Lifecycle", "charge → preheat → tap → cast → cool", CoreTheme.Pale),
                    ("Maintenance", "4-day service cycle; neglect risks breakout incidents", CoreTheme.Warm),
                    ("Treaties", "sulfur, labor & milk, railway charter, constitution", CoreTheme.Warm)
                },
                "OPEN THE FOUNDRY FLOOR",
                () => OnOpenExpansionRequested?.Invoke("silent_foundry")
            );

            // ── 7. The Muster (Exp 06) ──
            int currentsCount = _muster?.Roster?.Count ?? 15;
            AddModuleCard(
                "EXP 06 — THE MUSTER",
                "15 Sector currents, Deserter Coalition Camp, Unsigned Order witness dossiers, and 6-faction geopolitical crisis.",
                "MOBILIZING",
                CoreTheme.Hot,
                new (string, string, (float r, float g, float b, float a))[]
                {
                    ("Sector Currents", $"{currentsCount} Active Sector Flashpoints", CoreTheme.Warm),
                    ("Coalition Camp", "Voluntary refugee assembly at Weighbridge", CoreTheme.Pale),
                    ("Witness Dossiers", "11 Unsigned Order sworn statements", CoreTheme.Pale)
                },
                "OPEN THE MUSTER INTELLIGENCE",
                () => OnOpenExpansionRequested?.Invoke("muster")
            );

            // ── 7. The Dose / The Vigil (Exp 07) ──
            string medStatus = _medical != null ? $"{_medical.Engine.CaptureState().survivors.Count} Patients Tracked" : "Infirmary Active";
            AddModuleCard(
                "EXP 07 — THE DOSE & THE VIGIL",
                "Clinical dose ledger, triage queue, somatic flashbacks, acute rad sickness, combat trauma, and chelation therapy.",
                "INFIRMARY ACTIVE",
                CoreTheme.Warm,
                new (string, string, (float r, float g, float b, float a))[]
                {
                    ("Dose Ledger", "Real-time whole-body mSv dosimeter tracking", CoreTheme.Pale),
                    ("Triage Protocol", medStatus, CoreTheme.Warm),
                    ("Therapy Regimen", "Potassium Iodide, Chelation, Sedatives", CoreTheme.Pale)
                },
                "OPEN MEDICAL & DOSE LEDGER",
                () => OnOpenExpansionRequested?.Invoke("medical")
            );

            // ── 8. The Verdict (Exp 08) ──
            string verdictPhase = _verdict != null ? _verdict.Reckoning.State.phase.ToString() : "Standby";
            AddModuleCard(
                "EXP 08 — THE VERDICT",
                "Shelter reckoning machine, broadcast intercepts, moral culpability trials, and irreversible saga choices.",
                verdictPhase.ToUpperInvariant(),
                CoreTheme.Hot,
                new (string, string, (float r, float g, float b, float a))[]
                {
                    ("Machine Register", $"Phase: {verdictPhase}", CoreTheme.Warm),
                    ("Signal Intercepts", "High-frequency military radio wiretaps", CoreTheme.Pale),
                    ("Evidence Ledger", "Historical artifact forensics", CoreTheme.Pale)
                },
                "OPEN THE VERDICT CONSOLE",
                () => OnOpenExpansionRequested?.Invoke("verdict")
            );

            // ── 9. The Black Flotilla / Maritime (Exp 09) ──
            string diveStatus = _maritime != null && _maritime.Dive.IsActive ? "DIVE IN PROGRESS" : "DRYDOCK STANDBY";
            AddModuleCard(
                "EXP 09 — THE BLACK FLOTILLA",
                "Submerged stealth salvage, dive compressor operations, acoustic noise management, and progressive chamber breach.",
                diveStatus,
                CoreTheme.Hot,
                new (string, string, (float r, float g, float b, float a))[]
                {
                    ("Submerged Ops", diveStatus, CoreTheme.Warm),
                    ("Hull Integrity", "4 Sealed Chambers (Airlock, Reactor, Armory, Hold)", CoreTheme.Pale),
                    ("Hazard Mitigation", "Acoustic noise dampening & dive air gauge", CoreTheme.Pale)
                },
                "OPEN MARITIME SALVAGE OPERATIONS",
                () => OnOpenExpansionRequested?.Invoke("maritime")
            );

            // ── 9b. District 8 Deep Coast (Exp 01 sibling layer) ──
            string coastStage = _deepCoast != null ? _deepCoast.DeepCoast.Stage.ToString() : "Sealed";
            string coastAccess = _deepCoast != null
                ? (_deepCoast.IsFleetActive ? "FLEET STOOD UP" : "municipal / office")
                : "sealed";
            AddModuleCard(
                "DISTRICT 8 — THE DEEP COAST",
                "The coastal perimeter, the flooded service channel, and the deep berth at the Northern Sound Icebreaker Dock.",
                "STAGE: " + coastStage,
                CoreTheme.Warm,
                new (string, string, (float r, float g, float b, float a))[]
                {
                    ("Reopening Stage", coastStage, CoreTheme.Warm),
                    ("Access", coastAccess, CoreTheme.Pale),
                    ("Dock Dive", _deepCoast != null && _deepCoast.DeepCoast.CanStartDockOperation ? "AVAILABLE" : "idle", CoreTheme.Hot)
                },
                "OPEN DEEP COAST OPERATIONS",
                () => OnOpenExpansionRequested?.Invoke("deep_coast")
            );

            // ── 10. The Year of Ash (Exp 06/07 Timeline) ──
            AddModuleCard(
                "EXP 10 — THE YEAR OF ASH",
                "365-day seasonal fallout timeline, nuclear winter blizzard cycles, black rain storms, and survival pacing.",
                $"DAY {_currentDay}",
                CoreTheme.Warm,
                new (string, string, (float r, float g, float b, float a))[]
                {
                    ("Current Timeline", $"Day {_currentDay} of Nuclear Winter", CoreTheme.Warm),
                    ("Atmospheric State", "Fallout particulate suspension active", CoreTheme.Pale),
                    ("Seasonal Cycles", "Blizzard, Ashfall, Thaw, Black Rain", CoreTheme.Pale)
                },
                "OPEN WEATHER & METEOROLOGY",
                () => OnOpenExpansionRequested?.Invoke("weather")
            );

            // ── 11. The Orbital Harrow / Sky-Layer Armor (Exp 11) ──
            string armorStatus = _world?.SkyArmorStatusLine() ?? "Operational";
            AddModuleCard(
                "EXP 11 — THE ORBITAL HARROW",
                "Orbital kinetic fallout threat, ablative sky-layer armor plating, impact deflection, and blast door reinforcement.",
                "FORTIFIED",
                CoreTheme.Warm,
                new (string, string, (float r, float g, float b, float a))[]
                {
                    ("Armor Integrity", armorStatus, CoreTheme.Warm),
                    ("Debris Threat", "4 Kinetic Threat Tiers Tracked", CoreTheme.Pale),
                    ("Deflection Systems", "Bunker roof ablative shielding", CoreTheme.Pale)
                },
                "OPEN SKY ARMOR / SHELTER",
                () => OnOpenExpansionRequested?.Invoke("shelter")
            );

            // ── 12. The Century Seed (Exp 12) ──
            var gen = _expansions?.Generational;
            string genStatus = gen != null ? $"Chapter {gen.CurrentChapterIndex} · Year {gen.TotalYearsElapsed}" : "Active";
            AddModuleCard(
                "EXP 12 — THE CENTURY SEED",
                "Generational succession engine, survivor aging, elder retirement, mentoring relations, inherited traits, and death wills.",
                "LINEAGE ACTIVE",
                CoreTheme.Warm,
                new (string, string, (float r, float g, float b, float a))[]
                {
                    ("Succession Chapter", genStatus, CoreTheme.Warm),
                    ("Mentoring Pairs", "Survivor trait inheritance active", CoreTheme.Pale),
                    ("Legacy Score", "Multi-generational scorecards tracked", CoreTheme.Pale)
                },
                "OPEN SUCCESSION CONSOLE",
                () => OnOpenExpansionRequested?.Invoke("century_seed")
            );

            // ── XX. Endgame / Epilogue Matrix ──
            AddModuleCard(
                "ENDGAME — EPILOGUE MATRIX",
                "Comprehensive chronicle evaluation across 12 regional fates, survivor demographic balance, moral standing, and unlocked historical flags.",
                "EVALUATING",
                CoreTheme.Hot,
                new (string, string, (float r, float g, float b, float a))[]
                {
                    ("Regional Fate", "Wasteland demographic projections", CoreTheme.Warm),
                    ("Moral Standing", "Principled vs utilitarian decisions", CoreTheme.Pale),
                    ("Historical Chronicle", "Permanent record of the shelter", CoreTheme.Pale)
                },
                "OPEN EPILOGUE MATRIX",
                () => OnOpenExpansionRequested?.Invoke("epilogue")
            );
        }

        private void AddModuleCard(
            string title,
            string description,
            string badgeText,
            (float r, float g, float b, float a) badgeColor,
            (string Label, string Value, (float r, float g, float b, float a) Color)[] dataRows,
            string buttonLabel,
            Action onButtonClicked)
        {
            var card = AshfallUiHelpers.MakePanel();
            var margin = AshfallUiHelpers.MakeMargins((int)CoreTheme.SpacingSm);
            card.AddChild(margin);

            var vbox = AshfallUiHelpers.MakeVBox((int)CoreTheme.SpacingSm);
            margin.AddChild(vbox);

            var topRow = AshfallUiHelpers.MakeHBox((int)CoreTheme.SpacingSm);
            vbox.AddChild(topRow);

            var lblTitle = AshfallUiHelpers.MakeSectionHeader(title);
            lblTitle.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            lblTitle.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Hot));
            topRow.AddChild(lblTitle);

            var badge = AshfallUiHelpers.MakeSmall($"[{badgeText}]");
            badge.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(badgeColor));
            topRow.AddChild(badge);

            var lblDesc = AshfallUiHelpers.MakeBody(description);
            lblDesc.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(CoreTheme.Muted));
            vbox.AddChild(lblDesc);

            for (int i = 0; i < dataRows.Length; i++)
            {
                var row = dataRows[i];
                vbox.AddChild(AshfallUiHelpers.MakeDataRow(row.Label, row.Value, AshfallUiHelpers.ToColor(row.Color)));
            }

            var btn = AshfallUiHelpers.MakeButton(buttonLabel, onButtonClicked);
            btn.CustomMinimumSize = new Vector2(0, 36);
            vbox.AddChild(btn);

            _modulesContainer.AddChild(card);
        }

        private static void ClearContainer(VBoxContainer container)
        {
            if (container == null) return;
            while (container.GetChildCount() > 0)
            {
                var child = container.GetChild(0);
                container.RemoveChild(child);
                child.QueueFree();
            }
        }
    }
}
